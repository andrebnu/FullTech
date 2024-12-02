BancoChu API
Este é um projeto de API para o sistema BancoChu, desenvolvido com ASP.NET Core e Entity Framework Core. Ele permite a manipulação de dados para contas bancárias, transferências e feriados.

Índice
Requisitos
Configuração do Projeto
1. Clonar o Repositório
2. Restaurar Pacotes
3. Configurar a String de Conexão
4. Criar o Banco de Dados
5. Executar o Projeto
6. Testar a API
Endpoints
Docker
Estrutura de Resposta de Erro
Status Codes Comuns
Requisitos
Certifique-se de ter os seguintes itens instalados:

Git
Visual Studio ou Visual Studio Code (com a extensão C#)
.NET 6 SDK
SQL Server ou outro banco de dados configurado.
Configuração do Projeto
1. Clonar o Repositório
Clone o repositório do projeto para sua máquina local:
bash : git clone https://github.com/andrebnu/FullTech

2. Restaurar Pacotes
No Visual Studio:

Abra o projeto BancoChu.API.
Clique com o botão direito do mouse no projeto e selecione Restore NuGet Packages.
Via terminal:
bash : dotnet restore

3. Configurar a String de Conexão
Abra o arquivo appsettings.json e configure a string de conexão com o banco de dados:
json : {
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BancoChu;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
Substitua os valores com as configurações do seu banco de dados.

4. Criar o Banco de Dados
Utilize o Entity Framework Core para aplicar a migração inicial:
bash : dotnet ef database update
Isso criará as tabelas no banco de dados configurado.

5. Executar o Projeto
No Visual Studio:
 Clique em Run ou pressione F5.
Via terminal: 
 bash : dotnet run
A API estará disponível em https://localhost:5001 ou http://localhost:5000.

6. Testar a API
Você pode testar os endpoints usando:

Swagger (disponível em /swagger)
Postman ou Insomnia para enviar requisições HTTP.

Endpoints
1. Transferências
POST /api/transferencia
Realiza uma transferência entre contas.

Regras:

Transferência apenas entre contas existentes.
Saldo suficiente na conta de origem.
Operação válida apenas em dias úteis.

Exemplo de Requisição:
json : {
  "ContaOrigemId": 1,
  "ContaDestinoId": 2,
  "Valor": 100.0
}

Respostas:

201 Created: Transferência bem-sucedida.
400 Bad Request: Saldo insuficiente, contas inválidas, ou operação em feriado/fim de semana.

GET /api/transferencia/extrato
Retorna o extrato de transferências realizadas em um período.

Parâmetros:

dataInicio (formato: dd/MM/yyyy)
dataFim (formato: dd/MM/yyyy)
Exemplo de Requisição:
bash : GET /api/transferencia/extrato?dataInicio=01/12/2023&dataFim=02/12/2023

GET /api/transferencia/{id}
Retorna os detalhes de uma transferência específica.

Parâmetro:
id (inteiro): ID da transferência.

Exemplo de Requisição:
bash : GET /api/transferencia/1

2. Contas
GET /api/conta
Retorna uma lista de todas as contas registradas.

Respostas:

200 OK: Lista de contas retornada com sucesso.
500 Internal Server Error: Erro no servidor.

POST /api/conta
Cria uma nova conta no sistema.

Regra: O número da conta deve ser único.

Estrutura de Resposta de Erro
Em caso de falha, os endpoints retornam objetos de erro no seguinte formato:
bash : {
  "Message": "Erro de validação",
  "Detail": "Saldo insuficiente na conta de origem."
}

Status Codes Comuns
200 OK: Operação bem-sucedida.
201 Created: Recurso criado com sucesso.
400 Bad Request: Dados inválidos ou erro de validação.
404 Not Found: Recurso não encontrado.
500 Internal Server Error: Erro inesperado no servidor.

Docker
1. Dockerfile
O Dockerfile já está configurado para build e execução com multi-stage build.

2. Docker Compose
Exemplo de configuração docker-compose.yml:
version: '3.8'

services:
  banco-chu-api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    networks:
      - banco-chu-network

networks:
  banco-chu-network:
    driver: bridge

3. Passos para Execução
Build da Imagem:
bash : docker-compose build

Iniciar os Containers:
bash : docker-compose up

A aplicação estará disponível em http://localhost:5000.

Parar os Containers:
bash : docker-compose down


