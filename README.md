# Windows Developer Productivity Suite (WDPS)

A comprehensive Windows desktop application that tracks developer productivity metrics and demonstrates product-led growth through intelligent feature recommendations and user engagement optimization.

---

## Table of Contents
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Platform Support](#platform-support)
- [Setup Instructions](#setup-instructions)
- [Configuration](#configuration)
- [Building the Project](#building-the-project)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [Troubleshooting](#troubleshooting)
- [Development Notes](#development-notes)
- [License](#license)

---

## Features
- **System Metrics Collection**: CPU usage, memory consumption, active window focus, process monitoring
- **Code Activity Tracking**: File system event monitoring, project switch detection, development tool usage
- **Local Analytics Engine**: SQLite storage, background analytics, custom pipeline
- **Growth-Driven Dashboard**: Multiple UI themes, A/B testing, JSON config
- **Progressive Feature Discovery**: Intelligent tooltips, feature suggestions, local pattern storage

## Tech Stack
- **Language:** C#
- **Framework:** .NET 8
- **UI:** WPF (Windows only, to be implemented)
- **ORM:** Entity Framework Core (SQLite)
- **Logging:** Serilog
- **System APIs:** Windows Performance Counters, WMI, user32.dll (Windows only)
- **Analytics:** JSON, local SQLite

## Project Structure
- `WDPS.Core/` — Core logic, models, services, data access
- `WDPS.Tests/` — xUnit test project for core logic
- `WDPS.App/` — (To be implemented) WPF UI project

## Platform Support
- **Windows:** Full functionality (system metrics, UI, analytics, etc.)
- **Linux/Mac:** Only core logic and tests will run. System metrics and Windows API features will log errors or be non-functional.

---

## Setup Instructions

### 1. Prerequisites
- Windows 10+ (for full functionality)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- (Optional) Visual Studio 2022+ or VS Code
- Git

### 2. Clone the Repository
```sh
git clone <your-repo-url>
cd WDPS
```

### 3. Restore Dependencies
```sh
dotnet restore
```

### 4. Configure the Application
Edit `WDPS.Core/appsettings.json` as needed:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=wdps.db"
  },
  "AppSettings": {
    "Database": {
      "ConnectionString": "Data Source=wdps.db"
    },
    "Logging": {
      "LogFilePath": "logs/wdps.log",
      "MinimumLevel": "Information"
    },
    "Metrics": {
      "CollectionIntervalMinutes": 1,
      "EnableCpuTracking": true,
      "EnableMemoryTracking": true,
      "EnableWindowTracking": true
    }
  }
}
```

---

## Building the Project
From the root directory:
```sh
dotnet build
```

---

## Running the Application
**Note:** The main application UI (`WDPS.App`) is not yet implemented. The core logic and background services are in `WDPS.Core`.

To run background services (on Windows):
1. Create a console or WPF app referencing `WDPS.Core` and call the service registration in `ServiceCollectionExtensions`.
2. (To be implemented) Once the WPF UI is added, run:
   ```sh
   dotnet run --project WDPS.App
   ```

---

## Running Tests
From the root or `WDPS.Tests` directory:
```sh
dotnet test
```
- Tests will run on any platform, but system metrics tests will only fully work on Windows.
- On Linux/Mac, tests will pass but log errors for Windows-only features.

---

## Troubleshooting
- **Linux/Mac:**
  - System metrics collection will log errors (e.g., `user32.dll` not found, `PerformanceCounter` not supported).
  - This is expected; move to Windows for full functionality.
- **Windows:**
  - If you see permission errors, run as administrator.
  - Ensure .NET 8 SDK is installed.
- **Database:**
  - SQLite database file (`wdps.db`) will be created in the working directory.
- **Logs:**
  - Log files are written to `logs/wdps.log` (configurable).

---

## Development Notes
- **To add the WPF UI:**
  - On Windows, run:
    ```sh
    dotnet new wpf -n WDPS.App -f net8.0
    dotnet sln add WDPS.App/WDPS.App.csproj
    ```
  - Reference `WDPS.Core` from `WDPS.App` and implement the dashboard, onboarding, A/B testing, etc.
- **Feature Flags & A/B Testing:**
  - Use JSON config and local analytics as described in the requirements.
- **Extending Analytics:**
  - Add new models/services in `WDPS.Core` as needed.

---

## License
This project is licensed under the MIT License - see the LICENSE file for details. 