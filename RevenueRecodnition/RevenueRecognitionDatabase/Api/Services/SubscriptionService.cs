using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IClientRepository _clientRepository;
    private readonly IProductRepository _productRepository;
    private readonly IDicountRepository _dicountRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IPayementRepository _payementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionService( IClientRepository clientRepository, IProductRepository productRepository, IDicountRepository dicountRepository, ISubscriptionRepository subscriptionRepository, IPayementRepository payementRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _productRepository = productRepository;
        _dicountRepository = dicountRepository;
        _subscriptionRepository = subscriptionRepository;
        _payementRepository = payementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> AddSubscriptionAsync(AddSubscriptionDTO dto)
    {
        var client = await _clientRepository.GetClientWithoutSoftDeletedAsync(dto.IdClient);
        EnsureClientExists(client);
        var product = await _productRepository.GetProductAsync(dto.IdProduct);
        EnsureProductExists(product);
        //EnsureRenewalPeriodIsInRange(dto.RenewalPeriodInMonths); //Commented out because in Model (Domain model)
        var discount = GetDiscountCalue(await _dicountRepository.GetCurrentHighestDiscountAsync());
        
        var subscriptionToAdd = new Subscription()
        {
            Name = dto.Name,
            RenewalPeriod =dto.RenewalPeriodInMonths,
            StartDateRenewalPayement = DateTime.Now.AddMonths(dto.RenewalPeriodInMonths),
            EndDateRenewalPayement = DateTime.Now.AddMonths(dto.RenewalPeriodInMonths*2),
            IdClient = dto.IdClient,
            IdProduct = dto.IdProduct,
            Price = (product.BasePrice/12)*dto.RenewalPeriodInMonths*0.95M,
        };
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _subscriptionRepository.AddSubscriptionAsync(subscriptionToAdd);
            
            var payement = new Payment()
            {
                IdSubscription = subscriptionToAdd.IdSubscription,
                Amount = (product.BasePrice/12)*dto.RenewalPeriodInMonths*((100-discount)*0.01M),
                DatePayed = DateTime.Now
            };
            await _payementRepository.AddPaymentAsync(payement);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw e;
        }

        await _unitOfWork.CommitTransactionAsync();
        return subscriptionToAdd.IdSubscription;
    }

    public async Task PayForSubscriptionAsync(PayementForSubscription dto)
    {
        var subscription =await  _subscriptionRepository.GetSubscriptionAsync(dto.subscriptionID);
        EnsureSubscriptionExists(subscription);
        EnsureSubscriptionIsNotCancelled(subscription);
        EnsurePayementDates(subscription);
        EnsurePayementAmountIsCorrect(subscription,dto.Amount);
        
        var payement = new Payment()
        {
            IdSubscription = subscription.IdSubscription,
            Amount = subscription.Price,
            DatePayed = DateTime.Now
        };
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _payementRepository.AddPaymentAsync(payement);
            await _subscriptionRepository.UpdateSubscriptionDatesAsync(dto.subscriptionID);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw e;
        }
        await _unitOfWork.CommitTransactionAsync();
    }
    
    
    private void EnsurePayementAmountIsCorrect(Subscription subscription,decimal amount)
    {
        if (subscription.Price != amount)
        {
            throw new BadRequestExeption("Incorrect Payement amount");
        }
    }
    // private void EnsureRenewalPeriodIsInRange(int period) //Commented out because in Model (Domain model)
    // {
    //     if (period<1 || period>24)
    //     {
    //         throw new BadRequestExeption("Renewal period can only be from 1 - 24 months");
    //     }
    // }
    private void EnsureClientExists(Client? client)
    {
        if (client == null)
        {
            throw new NotFoundExeption("Client not found.");
        }
    }
    
    private void EnsureProductExists(Product? product)
    {
        if (product == null)
        {
            throw new NotFoundExeption("Product not found.");
        }
    }
    
    private void EnsureSubscriptionExists(Subscription? subscription)
    {
        if (subscription == null)
        {
            throw new NotFoundExeption("Subscription not found.");
        }
    }
    
    private void EnsurePayementDates(Subscription subscription)
    {
        if (subscription.EndDateRenewalPayement < DateTime.Now)
        {
            throw new BadRequestExeption("Date has passed for payement.");
        }

        if (subscription.StartDateRenewalPayement > DateTime.Now)
        {
            throw new BadRequestExeption("Cannot pay for next period.");
        }
    }
    
    private void EnsureSubscriptionIsNotCancelled(Subscription subscription)
    {
        if (subscription.Canceled)
        {
            throw new BadRequestExeption("Subscription is canceled.");
        }
    }
    
    private decimal GetDiscountCalue(Discount? discount)
    {
        if (discount == null)
        {
            return 0;
        }

        return discount.Percentage;
    }
}