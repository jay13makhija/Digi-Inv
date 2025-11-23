# 📦 DigiInv

A robust, microservices-ready RESTful API built with **.NET 10** following **Clean Architecture** principles. This system manages users, products, and orders with secure JWT authentication.

## 🚀 Features Implemented

### 1. 🔐 Authentication & Authorization
- **JWT-Based Security**: Secure stateless authentication using JSON Web Tokens.
- **Role-Based Access Control (RBAC)**:
  - **Admin**: Can create, update, and delete products.
  - **User**: Can browse products and place orders.
- **Endpoints**:
  - `POST /api/v1/Auth/register`: Register a new user.
  - `POST /api/v1/Auth/login`: Authenticate and receive a Bearer token.

### 2. 📦 Product Management
- **Stock Tracking**: Real-time stock validation and deduction.
- **Soft Delete**: Products are marked as deleted (`IsDeleted`) instead of being removed from DB.
- **Categories & Filtering**: Filter products by category (e.g., `?category=Electronics`).
- **Discounts**: Support for product discounts which are applied during order calculation.
- **Digital Products**: Support for digital download URLs.
- **Caching**: In-memory caching for product listings (5-minute sliding expiration) to improve performance.
- **Endpoints**:
  - `GET /api/v1/Products`: List all available products (Paged, Filtered).
  - `GET /api/v1/Products/{id}`: View product details.
  - `POST /api/v1/Products`: Add a new product (Admin only).
  - `PUT /api/v1/Products/{id}`: Update product details (Admin only).
  - `DELETE /api/v1/Products/{id}`: Remove a product (Admin only).

### 3. 🛒 Order Management
- **Stock Validation**: Prevents ordering out-of-stock items.
- **Order Workflow**: Status tracking (Pending, Shipped, Delivered, Cancelled).
- **History**: Users can view their order history with paging.
- **Notifications**: Mock email notifications sent upon order confirmation.
- **Endpoints**:
  - `POST /api/v1/Orders`: Create a new order with multiple items.
  - `GET /api/v1/Orders/{id}`: View order details.
  - `GET /api/v1/Orders/user/{userId}`: View all orders for a specific user (Paged).

### 4. 🏗️ Architecture & Design Patterns
- **Clean Architecture**: Separation of concerns into Domain, Application, Infrastructure, and API layers.
- **Repository Pattern**: Abstraction layer for data access (`IGenericRepository`, `IOrderRepository`).
- **Unit of Work (UoW)**: Transaction consistency.
- **AutoMapper**: Efficient object-to-object mapping.
- **Serilog**: Structured logging for requests and errors.
- **Global Exception Handling**: Centralized middleware for error management.
- **Standardized API Responses**: Consistent `ApiResponse<T>` and `PagedResponse<T>` wrappers.
- **Rate Limiting**: Protection against API abuse (100 req/min).
- **API Versioning**: URL-based versioning (e.g., `/api/v1/...`).

## 🛠️ Technology Stack

- **Framework**: .NET 10 (Preview) / .NET 8 Compatible
- **Language**: C#
- **Database**: Entity Framework Core (InMemory for MVP, ready for SQL Server)
- **Authentication**: JWT Bearer
- **Documentation**: Swagger / OpenAPI (Versioning enabled)
- **Containerization**: Docker & Docker Compose
- **Logging**: Serilog
- **Mapping**: AutoMapper
- **Caching**: Microsoft.Extensions.Caching.Memory

## 📂 Project Structure

```
DigiInv
├── DigiInv.Domain/        # Core entities (BaseEntity, Product, Order)
├── DigiInv.Application/   # Services, DTOs, Mappings, Interfaces, Wrappers
├── DigiInv.Infrastructure/# EF Core, Repositories, Services Impl
├── DigiInv.Api/           # Controllers, Middleware, Program.cs
├── docker-compose.yml                 # Container orchestration
└── README.md                          # This file
```

## 🏃‍♂️ How to Run

### Option 1: Local Development

1.  **Restore Dependencies**:
    ```bash
    dotnet restore
    ```
2.  **Run the API**:
    ```bash
    cd DigiInv.Api
    dotnet run
    ```
3.  **Access Swagger**:
    Open your browser to the URL shown (e.g., `https://localhost:7049/swagger`).
    Note: API is versioned, so endpoints are like `/api/v1/...`.

### Option 2: Docker

1.  **Build and Run**:
    ```bash
    docker-compose up --build
    ```
2.  **Access API**:
    `http://localhost:5000/swagger`

## 🧪 Testing the API

1.  **Register**: Create a user account via `/api/v1/Auth/register`.
2.  **Login**: Get your token via `/api/v1/Auth/login`.
3.  **Authorize**: Click "Authorize" in Swagger and enter `Bearer <token>`.
4.  **Create Product**: (Admin) Add a product with `StockQuantity > 0`.
5.  **Place Order**: Buy the product. Check if `StockQuantity` decreases.
6.  **Check History**: View your orders via `/api/v1/Orders/user/{userId}`.

## 📝 Notes

- **Database**: Uses `InMemoryDatabase`. Data resets on restart.
- **Caching**: Product list is cached for 5 minutes.
- **Rate Limiting**: Limit of 100 requests per minute per user/IP.
