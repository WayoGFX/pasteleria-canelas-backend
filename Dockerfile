# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["PasteleriaCanelas.Api/PasteleriaCanelas.Api.csproj", "PasteleriaCanelas.Api/"]
COPY ["PasteleriaCanelas.Services/PasteleriaCanelas.Services.csproj", "PasteleriaCanelas.Services/"]
COPY ["PasteleriaCanelas.Data/PasteleriaCanelas.Data.csproj", "PasteleriaCanelas.Data/"]
COPY ["PasteleriaCanelas.Domain/PasteleriaCanelas.Domain.csproj", "PasteleriaCanelas.Domain/"]

# Restaurar dependencias
RUN dotnet restore "PasteleriaCanelas.Api/PasteleriaCanelas.Api.csproj"

# Copiar todo el código fuente
COPY . .

# Build
WORKDIR "/src/PasteleriaCanelas.Api"
RUN dotnet build "PasteleriaCanelas.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "PasteleriaCanelas.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto (Railway lo asigna dinámicamente)
EXPOSE 8080

# Variable de entorno para el puerto
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PasteleriaCanelas.Api.dll"]
