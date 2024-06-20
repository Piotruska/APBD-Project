using Microsoft.EntityFrameworkCore;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Repositories.Interfaces;
using RevenueRecodnition.DataBase.Context;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public class ClientRepository : IClientRepository
{
    private RRConext _context;

    public ClientRepository(RRConext context)
    {
        _context = context;
    }

    public async Task<Client?> GetClientWithSoftDeletedAsync(int clientId)
    {
        return await _context.Clients.FindAsync(clientId);
    }
    
    public async Task<Client?> GetClientWithoutSoftDeletedAsync(int clientId)
    {
        return await _context.Clients
            .Where(x=>x.IdClient == clientId)
            .Where(x=>!x.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientWithSoftDeletedAllInfoAsync(int clientId)
    {
        return await _context.Clients
            .Include(x => x.IndividualClient)
            .Include(x => x.CompanyClient)
            .Include(x => x.Contracts)
            .Include(x => x.Subscriptions)
            .FirstAsync(x=>x.IdClient == clientId);
    }

    public async Task<Client?> GetClientWithoutSoftDeletedAllInfoAsync(int clientId)
    {
        return await _context.Clients
            .Include(x => x.IndividualClient)
            .Include(x => x.CompanyClient)
            .Include(x => x.Contracts)
            .Include(x => x.Subscriptions)
            .Where(x=>x.IdClient == clientId)
            .Where(x=>!x.IsDeleted)
            .FirstOrDefaultAsync();
        
    }

    public async Task AddClientAsync(Client Client)
    {
        await _context.Clients.AddAsync(Client);
        await _context.SaveChangesAsync();
    }

    public async Task AddIndividualClientAsync(IndividualClient individualClient)
    {
        await _context.IndividualClients.AddAsync(individualClient);
        await _context.SaveChangesAsync();
    }

    public async Task AddCompanyClientAsync(CompanyClient companyClient)
    {
        await _context.CompanyClients.AddAsync(companyClient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto, int clientId)
    {
        var clientToUpdate = await _context.Clients
            .Include(x => x.IndividualClient)
            .Include(x => x.CompanyClient)
            .FirstAsync(x =>x.IdClient == clientId);

        clientToUpdate.Address = dto.Address;
        clientToUpdate.Email = dto.Email;
        clientToUpdate.PhoneNumber = dto.PhoneNumber;
        clientToUpdate.IndividualClient.FirstName = dto.FirstName;
        clientToUpdate.IndividualClient.LastName = dto.LastName;
        
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCompanyClientAsync(UpdateCompanyClientDto dto, int clientId)
    {
        var clientToUpdate = await _context.Clients.FindAsync(clientId);

        clientToUpdate.Address = dto.Address;
        clientToUpdate.Email = dto.Email;
        clientToUpdate.PhoneNumber = dto.PhoneNumber;
        clientToUpdate.CompanyClient.ComapnyName = dto.CompanyName;
        
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteIndividualCLientAsync(int clientId)
    {
        var clientToSoftDelete = await _context.Clients.FindAsync(clientId);
        clientToSoftDelete.IsDeleted = true;
        await _context.SaveChangesAsync();
    }
}