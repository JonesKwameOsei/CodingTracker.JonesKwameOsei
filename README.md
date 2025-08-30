# 🚀 CodingTracker

<div align="center">

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=csharp)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

*A sophisticated console application for tracking your coding sessions with precision and style*

[Features](#-features) • [Installation](#-installation) • [Usage](#-usage) • [Architecture](#-architecture) • [Contributing](#-contributing)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Usage](#-usage)
  - [Manual Session Entry](#manual-session-entry)
  - [Live Session Tracking](#live-session-tracking)
  - [Session Management](#session-management)
  - [Filtering & Analysis](#filtering--analysis)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Database Schema](#-database-schema)
- [Configuration](#-configuration)
- [Skills Gained](#-skills-gained)
- [Challenges Faced](#-challenges-faced)
- [Future Features](#-future-features)
- [Contributing](#-contributing)
- [License](#-license)
- [Acknowledgments](#-acknowledgments)

## 🎯 Overview

**CodingTracker** is an elegant console application designed for developers who want to monitor their coding productivity with precision. Built with .NET 9 and powered by a robust SQLite database, it offers both manual session logging and real-time tracking capabilities with an intuitive, colorful interface.

Whether you're tracking your daily coding habits, analyzing productivity patterns, or simply curious about how much time you spend programming, CodingTracker provides the insights you need in a beautifully crafted terminal experience.

## ✨ Features

### 🎮 Core Functionality
- **Manual Session Entry** - Log coding sessions with custom start/end times
- **Live Session Tracking** - Real-time stopwatch with pause/resume capabilities
- **Session Management** - Full CRUD operations for all your coding records
- **Smart Filtering** - Filter sessions by periods (today, week, month, year, custom ranges)
- **Data Analysis** - Comprehensive summaries with total time and session counts

### 🎨 User Experience
- **Rich Terminal UI** - Powered by Spectre.Console for beautiful, interactive displays
- **Intuitive Navigation** - Clean menu system with keyboard shortcuts
- **Live Updates** - Real-time session display with pause indicators
- **Data Validation** - Robust input validation with helpful error messages
- **Flexible Ordering** - Sort sessions chronologically or reverse chronologically

### 🔧 Technical Features
- **SQLite Database** - Lightweight, embedded database for reliable data persistence
- **Seed Data Support** - Automatic generation of sample data for testing
- **Configuration Management** - JSON-based configuration system
- **Modular Architecture** - Clean separation of concerns with dedicated layers

## 🔧 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- Windows, macOS, or Linux operating system
- Terminal with Unicode support (recommended for optimal display)

## 📦 Installation

### Clone the Repository
```bash
git clone https://github.com/JonesKwameOsei/CodingTracker.JonesKwameOsei.git
cd CodingTracker.JonesKwameOsei
```

### Build the Application
```bash
dotnet build
```

### Run the Application
```bash
dotnet run --project CodingTracker.JonesKwameOsei
```

## 🎯 Usage

### Manual Session Entry
1. Select **"Add Coding Session (Manual)"** from the main menu
2. Enter the programming language
3. Specify start and end times using the format: `dd-MM-yy HH:mm`
4. Session is automatically saved to the database

### Live Session Tracking
1. Choose **"Start Live Coding Session"** 
2. Select your programming language
3. Use keyboard controls:
   - `P` - Pause/Resume the session
   - `L` - Add a lap marker
   - `S` - Stop and save the session
   - `Q` - Cancel without saving
   - `H` - Display help

### Session Management
- **View All Sessions** - Display complete session history
- **Filter Sessions** - View sessions by specific time periods
- **Update Session** - Modify existing session details
- **Delete Session** - Remove sessions with confirmation

### Filtering & Analysis
Filter your sessions by:
- **Today** / **Yesterday**
- **Last 7 Days**
- **This Week** / **Last Week**
- **This Month** / **Last Month**
- **This Year** / **Last Year**
- **Custom Date Range**

Each filtered view includes:
- Total number of sessions
- Cumulative coding time
- Chronological or reverse-chronological ordering

## 🏗️ Architecture

```
CodingTracker.JonesKwameOsei/
├── 📁 Core Models
│   ├── CodingRecord.cs      # Data model for coding sessions
│   ├── LiveSession.cs       # Real-time session management
│   └── Enums.cs            # Application enumerations
├── 📁 Data Layer
│   ├── DataAccess.cs       # Database operations with Dapper
│   └── SeedData.cs         # Sample data generation
├── 📁 Business Logic
│   ├── UserInterface.cs    # UI orchestration and menu system
│   └── Validation.cs       # Input validation and sanitization
├── 📁 Configuration
│   ├── Program.cs          # Application entry point
│   └── appsettings.json    # Database configuration
└── 📁 Project Files
    └── CodingTracker.JonesKwameOsei.csproj
```

## 🛠️ Technologies

| Technology | Purpose | Version |
|------------|---------|---------|
| **.NET** | Runtime Framework | 9.0 |
| **C#** | Programming Language | 13.0 |
| **SQLite** | Database Engine | Latest |
| **Dapper** | Object-Relational Mapping | 2.1.66 |
| **Spectre.Console** | Terminal UI Framework | 0.50.0 |
| **Microsoft.Extensions.Configuration** | Configuration Management | 9.0.8 |

## 🗄️ Database Schema

```sql
CREATE TABLE codingSessions (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    Language    TEXT NOT NULL,
    DateStart   TEXT NOT NULL,
    DateEnd     TEXT NOT NULL
);
```

The `Duration` property is calculated dynamically from `DateStart` and `DateEnd` fields.

## ⚙️ Configuration

Configuration is managed through `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YourDBName"
  }
}
```

You can modify the database location by updating the connection string.

## 🎓 Skills Gained

This project served as an excellent learning opportunity to master several key technologies and concepts:

### **Primary Learning Achievement**
- **🗃️ Database Manipulation with ``Dapper``** - The most significant skill gained was learning to efficiently interact with SQLite databases using Dapper ORM. This included:
  - Writing raw SQL queries for CRUD operations
  - Parameterised queries for security and performance
  - Database schema design and table creation
  - Connection management and disposal patterns
  - Mapping database results to C# objects

### **Additional Skills Developed**
- **🎨 Advanced Console UI Design** - Mastering Spectre.Console for creating rich, interactive terminal applications
- **⏱️ Real-time Programming** - Implementing live session tracking with stopwatch functionality and non-blocking keyboard input
- **🔧 Configuration Management** - Setting up JSON-based configuration systems in .NET applications
- **📊 Data Filtering & Analysis** - Creating flexible filtering systems with LINQ for time-based data analysis
- **🏗️ Clean Architecture** - Implementing separation of concerns with dedicated layers for data access, business logic, and presentation

## 🚧 Challenges Faced

### **Major Technical Challenges**

#### **1. Live Coding Session Implementation**
- **Challenge**: Creating a real-time session tracker that could handle pause/resume functionality while maintaining accurate time tracking
- **Solution**: Implemented a `LiveSession` class using `System.Diagnostics.Stopwatch` for precise timing, combined with non-blocking keyboard input handling
- **Key Learning**: Understanding the difference between wall-clock time and active session time, and how to properly manage stopwatch state transitions

#### **2. Complex Filtering Functionality**
- **Challenge**: Building a flexible filtering system that could handle various time periods (today, week, month, year, custom ranges) while maintaining performance
- **Solution**: Developed a comprehensive enum-based filtering system with dynamic date range calculation and LINQ-based filtering
- **Key Learning**: Working with `DateTime` calculations, culture-specific week start days, and efficient in-memory data filtering

#### **3. User Interface State Management**
- **Challenge**: Managing console clearing and display updates without losing user context or creating flickering effects
- **Solution**: Strategic placement of `Console.Clear()` calls and implementing proper wait states after displaying information
- **Key Learning**: Understanding console application lifecycle and user experience considerations in terminal applications

## 🚀 Future Features

### **Planned Enhancements**

#### **1. 🎯 Goal Setting & Progress Tracking**
- **Feature**: Allow users to set daily, weekly, monthly, or yearly coding goals
- **Implementation Options**:
  - **SQL Approach**: Store goals in a separate table with progress calculations via aggregate queries
  - **C# Approach**: In-memory goal tracking with LINQ-based progress calculations
- **Functionality**:
  - Set target hours for specific time periods
  - Real-time progress indicators showing completion percentage
  - Automatic calculation of required daily hours to meet remaining goals
  - Visual progress bars using Spectre.Console charts
  - Achievement notifications when goals are met or exceeded

#### **2. 📈 Advanced Analytics Dashboard**
- **Productivity Trends**: Weekly/monthly productivity charts and patterns
- **Language Statistics**: Time distribution across different programming languages
- **Peak Performance Hours**: Analysis of most productive time periods
- **Streak Tracking**: Consecutive coding days and longest streaks

#### **3. 📤 Data Export & Reporting**
- **CSV/JSON Export**: Export session data for external analysis
- **Weekly/Monthly Reports**: Automated summary reports with insights
- **Integration Options**: Export to popular time-tracking services

#### **4. ⚡ Performance Optimizations**
- **Database Indexing**: Optimize queries for large datasets
- **Lazy Loading**: Implement pagination for large session lists
- **Caching**: Cache frequently accessed data for improved performance

#### **5. 🔔 Smart Notifications**
- **Break Reminders**: Configurable alerts for extended coding sessions
- **Daily Goal Alerts**: Notifications about progress toward daily goals
- **Productivity Insights**: Weekly summaries with actionable recommendations

## 🤝 Contributing

We welcome contributions! Here's how you can help:

### Development Setup
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes and ensure tests pass
4. Commit your changes: `git commit -m 'Add amazing feature'`
5. Push to the branch: `git push origin feature/amazing-feature`
6. Open a Pull Request

### Contribution Guidelines
- Follow C# coding conventions and best practices
- Ensure all new features include appropriate validation
- Test your changes thoroughly before submitting
- Update documentation for any new features
- Keep commits focused and write clear commit messages

## 📄 License

This project is not licensed under any license. 

## 🙏 Acknowledgments

- **[Spectre.Console](https://spectreconsole.net/)** - For the beautiful terminal UI framework
- **[Dapper](https://dapperlib.github.io/Dapper/)** - For the elegant ORM solution
- **Microsoft** - For the excellent .NET ecosystem
- **The C# Academy** - For project inspiration and learning resources

---

<div align="center">

**Built with ❤️ by [Jones Kwame Osei](https://github.com/JonesKwameOsei)**

⭐ Star this repository if you find it helpful!

</div>