using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Repositories;

public interface IClientRepository
{
    public Task<Client?> GetAllClientWithSoftDeletedAsync(int clientId);
    public Task<Client?> GetAllClientWithoutSoftDeletedAsync(int clientId);
    public Task AddClientAsync(Client Client);
    public Task AddIndividualClientAsync(IndividualClient individualClient);
    public Task AddCompanyClientAsync(CompanyClient companyClient);
    public Task UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto, int clientId);
    public Task UpdateCompanyClientAsync(UpdateCompanyClientDto dto, int clientId);
    public Task SoftDeleteIndividualCLientAsync(int clientId);
}