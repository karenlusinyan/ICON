# 🔹 Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

EXPOSE 8080
# EXPOSE 8081

# Copy all .csproj files and restore as distinct layers. Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY _ICON.sln _ICON.sln
COPY src/AuthService/AuthService.csproj src/AuthService/AuthService.csproj
COPY src/Security/Security.csproj src/Security/Security.csproj
COPY src/Shared/Shared.csproj src/Shared/Shared.csproj
COPY src/TaskService/TaskService.csproj src/TaskService/TaskService.csproj

# Restore package deps
RUN dotnet restore _ICON.sln

# Copy the app folder over
COPY src/AuthService src/AuthService
COPY src/Security src/Security
COPY src/Shared src/Shared
WORKDIR /app/src/AuthService
RUN dotnet publish -c Release -o /app/src/out

# 🔹 Stage 2: Create the Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt update && apt install -y curl wget && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "AuthService.dll" ]
