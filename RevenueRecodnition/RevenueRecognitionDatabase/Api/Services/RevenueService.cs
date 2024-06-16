using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class RevenueService : IRevenueService
{
    private readonly IExchangeRateService _exchangeRateService;
    private IClientRepository _clientRepository;
    private IContracrRepository _contractRepository;
    private IProductRepository _productRepository;
    private IDicountRepository _dicountRepository;
    private ISubscriptionRepository _subscriptionRepository;
    private IPayementRepository _payementRepository;

    public RevenueService(IExchangeRateService exchangeRateService,IClientRepository clientRepository, IContracrRepository contractRepository, IProductRepository productRepository, IDicountRepository dicountRepository, ISubscriptionRepository subscriptionRepository, IPayementRepository payementRepository)
    {
        _exchangeRateService = exchangeRateService;
        _clientRepository = clientRepository;
        _contractRepository = contractRepository;
        _productRepository = productRepository;
        _dicountRepository = dicountRepository;
        _subscriptionRepository = subscriptionRepository;
        _payementRepository = payementRepository;
    }

    public async Task<decimal> CalculateCurrentRevenueAsync(RevenueCalculationRequest request)
    {
        decimal totalRevenuePLN = 0M;
    
        if (request.For == "company")
        {
            var contracts = await _contractRepository.GetListOfSignedContracts();
            foreach (var contract in contracts)
            {
                totalRevenuePLN += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsAsync();
            foreach (var subscription in subscriptions)
            {
                foreach (var payment in subscription.Payments)
                {
                    totalRevenuePLN += payment.Amount;
                }
            }
    
        }
        else if (request.For == "product" )
        {
            var contracts = await _contractRepository.GetListOfSignedContractsForProduct(request.ProductId);
            foreach (var contract in contracts)
            {
                totalRevenuePLN  += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsForProductAsync(request.ProductId);
            foreach (var subscription in subscriptions)
            {
                foreach (var payment in subscription.Payments)
                {
                    totalRevenuePLN  += payment.Amount;
                }
            }
        }
        else
        {
            throw new BadRequestExeption("Invalid request. for: [company,product]");
        }
    
        if (request.CurrencyCode == "PLN")
        {
            return totalRevenuePLN;
        }
    
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.CurrencyCode);
        return totalRevenuePLN * exchangeRate;
    }
    
    public async Task<decimal> CalculatePredictedRevenueAsync(RevenueCalculationRequest request)
    {
        decimal totalRevenuePLN = 0M;
    
        if (request.For == "company")
        {
            var contracts = await _contractRepository.GetListOfAllContractsNotPastDates();
            foreach (var contract in contracts)
            {
                totalRevenuePLN += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsAsync();
            foreach (var subscription in subscriptions)
            {
                foreach (var payment in subscription.Payments)
                {
                    totalRevenuePLN += payment.Amount;
                }
            }

            var notCanceledSubscription = await _subscriptionRepository.GetListOfSubscriptionsNotCanceledAsync();

            foreach (var subscription in notCanceledSubscription)
            {
                totalRevenuePLN += subscription.Price;
            }
        }
        else if (request.For == "product")
        {
            var contracts = await _contractRepository.GetListOfAllContractsNotPastDatesForProduct(request.ProductId);
            foreach (var contract in contracts)
            {
                totalRevenuePLN += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsForProductAsync(request.ProductId);
            foreach (var subscription in subscriptions)
            {
                foreach (var payment in subscription.Payments)
                {
                    totalRevenuePLN += payment.Amount;
                }
            }

            var notCanceledSubscription = await _subscriptionRepository.GetListOfSubscriptionsForProductNotCanceledAsync(request.ProductId);

            foreach (var subscription in notCanceledSubscription)
            {
                totalRevenuePLN += subscription.Price;
            }
        }
        else
        {
            throw new BadRequestExeption("Invalid request. for: [company,product]");
        }
    
        if (request.CurrencyCode == "PLN")
        {
            return totalRevenuePLN;
        }
    
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.CurrencyCode);
        return totalRevenuePLN * exchangeRate;
    }
    
    private void EnsureProductExists(Product product)
    {
        if (product == null)
        {
            throw new NotFoundExeption("Product not found.");
        }
    }
}