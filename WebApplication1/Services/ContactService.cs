using AutoMapper;
using ClassLibrary1.DTOs;
using ClassLibrary1.Models;
using ClassLibrary1.Repositories;
using ClassLibrary1.Services;

namespace WebApplication1.Service;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IContactRepository contactRepository, IMapper mapper, ILogger<ContactService> logger)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ContactDTO>> GetContactsAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Getting contacts: Page {PageNumber}, Size {PageSize}", pageNumber, pageSize);
        var contacts = await _contactRepository.GetContactsAsync(pageNumber, pageSize);
        return _mapper.Map<IEnumerable<ContactDTO>>(contacts);
    }

    public async Task<IEnumerable<ContactDTO>> SearchContactAsync(string name, int pageNumber, int pageSize)
    {
        var contacts = await _contactRepository.SearchContactAsync(name, pageNumber, pageSize);
        return _mapper.Map<IEnumerable<ContactDTO>>(contacts);
    }

    public async Task<ContactDTO> GetContactByIdAsync(int id)
    {
        _logger.LogInformation("Getting contact using id");
        var contact = await _contactRepository.GetContactByIdAsync(id);
        return contact == null ? null : _mapper.Map<ContactDTO>(contact);
    }

    public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO)
    {
        _logger.LogInformation("Creating contact");
        var contact = _mapper.Map<Contact>(contactDTO);
        var createdContact = await _contactRepository.CreateContactAsync(contact);
        return _mapper.Map<ContactDTO>(createdContact);
    }

    public async Task<bool> UpdateContactAsync(int id, ContactDTO contactDTO)
    {
        if (id != contactDTO.Id)
        {
            _logger.LogInformation("Invalid ID for update contact");
            return false;
        }

        _logger.LogInformation("Updating contact");
        var contact = _mapper.Map<Contact>(contactDTO);
        return await _contactRepository.UpdateContactAsync(contact);
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        _logger.LogWarning("Deleting contact");
        return await _contactRepository.DeleteContactAsync(id);
    }
}