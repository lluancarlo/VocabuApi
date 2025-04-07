# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln .
COPY Vocabu.API/*.csproj ./Vocabu.API/
COPY Vocabu.BL/*.csproj ./Vocabu.BL/
COPY Vocabu.DAL/*.csproj ./Vocabu.DAL/
COPY Vocabu.Domain/*.csproj ./Vocabu.Domain/

# Restore NuGet packages
RUN dotnet restore

# Copy the full source code
COPY . .

# Build and publish the application
WORKDIR /app/Vocabu.API
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app

# Copy published output from build stage
COPY --from=build /out .

# Expose the HTTP port
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
