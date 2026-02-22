# Fleet Management System

A fleet management web application built to demonstrate skills and patterns acquired in my previous role as a software developer.

## Overview

Fleet Management System allows users to manage a vehicle fleet through a clean interface. Each vehicle has a detail page showing its components and driver assignment history, all editable inline via DevExtreme data grids.

### App Preview
*Below are some screenshots of the application in action:*

**1. Vehicles Dashboard**
![Vehicles dashboard displaying a data grid with a list of fleet vehicles, including columns for vehicle ID, make, model, year, and status. The interface shows inline editing capabilities with action buttons for adding, editing, and deleting vehicles. The layout uses a clean, professional design with DevExtreme components on a light background.](docs/vehicles.png)

**2. Master-Detail View (Vehicle Components)**
![Vehicle Components Tab](docs/details-components.png)

**3. Master-Detail View (Driver History)**
![Driver History Tab](docs/details-drivers.png)

## Features

- Vehicle list with inline add, edit, and delete
- Vehicle detail page with:
  - General information (read-only form)
  - Vehicle components management
  - Driver assignment history
- Data seeded automatically on first run with realistic sample data

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 10 MVC |
| ORM | Entity Framework Core (SQLite) |
| UI Components | DevExtreme (jQuery) |
| Mapping | AutoMapper |
| Architecture | Service layer + DTOs |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A licensed copy of [DevExtreme](https://js.devexpress.com/) (jQuery version)

### DevExtreme Setup

DevExtreme libraries are not included in this repository due to licensing restrictions.
After cloning, copy your DevExtreme distribution files into the following folders:
```text
Fleet_Management_System/wwwroot/
├── css/
│   └── devextreme/
│       └── dx.light.css (and other required css/icons)
└── js/
    └── devextreme/
        ├── dx.all.js
        ├── dx.aspnet.data.js
        └── dx.aspnet.mvc.js