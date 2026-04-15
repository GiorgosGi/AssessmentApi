# Assessment Project

## 🎯 Overview

This API provides two main sets of functionality:

1. **Country Service**: Aggregates country data from the REST Countries API, caches results, and persists them to a SQL Server database.
2. **Math Service**: In-memory service that computes the second-largest distinct number in a dataset, independent of external dependencies or persistence.

### Key Features

- ✅ Clean Architecture with separated concerns (Domain, Application, Infrastructure)
- ✅ Entity Framework Core with SQL Server
- ✅ In-memory caching
- ✅ Comprehensive logging with Serilog
- ✅ API documentation with Swagger/OpenAPI
- ✅ Cancellation token support
- ✅ Exception handling with detailed error responses
- ✅ Unit tests with xUnit (for demonstration purposes)

## 🏗️ Architecture

The project follows **Clean Architecture** principles:

```
AssessmentApi/
├── Domain/                    # Core business logic (entities)
├── Application/               # Business rules (services, interfaces, DTOs)
├── Infrastructure/            # External concerns (database, HTTP clients)
├── Api/                       # Presentation layer (controllers, API setup)
└── Tests/                     # Unit tests
```

### Architecture Layers

#### Domain Layer (`Domain/`)
- Contains core business entities
- No external dependencies

#### Application Layer (`Application/`)
- Contains business logic and service interfaces
- Defines DTOs (Data Transfer Objects)
- Implements the dependency injection contracts

#### Infrastructure Layer (`Infrastructure/`)
- Implements external integrations
- Contains EF Core configurations
- HTTP client for REST Countries API
- Database context and migrations

#### Api Layer (`Api/`)
- ASP.NET Core API controllers
- Request/response handling
- Swagger/OpenAPI configuration
- Dependency injection setup

### Dependency Rules:
- Api → depends on Application and Infrastructure (for dependency injection setup)
- Application → depends on Domain
- Infrastructure → depends on Application and Domain
- Domain → has no dependencies

### Data Flow for Country Service:

```
Client Request
    ↓
Controller (API Layer)
    ↓
Service (Application Layer)
    ├→ Check Cache (Memory)
    ├→ Query Database (Infrastructure)
    └→ Call External API (REST Countries)
         ↓
    Persist to Database
         ↓
    Update Cache
    ↓
Response
```

## 📋 Prerequisites

- **.NET SDK 10.0** or later
- **SQL Server 2019** or later (or SQL Server Express LocalDB for development)
- **Visual Studio 2022** or **Visual Studio Code**
- **Git** for version control

## 🚀 Setup Instructions

### 1. Clone the Repository

git clone <repo>

### 2. Restore Dependencies

dotnet restore

### 3. Configure Database Connection

Open `AssessmentApi/appsettings.json` and update the connection string if needed:

### 4. Apply Database Migrations

dotnet ef database update

### 5. Run the Application

dotnet run

The API will start at:
- **HTTP**: `http://localhost:5238`
- **HTTPS**: `https://localhost:7208`

### 6. Access Swagger Documentation

Open your browser and navigate to: `https://localhost:7208/swagger/index.html`

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

## 📡 API Endpoints

### Countries Endpoint

#### Get All Countries

```http
GET /api/countries
```

**Response (200 OK):**

```json
[
  {
    "name": "Mexico",
    "capital": "Mexico City",
    "borders": ["BLZ", "GTM", "USA"]
  },
  {
    "name": "Canada",
    "capital": "Ottawa",
    "borders": ["USA"]
  }
]
```

**Response Codes:**
- `200 OK` - Successfully retrieved countries
- `503 Service Unavailable` - External API or database unavailable
- `500 Internal Server Error` - Unexpected error

**Example cURL:**

```bash
curl -X GET "https://localhost:7208/api/countries" \
  -H "Accept: application/json"
```

### Math Endpoint

#### Get Second Largest Number

```http
POST /api/math/second-largest
Content-Type: application/json
```

**Request Body:**

```json
{
  "requestArrayObj": [5, 20, 9, 3, 27]
}
```

**Response (200 OK):**

```json
{
  "value": 20
}
```

**Request Examples:**

```bash
# With curl
curl -X POST "https://localhost:7208/api/math/second-largest" \
  -H "Content-Type: application/json" \
  -d '{"requestArrayObj": [5, 20, 9, 3, 27]}'

# With PowerShell
$body = @{
    requestArrayObj = @(5, 20, 9, 3, 27)
} | ConvertTo-Json

Invoke-WebRequest -Uri "https://localhost:7208/api/math/second-largest" `
  -Method POST `
  -ContentType "application/json" `
  -Body $body
```

**Error Responses:**

- `400 Bad Request` - Array has fewer than 2 distinct values or null input
- `500 Internal Server Error` - Unexpected error

**Example Errors:**

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "requestArrayObj": ["At least two distinct numbers required"]
  }
}
```

### Adding a New Feature

1. **Define the Domain Model** in `Domain/`
2. **Create Service Interfaces** in `Application/Interfaces/`
3. **Implement Services** in `Application/Services/`
4. **Add Repository/Data Access** in `Infrastructure/Repositories/`
5. **Create Controller** in `Api/Controllers/`
6. **Register in DI** in `Program.cs`
7. **Write Tests** in `Tests/`


## 📊 Monitoring & Logging

### Log Files

Logs are written to:
- **Console**: Real-time output
- **File**: `logs/log-YYYYMMDD.txt` (daily rotation)
