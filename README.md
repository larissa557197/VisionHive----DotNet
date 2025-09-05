# VisionHive API

API RESTful em .NET 8 para gestão de **Filiais**, **Pátios** e **Motos**
A solução segue boas práticas de arquitetura em camadas (Domain / Application / Infrastructure / API) e expõe endpoints CRUD para o domínio abaixo.

---

## 1) Descrição do domínio

O domínio representa a operação de pátios de motos em diferentes **filiais**:
- **Filial**: unidade física identificada por *Nome*, *Bairro* e *CNPJ*; pode conter vários pátios.
- **Pátio**: área de guarda de motos, com *Nome*, *LimiteMotos* e vínculo com uma filial.
- **Moto**: veículo identificado por *Placa*, *Chassi*, *Número do Motor* e uma *Prioridade* (Baixa/Media/Alta/Sucata), sempre alocada a um pátio.

Relações principais:
- 1 **Filial** → N **Pátios**
- 1 **Pátio** → N **Motos**

---

## 2) Instruções de execução

### Requisitos
- .NET SDK 8.0+
- Banco Oracle acessível (ou ajustar o appsettings para sua instância)
- (Opcional) Ferramentas EF Core: `dotnet tool install --global dotnet-ef`

### Passos

1. **Clonar o repositório**
```bash
git clone <seu-fork-ou-repo>
cd Cp4-DotNet-main/Cp4-DotNet-main
```

2. **Configurar a connection string do Oracle**  
Edite `VisionHive.API/appsettings.json` e ajuste `ConnectionStrings:Oracle` para seu usuário/senha/host:
```json
"ConnectionStrings": {
  "Oracle": "Data Source=<host>:<port>/<service_name>;User ID=<USUARIO>;Password=<SENHA>;"
}
```

3. **Restaurar e compilar**
```bash
dotnet restore
dotnet build
```

4. **Aplicar migrations** (cria/atualiza as tabelas no Oracle)
> Execute a partir da pasta `Cp4-DotNet-main/Cp4-DotNet-main`:
```bash
dotnet ef database update   --project VisionHive.Infrastructure   --startup-project VisionHive.API
```

5. **Rodar a API**
```bash
dotnet run --project VisionHive.API
```
Por padrão a API sobe em `https://localhost:7072` e `http://localhost:5255` (ajustado pelo `launchSettings.json` do seu ambiente).

6. **Swagger**
Em ambiente *Development*, acesse:
```
https://localhost:7072/swagger
```

---

## 3) Exemplos de requisições

### Entidades e DTOs
**Filial**
- Request (`POST /api/Filial`)
```json
{
  "nome": "Filial Lapa",
  "bairro": "Lapa",
  "cnpj": "12.345.678/0001-99"
}
```

- Response (exemplo)
```json
{
  "id": "e4d7ec2c-9e36-4a19-9d1e-2d4f9c2e5b11",
  "nome": "Filial Lapa",
  "bairro": "Lapa",
  "cnpj": "12.345.678/0001-99",
  "patios": []
}
```

**Pátio**
- Request (`POST /api/Patio`)
```json
{
  "nome": "Pátio A",
  "limiteMotos": 120,
  "filialId": "e4d7ec2c-9e36-4a19-9d1e-2d4f9c2e5b11"
}
```

**Moto**
- Request (`POST /api/Moto`)
```json
{
  "placa": "ABC1D23",
  "chassi": "9BWZZZ377VT004251",
  "numeroMotor": "MTR-998877",
  "prioridade": 2,
  "patioId": "8c2c6a7f-0459-4b37-b9e0-8b1f4bf1f111"
}
```
> **Prioridade**: 1=Baixa, 2=Media, 3=Alta, 4=Sucata

### Rotas por recurso

#### Filial (`/api/Filial`)
| Método | Rota                  | Descrição                     |
|-------:|-----------------------|-------------------------------|
| GET    | `/api/Filial`         | Lista todas as filiais        |
| GET    | `/api/Filial/{id}`    | Busca filial por ID (GUID)    |
| POST   | `/api/Filial`         | Cria uma filial               |
| PUT    | `/api/Filial/{id}`    | Atualiza uma filial           |
| DELETE | `/api/Filial/{id}`    | Remove uma filial             |

**Exemplos cURL**
```bash
# GET todas
curl -s http://localhost:5255/api/Filial

# GET por ID
curl -s http://localhost:5255/api/Filial/00000000-0000-0000-0000-000000000000

# POST
curl -s -X POST http://localhost:5255/api/Filial   -H "Content-Type: application/json"   -d '{"nome":"Filial Centro","bairro":"Centro","cnpj":"11.222.333/0001-44"}'

# PUT
curl -s -X PUT http://localhost:5255/api/Filial/00000000-0000-0000-0000-000000000000   -H "Content-Type: application/json"   -d '{"nome":"Filial Centro Atualizada","bairro":"Centro","cnpj":"11.222.333/0001-44"}'

# DELETE
curl -s -X DELETE http://localhost:5255/api/Filial/00000000-0000-0000-0000-000000000000
```

#### Pátio (`/api/Patio`)
| Método | Rota                 | Descrição                    |
|-------:|----------------------|------------------------------|
| GET    | `/api/Patio`         | Lista todos os pátios        |
| GET    | `/api/Patio/{id}`    | Busca pátio por ID (GUID)    |
| POST   | `/api/Patio`         | Cria um pátio                |
| PUT    | `/api/Patio/{id}`    | Atualiza um pátio            |
| DELETE | `/api/Patio/{id}`    | Remove um pátio              |

**Exemplo GET pátios (response resumido)**
```json
[
  {
    "id": "8c2c6a7f-0459-4b37-b9e0-8b1f4bf1f111",
    "nome": "Pátio A",
    "limiteMotos": 120,
    "filialId": "e4d7ec2c-9e36-4a19-9d1e-2d4f9c2e5b11",
    "filial": "Filial Lapa",
    "motos": []
  }
]
```

#### Moto (`/api/Moto`)
| Método | Rota                | Descrição                  |
|-------:|---------------------|----------------------------|
| GET    | `/api/Moto`         | Lista todas as motos       |
| GET    | `/api/Moto/{id}`    | Busca moto por ID (GUID)   |
| POST   | `/api/Moto`         | Cria uma moto              |
| PUT    | `/api/Moto/{id}`    | Atualiza uma moto          |
| DELETE | `/api/Moto/{id}`    | Remove uma moto            |

**Exemplo GET motos (response resumido)**
```json
[
  {
    "id": "4b05db6c-bb7a-4d07-8e2b-3c1b8c9f1234",
    "placa": "ABC1D23",
    "chassi": "9BWZZZ377VT004251",
    "numeroMotor": "MTR-998877",
    "prioridade": "Media",
    "patio": "Pátio A"
  }
]
```

---

## Estrutura do repositório (resumo)
```
VisionHive.Domain/           # Entidades de domínio
VisionHive.Application/      # DTOs, Enums e contratos
VisionHive.Infrastructure/   # EF Core, Mappings, Migrations, Context
VisionHive.API/              # Controllers, Program.cs, Swagger
VisionHive.API.Test/         # Projeto de testes (placeholder)
```

---

## Observações
- Swagger com Anotações de resposta (`ProducesResponseType`) já configurado.
- EF Core com provedor **Oracle** via `UseOracle(...)` em `Program.cs`.
- Campos de **Prioridade** aceitam inteiros 1–4 de acordo com o enum.

---

## Integrantes
|        NOME          |   RM   |
| João Victor Michaeli | 555678 |
| Henrique  Garcia     | 558062 |
| Larissa Muniz        | 557197 |
