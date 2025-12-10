using Api.Dtos;
using Api.Models;

namespace Api.Helpers
{
  public static class DocConvenioMapper
  {
    public static DocConvenioReadDto MapToDocConvenioReadDto(DocConvenio doc)
    {
      return new DocConvenioReadDto
      {
        Id = doc.Id,
        ConvenioId = doc.ConvenioId,
        TipoDocumento = doc.TipoDocumento,
        NomeArquivoOriginal = doc.NomeArquivoOriginal,
        NomeArquivoSalvo = doc.NomeArquivoSalvo,
        CaminhoArquivo = doc.CaminhoArquivo,
        TamanhoBytes = doc.TamanhoBytes,
        Descricao = doc.Descricao,
        UploadedByUserId = doc.UploadedByUserId,
        CreatedAt = doc.CreatedAt,
        UpdatedAt = doc.UpdatedAt
      };
    }
  }
}
