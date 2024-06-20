using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.Api.Services.Interfaces;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class RevenueService : IRevenueService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IContracrRepository _contractRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;

    public RevenueService(IExchangeRateService exchangeRateService, IContracrRepository contractRepository, ISubscriptionRepository subscriptionRepository)
    {
        _exchangeRateService = exchangeRateService;
        _contractRepository = contractRepository;
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<decimal> CalculateCurrentRevenueAsync(RevenueCalculationRequestDTO requestDto)
    {
        decimal totalRevenuePLN = 0M;
    
        if (requestDto.For == "company")
        {
            var contracts = await _contractRepository.GetListOfSignedContractsAsync();
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
        else if (requestDto.For == "product" )
        {
            var contracts = await _contractRepository.GetListOfSignedContractsForProductAsync(requestDto.ProductId);
            foreach (var contract in contracts)
            {
                totalRevenuePLN  += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId);
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
            throw new BadRequestExeption("Invalid requestDto. for: [company,product]");
        }
    
        if (requestDto.CurrencyCode == "PLN")
        {
            return totalRevenuePLN;
        }
    
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(requestDto.CurrencyCode);
        return totalRevenuePLN * exchangeRate;
    }
    
    public async Task<decimal> CalculatePredictedRevenueAsync(RevenueCalculationRequestDTO requestDto)
    {
        decimal totalRevenuePLN = 0M;
    
        if (requestDto.For == "company")
        {
            var contracts = await _contractRepository.GetListOfAllContractsNotPastDatesAsync();
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
        else if (requestDto.For == "product")
        {
            var contracts = await _contractRepository.GetListOfAllContractsNotPastDatesForProductAsync(requestDto.ProductId);
            foreach (var contract in contracts)
            {
                totalRevenuePLN += contract.Price;
            }
            
            var subscriptions = await _subscriptionRepository.GetListOfSubscriptionsWithPayementsForProductAsync(requestDto.ProductId);
            foreach (var subscription in subscriptions)
            {
                foreach (var payment in subscription.Payments)
                {
                    totalRevenuePLN += payment.Amount;
                }
            }

            var notCanceledSubscription = await _subscriptionRepository.GetListOfSubscriptionsForProductNotCanceledAsync(requestDto.ProductId);

            foreach (var subscription in notCanceledSubscription)
            {
                totalRevenuePLN += subscription.Price;
            }
        }
        else
        {
            throw new BadRequestExeption("Invalid requestDto. for: [company,product]");
        }
    
        if (requestDto.CurrencyCode == "PLN")
        {
            return totalRevenuePLN;
        }
    
        var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(requestDto.CurrencyCode);
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