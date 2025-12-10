namespace Api.Dtos
{
  public class ConvenioReadDto
  {
    public int Id { get; set; }
    public string NumeroConvenio { get; set; } = default!;
    public string Titulo { get; set; } = default!;
    public string? Descricao { get; set; }
    public string OrgaoConcedente { get; set; } = default!;
    public DateTime? DataPublicacaoDiario { get; set; }
    public DateTime? DataVigenciaInicio { get; set; }
    public DateTime? DataVigenciaFim { get; set; }
    public bool Status { get; set; }
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}