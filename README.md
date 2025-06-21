# KMT Backend - Knowledge Management Tool

A comprehensive .NET 8.0 Web API backend system for managing employees, departments, leave requests, missions, and user permissions. Built with Clean Architecture principles and following enterprise-level best practices.

## 🏗️ Project Architecture

This project follows **Clean Architecture** with a **multi-layered structure**:

```
KmtBackend/
├── KmtBackend.API/          # Presentation Layer (Web API)
├── KmtBackend.BLL/          # Business Logic Layer
├── KmtBackend.DAL/          # Data Access Layer
├── KmtBackend.Infrastructure/ # Infrastructure Layer
└── KmtBackend.Models/       # Shared Models/DTOs
```

### **Layer Responsibilities**

#### **1. KmtBackend.API (Presentation Layer)**
- **Controllers**: REST API endpoints for all business operations
- **Attributes**: Custom authorization attributes (`RequirePermissionAttribute`)
- **Common**: Response wrappers and common utilities
- **Mapping**: AutoMapper configurations for DTO mapping
- **Middlewares**: Custom middleware (localization)
- **ServicesExtension**: Dependency injection setup

#### **2. KmtBackend.BLL (Business Logic Layer)**
- **Managers**: Business logic implementation classes
- **Interfaces**: Contracts for business operations
- Handles core business rules and workflows

#### **3. KmtBackend.DAL (Data Access Layer)**
- **Entities**: Domain models and database entities
- **Context**: Entity Framework DbContext
- **Repositories**: Data access implementations
- **Migrations**: Database schema changes
- **Seed**: Database seeding logic
- **Constants**: Application constants and permissions

#### **4. KmtBackend.Infrastructure (Infrastructure Layer)**
- **TokenGenerator**: JWT authentication implementation
- **Helpers**: Utility classes (phone number validation)

#### **5. KmtBackend.Models (Shared Models)**
- **DTOs**: Data Transfer Objects for API requests/responses
- **Enums**: Application enumerations
- Shared across all layers

## 🗄️ Data Models & Relationships

### **Core Entities**

#### **User** (Central Entity)
- **Authentication**: Username, Email, PasswordHash, PhoneNumber
- **Personal Info**: BirthDate, Gender, HireDate, TerminationDate
- **Work Info**: PriorWorkExperienceMonths, Department, Title
- **Relationships**: 
  - Many-to-Many with Role
  - Many-to-One with Department & Title
  - One-to-Many with LeaveBalance & LeaveRequest
  - Many-to-Many with Mission (via MissionAssignment)

#### **Department & Title**
- **Department**: Organizational structure with bilingual support (EN/AR)
- **Title**: Job positions with bilingual support
- **Relationships**: One-to-Many with User

#### **Authorization System**
- **Role**: User roles with permissions
- **Permission**: Granular permissions (format: "resource.action")
- **Relationships**: Many-to-Many between Role and Permission

#### **Leave Management**
- **LeaveType**: Different types of leave (annual, sick, etc.)
- **LeaveBalance**: User's leave balance per year
- **LeaveRequest**: Leave applications with approval workflow
- **Features**: Seniority-based calculations, gender-specific leaves, carry-over rules

#### **Mission Management**
- **Mission**: Task assignments with location and time tracking
- **MissionAssignment**: Junction table for user-mission assignments
- **Features**: Time tracking, location management, assignment tracking

### **Database Constraints**
- Unique constraints on Email, Permission Code, Role Name, LeaveType Name
- Foreign key constraints with restricted deletion
- Composite unique constraint on LeaveBalance (User + Type + Year)

## 🚀 Getting Started

### **Prerequisites**

1. **.NET 8.0 SDK**
   - Download from: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify installation: `dotnet --version`

2. **SQL Server**
   - **SQL Server Express** (free) or **SQL Server Developer Edition**
   - Or **SQL Server LocalDB** (comes with Visual Studio)
   - Verify installation: `sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT @@VERSION"`

3. **IDE (Optional but Recommended)**
   - Visual Studio 2022
   - VS Code with C# extension

### **Configuration**

#### **1. Database Connection**
Create `KmtBackend.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=KmtBackendDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "JwtSettings": {
    "Secret": "your-super-secret-key-with-at-least-32-characters-for-development",
    "Issuer": "KmtBackend",
    "Audience": "KmtBackend",
    "ExpiryMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Note**: For production, use a proper SQL Server instance and change the connection string accordingly.

#### **2. JWT Configuration**
- **Secret**: Use a strong secret key (at least 32 characters)
- **Issuer/Audience**: Can be customized for your organization
- **ExpiryMinutes**: Token expiration time

### **Installation & Setup**

#### **1. Clone and Navigate**
```bash
cd KmtBackend
```

#### **2. Restore Dependencies**
```bash
dotnet restore
```

#### **3. Build the Project**
```bash
dotnet build
```

#### **4. Run Database Migrations**
```bash
cd KmtBackend.API
dotnet ef database update
```

#### **5. Run the Application**
```bash
dotnet run
```

The application will:
- Apply database migrations automatically
- Seed initial data (permissions, roles, super admin user, leave types)
- Start the web server

### **Access Points**

- **API Base URL**: `https://localhost:7073` or `http://localhost:5114`
- **Swagger UI**: `https://localhost:7073/swagger` or `http://localhost:5114/swagger`
- **Default Admin Credentials**:
  - Email: `admin@admin.com`
  - Password: `Admin@123`

## 🔐 Authentication & Authorization

### **JWT Authentication**
- Bearer token-based authentication
- Configurable expiration time
- Secure token validation

### **Role-Based Access Control (RBAC)**
- **Permissions**: Granular permissions (e.g., "users.create", "leaves.approve")
- **Roles**: Collections of permissions
- **Users**: Assigned multiple roles

### **Default Super Admin**
- Automatically created during database seeding
- Has all permissions
- Can manage users, roles, and system settings

## 📋 API Endpoints

### **Authentication**
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh token

### **User Management**
- `GET /api/users` - Get all users
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### **Department Management**
- `GET /api/departments` - Get all departments
- `POST /api/departments` - Create department
- `PUT /api/departments/{id}` - Update department

### **Leave Management**
- `GET /api/leave-requests` - Get leave requests
- `POST /api/leave-requests` - Create leave request
- `PUT /api/leave-requests/{id}/approve` - Approve leave request
- `PUT /api/leave-requests/{id}/reject` - Reject leave request

### **Mission Management**
- `GET /api/missions` - Get all missions
- `POST /api/missions` - Create mission
- `POST /api/missions/{id}/assign-users` - Assign users to mission

### **Role & Permission Management**
- `GET /api/roles` - Get all roles
- `POST /api/roles` - Create role
- `POST /api/roles/{id}/assign-permissions` - Assign permissions to role

## 🛠️ Development

### **Project Structure**
```
KmtBackend/
├── KmtBackend.API/
│   ├── Controllers/          # API endpoints
│   ├── Attributes/           # Custom attributes
│   ├── Middlewares/          # Custom middleware
│   ├── Mapping/              # AutoMapper configs
│   └── ServicesExtension/    # DI configuration
├── KmtBackend.BLL/
│   ├── Managers/             # Business logic
│   └── Interfaces/           # Business contracts
├── KmtBackend.DAL/
│   ├── Entities/             # Domain models
│   ├── Repositories/         # Data access
│   ├── Context/              # EF DbContext
│   └── Migrations/           # Database migrations
├── KmtBackend.Infrastructure/
│   └── TokenGenerator/       # JWT implementation
└── KmtBackend.Models/
    ├── DTOs/                 # Data transfer objects
    └── Enums/                # Application enums
```

### **Adding New Features**

1. **Create Entity** in `KmtBackend.DAL/Entities/`
2. **Add Repository** in `KmtBackend.DAL/Repositories/`
3. **Create Manager** in `KmtBackend.BLL/Managers/`
4. **Add DTOs** in `KmtBackend.Models/DTOs/`
5. **Create Controller** in `KmtBackend.API/Controllers/`
6. **Add Migrations**: `dotnet ef migrations add MigrationName`

### **Database Migrations**
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## 🧪 Testing

### **API Testing**
- Use **Swagger UI** for interactive API testing
- Import Swagger JSON into **Postman** for advanced testing
- Use **curl** or **HTTPie** for command-line testing

### **Database Testing**
```bash
# Connect to database
sqlcmd -S "(localdb)\mssqllocaldb" -d KmtBackendDb

# View tables
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'
```

## 🚀 Deployment

### **Production Considerations**

1. **Database**
   - Use production SQL Server instance
   - Configure connection pooling
   - Set up backup strategies

2. **Security**
   - Change JWT secret to strong key
   - Configure HTTPS
   - Set up proper CORS policies
   - Use environment variables for secrets

3. **Performance**
   - Configure caching strategies
   - Optimize database queries
   - Set up monitoring and logging

### **Environment Variables**
```bash
# Database
ConnectionStrings__DefaultConnection="Server=prod-server;Database=KmtBackendDb;..."

# JWT
JwtSettings__Secret="your-production-secret-key"
JwtSettings__Issuer="your-domain.com"
JwtSettings__Audience="your-domain.com"
```

## 📝 License

This project is proprietary software. All rights reserved.

## 🤝 Contributing

1. Follow the existing code structure and patterns
2. Add appropriate unit tests
3. Update documentation
4. Follow Git commit conventions

## 📞 Support

For technical support or questions, please contact the development team.

---

**KMT Backend** - Empowering organizations with comprehensive knowledge and resource management. 