# API-ClinicaMedica

Esta é uma API para gerenciamento de uma clínica médica, desenvolvida em .NET.

## Funcionalidades
- Cadastro e gerenciamento de pacientes
- Cadastro e gerenciamento de médicos
- Agendamento de consulta e cancelamento de consultas

## ⚖️ Regras de Negócio para Agendamento de Consultas

### 📋 Validações Obrigatórias
```
✅ Paciente deve estar cadastrado e ativo no sistema
✅ Médico deve estar cadastrado e com CRM válido
✅ Data/hora da consulta deve ser futura (não pode agendar no passado)
✅ Horário deve estar dentro do funcionamento da clínica (08h às 18h)
✅ Consulta só pode ser agendada de segunda a sexta-feira
✅ Intervalos de 30 minutos entre consultas do mesmo médico
✅ Se o médico não for informado no momento de cadastro da consulta, o sistema busca um de mesma especialidade disponível na Data e Hora desejada.
✅ Especialidade deve ser válida
```

### 🚫 Restrições de Agendamento
```
❌ Médico não pode ter consultas sobrepostas
❌ Não é permitido agendar em finais de semana
❌ Agendamento deve ser feito com pelo menos 1 hora de antecedência
❌ Consultas não podem ter horário quebrado. Ex: 08:02
❌ Validação de dados de entrada
```

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
- **Middleware**: Tratamento de exceções globais

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
└── Tests/              # Testes unitários
```

## Estrutura do Projeto
- **Application/**: Camada de aplicação (serviços, DTOs, validações)
- **Domain/**: Entidades, enums e value objects
- **Infra/**: Repositórios e contexto de dados
- **Controllers/**: Endpoints da API
- **Middleware/**: Middlewares personalizados
- **Migrations/**: Migrações do banco de dados
- **Tests/**: Testes Unitários

## 🚀 Como executar
1. **Pré-requisitos**: .NET SDK 8.0 ou superior
2. **Clone o repositório**:
   ```bash
   git clone https://github.com/matheusmaiagoulart/API-ClinicaMedica.git
   cd API-ClinicaMedica
   ```
3. **Restaure os pacotes**:
   ```bash
   dotnet restore
   ```
4. **Configure a string de conexão** em `appsettings.json`
5. **Execute as migrações**:
   ```bash
   dotnet ef database update
   ```
6. **Inicie a aplicação**:
   ```bash
   dotnet run --project API-ClinicaMedica/API-ClinicaMedica.csproj
   ```

## 🧪 Testes
```bash
# Executar todos os testes
dotnet test
```

## Configuração
- As configurações de conexão estão em `appsettings.json`.
- Para ambiente de desenvolvimento, utilize `appsettings.Development.json`.


