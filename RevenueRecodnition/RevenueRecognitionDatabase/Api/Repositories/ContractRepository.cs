using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;
using RevenueRecodnition.Api.Repositories.Interfaces;

namespace RevenueRecodnition.Api.Repositories;

public class ContractRepository : IContracrRepository
{
    private RRConext _context;

    public ContractRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Contract?> GetActiveContractForProductAsync(int productId,int clientId)
    {
        return await _context.Contracts
            .Where(x => x.IdClient == clientId)
            .Where(x => x.IdProduct == productId)
            .Where(x => x.IsSigned || (x.StartDatePayement <= DateTime.Now && x.EndDatePayement >= DateTime.Now))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Contract>> GetListOfSignedContractsAsync()
    {
        var test = await _context.Contracts.FindAsync(1);
        
        var list =  await _context.Contracts.
            Where(x => x.IsSigned)
            .ToListAsync();

        return list;
    }

    public async Task<List<Contract>> GetListOfSignedContractsForProductAsync(int productId)
    {
        return await _context.Contracts
            .Where(x => x.IsSigned && x.IdProduct == productId)
            .ToListAsync();
    }

    public async Task<List<Contract>> GetListOfAllContractsNotPastDatesAsync()
    {
        return await _context.Contracts
            .Where(x => !(x.EndDatePayement < DateTime.Now) || x.IsSigned).ToListAsync();
    }

    public async Task<List<Contract>> GetListOfAllContractsNotPastDatesForProductAsync(int productId)
    {
        return await _context.Contracts
            .Where(x=>x.IdProduct == productId)
            .Where(x => !(x.EndDatePayement < DateTime.Now) || x.IsSigned).ToListAsync();
    }

    public async Task<int> AddContractAsync(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        return contract.IdContract;
    }

    public async Task<Contract?> GetContractAsync(int contractId)
    {
        return await _context.Contracts.FindAsync(contractId);
    }

    public async Task SignContractAsync(int contractId)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        contract.IsSigned = true;
        await _context.SaveChangesAsync();
    }
}