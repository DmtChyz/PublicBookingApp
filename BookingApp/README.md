# BookingApp - Full-Stack Event Booking Platform

**A complete, production-grade, full-stack application featuring a .NET 8 backend API built on Clean Architecture, and a responsive React frontend.**

[![Live Demo](https://img.shields.io/badge/Live-Demo-brightgreen?style=for-the-badge&logo=vercel)](https://vite-project-nine-iota.vercel.app)
[![Backend API](https://img.shields.io/badge/Backend-API-blue?style=for-the-badge&logo=azure-pipelines)](https://api20260106040424-cefehuf4hfgrgybr.germanywestcentral-01.azurewebsites.net/swagger)

---

** Main page screenshot link
https://www.canva.com/design/DAG9y1pIp3o/-mVCn8_A0YfHoG1Myggo0w/edit?utm_content=DAG9y1pIp3o&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton

## About This Project

BookingApp is a comprehensive event booking platform built from the ground up. As my first major project, the primary objective was to master and integrate a professional-grade, full-stack technology stack, including automated CI/CD pipelines for a real-world deployment workflow. The application allows users to browse, book, and manage events, while providing a foundation for event contributors to manage their own listings.

## Architecture & Key Patterns

- **Clean Architecture:** The backend is logically layered (Domain, Application, Infrastructure, API) to ensure separation of concerns and maintainability.
- **CQRS (Command Query Responsibility Segregation):** MediatR is used to separate write operations (Commands) from read operations (Queries), simplifying the application logic.
- **Custom Middleware:** Centralized exception handling provides consistent and clean error responses across the entire API.
- **Repository & Unit of Work Patterns:** Abstract the data layer to enable testability and manage database transactions efficiently.
- **Service Layer:** Encapsulates business logic, acting as a bridge between the API controllers and the application core.
- **Server State Management:** The frontend uses TanStack Query to manage caching, background refetching, and optimistic updates for a smooth and responsive user experience.

---

## Tech Stack

### Backend
| Technology | Purpose |
| :--- | :--- |
| **.NET 8** | Core API Framework |
| **Entity Framework Core** | ORM for Database Interaction |
| **SQL Server** | Relational Database |
| **MediatR** | CQRS Pattern Implementation |
| **FluentValidation** | Advanced Data Validation |
| **AutoMapper** | Object-to-Object Mapping |
| **Brevo** | Email Services for Password Reset |
| **JWT** | Authentication & Authorization |

### Frontend
| Technology | Purpose |
| :--- | :--- |
| **React** | Core UI Library |
| **Vite** | Build Tool & Dev Server |
| **TypeScript** | Static Typing |
| **React Router** | Client-Side Routing |
| **TanStack Query** | Server State Management & Caching |
| **Axios** | HTTP Client for API Communication |
| **CSS Modules** | Component-Scoped Styling |

### Deployment & DevOps
| Technology | Purpose |
| :--- | :--- |
| **Azure App Service** | Hosting for the .NET Backend |
| **Azure SQL Database** | Cloud Database Hosting |
| **Vercel** | Hosting for the React Frontend |
| **GitHub Actions** | CI/CD for Automated Backend Deployment |

---

## Features

- **User Authentication:** Secure user registration, login, and JWT-based session management.
- **Role-Based Authorization:** Backend is architected with a role system (e.g., User, Admin) to control access to different API endpoints.
- **Password Reset:** Fully implemented "forgot password" flow via Brevo email integration.
- **Event Browsing & Management:** Publicly accessible, sortable event list, with the ability for event creators to delete their events (conditional on no existing bookings).
- **Event Booking:** Authenticated users can book a specified number of seats for an event.
- **Profile Management:** Users can view their booking history and edit their own profile information.
- **Static Content Pages:** Includes standard informational pages like 'About Us' and 'For Contributors'.
- **Fully Responsive UI:** The application is designed to be fully functional and visually appealing on both desktop and mobile devices.

---

## Local Setup & Installation

To run this project on your local machine, you will need to set up both the backend and the frontend.

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en) (v18 or later)
- An SQL Server instance (e.g., SQL Server Express or Docker)
- A Brevo account for email functionality (optional)

### Backend Setup

1.  **Clone the backend repository:**
    ```bash
    git clone [URL_OF_YOUR_BACKEND_REPO]
    cd [backend-repo-name]
    ```
2.  **Configure User Secrets:**
    - Navigate to the `API` project directory.
    - Set up the database connection string and JWT settings using the .NET Secret Manager:
      ```bash
      dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your_SQL_Server_Connection_String"
      dotnet user-secrets set "JwtSettings:Secret" "Your_Super_Secret_Key_For_JWT_Tokens"
      dotnet user-secrets set "JwtSettings:Issuer" "Your_Issuer"
      dotnet user-secrets set "JwtSettings:Audience" "Your_Audience"
      ```
3.  **Apply Migrations & Seed Database:**
    - Run the application. The database migrations will be applied, and the database will be seeded with initial data automatically on the first run.
    ```bash
    dotnet run --project API
    ```
4.  The backend API will be running at `https://localhost:XXXX`.

### Frontend Setup

1.  **Clone the frontend repository:**
    ```bash
    git clone [URL_OF_YOUR_FRONTEND_REPO]
    cd [frontend-repo-name]
    ```
2.  **Install dependencies:**
    ```bash
    npm install
    ```
3.  **Create an environment file:**
    - Create a new file named `.env.local` in the root of the project.
    - Add the URL of your local backend API:
      ```
      VITE_API_BASE_URL=https://localhost:XXXX (XXXX - your port)
      ```
      *(Replace XXXX with the port your local backend is running on)*
4.  **Run the application:**
    ```bash
    npm run dev
    ```
5.  The frontend will be available at `http://localhost:XXXX`. (XXXX - your port)
