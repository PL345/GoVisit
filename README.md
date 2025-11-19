# GoVisit - Government Appointment Scheduler API

A modern REST API for managing government office appointments, built with .NET and deployed on AWS Lambda.

## Overview

GoVisit streamlines the appointment booking process for government offices, allowing citizens to schedule appointments for various services while considering office availability, working hours, and existing bookings.

## Features

- **Appointment Management** - Create, view, update, and cancel appointments
- **Available Slots Search** - Find open time slots by service and date range
- **Office Management** - Manage government offices and their services
- **Service Catalog** - Browse services by category
- **Schedule Management** - Configure office hours, breaks, and holidays
- **Citizen Profiles** - Store and manage citizen information

## Tech Stack

- **Backend:** C# / .NET 8, ASP.NET Core Web API
- **Database:** MongoDB Atlas (Cloud)
- **Architecture:** Clean Architecture (4 layers)
- **Deployment:** AWS Lambda + API Gateway (Serverless)
- **Documentation:** Swagger/OpenAPI

## Architecture

```
GoVisit.Core          → Models & Interfaces
GoVisit.Application   → Business Logic & Services
GoVisit.Infrastructure → Data Access & Repositories
GoVisitApi            → REST API Controllers
```

## API Endpoints

### Appointments
- `GET /api/Appointments` - Get all appointments
- `GET /api/Appointments/{id}` - Get appointment by ID
- `GET /api/Appointments/office/{officeId}` - Get appointments by office
- `GET /api/Appointments/date/{date}` - Get appointments by date
- `POST /api/Appointments/available` - Search available time slots
- `POST /api/Appointments` - Create new appointment
- `PUT /api/Appointments/{id}/status` - Update appointment status

### Offices
- `GET /api/Offices` - Get all offices
- `GET /api/Offices/by-service/{serviceType}` - Get offices by service

### Services
- `GET /api/Services` - Get all services
- `GET /api/Services/category/{category}` - Get services by category

### Schedules
- `GET /api/Schedules` - Get all schedules
- `GET /api/Schedules/office/{officeId}/service/{serviceType}` - Get specific schedule

## Quick Start

### Prerequisites
- .NET 8 SDK
- MongoDB connection string
- AWS CLI (for deployment)

### Local Development

1. Clone the repository
```bash
git clone https://github.com/yourusername/govisit.git
cd govisit
```

2. Configure MongoDB connection
```bash
# Update appsettings.Development.json
{
  "MongoDB": {
    "ConnectionString": "your-mongodb-connection-string",
    "DatabaseName": "GoVisitDB"
  }
}
```

3. Restore dependencies
```bash
dotnet restore
```

4. Run the API
```bash
cd GoVisitApi
dotnet run
```

5. Access Swagger UI
```
https://localhost:5001/swagger
```

### Seed Sample Data

```bash
POST /api/Seed/seed-data
```

## Example Usage

### Search for Available Appointments

```bash
POST /api/Appointments/available
Content-Type: application/json

{
  "serviceType": "PASSPORT",
  "startDate": "2025-11-20T00:00:00Z",
  "endDate": "2025-11-27T00:00:00Z",
  "officeId": "TEL_AVIV_CENTER"
}
```

### Create an Appointment

```bash
POST /api/Appointments
Content-Type: application/json

{
  "citizenId": "123456789",
  "citizenName": "John Doe",
  "citizenPhone": "050-1234567",
  "officeId": "TEL_AVIV_CENTER",
  "serviceType": "PASSPORT",
  "appointmentDate": "2025-11-25T00:00:00Z",
  "appointmentTime": "09:30:00"
}
```

## Deployment

### AWS Lambda Deployment

1. Install AWS Lambda Tools
```bash
dotnet tool install -g Amazon.Lambda.Tools
```

2. Deploy to AWS
```bash
cd GoVisitApi
dotnet lambda deploy-serverless
```

3. Configure environment variables in AWS Console
- `MONGODB_CONNECTION_STRING`

## Documentation

- [API Specification](docs/API_SPECIFICATION.md) - Complete API documentation (Hebrew)
- [Architecture](docs/ARCHITECTURE.md) - Technical architecture details (Hebrew)

## Project Structure

```
GoVisit/
├── GoVisit.Core/              # Domain models & interfaces
│   ├── Models/
│   └── Interfaces/
├── GoVisit.Application/       # Business logic
│   └── Services/
├── GoVisit.Infrastructure/    # Data access
│   └── Repositories/
├── GoVisitApi/               # API layer
│   ├── Controllers/
│   └── Middleware/
└── docs/                     # Documentation
```

## Key Features

### Smart Slot Generation
- Efficient algorithm for finding available time slots
- Considers office schedules, breaks, and holidays
- Filters out already booked appointments
- Optimized with Dictionary and HashSet for O(1) lookups

### Validation
- Office availability checks
- Service availability at specific offices
- Schedule validation (working hours, breaks)
- Duplicate booking prevention

### Performance
- Single database query per collection
- In-memory filtering with LINQ
- Asynchronous operations throughout
- Optimized for serverless cold starts

## Services Available

- Passport Issuance (הנפקת דרכון)
- ID Card Issuance (הנפקת תעודת זהות)
- Driving License (רישיון נהיגה)
- Business License (רישיון עסק)
- Birth Certificate (תעודת לידה)
- Marriage Certificate (תעודת נישואין)

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License.

## Contact

For questions or support, please open an issue on GitHub.

---

**Version:** 1.0  
**Last Updated:** November 2025
