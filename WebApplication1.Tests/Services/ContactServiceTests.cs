using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassLibrary1.DTOs;
using ClassLibrary1.Models;
using ClassLibrary1.Repositories;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using WebApplication1.Service;
using Xunit;

namespace WebApplication1.Tests.Services;

[TestSubject(typeof(ContactService))]
public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<ContactService>> _loggerMock;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _repoMock = new Mock<IContactRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<ContactService>>();
        _service = new ContactService(_repoMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetContactsAsync_ReturnsMappedContactDTOs()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "John", LastName = "Doe", Addresses = new List<Address>() },
            new Contact { Id = 2, FirstName = "Jane", LastName = "Smith", Addresses = new List<Address>() }
        };

        _repoMock.Setup(r => r.GetContactsAsync(1, 10)).ReturnsAsync(contacts);

        var mappedDtos = new List<ContactDTO>
        {
            new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe", Addresses = new List<AddressDTO>() },
            new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Smith", Addresses = new List<AddressDTO>() }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<ContactDTO>>(contacts)).Returns(mappedDtos);

        // Act
        var result = await _service.GetContactsAsync(1, 10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("John", result.First().FirstName);
    }

    [Fact]
    public async Task SearchContactAsync_ReturnsMappedContactDTOs()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { Id = 3, FirstName = "Alice", LastName = "Wonder", Addresses = new List<Address>() }
        };

        _repoMock.Setup(r => r.SearchContactAsync("Alice", 1, 10))
            .ReturnsAsync(contacts);

        var mappedDtos = new List<ContactDTO>
        {
            new ContactDTO { Id = 3, FirstName = "Alice", LastName = "Wonder", Addresses = new List<AddressDTO>() }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<ContactDTO>>(contacts)).Returns(mappedDtos);

        // Act
        var result = await _service.SearchContactAsync("Alice", 1, 10);

        // Assert
        Assert.Single(result);
        Assert.Equal("Alice", result.First().FirstName);
    }

    [Fact]
    public async Task GetContactByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetContactByIdAsync(1)).ReturnsAsync((Contact)null);

        // Act
        var result = await _service.GetContactByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetContactByIdAsync_ReturnsMappedContactDTO_WhenFound()
    {
        // Arrange
        var contact = new Contact { Id = 1, FirstName = "Alice", LastName = "Wonder", Addresses = new List<Address>() };
        _repoMock.Setup(r => r.GetContactByIdAsync(1)).ReturnsAsync(contact);

        var contactDTO = new ContactDTO
            { Id = 1, FirstName = "Alice", LastName = "Wonder", Addresses = new List<AddressDTO>() };
        _mapperMock.Setup(m => m.Map<ContactDTO>(contact)).Returns(contactDTO);

        // Act
        var result = await _service.GetContactByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Alice", result.FirstName);
    }

    [Fact]
    public async Task CreateContactAsync_ReturnsMappedContactDTO()
    {
        // Arrange
        var contactDTO = new ContactDTO
            { Id = 0, FirstName = "Bob", LastName = "Builder", Addresses = new List<AddressDTO>() };
        var contactEntity = new Contact
            { Id = 0, FirstName = "Bob", LastName = "Builder", Addresses = new List<Address>() };
        var createdContact = new Contact
            { Id = 10, FirstName = "Bob", LastName = "Builder", Addresses = new List<Address>() };
        var createdContactDTO = new ContactDTO
            { Id = 10, FirstName = "Bob", LastName = "Builder", Addresses = new List<AddressDTO>() };

        _mapperMock.Setup(m => m.Map<Contact>(contactDTO)).Returns(contactEntity);
        _repoMock.Setup(r => r.CreateContactAsync(contactEntity)).ReturnsAsync(createdContact);
        _mapperMock.Setup(m => m.Map<ContactDTO>(createdContact)).Returns(createdContactDTO);

        // Act
        var result = await _service.CreateContactAsync(contactDTO);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
    }

    [Fact]
    public async Task UpdateContactAsync_ReturnsFalse_WhenIdsDoNotMatch()
    {
        // Arrange
        var contactDTO = new ContactDTO
            { Id = 2, FirstName = "Sam", LastName = "Smith", Addresses = new List<AddressDTO>() };

        // Act
        var result = await _service.UpdateContactAsync(1, contactDTO);

        // Assert
        Assert.False(result);
        _repoMock.Verify(r => r.UpdateContactAsync(It.IsAny<Contact>()), Times.Never);
    }

    [Fact]
    public async Task UpdateContactAsync_ReturnsTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var contactDTO = new ContactDTO
            { Id = 1, FirstName = "Sam", LastName = "Smith", Addresses = new List<AddressDTO>() };
        var contactEntity = new Contact
            { Id = 1, FirstName = "Sam", LastName = "Smith", Addresses = new List<Address>() };

        _mapperMock.Setup(m => m.Map<Contact>(contactDTO)).Returns(contactEntity);
        _repoMock.Setup(r => r.UpdateContactAsync(contactEntity)).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateContactAsync(1, contactDTO);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteContactAsync_ReturnsRepositoryResult()
    {
        // Arrange
        _repoMock.Setup(r => r.DeleteContactAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteContactAsync(1);

        // Assert
        Assert.True(result);
    }
}