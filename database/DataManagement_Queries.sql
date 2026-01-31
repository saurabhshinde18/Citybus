-- City Bus Audio Scheduler Database - Test Data and Maintenance Scripts

-- ========== USER MANAGEMENT ==========
-- View all users
SELECT * FROM Users;

-- Create a new admin user
INSERT INTO Users (Name, Email, PasswordHash, Role, IsActive) VALUES 
('Operator User', 'operator@citybus.gov', 'hashed_password_here', 'Operator', TRUE);

-- Update user status
UPDATE Users SET IsActive = FALSE WHERE Email = 'operator@citybus.gov';

-- ========== ROUTE MANAGEMENT ==========
-- View all routes with bus count
SELECT r.*, COUNT(b.Id) as BusCount
FROM Routes r
LEFT JOIN Buses b ON r.Id = b.RouteId
GROUP BY r.Id
ORDER BY r.Name;

-- Add a new route
INSERT INTO Routes (Name, StartPoint, EndPoint, DistanceKm, Description) VALUES 
('Route 10', 'Market Square', 'Industrial Area', 18.5, 'New industrial zone connection');

-- Update route details
UPDATE Routes SET DistanceKm = 13.2 WHERE Id = 1;

-- ========== BUS MANAGEMENT ==========
-- View all buses with their routes
SELECT b.*, r.Name as RouteName
FROM Buses b
LEFT JOIN Routes r ON b.RouteId = r.Id
ORDER BY b.BusNumber;

-- Add a new bus
INSERT INTO Buses (BusNumber, RouteId, RegistrationNumber, Capacity, Status) VALUES 
('B007', 1, 'DL-01-AB-1240', 45, 'Active');

-- Update bus status to maintenance
UPDATE Buses SET Status = 'Maintenance' WHERE BusNumber = 'B007';

-- ========== AUDIO FILES MANAGEMENT ==========
-- View all audio files by category
SELECT Category, COUNT(*) as FileCount, SUM(FileSizeBytes)/1024/1024 as TotalSizeMB
FROM AudioFiles
GROUP BY Category;

-- Find emergency audio files
SELECT * FROM AudioFiles WHERE Category = 'Emergency' AND IsActive = TRUE;

-- Find largest audio files
SELECT Name, Category, FileSizeBytes/1024/1024 as SizeMB, DurationSeconds
FROM AudioFiles
ORDER BY FileSizeBytes DESC
LIMIT 10;

-- ========== SCHEDULE MANAGEMENT ==========
-- View all active schedules
SELECT s.*, a.Name as AudioName, b.BusNumber
FROM Schedules s
JOIN AudioFiles a ON s.AudioFileId = a.Id
JOIN Buses b ON s.BusId = b.Id
WHERE s.IsActive = TRUE
ORDER BY s.StartDateTime;

-- View schedules for specific bus
SELECT s.*, a.Name as AudioName
FROM Schedules s
JOIN AudioFiles a ON s.AudioFileId = a.Id
WHERE s.BusId = 1
ORDER BY s.StartDateTime;

-- Find upcoming schedules (next 7 days)
SELECT s.*, a.Name as AudioName, b.BusNumber
FROM Schedules s
JOIN AudioFiles a ON s.AudioFileId = a.Id
JOIN Buses b ON s.BusId = b.Id
WHERE s.StartDateTime BETWEEN NOW() AND DATE_ADD(NOW(), INTERVAL 7 DAY)
AND s.IsActive = TRUE
ORDER BY s.StartDateTime;

-- Cancel all schedules for a bus
UPDATE Schedules SET IsActive = FALSE WHERE BusId = 1;

-- ========== EMERGENCY LOGS ==========
-- View recent emergency broadcasts
SELECT el.*, a.Name as AudioName, b.BusNumber
FROM EmergencyLogs el
LEFT JOIN AudioFiles a ON el.AudioFileId = a.Id
LEFT JOIN Buses b ON el.BusId = b.Id
ORDER BY el.BroadcastedAt DESC
LIMIT 20;

-- Count emergency broadcasts by date
SELECT DATE(BroadcastedAt) as BroadcastDate, COUNT(*) as Count
FROM EmergencyLogs
GROUP BY DATE(BroadcastedAt)
ORDER BY BroadcastedAt DESC;

-- ========== PERFORMANCE & MAINTENANCE ==========
-- Optimize all tables
OPTIMIZE TABLE Users;
OPTIMIZE TABLE Routes;
OPTIMIZE TABLE Buses;
OPTIMIZE TABLE AudioFiles;
OPTIMIZE TABLE Schedules;
OPTIMIZE TABLE EmergencyLogs;

-- Check database size
SELECT 
    table_name,
    ROUND(((data_length + index_length) / 1024 / 1024), 2) AS size_mb
FROM information_schema.TABLES
WHERE table_schema = 'CityBusAudioDB'
ORDER BY (data_length + index_length) DESC;

-- ========== REPORTING QUERIES ==========
-- Daily schedule summary
SELECT 
    DATE(StartDateTime) as ScheduleDate,
    COUNT(*) as TotalSchedules,
    COUNT(DISTINCT BusId) as BusesScheduled,
    COUNT(DISTINCT AudioFileId) as UniquAudioFiles
FROM Schedules
WHERE IsActive = TRUE
GROUP BY DATE(StartDateTime)
ORDER BY ScheduleDate DESC;

-- Audio usage report
SELECT 
    a.Name,
    a.Category,
    COUNT(s.Id) as TimesScheduled,
    COUNT(el.Id) as EmergencyBroadcasts,
    a.DurationSeconds
FROM AudioFiles a
LEFT JOIN Schedules s ON a.Id = s.AudioFileId
LEFT JOIN EmergencyLogs el ON a.Id = el.AudioFileId
GROUP BY a.Id
ORDER BY TimesScheduled DESC;

-- Bus activity report
SELECT 
    b.BusNumber,
    r.Name as Route,
    COUNT(s.Id) as ScheduledAnnouncements,
    b.Status,
    b.Capacity
FROM Buses b
LEFT JOIN Routes r ON b.RouteId = r.Id
LEFT JOIN Schedules s ON b.Id = s.BusId AND s.IsActive = TRUE
GROUP BY b.Id
ORDER BY b.BusNumber;
