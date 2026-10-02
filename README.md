# HealthFlow API

API REST para gerenciamento de pacientes, especialidades, profissionais e agendamentos de uma clínica de saúde integrada.

Projeto desenvolvido para portfólio com foco em organização em camadas, autenticação JWT, Entity Framework Core e testes unitários.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- JWT Bearer Authentication
- Swagger/OpenAPI
- xUnit e Moq

## Arquitetura

```text
HealthFlow.Api          Controllers, Swagger e autenticação
HealthFlow.Data         DbContext e migrations
HealthFlow.Model        Entidades, DTOs e enums
HealthFlow.Repository   Acesso aos dados
HealthFlow.Service      Regras de negócio
HealthFlow.Tests        Testes unitários
```

## Funcionalidades

- Cadastro e login de usuários;
- Autenticação com JWT;
- Cadastro de pacientes;
- Cadastro de especialidades;
- Cadastro de profissionais;
- Criação e consulta de agendamentos;
- Confirmação, cancelamento e conclusão de agendamentos;
- Validação de conflitos de horário;
- Testes unitários das principais regras de negócio.

## Pré-requisitos

- .NET 10 SDK;
- Visual Studio 2022 ou superior;
- SQL Server LocalDB;
- Git.

## Configuração

Clone o repositório e acesse a pasta do projeto:

```powershell
git clone URL_DO_REPOSITORIO
cd HealthFlow
```

Inicie o LocalDB:

```powershell
sqllocaldb start MSSQLLocalDB
```

Configure os User Secrets da API:

```powershell
dotnet user-secrets init --project HealthFlow.Api

dotnet user-secrets set "ConnectionStrings:HealthFlowConnection" "Server=(localdb)\MSSQLLocalDB;Database=HealthFlowDb;Trusted_Connection=True;TrustServerCertificate=True" --project HealthFlow.Api

dotnet user-secrets set "Jwt:Key" "HealthFlow-Development-Key-ChangeThis-LongSecret-2026" --project HealthFlow.Api
```

> Em um projeto real, utilize uma chave JWT forte e não compartilhe informações sensíveis.

## Executando o projeto

Restaure os pacotes, aplique as migrations e execute a API:

```powershell
dotnet restore
dotnet ef database update --project HealthFlow.Data --startup-project HealthFlow.Api
dotnet run --project HealthFlow.Api
```

Ou execute pelo Visual Studio com `Ctrl + F5`.

Swagger:

```text
https://localhost:PORTA/swagger
```

A porta pode variar conforme a configuração local.

## Autenticação

Primeiro, crie um usuário:

```http
POST /api/Auth/register
```

```json
{
  "name": "Gabriela",
  "email": "gabriela@email.com",
  "password": "123456"
}
```

Faça login:

```http
POST /api/Auth/login
```

```json
{
  "email": "gabriela@email.com",
  "password": "123456"
}
```

Copie o token retornado, clique em **Authorize** no Swagger e informe:

```text
Bearer SEU_TOKEN
```

Os demais endpoints exigem autenticação.

## Principais endpoints

| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/Auth/register` | Cadastrar usuário |
| POST | `/api/Auth/login` | Fazer login |
| GET | `/api/Patients` | Listar pacientes |
| POST | `/api/Patients` | Cadastrar paciente |
| GET | `/api/Specialties` | Listar especialidades |
| POST | `/api/Specialties` | Cadastrar especialidade |
| GET | `/api/Professionals` | Listar profissionais |
| POST | `/api/Professionals` | Cadastrar profissional |
| GET | `/api/Appointments` | Listar agendamentos |
| POST | `/api/Appointments` | Criar agendamento |
| PUT | `/api/Appointments/{id}/confirm` | Confirmar agendamento |
| PUT | `/api/Appointments/{id}/cancel` | Cancelar agendamento |
| PUT | `/api/Appointments/{id}/complete` | Concluir agendamento |

## Testes

Execute todos os testes com:

```powershell
dotnet test
```

Os testes utilizam xUnit e Moq e cobrem entidades, autenticação e regras de agendamento.

## Autor

Desenvolvido por **Gabriela** para fins de estudo e portfólio.
