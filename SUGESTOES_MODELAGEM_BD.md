# Sugestões de Modelagem - Sistema de Transparência SMS

## 📋 Análise do Documento de Requisição

### Solicitante
- **Nome:** Bruno Barioni Ribeiro Rosa
- **Cargo:** Chefe de Divisão - Sistemas Saúde
- **Data:** 06/11/2024
- **Prioridade:** Alta

### Motivação
O Tribunal de Contas exige maior transparência da Secretaria Municipal de Saúde. É necessário publicar os convênios na página da prefeitura de forma fácil e transparente, não apenas valores no Portal da Transparência.

### Requisitos Reais do Sistema

**Requisitos Funcionais:**
1. ✅ Página web para visualizar e baixar PDFs de convênios
2. ✅ Upload de PDFs pela SMS (setor de convênios/financeiro)
3. ✅ Classificação em "Convênios Ativos" e "Histórico de Convênios"
4. ✅ Quando expirar vigência, mover de "Ativos" para "Histórico"

**Requisitos Não-Funcionais:**
- ✅ Plataforma web
- ✅ Sem integração com outros sistemas
- ✅ Sistema permanente
- ✅ Apenas upload de arquivos PDFs

**Usuários Principais:**
- Bruno Barioni Ribeiro Rosa
- Maria Roseli Zutin Franzini
- Setor de Convênios/Financeiro da SMS

---

## 📋 Análise do Boilerplate Existente

### Tabelas Atuais
O boilerplate já possui uma estrutura sólida de autenticação e controle de acesso:

- **users** - Usuários do sistema administrativo
- **system_resources** - Recursos/módulos do sistema para controle de permissões
- **access_permissions** - Permissões de acesso dos usuários aos recursos
- **system_logs** - Auditoria de ações do sistema

### Usuários Padrão
- **root** (email: root@admin.com, senha: root1234) - Administrador principal
- 10 usuários de teste (alice, bob, carol, etc.) - Disponíveis quando `RUN_USERS_SEED=true`

---

## 🗄️ Modelagem Proposta (Simplificada)

### 1. Tabela: `convenios`

Armazena informações básicas dos convênios da secretaria de saúde.

```csharp
[Table("convenios")]
public class Convenio
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("numero_convenio")]
    [MaxLength(100)]
    public required string NumeroConvenio { get; set; } // Ex: "Convênio 123/2024"

    [Required]
    [Column("titulo")]
    [MaxLength(255)]
    public required string Titulo { get; set; }

    [Column("descricao")]
    [MaxLength(2000)]
    public string? Descricao { get; set; }

    [Required]
    [Column("orgao_concedente")]
    [MaxLength(255)]
    public required string OrgaoConcedente { get; set; } // Ex: "Ministério da Saúde"

    [Column("data_publicacao_diario")]
    public DateTime? DataPublicacaoDiario { get; set; }

    [Column("data_vigencia_inicio")]
    public DateTime? DataVigenciaInicio { get; set; }

    [Column("data_vigencia_fim")]
    public DateTime? DataVigenciaFim { get; set; }

    [Required]
    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "ativo"; // "ativo" ou "historico"

    [Required]
    [Column("visivel_no_portal")]
    public bool VisivelNoPortal { get; set; } = true;

    [Column("observacoes")]
    [MaxLength(1000)]
    public string? Observacoes { get; set; }

    [Required]
    [Column("created_by_user_id")]
    public required int CreatedByUserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedByUser { get; set; }

    public ICollection<DocumentoConvenio> Documentos { get; set; } = new List<DocumentoConvenio>();
}
```

### 2. Tabela: `documentos_convenio`

Armazena os arquivos PDF relacionados aos convênios.

```csharp
[Table("documentos_convenio")]
public class DocumentoConvenio
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("convenio_id")]
    public required int ConvenioId { get; set; }

    [Required]
    [Column("tipo_documento")]
    [MaxLength(100)]
    public required string TipoDocumento { get; set; } // Ex: "termo_convenio", "aditivo", "prestacao_contas"

    [Required]
    [Column("nome_arquivo_original")]
    [MaxLength(255)]
    public required string NomeArquivoOriginal { get; set; } // Nome do arquivo enviado

    [Required]
    [Column("nome_arquivo_salvo")]
    [MaxLength(255)]
    public required string NomeArquivoSalvo { get; set; } // Nome único no servidor

    [Required]
    [Column("caminho_arquivo")]
    [MaxLength(500)]
    public required string CaminhoArquivo { get; set; } // Caminho no servidor

    [Required]
    [Column("tamanho_bytes")]
    public required long TamanhoBytes { get; set; }

    [Column("descricao")]
    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Required]
    [Column("uploaded_by_user_id")]
    public required int UploadedByUserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    [ForeignKey(nameof(ConvenioId))]
    public Convenio? Convenio { get; set; }

    [ForeignKey(nameof(UploadedByUserId))]
    public User? UploadedByUser { get; set; }
}
```

---

## 🔄 Atualização do System Resources

Adicionar novo recurso ao seed para controle de permissões:

```csharp
// Em DbInitializer.cs - SeedSystemResourcesAsync
new SystemResource { Name = "convenios", ExhibitionName = "Gestão de Convênios" }
```

---

## 📊 Diagrama de Relacionamentos

```
users (existente)
  ├── 1:N → convenios (created_by_user_id)
  ├── 1:N → documentos_convenio (uploaded_by_user_id)
  └── 1:N → access_permissions (existente)

convenios
  ├── N:1 → users (created_by_user_id)
  └── 1:N → documentos_convenio (convenio_id)

documentos_convenio
  ├── N:1 → convenios (convenio_id)
  └── N:1 → users (uploaded_by_user_id)
```

---

## 🛠️ Considerações de Implementação

### 1. Armazenamento de Arquivos

**Opções:**

**A) Armazenamento Local (Filesystem)**
```
/uploads/
  └── convenios/
      ├── 2024/
      │   ├── 01/
      │   │   ├── uuid-termo-convenio.pdf
      │   │   └── uuid-aditivo.pdf
      │   └── 02/
      └── 2025/
```

**Prós:** Simples, sem custos adicionais
**Contras:** Backup manual, escalabilidade limitada

**B) Armazenamento em Nuvem (S3, Azure Blob, etc.)**

**Prós:** Escalável, backup automático, CDN
**Contras:** Custo, dependência externa

**Recomendação:** Iniciar com filesystem local e migrar para nuvem se necessário.

### 2. Segurança dos Arquivos

- **Gerar nomes únicos** para evitar sobrescrita: `{Guid.NewGuid()}-{nomeOriginal}`
- **Validar tipos de arquivo** permitidos (PDF, DOC, DOCX, XLS, XLSX)
- **Limitar tamanho** de upload (ex: 10MB por arquivo)
- **Sanitizar nomes** de arquivos para evitar path traversal
- **Servir arquivos** via endpoint controlado, não diretamente da pasta

### 3. API Endpoints Sugeridos

```
# Endpoints Públicos (sem autenticação)
GET    /api/convenios/ativos              # Lista convênios ativos
GET    /api/convenios/historico           # Lista convênios históricos
GET    /api/convenios/{id}                # Detalhes do convênio
GET    /api/convenios/{id}/documentos     # Lista documentos do convênio
GET    /api/documentos/{id}/download      # Download de documento específico

# Endpoints Administrativos (autenticados com permissão "convenios")
GET    /api/admin/convenios               # Lista todos convênios (admin)
POST   /api/admin/convenios               # Criar convênio
PUT    /api/admin/convenios/{id}          # Atualizar convênio
DELETE /api/admin/convenios/{id}          # Remover convênio
PUT    /api/admin/convenios/{id}/status   # Mover entre ativo/histórico
POST   /api/admin/convenios/{id}/documentos        # Upload documento PDF
DELETE /api/admin/documentos/{id}                  # Remover documento
```

### 4. Workflow do Sistema

Conforme descrito no documento de requisição:

1. **Criação:** Funcionário cria registro do convênio no sistema
2. **Aprovação:** Convênio passa por processo de aprovação (pode ser controlado pelo campo `visivel_no_portal`)
3. **Publicação no Diário Oficial:** Registrar data no campo `data_publicacao_diario`
4. **Publicação no Portal:** Tornar visível no site (`visivel_no_portal = true`, `status = "ativo"`)
5. **Vigência:** Sistema permite marcar data de início e fim de vigência
6. **Movimentação para Histórico:** Quando expirar vigência, alterar `status` de "ativo" para "historico"

### 5. Validações Importantes

- ✅ Número do convênio único
- ✅ Título obrigatório
- ✅ Data de vigência fim >= data de vigência início (quando informada)
- ✅ Usuário tem permissão "convenios" para operações admin
- ✅ Arquivo deve ser PDF (validar MIME type: `application/pdf`)
- ✅ Arquivo não excede tamanho máximo (sugestão: 10MB)
- ✅ Status deve ser "ativo" ou "historico"

### 6. Auditoria (System Logs)

Registrar ações importantes:
- Criação de convênio
- Atualização de convênio
- Remoção de convênio
- Upload de documento
- Remoção de documento
- Alteração de status (ativo ↔ histórico)
- Alteração de visibilidade

---

## 📝 Índices Recomendados

Para otimizar consultas:

```csharp
// Em ApiDbContext.cs - OnModelCreating

// Índices para convenios
modelBuilder.Entity<Convenio>()
    .HasIndex(c => c.NumeroConvenio)
    .IsUnique();

modelBuilder.Entity<Convenio>()
    .HasIndex(c => c.Status);

modelBuilder.Entity<Convenio>()
    .HasIndex(c => c.VisivelNoPortal);

modelBuilder.Entity<Convenio>()
    .HasIndex(c => c.DataAssinatura);

// Índices para documentos_convenio
modelBuilder.Entity<DocumentoConvenio>()
    .HasIndex(d => d.ConvenioId);

modelBuilder.Entity<DocumentoConvenio>()
    .HasIndex(d => d.TipoDocumento);

modelBuilder.Entity<DocumentoConvenio>()
    .HasIndex(d => d.VisivelNoPortal);
```

---

## 🚀 Próximos Passos

### Fase 1: Backend - Estrutura de Dados

1. **Criar Models** em `Api/Models/`
   - `Convenio.cs`
   - `DocumentoConvenio.cs`

2. **Atualizar DbContext** em `Api/Data/ApiDbContext.cs`
   - Adicionar DbSets para `Convenios` e `DocumentosConvenio`
   - Configurar relacionamentos e índices no `OnModelCreating`

3. **Atualizar System Resources Seed** em `Api/Data/DbInitializer.cs`
   - Adicionar recurso "convenios" na função `SeedSystemResourcesAsync`

4. **Criar Migration**
   ```bash
   cd Api
   dotnet ef migrations add AddConveniosTables
   dotnet ef database update
   ```

### Fase 2: Backend - Lógica de Negócio

5. **Criar DTOs** em `Api/Dtos/`
   - `ConvenioCreateDto.cs`
   - `ConvenioUpdateDto.cs`
   - `ConvenioReadDto.cs`
   - `ConvenioListDto.cs` (versão resumida para listagens)
   - `DocumentoConvenioUploadDto.cs`
   - `DocumentoConvenioReadDto.cs`

6. **Implementar Services** em `Api/Services/`
   - `ConvenioService.cs` - CRUD de convênios
   - `DocumentoConvenioService.cs` - Upload, download, remoção de documentos
   - `FileStorageService.cs` - Gerenciamento de arquivos no filesystem

7. **Criar Controllers** em `Api/Controllers/`
   - `ConveniosController.cs` - Endpoints públicos
   - `AdminConveniosController.cs` - Endpoints administrativos

8. **Configurar Upload de Arquivos**
   - Criar pasta `/uploads/convenios/` (ou usar configuração do .env)
   - Configurar middleware de upload no `Program.cs`
   - Definir limite de tamanho de arquivo

### Fase 3: Frontend - Interface Pública

9. **Criar Páginas Públicas** em `WebApp/src/pages/`
   - `ConveniosAtivos.tsx` - Lista convênios ativos
   - `ConveniosHistorico.tsx` - Lista convênios históricos
   - `ConvenioDetalhes.tsx` - Detalhes e documentos do convênio

10. **Criar Componentes Públicos** em `WebApp/src/components/`
    - `ConvenioCard.tsx` - Card de convênio
    - `DocumentosList.tsx` - Lista de documentos para download

### Fase 4: Frontend - Painel Administrativo

11. **Criar Páginas Admin** em `WebApp/src/pages/admin/`
    - `ConveniosGestao.tsx` - Lista e gerencia convênios
    - `ConvenioForm.tsx` - Formulário criar/editar convênio
    - `DocumentosUpload.tsx` - Upload de documentos

12. **Criar Componentes Admin** em `WebApp/src/components/admin/`
    - `ConvenioFormFields.tsx` - Campos do formulário
    - `FileUploadZone.tsx` - Zona de drag-and-drop para PDFs
    - `StatusToggle.tsx` - Botão para alternar ativo/histórico

### Fase 5: Testes e Ajustes

13. **Testes**
    - Testar upload de PDFs de diferentes tamanhos
    - Testar validações (arquivo muito grande, tipo inválido)
    - Testar movimentação ativo ↔ histórico
    - Testar visibilidade no portal

14. **Documentação**
    - Atualizar README com instruções de uso
    - Documentar endpoints no Swagger
    - Criar manual para usuários (Bruno e Maria)

---

## 💡 Funcionalidades Adicionais (Futuras)

Sugestões para versões futuras do sistema, após validação inicial:

- **Busca/Filtros:** Por período, órgão concedente, número do convênio
- **Notificações por Email:** Alertar administradores sobre convênios próximos ao vencimento
- **Dashboard:** Estatísticas de convênios ativos, históricos, documentos publicados
- **Versionamento:** Histórico de alterações em documentos (caso sejam substituídos)
- **Múltiplos tipos de arquivo:** Permitir outros formatos além de PDF (DOC, XLS, imagens)
- **Categorização:** Organizar por áreas (Atenção Básica, Infraestrutura, etc.)
- **Logs Detalhados:** Visualização dos logs de auditoria diretamente no painel admin
- **SEO:** Meta tags e sitemap para melhor indexação pelos motores de busca
- **API Pública:** Endpoint para outros sistemas consultarem os convênios
- **Impressão:** Gerar relatório consolidado de todos os documentos de um convênio

---

## ❓ Questões em Aberto

Perguntas para validar com os solicitantes (Bruno e Maria):

1. **Tipos de Documentos:**
   - Quais tipos de documentos serão anexados? (termo inicial, aditivos, prestação de contas, relatórios?)
   - Precisa categorizar os documentos ou todos são tratados igualmente?

2. **Workflow de Aprovação:**
   - Existe algum processo de aprovação antes de publicar no portal?
   - Quem pode criar vs quem pode publicar (tornar visível)?

3. **Movimentação para Histórico:**
   - A movimentação de "ativo" para "histórico" será manual ou automática baseada na data de vigência?
   - Precisa de notificação quando um convênio estiver próximo de expirar?

4. **Volume de Dados:**
   - Quantos convênios estão ativos atualmente?
   - Quantos convênios novos por ano em média?
   - Tamanho médio dos arquivos PDF?

5. **Permissões:**
   - Apenas Bruno e Maria terão acesso administrativo?
   - Precisa de níveis diferentes de permissão (ex: editor vs aprovador)?

6. **Integração Futura:**
   - Existe possibilidade de integração com sistema de diário oficial?
   - Existe sistema de gestão financeira que poderia ser integrado?

---

## 📚 Referências Técnicas

- [Entity Framework Core - Relationships](https://learn.microsoft.com/en-us/ef/core/modeling/relationships)
- [ASP.NET Core - File Upload](https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads)
- [PostgreSQL - Índices](https://www.postgresql.org/docs/current/indexes.html)
- [Lei de Acesso à Informação (LAI) - Lei 12.527/2011](http://www.planalto.gov.br/ccivil_03/_ato2011-2014/2011/lei/l12527.htm)
- [Portal da Transparência - Boas Práticas](https://www.gov.br/cgu/pt-br/assuntos/transparencia-publica)

---

## 📋 Resumo Executivo

### Objetivo
Desenvolver sistema web para publicação de convênios da Secretaria Municipal de Saúde, atendendo às exigências do Tribunal de Contas quanto à transparência pública.

### Solução Proposta
- **Backend:** .NET 8 + PostgreSQL (aproveitando boilerplate existente)
- **Frontend:** React + TypeScript + Material-UI
- **Estrutura:** 2 tabelas novas (`convenios` e `documentos_convenio`)
- **Funcionalidades Principais:**
  - Upload e gestão de PDFs de convênios
  - Classificação em "Ativos" e "Histórico"
  - Portal público para consulta e download
  - Painel administrativo para gestão
  - Auditoria completa de ações

### Benefícios
✅ Atende exigências do Tribunal de Contas
✅ Transparência e acesso público facilitado
✅ Gestão centralizada de documentos
✅ Rastreabilidade completa (quem fez, quando)
✅ Sem integração complexa com outros sistemas
✅ Solução permanente e escalável

### Prazo Estimado de Desenvolvimento
- **Fase 1 (Backend):** ~2 semanas
- **Fase 2 (Frontend Público):** ~1 semana
- **Fase 3 (Frontend Admin):** ~1 semana
- **Fase 4 (Testes):** ~3-5 dias
- **Total:** ~4-5 semanas

### Recursos Necessários
- 1 desenvolvedor full-stack
- Servidor para hospedagem (já existe)
- Domínio/subdomínio para acesso público
- Apoio de Bruno e Maria para validações e testes

---

**Documento gerado em:** 2024-12-09
**Baseado em:** Solicitação de 06/11/2024 - Bruno Barioni Ribeiro Rosa
**Versão:** 2.0 (atualizada com requisitos reais)
