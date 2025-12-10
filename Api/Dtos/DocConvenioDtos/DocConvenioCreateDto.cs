namespace Api.Dtos
{
  public class DocConvenioCreateDto
  {
    public required int ConvenioId { get; set; }
    public required string TipoDocumento { get; set; }
    public required string NomeArquivoOriginal { get; set; }
    public required string NomeArquivoSalvo { get; set; }
    public required string CaminhoArquivo { get; set; }
    public required long TamanhoBytes { get; set; }
    public string? Descricao { get; set; }
  }
}