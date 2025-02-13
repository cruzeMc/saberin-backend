using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using Xunit;

namespace WebApplication1.Tests.Data;

[TestSubject(typeof(AppDbContext))]
public class AppDbContextTests
{
    private AppDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task DeletingContact_CascadesToAddresses()
    {
        // Arrange
        var dbName = nameof(DeletingContact_CascadesToAddresses);
        using var context = GetInMemoryDbContext(dbName);

        var contact = new Contact
        {
            FirstName = "John",
            LastName = "Doe",
            Addresses =
            {
                new Address { Street = "123 Main St", City = "Springfield", State = "IL", ZipCode = "12345" }
            }
        };

        context.Contacts.Add(contact);
        await context.SaveChangesAsync();

        // Act: Delete the contact.
        context.Contacts.Remove(contact);
        await context.SaveChangesAsync();
        

        // Assert: Ensure address is also deleted.
        var addressesCount = context.Addresses.Count();
        Assert.Equal(0, addressesCount);
    }
}