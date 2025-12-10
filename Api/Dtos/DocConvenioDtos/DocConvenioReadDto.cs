namespace Api.Dtos
{
  public class DocConvenioReadDto
  {
    public int Id { get; set; }
    public int ConvenioId { get; set; }
    public string TipoDocumento { get; set; } = default!;
    public string NomeArquivoOriginal { get; set; } = default!;
    public string NomeArquivoSalvo { get; set; } = default!;
    public string CaminhoArquivo { get; set; } = default!;
    public long TamanhoBytes { get; set; }
    public string? Descricao { get; set; }
    public int UploadedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
