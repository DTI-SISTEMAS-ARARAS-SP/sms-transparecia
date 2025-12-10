using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;
using Api.Validations;

namespace Api.Services.DocConveniosServices
{
  public class UpdateDocConvenio
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
    private readonly IGenericRepository<Convenio> _convenioRepo;
    private readonly CreateSystemLog _createSystemLog;

    public UpdateDocConvenio(
      IGenericRepository<DocConvenio> docConvenioRepo,
      IGenericRepository<Convenio> convenioRepo,
      CreateSystemLog createSystemLog)
    {
      _docConvenioRepo = docConvenioRepo;
      _convenioRepo = convenioRepo;
      _createSystemLog = createSystemLog;
    }

    public async Task<DocConvenioReadDto?> ExecuteAsync(int id, DocConvenioUpdateDto dto)
    {
      ValidateEntity.HasExpectedProperties<DocConvenioUpdateDto>(dto);
      ValidateEntity.HasExpectedValues<DocConvenioUpdateDto>(dto);

      var docConvenio = await _docConvenioRepo.GetByIdAsync(id);
      if (docConvenio == null)
        return null;

      if (dto.ConvenioId.HasValue)
      {
        await ValidateEntity.EnsureEntityExistsAsync(_convenioRepo, dto.ConvenioId.Value, "Convênio");
        docConvenio.ConvenioId = dto.ConvenioId.Value;
      }

      docConvenio.TipoDocumento = dto.TipoDocumento ?? docConvenio.TipoDocumento;
      docConvenio.NomeArquivoOriginal = dto.NomeArquivoOriginal ?? docConvenio.NomeArquivoOriginal;
      docConvenio.NomeArquivoSalvo = dto.NomeArquivoSalvo ?? docConvenio.NomeArquivoSalvo;
      docConvenio.CaminhoArquivo = dto.CaminhoArquivo ?? docConvenio.CaminhoArquivo;
      docConvenio.TamanhoBytes = dto.TamanhoBytes ?? docConvenio.TamanhoBytes;
      docConvenio.Descricao = dto.Descricao ?? docConvenio.Descricao;
      docConvenio.UpdatedAt = DateTime.UtcNow;

      await _docConvenioRepo.UpdateAsync(docConvenio);

      await _createSystemLog.ExecuteAsync(LogActionDescribe.Update("DocConvenio", docConvenio.Id));

      return DocConvenioMapper.MapToDocConvenioReadDto(docConvenio);
    }
  }
}
