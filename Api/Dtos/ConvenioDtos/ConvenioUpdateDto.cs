namespace Api.Dtos
{
  public class ConvenioUpdateDto
  {
    public int Id { get; set; }
    public string? NumeroConvenio { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public string? OrgaoConcedente { get; set; }
    public DateTime? DataPublicacaoDiario { get; set; }
    public DateTime? DataVigenciaInicio { get; set; }
    public DateTime? DataVigenciaFim { get; set; }
    public bool? Status { get; set; }
  }
}
