# Etapa 1: Construccion
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos
COPY . .

# Restaura las dependecias
RUN dotnet restore

# Publica el proyecto en modo Release
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Ejecucion
FROM  mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia los archivos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Expone el puerto utilizado por la aplicacion
EXPOSE 5001
EXPOSE 80

#Comando para iniciar la aplicacion
ENTRYPOINT ["dotnet","FonosoftAPI.dll"]