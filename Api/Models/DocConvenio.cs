using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("documentos_convenio")]
public class DocConvenio
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
    public required string NomeArquivoOriginal { get; set; } 
    
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
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(ConvenioId))] 
    public Convenio? Convenio { get; set; }
    
    [ForeignKey(nameof(UploadedByUserId))] 
    public User? UploadedByUser { get; set; }
}