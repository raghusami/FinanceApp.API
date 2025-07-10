# 💼 FinanceApp.API – Company Backend Service

**FinanceApp.API** is an enterprise-grade, secure, scalable .NET Core Web API used to manage financial records, reports, and transactions for internal and external finance applications.

This backend service is built following clean architecture principles, with support for JWT authentication, versioned APIs, centralized exception handling, logging, and real-time updates using SignalR.

---

## 🚀 Project Overview

- **Project Name**: FinanceApp.API  
- **Owner**: [Your Company Name]  
- **Technology Stack**: ASP.NET Core, EF Core, JWT, Serilog, SQL Server  
- **Purpose**: Handles finance-related operations and serves secure APIs for web/mobile clients.  
- **Status**: 🟢 Production-ready / 🔵 Under Development / 🔴 Internal Use Only

---

## 📦 Core Modules

| Module               | Description                                              |
|----------------------|----------------------------------------------------------|
| **Authentication**   | Secure login and JWT-based token generation              |
| **Income Records**   | APIs to manage income sources and transaction logs       |
| **Expense Records**  | Endpoints for expense tracking and reporting             |
| **Reports Module**   | Generates summaries, charts, and export-ready reports    |
| **Admin Panel**      | Admin access to user management, audit logs, etc.        |
| **SignalR Hub**      | For real-time updates on dashboard                       |

---

## 🔐 Security Standards

- JWT Bearer Token Authentication
- HTTPS enforced for all endpoints
- Token validation includes:
  - Expiry check
  - Audience/Issuer match
  - Signing key verification
- CORS policy applied based on environment

---

## ⚙️ Environment Setup

### Prerequisites

- .NET SDK 7+
- SQL Server
- Visual Studio or VS Code
- Postman / Swagger

### Environment Files

#### appsettings.json

```json
{
  "ConnectionStrings": {
    "DBConfiguration": "Server=.;Database=FinanceAppDB;Trusted_Connection=True;"
  },
  "AuthConfiguration": {
    "Issuer": "FinanceApp.API",
    "Audience": "FinanceApp.Client",
    "ExpiresInMinutes": 60,
    "SecurityKey": "MyUltraSecureJWTKey@2025#Strong!123"
  },
  "RateLimiter": {
    "ConcurrencyPermitLimit": 10,
    "ConcurrencyQueueLimit": 20,
    "FixedWindowRateLimiter": 30,
    "FixedWindowSize": 60,
    "FixedWindowQueueLimit": 10
  }
}
