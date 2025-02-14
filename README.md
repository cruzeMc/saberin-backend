## Overview

WebApplication1 is the backend API for the Contact Manager application. It exposes RESTful endpoints to perform CRUD operations on contacts and addresses. The API uses Entity Framework Core for data access, AutoMapper for model-to-DTO mapping, and ASP.NET Core for building RESTful services.

## Folder Structure
```
WebApplication1/
├── Controllers/
│   └── ContactController.cs - Contains endpoints for managing contacts (GET, POST, PUT, DELETE).
├── Data/
│   └── AppDbContext.cs - Defines the EF Core DbContext for accessing Contacts and Addresses.
├── Mappings/
│   └── ContactProfile.cs - Configures AutoMapper mappings between domain models and DTOs.
├── Repositories/
│   └── ContactRepository.cs - Implements data access methods for contacts.
├── Services/
│   └── ContactService.cs - Implements business logic for managing contacts.
├── appsettings.json - Contains configuration settings such as connection strings.
└── Program.cs - The entry point for the application.
```

## Setup Instructions

1. **Clone the Repository:**  
   Clone the repository to your local machine:
   ```bash
   git clone <repository-url>

2. **Clone the Repository:**  
   Clone the repository to your local machine:
   ```bash
   git clone <repository-url>

3. **Navigate to the root directory:**  


4. **Restore Dependencies:**  
   ```bash
   dotnet restore

5. **Build the Project:**  
   ```bash
   dotnet build

6. **Run Tests:**  
   ```bash
   dotnet test

7. **Run:**  
   Run the API by executing:
   ```bash
   dotnet run

## How to Use

The API provides the following endpoints:

*   **`GET`** `/api/contact?pageNumber=1&pageSize=10`: Retrieves a paginated list of contacts.
*   **`GET`** `/api/contact/search?name=John&pageNumber=1&pageSize=10`: Searches for contacts by name (supports combined first and last name queries).
*   **`GET`** `/api/contact/{id}`: Retrieves details of a specific contact.
*   **`POST`** `/api/contact`: Creates a new contact. The request body should be in JSON with contact details (first name, last name, and address).
*   **`PUT`** `/api/contact/{id}`: Updates an existing contact. The URL ID must match the ID in the JSON body.
*   **`DELETE`** `/api/contact/{id}`: Deletes a contact (along with its associated addresses via cascade delete).