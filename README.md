# Clinic Booking System (PHCIS)

A comprehensive Primary Health Care Information System (PHCIS) solution for managing clinic appointments, enabling patients to book appointments and clinics to manage schedules efficiently.

## Table of Contents

1. [Project Overview](#project-overview)
2. [Requirements Analysis](#requirements-analysis)
3. [System Architecture](#system-architecture)
4. [Data Model](#data-model)
5. [Technology Stack](#technology-stack)
6. [Setup & Installation](#setup--installation)
7. [Features](#features)
8. [Code Quality](#code-quality)
9. [API Endpoints](#api-endpoints)

---

## Project Overview

The Clinic Booking System simplifies appointment scheduling for healthcare providers and patients. It provides:
- **Patients**: Easy booking, rescheduling, and cancellation of appointments
- **Clinic Staff**: Management of clinics, doctors, and appointment schedules
- **System Admin**: User management, reporting, and system configuration

---

## Requirements Analysis

### Stakeholders
1. **Patients** - Book, view, and manage appointments
2. **Clinic Staff** - Manage clinics, doctors, and schedules
3. **Clinic Administrators** - Configure clinic settings and manage staff
4. **System Administrator** - System maintenance, security, and monitoring

### Functional Requirements

#### Patient Management
- [x] Patient registration and authentication
- [x] View available clinics and doctors
- [x] View available time slots for clinics
- [x] Book appointments
- [x] Reschedule appointments
- [x] Cancel appointments
- [x] View appointment history
- [x] Receive appointment confirmations
- [x] Receive appointment reminders

#### Clinic Management
- [x] Clinic registration and profile management
- [x] Manage clinic details (location, hours, specialization)
- [x] Manage doctors/healthcare providers
- [x] Define working hours and available time slots
- [x] View upcoming appointments
- [x] Mark appointments as completed or no-show
- [x] View patient feedback/ratings

#### Appointment Management
- [x] Create appointment slots
- [x] Prevent double bookings
- [x] Manage appointment capacity
- [x] Generate appointment reports
- [x] Appointment status tracking (Scheduled/Completed/Cancelled/NoShow)

### Non-Functional Requirements

#### Security
- [x] Authentication (JWT-based)
- [x] Authorization (Role-based access control)
- [x] Data encryption (HTTPS/TLS)
- [x] Secure password policies
- [x] HIPAA compliance considerations
- [x] Audit logging of all operations

#### Performance
- [x] Response time < 2 seconds for API calls
- [x] Support 1000+ concurrent users
- [x] Efficient database queries (indexing, caching)
- [x] Lazy loading for large datasets

#### Scalability
- [x] Modular architecture for future microservices
- [x] Containerized deployment (Docker)
- [x] Database replication support
- [x] Horizontal scaling capability

#### Reliability & Availability
- [x] 99.5% uptime SLA
- [x] Backup and disaster recovery procedures
- [x] Error handling and logging
- [x] Graceful degradation

#### Usability
- [x] Intuitive user interface
- [x] Mobile-responsive design
- [x] Multi-language support (optional)
- [x] Accessibility compliance (WCAG 2.1)

---

## System Architecture

### Architecture Pattern: Layered Architecture with Modular Design

```
┌─────────────────────────────────────────────────────┐
│              Presentation Layer                      │
│  (Blazor WASM - UI Components, Pages, State Mgmt)  │
└─────────────┬───────────────────────────────────────┘
              │ (HTTP/REST)
┌─────────────▼───────────────────────────────────────┐
│         ASP.NET Core Web API Layer                   │
│  (Controllers, DTOs, Request Validation)            │
└─────────────┬───────────────────────────────────────┘
              │
┌─────────────▼───────────────────────────────────────┐
│         Business Logic Layer                         │
│  (Services, Use Cases, Domain Logic)                │
└─────────────┬───────────────────────────────────────┘
              │
┌─────────────▼───────────────────────────────────────┐
│         Data Access Layer                            │
│  (Entity Framework Core, Repositories)              │
└─────────────┬───────────────────────────────────────┘
              │
┌─────────────▼───────────────────────────────────────┐
│         Database Layer                               │
│  (SQL Server / PostgreSQL)                          │
└─────────────────────────────────────────────────────┘
```

### Module Structure

```
ClinicBookingSystem/
├── ClinicBookingSystem.API/                 # ASP.NET Core Web API
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── PatientsController.cs
│   │   ├── ClinicsController.cs
│   │   ├── AppointmentsController.cs
│   │   ├── TimeSlotsController.cs
│   │   └── RatingsController.cs
│   ├── Services/
│   │   ├── IAuthService.cs & AuthService.cs
│   │   ├── IAppointmentService.cs & AppointmentService.cs
│   │   ├── IClinicService.cs & ClinicService.cs
│   │   ├── ITimeSlotService.cs & TimeSlotService.cs
│   │   └── INotificationService.cs & NotificationService.cs
│   ├── DTOs/
│   │   ├── Request/
│   │   │   ├── BookAppointmentDto.cs
│   │   │   ├── RescheduleAppointmentDto.cs
│   │   │   └── CreateClinicDto.cs
│   │   └── Response/
│   │       ├── AppointmentResponseDto.cs
│   │       ├── ClinicResponseDto.cs
│   │       └── TimeSlotResponseDto.cs
│   ├── Models/
│   ├── Data/
│   │   ├── ClinicDbContext.cs
│   │   └── Migrations/
│   ├── Middleware/
│   │   ├── ExceptionHandlingMiddleware.cs
│   │   └── AuditLoggingMiddleware.cs
│   └── Program.cs
├── ClinicBookingSystem.Core/                # Domain Models & Interfaces
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Patient.cs
│   │   ├── Clinic.cs
│   │   ├── Doctor.cs
│   │   ├── TimeSlot.cs
│   │   ├── Appointment.cs
│   │   ├── Rating.cs
│   │   └── AuditLog.cs
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   └── IEntity.cs
│   ├── Enums/
│   │   ├── AppointmentStatus.cs
│   │   ├── UserRole.cs
│   │   └── ReminderType.cs
│   └── Constants/
│       └── AppConstants.cs
├── ClinicBookingSystem.Infrastructure/      # Data Access & External Services
│   ├── Data/
│   │   └── ClinicDbContext.cs
│   ├── Repositories/
│   │   ├── Repository.cs
│   │   ├── AppointmentRepository.cs
│   │   ├── ClinicRepository.cs
│   │   ├── PatientRepository.cs
│   │   └── UnitOfWork.cs
│   ├── Migrations/
│   │   ├── 001_InitialCreate.cs
│   │   └── 002_AddAuditTables.cs
│   └── Services/
│       ├── NotificationService.cs
│       └── ExternalApiService.cs
├── ClinicBookingSystem.Blazor/              # Blazor WASM Frontend
│   ├── Pages/
│   │   ├── Index.razor
│   │   ├── Auth/
│   │   │   ├── Login.razor
│   │   │   └── Register.razor
│   │   ├── Patient/
│   │   │   ├── BrowseClinics.razor
│   │   │   ├── BookAppointment.razor
│   │   │   ├── MyAppointments.razor
│   │   │   └── RateClinic.razor
│   │   ├── Clinic/
│   │   │   ├── Dashboard.razor
│   │   │   ├── ManageTimeSlots.razor
│   │   │   ├── ViewAppointments.razor
│   │   │   └── ClinicProfile.razor
│   │   └── Admin/
│   │       ├── UserManagement.razor
│   │       ├── AuditLogs.razor
│   │       └── SystemSettings.razor
│   ├── Components/
│   │   ├── AppointmentCard.razor
│   │   ├── ClinicCard.razor
│   │   ├── TimeSlotSelector.razor
│   │   ├── ConfirmationDialog.razor
│   │   └── NotificationBanner.razor
│   ├── Services/
│   │   ├── IApiService.cs & ApiService.cs
│   │   ├── IAuthService.cs & AuthService.cs
│   │   ├── IAppointmentService.cs & AppointmentService.cs
│   │   └── IClinicService.cs & ClinicService.cs
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   └── Sidebar.razor
│   ├── Shared/
│   │   └── Error.razor
│   ├── App.razor
│   └── Program.cs
├── ClinicBookingSystem.Tests/               # Unit & Integration Tests
│   ├── UnitTests/
│   │   ├── Services/
│   │   │   ├── AppointmentServiceTests.cs
│   │   │   ├── ClinicServiceTests.cs
│   │   │   └── TimeSlotServiceTests.cs
│   │   └── Validators/
│   │       └── BookAppointmentValidatorTests.cs
│   ├── IntegrationTests/
│   │   ├── ApiTests/
│   │   │   ├── AppointmentsControllerTests.cs
│   │   │   ├── ClinicsControllerTests.cs
│   │   │   └── AuthControllerTests.cs
│   │   └── Fixtures/
│   │       └── DatabaseFixture.cs
│   └── Fixtures/
│       └── TestDataFactory.cs
├── docker-compose.yml
├── Dockerfile
├── .github/
│   └── workflows/
│       └── ci-cd.yml
├── LICENSE
└── README.md
```

---

## Data Model

### Entity Relationship Diagram (ERD)

```
Users (BaseEntity)
├── Id (PK, GUID)
├── Email (UNIQUE, NOT NULL)
├── PasswordHash
├── FirstName
├── LastName
├── PhoneNumber
├── DateOfBirth
├── Gender
├── ProfilePictureUrl
├── IsActive
├── UserRole (enum)
├── CreatedAt
├── UpdatedAt
└── DeletedAt (soft delete)

Patients (Inherits Users)
├── UserId (FK to Users)
├── IdentityNumber (UNIQUE)
├── Address
├── City
├── Province
├── PostalCode
├── AllergyInformation
├── MedicalHistory
└── EmergencyContactPhone

Clinics (BaseEntity)
├── Id (PK, GUID)
├── Name
├── Address
├── City
├── Province
├── PostalCode
├── Latitude
├── Longitude
├── PhoneNumber
├── Email
├── LogoUrl
├── OperatingHoursJson (JSON field)
├── Specialization
├── MaxDailyAppointments
├── Rating (decimal 0-5)
├── IsActive
├── CreatedAt
├── UpdatedAt

Doctors (BaseEntity)
├── Id (PK, GUID)
├── ClinicId (FK to Clinics)
├── FirstName
├── LastName
├── Email
├── PhoneNumber
├── LicenseNumber
├── Specialization
├── ConsultationFee
├── IsActive
├── CreatedAt
├── UpdatedAt

TimeSlots (BaseEntity)
├── Id (PK, GUID)
├── DoctorId (FK to Doctors)
├── ClinicId (FK to Clinics)
├── StartTime
├── EndTime
├── SlotDate
├── Capacity
├── AvailableSlots
├── IsBooked
├── CreatedAt
└── UpdatedAt

Appointments (BaseEntity)
├── Id (PK, GUID)
├── PatientId (FK to Patients)
├─�� TimeSlotId (FK to TimeSlots)
├── ClinicId (FK to Clinics)
├── DoctorId (FK to Doctors)
├── AppointmentDate
├── Status (enum: Scheduled/Completed/Cancelled/NoShow)
├── ReasonForVisit
├── Notes
├── IsPaid
├── CreatedAt
├── CancelledAt
└── CancellationReason

Ratings (BaseEntity)
├── Id (PK, GUID)
├── AppointmentId (FK to Appointments)
├── DoctorId (FK to Doctors)
├── PatientId (FK to Patients)
├── ClinicId (FK to Clinics)
├── Rating (1-5)
├── Comment
├── CreatedAt

AuditLogs (BaseEntity)
├── Id (PK, GUID)
├── UserId (FK to Users, nullable)
├── Action
├── Entity
├── EntityId
├── OldValue (JSON)
├── NewValue (JSON)
├── IpAddress
├── Timestamp
```

---

## Technology Stack

### Frontend
- **Blazor WebAssembly** - Interactive C# UI
- **MudBlazor** - Material Design components
- **FluentValidation** - Client-side validation
- **HttpClientFactory** - API communication

### Backend
- **ASP.NET Core 8+** - Modern web framework
- **Entity Framework Core** - ORM
- **AutoMapper** - DTO mapping
- **FluentValidation** - Server-side validation
- **Serilog** - Structured logging
- **JWT Bearer** - Token-based authentication

### Database
- **SQL Server** or **PostgreSQL** - Relational database
- **EF Core Migrations** - Schema versioning

### Authentication & Authorization
- **ASP.NET Core Identity** - User management
- **JWT Tokens** - API security
- **Role-Based Access Control (RBAC)** - Permission management

### Testing
- **xUnit** - Unit testing framework
- **Moq** - Mocking library
- **FluentAssertions** - Assertion helpers
- **Testcontainers** - Test database containers

### DevOps & Deployment
- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **GitHub Actions** - CI/CD pipeline
- **Swagger/OpenAPI** - API documentation

---

## Setup & Installation

### Prerequisites
- .NET 8 SDK or later
- SQL Server 2019+ or PostgreSQL 12+
- Docker Desktop (for containerization)
- Git

### Local Development Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/swdmajozi/ClinicBookingSystem.git
   cd ClinicBookingSystem
   ```

2. **Configure Database Connection**
   - Update `appsettings.json` in `ClinicBookingSystem.API` project:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=ClinicBookingSystemDb;User Id=sa;Password=YourPassword;"
     }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   cd ClinicBookingSystem.API
   dotnet ef database update
   ```

4. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

5. **Run the API** (Terminal 1)
   ```bash
   cd ClinicBookingSystem.API
   dotnet run
   ```
   - API runs at: `https://localhost:7000`
   - Swagger Docs: `https://localhost:7000/swagger`

6. **Run the Blazor Frontend** (Terminal 2)
   ```bash
   cd ClinicBookingSystem.Blazor
   dotnet run
   ```
   - Frontend runs at: `https://localhost:7001`

### Docker Deployment

1. **Build and Run**
   ```bash
   docker-compose up --build
   ```

2. **Access Services**
   - Frontend: `http://localhost:3000`
   - API: `http://localhost:5000`
   - SQL Server: `localhost:1433`

3. **Shutdown**
   ```bash
   docker-compose down
   ```

---

## Features

### 🏥 Patient Features
- 📋 **View Clinics** - Browse available clinics with advanced filters
- 👨‍⚕️ **Search Doctors** - Find doctors by specialization
- 📅 **Book Appointments** - Select clinic, doctor, date, and time
- 🔔 **Instant Confirmation** - Receive booking confirmation immediately
- 📧 **Reminders** - Email/SMS reminders 24 hours before appointment
- ✏️ **Reschedule** - Modify appointment date/time (if allowed)
- ❌ **Cancel** - Cancel appointments with cancellation reason
- ⭐ **Rate & Review** - Provide feedback on clinic experience
- 📜 **Appointment History** - View past appointments and medical records
- 📱 **Mobile Responsive** - Book anytime, anywhere

### 🏥 Clinic Features
- 🏢 **Clinic Management** - Configure clinic details and operating hours
- 👨‍⚕️ **Doctor Management** - Add/remove doctors and manage specializations
- 📊 **Schedule Management** - Create, update, and manage time slots
- 👥 **Appointment View** - See upcoming, completed, and cancelled appointments
- 📈 **Reports** - Generate appointment and performance reports
- ⭐ **Ratings View** - Monitor clinic ratings and patient feedback
- 📞 **Patient Communication** - Send reminders and notifications
- 💰 **Billing** - Track payments and appointments

### 👨‍💼 Admin Features
- 🔐 **User Management** - Create, edit, and deactivate users
- 🛡️ **Audit Logs** - Track all system activities and changes
- 📊 **System Analytics** - Monitor usage, performance, and trends
- ⚙️ **System Configuration** - Configure system-wide settings
- 📧 **Notification Settings** - Configure reminder templates and delivery
- 🎯 **Permissions Management** - Manage user roles and permissions

---

## Code Quality

### SOLID Principles Implementation
- **S**ingle Responsibility - Each class has one reason to change
- **O**pen/Closed - Open for extension, closed for modification
- **L**iskov Substitution - Derived classes can substitute base classes
- **I**nterface Segregation - Specific interfaces over general ones
- **D**ependency Inversion - Depend on abstractions, not concrete types

### Naming Conventions
```csharp
// Classes & Interfaces: PascalCase
public interface IAppointmentService { }
public class AppointmentService { }

// Methods & Properties: PascalCase
public async Task<AppointmentDto> GetAppointmentByIdAsync(Guid id) { }

// Variables & Parameters: camelCase
var appointmentId = Guid.NewGuid();
int maxAppointments = 10;

// Constants: UPPER_SNAKE_CASE
private const int MAX_APPOINTMENT_DURATION = 30; // minutes
private const string DEFAULT_TIMEZONE = "UTC";

// Private Fields: _camelCase
private readonly IRepository<Appointment> _appointmentRepository;
private readonly ILogger<AppointmentService> _logger;
```

### Documentation Standards
- **XML Comments** on all public methods and classes
- **README files** for complex modules
- **Inline Comments** for business logic and algorithms
- **Architecture Decision Records (ADRs)** for major decisions

### Testing Strategy

#### Unit Tests (80%+ coverage)
```csharp
[Fact]
public async Task BookAppointment_WithValidData_ShouldCreateAppointment()
{
    // Arrange
    var appointmentService = new AppointmentService(...);
    var bookingDto = new BookAppointmentDto { /* ... */ };
    
    // Act
    var result = await appointmentService.BookAppointmentAsync(bookingDto);
    
    // Assert
    Assert.NotNull(result);
    Assert.True(result.Id != Guid.Empty);
}
```

#### Integration Tests
- Test API endpoints with real database
- Validate end-to-end workflows
- Test authentication and authorization

#### UI Tests (bUnit)
- Test Blazor components
- Test user interactions
- Test validation feedback

---

## API Endpoints

### 🔐 Authentication Endpoints
```
POST   /api/auth/register              # User registration
POST   /api/auth/login                 # User login
POST   /api/auth/refresh-token         # Refresh JWT token
POST   /api/auth/logout                # User logout
POST   /api/auth/forgot-password       # Request password reset
POST   /api/auth/reset-password        # Reset password with token
```

### 👥 Patient Endpoints
```
GET    /api/patients/{id}              # Get patient profile
PUT    /api/patients/{id}              # Update patient profile
GET    /api/patients/{id}/appointments # Get patient appointments
DELETE /api/patients/{id}              # Delete patient account
```

### 🏥 Clinic Endpoints
```
GET    /api/clinics                    # List all clinics (with filters)
GET    /api/clinics/{id}               # Get clinic details
GET    /api/clinics/{id}/doctors       # Get clinic doctors
GET    /api/clinics/{id}/ratings       # Get clinic ratings
GET    /api/clinics/{id}/time-slots    # Get available time slots
POST   /api/clinics                    # Create clinic (Admin)
PUT    /api/clinics/{id}               # Update clinic (Clinic Admin)
DELETE /api/clinics/{id}               # Delete clinic (Admin)
```

### 👨‍⚕️ Doctor Endpoints
```
GET    /api/doctors                    # List all doctors
GET    /api/doctors/{id}               # Get doctor details
POST   /api/doctors                    # Create doctor (Clinic Admin)
PUT    /api/doctors/{id}               # Update doctor (Clinic Admin)
DELETE /api/doctors/{id}               # Delete doctor (Clinic Admin)
```

### 📅 Appointment Endpoints
```
POST   /api/appointments               # Book appointment
GET    /api/appointments/{id}          # Get appointment details
GET    /api/appointments               # List appointments (filtered)
PUT    /api/appointments/{id}          # Reschedule appointment
DELETE /api/appointments/{id}          # Cancel appointment
PATCH  /api/appointments/{id}/status   # Update appointment status (Clinic staff)
```

### ⏰ Time Slot Endpoints
```
GET    /api/time-slots                 # Get available time slots
GET    /api/time-slots/{id}            # Get time slot details
POST   /api/time-slots                 # Create time slot (Clinic Admin)
PUT    /api/time-slots/{id}            # Update time slot (Clinic Admin)
DELETE /api/time-slots/{id}            # Remove time slot (Clinic Admin)
```

### ⭐ Rating Endpoints
```
POST   /api/ratings                    # Submit rating/review
GET    /api/clinics/{id}/ratings       # Get clinic ratings
GET    /api/doctors/{id}/ratings       # Get doctor ratings
PUT    /api/ratings/{id}               # Update rating (Owner)
DELETE /api/ratings/{id}               # Delete rating (Owner/Admin)
```

### 🛡️ Admin Endpoints
```
GET    /api/admin/users                # List all users
GET    /api/admin/users/{id}           # Get user details
POST   /api/admin/users                # Create user
PUT    /api/admin/users/{id}           # Update user
DELETE /api/admin/users/{id}           # Delete user
GET    /api/admin/audit-logs           # Get audit logs
GET    /api/admin/analytics/dashboard  # Get system analytics
```

---

## Development Workflow

### Creating a Feature

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/appointment-reminders
   ```

2. **Implement Feature**
   - Write unit tests first (TDD)
   - Implement functionality
   - Ensure all tests pass

3. **Code Quality Checks**
   ```bash
   dotnet format
   dotnet test
   ```

4. **Create Pull Request**
   - Link to relevant issues
   - Describe changes and rationale
   - Request review from team

5. **Merge to Main**
   - All checks pass
   - Code review approved
   - Branch automatically deleted

---

## Environment Variables

Create `.env` file in root directory:

```env
# Database
DB_HOST=localhost
DB_PORT=1433
DB_NAME=ClinicBookingSystemDb
DB_USER=sa
DB_PASSWORD=YourPassword

# API
API_PORT=5000
API_JWT_SECRET=your-super-secret-jwt-key-min-32-chars

# Email Configuration
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=your-email@gmail.com
SMTP_PASSWORD=your-app-password

# Frontend
FRONTEND_URL=http://localhost:3000
API_URL=http://localhost:5000
```

---

## License

This project is licensed under the MIT License - see [LICENSE](LICENSE) file for details.

---

## Support & Contribution

- **Issues**: Report bugs via GitHub Issues
- **Discussions**: Join GitHub Discussions for feature requests
- **Contributing**: See [CONTRIBUTING.md](CONTRIBUTING.md)
- **Code of Conduct**: See [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md)

---

## Project Status

- ✅ Phase 1: Requirements & Architecture (Complete)
- 🔄 Phase 2: Core Implementation (In Progress)
- ⏳ Phase 3: Testing & QA (Planned)
- ⏳ Phase 4: Deployment & DevOps (Planned)

---

**Last Updated**: May 23, 2026  
**Version**: 1.0.0-beta  
**Maintainers**: @swdmajozi
