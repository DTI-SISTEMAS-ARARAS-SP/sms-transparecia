# SMS Transparência - API de Convênios

API REST desenvolvida em .NET 8 para gerenciamento de convênios da Prefeitura Municipal de Araras. Sistema completo com upload de documentos, autenticação JWT, controle de acesso baseado em permissões (RBAC) e auditoria.

## Índice

- [Tecnologias](#tecnologias)
- [Funcionalidades](#funcionalidades)
- [Arquitetura](#arquitetura)
- [Instalação e Configuração](#instalação-e-configuração)
- [Endpoints](#endpoints)
- [Autenticação e Autorização](#autenticação-e-autorização)
- [Upload de Arquivos](#upload-de-arquivos)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Banco de Dados](#banco-de-dados)
- [Validações](#validações)
- [Logs e Auditoria](#logs-e-auditoria)

## Tecnologias

- **.NET 8** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **JWT (JSON Web Token)** - Autenticação
- **BCrypt.Net** - Hash de senhas
- **Swagger/OpenAPI** - Documentação de API
- **Resend** - Serviço de e-mail
- **Docker** - Containerização

## Funcionalidades

### Módulo de Convênios
- Cadastro completo de convênios
- Listagem com paginação
- Busca avançada por múltiplos campos
- Edição de convênios existentes
- Exclusão (soft delete)
- Ativação/desativação de convênios
- Endpoint público para listar convênios ativos

### Módulo de Documentos
- Upload de arquivos PDF (máx 10MB)
- Tipificação de documentos (termo_convenio, aditivo, prestacao_contas, etc.)
- Download seguro de documentos
- Listagem de documentos por convênio
- Exclusão de documentos com remoção física do arquivo
- Rastreamento de quem fez upload e quando

### Módulo de Autenticação
- Login local com usuário e senha
- Login externo (integração LDAP/OAuth)
- Geração e validação de tokens JWT
- Reset de senha via e-mail
- Middleware de autenticação obrigatória

### Módulo de Controle de Acesso
- RBAC (Role-Based Access Control)
- Permissões granulares por recurso
- Proteção de endpoints por permissão
- Gerenciamento de recursos do sistema

### Módulo de Auditoria
- Logs de todas as ações do sistema
- Rastreamento de usuário, ação e timestamp
- Visualização de histórico de alterações

## Arquitetura

A API segue uma arquitetura em camadas:

```
Controllers/          → Endpoints HTTP (thin controllers)
Services/            → Lógica de negócio
Repositories/        → Acesso aos dados (padrão Repository)
Data/                → DbContext e configurações EF Core
Models/              → Entidades do banco de dados
Dtos/                → Data Transfer Objects
Helpers/             → Utilitários (JWT, hashing, mapping)
Middlewares/         → Interceptadores de requisição
Validations/         → Validações customizadas
```

### Princípios Aplicados
- **Separation of Concerns**: Cada camada tem responsabilidade única
- **Dependency Injection**: Todas as dependências são injetadas
- **Repository Pattern**: Abstração do acesso aos dados
- **DTO Pattern**: Separação entre modelos de domínio e transporte

## Instalação e Configuração

### Pré-requisitos
- .NET 8 SDK
- PostgreSQL 12+
- Docker e Docker Compose (opcional)

### Configuração de Ambiente

Crie um arquivo `.env` na raiz do projeto Api:

```env
# Banco de Dados
DB_HOST=localhost
DB_PORT=5432
DB_USER=postgres
DB_PASSWORD=sua_senha
DB_NAME=sms_convenios

# API
API_PORT=5209

# JWT
JWT_SECRET_KEY=sua_chave_secreta_base64_aqui

# Frontend (para CORS)
WEB_APP_URL=http://localhost:5173

# E-mail (Resend)
RESEND_API_KEY=re_sua_api_key
RESEND_FROM_EMAIL=noreply@exemplo.com.br

# Armazenamento de Arquivos
FILE_STORAGE_BASE_PATH=./uploads/convenios
FILE_STORAGE_MAX_FILE_SIZE_MB=10
```

### Executar com Docker

```bash
# Desenvolvimento
docker compose -f docker-compose.dev.yml up

# Homologação
docker compose -f docker-compose.homog.yml up

# Produção
docker compose -f docker-compose.prod.yml up
```

### Executar sem Docker

```bash
# Restaurar dependências
dotnet restore

# Aplicar migrations
dotnet ef database update

# Executar
dotnet run
```

A API estará disponível em `http://localhost:5209`
Documentação Swagger em `http://localhost:5209/swagger`

## Endpoints

### Convênios

#### Criar Convênio
```http
POST /api/convenios
Authorization: Bearer {token}
Content-Type: application/json

{
  "numeroConvenio": "2024/001",
  "titulo": "Convênio de Saúde com Estado de São Paulo",
  "descricao": "Convênio para fornecimento de medicamentos",
  "orgaoConcedente": "Secretaria Estadual de Saúde",
  "dataPublicacaoDiario": "2024-01-15T00:00:00Z",
  "dataVigenciaInicio": "2024-02-01T00:00:00Z",
  "dataVigenciaFim": "2025-01-31T00:00:00Z",
  "status": true
}
```

**Resposta 201 Created:**
```json
{
  "id": 1,
  "numeroConvenio": "2024/001",
  "titulo": "Convênio de Saúde com Estado de São Paulo",
  "descricao": "Convênio para fornecimento de medicamentos",
  "orgaoConcedente": "Secretaria Estadual de Saúde",
  "dataPublicacaoDiario": "2024-01-15T00:00:00Z",
  "dataVigenciaInicio": "2024-02-01T00:00:00Z",
  "dataVigenciaFim": "2025-01-31T00:00:00Z",
  "status": true,
  "createdByUserId": 1,
  "createdAt": "2024-12-01T10:00:00Z",
  "updatedAt": "2024-12-01T10:00:00Z",
  "totalDocumentos": 0,
  "documentos": null
}
```

#### Listar Convênios (Paginado)
```http
GET /api/convenios?page=1&pageSize=10
Authorization: Bearer {token}
```

**Resposta 200 OK:**
```json
{
  "data": [
    {
      "id": 1,
      "numeroConvenio": "2024/001",
      "titulo": "Convênio de Saúde...",
      "totalDocumentos": 3,
      ...
    }
  ],
  "totalItems": 45,
  "page": 1,
  "pageSize": 10,
  "totalPages": 5
}
```

#### Listar Convênios Ativos (Público)
```http
GET /api/convenios/ativos
```

**Resposta 200 OK:**
```json
[
  {
    "id": 1,
    "numeroConvenio": "2024/001",
    "titulo": "Convênio de Saúde...",
    "status": true,
    ...
  }
]
```

#### Buscar Convênio por ID
```http
GET /api/convenios/{id}
Authorization: Bearer {token}
```

#### Buscar Convênios (Search)
```http
GET /api/convenios/search?key=saude&page=1&pageSize=10
Authorization: Bearer {token}
```

Busca nos campos: `numeroConvenio`, `titulo`, `orgaoConcedente`

#### Atualizar Convênio
```http
PUT /api/convenios/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "numeroConvenio": "2024/001",
  "titulo": "Convênio de Saúde Atualizado",
  "descricao": "Nova descrição",
  ...
}
```

#### Deletar Convênio
```http
DELETE /api/convenios/{id}
Authorization: Bearer {token}
```

**Resposta 204 No Content**

---

### Documentos de Convênios

#### Upload de Documento
```http
POST /api/docconvenios/convenio/{convenioId}/upload
Authorization: Bearer {token}
Content-Type: multipart/form-data

file: [arquivo.pdf]
tipoDocumento: "termo_convenio"
descricao: "Termo original do convênio" (opcional)
```

**Tipos de documento aceitos:**
- `termo_convenio`
- `aditivo`
- `rescisao`
- `prestacao_contas`
- `relatorio`
- `comprovante`
- `outro`

**Validações:**
- Tamanho máximo: 10MB
- Formato aceito: apenas PDF
- MIME type: application/pdf

**Resposta 201 Created:**
```json
{
  "id": 1,
  "convenioId": 1,
  "tipoDocumento": "termo_convenio",
  "nomeArquivoOriginal": "convenio_termo.pdf",
  "nomeArquivoSalvo": "abc123-convenio_termo.pdf",
  "caminhoArquivo": "2024/12/1/abc123-convenio_termo.pdf",
  "tamanhoBytes": 524288,
  "descricao": "Termo original do convênio",
  "uploadedByUserId": 1,
  "createdAt": "2024-12-01T10:00:00Z",
  "updatedAt": "2024-12-01T10:00:00Z"
}
```

#### Listar Documentos de um Convênio
```http
GET /api/docconvenios/convenio/{convenioId}
Authorization: Bearer {token}
```

**Resposta 200 OK:**
```json
[
  {
    "id": 1,
    "convenioId": 1,
    "tipoDocumento": "termo_convenio",
    "nomeArquivoOriginal": "convenio_termo.pdf",
    "tamanhoBytes": 524288,
    ...
  }
]
```

#### Download de Documento
```http
GET /api/docconvenios/{id}/download
Authorization: Bearer {token}
```

**Resposta 200 OK:**
- Content-Type: application/pdf
- Content-Disposition: attachment; filename="convenio_termo.pdf"
- Body: [bytes do arquivo]

#### Deletar Documento
```http
DELETE /api/docconvenios/{id}
Authorization: Bearer {token}
```

**Resposta 204 No Content**

**Observação:** Remove o registro do banco e o arquivo físico do disco.

---

### Autenticação

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "login": "usuario",
  "password": "senha123"
}
```

**Resposta 200 OK:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": 1,
    "name": "João Silva",
    "login": "usuario",
    "email": "joao@exemplo.com",
    ...
  }
}
```

#### Solicitar Reset de Senha
```http
POST /api/auth/password-reset-request
Content-Type: application/json

{
  "email": "joao@exemplo.com"
}
```

#### Reset de Senha
```http
POST /api/auth/reset-password
Content-Type: application/json

{
  "token": "reset_token",
  "newPassword": "novaSenha123"
}
```

---

### Usuários

#### Listar Usuários
```http
GET /api/users?page=1&pageSize=10
Authorization: Bearer {token}
```

#### Criar Usuário
```http
POST /api/users
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Maria Santos",
  "login": "maria.santos",
  "email": "maria@exemplo.com",
  "password": "senha123"
}
```

#### Atualizar Usuário
```http
PUT /api/users/{id}
Authorization: Bearer {token}
```

#### Deletar Usuário
```http
DELETE /api/users/{id}
Authorization: Bearer {token}
```

---

### Recursos do Sistema

#### Listar Recursos
```http
GET /api/systemresources
Authorization: Bearer {token}
```

#### Criar Recurso
```http
POST /api/systemresources
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "convenios",
  "description": "Gerenciamento de Convênios"
}
```

---

### Logs de Auditoria

#### Listar Logs
```http
GET /api/systemlogs?page=1&pageSize=50
Authorization: Bearer {token}
```

**Resposta 200 OK:**
```json
{
  "data": [
    {
      "id": 1,
      "userId": 1,
      "action": "CREATE",
      "resource": "convenios",
      "resourceId": 1,
      "timestamp": "2024-12-01T10:00:00Z",
      "details": "Criou convênio 2024/001"
    }
  ],
  "totalItems": 150,
  "page": 1,
  "pageSize": 50,
  "totalPages": 3
}
```

## Autenticação e Autorização

### Autenticação JWT

Todos os endpoints (exceto `/api/auth/login` e `/api/convenios/ativos`) requerem autenticação.

**Como usar:**
1. Faça login no endpoint `/api/auth/login`
2. Receba o token JWT na resposta
3. Inclua o token no header de todas as requisições:
   ```
   Authorization: Bearer {seu_token_aqui}
   ```

**Token contém:**
- `sub`: ID do usuário
- `login`: Login do usuário
- `name`: Nome do usuário
- `exp`: Timestamp de expiração

### Autorização RBAC

O sistema utiliza controle de acesso baseado em recursos (RBAC).

**Recursos do sistema:**
- `users` - Gerenciamento de usuários
- `resources` - Gerenciamento de recursos
- `reports` - Relatórios e auditoria
- `convenios` - Gerenciamento de convênios

**Middleware de validação:**
O middleware `ValidateUserPermissionsMiddleware` verifica automaticamente se o usuário tem permissão para acessar cada endpoint baseado no recurso requerido.

**Endpoints públicos:**
- `POST /api/auth/login`
- `POST /api/auth/external-login`
- `POST /api/auth/password-reset-request`
- `POST /api/auth/reset-password`
- `GET /api/convenios/ativos`

## Upload de Arquivos

### Processo de Upload

1. **Validação do arquivo:**
   - Tamanho máximo: 10MB
   - Extensão permitida: `.pdf`
   - MIME type: `application/pdf`

2. **Sanitização do nome:**
   - Remove caracteres especiais
   - Adiciona GUID para garantir unicidade
   - Formato: `{guid}-{nome_sanitizado}.pdf`

3. **Estrutura de armazenamento:**
   ```
   uploads/convenios/
   └── YYYY/
       └── MM/
           └── {convenioId}/
               └── {guid}-arquivo.pdf
   ```

4. **Registro no banco:**
   - Caminho relativo
   - Nome original e nome salvo
   - Tamanho em bytes
   - ID do usuário que fez upload
   - Timestamps

### Exemplo de Estrutura
```
uploads/convenios/
├── 2024/
│   ├── 12/
│   │   ├── 1/
│   │   │   ├── abc123-termo_convenio.pdf
│   │   │   └── def456-aditivo_01.pdf
│   │   └── 2/
│   │       └── ghi789-prestacao_contas.pdf
│   └── 11/
│       └── 3/
│           └── jkl012-relatorio.pdf
└── 2025/
    └── 01/
        └── ...
```

### Download de Arquivos

1. Busca o documento no banco de dados
2. Verifica se o arquivo existe no disco
3. Lê os bytes do arquivo
4. Retorna com headers apropriados:
   - `Content-Type: application/pdf`
   - `Content-Disposition: attachment; filename="{nome_original}"`

## Estrutura do Projeto

```
Api/
├── Controllers/               # Endpoints HTTP
│   ├── ConveniosController.cs
│   ├── DocConveniosController.cs
│   ├── AuthController.cs
│   ├── UsersController.cs
│   ├── SystemResourcesController.cs
│   └── SystemLogsController.cs
├── Services/                  # Lógica de negócio
│   ├── ConveniosServices/
│   │   ├── CreateConvenio.cs
│   │   ├── GetAllConvenios.cs
│   │   ├── GetConvenioById.cs
│   │   ├── GetConveniosAtivos.cs
│   │   ├── SearchConvenios.cs
│   │   ├── UpdateConvenio.cs
│   │   └── DeleteConvenio.cs
│   ├── DocConveniosServices/
│   │   ├── UploadDocConvenio.cs
│   │   ├── DownloadDocConvenio.cs
│   │   ├── GetDocConveniosByConvenioId.cs
│   │   └── DeleteDocConvenio.cs
│   ├── FileStorageService.cs
│   └── ...
├── Repositories/              # Acesso aos dados
│   ├── IGenericRepository.cs
│   └── GenericRepository.cs
├── Data/                      # DbContext e configurações
│   ├── ApiDbContext.cs
│   ├── DbInitializer.cs
│   └── Configurations/
├── Models/                    # Entidades
│   ├── Convenio.cs
│   ├── DocConvenio.cs
│   ├── User.cs
│   ├── AccessPermission.cs
│   ├── SystemResource.cs
│   └── SystemLog.cs
├── Dtos/                      # Data Transfer Objects
│   ├── ConvenioDtos/
│   │   ├── ConvenioCreateDto.cs
│   │   ├── ConvenioReadDto.cs
│   │   └── ConvenioUpdateDto.cs
│   ├── DocConvenioDtos/
│   │   ├── DocConvenioReadDto.cs
│   │   └── ...
│   └── ...
├── Helpers/                   # Utilitários
│   ├── ConvenioMapper.cs
│   ├── DocConvenioMapper.cs
│   ├── JsonWebToken.cs
│   ├── PasswordHashing.cs
│   ├── CurrentAuthUser.cs
│   └── Logger.cs
├── Middlewares/              # Interceptadores
│   ├── ExceptionHandlerMiddleware.cs
│   ├── RequireAuthorizationMiddleware.cs
│   └── ValidateUserPermissionsMiddleware.cs
├── Validations/              # Validações
│   ├── ValidateEntity.cs
│   └── ValidateDateRange.cs
├── Interfaces/               # Contratos
│   └── IGenericRepository.cs
├── Program.cs                # Entry point e configuração
├── appsettings.json
├── .env
└── Dockerfile
```

## Banco de Dados

### Modelo de Dados

#### Tabela: convenios
```sql
CREATE TABLE convenios (
    id SERIAL PRIMARY KEY,
    numero_convenio VARCHAR(255) UNIQUE NOT NULL,
    titulo VARCHAR(500) NOT NULL,
    descricao TEXT,
    orgao_concedente VARCHAR(500) NOT NULL,
    data_publicacao_diario TIMESTAMP,
    data_vigencia_inicio TIMESTAMP,
    data_vigencia_fim TIMESTAMP,
    status BOOLEAN DEFAULT true,
    created_by_user_id INTEGER REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);
```

#### Tabela: documentos_convenio
```sql
CREATE TABLE documentos_convenio (
    id SERIAL PRIMARY KEY,
    convenio_id INTEGER REFERENCES convenios(id) ON DELETE CASCADE,
    tipo_documento VARCHAR(100) NOT NULL,
    nome_arquivo_original VARCHAR(500) NOT NULL,
    nome_arquivo_salvo VARCHAR(500) NOT NULL,
    caminho_arquivo VARCHAR(1000) NOT NULL,
    tamanho_bytes BIGINT NOT NULL,
    descricao TEXT,
    uploaded_by_user_id INTEGER REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);
```

#### Relacionamentos
- `convenios` ← 1:N → `documentos_convenio`
- `users` ← 1:N → `convenios` (created_by)
- `users` ← 1:N → `documentos_convenio` (uploaded_by)
- `users` ← 1:N → `access_permissions`
- `system_resources` ← 1:N → `access_permissions`

### Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration

# Aplicar migrations
dotnet ef database update

# Reverter última migration
dotnet ef database update PreviousMigrationName
```

### Seed de Dados

O sistema executa automaticamente seeds na inicialização (`DbInitializer.SeedAllAsync`):
- Usuário administrador padrão
- Recursos do sistema
- Permissões básicas

## Validações

### Validações de Convênio

**ConvenioCreateDto:**
- `NumeroConvenio`: obrigatório, único no banco
- `Titulo`: obrigatório, máx 500 caracteres
- `OrgaoConcedente`: obrigatório, máx 500 caracteres
- `DataVigenciaFim`: deve ser >= `DataVigenciaInicio`

**Validações executadas:**
```csharp
// Validação de propriedades obrigatórias
ValidateEntity.HasExpectedProperties(dto)

// Validação de valores
ValidateEntity.HasExpectedValues(dto)

// Validação de período
ValidateDateRange.EnsureValidPeriod(inicio, fim)

// Unicidade de número
_repository.ExistsByField("NumeroConvenio", dto.NumeroConvenio)
```

### Validações de Upload

**FileStorageService validações:**
```csharp
// Tamanho máximo
if (file.Length > 10 * 1024 * 1024) // 10MB
    throw new Exception("Arquivo muito grande");

// Extensão
if (Path.GetExtension(file.FileName) != ".pdf")
    throw new Exception("Apenas PDFs são permitidos");

// MIME type
if (file.ContentType != "application/pdf")
    throw new Exception("Tipo de arquivo inválido");
```

## Logs e Auditoria

### Sistema de Logs

Todas as operações CRUD são registradas automaticamente na tabela `system_logs`.

**Informações registradas:**
- `userId`: Quem executou a ação
- `action`: Tipo de ação (CREATE, UPDATE, DELETE, READ)
- `resource`: Recurso afetado (convenios, users, etc.)
- `resourceId`: ID do recurso (se aplicável)
- `timestamp`: Quando ocorreu
- `details`: Detalhes adicionais

**Exemplo de uso:**
```csharp
await _systemLogsService.CreateLogAsync(
    userId: currentUserId,
    action: "CREATE",
    resource: "convenios",
    resourceId: convenio.Id,
    details: $"Criou convênio {convenio.NumeroConvenio}"
);
```

### Consultar Logs

```http
GET /api/systemlogs?page=1&pageSize=50&userId=1&action=CREATE
Authorization: Bearer {token}
```

**Filtros disponíveis:**
- `userId`: Filtrar por usuário
- `action`: Filtrar por tipo de ação
- `resource`: Filtrar por recurso
- `startDate`: Data inicial
- `endDate`: Data final

## Tratamento de Erros

### Middleware de Exceção

O `ExceptionHandlerMiddleware` captura todas as exceções e retorna respostas padronizadas:

**400 Bad Request:**
```json
{
  "error": "Validação falhou",
  "message": "O campo NumeroConvenio é obrigatório"
}
```

**401 Unauthorized:**
```json
{
  "error": "Não autenticado",
  "message": "Token inválido ou expirado"
}
```

**403 Forbidden:**
```json
{
  "error": "Acesso negado",
  "message": "Você não tem permissão para acessar este recurso"
}
```

**404 Not Found:**
```json
{
  "error": "Não encontrado",
  "message": "Convênio com ID 123 não encontrado"
}
```

**500 Internal Server Error:**
```json
{
  "error": "Erro interno",
  "message": "Ocorreu um erro inesperado. Contate o administrador."
}
```

## Segurança

### Proteções Implementadas

1. **Autenticação JWT obrigatória** em endpoints privados
2. **RBAC** com validação de permissões por recurso
3. **Hash BCrypt** para senhas (salt automático)
4. **Validação de upload** (tamanho, tipo, MIME)
5. **Sanitização de nomes** de arquivo
6. **CORS configurado** para origens permitidas
7. **SQL Injection** prevenido por EF Core (prepared statements)
8. **Rate limiting** (configurável via middleware)
9. **HTTPS** recomendado em produção
10. **Logs de auditoria** completos

### Boas Práticas

- Nunca exponha a `JWT_SECRET_KEY`
- Use HTTPS em produção
- Mantenha as dependências atualizadas
- Configure backup automático do banco
- Monitore os logs de erro
- Limite permissões do usuário do banco de dados

## Testes

### Testar com cURL

```bash
# Login
curl -X POST http://localhost:5209/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"login":"admin","password":"senha123"}'

# Criar convênio
curl -X POST http://localhost:5209/api/convenios \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "numeroConvenio":"2024/001",
    "titulo":"Teste",
    "orgaoConcedente":"Governo",
    "status":true
  }'

# Upload de arquivo
curl -X POST http://localhost:5209/api/docconvenios/convenio/1/upload \
  -H "Authorization: Bearer {token}" \
  -F "file=@documento.pdf" \
  -F "tipoDocumento=termo_convenio"
```

### Testar com Swagger

Acesse `http://localhost:5209/swagger` e use a interface interativa.

**Autenticar no Swagger:**
1. Clique em "Authorize" no topo
2. Digite: `Bearer {seu_token}`
3. Clique em "Authorize" e depois "Close"

## Troubleshooting

### Problema: Erro de conexão com o banco

**Solução:**
1. Verifique se o PostgreSQL está rodando
2. Confirme as credenciais no arquivo `.env`
3. Teste a conexão: `psql -h localhost -U postgres -d sms_convenios`

### Problema: Upload falha com "File too large"

**Solução:**
1. Verifique o tamanho do arquivo (máx 10MB)
2. Ajuste `FILE_STORAGE_MAX_FILE_SIZE_MB` no `.env`
3. Aumente `MultipartBodyLengthLimit` em `Program.cs`

### Problema: Token inválido ou expirado

**Solução:**
1. Faça login novamente para obter novo token
2. Verifique se o token está sendo enviado corretamente
3. Confirme que `JWT_SECRET_KEY` está configurado

### Problema: CORS error

**Solução:**
1. Adicione a origem permitida em `Program.cs` na política CORS
2. Verifique `WEB_APP_URL` no `.env`
3. Certifique-se de que o frontend está enviando credenciais

## Performance

### Otimizações Implementadas

- **Paginação** em todos os endpoints de listagem
- **Índices** em campos de busca (numero_convenio, titulo)
- **Lazy loading** desabilitado (explicit loading quando necessário)
- **AsNoTracking** em queries read-only
- **Conexão pooling** do PostgreSQL
- **Caching** de FileStorageService (singleton)

### Recomendações

- Use índices compostos para buscas frequentes
- Implemente cache Redis para dados estáticos
- Configure CDN para arquivos estáticos
- Monitore queries lentas com logging
- Use compressão gzip para respostas

## Suporte

Para questões, bugs ou sugestões:
- Abra uma issue no repositório do projeto
- Contate a equipe de desenvolvimento DTI - Prefeitura de Araras

## Licença

Projeto de uso interno da Prefeitura Municipal de Araras - SP.
