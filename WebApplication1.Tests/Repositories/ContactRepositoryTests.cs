using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Repositories;
using Xunit;

namespace WebApplication1.Tests.Repositories;

[TestSubject(typeof(ContactRepository))]
public class ContactRepositoryTests
{
        // Helper method to create a new in-memory DbContext instance.
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AppDbContext(options);
        }

        // Helper method to create a sample Contact with one Address.
        private Contact CreateSampleContact(string firstName = "John", string lastName = "Doe")
        {
            return new Contact
            {
                FirstName = firstName,
                LastName = lastName,
                Addresses = new List<Address>
                {
                    new Address 
                    { 
                        Street = "123 Main Street", 
                        City = "SampleCity", 
                        State = "ST", 
                        ZipCode = "12345",
                        // ContactId is set automatically once the contact is added if configured
                    }
                }
            };
        }

        [Fact]
        public async Task GetContactsAsync_ReturnsContacts()
        {
            // Arrange
            var dbName = nameof(GetContactsAsync_ReturnsContacts);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            
            // Seed data
            context.Contacts.Add(CreateSampleContact("John", "Doe"));
            context.Contacts.Add(CreateSampleContact("Jane", "Smith"));
            await context.SaveChangesAsync();
            
            // Act
            var contacts = await repo.GetContactsAsync(pageNumber: 1, pageSize: 10);
            
            // Assert
            Assert.NotNull(contacts);
            Assert.Equal(2, contacts.Count());
        }
        
        [Fact]
        public async Task SearchContactAsync_ReturnsMatchingContacts()
        {
            // Arrange
            var dbName = nameof(SearchContactAsync_ReturnsMatchingContacts);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            
            // Seed data
            context.Contacts.Add(CreateSampleContact("John", "Doe"));
            context.Contacts.Add(CreateSampleContact("Jane", "Smith"));
            await context.SaveChangesAsync();
            
            // Act
            var results = await repo.SearchContactAsync("Jane", pageNumber: 1, pageSize: 10);
            
            // Assert
            Assert.Single(results);
            Assert.Equal("Jane", results.First().FirstName);
        }
        
        [Fact]
        public async Task GetContactByIdAsync_ReturnsContact()
        {
            // Arrange
            var dbName = nameof(GetContactByIdAsync_ReturnsContact);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            var contact = CreateSampleContact("Alice", "Wonderland");
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            
            // Act
            var retrieved = await repo.GetContactByIdAsync(contact.Id);
            
            // Assert
            Assert.NotNull(retrieved);
            Assert.Equal("Alice", retrieved.FirstName);
        }
        
        [Fact]
        public async Task CreateContactAsync_CreatesContact()
        {
            // Arrange
            var dbName = nameof(CreateContactAsync_CreatesContact);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            var contact = CreateSampleContact("Bob", "Builder");
            
            // Act
            var created = await repo.CreateContactAsync(contact);
            
            // Assert
            Assert.NotNull(created);
            Assert.NotEqual(0, created.Id);
            Assert.Equal(1, context.Contacts.Count());
        }
        
        [Fact]
        public async Task UpdateContactAsync_UpdatesContact()
        {
            // Arrange
            var dbName = nameof(UpdateContactAsync_UpdatesContact);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            var contact = CreateSampleContact("Charlie", "Chaplin");
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            
            // Act
            contact.FirstName = "Charles";
            var result = await repo.UpdateContactAsync(contact);
            
            // Assert
            Assert.True(result);
            var updated = await context.Contacts.FindAsync(contact.Id);
            Assert.Equal("Charles", updated.FirstName);
        }
        
        [Fact]
        public async Task DeleteContactAsync_DeletesContact()
        {
            // Arrange
            var dbName = nameof(DeleteContactAsync_DeletesContact);
            using var context = GetDbContext(dbName);
            var repo = new ContactRepository(context);
            var contact = CreateSampleContact("Daisy", "Duck");
            context.Contacts.Add(contact);
            await context.SaveChangesAsync();
            
            // Act
            var result = await repo.DeleteContactAsync(contact.Id);
            
            // Assert
            Assert.True(result);
            var deleted = await context.Contacts.FindAsync(contact.Id);
            Assert.Null(deleted);
        }
    }