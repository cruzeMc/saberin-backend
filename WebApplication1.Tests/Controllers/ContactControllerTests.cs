using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using Xunit;

namespace WebApplication1.Tests.Controllers;

[TestSubject(typeof(ContactController))]
public class ContactControllerTests
{
    private readonly Mock<IContactService> _serviceMock;
    private readonly ContactController _controller;

    public ContactControllerTests()
    {
        _serviceMock = new Mock<IContactService>();
        _controller = new ContactController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetContacts_ReturnsOkResult_WithListOfContacts()
    {
        // Arrange
        var contacts = new List<ContactDTO>
        {
            new ContactDTO { Id = 1, FirstName = "John", LastName = "Doe" },
            new ContactDTO { Id = 2, FirstName = "Jane", LastName = "Smith" }
        };

        _serviceMock.Setup(s => s.GetContactsAsync(1, 10))
            .ReturnsAsync(contacts);

        // Act
        var result = await _controller.GetContacts(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContacts = Assert.IsAssignableFrom<IEnumerable<ContactDTO>>(okResult.Value);
        Assert.Equal(2, returnedContacts.Count());
    }

    [Fact]
    public async Task SearchContacts_ReturnsOkResult_WithMatchingContacts()
    {
        // Arrange
        var contacts = new List<ContactDTO>
        {
            new ContactDTO { Id = 3, FirstName = "Alice", LastName = "Wonder" }
        };

        _serviceMock.Setup(s => s.SearchContactAsync("Alice", 1, 10))
            .ReturnsAsync(contacts);

        // Act
        var result = await _controller.SearchContacts("Alice", 1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContacts = Assert.IsAssignableFrom<IEnumerable<ContactDTO>>(okResult.Value);
        Assert.Single(returnedContacts);
    }

    [Fact]
    public async Task GetContactById_ReturnsNotFound_WhenServiceReturnsNull()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync((ContactDTO)null);

        // Act
        var result = await _controller.GetContactById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetContactById_ReturnsOkResult_WhenContactIsFound()
    {
        // Arrange
        var contact = new ContactDTO { Id = 1, FirstName = "Alice", LastName = "Wonder" };
        _serviceMock.Setup(s => s.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        // Act
        var result = await _controller.GetContactById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContact = Assert.IsType<ContactDTO>(okResult.Value);
        Assert.Equal("Alice", returnedContact.FirstName);
    }

    [Fact]
    public async Task CreateContact_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var newContact = new ContactDTO { Id = 0, FirstName = "Bob", LastName = "Builder" };
        var createdContact = new ContactDTO { Id = 5, FirstName = "Bob", LastName = "Builder" };

        _serviceMock.Setup(s => s.CreateContactAsync(newContact))
            .ReturnsAsync(createdContact);

        // Act
        var result = await _controller.CreateContact(newContact);

        // Assert
        var createdAtAction = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedContact = Assert.IsType<ContactDTO>(createdAtAction.Value);
        Assert.Equal(5, returnedContact.Id);
    }

    [Fact]
    public async Task UpdateContact_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var contactToUpdate = new ContactDTO { Id = 2, FirstName = "Sam", LastName = "Smith" };

        // Act
        var result = await _controller.UpdateContact(1, contactToUpdate);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateContact_ReturnsNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        var contactToUpdate = new ContactDTO { Id = 1, FirstName = "Sam", LastName = "Smith" };
        _serviceMock.Setup(s => s.UpdateContactAsync(1, contactToUpdate))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateContact(1, contactToUpdate);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateContact_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var contactToUpdate = new ContactDTO { Id = 1, FirstName = "Sam", LastName = "Smith" };
        _serviceMock.Setup(s => s.UpdateContactAsync(1, contactToUpdate))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateContact(1, contactToUpdate);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteContact_ReturnsNotFound_WhenServiceReturnsFalse()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteContactAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteContact(1);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteContact_ReturnsNoContent_WhenServiceReturnsTrue()
    {
        // Arrange
        _serviceMock.Setup(s => s.DeleteContactAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteContact(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}