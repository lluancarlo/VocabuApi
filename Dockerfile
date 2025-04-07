# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only csproj files first to leverage Docker caching
COPY VocabuApi.sln ./
COPY Vocabu.API/Vocabu.API.csproj Vocabu.API/
COPY Vocabu.BL/Vocabu.BL.csproj Vocabu.BL/
COPY Vocabu.DAL/Vocabu.DAL.csproj Vocabu.DAL/
COPY Vocabu.Domain/Vocabu.Domain.csproj Vocabu.Domain/
COPY Generator.Common/Generator.Common.csproj Generator.Common/

# Restore dependencies
RUN dotnet restore VocabuApi.sln

# Now copy all project files
COPY . .

# Publish the API
WORKDIR /src/Vocabu.API
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app

COPY --from=build /app ./

EXPOSE 80
ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
