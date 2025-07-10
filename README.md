# FinanceApp API 🚀

A secure, scalable, and versioned ASP.NET Core Web API for managing finance-related data. Built with JWT Authentication, Swagger, FluentValidation, EF Core, and supports API versioning and response compression.

---

## 📌 Features

- ✅ JWT-based Authentication and Authorization
- ✅ API Versioning with Swagger UI per version
- ✅ Rate Limiting (Concurrency + Fixed Window)
- ✅ FluentValidation with custom error formatting
- ✅ Centralized Exception Handling Middleware
- ✅ AutoMapper Integration
- ✅ CORS support for development and production
- ✅ SQL Server DB Context with EF Core
- ✅ Form size limits & Response compression (Gzip & Brotli)
- ✅ SignalR support for real-time updates (optional)

---

## 🧱 Tech Stack

- ASP.NET Core 7.0+
- Entity Framework Core
- JWT Bearer Authentication
- Swagger & Swashbuckle
- FluentValidation
- AutoMapper
- Serilog
- SQL Server
- SignalR (optional)

---

## 🔐 Authentication

This API uses JWT (JSON Web Token) authentication. Pass the token in the header:


Token claims example:

```json
{
  "unique_name": "Admin",
  "email": "admin@example.com",
  "nameid": "1001",
  "CreatedAt": "2025-07-01T09:25:37",
  "iss": "FinanceApp.API",
  "aud": "FinanceApp.Client"
}
