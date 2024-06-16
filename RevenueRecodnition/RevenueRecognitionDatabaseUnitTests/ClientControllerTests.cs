using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RevenueRecodnition.Api.Controllers;
using RevenueRecodnition.Api.Models;
using RevenueRecodnition.Api.Modls;
using RevenueRecodnition.Api.Services;

namespace RevenueRecognitionDatabaseUnitTests;

[TestFixture]
public class ClientControllerTests
{
    private Mock<IClientService> _mockService;
    private ClientController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IClientService>();
        _controller = new ClientController(_mockService.Object);
    }

    [Test]
    public async Task AddIndividualClientAsync_ReturnsNoContent()
    {
        // Arrange
        var dto = new AddIndividualClientDTO();
        _mockService.Setup(s => s.AddIndividualClientAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddIndividualClientAsync(dto);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        _mockService.Verify(s => s.AddIndividualClientAsync(dto), Times.Once);
    }

    [Test]
    public async Task AddCompanyClientAsync_ReturnsNoContent()
    {
        // Arrange
        var dto = new AddCompanyClientDTO();
        _mockService.Setup(s => s.AddCompanyClientAsync(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddCompanyClientAsync(dto);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        _mockService.Verify(s => s.AddCompanyClientAsync(dto), Times.Once);
    }

    [Test]
    public async Task UpdateCompanyClientAsync_ReturnsNoContent()
    {
        // Arrange
        var dto = new UpdateCompanyClientDto();
        int companyId = 1;
        _mockService.Setup(s => s.UpdateCompanyClientAsync(dto, companyId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateCompanyClientAsync(dto, companyId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        _mockService.Verify(s => s.UpdateCompanyClientAsync(dto, companyId), Times.Once);
    }

    [Test]
    public async Task UpdateIndividualClientAsync_ReturnsNoContent()
    {
        // Arrange
        var dto = new UpdateIndividualClientDTO();
        int individualId = 1;
        _mockService.Setup(s => s.UpdateIndividualCLientAsync(dto, individualId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateIndividualCLientAsync(dto, individualId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        _mockService.Verify(s => s.UpdateIndividualCLientAsync(dto, individualId), Times.Once);
    }

    [Test]
    public async Task SoftDeleteIndividualClientAsync_ReturnsNoContent()
    {
        // Arrange
        int individualId = 1;
        _mockService.Setup(s => s.SoftDeleteIndividualCLientAsync(individualId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SoftDeleteIndividualCLientAsync(individualId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        _mockService.Verify(s => s.SoftDeleteIndividualCLientAsync(individualId), Times.Once);
    }
}
