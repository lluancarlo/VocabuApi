FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build


EXPOSE 80

# Copy everything
COPY . ./

WORKDIR /src

# Restore dependencies
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -o release_build

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview

COPY --from=build /src/release_build .

ENTRYPOINT ["dotnet", "Vocabu.API.dll"]
