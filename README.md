# Fleet Management System

A fleet management web application built to demonstrate skills and patterns from a previous role as a junior full-stack developer. The UI was migrated to Blazor + MudBlazor for an open, license-free stack while keeping the same domain model and service-layer architecture.

## Overview

Fleet Management System lets you manage a vehicle fleet through a clean interface. Each vehicle has a detail page showing its components and driver assignment history, editable via MudBlazor data grids and dialogs.

### App Preview
*Below are some screenshots of the application in action:*

**1. Vehicles Dashboard**
![Vehicles dashboard](docs/vehicles.png)

**2. Master-Detail View (Vehicle Components)**
![Vehicle Components Tab](docs/details-components.png)

**3. Master-Detail View (Driver History)**
![Driver History Tab](docs/details-drivers.png)

## Features

- Vehicle list with add, edit, and delete (dialog-based)
- Vehicle detail page with:
  - General information (read-only form)
  - Vehicle components management
  - Driver assignment history
- Data seeded automatically on first run with realistic sample data

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 Blazor (Interactive Server) |
| ORM | Entity Framework Core (SQLite) |
| UI Components | MudBlazor |
| Mapping | AutoMapper |
| Architecture | Service layer + DTOs |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
cd "Fleet Management System"
dotnet restore
dotnet run --launch-profile http
```

Open [http://localhost:5090](http://localhost:5090). On first start the app applies EF migrations and seeds sample vehicles, components, and driver assignments.
