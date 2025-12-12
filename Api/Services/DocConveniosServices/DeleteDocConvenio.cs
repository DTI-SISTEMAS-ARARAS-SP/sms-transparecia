using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;

namespace Api.Services.DocConveniosServices
{
  public class DeleteDocConvenio
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
    private readonly FileStorageService _fileStorageService;
    private readonly CreateSystemLog _createSystemLog;

    public DeleteDocConvenio(
      IGenericRepository<DocConvenio> docConvenioRepo,
      FileStorageService fileStorageService,
      CreateSystemLog createSystemLog)
    {
      _docConvenioRepo = docConvenioRepo;
      _fileStorageService = fileStorageService;
      _createSystemLog = createSystemLog;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
      // Buscar documento antes de deletar (para ter o caminho do arquivo)
      var documento = await _docConvenioRepo.GetByIdAsync(id);

      if (documento == null)
        return false;

      // Deletar registro do banco
      var deleted = await _docConvenioRepo.DeleteAsync(id);

      if (deleted)
      {
        // Deletar arquivo físico
        _fileStorageService.DeleteFile(documento.CaminhoArquivo);

        await _createSystemLog.ExecuteAsync(
          LogActionDescribe.Delete("DocConvenio", id));
      }

      return deleted;
    }
  }
}
