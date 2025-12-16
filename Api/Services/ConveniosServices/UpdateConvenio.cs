using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;
using Api.Validations;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Api.Middlewares;

namespace Api.Services.ConveniosServices
{
  public class UpdateConvenio
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;
    private readonly CreateSystemLog _createSystemLog;

    public UpdateConvenio(
      IGenericRepository<Convenio> convenioRepo,
      CreateSystemLog createSystemLog)
    {
      _convenioRepo = convenioRepo;
      _createSystemLog = createSystemLog;
    }

    public async Task<ConvenioReadDto?> ExecuteAsync(int id, ConvenioUpdateDto dto)
    {
      ValidateEntity.HasExpectedProperties<ConvenioUpdateDto>(dto);
      ValidateEntity.HasExpectedValues<ConvenioUpdateDto>(dto);

      var convenio = await _convenioRepo.GetByIdAsync(id);
      if (convenio == null)
        return null;

      if (!string.IsNullOrWhiteSpace(dto.NumeroConvenio) && dto.NumeroConvenio != convenio.NumeroConvenio)
      {
        bool isDuplicate = await _convenioRepo.Query()
          .AnyAsync(c => c.Id != id && c.NumeroConvenio == dto.NumeroConvenio);

        if (isDuplicate)
          throw new AppException("Número de convênio já cadastrado.", (int)HttpStatusCode.Conflict);
      }

      convenio.NumeroConvenio = dto.NumeroConvenio ?? convenio.NumeroConvenio;
      convenio.Titulo = dto.Titulo ?? convenio.Titulo;
      convenio.Descricao = dto.Descricao ?? convenio.Descricao;
      convenio.OrgaoConcedente = dto.OrgaoConcedente ?? convenio.OrgaoConcedente;
      convenio.DataPublicacaoDiario = dto.DataPublicacaoDiario.HasValue
        ? DateTime.SpecifyKind(dto.DataPublicacaoDiario.Value, DateTimeKind.Utc)
        : convenio.DataPublicacaoDiario;
      convenio.DataVigenciaInicio = dto.DataVigenciaInicio.HasValue
        ? DateTime.SpecifyKind(dto.DataVigenciaInicio.Value, DateTimeKind.Utc)
        : convenio.DataVigenciaInicio;
      convenio.DataVigenciaFim = dto.DataVigenciaFim.HasValue
        ? DateTime.SpecifyKind(dto.DataVigenciaFim.Value, DateTimeKind.Utc)
        : convenio.DataVigenciaFim;
      convenio.Status = dto.Status ?? convenio.Status;
      convenio.UpdatedAt = DateTime.UtcNow;

      // Validar período de vigência após atualização
      ValidateDateRange.EnsureValidPeriod(convenio.DataVigenciaInicio, convenio.DataVigenciaFim);

      await _convenioRepo.UpdateAsync(convenio);

      await _createSystemLog.ExecuteAsync(LogActionDescribe.Update("Convenio", convenio.Id));

      // Recarregar com documentos para retornar
      var convenioAtualizado = await _convenioRepo.Query()
        .Include(c => c.Documentos)
        .FirstOrDefaultAsync(c => c.Id == id);

      return convenioAtualizado != null
        ? ConvenioMapper.MapToConvenioReadDto(convenioAtualizado)
        : null;
    }
  }
}
