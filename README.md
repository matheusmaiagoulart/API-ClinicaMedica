# API-ClinicaMedica

Esta é uma API para gerenciamento de uma clínica médica, desenvolvida em .NET.

## Funcionalidades
- Cadastro e gerenciamento de pacientes
- Cadastro e gerenciamento de médicos
- Agendamento, consulta e cancelamento de consultas
- Autenticação de usuários

## Estrutura do Projeto
- **Application/**: Camada de aplicação (serviços, DTOs, validações)
- **Domain/**: Entidades, enums e value objects
- **Infra/**: Repositórios e contexto de dados
- **Controllers/**: Endpoints da API
- **Middleware/**: Middlewares personalizados
- **Migrations/**: Migrações do banco de dados
- **Tests/**: Testes automatizados

## Como executar
1. Instale o .NET SDK (versão recomendada: 7.0 ou superior)
2. Restaure os pacotes:
   ```bash
   dotnet restore
   ```
3. Execute as migrações:
   ```bash
   dotnet ef database update
   ```
4. Inicie a aplicação:
   ```bash
   dotnet run --project API-ClinicaMedica/API-ClinicaMedica.csproj
   ```

## Testes
Para rodar os testes:
```bash
dotnet test
```

## Configuração
- As configurações de conexão estão em `appsettings.json`.
- Para ambiente de desenvolvimento, utilize `appsettings.Development.json`.

## Contato
Dúvidas ou sugestões? Entre em contato com o desenvolvedor.

