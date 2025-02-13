using ClassLibrary1.DTOs;
using ClassLibrary1.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    // GET: api/contact?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContactDTO>>> GetContacts(int pageNumber = 1, int pageSize = 10)
    {
        var contacts = await _contactService.GetContactsAsync(pageNumber, pageSize);
        return Ok(contacts);
    }
    
    // GET: api/contact/search?name=John&pageNumber=1&pageSize=10
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ContactDTO>>> SearchContacts(string name, int pageNumber = 1, int pageSize = 10)
    {
        var contacts = await _contactService.SearchContactAsync(name, pageNumber, pageSize);
        return Ok(contacts);
    }

    // GET: api/contact/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDTO>> GetContactById(int id)
    {
        var contact = await _contactService.GetContactByIdAsync(id);
        return contact == null ? NotFound() : Ok(contact);
    }

    // POST: api/contact
    [HttpPost]
    public async Task<ActionResult<ContactDTO>> CreateContact([FromBody] ContactDTO contactDTO)
    {
        var createdContact = await _contactService.CreateContactAsync(contactDTO);
        return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
    }

    // PUT: api/contact/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDTO contactDTO)
    {
        if (id != contactDTO.Id)
            return BadRequest(new { Message = "ID in url doesn't match ID in request body." });

        var success = await _contactService.UpdateContactAsync(id, contactDTO);
        if (!success)
            return NotFound(new { Message = $"Contact with ID {id} not found." });

        return NoContent();
    }

    // DELETE: api/contact/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var success = await _contactService.DeleteContactAsync(id);
        if (!success)
            return NotFound(new { Message = $"Contact with ID {id} not found." });

        return NoContent();
    }
}