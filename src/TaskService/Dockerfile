# 🔹 Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 8080
# EXPOSE 8081

# Copy all .csproj files and restore as distinct layers. Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY ICON.sln ICON.sln
COPY src/AuthService/AuthService.csproj src/AuthService/AuthService.csproj
COPY src/AuthService.Tests/AuthService.Tests.csproj src/AuthService.Tests/AuthService.Tests.csproj
COPY src/Security/Security.csproj src/Security/Security.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj
COPY src/TaskService/TaskService.csproj src/TaskService/TaskService.csproj
COPY src/TaskService.Tests/TaskService.Tests.csproj src/TaskService.Tests/TaskService.Tests.csproj

# Restore package deps
RUN dotnet restore ICON.sln

# Copy the app folder over
COPY src/AuthService.Tests src/AuthService.Tests
COPY src/Security src/Security
COPY src/Shared src/Shared
COPY src/TaskService src/TaskService
COPY src/TaskService.Tests src/TaskService.Tests
WORKDIR /app/src/TaskService
RUN dotnet publish -c Release -o /app/src/out

# 🔹 Stage 2: Create the Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt update && apt install -y curl wget && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "TaskService.dll" ]
