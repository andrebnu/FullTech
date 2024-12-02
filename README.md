# BancoChu API

Este é um projeto de API para o sistema **BancoChu**, desenvolvido com **ASP.NET Core** e **Entity Framework Core**. Ele permite a manipulação de dados para contas bancárias, transferências e feriados.

## Requisitos

Antes de começar, verifique se você tem os seguintes itens instalados:

- [Git](https://git-scm.com/)
- [Visual Studio](https://visualstudio.microsoft.com/) (ou [Visual Studio Code](https://code.visualstudio.com/) com as extensões C#)
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) ou outro banco de dados configurado (se estiver usando outro banco, ajuste a string de conexão)

## Configuração do Projeto

### 1. Clonar o Repositório

Clone o repositório para sua máquina local com o comando:

```bash
git clone (https://github.com/andrebnu/FullTech)

2. Restaurar Pacotes
No Visual Studio, abra o projeto BancoChu.API. Para restaurar todos os pacotes NuGet, clique com o botão direito do mouse no projeto e selecione Restore NuGet Packages. Alternativamente, você pode usar o terminal para restaurar os pacotes:
bash : dotnet restore

3. Configurar a String de Conexão
Abra o arquivo appsettings.json e configure a string de conexão com o banco de dados:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BancoChu;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
Altere os valores de acordo com as configurações do seu banco de dados.

4. Criar Banco de Dados
O projeto usa Entity Framework Core para gerenciar o banco de dados. Para criar as tabelas, execute a migração inicial no terminal:
bash: dotnet ef database update
Isso aplicará as migrações e criará as tabelas necessárias no banco de dados configurado.

5. Executar o Projeto
Para executar o projeto, você pode usar o Visual Studio ou a linha de comando.

No Visual Studio:

Clique em Run ou pressione F5 para iniciar o projeto em modo de depuração.
Na Linha de Comando:

Se preferir usar o terminal, execute: 
bash :dotnet run
Isso iniciará a API na URL padrão https://localhost:

6. Testando a API
Você pode testar a API usando o Swagger ou ferramentas como postman para enviar requisições HTTP para os endpoints.
## Endpoints

### **1. TransferênciaController**

#### **POST /api/transferencia**

**Descrição:**  
Este endpoint permite realizar uma transferência entre contas. A transferência só pode ser realizada em dias úteis (excluindo fins de semana e feriados).

**Requisitos:**
- A transferência deve ocorrer entre contas existentes.
- A conta de origem deve ter saldo suficiente para realizar a transferência.
- Não pode ser realizada em feriados ou fins de semana.

**Body da Requisição (JSON):**
json
{
  "ContaOrigemId": 1,
  "ContaDestinoId": 2,
  "Valor": 100.0
}
Resposta:

201 Created: Quando a transferência for bem-sucedida.
400 BadRequest: Quando ocorrer erro de validação (e.g., saldo insuficiente, conta não encontrada, fim de semana, feriado).

GET /api/transferencia/extrato
Descrição:
Esse endpoint retorna o extrato de transferências realizadas dentro de um período especificado.

Parâmetros de Query:

dataInicio (string): Data de início do período no formato "dd/MM/yyyy".
dataFim (string): Data de fim do período no formato "dd/MM/yyyy".
Exemplo de Requisição:
GET /api/transferencia/extrato?dataInicio=01/12/2023&dataFim=02/12/2023

GET /api/transferencia/{id}
Descrição:
Este endpoint retorna os detalhes de uma transferência específica.

Parâmetros:

id (int): O ID da transferência que você deseja buscar.
Exemplo de Requisição:
GET /api/transferencia/1

GET /api/conta
Descrição:
Esse endpoint retorna uma lista de todas as contas registradas no banco.

Resposta:

200 OK: Quando a lista de contas é retornada com sucesso.
500 Internal Server Error: Se ocorrer um erro no servidor ao buscar as contas.

POST /api/conta
Descrição:
Este endpoint cria uma nova conta no sistema. O número da conta deve ser único.

Erro Geral
Todos os endpoints retornam objetos de erro padrão quando algo dá errado, com a seguinte estrutura:

Exemplo de Resposta de Erro:
{
  "Message": "Erro de validação",
  "Detail": "Saldo insuficiente na conta de origem."
}
Status Codes Comuns:

200 OK: Sucesso na operação.
201 Created: Recurso criado com sucesso.
400 BadRequest: Erro de validação (dados inválidos, formato de data inválido, etc.).
404 NotFound: Recurso não encontrado.
500 Internal Server Error: Erro inesperado no servidor.



