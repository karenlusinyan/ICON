# ICON Trial Application Setup

## Requirements

- Docker (required to run infrastructure services)
- .NET SDK **8.0 or later**
- React **18 or later**
- Node.js **22.18.0 or later**
- npm **10.9.3 or later**

---

## Clone project

```sh
# Clone project from GitHub repository
git clone https://github.com/karenlusinyan/ICON.git
```

---

## Docker setup

```sh
# Check Docker installation
docker --version

# If Docker is not installed, download and install it from:
# https://www.docker.com/products/docker-desktop

# Start SQL Server container only
docker compose up -d sqlserver
```

---

## Back-End setup

```sh
# Navigate to project root
cd ICON

# Build the solution (requires .NET SDK 8.0 or later)
dotnet build

# If the .NET CLI is not installed, download and install it from:
# https://dotnet.microsoft.com/download
```

---

## Run Project (Locally)

```sh
# Open a new terminal and navigate to AuthService module
cd src/AuthService

# Check SQL Server connection password in:
# src/AuthService/appsettings.json
#
# Ensure the password matches the one used by the SQL Server Docker container.
# The password is defined in the .env file (located in the root folder).
# SQLSERVER_PASSWORD="SqlServer@2026!"
#
# Example:
# "DefaultConnection": "Server=localhost,1433;User Id=sa;Password=SqlServer@2026!;TrustServerCertificate=True;Database=<DatabaseName>"

dotnet watch
```

```sh
# Open a new terminal and navigate to TaskService module
cd src/TaskService

# Check SQL Server connection password in:
# src/TaskService/appsettings.json
#
# Ensure the password matches the one used by the SQL Server Docker container.
# The password is defined in the .env file (located in the root folder).
# SQLSERVER_PASSWORD="SqlServer@2026!"
#
# Example:
# "DefaultConnection": "Server=localhost,1433;User Id=sa;Password=SqlServer@2026!;TrustServerCertificate=True;Database=<DatabaseName>"

dotnet watch
```

---

## Front-End setup

```sh
# Check installed Node.js and npm versions
node --version
npm --version

# If Node.js / npm is not installed, download and install it from:
# https://nodejs.org/

# Open a new terminal and navigate to the front-end client application
cd frontend/client-app

# Install front-end dependencies
npm install

# Run client
npm run dev

# Open browser and navigate to
http://localhost:3000/
```

---

## Run Project (Containerized)

```sh
# Navigate to the project root directory
cd ICON

# Start all services using Docker Compose
docker compose up -d

# Open browser and navigate to:
http://host.docker.internal:3000/
```

---

## Default credentials (LOGIN)

`Username:` superadmin  
`Password:` Pa$$w0rd

---

### Windows hosts configuration (Optional)

> If the application is not reachable via  
`http://host.docker.internal:3000/`, verify that the following entry
exists in the Windows `hosts` file:

```sh
# Open hosts file with Administrator privileges
notepad.exe C:\Windows\System32\drivers\etc\hosts

# Use ONE address per hostname (loopback)
127.0.0.1   host.docker.internal

# After saving the file, flush DNS cache
# (run PowerShell as Administrator)
ipconfig /flushdns
```
