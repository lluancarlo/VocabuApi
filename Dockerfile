# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Use a directory Docker espera
WORKDIR /source

# Copia apenas arquivos de projeto e solução inicialmente
COPY VocabuApi.sln ./
COPY Vocabu.API/Vocabu.API.csproj Vocabu.API/
COPY Vocabu.BL/Vocabu.BL.csproj Vocabu.BL/
COPY Vocabu.DAL/Vocabu.DAL.csproj Vocabu.DAL/
COPY Vocabu.Domain/Vocabu.Domain.csproj Vocabu.Domain/
COPY Generator.Common/Generator.Common.csproj Generator.Common/

# Restaura dependências
RUN dotnet restore

# Copia todos os arquivos restantes
COPY . .

# Publica a aplicação
WORKDIR /source/Vocabu.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
