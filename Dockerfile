# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file
COPY . ./

# Restore dependencies
RUN dotnet restore VocabuApi.sln

# Build and publish
WORKDIR /app/Vocabu.API
RUN dotnet publish -c Release -o /out

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
