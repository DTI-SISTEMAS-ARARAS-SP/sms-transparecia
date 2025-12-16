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
  public class CreateConvenio
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;
    private readonly CreateSystemLog _createSystemLog;
    private readonly CurrentAuthUser _currentAuthUser;

    public CreateConvenio(
      IGenericRepository<Convenio> convenioRepo,
      CreateSystemLog createSystemLog,
      CurrentAuthUser currentAuthUser)
    {
      _convenioRepo = convenioRepo;
      _createSystemLog = createSystemLog;
      _currentAuthUser = currentAuthUser;
    }

    public async Task<ConvenioReadDto> ExecuteAsync(ConvenioCreateDto dto)
    {
      ValidateEntity.HasExpectedProperties<ConvenioCreateDto>(dto);
      ValidateEntity.HasExpectedValues<ConvenioCreateDto>(dto);

      // Validar período de vigência
      ValidateDateRange.EnsureValidPeriod(dto.DataVigenciaInicio, dto.DataVigenciaFim);

      if (await _convenioRepo.Query().AnyAsync(c => c.NumeroConvenio == dto.NumeroConvenio))
        throw new AppException("Número de convênio já cadastrado.", (int)HttpStatusCode.Conflict);

      var convenio = new Convenio
      {
        NumeroConvenio = dto.NumeroConvenio,
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        OrgaoConcedente = dto.OrgaoConcedente,
        DataPublicacaoDiario = dto.DataPublicacaoDiario.HasValue
          ? DateTime.SpecifyKind(dto.DataPublicacaoDiario.Value, DateTimeKind.Utc)
          : null,
        DataVigenciaInicio = dto.DataVigenciaInicio.HasValue
          ? DateTime.SpecifyKind(dto.DataVigenciaInicio.Value, DateTimeKind.Utc)
          : null,
        DataVigenciaFim = dto.DataVigenciaFim.HasValue
          ? DateTime.SpecifyKind(dto.DataVigenciaFim.Value, DateTimeKind.Utc)
          : null,
        Status = dto.Status,
        CreatedByUserId = _currentAuthUser.GetId(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      await _convenioRepo.CreateAsync(convenio);

      await _createSystemLog.ExecuteAsync(LogActionDescribe.Create("Convenio", convenio.Id));

      return ConvenioMapper.MapToConvenioReadDto(convenio);
    }
  }
}
