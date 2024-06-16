using RevenueRecodnition.Api.Exeptions;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Repositories;
using RevenueRecodnition.DataBase.Entities;

namespace RevenueRecodnition.Api.Services;

public class ClientService : IClientService
{
    private IClientRepository _clientRepository;
    private IUnitOfWork _unitOfWork;

    public ClientService(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddIndividualClientAsync(AddIndividualClientDTO dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var ClientBaseToAdd = new Client()
            {
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            await _clientRepository.AddClientAsync(ClientBaseToAdd);

            var individualClientToAdd = new IndividualClient()
            {
                IdClient = ClientBaseToAdd.IdClient,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PESEL = dto.PESEL
            };
            await _clientRepository.AddIndividualClientAsync(individualClientToAdd);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw e;
        }

        await _unitOfWork.CommitTransactionAsync();
    }
    public async Task AddCompanyClientAsync(AddCompanyClientDTO dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var ClientBaseToAdd = new Client()
            {
                Address = dto.Address,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            await _clientRepository.AddClientAsync(ClientBaseToAdd);

            var compnayClientToAdd = new CompanyClient()
            {
                IdClient = ClientBaseToAdd.IdClient,
                ComapnyName = dto.CompanyName,
                KRS = dto.KRS
            };
            await _clientRepository.AddCompanyClientAsync(compnayClientToAdd);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw e;
        }

        await _unitOfWork.CommitTransactionAsync();
    }
    public async Task UpdateIndividualCLientAsync(UpdateIndividualClientDTO dto, int clientId)
    {
        Client? clientToUpdate = await _clientRepository.GetClientWithSoftDeletedAsync(clientId);
        EnsureClientExists(clientToUpdate);
        clientToUpdate = await _clientRepository.GetClientWithSoftDeletedAllInfoAsync(clientId);
        EnsureClientIsNotSoftDeleted(clientToUpdate);
        EnsureClientIsIndividual(clientToUpdate);
        await _clientRepository.UpdateIndividualCLientAsync(dto, clientId);
    }

    public async Task UpdateCompanyClientAsync(UpdateCompanyClientDto dto, int clientId)
    {
        Client? clientToUpdate = await _clientRepository.GetClientWithSoftDeletedAsync(clientId);
        EnsureClientExists(clientToUpdate);
        clientToUpdate = await _clientRepository.GetClientWithSoftDeletedAllInfoAsync(clientId);
        EnsureClientIsNotSoftDeleted(clientToUpdate);
        EnsureClientIsCompany(clientToUpdate);
        await _clientRepository.UpdateCompanyClientAsync(dto, clientId);
    }

    public async Task SoftDeleteIndividualCLientAsync(int clientId)
    {
        Client? clientToUpdate = await _clientRepository.GetClientWithoutSoftDeletedAsync(clientId);
        EnsureClientExists(clientToUpdate);
        await _clientRepository.SoftDeleteIndividualCLientAsync(clientId);
    }

    private void EnsureClientExists(Client? client)
    {
        if (client == null)
        {
            throw new NotFoundExeption("Client not found.");
        }
    }
    
    private void EnsureClientIsNotSoftDeleted(Client? client)
    {
        if (client.IsDeleted)
        {
            throw new NotFoundExeption("Client not found. (SoftDeleted)");
        }
    }
    
    private void EnsureClientIsIndividual(Client client)
    {
        if (client.IndividualClient == null)
        {
            throw new BadRequestExeption($"Error : Client {client.IdClient} is an Company Client");
        }
    }
    
    private void EnsureClientIsCompany(Client client)
    {
        if (client.CompanyClient == null)
        {
            throw new BadRequestExeption($"Error : Client {client.IdClient} is an Individual Client");
        }
    }

}