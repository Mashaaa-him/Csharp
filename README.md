# Task Manager App

A modern, dark-themed SaaS **.NET MVC CRUD Application** with full **PostgreSQL database integration** and **bulk CSV data import** capabilities.

---

## 🛠️ Prerequisites

Before running this application locally, ensure you have the following installed:
*   **.NET 8.0 SDK** (Software Development Kit)
*   **PostgreSQL Server** (Active and running locally)
*   **Git**

---

## 🚀 Local Setup Instructions

Follow these step-by-step commands in your terminal to spin up the application environment:

### 1. Clone the Repository
Pull the code down to your local machine using Git:
```bash
git clone https://github.com
cd c
```

### 2. Configure Database Credentials
Open the `appsettings.json` file in your root folder and update the connection string with your local PostgreSQL user password:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=SupportDb;Username=postgres;Password=YOUR_ACTUAL_PASSWORD_HERE"
}
```

### 3. Generate the Database Schema
You do not need to manually create tables inside PostgreSQL. Run the database update command, and Entity Framework Core will instantly construct your database and task tables:
```bash
dotnet ef database update
```

### 4. Launch the Web Application
Compile the codebase and fire up the local development hosting server:
```bash
dotnet run
```

---

## 🌐 Interacting with the App

Once the terminal prints the confirmation logs, open your browser and navigate to the application dashboard:

👉 **`http://localhost:5126/Task`**

### ✨ Implemented Features
*   **Full CRUD Cycle:** Create, Read, Edit, and Delete tasks securely.
*   **Bulk CSV Import:** Upload a standard `.csv` spreadsheet file to sync tasks instantly.
*   **Persistent Theme Toggle:** Smooth CSS-driven Dark and Light mode transitions that save your preference automatically.
*   **PostgreSQL Sorting:** Server-side sorting using `.OrderBy()` to keep tasks locked in place when modified.

Bear with me, I haven't learnt Docker yet. Catch it in the next projo😁. Peace.
