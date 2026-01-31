# 🚀 How to Run the City Bus Audio Scheduler

## Status: Backend Built Successfully ✅

The ASP.NET Core backend compiled without errors! Now let's set up the complete system.

---

## Prerequisites Check

### ✅ Completed
- **.NET SDK 8.0**: Installed (supports .NET 6.0 projects)
- **Backend**: Compiled successfully

### ⚠️ Still Need
- **MySQL Server**: Required for database
- **Simple HTTP Server**: For frontend (Python or Node.js)

---

## Step 1: Set Up MySQL Database (If Not Installed)

### Option A: Using MySQL Command Line (If MySQL is already installed)

```bash
mysql -u root -p < C:\Users\shind\Citybus\database\CityBusAudioDB_Schema.sql
```

### Option B: Manual Setup

1. Download MySQL Community Server from: https://dev.mysql.com/downloads/mysql/
2. Install with default settings
3. Create database and tables manually:

```sql
CREATE DATABASE CityBusAudioDB CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE CityBusAudioDB;
-- Then run the SQL schema file
```

---

## Step 2: Start the Backend (ASP.NET Core API)

### Terminal 1: Backend Server

```bash
cd C:\Users\shind\Citybus\backend\CityBusAPI
dotnet run
```

**Expected Output:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

⚠️ **Note**: If database connection fails:
1. Update connection string in `appsettings.json`
2. Default: `Server=localhost;Database=CityBusAudioDB;User Id=root;Password=your_mysql_password;`

---

## Step 3: Start the Frontend (HTML/JavaScript Server)

### Terminal 2: Frontend Server

Choose one option:

**Option A: Python (Easiest)**
```bash
cd C:\Users\shind\Citybus\frontend
python -m http.server 8000
```

**Option B: Node.js (if installed)**
```bash
cd C:\Users\shind\Citybus\frontend
npx http-server -p 8000
```

**Expected Output:**
```
Serving HTTP on 0.0.0.0 port 8000 (http://localhost:8000/) ...
```

---

## Step 4: Access the Application

### 🌐 Open in Browser

Open your web browser and go to:
```
http://localhost:8000
```

You should see the **City Bus Audio Scheduler Login Page** ✅

---

## Step 5: Login with Default Credentials

| Field | Value |
|-------|-------|
| Email | `admin@citybus.gov` |
| Password | `Password@123` |

Click **Login** to access the dashboard.

---

## Application URLs

| Component | URL | Port |
|-----------|-----|------|
| Frontend (Web App) | http://localhost:8000 | 8000 |
| Backend (API) | http://localhost:5000 | 5000 |
| MySQL Database | localhost | 3306 |

---

## API Endpoints (If Testing Directly)

### Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@citybus.gov","password":"Password@123"}'
```

### Get All Audio Files
```bash
curl -X GET http://localhost:5000/api/audio \
  -H "Authorization: Bearer YOUR_TOKEN"
```

---

## Troubleshooting

### Port Already in Use
If port 8000 or 5000 is already in use:

```bash
# Find process using port 8000
netstat -ano | findstr :8000

# Use different port
python -m http.server 9000
```

### Database Connection Error
1. Verify MySQL is running
2. Check connection string in `appsettings.json`
3. Ensure database `CityBusAudioDB` exists

### Frontend Can't Reach Backend
1. Verify backend is running on http://localhost:5000
2. Check browser console for CORS errors
3. Update API_BASE_URL in `frontend/assets/js/config.js` if needed

### "Module Not Found" Errors
Make sure you're in the correct directory before running commands.

---

## Features to Test

Once logged in, you can:

✅ View Dashboard with statistics  
✅ Upload audio files (MP3/WAV)  
✅ Create schedules for announcements  
✅ Manage buses and routes  
✅ Send emergency broadcasts  
✅ View audit logs and history  

---

## Performance Notes

- **First Load**: May take a few seconds for API startup
- **Database Seeding**: Automatic on first run (admin user, sample routes, buses, audio files)
- **Token Expiration**: 8 hours (configurable in appsettings.json)

---

## Next Steps

1. ✅ Run the application
2. Test all features in the UI
3. Review logs in both terminal windows
4. Deploy to production (see README.md for cloud deployment)

---

**Questions?** Check the complete documentation:
- [README.md](README.md) - Full project guide
- [API_REFERENCE.md](API_REFERENCE.md) - All endpoints
- [SETUP.md](SETUP.md) - Detailed setup guide

---

**Ready to start?** Run the commands above in separate terminal windows! 🎉
