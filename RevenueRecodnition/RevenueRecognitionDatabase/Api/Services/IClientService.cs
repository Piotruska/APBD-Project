using Microsoft.AspNetCore.Mvc;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;

namespace RevenueRecodnition.Api.Services;

public interface IClientService
{
    public Task AddIndividualClientAsync(AddIndividualClientDTO dto);
    public Task AddCompanyClientAsync(AddCompanyClientDTO dto);
    public Task UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto,int clientId);
    public Task UpdateCompanyClientAsync(UpdateCompanyClientDto dto,int clientId);
    public Task SoftDeleteIndividualCLientAsync(int clientId);

}