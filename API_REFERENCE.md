# City Bus Audio Scheduler - API Complete Reference

## Base URL
```
http://localhost:5000/api
```

## Authentication

All endpoints (except login) require JWT Bearer token in the Authorization header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 1. AUTHENTICATION ENDPOINTS

### 1.1 Login

**Request:**
```http
POST /auth/login
Content-Type: application/json

{
    "email": "admin@citybus.gov",
    "password": "Password@123"
}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "Login successful",
    "data": {
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDQwNjU2MDAsImV4cCI6MTY0NDA5MTYwMCwiaWF0IjoxNjQ0MDY1NjAwLCJpc3MiOiJDaXR5QnVzQVBJIiwiYXVkIjoiQ2l0eUJ1c0NsaWVudCIsInN1YiI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy8yMDA5LzA5L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy8yMDA5LzA5L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4gVXNlciIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnLzIwMDkvMDkvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImFkbWluQGNpdHlidXMuZ292IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4ifQ.signature",
        "user": {
            "id": 1,
            "name": "Admin User",
            "email": "admin@citybus.gov",
            "role": "Admin"
        }
    }
}
```

**Error Response (401 Unauthorized):**
```json
{
    "success": false,
    "message": "Invalid email or password"
}
```

---

## 2. AUDIO FILE ENDPOINTS

### 2.1 Get All Audio Files

**Request:**
```http
GET /audio
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": [
        {
            "id": 1,
            "name": "Route 5 Announcement",
            "category": "Route",
            "durationSeconds": 15,
            "description": "Standard route announcement",
            "isActive": true,
            "createdAt": "2026-01-30T10:00:00"
        },
        {
            "id": 2,
            "name": "Emergency Alert Siren",
            "category": "Emergency",
            "durationSeconds": 20,
            "description": "High priority emergency alert",
            "isActive": true,
            "createdAt": "2026-01-30T10:15:00"
        },
        {
            "id": 3,
            "name": "Festival Message - Diwali",
            "category": "Festival",
            "durationSeconds": 45,
            "description": "Festival celebration message",
            "isActive": true,
            "createdAt": "2026-01-30T10:30:00"
        }
    ]
}
```

### 2.2 Get Single Audio File

**Request:**
```http
GET /audio/1
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "id": 1,
        "name": "Route 5 Announcement",
        "category": "Route",
        "durationSeconds": 15,
        "description": "Standard route announcement",
        "isActive": true,
        "createdAt": "2026-01-30T10:00:00"
    }
}
```

### 2.3 Upload Audio File

**Request:**
```http
POST /audio/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

file: @/path/to/audio.mp3
```

**Response (201 Created):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "id": 9,
        "name": "New Audio File",
        "category": "Route",
        "durationSeconds": 0,
        "description": "",
        "isActive": true,
        "createdAt": "2026-01-30T15:45:00"
    }
}
```

**Error Response (400 Bad Request):**
```json
{
    "success": false,
    "message": "Invalid file format or size exceeds limit"
}
```

### 2.4 Update Audio File

**Request:**
```http
PUT /audio/1
Authorization: Bearer {token}
Content-Type: application/json

{
    "name": "Updated Route Announcement",
    "category": "Route",
    "description": "Updated description",
    "isActive": false
}
```

### 2.5 Delete Audio File

**Request:**
```http
DELETE /audio/1
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "Audio file deleted successfully",
    "data": null
}
```

---

## 3. SCHEDULE ENDPOINTS

### 3.1 Get All Schedules

**Request:**
```http
GET /schedules
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": [
        {
            "id": 1,
            "audioName": "Route 5 Announcement",
            "busNumber": "B001",
            "startDateTime": "2026-02-01T08:00:00",
            "repeatPattern": "Daily",
            "isActive": true
        },
        {
            "id": 2,
            "audioName": "Festival Message - Diwali",
            "busNumber": "B002",
            "startDateTime": "2026-02-15T10:00:00",
            "repeatPattern": "None",
            "isActive": true
        }
    ]
}
```

### 3.2 Create Schedule

**Request:**
```http
POST /schedules
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 1,
    "busId": 1,
    "startDateTime": "2026-02-05T09:00:00",
    "repeatPattern": "Weekly",
    "isActive": true
}
```

**Response (201 Created):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "id": 6,
        "audioName": "Route 5 Announcement",
        "busNumber": "B001",
        "startDateTime": "2026-02-05T09:00:00",
        "repeatPattern": "Weekly",
        "isActive": true
    }
}
```

### 3.3 Get Single Schedule

**Request:**
```http
GET /schedules/1
Authorization: Bearer {token}
```

### 3.4 Update Schedule

**Request:**
```http
PUT /schedules/1
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 2,
    "busId": 2,
    "startDateTime": "2026-02-05T10:00:00",
    "repeatPattern": "Daily",
    "isActive": true
}
```

### 3.5 Delete Schedule

**Request:**
```http
DELETE /schedules/1
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "Schedule deleted successfully",
    "data": null
}
```

---

## 4. BUS ENDPOINTS

### 4.1 Get All Buses

**Request:**
```http
GET /buses
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": [
        {
            "id": 1,
            "busNumber": "B001",
            "routeName": "Route 1",
            "status": "Active",
            "capacity": 45
        },
        {
            "id": 2,
            "busNumber": "B002",
            "routeName": "Route 2",
            "status": "Active",
            "capacity": 45
        },
        {
            "id": 3,
            "busNumber": "B003",
            "routeName": "Route 3",
            "status": "Inactive",
            "capacity": 50
        }
    ]
}
```

### 4.2 Create Bus

**Request:**
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

**Response (201 Created):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "id": 7,
        "busNumber": "B007",
        "routeName": "Route 1",
        "status": "Active",
        "capacity": 45
    }
}
```

### 4.3 Get Single Bus

**Request:**
```http
GET /buses/1
Authorization: Bearer {token}
```

### 4.4 Update Bus

**Request:**
```http
PUT /buses/1
Authorization: Bearer {token}
Content-Type: application/json

{
    "busNumber": "B001",
    "routeId": 2,
    "registrationNumber": "DL-01-AB-1234",
    "capacity": 50,
    "status": "Maintenance"
}
```

### 4.5 Delete Bus

**Request:**
```http
DELETE /buses/1
Authorization: Bearer {token}
```

---

## 5. ROUTE ENDPOINTS

### 5.1 Get All Routes

**Request:**
```http
GET /routes
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": [
        {
            "id": 1,
            "name": "Route 1",
            "startPoint": "Central Station",
            "endPoint": "Airport",
            "distanceKm": 25.5
        },
        {
            "id": 2,
            "name": "Route 2",
            "startPoint": "Downtown",
            "endPoint": "University",
            "distanceKm": 15.3
        },
        {
            "id": 3,
            "name": "Route 5",
            "startPoint": "Railway Station",
            "endPoint": "Bus Terminal",
            "distanceKm": 12.1
        }
    ]
}
```

### 5.2 Create Route

**Request:**
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

**Response (201 Created):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "id": 5,
        "name": "Route 10",
        "startPoint": "Market Square",
        "endPoint": "Industrial Area",
        "distanceKm": 18.5
    }
}
```

### 5.3 Get Single Route

**Request:**
```http
GET /routes/1
Authorization: Bearer {token}
```

### 5.4 Update Route

**Request:**
```http
PUT /routes/1
Authorization: Bearer {token}
Content-Type: application/json

{
    "name": "Route 1 - Updated",
    "startPoint": "New Central Station",
    "endPoint": "New Airport Terminal",
    "distanceKm": 26.0
}
```

### 5.5 Delete Route

**Request:**
```http
DELETE /routes/1
Authorization: Bearer {token}
```

---

## 6. DASHBOARD ENDPOINTS

### 6.1 Get Dashboard Statistics

**Request:**
```http
GET /dashboard/stats
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": {
        "totalAudioFiles": 8,
        "activeSchedules": 5,
        "totalBuses": 6,
        "upcomingAnnouncements": 3
    }
}
```

---

## 7. EMERGENCY ENDPOINTS

### 7.1 Broadcast Emergency Alert

**Request:**
```http
POST /emergency/broadcast
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 2,
    "busTarget": "all",
    "routeId": null,
    "priority": "Critical",
    "message": "Traffic alert: Major accident on Route 1 near Central Station"
}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": "Emergency broadcast sent successfully",
    "data": null
}
```

**Alternative Request (Route-Specific):**
```http
POST /emergency/broadcast
Authorization: Bearer {token}
Content-Type: application/json

{
    "audioFileId": 2,
    "busTarget": "route",
    "routeId": 1,
    "priority": "High",
    "message": "Minor traffic congestion on Route 1"
}
```

### 7.2 Get Emergency Broadcast History

**Request:**
```http
GET /emergency/history
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
    "success": true,
    "message": null,
    "data": [
        {
            "id": 1,
            "audioName": "Emergency Alert Siren",
            "audioId": 2,
            "busTarget": "All Buses",
            "priority": "Critical",
            "message": "Traffic alert: Major accident on Route 1",
            "status": "Completed",
            "broadcastedAt": "2026-01-30T14:30:00"
        }
    ]
}
```

---

## Error Responses

### 400 Bad Request
```json
{
    "success": false,
    "message": "Validation error or bad request",
    "data": null
}
```

### 401 Unauthorized
```json
{
    "success": false,
    "message": "Unauthorized - Invalid or missing token",
    "data": null
}
```

### 404 Not Found
```json
{
    "success": false,
    "message": "Resource not found",
    "data": null
}
```

### 500 Internal Server Error
```json
{
    "success": false,
    "message": "Internal server error",
    "data": null
}
```

---

## Rate Limits & Constraints

| Constraint | Value | Notes |
|-----------|-------|-------|
| Max Audio File Size | 50 MB | MP3 and WAV only |
| Max Audio Files | Unlimited | Limited by storage |
| Max Buses | Unlimited | Limited by database |
| Max Routes | Unlimited | Limited by database |
| Max Schedules | Unlimited | Limited by database |
| Token Expiration | 8 hours | Can be configured |
| Bus Number Length | 50 chars | Unique required |
| Audio Name Length | 200 chars | - |
| Route Name Length | 100 chars | - |

---

## Status Codes

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful GET, PUT, DELETE |
| 201 | Created | Successful POST (resource created) |
| 400 | Bad Request | Invalid input/request |
| 401 | Unauthorized | Missing/invalid token |
| 404 | Not Found | Resource doesn't exist |
| 500 | Server Error | Internal error |

---

## Swagger UI

Access interactive API documentation:
```
http://localhost:5000/swagger
```

---

**Version:** 1.0.0  
**Last Updated:** January 30, 2026
