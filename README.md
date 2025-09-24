# 🚀 VisionHive API

API RESTful desenvolvida em **.NET 8** para gestão de **Filiais**, **Pátios** e **Motos**, criada no contexto do desafio **Mottu - Vision Hive**.  
O projeto segue boas práticas de **Clean Architecture** e **REST**, com documentação via **Swagger** e persistência em **Oracle Database**.

---

## 📖 1. Descrição do Domínio

O sistema simula a operação de pátios de motos em diferentes **filiais**:

- 🏢 **Filial**: unidade física (nome, bairro e CNPJ), que pode conter vários pátios.  
- 🏟️ **Pátio**: espaço para guarda de motos, com limite máximo definido e vínculo com uma filial.  
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
cd Cp4-DotNet-main/Cp4-DotNet-main

# 2. Ajustar a connection string no appsettings.json
"ConnectionStrings": {
  "Oracle": "Data Source=<host>:<port>/<service_name>;User ID=<USUARIO>;Password=<SENHA>;"
}

# 3. Restaurar e compilar
dotnet restore
dotnet build

# 4. Aplicar migrations no Oracle
dotnet ef database update   --project VisionHive.Infrastructure   --startup-project VisionHive.API

# 5. Rodar a API
dotnet run --project VisionHive.API
```

📍 A API sobe por padrão em:  
- http://localhost:5255  
- https://localhost:7072  

📄 Acesse a documentação Swagger em:  
👉 [https://localhost:7072/swagger](https://localhost:7072/swagger)  

---

## 📡 3. Exemplos de Requisições

### 🏢 Filial (`/api/Filial`)

| Método | Rota              | Descrição                  |
|--------|-------------------|----------------------------|
| GET    | `/api/Filial`     | Lista todas as filiais     |
| GET    | `/api/Filial/{id}`| Busca filial por ID (GUID) |
| POST   | `/api/Filial`     | Cria uma filial            |
| PUT    | `/api/Filial/{id}`| Atualiza uma filial        |
| DELETE | `/api/Filial/{id}`| Remove uma filial          |

**Exemplo POST**
```json
{
  "nome": "Filial Lapa",
  "bairro": "Lapa",
  "cnpj": "12.345.678/0001-99"
}
```

---

### 🏟️ Pátio (`/api/Patio`)

| Método | Rota             | Descrição                 |
|--------|------------------|---------------------------|
| GET    | `/api/Patio`     | Lista todos os pátios     |
| GET    | `/api/Patio/{id}`| Busca pátio por ID (GUID) |
| POST   | `/api/Patio`     | Cria um pátio             |
| PUT    | `/api/Patio/{id}`| Atualiza um pátio         |
| DELETE | `/api/Patio/{id}`| Remove um pátio           |

**Exemplo POST**
```json
{
  "nome": "Pátio A",
  "limiteMotos": 120,
  "filialId": "e4d7ec2c-9e36-4a19-9d1e-2d4f9c2e5b11"
}
```

---

### 🏍️ Moto (`/api/Moto`)

| Método | Rota            | Descrição                 |
|--------|-----------------|---------------------------|
| GET    | `/api/Moto`     | Lista todas as motos      |
| GET    | `/api/Moto/{id}`| Busca moto por ID (GUID)  |
| POST   | `/api/Moto`     | Cria uma moto             |
| PUT    | `/api/Moto/{id}`| Atualiza uma moto         |
| DELETE | `/api/Moto/{id}`| Remove uma moto           |

**Exemplo POST**
```json
{
  "placa": "ABC1D23",
  "chassi": "9BWZZZ377VT004251",
  "numeroMotor": "MTR-998877",
  "prioridade": 2,
  "patioId": "8c2c6a7f-0459-4b37-b9e0-8b1f4bf1f111"
}
```

> **Prioridade**:  
> `1 = Baixa | 2 = Média | 3 = Alta | 4 = Sucata`

---

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
