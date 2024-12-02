# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia os arquivos do projeto
COPY . ./

# Restaura as dependências e compila o projeto
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /out ./

# Expõe a porta da aplicação
EXPOSE 5000
EXPOSE 5001

# Define o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "BancoChu.API.dll"]
