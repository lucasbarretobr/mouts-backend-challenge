# Ambev Developer Evaluation

API REST para gerenciamento de usuários e vendas, desenvolvida em .NET 8.

## Dependências

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) com Docker Compose
- Git
- Opcional: Visual Studio 2022 ou superior, com a carga de trabalho **ASP.NET e desenvolvimento Web**

O Docker Compose utiliza PostgreSQL, Kafka, Kafka UI, MongoDB e Redis. A API usa PostgreSQL como banco relacional principal e Kafka para mensageria.

## Como executar localmente

A forma mais simples é executar a infraestrutura e a API com Docker Compose:

1. Clone o repositório e entre na pasta do backend:

   ```powershell
   git clone https://github.com/lucasbarretobr/mouts-backend-challenge.git
   cd mouts-backend-challenge\template\backend
   ```

2. Inicie os serviços:

   ```powershell
   docker compose up --build
   ```

3. Acesse a documentação da API em <http://localhost:8080/swagger>.
   O health check está em <http://localhost:8080/health> e a Kafka UI em <http://localhost:8082>.

4. Para parar os serviços, pressione `Ctrl+C` ou execute:

   ```powershell
   docker compose down
   ```

As migrações do Entity Framework Core são aplicadas automaticamente na inicialização da API. Para apagar também os dados persistidos e recriar os containers, use `docker compose down -v`.

### Executar a API fora do Docker

Com o Docker Desktop em execução, inicie o banco e execute a API:

```powershell
cd mouts-backend-challenge\template\backend
docker compose up -d ambev.developerevaluation.database
dotnet restore Ambev.DeveloperEvaluation.sln
dotnet run --project src\Ambev.DeveloperEvaluation.WebApi\Ambev.DeveloperEvaluation.WebApi.csproj --launch-profile http
```

Nesse modo, a API fica disponível em <http://localhost:5119/swagger>. A configuração local aponta para o PostgreSQL na porta `5433`. Use `docker compose up -d` caso também sejam necessários Kafka, MongoDB ou Redis.

## Testes

Para restaurar dependências, compilar e executar todos os testes:

```powershell
cd mouts-backend-challenge\template\backend
dotnet test Ambev.DeveloperEvaluation.sln
```

Os testes estão organizados em `Unit`, `Functional` e `Integration`, dentro da pasta `template/backend/tests`.

Para executar apenas os testes unitários:

```powershell
dotnet test tests\Ambev.DeveloperEvaluation.Unit\Ambev.DeveloperEvaluation.Unit.csproj
```

## Banco de dados

O projeto utiliza **PostgreSQL 18**, acessado com Entity Framework Core e Npgsql. No Docker Compose, a aplicação usa `ambev.developerevaluation.database:5432`; a máquina local usa `localhost:5433`.

| Item | Valor |
|---|---|
| Banco | `developer_evaluation` |
| Usuário | `developer` |
| Senha local | `ev@luAt10n` |

A string de conexão pode ser substituída por `ConnectionStrings__DefaultConnection`. Não utilize as credenciais padrão em produção.

## Autenticação

A API utiliza tokens JWT (Bearer). Um usuário administrador local é criado pelas migrações:

- E-mail: `admin@ambev.com`
- Senha: `Admin@123`

1. Faça login em `POST /api/Auth` pelo Swagger ou por uma ferramenta HTTP:

   ```powershell
   $body = @{ email = "admin@ambev.com"; password = "Admin@123" } | ConvertTo-Json
   $response = Invoke-RestMethod -Method Post -Uri http://localhost:8080/api/Auth -ContentType "application/json" -Body $body
   $token = $response.data.token
   ```

2. Envie o token nas rotas protegidas, como as rotas de vendas:

   ```text
   Authorization: Bearer <token>
   ```

No Swagger, clique em **Authorize** e informe `Bearer <token>`. A chave de assinatura está em `Jwt:SecretKey`; configure uma chave segura por `Jwt__SecretKey` fora do ambiente local.

## Estrutura principal

- `src/Ambev.DeveloperEvaluation.WebApi`: endpoints HTTP e Swagger;
- `src/Ambev.DeveloperEvaluation.Application`: casos de uso;
- `src/Ambev.DeveloperEvaluation.Domain`: entidades e regras de domínio;
- `src/Ambev.DeveloperEvaluation.ORM`: persistência e migrações;
- `tests`: testes unitários, funcionais e de integração.
