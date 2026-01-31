# Quick Start Guide - City Bus Audio Scheduler

## 5-Minute Quick Start

### Step 1: Install MySQL (If Not Already Installed)
```bash
# Download and install MySQL Server 8.0
# https://dev.mysql.com/downloads/mysql/

# Verify installation
mysql --version
```

### Step 2: Create Database
```bash
# Connect to MySQL
mysql -u root -p

# Run the schema script
mysql -u root -p < database/CityBusAudioDB_Schema.sql

# Verify (should see tables)
USE CityBusAudioDB;
SHOW TABLES;
EXIT;
```

### Step 3: Run Backend API
```bash
# Terminal 1
cd backend/CityBusAPI

# Restore packages
dotnet restore

# Update connection string if needed (edit appsettings.json)
# Default: "Server=localhost;Database=CityBusAudioDB;User Id=root;Password=;"

# Run
dotnet run

# Should see: "Application started. Press Ctrl+C to shut down."
# API runs at http://localhost:5000
```

### Step 4: Run Frontend
```bash
# Terminal 2
cd frontend

# Option A: Using Python
python -m http.server 8000

# Option B: Using Node.js
npx http-server . -p 8000

# Access at http://localhost:8000
```

### Step 5: Login
```
Email: admin@citybus.gov
Password: Password@123
```

## Testing API with Postman

### 1. Import Collection
Create new request with these steps:

### 2. Login Request
```
POST http://localhost:5000/api/auth/login
Content-Type: application/json

Body:
{
    "email": "admin@citybus.gov",
    "password": "Password@123"
}

Response: Copy the token from response
```

### 3. Get Audio Files
```
GET http://localhost:5000/api/audio
Authorization: Bearer {paste_token_here}

Response: List of all audio files
```

### 4. Get Buses
```
GET http://localhost:5000/api/buses
Authorization: Bearer {token}
```

### 5. Get Dashboard Stats
```
GET http://localhost:5000/api/dashboard/stats
Authorization: Bearer {token}
```

## Troubleshooting Quick Fixes

### "Cannot connect to database"
```bash
# Check MySQL is running
mysql -u root -p -e "SELECT 1;"

# Check connection string in appsettings.json
cat backend/CityBusAPI/appsettings.json
```

### "Port 5000 already in use"
```bash
# Kill process using port 5000
# Windows
netstat -ano | findstr :5000
taskkill /PID <PID> /F

# Linux/Mac
lsof -i :5000
kill -9 <PID>
```

### "Frontend can't reach API"
```bash
# Verify backend is running
curl http://localhost:5000/swagger

# Check CORS settings in Program.cs
# Ensure frontend origin is in AllowedOrigins
```

## File Locations Summary

| Component | Location | Default Port |
|-----------|----------|--------------|
| Frontend | `frontend/` | 8000 |
| Backend | `backend/CityBusAPI/` | 5000 |
| Database | MySQL Server | 3306 |
| SQL Scripts | `database/` | N/A |

## Next Steps

1. ✅ Explore the dashboard
2. ✅ Upload an audio file
3. ✅ Create a schedule
4. ✅ Add buses and routes
5. ✅ Test emergency broadcast
6. ✅ Check API documentation

## Key Features to Try

- **Dashboard**: View real-time statistics
- **Audio Upload**: Drag & drop MP3/WAV files (max 50MB)
- **Scheduling**: Create daily/weekly/monthly schedules
- **Bus Management**: Add and manage buses
- **Emergency**: One-click broadcast to all buses

## Default Credentials

```
Username: admin@citybus.gov
Password: Password@123
```

## API Endpoints Quick Reference

```
Authentication:
POST   /api/auth/login

Audio:
GET    /api/audio
POST   /api/audio/upload
DELETE /api/audio/{id}

Schedules:
GET    /api/schedules
POST   /api/schedules
DELETE /api/schedules/{id}

Buses:
GET    /api/buses
POST   /api/buses
DELETE /api/buses/{id}

Routes:
GET    /api/routes
POST   /api/routes
DELETE /api/routes/{id}

Dashboard:
GET    /api/dashboard/stats

Emergency:
POST   /api/emergency/broadcast
GET    /api/emergency/history
```

## Common Tasks

### Upload Audio File
1. Go to Audio Files → Click "Upload New"
2. Drag & drop or click to browse
3. File auto-uploads and appears in list

### Create Schedule
1. Go to Schedules → Click "Create Schedule"
2. Select audio, bus, date/time
3. Set repeat pattern
4. Click Create

### Send Emergency Alert
1. Go to Emergency Alert
2. Select emergency audio
3. Choose target buses
4. Click BROADCAST NOW

---

**Need Help?** Check the full README.md or review API documentation in Swagger UI at http://localhost:5000/swagger
