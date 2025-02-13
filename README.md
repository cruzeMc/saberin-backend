## Overview

WebApplication1 is the backend API for the Contact Manager application. It exposes RESTful endpoints to perform CRUD operations on contacts and addresses. The API uses Entity Framework Core for data access, AutoMapper for model-to-DTO mapping, and ASP.NET Core for building RESTful services.

## Folder Structure


## Setup Instructions

1. **Clone the Repository:**  
   Clone the repository to your local machine:
   ```bash
   git clone <repository-url>

2. **Configure Database:**  
   Configure the database by opening appsettings.json and updating the connection string under ConnectionStrings. For example, if using SQL Server:
   ```bash
   "ConnectionStrings": {
         "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ContactManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }


3. **Restore Dependencies:**  
   From the solution root, run 
    ```bash
          dotnet restore

4. **Build the Project:**  
   Navigate to the ClassLibrary1 directory and build the project:  
   ```bash
   dotnet build

5. **Run Tests:**  
   Build the project by navigating to the WebApplication1 directory and running:
   ```bash
   dotnet test

6. **Run:**  
   Run the API by executing:
   ```bash
   dotnet run

## How to Use

The API provides the following endpoints:

*   **`GET`** `/api/contact?pageNumber=1&pageSize=10`: Retrieves a paginated list of contacts.
*   **`GET`** `/api/contact/search?name=John&pageNumber=1&pageSize=10`: Searches for contacts by name (supports combined first and last name queries).
*   **`GET`** `/api/contact/{id}`: Retrieves details of a specific contact.
*   **`POST`** `/api/contact`: Creates a new contact. The request body should be JSON with contact details (first name, last name, and addresses).
*   **`PUT`** `/api/contact/{id}`: Updates an existing contact. The URL ID must match the ID in the JSON body.
*   **`DELETE`** `/api/contact/{id}`: Deletes a contact (along with its associated addresses via cascade delete).