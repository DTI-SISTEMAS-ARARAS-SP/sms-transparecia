using Api.Dtos;
using Api.Models;

namespace Api.Helpers
{
  public static class ConvenioMapper
  {
    public static ConvenioReadDto MapToConvenioReadDto(Convenio convenio)
    {
      return new ConvenioReadDto
      {
        Id = convenio.Id,
        NumeroConvenio = convenio.NumeroConvenio,
        Titulo = convenio.Titulo,
        Descricao = convenio.Descricao,
        OrgaoConcedente = convenio.OrgaoConcedente,
        DataPublicacaoDiario = convenio.DataPublicacaoDiario,
        DataVigenciaInicio = convenio.DataVigenciaInicio,
        DataVigenciaFim = convenio.DataVigenciaFim,
        Status = convenio.Status,
        CreatedByUserId = convenio.CreatedByUserId,
        CreatedAt = convenio.CreatedAt,
        UpdatedAt = convenio.UpdatedAt
      };
    }
  }
}
