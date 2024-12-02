# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante do código e compila o projeto
COPY . ./
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copia os arquivos publicados da etapa de build
COPY --from=build /out ./

# Expõe as portas da aplicação
EXPOSE 5000
EXPOSE 5001

# Define o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "BancoChu.API.dll"]
