# 🚀 VisionHive API

API RESTful desenvolvida em **.NET 8** para gestão de **Filiais**, **Pátios** e **Motos**, criada no contexto do desafio **Mottu - Vision Hive**.  
O projeto segue boas práticas de **Clean Architecture** e **REST**, com documentação via **Swagger** e persistência em **Oracle Database**.

---

## 📖 1. Domínio

O sistema simula a operação de pátios de motos em diferentes **filiais**:

- 🏢 **Filial**: unidade física (nome, bairro e CNPJ), que pode conter vários pátios.  
- 🏟️ **Pátio**: espaço onde guarda de motos, com limite máximo definido e vínculo com uma filial.  
- 🏍️ **Moto**: veículo identificado por placa, chassi, número do motor e prioridade (*Baixa, Média, Alta, Sucata*), sempre associado a um pátio.  

**Relações principais:**
- 1 **Filial** ➝ N **Pátios**  
- 1 **Pátio** ➝ N **Motos**  

---

## ⚙️ 2. Instruções de Execução

### 📌 Requisitos
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)  
- Banco **Oracle** (para a V1) acessível (ou ajustar `appsettings.json`)  
- Banco **MongoDB** (para a V2)
- (Opcional) Ferramentas EF Core:  
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### ▶️ Passo a passo

```bash
# 1. Clonar o repositório
git clone <seu-fork-ou-repo>
cd VisionHive----DotNet

# 2. Configurar appsettings.json e appsettings.Development.json

  {
    "ConnectionStrings": {
    "Oracle": "Data Source=<host>:<port>/<service_name>;User ID=<USUARIO>;Password=<SENHA>;",
    "MongoDB": "mongodb://localhost:27017"
     },
   "MongoSettings": {
     "DatabaseName": "VisionHiveDB"
   },
   "Jwt": {
     "SecretKey": "chave-super-secreta"
   }
  }

# 3. Restaurar e compilar
dotnet restore
dotnet build

# 4. (Opcional) Aplicar migrations no Oracle
dotnet ef database update   --project VisionHive.Infrastructure   --startup-project VisionHive.API

# 5. Rodar a API
dotnet run --project VisionHive.API

#6. Rodar os Testes
dotnet test VisionHive.API.Test
```

📍 A API sobe por padrão em:  
- `http://localhost:5259/swagger` (UI) 
- `http://localhost:5259/health` (Health Check)

---

## 📡 3. Versionamento 
As API é dividia em **duas versões** com bases diferentes:
| Versão | Banco | Autenticação | Rotas base | Descrição |
|--------|-------|--------------|------------|-----------|
|   v1   | Oracle | Sem JWT | `/api/v1` | Versão anterior com EF Core e HATEOAS |
|   v2   | MongoDB | JWT (Bearer) | `/api/v2` | Versão moderna com repositórios Mongo e autenticação |

---

## 📘 Endpoints

### V1 (Oracle)

#### 🏢 Filial (`/api/v1/filiais`)

| Método | Rota                   | Descrição                                  |
|--------|------------------------|--------------------------------------------|
| GET    | `/api/v1/filiais`      | Lista todas as filiais com paginação/filtro |
| GET    | `/api/v1/filiais/{id}` | Busca filial por ID (GUID)                 |
| POST   | `/api/v1/filiais`      | Cria uma filial                            |
| PUT    | `/api/v1/filiais/{id}` | Atualiza uma filial                        |
| DELETE | `/api/v1/filiais/{id}` | Remove uma filial                          |

**Exemplo POST(request)**
```json
{
  "nome": "Filial Zona Norte",
  "bairro": "Santana",
  "cnpj": "12.345.678/0001-99"
}
```
**Resposta 201 (com HATEOAS)**
```json
{
  "data":{
    "id": "GUID...",
    "nome": "Filial Zona Norte",
    "bairro": "Santana",
    "cnpj": "12.345.678/0001-99",
    "patios": []
  },
  "_links": {
    "self":   { "href": "http://localhost:5259/api/v1/filiais/GUID...", "method": "GET" },
    "update": { "href": "http://localhost:5259/api/v1/filiais/GUID...", "method": "PUT" },
    "delete": { "href": "http://localhost:5259/api/v1/filiais/GUID...", "method": "DELETE" }
  }
}
```

---

#### 🏟️ Pátio (`/api/v1/patios`)

| Método | Rota                  | Descrição                            |
|--------|-----------------------|--------------------------------------|
| GET    | `/api/v1/patios`      | Lista todos os pátios com paginação/filtro |
| GET    | `/api/v1/patios/{id}` | Busca pátio por ID                   |
| POST   | `/api/v1/patios`      | Cria um pátio                        |
| PUT    | `/api/v1/patios/{id}` | Atualiza um pátio                    |
| DELETE | `/api/v1/patios/{id}` | Remove um pátio                      |

**Exemplo POST**
```json
{
  "nome": "Pátio Central",
  "limiteMotos": 100,
  "filialId": "GUID_DA_FILIAL"
}
```
**GET lista (resposta esperada)**
```json
{
  "items": [
    {
      "data": {
        "id": "GUID...",
        "nome": "Pátio Central",
        "limiteMotos": 100,
        "filialId": "GUID_DA_FILIAL",
        "filial": "Filial Zona Norte",
        "motos": []
      },
      "_links": {
        "self": { "href": "http://localhost:5259/api/v1/patios/GUID...", "method": "GET" }
      }
    }
  ],
  "page": 1,
  "pageSize": 5,
  "totalItems": 1,
  "totalPages": 1,
  "_links": { "self": "...", "next": null, "prev": null }
}
```
---

#### 🏍️ Moto (`/api/v1/motos`)

| Método | Rota                 | Descrição                                 |
|--------|----------------------|-------------------------------------------|
| GET    | `/api/v1/motos`      | Lista todas as motos com paginação/filtro |
| GET    | `/api/v1/motos/{id}` | Busca moto por ID                         |
| POST   | `/api/v1/motos`      | Cria uma moto                             |
| PUT    | `/api/v1/motos/{id}` | Atualiza uma moto                         |
| DELETE | `/api/v1/motos/{id}` | Remove uma moto                           |

**Exemplo POST(usa `patioId`)**
```json
{
  "placa": "ABC1D23",
  "chassi": "9BWZZZ377VT004251",
  "numeroMotor": "MTR-998877",
  "prioridade": 2,
  "patioId": "GUID_DO_PATIO"
}
```
**Response 201**
```json
{
  "data": {
    "id": "GUI...",
    "placa": "ABC1D23",
    "chassi": "9BWZZZ377VT004251",
    "numeroMotor": "MTR-998877",
    "prioridade": "Média",
    "patioId": "GUID_DO_PATIO",
    "patio": "Pátio Central"
  },
  "_links": {
    "self":   { "href": "http://localhost:5259/api/v1/motos/d93c7f46-8b2c-46f9-a6bb-7e6e6c2f8b71", "method": "GET" },
    "update": { "href": "http://localhost:5259/api/v1/motos/d93c7f46-8b2c-46f9-a6bb-7e6e6c2f8b71", "method": "PUT" },
    "delete": { "href": "http://localhost:5259/api/v1/motos/d93c7f46-8b2c-46f9-a6bb-7e6e6c2f8b71", "method": "DELETE" }
  }
}

```

> **Prioridade**:  
> `1 = Baixa | 2 = Média | 3 = Alta | 4 = Sucata`

---

#### 🧭 HATEOAS & Paginação
- **Detalhe** (`GET / {id}`): `_links.self`, `_links.update`, `_links.delete`
- **Lista** (`GET /`): cada item com `_links.self` e, no nível da página, `_links.self/next/prev` + `page`, `pageSize`, `totalItems`, `totalPages`

---

## V2 (MongoDB + JWT)

### 🏢 Filial (`/api/v2/filiais`)
| Método | Rota                   | Descrição                                  |
|--------|------------------------|--------------------------------------------|
| GET    | `/api/v2/filiais`      | Lista todas as filiais com paginação/filtro |
| GET    | `/api/v2/filiais/{id}` | Busca filial por ID (GUID)                 |
| POST   | `/api/v2/filiais`      | Cria uma filial                            |
| PUT    | `/api/v2/filiais/{id}` | Atualiza uma filial                        |
| DELETE | `/api/v2/filiais/{id}` | Remove uma filial                          |

POST
```json

{
  "nome": "Filial Zona Norte",
  "bairro": "Santana",
  "cnpj": "58.161.539/0837-06"
}
```

GET
```json
{
  "id": "4323484c-85ca-4328-94cd-0b281dbb29d5",
  "nome": "Filial ZN",
  "bairro": "Santana",
  "cnpj": "58.161.539/0837-06",
  "patios": [
    {
      "id": "73912d21-c2a0-4f93-bed7-84085a22aa96",
      "nome": "Pátio Zona Norte",
      "limiteMotos": 100,
      "motos": []
    }
  ]
}
```
---

#### 🏟️ Pátio (`/api/v2/patios`)

| Método | Rota                  | Descrição                            |
|--------|-----------------------|--------------------------------------|
| GET    | `/api/v2/patios`      | Lista todos os pátios com paginação/filtro |
| GET    | `/api/v2/patios/{id}` | Busca pátio por ID                   |
| POST   | `/api/v2/patios`      | Cria um pátio                        |
| PUT    | `/api/v2/patios/{id}` | Atualiza um pátio                    |
| DELETE | `/api/v2/patios/{id}` | Remove um pátio                      |

POST
```json
{
  "nome": "Pátio Zona Norte",
  "limiteMotos": 100,
  "filialId": "4323484c-85ca-4328-94cd-0b281dbb29d5"
}
```

GET
```json
{
  "id": "73912d21-c2a0-4f93-bed7-84085a22aa96",
  "nome": "Pátio Zona Norte",
  "limiteMotos": 100,
  "filialId": "4323484c-85ca-4328-94cd-0b281dbb29d5",
  "motos": []
}
```

---

#### 🏍️ Moto (`/api/v2/motos`)

| Método | Rota                 | Descrição                                 |
|--------|----------------------|-------------------------------------------|
| GET    | `/api/v2/motos`      | Lista todas as motos com paginação/filtro |
| GET    | `/api/v2/motos/{id}` | Busca moto por ID                         |
| POST   | `/api/v2/motos`      | Cria uma moto                             |
| PUT    | `/api/v2/motos/{id}` | Atualiza uma moto                         |
| DELETE | `/api/v2/motos/{id}` | Remove uma moto                           |

POST
```json
{
"placa": "ABC1D23",
"chassi": "9BWZZZ377VT004251",
"numeroMotor": "MTR-998877",
"prioridade": 3,
"patioId": "73912d21-c2a0-4f93-bed7-84085a22aa96"
}
```

GET
```json
{
  "id": "c9403e79-8f61-4c2a-a80d-1f6587d047f1",
  "placa": "ABC1D23",
  "prioridade": "Alta",
  "patioId": "73912d21-c2a0-4f93-bed7-84085a22aa96"
}
```

---

## 🔒 Autenticação JWT (V2)
A versão v2 exige token Bearer JWT em todas as reuquisições:

**Endpoints públicos:**
- `/api/v2/auth/login`
```json
{
  "email": "admin@fiap.com",
  "pass": "123456"
}
```

Header 
`Authorization: Bearer <seu_token_aqui>`

Após o login, o token deve ser usado em todas as rotas protegidas (`Filiais`, `Patios`, `Motos`)

---

## 🩺 Health Check 
Monitora conectividade e dependência externas:

| Nome | Tipo | Status |
|------|------|--------|
| Oracle | Database | ✅      |
| MongoDB | Database | ✅      |
| Fiap/ Google | HTTP |  ✅     |

---

## 📂 Arquitetura

```
VisionHive.API/            # Controllers, Program.cs, Swagger, Auth (JWT)
VisionHive.Application/    # DTOs, UseCases, Configs, HealthChecks
VisionHive.Domain/         # Entidades e Enums
VisionHive.Infrastructure/ # EF Core (Oracle), Mappings, Migrations, MongoDB e Repositories
VisionHive.API.Test/       # Testes unitários com xUnit e Moq
```
### Justificativa da Arquitetura
Adotamos uma **arquitetura em camadas** para separar responsabilidades e facilitar testes, manutenção e evolução
```
┌───────────────────┐
│     API (Web)     │  ← Controllers, validação básica, Swagger (OpenAPI + Examples)
└───────▲───────────┘
        │ DTOs (Request/Response) – contrato REST desacoplado do domínio
┌───────┴───────────┐
│   Application     │  ← Use Cases (orquestram regras), portas/serviços
└───────▲───────────┘
        │ Interfaces (ex.: IFilialRepository)
┌───────┴───────────┐
│      Domain       │  ← Entidades, Enums, regras puras
└───────▲───────────┘
        │ Implementações de portas (infra de dados)
┌───────┴───────────┐
│  Infrastructure   │  ← EF Core (DbContext, Migrations), Repositórios
└───────────────────┘

```
#### Porque essa arquitetura?
- **Coesão e desacoplamento**: 
     - Controllers só tratam HTTP; 
     - **UseCases** encapsulam regras
     - **Infra** apenas persiste dados 
- **Testabilidade**: com interfaces e dependências invertidas, os use cases podem ser testados sem banco real
- **Evolução**: trocar banco (Oracle → SQL Server) ou expor outra interface, não exige reescrever o domínio/regra
- **Boas práticas REST**: 
     - **DTOs** de request/response → contrato estável
     - **Status codes** adequados (201 / 200 / 204 / 400 / 404)
     - **Paginação + HATEOAS** para navegabilidade e discoverability
     - **Swagger/OpenAPI** com **exemplos** e **modelos** claros, ajudando DX e correção acadêmica
#### Descisões técnicas
- **.NET 8 Web API** pela maturidade, tooling e integração nativa com Swagger
- **EF Core** para mapeamento ORM e **Migrations** (versionamento de esquema)
- **Swashbuckle + Filters** para exemplos reais de payloads no Swagger
- **DTOs** evitam vazar o modelo de domínio e permitem versionar a API sem quebrar clientes

---

## 🧩 Tecnologias e padrões 
- .NET 8 Web API
- Entity Framework Core (Oracle)
- MongoDB Driver (.NET)
- JWT Authentication
- Health Checks 
- Swagger + Versioning
- xUnit + Moq para testes
- Clean Architecure + DDD

---

## ✅ Testes Automatizados 

Os testes unitários foram implementados com `xUnit`, `FluentAssertions` e `Moq`

Comandos:

```bash

dotnet test VisionHive.API.Test

```

Cobertura:
- ✅ Controller V2 (Mongo)
- ✅ MotoControllerV2Test corrigido com mock de IMongoDatabase
- ✅ UseCases e validações de domínio

---

## 🧑‍💻 Integrantes do Projeto
- Larissa Muniz (RM557197)  
- João Victor Michaeli (RM555678)  
- Henrique Garcia (RM558062)  

---
✨ Desenvolvido para o **Challenge - Mottu** na **FIAP**
🧩 Versão Final com **Oracle (v1) + MongoDB (v2) + JWT + HealthCheck + Testes Unitários
