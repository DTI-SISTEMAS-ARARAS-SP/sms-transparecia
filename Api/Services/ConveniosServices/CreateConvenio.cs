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

      if (await _convenioRepo.Query().AnyAsync(c => c.NumeroConvenio == dto.NumeroConvenio))
        throw new AppException("Número de convênio já cadastrado.", (int)HttpStatusCode.Conflict);

      var convenio = new Convenio
      {
        NumeroConvenio = dto.NumeroConvenio,
        Titulo = dto.Titulo,
        Descricao = dto.Descricao,
        OrgaoConcedente = dto.OrgaoConcedente,
        DataPublicacaoDiario = dto.DataPublicacaoDiario,
        DataVigenciaInicio = dto.DataVigenciaInicio,
        DataVigenciaFim = dto.DataVigenciaFim,
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
