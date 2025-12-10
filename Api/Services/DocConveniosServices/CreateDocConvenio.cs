using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;
using Api.Validations;

namespace Api.Services.DocConveniosServices
{
  public class CreateDocConvenio
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
    private readonly IGenericRepository<Convenio> _convenioRepo;
    private readonly CreateSystemLog _createSystemLog;
    private readonly CurrentAuthUser _currentAuthUser;

    public CreateDocConvenio(
      IGenericRepository<DocConvenio> docConvenioRepo,
      IGenericRepository<Convenio> convenioRepo,
      CreateSystemLog createSystemLog,
      CurrentAuthUser currentAuthUser)
    {
      _docConvenioRepo = docConvenioRepo;
      _convenioRepo = convenioRepo;
      _createSystemLog = createSystemLog;
      _currentAuthUser = currentAuthUser;
    }

    public async Task<DocConvenioReadDto> ExecuteAsync(DocConvenioCreateDto dto)
    {
      ValidateEntity.HasExpectedProperties<DocConvenioCreateDto>(dto);
      ValidateEntity.HasExpectedValues<DocConvenioCreateDto>(dto);

      await ValidateEntity.EnsureEntityExistsAsync(_convenioRepo, dto.ConvenioId, "Convênio");

      var docConvenio = new DocConvenio
      {
        ConvenioId = dto.ConvenioId,
        TipoDocumento = dto.TipoDocumento,
        NomeArquivoOriginal = dto.NomeArquivoOriginal,
        NomeArquivoSalvo = dto.NomeArquivoSalvo,
        CaminhoArquivo = dto.CaminhoArquivo,
        TamanhoBytes = dto.TamanhoBytes,
        Descricao = dto.Descricao,
        UploadedByUserId = _currentAuthUser.GetId(),
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      await _docConvenioRepo.CreateAsync(docConvenio);

      await _createSystemLog.ExecuteAsync(LogActionDescribe.Create("DocConvenio", docConvenio.Id));

      return DocConvenioMapper.MapToDocConvenioReadDto(docConvenio);
    }
  }
}
