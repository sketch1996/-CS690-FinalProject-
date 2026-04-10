#!/bin/bash
set -e

WIKI_REPO="https://github.com/sketch1996/-CS690-FinalProject-.wiki.git"
WIKI_DIR="_wiki_tmp"

rm -rf "$WIKI_DIR"
git clone "$WIKI_REPO" "$WIKI_DIR"

cat > "$WIKI_DIR/Design.md" <<'MD'
# Design – Device Tracker

This page contains the design materials for the Device Tracker project, including the domain model, activity diagrams, UI design, prototype, and system architecture.

---

## Domain Model

The domain model represents the core objects in the Device Tracker system and the relationships between them.

### Main Domain Classes
1. **Device**
   - Represents a field device or controller being tracked
   - Attributes: Id, Name, Type, Status, SystemName, Location, IpAddress, AssignedTo, CreatedAt, UpdatedAt

2. **Issue**
   - Represents a problem or fault associated with a device
   - Attributes: Id, DeviceId, Title, Description, Severity, IsResolved, CreatedAt, ResolvedAt

3. **Assignment**
   - Represents a technician assignment for a specific device
   - Attributes: Id, DeviceId, TechnicianId, TaskDescription, AssignedDate, Status

4. **Technician**
   - Represents a technician who can be assigned to devices and issues
   - Attributes: Id, Name, Role, ContactInfo

5. **SystemGroup**
   - Represents the larger system or equipment group a device belongs to
   - Attributes: Id, Name, Description

### Enumerations
- **DeviceType**
- **DeviceStatus**
- **IssueSeverity**

### Relationships
- One **SystemGroup** can contain many **Devices**
- One **Device** can have many **Issues**
- One **Device** can have many **Assignments**
- One **Technician** can have many **Assignments**

![Domain Model](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/domain-model.png)

---

## Activity Diagrams

The system includes one activity diagram for each major use case. These diagrams show both the normal flow and alternate/error flows, including “device found” and “device not found” decision paths where appropriate.

### 1. Add Device
Shows the process of entering device information, validating the input, and saving the record.

![Activity Diagram - Add Device](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-add-device.png)

### 2. Search / View Device
Shows the process of searching for a device and following either the **Yes** path if the device is found or the **No** path if the device is not found.

![Activity Diagram - Search Device](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-search-device.png)

### 3. Update Device Status
Shows the workflow for locating a device, updating its status, and saving the change.

![Activity Diagram - Update Status](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-update-status.png)

### 4. Assign Technician
Shows the process of finding a device, selecting a technician, and saving the assignment.

![Activity Diagram - Assign Technician](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-assign-technician.png)

### 5. Log Device Issue
Shows the workflow for searching for a device, entering issue details, and saving the issue record.

![Activity Diagram - Log Device Issue](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-log-issue.png)

### 6. Generate Report
Shows the process of selecting report criteria, generating a report, and displaying/exporting the result.

![Activity Diagram - Generate Report](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/activity-generate-report.png)

---

## User Interface (UI) Design

The UI design section shows the layout of the console screens used throughout the system. These mockups focus on what the user sees on each screen, including menus, input prompts, search screens, status displays, and reports.

Key screens included:
- Main Menu
- Add Device Screen
- Search Device Screen
- Update Status Screen
- Assign Technician Screen
- Log Issue Screen
- Report Screen

![UI Design](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/ui-design.png)

---

## Prototype

The prototype shows the full user journey through the application. Unlike the UI Design section, which focuses on individual screens, this prototype focuses on how a user moves from one feature to the next.

Example flows shown:
- Main Menu → Search Device → Device Found / Device Not Found
- Main Menu → Add Device → Save Confirmation
- Main Menu → Assign Technician → Success Message
- Main Menu → Generate Report → View Report Output

![Prototype](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/prototype.png)

---

## Architecture

The application follows a 4-layer console architecture:

1. **UI Layer**
   - Handles console input/output
   - Displays menus and messages
   - Collects user input

2. **Service Layer**
   - Contains the application logic
   - Coordinates validation, searching, updating, and reporting

3. **Data Layer**
   - Handles file storage and retrieval
   - Reads and writes device, issue, assignment, and technician data

4. **Model Layer**
   - Contains the domain classes and enumerations used throughout the system

This layered structure keeps the program organized, maintainable, and easy to extend.

![Architecture](https://raw.githubusercontent.com/sketch1996/-CS690-FinalProject-/main/docs/images/architecture.png)
MD

cd "$WIKI_DIR"
git add Design.md
git commit -m "Fix Design wiki structure and rubric coverage" || true
git push
cd ..
rm -rf "$WIKI_DIR"

echo "Wiki updated."
