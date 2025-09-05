#  VisionHive API

A **VisionHive** é uma API RESTful desenvolvida para registrar e acompanhar denúncias ambientais. A aplicação organiza usuários, denúncias, localizações e órgãos públicos de forma estruturada, facilitando o monitoramento e fiscalização dos problemas ambientais reportados.

---

## 🔗 Endpoints Disponíveis

### 👤 Usuário
| Verbo | Rota                | Descrição                                           |
|-------|---------------------|-----------------------------------------------------|
| GET   | `/api/Usuario`      | Lista todos os usuários                             |
| POST  | `/api/Usuario`      | Cadastra um novo usuário                            |
| GET   | `/api/Usuario/{id}` | Detalha um usuário por ID                           |
| PUT   | `/api/Usuario/{id}` | Atualiza dados de um usuário existente              |
| DELETE| `/api/Usuario/{id}` | Remove um usuário do sistema                        |

### 🏛️ Órgão Público
| Verbo | Rota                     | Descrição                                      |
|-------|--------------------------|------------------------------------------------|
| GET   | `/api/OrgaoPublico`      | Lista todos os órgãos públicos                 |
| POST  | `/api/OrgaoPublico`      | Cadastra um novo órgão público                 |
| GET   | `/api/OrgaoPublico/{id}` | Detalha um órgão público por ID                |
| PUT   | `/api/OrgaoPublico/{id}` | Atualiza dados de um órgão público existente   |
| DELETE| `/api/OrgaoPublico/{id}` | Remove um órgão público                        |

### 📍 Localização
| Verbo | Rota                     | Descrição                                      |
|-------|--------------------------|------------------------------------------------|
| GET   | `/api/Localizacao`       | Lista todas as localizações                    |
| POST  | `/api/Localizacao`       | Registra uma nova localização                  |
| GET   | `/api/Localizacao/{id}`  | Detalha uma localização por ID                 |
| PUT   | `/api/Localizacao/{id}`  | Atualiza dados de uma localização existente    |
| DELETE| `/api/Localizacao/{id}`  | Remove uma localização                         |

### 🗺️ Estados
| Verbo | Rota                     | Descrição                                      |
|-------|--------------------------|------------------------------------------------|
| GET   | `/api/Estado`            | Lista todos os estados                         |
| POST  | `/api/Estado`            | Registra um novo estado                        |
| GET   | `/api/Estado/{id}`       | Detalha um estado por ID                       |
| PUT   | `/api/Estado/{id}`       | Atualiza dados de um estado existente          |
| DELETE| `/api/Estado/{id}`       | Remove um estado                               |

### 🧾 Denúncia
| Verbo | Rota                   | Descrição                                        |
|-------|------------------------|--------------------------------------------------|
| GET   | `/api/Denuncia`        | Lista todas as denúncias                         |
| POST  | `/api/Denuncia`        | Registra uma nova denúncia                       |
| GET   | `/api/Denuncia/{id}`   | Detalha uma denúncia por ID                      |
| PUT   | `/api/Denuncia/{id}`   | Atualiza dados de uma denúncia existente         |
| DELETE| `/api/Denuncia/{id}`   | Remove uma denúncia                              |

### 🏙️ Cidades
| Verbo | Rota                | Descrição                                           |
|-------|---------------------|-----------------------------------------------------|
| GET   | `/api/Cidade`       | Lista todas as cidades                              |
| POST  | `/api/Cidade`       | Registra uma nova cidade                            |
| GET   | `/api/Cidade/{id}`  | Detalha uma cidade por ID                           |
| PUT   | `/api/Cidade/{id}`  | Atualiza dados de uma cidade existente              |
| DELETE| `/api/Cidade/{id}`  | Remove uma cidade                                   |

### 🏘️ Bairros
| Verbo | Rota                | Descrição                                           |
|-------|---------------------|-----------------------------------------------------|
| GET   | `/api/Bairro`       | Lista todos os bairros                              |
| POST  | `/api/Bairro`       | Registra um novo bairro                             |
| GET   | `/api/Bairro/{id}`  | Detalha um bairro por ID                            |
| PUT   | `/api/Bairro/{id}`  | Atualiza dados de um bairro existente               |
| DELETE| `/api/Bairro/{id}`  | Remove um bairro                                    |

### 📊 Acompanhamento de Denúncia
| Verbo | Rota                                 | Descrição                                     |
|-------|--------------------------------------|-----------------------------------------------|
| GET   | `/api/AcompanhamentoDenuncia`        | Lista todos os acompanhamentos                |
| POST  | `/api/AcompanhamentoDenuncia`        | Cria novo acompanhamento                      | 
| GET   | `/api/AcompanhamentoDenuncia/{id}`   | Detalha um acompanhamento por ID              |
| PUT   | `/api/AcompanhamentoDenuncia/{id}`   | Atualiza dados de um acompanhamento por ID    |
| DELETE| `/api/AcompanhamentoDenuncia/{id}`   | Remove um acompanhamento                      |

---

## 🛠 Tecnologias Utilizadas

- ASP.NET Core 8 Web API
- Entity Framework Core
- Banco de Dados Oracle
- Swagger (OpenAPI)
- Serialização JSON com ReferenceHandler.IgnoreCycles

---

## 🧪 Exemplos de Testes

### 🔹 Criar Usuário

```json
POST /api/Usuario
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "123456",
  "tipoUsuario": "USER"
}
```

### 🔹 Criar Órgão Público

```json
POST /api/OrgaoPublico
{
  "nome": "Secretaria do Meio Ambiente MG",
  "areaAtuacao": "Ambiental"
}
```

### 🔹 Criar Localização

```json
POST /api/Localizacao
{
  "logradouro": "Av Paulista",
  "numero": "1106",
  "complemento": "5º andar",
  "cep": "01311-000",
  "idBairro": "GUID_DO_BAIRRO"
}
```

### 🔹 Criar Estado

```json
POST /api/Estado
{
  "nome": "Santa Catarina",
  "uf": "SC"
}
```

### 🔹 Criar Denúncia

```json
POST /api/Denuncia
{
  "idUsuario": "GUID_DO_USUARIO",
  "idLocalizacao": "GUID_DA_LOCALIZACAO",
  "idOrgaoPublico": "GUID_DO_ORGAO",
  "dataHora": "2025-06-02T16:03:04.057Z",
  "descricao": "Descarte de resíduos tóxicos próximo ao rio."
}
```

### 🔹 Criar Cidade

```json
POST /api/Cidade
{
  "nome": "Campinas",
  "idEstado": "GUID_DO_ESTADO"
}
```

### 🔹 Criar Bairro

```json
POST /api/Bairro
{
  "nome": "Taquaral",
  "idCidade": "GUID_DA_CIDADE"
}
```

### 🔹 Criar Acompanhamento

```json
POST /api/acompanhamentodenuncia
{
  "status": "EmAndamento",
  "dataAtualizacao": "2025-06-02T16:40:00.000Z",
  "observacao": "A prefeitura iniciou a limpeza.",
  "denunciaId": "GUID_DA_DENUNCIA"
}
```

---

## 🚀 Instruções de Execução

1. Clone o repositório:
   ```bash
   git clone https://github.com/larissa557197/EcoDenuncia-gs-DotNet.git 
   ```

2. Configure a string de conexão Oracle no `appsettings.json` :
   ```json
   "ConnectionStrings": {
    "Oracle": "Data Source=oracle.fiap.com.br:1521/orcl;User ID=RMXXXXXX;Password=XXXXXX;"
   }
   ```
   > ID= `SEU_RM` - Exemplo: RM123456, Password= `SUA_SENHA_ORACLE` - Exemplo: 123456

3. Aplique as migrations:
   ```bash
   dotnet ef database update
   ```

4. Execute o projeto:
   ```bash
   dotnet run
   ```

5. Acesse a documentação:
   ```
   http://localhost:{porta}/swagger/index.html
   ```
---

## 🧩 Diagramas do Projeto

O projeto está respaldado por uma estrutura bem definida, com diagramas que facilitam a compreensão:

- ### 🗺️ **Diagrama Entidade-Relacionamento (DER)**:
  - #### Representa as relações entre:
     ```
     Denuncia, Usuario, Localizacao, OrgaoPublico, AcompanhamentoDenuncia, Bairro, Cidade e Estado.
     ```
    - #### 📦 Entidades e Relacionamentos
       - 🧾 Denuncia:
         - `IdUsuario` → 🔗 `Usuario.IdUsuario`
         - `IdLocalizacao` → 🔗 `Localizacao.IdLocalizacao`
         - `IdOrgaoPublico` → 🔗 `OrgaoPublico.IdOrgaoPublico`
        - 📍 Localizacao:
          - `IdBairro` → 🔗 `Bairro.IdBairro`
        - 🏘️ Bairro:
          - `IdCidade` → 🔗 `Cidade.IdCidade`
        - 🏙️ Cidade:
          - `IdEstado` → 🔗 `Estado.IdEstado`
  

### 🧩 Relações:

| Entidade     | Relacionamento com           |   Tipo    |
|--------------|------------------------------|-----------|
| Denuncia     |      Usuario                 |    N:1    |
| Denuncia     |      Localizacao             |    N:1    |
| Denuncia     |      OrgaoPublico            |    N:1    |
| Denuncia     |      AcompanhamentoDenuncia  |    1:N    |
| Localizacao  |      Bairro                  |    N:1    |
| Bairro       |      Cidade                  |    N:1    |
| Cidade       |      Estado                  |    N:1    |


## 🧠 Arquitetura em Camadas

A solução segue o modelo de arquitetura em camadas com responsabilidades bem definidas:

### 📁 Controllers

* Expõem os endpoints da API REST `(GET, POST, PUT, DELETE)`.
* Chamam os métodos do domínio e lidam com DTOs para entrada e saída de dados.

### 📁 Domain

* Contém enums e regras de negócio.
* Livre de dependências externas (como Entity Framework).
* Ex: `StatusDenuncia`, `TipoUsuario`, classes de exceções.

### 📁 DTO (Data Transfer Objects)

* Objetos usados para transportar dados da API para o domínio e vice-versa.
* Subdividido em:

  * `Request`: dados enviados pelo cliente.
  * `Response`: dados retornados ao cliente.

### 📁 Infrastructure

* Onde estão os detalhes da implementação, acesso a banco e persistência.

#### 📁 Contexts

* Contém a classe `EcoDenunciaContext` (herda de `DbContext`).
* Define os `DbSet` de cada entidade e aplica configurações dos mapeamentos.

#### 📁 Mappings

* Contém as classes de configuração das entidades para o banco.
* Usa `IEntityTypeConfiguration<T>` para definir tabelas, tamanhos, chaves, relacionamentos etc.

#### 📁 Persistence

* Contém as **entidades** que representam as tabelas no banco Oracle.
* Cada entidade encapsula suas validações e métodos como `Create` ou `Atualizar`.

#### 📁 Repositories *(opcional)*

* Implementa o padrão **Repository Pattern** para abstrair o acesso ao banco.
* Facilita testes e manutenção.

### 📁 Migrations

* Geradas pelo Entity Framework Core.
* Representam as versões do banco de dados e permitem aplicar alterações via `dotnet ef`.

### 📄 appsettings.json / appsettings.Development.json

* Contêm configurações da aplicação, como string de conexão Oracle.

### 📄 Program.cs

* Arquivo principal de inicialização do ASP.NET Core.
* Registra serviços, middlewares e configura Swagger, contexto, CORS, etc.

### 📄 README.md

* Documentação geral do projeto, com instruções, exemplos de uso e arquitetura.


---

## 👥 Integrantes

| Nome             | RM       |
|------------------|----------|
| Larissa Muniz    | RM557197 |
| João V. Michaeli | RM555678 |
| Henrique Garcia  | RM558062 |

---

> Projeto acadêmico desenvolvido na FIAP — 2º Semestre
