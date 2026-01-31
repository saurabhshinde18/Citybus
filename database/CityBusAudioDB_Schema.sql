-- City Bus Audio Scheduler Database Schema

CREATE DATABASE IF NOT EXISTS CityBusAudioDB;
USE CityBusAudioDB;

-- Users Table
CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'Admin',
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    INDEX idx_email (Email),
    INDEX idx_role (Role)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Routes Table
CREATE TABLE Routes (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    StartPoint VARCHAR(100) NOT NULL,
    EndPoint VARCHAR(100) NOT NULL,
    DistanceKm DOUBLE NOT NULL,
    Description VARCHAR(500),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    INDEX idx_name (Name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Buses Table
CREATE TABLE Buses (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    BusNumber VARCHAR(50) NOT NULL UNIQUE,
    RouteId INT,
    RegistrationNumber VARCHAR(100),
    Capacity INT DEFAULT 45,
    Status VARCHAR(50) DEFAULT 'Active',
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (RouteId) REFERENCES Routes(Id) ON DELETE SET NULL,
    INDEX idx_bus_number (BusNumber),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Audio Files Table
CREATE TABLE AudioFiles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    FileName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) DEFAULT 'Route',
    DurationSeconds INT,
    FileSizeBytes BIGINT,
    Description VARCHAR(500),
    IsActive BOOLEAN DEFAULT TRUE,
    FilePath VARCHAR(500),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    INDEX idx_category (Category),
    INDEX idx_is_active (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Schedules Table
CREATE TABLE Schedules (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    AudioFileId INT NOT NULL,
    BusId INT NOT NULL,
    StartDateTime DATETIME NOT NULL,
    RepeatPattern VARCHAR(50) DEFAULT 'None',
    IsActive BOOLEAN DEFAULT TRUE,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (AudioFileId) REFERENCES AudioFiles(Id) ON DELETE CASCADE,
    FOREIGN KEY (BusId) REFERENCES Buses(Id) ON DELETE CASCADE,
    INDEX idx_bus_schedule (BusId, StartDateTime),
    INDEX idx_is_active (IsActive)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Emergency Logs Table
CREATE TABLE EmergencyLogs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    AudioFileId INT NOT NULL,
    BusId INT,
    RouteId INT,
    Priority VARCHAR(50) DEFAULT 'High',
    Message VARCHAR(500),
    Status VARCHAR(50) DEFAULT 'Completed',
    BroadcastedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AudioFileId) REFERENCES AudioFiles(Id) ON DELETE CASCADE,
    FOREIGN KEY (BusId) REFERENCES Buses(Id) ON DELETE SET NULL,
    FOREIGN KEY (RouteId) REFERENCES Routes(Id) ON DELETE SET NULL,
    INDEX idx_priority (Priority),
    INDEX idx_broadcasted_at (BroadcastedAt)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create default admin user (password: Password@123)
INSERT INTO Users (Name, Email, PasswordHash, Role, IsActive) VALUES 
(
    'Admin User',
    'admin@citybus.gov',
    '$2b$12$abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOP',
    'Admin',
    TRUE
);

-- Seed sample routes
INSERT INTO Routes (Name, StartPoint, EndPoint, DistanceKm, Description) VALUES 
('Route 1', 'Central Station', 'Airport', 25.5, 'Main airport connection route'),
('Route 2', 'Downtown', 'University', 15.3, 'Educational institution route'),
('Route 3', 'Shopping Mall', 'Hospital', 8.7, 'Commercial to medical facility'),
('Route 5', 'Railway Station', 'Bus Terminal', 12.1, 'Inter-terminal connection route');

-- Seed sample buses
INSERT INTO Buses (BusNumber, RouteId, RegistrationNumber, Capacity, Status) VALUES 
('B001', 1, 'DL-01-AB-1234', 45, 'Active'),
('B002', 2, 'DL-01-AB-1235', 45, 'Active'),
('B003', 3, 'DL-01-AB-1236', 50, 'Active'),
('B004', 4, 'DL-01-AB-1237', 45, 'Inactive'),
('B005', 1, 'DL-01-AB-1238', 45, 'Active'),
('B006', 2, 'DL-01-AB-1239', 45, 'Active');

-- Seed sample audio files
INSERT INTO AudioFiles (Name, FileName, Category, DurationSeconds, FileSizeBytes, Description, IsActive, FilePath) VALUES 
('Route 1 Destination', 'route1_announcement.mp3', 'Route', 15, 2456000, 'Standard route announcement for Route 1', TRUE, '/uploads/audio/route1_announcement.mp3'),
('Route 2 Update', 'route2_announcement.mp3', 'Route', 18, 2890000, 'Standard route announcement for Route 2', TRUE, '/uploads/audio/route2_announcement.mp3'),
('Emergency Alert Siren', 'emergency_siren.mp3', 'Emergency', 20, 3456000, 'Critical emergency alert with high priority', TRUE, '/uploads/audio/emergency_siren.mp3'),
('Medical Emergency', 'medical_emergency.mp3', 'Emergency', 25, 4123000, 'Medical emergency announcement', TRUE, '/uploads/audio/medical_emergency.mp3'),
('Diwali Festival Message', 'diwali_message.mp3', 'Festival', 45, 7234000, 'Festival celebration announcement for Diwali', TRUE, '/uploads/audio/diwali_message.mp3'),
('New Year Greeting', 'newyear_greeting.mp3', 'Festival', 30, 4890000, 'New Year celebration message', TRUE, '/uploads/audio/newyear_greeting.mp3'),
('Local Business Ad', 'local_ad_01.mp3', 'Advertisement', 30, 5123000, 'Advertisement for local business', TRUE, '/uploads/audio/local_ad_01.mp3'),
('Bank Services Ad', 'bank_ad_01.mp3', 'Advertisement', 25, 4567000, 'Banking services advertisement', FALSE, '/uploads/audio/bank_ad_01.mp3');

-- Sample schedules
INSERT INTO Schedules (AudioFileId, BusId, StartDateTime, RepeatPattern, IsActive) VALUES 
(1, 1, '2026-02-01 08:00:00', 'Daily', TRUE),
(2, 2, '2026-02-01 09:30:00', 'Daily', TRUE),
(3, 3, '2026-02-01 14:00:00', 'None', TRUE),
(5, 1, '2026-02-15 10:00:00', 'None', TRUE),
(4, 2, '2026-02-10 16:00:00', 'Weekly', TRUE);
