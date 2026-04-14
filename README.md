# CS690 Final Project – Device Tracker

**Data Center Device Management System**  
C# / .NET 10 / Console Application

---

## Prerequisites

.NET 10 SDK: https://dotnet.microsoft.com/download/dotnet/10.0

    dotnet --version

---

## Quick Start

    git clone https://github.com/sketch1996/-CS690-FinalProject-.git
    cd -CS690-FinalProject-
    dotnet run --project src/DeviceTracker

---

## Main Menu

When the app launches:

    1.  Manage Devices
    2.  Manage Issues
    3.  Manage Assignments
    4.  Reports
    0.  Exit

Type a number and press Enter. Type 0 at any screen to go back.

---

## Manage Devices

    1. List all devices      - Color-coded table of every device
    2. Add new device        - Enter name, type, system, location, IP
    3. View device details   - Full record, issues, assignments
    4. Update status         - Pending, Installed, Configured, Active, Faulty
    5. Assign device         - Assign to technician with task
    6. Filter / search       - Filter by status, type, system, technician
    7. Delete device         - Remove with confirmation

Status colors: Green=Active  Yellow=Pending  Red=Faulty  Cyan=Installed

---

## Manage Issues

    1. List all open issues  - Unresolved issues only
    2. List all issues       - Every issue
    3. Log new issue         - Device ID, description, severity, reporter
    4. Resolve issue         - Issue ID, resolver name, notes
    5. Update status         - Open, InProgress, or Resolved

---

## Manage Assignments

    1. List all assignments      - All assignments with status
    2. View by technician        - Filter by technician name
    3. Complete assignment       - Mark done with notes
    4. Team workload summary     - Open task count per technician

---

## Reports

    1. Full status report        - Counts by status and system, open issues
    2. System-specific report    - All devices in one system
    3. Export to file            - Saves to ~/DeviceTrackerReports/

---

## Data Storage

All data saves automatically - no manual save needed.

    Windows:       %APPDATA%\DeviceTracker\
    macOS / Linux: ~/.local/share/DeviceTracker/

---

## Documentation

User Guide:       https://github.com/sketch1996/-CS690-FinalProject-/wiki/User-Guide
Deployment Guide: https://github.com/sketch1996/-CS690-FinalProject-/wiki/Deployment-Guide
Development Guide: https://github.com/sketch1996/-CS690-FinalProject-/wiki/Development-Guide
