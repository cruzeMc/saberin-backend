using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ClassLibrary1.DTOs;
using ClassLibrary1.Models;
using JetBrains.Annotations;
using WebApplication1.Mappings;
using Xunit;

namespace WebApplication1.Tests.Mappings;

[TestSubject(typeof(ContactProfile))]
public class ContactProfileTests
{
    private readonly IMapper _mapper;

    public ContactProfileTests()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddProfile<ContactProfile>(); });
        // This will throw if your mapping configuration is invalid.
        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void ContactMapping_IsValid()
    {
        // Arrange: create a sample Contact with an Address.
        var contact = new Contact
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Addresses = new List<Address>
            {
                new Address
                {
                    Id = 10,
                    Street = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "12345"
                }
            }
        };

        // Act: map Contact to ContactDTO.
        var contactDto = _mapper.Map<ContactDTO>(contact);

        // Assert: verify properties are mapped correctly.
        Assert.Equal(contact.Id, contactDto.Id);
        Assert.Equal(contact.FirstName, contactDto.FirstName);
        Assert.Equal(contact.LastName, contactDto.LastName);
        Assert.NotNull(contactDto.Addresses);
        Assert.Single(contactDto.Addresses);
        Assert.Equal(contact.Addresses.First().Street, contactDto.Addresses.First().Street);

        // Act: map back from ContactDTO to Contact.
        var mappedBackContact = _mapper.Map<Contact>(contactDto);

        // Assert reverse mapping.
        Assert.Equal(contactDto.Id, mappedBackContact.Id);
        Assert.Equal(contactDto.FirstName, mappedBackContact.FirstName);
        Assert.Equal(contactDto.LastName, mappedBackContact.LastName);
        Assert.NotNull(mappedBackContact.Addresses);
        Assert.Single(mappedBackContact.Addresses);
        Assert.Equal(contactDto.Addresses.First().City, mappedBackContact.Addresses.First().City);
    }

    [Fact]
    public void AddressMapping_IsValid()
    {
        // Arrange: create a sample Address.
        var address = new Address
        {
            Id = 5,
            Street = "456 Side St",
            City = "OtherTown",
            State = "CA",
            ZipCode = "98765"
        };

        // Act: map Address to AddressDTO.
        var addressDto = _mapper.Map<AddressDTO>(address);

        // Assert: verify the mapping.
        Assert.Equal(address.Id, addressDto.Id);
        Assert.Equal(address.Street, addressDto.Street);
        Assert.Equal(address.City, addressDto.City);
        Assert.Equal(address.State, addressDto.State);
        Assert.Equal(address.ZipCode, addressDto.ZipCode);

        // Act: map back from AddressDTO to Address.
        var addressMappedBack = _mapper.Map<Address>(addressDto);

        // Assert reverse mapping.
        Assert.Equal(addressDto.Id, addressMappedBack.Id);
        Assert.Equal(addressDto.Street, addressMappedBack.Street);
        Assert.Equal(addressDto.City, addressMappedBack.City);
        Assert.Equal(addressDto.State, addressMappedBack.State);
        Assert.Equal(addressDto.ZipCode, addressMappedBack.ZipCode);
    }
}