# API-ClinicaMedica

Esta é uma API para gerenciamento de uma clínica médica, desenvolvida em .NET.

## Funcionalidades
- Cadastro e gerenciamento de pacientes
- Cadastro e gerenciamento de médicos
- Agendamento de consulta e cancelamento de consultas

## 🏛️ Arquitetura e Padrões

### Princípios SOLID
Código organizado seguindo responsabilidade única, extensibilidade e baixo acoplamento.

### Domain-Driven Design (DDD)
Modelagem baseada no domínio da clínica médica com entidades e regras de negócio bem definidas.

### Padrões Implementados
- **Repository Pattern**: Abstração do acesso a dados
- **Unit of Work**: Controle de transações
- **Dependency Injection**: Inversão de dependências
- **Result Pattern**: Padronização de erros

### Clean Architecture
Estrutura em camadas com separação clara de responsabilidades, garantindo manutenibilidade e testabilidade.

```API-ClinicaMedica/
├── Application/           # Camada de aplicação
│   ├── Services/         # Serviços de negócio
│   ├── DTOs/            # Data Transfer Objects
│   ├── Interfaces/      # Contratos de serviços
│   └── BusinessValidations/ # Validações de regras de negócio
├── Domain/              # Camada de domínio
│   ├── Entities/        # Entidades do domínio
│   └── Enums/          # Enumeradores
├── Infra/              # Camada de infraestrutura
│   ├── Repositories/   # Implementação dos repositórios
│   └── Interfaces/     # Contratos de repositórios
└── Tests/              # Testes unitários e de integração
```

## Estrutura do Projeto
- **Application/**: Camada de aplicação (serviços, DTOs, validações)
- **Domain/**: Entidades, enums e value objects
- **Infra/**: Repositórios e contexto de dados
- **Controllers/**: Endpoints da API
- **Middleware/**: Middlewares personalizados
- **Migrations/**: Migrações do banco de dados
- **Tests/**: Testes Unitários

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


