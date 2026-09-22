# Resume Management System — ASP.NET Core MVC 👩🏻‍💻

A complete academic web application for **creating, managing, editing, deleting, and professionally displaying resumes**.

The project was developed using **ASP.NET Core MVC, C#, Razor, HTML5, CSS3, .NET/DAO, and SQL Server**, following the MVC pattern and a clear separation between presentation, controllers, models, and data-access responsibilities.

> **Project language:** The application interface and field labels are primarily in Portuguese because the project was developed as an academic assignment in Brazil. The repository documentation is written in English to make the project easier to understand as a portfolio item.

---

## Table of Contents

- [Overview](#overview)
- [Project Goals](#project-goals)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Application Flow](#application-flow)
- [Data Model](#data-model)
- [Database](#database)
- [CRUD Operations](#crud-operations)
- [Resume Sections](#resume-sections)
- [Validation](#validation)
- [Security and Data Access](#security-and-data-access)
- [User Interface](#user-interface)
- [Requirements](#requirements)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Connection String Configuration](#connection-string-configuration)
- [Running the Application](#running-the-application)
- [Testing the Main Flow](#testing-the-main-flow)
- [Troubleshooting](#troubleshooting)
- [Academic Requirements Covered](#academic-requirements-covered)
- [Possible Improvements](#possible-improvements)
- [Learning Outcomes](#learning-outcomes)
- [Git and Repository Organization](#git-and-repository-organization)
- [Author](#author)
- [License](#license)

---

## Overview

The **Resume Management System** is a web-based CRUD application designed to store candidate information and present it as a professional resume.

The application was developed to satisfy an academic MVC assignment that requires a main menu and the following operations:

- list existing resumes;
- create a new resume;
- edit an existing resume;
- delete a resume;
- display a formatted resume using HTML and CSS.

The solution uses **one SQL Server table** for the complete resume record, as permitted by the assignment.

Repeated sections such as education, professional experience, and languages are represented by a fixed maximum number of fields.

---

## Project Goals

The main goals of the project are:

1. Apply the **Model-View-Controller (MVC)** pattern in a real web application.
2. Build a complete CRUD flow using **C# and ASP.NET Core**.
3. Separate SQL/database responsibilities into a dedicated **DAO** class.
4. Persist application data in **SQL Server**.
5. Use Razor Views with **HTML5 and CSS3** for the presentation layer.
6. Apply server-side validation through **Data Annotations**.
7. Present stored information in a readable and professional resume layout.
8. Demonstrate practical integration between a web application and a relational database.

---

## Features

### Resume Management

- List all registered resumes.
- Display **CPF, name, and intended position** in the main listing.
- Register a new resume.
- Edit resume information.
- Delete a resume with confirmation.
- Open a dedicated, formatted resume view.
- Print the resume directly from the browser.

### Resume Content

The system supports:

- Personal information.
- Professional objective.
- Desired position.
- Salary expectation.
- LinkedIn profile.
- City and address.
- Contact information.
- Up to **5 academic education/course records**.
- Up to **3 professional experience records**.
- Up to **3 languages**, including proficiency levels.

### Data Validation

- Required name and CPF.
- Email format validation.
- Maximum string lengths.
- Salary range validation.
- CPF uniqueness validation before insert/update.
- Database-level unique constraint on CPF.
- Anti-forgery validation for form submissions.

### User Experience

- Clear navigation between application screens.
- Success and error feedback through `TempData`.
- Responsive CSS layout.
- Separate screen for resume visualization.
- Print-friendly resume presentation.
- Empty optional records are omitted from the formatted resume.

---

## Technology Stack

| Technology | Purpose |
|---|---|
| **C#** | Application and business logic |
| **ASP.NET Core 8 MVC** | Web application framework |
| **.NET 8** | Application runtime and target framework |
| **Razor Views** | Server-side HTML rendering |
| **HTML5** | Page structure |
| **CSS3** | Styling and responsive presentation |
| **SQL Server** | Relational database |
| **Microsoft.Data.SqlClient** | SQL Server connectivity |
| **ADO.NET** | Database operations |
| **DAO Pattern** | Data-access separation |
| **Visual Studio** | Development environment |
| **Git / GitHub** | Source control and portfolio hosting |

---

## Architecture

The application follows the **MVC architecture** and uses a dedicated DAO layer for database operations.

```text
┌───────────────────────────────┐
│          Razor Views          │
│        HTML + CSS + UI        │
└───────────────┬───────────────┘
                │
                ▼
┌───────────────────────────────┐
│         Controllers           │
│ Request handling + validation │
└───────────────┬───────────────┘
                │
                ▼
┌───────────────────────────────┐
│             DAO               │
│   ADO.NET + parameterized SQL │
└───────────────┬───────────────┘
                │
                ▼
┌───────────────────────────────┐
│          SQL Server           │
│       CurriculoDB / table     │
└───────────────────────────────┘
```

### MVC Responsibilities

#### Model

`Models/Curriculo.cs` represents the resume data structure and contains validation metadata using `System.ComponentModel.DataAnnotations`.

#### View

Razor files under `Views/` render forms, tables, confirmation dialogs, navigation, and the final formatted resume.

#### Controller

`Controllers/CurriculoController.cs` handles HTTP requests, coordinates validation, calls the DAO, and selects the appropriate View.

#### DAO

`DAO/CurriculoDAO.cs` contains all database access operations, keeping SQL queries out of the controllers and views.

---

## Project Structure

```text
CurriculoMVC/
│
├── Controllers/
│   ├── CurriculoController.cs
│   └── HomeController.cs
│
├── DAO/
│   └── CurriculoDAO.cs
│
├── Database/
│   └── script.sql
│
├── Models/
│   └── Curriculo.cs
│
├── Properties/
│   └── launchSettings.json
│
├── Views/
│   ├── Curriculo/
│   │   ├── Create.cshtml
│   │   ├── Delete.cshtml
│   │   ├── Details.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Index.cshtml
│   │   └── _Form.cshtml
│   │
│   ├── Home/
│   │   └── Index.cshtml
│   │
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _ValidationScriptsPartial.cshtml
│   │   └── Error.cshtml
│   │
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
│
├── wwwroot/
│   └── css/
│       └── site.css
│
├── appsettings.json
├── appsettings.Development.json
├── CurriculoMVC.csproj
├── CurriculoMVC.sln
├── Program.cs
├── .gitignore
└── README.md
```

---

## Application Flow

The default route is configured as:

```text
{controller=Home}/{action=Index}/{id?}
```

The typical user flow is:

```text
Home
  │
  ├── List Resumes
  │      │
  │      ├── View Resume
  │      ├── Edit Resume
  │      └── Delete Resume
  │
  └── Create Resume
```

### Main CRUD Flow

```text
CREATE
User → Create View → CurriculoController → CurriculoDAO → SQL Server

READ
User → Index/Details → CurriculoController → CurriculoDAO → SQL Server

UPDATE
User → Edit View → CurriculoController → CurriculoDAO → SQL Server

DELETE
User → Delete Confirmation → CurriculoController → CurriculoDAO → SQL Server
```

---

## Data Model

The project intentionally uses **a single table**, matching the flexibility provided by the academic assignment.

### Core Fields

| Field | Description | Required |
|---|---|---:|
| `Id` | Unique database identifier | Automatic |
| `Nome` | Full name | Yes |
| `CPF` | Brazilian CPF identifier | Yes |
| `Endereco` | Address | No |
| `Telefone` | Phone number | No |
| `Email` | Email address | No |
| `PretensaoSalarial` | Expected salary | No |
| `CargoPretendido` | Desired position | No |
| `Objetivo` | Professional objective | No |
| `LinkedIn` | LinkedIn profile URL | No |
| `Cidade` | City | No |

### Education

The model supports up to five education/course entries:

```text
Formacao1
Formacao2
Formacao3
Formacao4
Formacao5
```

### Professional Experience

The model supports up to three professional experiences.

Each experience contains:

- company;
- position;
- period;
- description.

Fields:

```text
Empresa1 / Cargo1 / Periodo1 / DescricaoExperiencia1
Empresa2 / Cargo2 / Periodo2 / DescricaoExperiencia2
Empresa3 / Cargo3 / Periodo3 / DescricaoExperiencia3
```

### Languages

The model supports up to three languages with proficiency levels:

```text
Idioma1 / NivelIdioma1
Idioma2 / NivelIdioma2
Idioma3 / NivelIdioma3
```

---

## Database

The database script is located at:

```text
Database/script.sql
```

The script:

- creates the `CurriculoDB` database when it does not exist;
- switches to the `CurriculoDB` database;
- creates the `Curriculo` table when it does not exist;
- defines the primary key;
- defines CPF as a unique value;
- includes an optional example insert for testing.

### Database Name

```text
CurriculoDB
```

### Table Name

```text
Curriculo
```

### Primary Key

```sql
Id INT IDENTITY(1,1) PRIMARY KEY
```

### CPF Uniqueness

```sql
CPF VARCHAR(14) NOT NULL UNIQUE
```

This protects the database against duplicate CPF registrations in addition to the application-level validation.

---

## CRUD Operations

The DAO contains the main persistence operations:

| DAO Method | Purpose |
|---|---|
| `Listar()` | Returns all resumes ordered by name |
| `BuscarPorId(int id)` | Returns one resume by ID |
| `ExisteCpf(string cpf, int? idIgnorar = null)` | Checks CPF uniqueness |
| `Inserir(Curriculo c)` | Inserts a new resume |
| `Atualizar(Curriculo c)` | Updates an existing resume |
| `Excluir(int id)` | Deletes a resume |

The controller maps these operations to MVC actions such as:

- `Index`
- `Create`
- `Edit`
- `Details`
- `Delete`

---

## Resume Sections

The final formatted resume is designed to resemble a real professional document rather than a raw database record.

### Header

The header displays:

- full name;
- contact information;
- desired position.

### Professional Objective

The candidate's professional objective is displayed when data is available.

### Education

Only non-empty education records are displayed.

### Professional Experience

Each non-empty experience is presented with:

- company;
- role;
- period;
- description.

### Languages

Only completed language entries are rendered.

### Additional Information

Additional information such as LinkedIn, city, salary expectations, and contact information may be displayed when provided.

---

## Validation

The model uses Data Annotation attributes such as:

```csharp
[Required]
[StringLength]
[EmailAddress]
[Range]
```

Example:

```csharp
[Required(ErrorMessage = "Informe o nome.")]
[StringLength(150)]
public string Nome { get; set; }
```

Example email validation:

```csharp
[EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
[StringLength(150)]
public string? Email { get; set; }
```

Example salary validation:

```csharp
[Range(0, 999999999.99)]
public decimal? PretensaoSalarial { get; set; }
```

### CPF Validation

CPF uniqueness is checked before insert and update.

```text
Create → check CPF → validate model → insert

Edit   → check CPF → validate model → update
```

The database also contains a `UNIQUE` constraint for CPF, providing an additional level of protection.

---

## Security and Data Access

Although this is an academic project, several good development practices were included.

### Parameterized SQL

Database queries use SQL parameters instead of string concatenation.

Example:

```csharp
command.Parameters.AddWithValue("@Id", id);
```

Parameterized queries reduce the risk of SQL injection and provide a cleaner separation between SQL commands and user-provided data.

### Anti-Forgery Validation

Form submission actions use:

```csharp
[ValidateAntiForgeryToken]
```

This adds protection against Cross-Site Request Forgery (CSRF) attacks for POST requests.

### Database Constraints

CPF is marked as `UNIQUE` at the database level.

### Configuration Separation

The SQL Server connection string is stored in `appsettings.json` and loaded through configuration/dependency injection.

> For a production environment, secrets and database credentials should be managed through secure secret storage instead of being committed to source control.

---

## User Interface

The front end uses:

- Razor;
- HTML5;
- CSS3.

### Main Screens

| Screen | Purpose |
|---|---|
| Home | Application entry point and navigation |
| Resume List | Displays CPF, name and desired position |
| Create | Creates a new resume |
| Edit | Updates an existing resume |
| Delete | Confirms and performs deletion |
| Details | Displays the professional resume layout |

---

## Requirements

Before running the application, install:

1. **Visual Studio 2022** with ASP.NET and web development support, or another compatible .NET development environment.
2. **.NET 8 SDK**.
3. **SQL Server LocalDB** or another SQL Server instance.
4. Optionally, **SQL Server Management Studio (SSMS)** for database administration.

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR-USERNAME/resume-management-system.git
cd resume-management-system/CurriculoMVC
```

Replace:

```text
YOUR-USERNAME
```

with your GitHub username.

### 2. Restore Dependencies

From the directory containing `CurriculoMVC.csproj`:

```bash
dotnet restore
```

### 3. Build the Application

```bash
dotnet build
```

If the build finishes successfully, the application is ready to run.

---

## Database Setup

### Option A — SQL Server LocalDB

The default configuration uses:

```text
Server=(localdb)\MSSQLLocalDB;
Database=CurriculoDB;
Trusted_Connection=True;
TrustServerCertificate=True;
```

Run the script:

```text
Database/script.sql
```

using SQL Server Management Studio or another compatible SQL client.

### Option B — SQL Server Express

If you use SQL Server Express, a common server name is:

```text
.\SQLEXPRESS
```

Update the connection string accordingly.

### Option C — SQL Server on localhost

Example:

```text
localhost
```

The final connection string depends on the SQL Server instance and authentication method configured on your machine.

---

## Connection String Configuration

The default `appsettings.json` configuration is:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CurriculoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

The DAO retrieves the configured connection string through dependency injection/configuration.

Example:

```csharp
public CurriculoDAO(IConfiguration configuration)
{
    _connectionString = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not configured.");
}
```

### Important

Never commit sensitive credentials such as:

- database passwords;
- API keys;
- tokens;
- production credentials.

For local development, consider environment variables, .NET User Secrets, or another secure configuration mechanism when credentials are required.

---

## Running the Application

### Visual Studio

Open:

```text
CurriculoMVC.sln
```

Then run using:

```text
F5
```

or:

```text
Ctrl + F5
```

### Terminal

From the folder containing:

```text
CurriculoMVC.csproj
```

run:

```bash
dotnet run
```

The terminal will display the local application URL, for example:

```text
Now listening on: https://localhost:xxxx
```

Open the displayed URL in your browser.

---

## Testing the Main Flow

A recommended manual test sequence is:

### Test 1 — Create Resume

1. Open the application.
2. Select **Novo Currículo**.
3. Fill in the required fields.
4. Add at least one education record.
5. Optionally add professional experiences and languages.
6. Save the resume.

Expected result:

```text
Currículo cadastrado com sucesso.
```

### Test 2 — List Resumes

Open the resume list and verify that:

- CPF appears;
- name appears;
- desired position appears when available;
- action buttons are displayed.

### Test 3 — View Resume

Open **Exibir** and verify that the information is displayed in the professional resume layout.

### Test 4 — Edit Resume

Open **Alterar**, modify one or more fields, save, and verify that the updated values appear.

### Test 5 — Delete Resume

Open **Excluir**, confirm the deletion, and verify that the record disappears from the list.

### Test 6 — Duplicate CPF

Try registering the same CPF twice.

Expected behavior:

```text
Já existe um currículo cadastrado com este CPF.
```

---

## Troubleshooting

### Database Connection Error

If the application cannot connect to SQL Server:

1. Confirm that SQL Server or LocalDB is installed.
2. Confirm that the server name in `appsettings.json` is correct.
3. Confirm that `CurriculoDB` exists.
4. Execute `Database/script.sql`.
5. Restart the application.

### `Invalid object name 'Curriculo'`

The database may exist but the table does not.

Run:

```text
Database/script.sql
```

### `Cannot open database 'CurriculoDB'`

The database may have been created on a different SQL Server instance from the one referenced in the connection string.

Make sure both the application and the SQL script use the same SQL Server instance.

### Port Already in Use

Stop the previous application instance or run the project again with another launch profile/port.

### NuGet Restore Problems

Run:

```bash
dotnet restore
```

Then:

```bash
dotnet build
```

### `dotnet` Command Not Found

Install the **.NET 8 SDK** and restart the terminal or Visual Studio.

---

## Academic Requirements Covered

The project was designed to address the main requirements of the assignment.

| Requirement | Status |
|---|---|
| MVC architecture | ✅ |
| HTML formatting | ✅ |
| CSS formatting | ✅ |
| Main menu | ✅ |
| List existing resumes | ✅ |
| Display CPF and name | ✅ |
| Create resume | ✅ |
| Edit resume | ✅ |
| Delete resume | ✅ |
| Formatted resume display | ✅ |
| Professional visual presentation | ✅ |
| Academic education records | ✅ Up to 5 |
| Professional experiences | ✅ Up to 3 |
| Languages | ✅ Up to 3 |
| Optional fields | ✅ |
| Single database table | ✅ |
| Database creation script | ✅ |
| Script included in DAO as requested | ✅ |

### Important Academic Note

The assignment states that at least one education record should be required.

The current structure supports up to five education records, but this specific cross-field requirement may need additional custom validation depending on how strictly the instructor checks it.

---

## Possible Improvements

Although the project was developed according to the academic scope, it can be expanded into a more production-oriented application.

### Database Normalization

The current one-table structure was chosen to satisfy the assignment.

A production system could normalize the database into separate tables:

```text
Curriculo
   │
   ├── Formacoes
   ├── Experiencias
   └── Idiomas
```

This would remove the fixed limits and allow an arbitrary number of records.

### Authentication and Authorization

A future version could include:

- ASP.NET Core Identity;
- login and logout;
- role-based access control;
- user-specific resumes.

### API Layer

The application could expose REST API endpoints for integration with other systems.

### Advanced Validation

Possible improvements include:

- CPF digit validation;
- phone formatting;
- Brazilian currency formatting;
- LinkedIn URL validation;
- stronger cross-field validation.

### Deployment

Possible deployment targets include:

- Azure App Service;
- Azure SQL Database;
- IIS;
- Docker;
- other cloud platforms supporting ASP.NET Core.

### Automated Testing

A future version could add:

- unit tests;
- DAO integration tests;
- controller tests;
- end-to-end UI tests.

---

## Learning Outcomes

This project provides practical experience with:

- MVC web application architecture;
- HTTP requests and controller actions;
- Razor syntax;
- server-side rendering;
- model binding;
- Data Annotations;
- dependency injection;
- ADO.NET;
- SQL Server;
- parameterized SQL;
- CRUD operations;
- HTML/CSS development;
- Git and GitHub;
- separation of responsibilities between application layers.

The project demonstrates a complete flow from:

```text
User Input
    ↓
Razor View
    ↓
Controller
    ↓
DAO
    ↓
SQL Server
    ↓
DAO
    ↓
Controller
    ↓
Razor View
```

---

## Git and Repository Organization

The repository intentionally excludes generated .NET files such as:

```text
bin/
obj/
.vs/
```

These directories are covered by the `.gitignore` file and should not be committed to source control.

Recommended repository structure:

```text
resume-management-system/
│
├── CurriculoMVC/
│   ├── Controllers/
│   ├── DAO/
│   ├── Models/
│   ├── Views/
│   ├── wwwroot/
│   ├── Database/
│   ├── Properties/
│   ├── .gitignore
│   ├── CurriculoMVC.csproj
│   ├── CurriculoMVC.sln
│   ├── Program.cs
│   ├── appsettings.json
│   └── README.md
```

Recommended Git workflow:

```bash
git add .
git commit -m "Initial resume management system"
git push origin main
```

---

## Author

**Letícia Marin Reis**

GitHub:

```text
https://github.com/leticiamarinreis
```

---

## License

This project was created for **academic and educational purposes**.

---

## Final Notes

This repository contains the complete academic implementation of a resume management system using:

- ASP.NET Core MVC;
- C#;
- Razor;
- HTML5;
- CSS3;
- ADO.NET;
- DAO Pattern;
- SQL Server.

The project demonstrates a complete application flow:

```text
Form Input
    ↓
Controller
    ↓
DAO
    ↓
SQL Server
    ↓
Controller
    ↓
Razor View
```

The application combines CRUD functionality, relational database integration, MVC architecture, server-side validation, parameterized SQL, and professional resume presentation in a single academic project.
