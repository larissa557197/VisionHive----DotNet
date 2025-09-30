# 🚀 VisionHive API

API RESTful desenvolvida em **.NET 8** para gestão de **Filiais**, **Pátios** e **Motos**, criada no contexto do desafio **Mottu - Vision Hive**.  
O projeto segue boas práticas de **Clean Architecture** e **REST**, com documentação via **Swagger** e persistência em **Oracle Database**.

---

## 📖 1. Descrição do Domínio

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
- Banco **Oracle** acessível (ou ajustar `appsettings.json`)  
- (Opcional) Ferramentas EF Core:  
  ```bash
  dotnet tool install --global dotnet-ef
  ```

### ▶️ Passo a passo

```bash
# 1. Clonar o repositório
git clone <seu-fork-ou-repo>
cd VisionHive----DotNet

# 2. Ajustar a connection string no appsettings.json
"ConnectionStrings": {
  "Oracle": "Data Source=<host>:<port>/<service_name>;User ID=<USUARIO>;Password=<SENHA>;"
}

# 3. Restaurar e compilar
dotnet restore
dotnet build

# 4. (Opcional) Aplicar migrations no Oracle
dotnet ef database update   --project VisionHive.Infrastructure   --startup-project VisionHive.API

# 5. Rodar a API
dotnet run --project VisionHive.API
```

📍 A API sobe por padrão em:  
- `http://localhost:5259/docs` (UI) 
- `http://localhost:5259/swagger/v1/swagger.json` (OperAPI)

📄 Acesse a documentação Swagger em:  
👉 [http://localhost:5259/docs/index.html](http://localhost:5259/docs/index.html)  

---

## 📡 3. Endpoints (v1)
As rotas seguem o prefixo `api/v1` e estão no **plural**:

### 🏢 Filial (`/api/v1/filiais`)

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

### 🏟️ Pátio (`/api/v1/patios`)

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

### 🏍️ Moto (`/api/v1/motos`)

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

## HATEOAS & Paginação
- **Detalhe** (`GET / {id}`): `_links.self`, `_links.update`, `_links.delete`
- **Lista** (`GET /`): cada item com `_links.self` e, no nível da página, `_links.self/next/prev` + `page`, `pageSize`, `totalItems`, `totalPages`


## 📂 Arquitetura

```
VisionHive.API/            # Controllers, Program.cs, Swagger
VisionHive.Application/    # DTOs, Validações, DI
VisionHive.Domain/         # Entidades de domínio
VisionHive.Infrastructure/ # EF Core, Mappings, Migrations
VisionHive.API.Test/       # Projeto de testes
```

---

## 🧑‍💻 Integrantes do Projeto
- Larissa Muniz (RM557197)  
- João Victor Michaeli (RM555678)  
- Henrique Garcia (RM558062)  

---
✨ Desenvolvido para o **Challenge - Mottu** na **FIAP**
