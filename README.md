# TodoGraphQL API

API GraphQL construída com .NET 9, Hot Chocolate, MongoDB Atlas e arquitetura limpa (Clean Architecture). Inclui autenticação JWT, controle de roles, validação de inputs, rate limiting e logging estruturado.

---

## Índice

- [Visão Geral](#visão-geral)
- [Stack](#stack)
- [Arquitetura](#arquitetura)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Pré-requisitos](#pré-requisitos)
- [Configuração Local](#configuração-local)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Rodando com Docker](#rodando-com-docker)
- [GraphQL — Queries e Mutations](#graphql--queries-e-mutations)
- [Autenticação](#autenticação)
- [Roles e Permissões](#roles-e-permissões)
- [Validação](#validação)
- [Rate Limiting](#rate-limiting)
- [Logging](#logging)
- [CI/CD](#cicd)
- [Deploy](#deploy)
- [Roadmap](#roadmap)

---

## Visão Geral

API GraphQL para gerenciamento de tarefas (todos) com sistema de autenticação baseado em JWT e controle de acesso por roles. Cada usuário visualiza apenas suas próprias tarefas. Admins podem gerenciar roles de outros usuários. Usuários Business têm acesso ao painel financeiro.

---

## Stack

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 9 |
| GraphQL | Hot Chocolate 15 |
| Banco de dados | MongoDB Atlas |
| Autenticação | JWT Bearer |
| Hash de senha | BCrypt.Net |
| Validação | FluentValidation |
| Logging | Serilog |
| Containerização | Docker |
| CI/CD | GitHub Actions |
| Hospedagem | Render.com |

---

## Arquitetura

O projeto segue **Clean Architecture**, com dependências apontando sempre para dentro. O Domain não depende de nada externo.

```
┌──────────────────────────────────────┐
│            API (GraphQL)             │  depende de Application
├──────────────────────────────────────┤
│           Application                │  depende de Domain
├──────────────────────────────────────┤
│          Infrastructure              │  implementa interfaces do Application
├──────────────────────────────────────┤
│             Domain                   │  zero dependências externas
└──────────────────────────────────────┘
```

### Responsabilidades por camada

**Domain** — entidades, regras de negócio, interfaces de repositório e exceções de domínio. Nenhuma dependência externa.

**Application** — casos de uso (use cases), DTOs e interfaces de serviços. Orquestra o fluxo entre Domain e Infrastructure.

**Infrastructure** — implementações concretas dos repositórios (MongoDB), serviços (JWT, BCrypt) e contexto de banco de dados.

**API** — tipos GraphQL (Query, Mutation), inputs, validators, middlewares e configuração da aplicação (Program.cs).

### Fluxo de uma requisição

```
Cliente GraphQL
      │
      ▼
Middleware (Rate Limit, Auth)
      │
      ▼
GraphQL Type (Mutation/Query)
      │
      ▼
FluentValidation (valida input)
      │
      ▼
Use Case (Application)
      │
      ▼
Domain (regras de negócio)
      │
      ▼
Repository (Infrastructure)
      │
      ▼
MongoDB Atlas
```

---

## Estrutura de Pastas

```
TodoGraphQL/
├── src/
│   ├── TodoGraphQL.Domain/
│   │   ├── Entities/
│   │   │   ├── Todo.cs
│   │   │   ├── User.cs
│   │   │   └── DomainException.cs
│   │   └── Interfaces/
│   │       ├── ITodoRepository.cs
│   │       └── IUserRepository.cs
│   │
│   ├── TodoGraphQL.Application/
│   │   ├── DTOs/
│   │   │   ├── TodoDto.cs
│   │   │   ├── AuthDto.cs
│   │   │   └── UserDto.cs
│   │   ├── Interfaces/
│   │   │   └── ITokenService.cs
│   │   └── UseCases/
│   │       ├── Auth/
│   │       │   ├── LoginUseCase.cs
│   │       │   └── RegisterUseCase.cs
│   │       ├── Todos/
│   │       │   ├── GetTodosUseCase.cs
│   │       │   ├── AddTodoUseCase.cs
│   │       │   ├── CompleteTodoUseCase.cs
│   │       │   └── DeleteTodoUseCase.cs
│   │       └── Admin/
│   │           ├── GetUsersUseCase.cs
│   │           └── UpdateUserRoleUseCase.cs
│   │
│   ├── TodoGraphQL.Infrastructure/
│   │   ├── Persistence/
│   │   │   ├── MongoDbContext.cs
│   │   │   ├── TodoRepository.cs
│   │   │   └── UserRepository.cs
│   │   ├── Services/
│   │   │   └── TokenService.cs
│   │   └── DependencyInjection.cs
│   │
│   └── TodoGraphQL.API/
│       ├── GraphQL/
│       │   ├── Types/
│       │   │   ├── Query.cs
│       │   │   ├── Mutation.cs
│       │   │   ├── AuthMutation.cs
│       │   │   └── AdminMutation.cs
│       │   ├── Inputs/
│       │   │   ├── AddTodoInput.cs
│       │   │   ├── AuthInput.cs
│       │   │   └── UpdateRoleInput.cs
│       │   └── Validators/
│       │       ├── AddTodoValidator.cs
│       │       ├── AuthValidator.cs
│       │       ├── UpdateRoleValidator.cs
│       │       └── ValidationExtensions.cs
│       ├── Middleware/
│       │   └── AuthRateLimitMiddleware.cs
│       ├── appsettings.json
│       └── Program.cs
│
├── .github/
│   └── workflows/
│       └── ci.yml
├── Dockerfile
├── docker-compose.yml
├── Makefile
└── TodoGraphQL.sln
```

---

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [MongoDB Atlas](https://www.mongodb.com/atlas) (conta gratuita)
- `dotnet-ef` tool (se usar migrations): `dotnet tool install --global dotnet-ef`

---

## Configuração Local

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/TodoGraphQL-.git
cd TodoGraphQL-/TodoGraphQL
```

### 2. Configure as variáveis de ambiente

Crie o arquivo `src/TodoGraphQL.API/appsettings.Development.json`:

```json
{
  "MongoDB": {
    "ConnectionString": "mongodb+srv://usuario:senha@cluster.mongodb.net/?retryWrites=true&w=majority",
    "DatabaseName": "tododb"
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-minimo-32-caracteres!!",
    "Issuer": "TodoGraphQL",
    "Audience": "TodoGraphQL"
  }
}
```

> Este arquivo está no `.gitignore` e nunca deve ser commitado.

### 3. Rode a aplicação

```bash
cd src/TodoGraphQL.API
dotnet run
```

Acesse o playground GraphQL em: `http://localhost:5206/graphql`

---

## Variáveis de Ambiente

| Variável | Descrição | Exemplo |
|---|---|---|
| `MongoDB__ConnectionString` | Connection string do MongoDB Atlas | `mongodb+srv://...` |
| `MongoDB__DatabaseName` | Nome do banco de dados | `tododb` |
| `Jwt__Secret` | Chave secreta para assinar os tokens (mín. 32 chars) | `MinhaChaveSuperSecreta123!!` |
| `Jwt__Issuer` | Emissor do token JWT | `TodoGraphQL` |
| `Jwt__Audience` | Audiência do token JWT | `TodoGraphQL` |
| `ASPNETCORE_URLS` | URL que a aplicação escuta (produção) | `http://+:8080` |

> Em produção, use sempre variáveis de ambiente — nunca deixe secrets no código ou em arquivos versionados.

---

## Rodando com Docker

### Apenas subir a aplicação

```bash
# Build e sobe o container
docker build -t todo-graphql .
docker run -p 5000:8080 \
  -e MongoDB__ConnectionString="sua-connection-string" \
  -e Jwt__Secret="sua-chave-secreta" \
  -e Jwt__Issuer="TodoGraphQL" \
  -e Jwt__Audience="TodoGraphQL" \
  todo-graphql
```

### Com Makefile (recomendado)

```bash
make up      # sobe os containers em background
make down    # para os containers
make logs    # ver logs em tempo real
make reset   # para e apaga os volumes
```

---

## GraphQL — Queries e Mutations

O endpoint GraphQL é `/graphql`. Use o Banana Cake Pop (playground integrado) para testar.

### Autenticação

#### Cadastrar usuário
```graphql
mutation {
  register(input: {
    email: "usuario@email.com"
    password: "Senha123"
  }) {
    token
    email
    role
  }
}
```

#### Fazer login
```graphql
mutation {
  login(input: {
    email: "usuario@email.com"
    password: "Senha123"
  }) {
    token
    email
    role
  }
}
```

> Após o login, inclua o token em todas as requisições protegidas via header:
> `Authorization: Bearer SEU_TOKEN`

### Todos

#### Buscar tarefas do usuário logado
```graphql
query {
  todos {
    id
    title
    isCompleted
    createdAt
  }
}
```

#### Criar tarefa
```graphql
mutation {
  addTodo(input: { title: "Minha nova tarefa" }) {
    id
    title
    isCompleted
  }
}
```

#### Completar tarefa
```graphql
mutation {
  completeTodo(id: "ID_DA_TAREFA") {
    id
    isCompleted
  }
}
```

#### Deletar tarefa
```graphql
mutation {
  deleteTodo(id: "ID_DA_TAREFA")
}
```

### Admin

#### Listar todos os usuários (somente Admin)
```graphql
query {
  users {
    id
    email
    role
    createdAt
  }
}
```

#### Atualizar role de usuário (somente Admin)
```graphql
mutation {
  updateUserRole(input: {
    email: "usuario@email.com"
    role: "Business"
  })
}
```

---

## Autenticação

A autenticação usa **JWT (JSON Web Token)**. O token é gerado no login/cadastro e deve ser enviado em todas as requisições protegidas.

### Fluxo

```
1. POST /graphql → login()  → retorna JWT (válido por 8 horas)
2. Inclua nas requisições: Authorization: Bearer <token>
3. Hot Chocolate valida o token automaticamente
4. Claims do usuário ficam disponíveis via ClaimsPrincipal
```

### Claims incluídas no token

| Claim | Valor |
|---|---|
| `NameIdentifier` | ID do usuário no MongoDB |
| `Email` | Email do usuário |
| `Role` | Role do usuário (User, Business, Admin) |

---

## Roles e Permissões

O sistema possui três roles com permissões distintas:

| Role | Todos | Financeiro | Admin (usuários) |
|---|---|---|---|
| `User` | ✅ | ❌ | ❌ |
| `Business` | ✅ | ✅ | ❌ |
| `Admin` | ✅ | ✅ | ✅ |

### Promoção de roles

Somente um `Admin` pode promover outros usuários via mutation `updateUserRole`. Um usuário comum não consegue alterar sua própria role.

Para promover o primeiro Admin, altere diretamente no MongoDB Atlas:

```json
{ "$set": { "Role": "Admin" } }
```

---

## Validação

Validação ocorre em três camadas:

**1. FluentValidation (API)** — valida formato e tamanho dos inputs antes de qualquer processamento:

```
Email     → formato válido, máx. 100 chars
Password  → mín. 6 chars, pelo menos 1 maiúscula e 1 número
Title     → mín. 3 chars, máx. 200 chars, não pode ser só espaços
Role      → deve ser um valor válido do enum (User, Business, Admin)
```

**2. Use Cases (Application)** — valida regras de negócio:

```
Login     → usuário existe? senha correta?
Register  → email já cadastrado?
Complete  → tarefa pertence ao usuário?
Delete    → tarefa pertence ao usuário?
```

**3. Domain Entities** — invariantes da entidade (sempre válidas):

```
Todo.Create()  → título não pode ser vazio, userId obrigatório
User.Create()  → email não pode ser vazio
Todo.Complete() → tarefa já está completa?
```

---

## Rate Limiting

Proteção contra abuso e brute force:

| Tipo | Limite | Janela |
|---|---|---|
| Geral (todas as rotas) | 60 requisições | 1 minuto por IP |
| Auth (login + register) | 5 requisições | 1 minuto por IP |

Quando o limite é atingido, a API retorna `HTTP 429` com:

```json
{
  "errors": [{ "message": "Muitas tentativas. Aguarde 1 minuto." }]
}
```

---

## Logging

O projeto usa **Serilog** para logging estruturado. Logs são escritos no console em desenvolvimento e em arquivo em produção (`logs/log-YYYYMMDD.txt`, rotação diária, retém 7 dias).

### Níveis de log

| Nível | Quando |
|---|---|
| `Information` | Login bem-sucedido, todo criado, usuário cadastrado |
| `Warning` | Login falhou, rate limit atingido, email não encontrado |
| `Error` | Erros inesperados |
| `Fatal` | Falha na inicialização da aplicação |

### Exemplo de output

```
[14:32:01 INF] HTTP POST /graphql → 200 (42.3ms)
[14:32:01 INF] Tentativa de login para usuario@email.com
[14:32:01 INF] Login bem-sucedido para usuario@email.com | Role: Admin
[14:32:15 WRN] Rate limit atingido para IP: 177.25.3.2 | Operação: Auth
```

---

## CI/CD

O pipeline usa **GitHub Actions** e é acionado a cada push na branch `main`.

### Etapas do pipeline

```
1. Checkout do código
2. Configurar .NET 9
3. Restaurar dependências
4. Build (Release)
5. Publicar
6. Login no Docker Hub
7. Build da imagem Docker
8. Push da imagem com tag do commit
9. Tag como latest
```

### Secrets necessários no GitHub

| Secret | Descrição |
|---|---|
| `MONGODB_CONNECTION_STRING` | Connection string do Atlas |
| `JWT_SECRET` | Chave secreta JWT |
| `JWT_ISSUER` | Issuer do JWT |
| `JWT_AUDIENCE` | Audience do JWT |
| `DOCKER_USERNAME` | Usuário do Docker Hub |
| `DOCKER_PASSWORD` | Senha ou token do Docker Hub |

---

## Deploy

A API está hospedada no [Render.com](https://render.com) usando a imagem Docker publicada no Docker Hub.

### Configuração no Render

| Campo | Valor |
|---|---|
| Language | Docker |
| Branch | main |
| Root Directory | `TodoGraphQL` |
| Dockerfile Path | `./Dockerfile` |

### Variáveis de ambiente no Render

Configure as mesmas variáveis listadas na seção [Variáveis de Ambiente](#variáveis-de-ambiente) diretamente no painel do Render.

### URL de produção

```
https://todographql.onrender.com/graphql
```

---

## Roadmap

- [x] Clean Architecture
- [x] DTOs separados
- [x] Validação com FluentValidation
- [x] Rate Limiting
- [x] Logging com Serilog
- [x] Roles de usuário (User, Business, Admin)
- [ ] Índices no MongoDB (email único em users, userId em todos)
- [ ] Refresh tokens
- [ ] Testes com xUnit
