namespace Api.Dtos
{
  public class ConvenioCreateDto
  {
    public required string NumeroConvenio { get; set; }
    public required string Titulo { get; set; }
    public string? Descricao { get; set; }
    public required string OrgaoConcedente { get; set; }
    public DateTime? DataPublicacaoDiario { get; set; }
    public DateTime? DataVigenciaInicio { get; set; }
    public DateTime? DataVigenciaFim { get; set; }
    public bool Status { get; set; } = true;
  }
}
