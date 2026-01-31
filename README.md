# City Bus Audio Scheduler Web Application

A professional, production-ready web application for managing and scheduling audio announcements in municipal buses. Built with ASP.NET Core, React frontend, and MySQL database.

## 📋 Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Installation & Setup](#installation--setup)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Frontend Pages](#frontend-pages)
- [Usage Examples](#usage-examples)
- [Security](#security)
- [Performance Optimization](#performance-optimization)
- [Troubleshooting](#troubleshooting)
- [Deployment](#deployment)
- [Future Enhancements](#future-enhancements)

## Project Overview

The City Bus Audio Scheduler is a comprehensive management system designed for municipal transportation authorities to:

- Schedule audio announcements on buses (route information, emergency alerts, festival messages, advertisements)
- Manage a fleet of buses and routes
- Broadcast emergency messages with highest priority
- Track announcements and maintain logs
- Provide admin control over all audio content

## Features

### ✅ Core Features

#### Authentication & Security
- JWT-based authentication
- Role-based access control (Admin)
- Secure password hashing with PBKDF2
- Token expiration and refresh mechanisms

#### Audio Management
- Upload audio files (MP3, WAV)
- Organize by category (Route, Emergency, Festival, Advertisement)
- Set active/inactive status
- Track file metadata (duration, size, creation date)
- Preview functionality

#### Schedule Management
- Create schedules with audio + bus + date/time
- Support repeat patterns (Daily, Weekly, Monthly, None)
- Enable/disable schedules
- Automatic validation
- Conflict detection

#### Bus & Route Management
- Add/Edit/Delete buses
- Manage routes with start point, end point, distance
- Assign buses to routes
- Track bus status (Active, Inactive, Maintenance)

#### Emergency Announcement
- One-click broadcast to all buses
- Route-specific broadcasts
- Highest priority override
- Real-time status tracking
- Complete broadcast history

#### Dashboard
- Real-time statistics
- Total audio files count
- Active schedules display
- Upcoming announcements (next 7 days)
- Bus fleet overview
- Recent activity log

## Technology Stack

### Frontend
- **HTML5** - Semantic markup
- **CSS3** - Modern styling with Tailwind CSS
- **JavaScript (Vanilla)** - No framework dependencies
- **Tailwind CSS** - Utility-first CSS framework
- **Fetch API** - HTTP requests

### Backend
- **ASP.NET Core 6.0** - Modern framework
- **C#** - Server-side logic
- **Entity Framework Core** - ORM
- **JWT** - Authentication
- **MySQL** - Relational database

### Tools & Libraries
- Visual Studio 2022 (recommended)
- MySQL Server 8.0+
- Postman (for API testing)
- Git (version control)

## Project Structure

```
Citybus/
├── frontend/                          # Frontend files
│   ├── index.html                     # Login page
│   ├── dashboard.html                 # Main dashboard
│   ├── audio-management.html          # Audio file management
│   ├── schedule-management.html       # Schedule creation/management
│   ├── bus-management.html            # Bus & route management
│   ├── emergency.html                 # Emergency broadcast panel
│   └── assets/
│       ├── css/                       # Stylesheets (Tailwind)
│       └── js/
│           ├── config.js              # Configuration & constants
│           ├── api.js                 # API client methods
│           ├── auth.js                # Authentication logic
│           ├── dashboard.js           # Dashboard functionality
│           ├── audio-management.js    # Audio management logic
│           ├── schedule-management.js # Schedule management logic
│           ├── bus-management.js      # Bus management logic
│           └── emergency.js           # Emergency broadcast logic
│
├── backend/
│   └── CityBusAPI/                    # ASP.NET Core API
│       ├── Program.cs                 # Application startup
│       ├── appsettings.json           # Configuration
│       ├── appsettings.Development.json
│       ├── CityBusAPI.csproj          # Project file
│       ├── Controllers/
│       │   └── Controllers.cs         # All API controllers
│       ├── Services/
│       │   ├── TokenService.cs        # JWT token generation
│       │   ├── PasswordService.cs     # Password hashing
│       │   └── AudioService.cs        # Audio file handling
│       ├── Repositories/
│       │   └── Repositories.cs        # Data access layer
│       ├── Models/
│       │   └── Models.cs              # Entity models
│       ├── Data/
│       │   └── CityBusDbContext.cs    # EF Core context
│       ├── DTOs/
│       │   └── DTOs.cs                # Data transfer objects
│       └── Middleware/                # Custom middleware
│
└── database/
    ├── CityBusAudioDB_Schema.sql      # Database schema & seed data
    └── DataManagement_Queries.sql     # Maintenance & reporting queries
```

## Installation & Setup

### Prerequisites

1. **.NET SDK 6.0 or higher**
   - Download from: https://dotnet.microsoft.com/download/dotnet/6.0
   - Verify: `dotnet --version`

2. **MySQL Server 8.0+**
   - Download from: https://dev.mysql.com/downloads/mysql/
   - Verify: `mysql --version`

3. **Visual Studio 2022** (recommended) or VS Code
   - Download from: https://visualstudio.microsoft.com/

4. **Git**
   - Download from: https://git-scm.com/

### Step 1: Clone/Setup Project

```bash
# Navigate to project directory
cd c:\Users\shind\Citybus

# Create necessary directories (already done)
# Verify structure exists
dir /s
```

### Step 2: Frontend Setup

No build process needed! The frontend uses vanilla JavaScript and Tailwind CSS via CDN.

```bash
# The frontend is ready to use
# Open frontend/index.html in a browser
# Or run a local HTTP server:

# Using Python 3
cd frontend
python -m http.server 8000

# Or using Node.js http-server
npx http-server frontend -p 8000
```

### Step 3: Database Setup

```bash
# Connect to MySQL
mysql -u root -p

# Create database and run schema
mysql -u root -p < database/CityBusAudioDB_Schema.sql

# Or manually execute the SQL script:
# 1. Open MySQL Workbench or CLI
# 2. Copy contents of CityBusAudioDB_Schema.sql
# 3. Execute in your MySQL client
```

### Step 4: Backend Setup

```bash
# Navigate to backend directory
cd backend/CityBusAPI

# Restore dependencies
dotnet restore

# Update database connection string (if needed)
# Edit appsettings.json:
# "DefaultConnection": "Server=localhost;Database=CityBusAudioDB;User Id=root;Password=your_password;"

# Apply migrations (optional - first run will auto-migrate)
dotnet ef database update

# Build the project
dotnet build

# Run the backend
dotnet run
```

The backend will start at `http://localhost:5000`

## Database Setup

### MySQL Connection Details

```
Host: localhost
Port: 3306
Database: CityBusAudioDB
Username: root
Password: (your MySQL password)
```

### Database Tables

1. **Users** - Administrator accounts
2. **Routes** - Bus routes with start/end points
3. **Buses** - Bus fleet with capacity and status
4. **AudioFiles** - Uploaded audio announcements
5. **Schedules** - Scheduled audio on buses
6. **EmergencyLogs** - Emergency broadcast history

### Sample Data

The schema includes pre-populated sample data:
- 1 Admin user (admin@citybus.gov / Password@123)
- 4 Sample routes
- 6 Sample buses
- 8 Sample audio files
- 5 Sample schedules

## Running the Application

### Terminal 1 - Backend API

```bash
cd backend/CityBusAPI
dotnet run
# Backend runs at http://localhost:5000
# Swagger UI at http://localhost:5000/swagger
```

### Terminal 2 - Frontend

```bash
cd frontend
# Option 1: Using Python
python -m http.server 8000

# Option 2: Using Node.js
npx http-server . -p 8000

# Access at http://localhost:8000
```

### Login Credentials

```
Email: admin@citybus.gov
Password: Password@123
```

## API Documentation

### Base URL
```
http://localhost:5000/api
```

### Authentication Endpoints

#### Login
```http
POST /auth/login
Content-Type: application/json

{
    "email": "admin@citybus.gov",
    "password": "Password@123"
}

Response (200 OK):
{
    "success": true,
    "message": "Login successful",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIs...",
        "user": {
            "id": 1,
            "name": "Admin User",
            "email": "admin@citybus.gov",
            "role": "Admin"
        }
    }
}
```

### Audio File Endpoints

#### Get All Audio Files
```http
GET /audio
Authorization: Bearer {token}

Response (200 OK):
{
    "success": true,
    "data": [
        {
            "id": 1,
            "name": "Route 5 Announcement",
            "category": "Route",
            "durationSeconds": 15,
            "description": "Standard route announcement",
            "isActive": true,
            "createdAt": "2026-01-30T10:00:00"
        }
    ]
}
```

#### Upload Audio File
```http
POST /audio/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

file: <binary audio file (MP3/WAV)>

Response (201 Created):
{
    "success": true,
    "data": {
        "id": 9,
        "name": "New Audio",
        "category": "Route",
        "durationSeconds": 0,
        "description": "",
        "isActive": true,
        "createdAt": "2026-01-30T15:30:00"
    }
}
```

#### Delete Audio File
```http
DELETE /audio/{id}
Authorization: Bearer {token}

Response (200 OK):
{
    "success": true,
    "message": "Audio file deleted successfully"
}
```

### Schedule Endpoints

#### Create Schedule
```http
POST /schedules
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 1,
    "busId": 1,
    "startDateTime": "2026-02-01T08:00:00",
    "repeatPattern": "Daily",
    "isActive": true
}

Response (201 Created):
{
    "success": true,
    "data": {
        "id": 6,
        "audioName": "Route 5 Announcement",
        "busNumber": "B001",
        "startDateTime": "2026-02-01T08:00:00",
        "repeatPattern": "Daily",
        "isActive": true
    }
}
```

#### Get All Schedules
```http
GET /schedules
Authorization: Bearer {token}
```

#### Delete Schedule
```http
DELETE /schedules/{id}
Authorization: Bearer {token}
```

### Bus Endpoints

#### Get All Buses
```http
GET /buses
Authorization: Bearer {token}

Response (200 OK):
{
    "success": true,
    "data": [
        {
            "id": 1,
            "busNumber": "B001",
            "routeName": "Route 1",
            "status": "Active",
            "capacity": 45
        }
    ]
}
```

#### Create Bus
```http
POST /buses
Authorization: Bearer {token}
Content-Type: application/json

{
    "busNumber": "B007",
    "routeId": 1,
    "registrationNumber": "DL-01-AB-1240",
    "capacity": 45
}
```

### Route Endpoints

#### Get All Routes
```http
GET /routes
Authorization: Bearer {token}
```

#### Create Route
```http
POST /routes
Authorization: Bearer {token}
Content-Type: application/json

{
    "name": "Route 10",
    "startPoint": "Market Square",
    "endPoint": "Industrial Area",
    "distanceKm": 18.5
}
```

### Dashboard Endpoints

#### Get Dashboard Statistics
```http
GET /dashboard/stats
Authorization: Bearer {token}

Response (200 OK):
{
    "success": true,
    "data": {
        "totalAudioFiles": 8,
        "activeSchedules": 5,
        "totalBuses": 6,
        "upcomingAnnouncements": 3
    }
}
```

### Emergency Endpoints

#### Broadcast Emergency Alert
```http
POST /emergency/broadcast
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 2,
    "busTarget": "all",
    "routeId": null,
    "priority": "Critical",
    "message": "Traffic alert on Route 1"
}

Response (200 OK):
{
    "success": true,
    "message": "Emergency broadcast sent successfully"
}
```

#### Get Broadcast History
```http
GET /emergency/history
Authorization: Bearer {token}
```

## Frontend Pages

### 1. Login Page (`index.html`)
- Clean, modern login interface
- Email and password validation
- Remember me checkbox
- Demo credentials displayed
- Error message display
- Responsive design

**Features:**
- Form validation
- Token storage
- Redirect to dashboard on success
- Error handling

### 2. Dashboard (`dashboard.html`)
- Key metrics cards (total audio, active schedules, buses, upcoming announcements)
- Activity timeline
- Charts placeholder
- Quick navigation

**Statistics Displayed:**
- Total Audio Files
- Active Schedules
- Total Buses
- Upcoming Announcements

### 3. Audio Management (`audio-management.html`)
- Upload audio files (drag & drop)
- List all audio files
- Filter by category and status
- Search functionality
- Play/Delete buttons
- Upload progress indicator

**Supported File Types:**
- MP3 (MPEG-3)
- WAV (Waveform Audio)

**File Limits:**
- Max 50MB per file

### 4. Schedule Management (`schedule-management.html`)
- Create new schedules
- Assign audio to buses
- Set date and time
- Configure repeat patterns (Daily, Weekly, Monthly)
- Enable/Disable schedules
- View all schedules
- Edit and delete functionality

### 5. Bus & Route Management (`bus-management.html`)
- View all buses with status
- Add new buses
- Manage routes
- Track bus capacity
- Filter by status
- Assign routes to buses

### 6. Emergency Announcement (`emergency.html`)
- One-click broadcast button
- Select emergency audio files
- Target buses (all or specific route)
- Priority levels
- Broadcast history
- Confirmation dialog

## Usage Examples

### Example 1: Creating a Schedule

1. Go to **Schedules** page
2. Click **+ Create Schedule**
3. Select audio file: "Route 5 Announcement"
4. Select bus: "B001"
5. Set date: 2026-02-01
6. Set time: 08:00 AM
7. Repeat: Daily
8. Click **Create Schedule**

### Example 2: Uploading Audio

1. Go to **Audio Files** page
2. Click **+ Upload New**
3. Drag and drop MP3 file or click to browse
4. File uploads automatically
5. Audio appears in the list

### Example 3: Emergency Broadcast

1. Go to **Emergency Alert** page
2. Select emergency audio file
3. Choose target (All Buses or Route)
4. Set priority level
5. Click **BROADCAST NOW**
6. Confirm in dialog
7. Message appears in broadcast history

## Security

### Authentication
- JWT tokens with 8-hour expiration
- Secure password hashing using PBKDF2
- Token stored in localStorage
- Automatic logout on token expiration

### API Security
- All endpoints require Bearer token
- CORS enabled for frontend origins only
- Input validation on all endpoints
- SQL injection prevention via EF Core
- XSS protection

### Database Security
- Foreign key constraints
- Unique constraints on email and bus number
- Index optimization
- Proper charset (UTF-8) for international characters

### File Upload Security
- File type validation (MP3/WAV only)
- File size limits (50MB max)
- Secure file storage path
- Unique file naming to prevent conflicts

## Performance Optimization

### Database
- Indexes on frequently queried columns
- Foreign key relationships optimized
- Query optimization with EF Core
- Connection pooling

### Frontend
- Minimal JavaScript (vanilla JS, no frameworks)
- CSS via Tailwind CDN
- Responsive design
- Lazy loading for images
- Toast notifications instead of popups

### Backend
- Dependency injection
- Repository pattern for data access
- Async/await for non-blocking operations
- Proper error handling
- Logging for debugging

## Troubleshooting

### Frontend Issues

**Problem: Frontend won't load**
```
Solution: 
1. Check if backend is running (http://localhost:5000)
2. Verify API_BASE_URL in config.js
3. Check browser console for errors (F12)
4. Try clearing browser cache
```

**Problem: Login fails**
```
Solution:
1. Verify MySQL database is running
2. Check appsettings.json connection string
3. Ensure database has seed data
4. Check JWT settings are configured
```

**Problem: File upload fails**
```
Solution:
1. Check file size (max 50MB)
2. Verify file format (MP3 or WAV only)
3. Check upload path exists in backend
4. Check file permissions
```

### Backend Issues

**Problem: "No database provider"**
```
Solution: 
dotnet add package Pomelo.EntityFrameworkCore.MySql
```

**Problem: Connection timeout**
```
Solution:
1. Verify MySQL is running
2. Check connection string in appsettings.json
3. Verify database exists
4. Check MySQL user permissions
```

**Problem: Migration fails**
```
Solution:
dotnet ef database drop --force
dotnet ef database update
```

## Deployment

### Cloud Deployment Options

#### Azure
1. Create Azure SQL Database
2. Create Azure App Service for .NET
3. Publish from Visual Studio
4. Configure environment variables
5. Enable HTTPS

#### AWS
1. Use RDS for MySQL
2. Use Elastic Beanstalk for .NET
3. Configure security groups
4. Set up CloudFront for frontend

#### Docker Deployment
```dockerfile
# Create Dockerfile for backend
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY . .
EXPOSE 5000
CMD ["dotnet", "CityBusAPI.dll"]
```

### Environment Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "production_connection_string"
  },
  "Jwt": {
    "Key": "your-production-secret-key-min-32-chars",
    "ExpirationMinutes": 480
  },
  "Cors": {
    "AllowedOrigins": ["https://yourdomain.com"]
  }
}
```

## Future Enhancements

### Short Term (Next Sprint)
- [ ] Audio waveform preview
- [ ] Dark mode toggle
- [ ] Real-time notifications
- [ ] Bulk audio upload
- [ ] Schedule templates
- [ ] User activity logs

### Medium Term (Next Quarter)
- [ ] Mobile app (React Native)
- [ ] Advanced scheduling (cron-like patterns)
- [ ] Analytics dashboard
- [ ] Multi-language support
- [ ] Audio compression
- [ ] Integration with bus GPS tracking

### Long Term (Next Year)
- [ ] Machine learning for optimal scheduling
- [ ] Real-time bus fleet monitoring
- [ ] Integration with smart speakers
- [ ] Blockchain for audit trails
- [ ] Cloud storage integration (AWS S3)
- [ ] Advanced reporting and BI dashboards

## Support & Documentation

### Additional Resources
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Tailwind CSS](https://tailwindcss.com/)
- [MySQL Documentation](https://dev.mysql.com/doc/)

### Contact
For issues, feature requests, or support:
- Email: support@citybus.gov
- Documentation: https://docs.citybus.gov

---

**Version:** 1.0.0  
**Last Updated:** January 30, 2026  
**License:** Municipal Corporation License
