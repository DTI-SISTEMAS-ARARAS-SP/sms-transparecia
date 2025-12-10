namespace Api.Dtos
{
  public class DocConvenioUpdateDto
  {
    public int Id { get; set; }
    public int? ConvenioId { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NomeArquivoOriginal { get; set; }
    public string? NomeArquivoSalvo { get; set; }
    public string? CaminhoArquivo { get; set; }
    public long? TamanhoBytes { get; set; }
    public string? Descricao { get; set; }
  }
}
