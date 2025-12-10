using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;

namespace Api.Services.DocConveniosServices
{
  public class DeleteDocConvenio
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
    private readonly CreateSystemLog _createSystemLog;

    public DeleteDocConvenio(
      IGenericRepository<DocConvenio> docConvenioRepo,
      CreateSystemLog createSystemLog)
    {
      _docConvenioRepo = docConvenioRepo;
      _createSystemLog = createSystemLog;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
      var deleted = await _docConvenioRepo.DeleteAsync(id);

      if (deleted)
        await _createSystemLog.ExecuteAsync(LogActionDescribe.Delete("DocConvenio", id));

      return deleted;
    }
  }
}
