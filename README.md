# API de Chamados de Suporte

API REST desenvolvida em C# com ASP.NET Core para gerenciamento de chamados de suporte técnico.

O projeto simula uma aplicação de Service Desk, permitindo registrar, consultar, atualizar, filtrar e excluir chamados, utilizando SQL Server para persistência dos dados e testes automatizados para validar as principais regras da aplicação.

## 🎯 Objetivo

Desenvolver uma API de gerenciamento de chamados aplicando conceitos de:

* C# e .NET
* ASP.NET Core Web API
* REST
* Entity Framework Core
* SQL Server
* CRUD
* DTOs
* Validação de dados
* Testes automatizados com xUnit
* Migrations
* Git e GitHub

O projeto foi desenvolvido com foco em uma aplicação prática relacionada à área de Suporte Técnico / Service Desk.

## 🛠️ Tecnologias utilizadas

* C#
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server 2022
* xUnit
* Entity Framework Core InMemory
* OpenAPI
* Docker
* Git

## 📋 Funcionalidades

A API permite:

* Criar chamados
* Listar chamados
* Consultar chamado por ID
* Atualizar chamados
* Excluir chamados
* Filtrar chamados por status
* Filtrar chamados por prioridade
* Filtrar chamados por categoria
* Combinar filtros
* Controlar automaticamente a data de fechamento
* Reabrir chamados
* Validar dados de entrada através de DTOs
* Persistir dados em SQL Server

## 🔌 Endpoints

### Criar chamado

```http
POST /api/chamados
```

Exemplo:

```json
{
  "titulo": "Computador não liga",
  "descricao": "O computador do setor administrativo não está iniciando.",
  "solicitante": "Maria",
  "categoria": "Hardware",
  "prioridade": "Alta"
}
```

Ao criar um chamado, o sistema define automaticamente:

* Status: `Aberto`
* Data de abertura: data/hora atual

### Listar chamados

```http
GET /api/chamados
```

### Filtrar por status

```http
GET /api/chamados?status=Fechado
```

### Filtrar por prioridade

```http
GET /api/chamados?prioridade=Alta
```

### Filtrar por categoria

```http
GET /api/chamados?categoria=Hardware
```

### Combinar filtros

```http
GET /api/chamados?status=Fechado&prioridade=Alta
```

Também é possível combinar os filtros de categoria:

```http
GET /api/chamados?status=Aberto&categoria=Software
```

### Consultar chamado por ID

```http
GET /api/chamados/{id}
```

Exemplo:

```http
GET /api/chamados/1
```

Caso o chamado não exista, a API retorna:

```http
404 Not Found
```

### Atualizar chamado

```http
PUT /api/chamados/{id}
```

Exemplo:

```json
{
  "titulo": "Computador não liga",
  "descricao": "Problema identificado no equipamento.",
  "solicitante": "Maria",
  "categoria": "Hardware",
  "prioridade": "Alta",
  "status": "Fechado"
}
```

Quando o chamado é alterado para `Fechado`, a API registra automaticamente a data de fechamento.

### Excluir chamado

```http
DELETE /api/chamados/{id}
```

Em caso de sucesso, a API retorna:

```http
204 No Content
```

## 🧠 Regras de negócio

O projeto possui algumas regras simples relacionadas ao fluxo de atendimento.

### Novo chamado

Todo chamado criado inicia com:

```text
Status = Aberto
```

A data de abertura é registrada automaticamente.

### Fechamento

Quando o status é alterado para:

```text
Fechado
```

a API preenche automaticamente:

```text
DataFechamento
```

### Reabertura

Caso um chamado fechado seja alterado novamente para outro status, como:

```text
Em andamento
```

a data de fechamento é removida.

## 🗄️ Banco de dados

O projeto utiliza:

* SQL Server 2022
* Entity Framework Core
* Code First
* Migrations

A principal tabela da aplicação é:

```text
Chamados
```

Com os campos:

```text
Id
Titulo
Descricao
Solicitante
Categoria
Prioridade
Status
DataAbertura
DataFechamento
```

## 🧪 Testes automatizados

O projeto possui uma suíte de testes utilizando **xUnit** e **Entity Framework Core InMemory**.

Atualmente são **10 testes automatizados**, cobrindo:

* Status inicial do chamado
* Data de abertura
* Data de fechamento inicial
* Fechamento de chamado
* Reabertura de chamado
* Filtro por status
* Filtro por prioridade
* Filtro por categoria
* Combinação de filtros
* Retorno `404 Not Found` para chamado inexistente

Para executar os testes:

```bash
dotnet test
```

Resultado esperado:

```text
Total: 10
Passed: 10
Failed: 0
```

## 📁 Estrutura do projeto

```text
ServiceDesk
│
├── ServiceDesk.slnx
│
├── ServiceDesk.Api
│   │
│   ├── Controllers
│   │   └── ChamadosController.cs
│   │
│   ├── Data
│   │   └── ServiceDeskContext.cs
│   │
│   ├── DTOs
│   │   ├── CriarChamadoRequest.cs
│   │   └── AtualizarChamadoRequest.cs
│   │
│   ├── Models
│   │   └── Chamado.cs
│   │
│   ├── Migrations
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── ServiceDesk.Api.csproj
│
└── ServiceDesk.Api.Tests
    ├── UnitTest1.cs
    └── ServiceDesk.Api.Tests.csproj
```

## ▶️ Como executar o projeto

### Pré-requisitos

É necessário ter instalado:

* .NET SDK 10
* Docker Desktop
* SQL Server 2022

### 1. Clonar o projeto

```bash
git clone https://github.com/rubia-brito/ServiceDesk.git
```

Entre na pasta:

```bash
cd ServiceDesk
```

### 2. Iniciar o SQL Server

O projeto utiliza um container Docker para o SQL Server.

Exemplo:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=ServiceDesk@1234" -p 1433:1433 --name servicedesk-sql -d mcr.microsoft.com/mssql/server:2022-latest
```

### 3. Aplicar as migrations

Entre na pasta do projeto da API:

```bash
cd ServiceDesk.Api
```

Execute:

```bash
dotnet ef database update
```

### 4. Executar a API

```bash
dotnet run
```

A API será disponibilizada localmente em:

```text
http://localhost:5252
```

## 📚 Documentação OpenAPI

Durante o desenvolvimento, a API disponibiliza o documento OpenAPI para facilitar a consulta dos endpoints.

```text
http://localhost:5252/openapi/v1.json
```

## 🧪 Executar os testes

A partir da raiz da solução:

```bash
dotnet test
```

## 💡 Aprendizados

Este projeto foi desenvolvido como prática de desenvolvimento backend com C# e .NET, permitindo aplicar conhecimentos de:

* Desenvolvimento de APIs REST
* Programação Orientada a Objetos
* Persistência de dados
* SQL Server
* Entity Framework Core
* Validação de requisições
* DTOs
* Regras de negócio
* Testes automatizados
* Docker
* Git e GitHub

## 👩‍💻 Autora

**Rúbia De Souza Brito**

Estudante de Análise e Desenvolvimento de Sistemas, com foco em C#, .NET, SQL, QA/Testes de Software e Suporte Técnico.

GitHub:
https://github.com/rubia-brito

LinkedIn:
https://www.linkedin.com/in/rubia-brito-dev/