#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Koi.WebAPI/Koi.WebAPI.csproj", "Koi.WebAPI/"]
COPY ["Koi.Services/Koi.Services.csproj", "Koi.Services/"]
COPY ["Koi.DTOs/Koi.DTOs.csproj", "Koi.DTOs/"]
COPY ["Koi.Repositories/Koi.Repositories.csproj", "Koi.Repositories/"]
COPY ["Koi.BusinessObjects/Koi.BusinessObjects.csproj", "Koi.BusinessObjects/"]
RUN dotnet restore "./Koi.WebAPI/Koi.WebAPI.csproj"
COPY . .
WORKDIR "/src/Koi.WebAPI"
RUN dotnet build "./Koi.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Koi.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koi.WebAPI.dll"]