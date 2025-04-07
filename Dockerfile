# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Copy the solution file
COPY VocabuApi.sln .

# Copy all project folders
COPY Vocabu.API/ ./Vocabu.API/
COPY Vocabu.BL/ ./Vocabu.BL/
COPY Vocabu.DAL/ ./Vocabu.DAL/
COPY Vocabu.Domain/ ./Vocabu.Domain/
COPY Generator.Common/ ./Generator.Common/

# Restore dependencies
RUN dotnet restore VocabuApi.sln

# Build and publish the application
WORKDIR /app/Vocabu.API
RUN dotnet publish -c Release -o /out

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app

COPY --from=build /out .

EXPOSE 80

ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
