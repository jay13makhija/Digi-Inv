FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DigiInv.Api/DigiInv.Api.csproj", "DigiInv.Api/"]
COPY ["DigiInv.Application/DigiInv.Application.csproj", "DigiInv.Application/"]
COPY ["DigiInv.Domain/DigiInv.Domain.csproj", "DigiInv.Domain/"]
COPY ["DigiInv.Infrastructure/DigiInv.Infrastructure.csproj", "DigiInv.Infrastructure/"]
RUN dotnet restore "./DigiInv.Api/DigiInv.Api.csproj"
COPY . .
WORKDIR "/src/DigiInv.Api"
RUN dotnet build "./DigiInv.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DigiInv.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DigiInv.Api.dll"]
