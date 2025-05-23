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
- [Demo](#demo)
- [Troubleshooting](#troubleshooting)
- [Development Notes](#development-notes)
- [License](#license)

---

## Features
- **System Metrics Collection**: Real-time CPU usage, memory consumption, active window, and session duration tracking (Windows APIs)
- **Code Activity Tracking**: Monitors file edits, project switches, and tool usage with a code activity tracker
- **Local Analytics Engine**: Summarizes daily engagement, feature usage, retention, and conversion funnel using SQLite and background services
- **Growth-Driven Dashboard**: WPF UI with real-time metrics, analytics, onboarding wizard, feature suggestions, and premium feature simulation
- **A/B Testing & Feature Flags**: JSON-configurable feature flags and A/B test groups for UI and feature experiments
- **Progressive Feature Discovery**: Context-aware tooltips, onboarding wizard, and feature suggestions based on usage patterns
- **Cohort Analysis**: Simulated cohort analysis with chart placeholder and retention visualization

## Tech Stack
- **Language:** C#
- **Framework:** .NET 8
- **UI:** WPF (Windows only)
- **ORM:** Entity Framework Core (SQLite)
- **Logging:** Serilog
- **System APIs:** Windows Performance Counters, WMI, user32.dll (Windows only)
- **Analytics:** JSON, local SQLite

## Project Structure
- `WDPS.Core/` — Core logic, models, services, data access
- `WDPS.Tests/` — xUnit test project for core logic
- `WDPS.App/` — WPF UI project (dashboard, onboarding, analytics)

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
Edit `WDPS.Core/appsettings.json` and (optionally) `WDPS.App/FeatureFlagConfig.sample.json` as needed:

```json
{
  "FeatureFlags": {
    "EnableDarkTheme": true,
    "EnablePremiumFeatures": false,
    "ShowOnboarding": true,
    "EnableTooltips": true,
    "EnableCohortAnalysis": true
  },
  "ABTestGroups": {
    "DashboardLayout": "A",
    "OnboardingFlow": "B"
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
To launch the WPF dashboard:
```sh
dotnet run --project WDPS.App
```
- The dashboard will open, showing real-time metrics, analytics, onboarding, feature suggestions, and premium feature simulation.
- Tooltips are available on all key metrics and analytics for progressive feature discovery.
- The cohort analysis section includes a chart placeholder for retention visualization.

---

## Running Tests
From the root or `WDPS.Tests` directory:
```sh
dotnet test
```
- Tests will run on any platform, but system metrics tests will only fully work on Windows.
- On Linux/Mac, tests will pass but log errors for Windows-only features.

---

## Demo

1. **Launch the dashboard:**
   - Run `dotnet run --project WDPS.App` on Windows.
   - Interact with the UI to see real-time updates, onboarding progress, and feature suggestions.
2. **Try toggling feature flags:**
   - Edit `WDPS.App/FeatureFlagConfig.sample.json` to enable/disable features or switch A/B test groups.
   - Restart the app to see changes reflected in the UI.
3. **Simulate code activity:**
   - Use the app and edit files in tracked directories to see code activity and analytics update.
4. **Explore tooltips:**
   - Hover over metrics and analytics for intelligent, context-aware guidance.

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
- **To extend the dashboard:**
  - Add new features, charts, or onboarding steps in `WDPS.App`.
  - Use the feature flag system to experiment with new UI/UX ideas.
- **Feature Flags & A/B Testing:**
  - Use JSON config and local analytics as described in the requirements.
- **Extending Analytics:**
  - Add new models/services in `WDPS.Core` as needed.

---

## License
This project is licensed under the MIT License - see the LICENSE file for details. 