using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("convenio")]
    public class Convenio
    {
        [Key] 
        public int Id { get; set; }
        
        [Required]
        [Column("numero_convenio")]
        public required string NumeroConvenio { get; set; }
        
        [Required]
        [Column("titulo")]
        public required string Titulo { get; set; }
        
        [Column("descricao")]
        [MaxLength(2000)]
        public string? Descricao { get; set; }
        
        [Required]
        [Column("orgao_concedente")]
        [MaxLength(255)]
        public required string OrgaoConcedente { get; set; }
        
        [Column("data_publicacao_diario")]
        public DateTime? DataPublicacaoDiario { get; set; }
        
        [Column("data_vigencia_inicio")]
        public DateTime? DataVigenciaInicio { get; set; }

        [Column("data_vigencia_fim")]
        public DateTime? DataVigenciaFim { get; set; }
        
        [Required]
        [Column("status")]
        public bool Status { get; set; }
        
        [Required]
        [Column("created_by_user_id")]
        public required int CreatedByUserId { get; set; }
        
        [Column("created_at")] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [Column("updated_at")] 
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        // relacionamentos
        
        [ForeignKey(nameof(CreatedByUserId))] 
        public User? CreatedByUser { get; set; }
        
        
    }
}

