# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution file
COPY VocabuApi.sln ./

# Copy each project’s .csproj to leverage Docker caching
COPY Vocabu.API/Vocabu.API.csproj ./Vocabu.API/
COPY Vocabu.BL/Vocabu.BL.csproj ./Vocabu.BL/
COPY Vocabu.DAL/Vocabu.DAL.csproj ./Vocabu.DAL/
COPY Vocabu.Domain/Vocabu.Domain.csproj ./Vocabu.Domain/
COPY Generator.Common/Generator.Common.csproj ./Generator.Common/

# Restore dependencies
RUN dotnet restore VocabuApi.sln

# Copy the rest of the source code
COPY . .

# Build and publish
WORKDIR /app/Vocabu.API
RUN dotnet publish -c Release -o /out

# Stage 2: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /out .

EXPOSE 80
ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
