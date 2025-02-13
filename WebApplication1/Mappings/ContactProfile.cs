using AutoMapper;
using ClassLibrary1.DTOs;
using ClassLibrary1.Models;

namespace WebApplication1.Mappings;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        // Contact Mapping
        CreateMap<Contact, ContactDTO>().ReverseMap();

        // Address Mapping
        CreateMap<Address, AddressDTO>().ReverseMap();
    }
}