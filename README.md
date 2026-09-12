# AuthApi

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

API de autenticação e autorização construída com ASP.NET Core 10, seguindo os princípios de Clean Architecture. Utiliza MongoDB como banco de dados e JWT (JSON Web Tokens) para autenticação.

## Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MongoDB](https://www.mongodb.com/try/download/community) rodando localmente (padrão: `mongodb://localhost:27017`)

## Estrutura da Solução

```
AuthApiSolution.sln
├── AuthApi.API            → Camada de apresentação (controllers, middleware, configuração)
├── AuthApi.Application    → Camada de aplicação (serviços de negócio)
├── AuthApi.Domain         → Camada de domínio (entidades, interfaces)
└── AuthApi.Infrastructure → Camada de infraestrutura (acesso a dados, repositórios)
```

## Como Rodar

```bash
# Restaurar pacotes
dotnet restore

# Executar a API
dotnet run --project AuthApi.API
```

A API estará disponível em:

| URL | Descrição |
|-----|-----------|
| `http://localhost:5160` | API (raiz) |
| `http://localhost:5160/scalar/v1` | Documentação interativa (Scalar UI) |
| `http://localhost:5160/openapi/v1.json` | Documento OpenAPI (JSON) |

> **Importante**: A raiz `http://localhost:5160/` retorna página em branco — isso é normal. A documentação da API fica em **`/scalar/v1`**.

## Documentação da API (Scalar)

Ao executar a API em modo Development, a documentação interativa está disponível em:

```
http://localhost:5160/scalar/v1
```

O Scalar UI permite testar os endpoints diretamente pelo navegador, incluindo envio de tokens JWT para rotas autenticadas.

## Endpoints

### Autenticação

| Método | Rota | Descrição | Autenticação |
|--------|------|-----------|--------------|
| `POST` | `/Auth/register` | Registra um novo usuário | Não |
| `POST` | `/Auth/login` | Autentica e retorna um token JWT | Não |
| `POST` | `/Auth/validate` | Valida um token JWT | Não |
| `GET` | `/Auth/generate-key` | Gera uma nova chave de 32 bytes em Base64 | Não |

### Usuário

| Método | Rota | Descrição | Autenticação |
|--------|------|-----------|--------------|
| `GET` | `/User/profile` | Retorna o perfil do usuário autenticado | Sim (Bearer Token) |

### Exemplos de Uso

#### Registrar usuário

```bash
curl -X POST http://localhost:5160/Auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "wan",
    "email": "wan@example.com",
    "password": "MinhaSenh@123"
  }'
```

#### Login

```bash
curl -X POST http://localhost:5160/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "wan",
    "password": "MinhaSenh@123"
  }'
```

#### Acessar perfil (rota protegida)

```bash
curl -X GET http://localhost:5160/User/profile \
  -H "Authorization: Bearer <SEU_TOKEN_JWT>"
```

#### Gerar chave

```bash
curl -X GET http://localhost:5160/Auth/generate-key
```

## Configuração

As configurações estão em `AuthApi.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "MongoDb": "mongodb://localhost:27017"
  },
  "Jwt": {
    "Key": "zvfSCa5f4sdqQl7g9A7UdHa6xUYsOR9L/jgxQkKHGAM="
  }
}
```

- **MongoDb**: String de conexão com o MongoDB
- **Jwt:Key**: Chave secreta para assinar os tokens JWT (mínimo 32 caracteres/Base64)

> **Atenção**: Não commite chaves de produção no repositório. Use variáveis de ambiente ou User Secrets.

## Tecnologias

| Tecnologia | Versão |
|------------|--------|
| .NET | 10.0 (LTS) |
| ASP.NET Core | 10.0 |
| MongoDB.Driver | 3.11.1 |
| Microsoft.AspNetCore.OpenApi | 10.0.11 |
| Scalar (OpenAPI UI) | 2.3.0 |
| BCrypt.Net-Next | 4.0.3 |
| JWT Bearer | 10.0.11 |
| Microsoft.IdentityModel.Tokens | 8.22.0 |

## Licença

Este projeto está licenciado sob a [Licença MIT](LICENSE).
