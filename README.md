# CS690 Final Project – Device Tracker

**Data Center Device Management System**  
CS690 · C# / .NET 10 · Console Application

---

## Scenario

Cyrenna is a data center commissioning engineer managing hundreds of devices across multiple systems. She struggles with scattered spreadsheets, no single source of truth for device status, and no clear record of who worked on what. This application solves that problem.

---

## Features

- **Track devices** – Add and manage controllers, sensors, network equipment, servers, and more
- **Status management** – Move devices through their lifecycle: Pending → Installed → Configured → Active
- **Issue logging** – Log, track, and resolve device issues with severity levels
- **Team assignments** – Assign devices to technicians and track task completion
- **Filter & search** – Filter devices by status, type, system name, or assigned technician
- **Reports** – Generate status summaries and system-specific reports, export to file

---

## Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Run the application
```bash
git clone https://github.com/sketch1996/-CS690-FinalProject-.git
cd CS690-FinalProject
dotnet run --project src/DeviceTracker
```

### Run tests
```bash
dotnet test
```

---

## Project Structure

```
CS690-FinalProject/
├── src/
│   └── DeviceTracker/
│       ├── Models/          # Device, Issue, Assignment data models
│       ├── Services/        # DeviceService, IssueService, AssignmentService, ReportService
│       ├── Data/            # DataStore – JSON persistence
│       ├── UI/              # ConsoleHelper, MenuHandler
│       └── Program.cs       # Entry point
├── tests/
│   └── DeviceTracker.Tests/ # xUnit tests for all services
├── docs/                    # Documentation
└── CS690-FinalProject.sln
```

---

## Documentation

- [User Documentation](docs/USER_GUIDE.md)
- [Deployment Guide](docs/DEPLOYMENT.md)
- [Development Guide](docs/DEVELOPMENT.md)
- [Wiki](../../wiki)

---

## Releases

| Version | Description |
|---------|-------------|
| v1.0.0  | Core device tracking, status management, issue logging |
| v2.0.0  | Assignments, reports, modular architecture, full tests |
| v3.0.0  | All features complete, full documentation |
