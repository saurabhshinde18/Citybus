# City Bus Audio Scheduler - Complete Setup Instructions

## ✅ Project Completion Checklist

### Completed Components

#### Frontend ✓
- [x] Login page with authentication
- [x] Dashboard with statistics
- [x] Audio management page
- [x] Schedule management page
- [x] Bus & route management page
- [x] Emergency announcement panel
- [x] Responsive design (Tailwind CSS)
- [x] JavaScript functionality
- [x] API integration
- [x] Error handling & notifications

#### Backend ✓
- [x] ASP.NET Core 6.0 API
- [x] JWT authentication
- [x] Entity Framework Core ORM
- [x] All CRUD controllers
- [x] Service layer
- [x] Repository pattern
- [x] Dependency injection
- [x] Error handling
- [x] CORS configuration
- [x] Logging

#### Database ✓
- [x] MySQL schema design
- [x] Normalized tables
- [x] Foreign key relationships
- [x] Indexes for performance
- [x] Seed data
- [x] Query examples
- [x] Backup scripts

#### Documentation ✓
- [x] README with complete guide
- [x] Quick start guide
- [x] API reference
- [x] Setup instructions
- [x] Troubleshooting guide
- [x] Deployment guide

---

## Installation Steps

### Part 1: Prerequisites (15 minutes)

#### 1.1 Install .NET SDK
```bash
# Download .NET 6.0 SDK
# https://dotnet.microsoft.com/download/dotnet/6.0

# Verify installation
dotnet --version
# Should show: 6.0.x or higher
```

#### 1.2 Install MySQL Server
```bash
# Download MySQL Community Server
# https://dev.mysql.com/downloads/mysql/

# Verify installation
mysql --version
# Should show: mysql  Ver 8.0.x or higher

# Windows: Add MySQL to PATH if needed
# Set password during installation
```

#### 1.3 Install Git (Optional but Recommended)
```bash
# Download from https://git-scm.com/
git --version
```

---

### Part 2: Database Setup (10 minutes)

#### 2.1 Create Database
```bash
# Open Command Prompt or Terminal
cd c:\Users\shind\Citybus\database

# Login to MySQL
mysql -u root -p
# Enter your MySQL password

# Once in MySQL shell:
source CityBusAudioDB_Schema.sql;
EXIT;

# Verify (optional)
mysql -u root -p CityBusAudioDB -e "SHOW TABLES;"
```

#### 2.2 Verify Sample Data
```bash
mysql -u root -p CityBusAudioDB
SELECT COUNT(*) as UserCount FROM Users;
SELECT COUNT(*) as BusCount FROM Buses;
SELECT COUNT(*) as AudioCount FROM AudioFiles;
EXIT;
```

Expected output:
```
UserCount: 1
BusCount: 6
AudioCount: 8
```

---

### Part 3: Backend Setup (10 minutes)

#### 3.1 Navigate to Backend Directory
```bash
cd c:\Users\shind\Citybus\backend\CityBusAPI
```

#### 3.2 Restore NuGet Packages
```bash
dotnet restore
# Wait for download to complete
```

#### 3.3 Update Connection String (If Needed)

Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CityBusAudioDB;User Id=root;Password=YOUR_MYSQL_PASSWORD;"
  },
  ...
}
```

#### 3.4 Build Project
```bash
dotnet build
# Should complete without errors
```

#### 3.5 Run Backend
```bash
dotnet run
# Should display:
# info: Microsoft.Hosting.Lifetime[0]
#       Now listening on: http://localhost:5000
#       Now listening on: https://localhost:5001
```

#### 3.6 Verify Backend
Open browser and visit:
```
http://localhost:5000/swagger
```

You should see Swagger UI with all API endpoints.

---

### Part 4: Frontend Setup (5 minutes)

#### 4.1 Navigate to Frontend Directory
```bash
cd c:\Users\shind\Citybus\frontend
```

#### 4.2 Start HTTP Server

**Option A: Using Python 3**
```bash
python -m http.server 8000
# Navigate to http://localhost:8000
```

**Option B: Using Node.js (if installed)**
```bash
npx http-server . -p 8000
# Navigate to http://localhost:8000
```

**Option C: Using PowerShell (Windows)**
```powershell
python -m http.server 8000
```

#### 4.3 Access Application
Open browser and visit:
```
http://localhost:8000
```

---

### Part 5: First Login (2 minutes)

#### 5.1 Login Screen
You should see the login page with:
- Email field (pre-filled: admin@citybus.gov)
- Password field (pre-filled: Password@123)
- Demo credentials section

#### 5.2 Click "Sign In"
- Backend should authenticate
- You should be redirected to dashboard
- Should see stats cards with data

#### 5.3 Explore Dashboard
- View statistics
- Check sidebar navigation
- Click through different pages

---

## Post-Installation Verification

### Checklist

```bash
✓ MySQL running and database exists
✓ Backend API running on port 5000
✓ Swagger UI accessible at http://localhost:5000/swagger
✓ Frontend running on port 8000
✓ Login successful with demo credentials
✓ Dashboard shows statistics
✓ Sidebar navigation works
```

### Quick API Test

```bash
# Open PowerShell or Command Prompt
# Test backend connectivity
curl http://localhost:5000/swagger

# Should return HTML content (Swagger UI)
```

---

## Common Ports Reference

| Service | Port | URL |
|---------|------|-----|
| Frontend | 8000 | http://localhost:8000 |
| Backend API | 5000 | http://localhost:5000 |
| MySQL | 3306 | localhost:3306 |
| Swagger UI | 5000 | http://localhost:5000/swagger |

---

## File Structure After Setup

```
c:\Users\shind\Citybus\
├── frontend/
│   ├── index.html
│   ├── dashboard.html
│   ├── audio-management.html
│   ├── schedule-management.html
│   ├── bus-management.html
│   ├── emergency.html
│   └── assets/
│       ├── css/
│       └── js/
│
├── backend/
│   └── CityBusAPI/
│       ├── Program.cs
│       ├── appsettings.json
│       ├── CityBusAPI.csproj
│       ├── Controllers/
│       ├── Services/
│       ├── Repositories/
│       ├── Models/
│       ├── Data/
│       └── DTOs/
│
├── database/
│   ├── CityBusAudioDB_Schema.sql
│   └── DataManagement_Queries.sql
│
├── README.md
├── QUICKSTART.md
├── API_REFERENCE.md
└── SETUP.md (this file)
```

---

## Testing the Application

### Test 1: Login
```
URL: http://localhost:8000
Email: admin@citybus.gov
Password: Password@123
Expected: Redirect to dashboard
```

### Test 2: View Audio Files
```
1. Click "Audio Files" in sidebar
2. Should see pre-loaded audio files
3. Try search/filter functionality
```

### Test 3: Create Schedule
```
1. Click "Schedules" in sidebar
2. Click "+ Create Schedule"
3. Select audio file, bus, date, time
4. Click "Create Schedule"
5. Should see new schedule in list
```

### Test 4: Emergency Broadcast
```
1. Click "Emergency Alert" in sidebar
2. Select emergency audio file
3. Choose target buses (All Buses or Route)
4. Click "BROADCAST NOW"
5. Should see confirmation and broadcast history
```

### Test 5: API Test
```bash
# Get Auth Token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@citybus.gov","password":"Password@123"}'

# Copy token from response, then:

# Get All Audio Files
curl -X GET http://localhost:5000/api/audio \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Get Buses
curl -X GET http://localhost:5000/api/buses \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"

# Get Dashboard Stats
curl -X GET http://localhost:5000/api/dashboard/stats \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

---

## Troubleshooting

### Backend Won't Start

**Error: "Could not connect to database"**
```
Solution:
1. Verify MySQL is running: mysql -u root -p -e "SELECT 1;"
2. Check appsettings.json connection string
3. Verify database exists: mysql -u root -p -e "SHOW DATABASES;"
```

**Error: "Port 5000 already in use"**
```
Windows:
netstat -ano | findstr :5000
taskkill /PID <PID> /F

Linux:
lsof -i :5000
kill -9 <PID>
```

### Frontend Won't Load

**Error: "Cannot reach localhost:5000"**
```
Solution:
1. Verify backend is running
2. Check if port 5000 is accessible
3. Check browser console (F12) for CORS errors
4. Verify CORS is enabled in Program.cs
```

### Login Fails

**Error: "Invalid email or password"**
```
Solution:
1. Verify admin user exists:
   mysql -u root -p CityBusAudioDB
   SELECT * FROM Users;
2. Ensure database was seeded correctly
3. Check backend logs for errors
```

---

## Next Steps

### Immediate (Day 1)
- [ ] Complete installation
- [ ] Verify all components running
- [ ] Test login and navigation
- [ ] Upload test audio file

### Short Term (Week 1)
- [ ] Create sample schedules
- [ ] Add buses and routes
- [ ] Test emergency broadcast
- [ ] Review API documentation

### Medium Term (Month 1)
- [ ] Set up user management
- [ ] Configure production database
- [ ] Performance testing
- [ ] Security audit

### Long Term (Quarter 1)
- [ ] Deploy to cloud
- [ ] Set up monitoring
- [ ] Plan enhancements
- [ ] User training

---

## Support Resources

### Documentation Files
- `README.md` - Complete project documentation
- `QUICKSTART.md` - 5-minute quick start
- `API_REFERENCE.md` - Complete API reference
- `SETUP.md` - This file

### Online Resources
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [MySQL Documentation](https://dev.mysql.com/doc/)
- [Tailwind CSS](https://tailwindcss.com/docs)

### Default Credentials
```
Email: admin@citybus.gov
Password: Password@123
Role: Administrator
```

---

## Configuration Files Summary

### appsettings.json
Located in `backend/CityBusAPI/`

Key settings:
- Database connection string
- JWT secret key
- CORS allowed origins
- File upload limits
- Logging levels

### config.js
Located in `frontend/assets/js/`

Key settings:
- API base URL (http://localhost:5000/api)
- Token storage key
- User storage key

---

## Performance Tips

### Backend
1. Indexes are already configured in database
2. Connection pooling is enabled
3. Async/await used throughout
4. Entity Framework lazy loading disabled

### Frontend
1. Vanilla JavaScript (no framework overhead)
2. Tailwind CSS via CDN
3. Minimal DOM manipulation
4. Toast notifications instead of modals

### Database
1. Run `OPTIMIZE TABLE` on large tables regularly
2. Monitor query performance
3. Back up database regularly
4. Archive old logs periodically

---

## Backup & Recovery

### Database Backup
```bash
# Backup entire database
mysqldump -u root -p CityBusAudioDB > backup.sql

# Restore from backup
mysql -u root -p CityBusAudioDB < backup.sql
```

### Project Backup
```bash
# Zip entire project
powershell Compress-Archive -Path "c:\Users\shind\Citybus" -DestinationPath "c:\Users\shind\Citybus_backup.zip"
```

---

## Security Reminders

1. **Change MySQL Password**: Don't use default in production
2. **Change JWT Secret**: Update in appsettings.json
3. **HTTPS**: Enable in production
4. **CORS Origins**: Restrict to your domain
5. **File Upload**: Validate file types and sizes
6. **Password Hashing**: Never store plain passwords
7. **Token Expiration**: Set appropriate timeout

---

**Installation Complete!** 🎉

You now have a fully functional City Bus Audio Scheduler application ready for testing and deployment.

For questions or issues, refer to the troubleshooting section or check the detailed documentation files.
