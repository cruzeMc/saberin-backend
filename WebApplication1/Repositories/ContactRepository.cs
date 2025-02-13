using ClassLibrary1.Models;
using ClassLibrary1.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repositories;

public class ContactRepository : IContactRepository
{
    
    private readonly AppDbContext _context;

    public ContactRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contact>> GetContactsAsync(int pageNumber, int pageSize)
    {
        return await _context.Contacts
            .Include(c => c.Addresses)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contact>> SearchContactAsync(string name, int pageNumber, int pageSize)
    {
        var query = _context.Contacts
            .Include(c => c.Addresses)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var lowerName = name.ToLower();
            query = query
                .Where(c => (c.FirstName + " " + c.LastName).ToLower()
                    .Contains(lowerName));
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();
        return contact;
    }

    public async Task<bool> UpdateContactAsync(Contact contact)
    {
        _context.Contacts.Update(contact);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
            return false;

        _context.Contacts.Remove(contact);
        return await _context.SaveChangesAsync() > 0;
    }
}