using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class ContractRepository : IContracrRepository
{
    private RRConext _context;

    public ContractRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Contract?> GetActiveContractForProduct(int productId,int clientId)
    {
        return await _context.Contracts
            .Where(x => x.IdClient == clientId)
            .Where(x => x.IdProduct == productId)
            .Where(x => x.IsSigned || (x.StartDatePayement <= DateTime.Now && x.EndDatePayement >= DateTime.Now))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Contract>> GetListOfSignedContracts()
    {
        var test = await _context.Contracts.FindAsync(1);
        
        var list =  await _context.Contracts.
            Where(x => x.IsSigned)
            .ToListAsync();

        return list;
    }

    public async Task<List<Contract>> GetListOfSignedContractsForProduct(int productId)
    {
        return await _context.Contracts
            .Where(x => x.IsSigned && x.IdProduct == productId)
            .ToListAsync();
    }

    public async Task<List<Contract>> GetListOfAllContractsNotPastDates()
    {
        return await _context.Contracts
            .Where(x => !(x.EndDatePayement < DateTime.Now) || x.IsSigned).ToListAsync();
    }

    public async Task<List<Contract>> GetListOfAllContractsNotPastDatesForProduct(int productId)
    {
        return await _context.Contracts
            .Where(x=>x.IdProduct == productId)
            .Where(x => !(x.EndDatePayement < DateTime.Now) || x.IsSigned).ToListAsync();
    }

    public async Task<int> AddContract(Contract contract)
    {
        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        return contract.IdContract;
    }

    public async Task<Contract?> GetContract(int contractId)
    {
        return await _context.Contracts.FindAsync(contractId);
    }

    public async Task SignContract(int contractId)
    {
        var contract = await _context.Contracts.FindAsync(contractId);
        contract.IsSigned = true;
        await _context.SaveChangesAsync();
    }
}