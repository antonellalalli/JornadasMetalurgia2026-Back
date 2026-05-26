# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto específicamente
# Ajusta el nombre si tu archivo tiene otro nombre exacto
COPY ["Jornadas-Metalurgia-2026.csproj", "./"]

# Restaurar dependencias
RUN dotnet restore "Jornadas-Metalurgia-2026.csproj"

# Copiar todo el resto del código
COPY . .

# Publicar la aplicación
RUN dotnet publish "Jornadas-Metalurgia-2026.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Jornadas-Metalurgia-2026.dll"]