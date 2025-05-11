
# 💳 Carteira Digital API

API para gerenciamento de carteiras digitais e transações financeiras.

## 🛠️ Tecnologias

- .NET 8  
- PostgreSQL  
- Entity Framework Core  
- JWT Authentication  
- Docker  

## 🧱 Arquitetura

- Clean Architecture  
- Domain-Driven Design (DDD)  
- Repository Pattern  

## 📦 Requisitos

- .NET 8 SDK  
- Docker e Docker Compose  

## ▶️ Executando o Projeto

### ✅ Com Docker (recomendado)

1. Clone o repositório  
2. Crie um arquivo `.env` na raiz do projeto com as variáveis de ambiente:

```env
JWT__SECRET=your_super_secret_key_here_at_least_32_characters_long
```

3. Execute o comando:

```bash
docker-compose up --build
```

A API estará disponível em `http://localhost:5000` e o banco de dados PostgreSQL em `localhost:5432`.

---

### ❌ Sem Docker

1. Certifique-se de ter o PostgreSQL instalado e rodando  
2. Crie um banco de dados chamado `digitalwallet`  
3. Atualize a connection string no arquivo `appsettings.json` no projeto `DigitalWallet.Api`  
4. Execute os comandos:

```bash
dotnet restore
dotnet build
dotnet run --project src/DigitalWallet.Api
```

---

## 🧪 Populando o Banco de Dados

O banco de dados é automaticamente populado com dados de demonstração ao iniciar a aplicação.

**Usuários iniciais:**

- Email: `admin@example.com` / Senha: `Admin@123`  
- Email: `john.doe@example.com` / Senha: `John@123`  
- Email: `jane.smith@example.com` / Senha: `Jane@123`  

Cada usuário inicia com um saldo de **R$ 1000,00**.

---

## 🔗 Endpoints

### 🔐 Autenticação

- `POST /api/auth/register` — Registrar um novo usuário  
- `POST /api/auth/login` — Login e obtenção do token JWT  

### 👛 Carteira (requer autenticação)

- `GET /api/wallet/balance` — Consultar saldo  
- `POST /api/wallet/deposit` — Adicionar saldo  
- `POST /api/wallet/transfer` — Transferir para outro usuário  
- `GET /api/wallet/transactions` — Listar transações (com filtros opcionais de data)  

---

## 🧪 Testes

Para executar os testes:

```bash
dotnet test
```

---

## ✅ Linter

O projeto utiliza **StyleCop** para análise de código. Para verificar:

```bash
dotnet build
```

Os avisos do StyleCop serão exibidos durante a construção.

---

## ✅ Conclusão

Esta implementação completa atende a todos os requisitos solicitados:

1. Autenticação com JWT  
2. CRUD de usuários  
3. Gerenciamento de carteiras digitais  
4. Operações financeiras (depósito, transferência)  
5. Consulta de histórico de transações  
6. Arquitetura moderna (Clean Architecture + DDD)  
7. Configuração para Docker  
8. Linter (StyleCop)  
9. Testes automatizados (unitários e de integração)  
10. Script para popular o banco de dados inicial  

A API segue os padrões REST e está pronta para ser consumida por um front-end. O projeto pode ser facilmente estendido com novas funcionalidades seguindo a mesma estrutura.
