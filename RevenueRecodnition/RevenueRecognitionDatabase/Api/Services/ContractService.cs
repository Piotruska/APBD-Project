using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class ContractService : IContractService
{
    private readonly IExchangeRateService _exchangeRateService;
    private IClientRepository _clientRepository;
    private IContracrRepository _contractRepository;
    private IProductRepository _productRepository;
    private IDicountRepository _dicountRepository;
    private ISubscriptionRepository _subscriptionRepository;
    private IPayementRepository _payementRepository;

    public ContractService(IExchangeRateService exchangeRateService,IClientRepository clientRepository, IContracrRepository contractRepository, IProductRepository productRepository, IDicountRepository dicountRepository, ISubscriptionRepository subscriptionRepository, IPayementRepository payementRepository)
    {
        _exchangeRateService = exchangeRateService;
        _clientRepository = clientRepository;
        _contractRepository = contractRepository;
        _productRepository = productRepository;
        _dicountRepository = dicountRepository;
        _subscriptionRepository = subscriptionRepository;
        _payementRepository = payementRepository;
    }


    public async Task<int> CreateContractAsync(CreateContractDTO dto)
    {
        //Data Prep and Retreval
        var client = await _clientRepository.GetClientWithoutSoftDeletedAsync(dto.ClientId);
        EnsureClientExists(client);
        client = await _clientRepository.GetClientWithoutSoftDeletedAllInfoAsync(dto.ClientId);
        var product = await _productRepository.GetProductAsync(dto.ProductId);
        EnsureProductExists(product);
        var discount = GetDiscountCalue(await _dicountRepository.GetCurrentHighestDiscountAsync());
        

        var subscirption = await _subscriptionRepository.GetActiveSubscriptionsForProductAsync(dto.ProductId,dto.ClientId);
        var contract = await _contractRepository.GetActiveContractForProductAsync(dto.ProductId,dto.ClientId);
        EnsureClientDoseNotHaveASubscriptionOrContractForThisProduct(subscirption,contract);
        
        // EnsureTimeRangeForPayementIsInBounderies(dto); //Commented out because in Model (Domain model)
        // EnsureTimeRangeForSupportIsInBounderies(dto); //Commented out because in Model (Domain model)
        DateTime StartDate = DateTime.Now;
        DateTime EndDatePayement = StartDate.AddDays(dto.TimePeriodForPayement);
        DateTime EndDateContract = StartDate.AddYears(dto.ContractLengthInYears);
        DateTime EndDateSupport = StartDate.AddYears(1+dto.AdditionalSupportTimeInYears);
        
        
        //Price and dicount Calculations
        var totalDiscount = discount + IsClientAPreviousClient(client);
        var BasePrice = 1000 * dto.AdditionalSupportTimeInYears + product.BasePrice * dto.ContractLengthInYears;
        var totalPrice =(decimal) (BasePrice - (BasePrice * totalDiscount)*0.01M);

        var contractToAdd = new Contract()
        {
            StartDatePayement = StartDate,
            EndDatePayement = EndDatePayement,
            StartDateContract = StartDate,
            EndDateContract = EndDateContract,
            StartDateSupport = StartDate,
            EndDateSupport = EndDateSupport,
            Price = totalPrice,
            IsSigned = false,
            IdClient = client.IdClient,
            IdProduct = product.IdProduct
        };

        return await _contractRepository.AddContractAsync(contractToAdd);
    }

    public async Task IssuePayementForContractAsync(PaymentForContractDTO dto)
    {
        var contract = await _contractRepository.GetContractAsync(dto.contractID);
        EnsureContractExists(contract);
        EnsureContractIsNotSigned(contract);
        EnsurePaymentDateHasNotPassed(contract);

        var payaments = await _payementRepository.GetPayementsForContractAsync(dto.contractID);
        decimal AmoutPayed = 0;
        decimal ContractCost = contract.Price;
        foreach (var payament in payaments)
        {
            AmoutPayed = +payament.Amount;
        }
        decimal AmoutDue = ContractCost - AmoutPayed;
        EnsurePaymentIsNotLargerThenAMountDue(AmoutDue, dto.Amount);

        var payement = new Payment()
        {
            IdContract = dto.contractID,
            Amount = dto.Amount,
            DatePayed = DateTime.Now
        };

        await _payementRepository.AddPaymentAsync(payement);

        var currentAmount = AmoutPayed + dto.Amount;
        if (currentAmount == ContractCost)
        {
            await _contractRepository.SignContractAsync(dto.contractID);
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

    private void EnsureContractExists(Contract contract)
    {
        if (contract == null)
        {
            throw new NotFoundExeption("Contract not found.");
        }
    }
    
    private void EnsureContractIsNotSigned(Contract contract)
    {
        if (contract.IsSigned)
        {
            throw new BadRequestExeption("Contract is signed.");
        }
    }
    
    private void EnsurePaymentDateHasNotPassed(Contract contract)
    {
        if (contract.EndDatePayement < DateTime.Now)
        {
            throw new BadRequestExeption("Time has expired.");
        }
    } 
    
    private void EnsurePaymentIsNotLargerThenAMountDue(decimal AmoutDue,decimal PaymentAmount)
    {
        if (AmoutDue < PaymentAmount)
        {
            throw new BadRequestExeption($"Payement is to large. Amount due : {AmoutDue}");
        }
    }
    
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
    
    private void EnsureClientDoseNotHaveASubscriptionOrContractForThisProduct(Subscription subscription,Contract contract)
    {
        if (subscription != null )
        {
            throw new BadRequestExeption($"Client has an active subscirption for this program ");
        }

        if (contract != null)
        {
            throw new BadRequestExeption($"Client has a active Contract or has an issues unpayed contract for this program");
        }
    }
    
    // private void EnsureTimeRangeForPayementIsInBounderies(CreateContractDTO dto) //Commented out because in Model (Domain model)
    // {
    //     if (dto.TimePeriodForPayement < 3 || dto.TimePeriodForPayement > 30)
    //     {
    //         throw new BadRequestExeption("TimePeriod cannot be less then 3 or larger then 30");
    //     }
    // }
    
    // private void EnsureTimeRangeForSupportIsInBounderies(CreateContractDTO dto) //Commented out because in Model (Domain model)
    // {
    //     if (dto.AdditionalSupportTimeInYears > 3 || dto.AdditionalSupportTimeInYears < 0)
    //     {
    //         throw new BadRequestExeption("Support can only be [0,1,2,3] years");
    //     }
    // }
    
    private decimal IsClientAPreviousClient(Client client)
    {
        if (client.Contracts.Any(x=>x.IsSigned) || client.Subscriptions.Any())
        {
            return 5;
        }
        return 0;
    }
}