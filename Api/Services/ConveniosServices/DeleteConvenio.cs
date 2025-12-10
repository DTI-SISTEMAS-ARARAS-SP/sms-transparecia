using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Api.Services;

namespace Api.Services.ConveniosServices
{
  public class DeleteConvenio
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;
    private readonly CreateSystemLog _createSystemLog;

    public DeleteConvenio(
      IGenericRepository<Convenio> convenioRepo,
      CreateSystemLog createSystemLog)
    {
      _convenioRepo = convenioRepo;
      _createSystemLog = createSystemLog;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
      var convenio = await _convenioRepo.GetByIdAsync(id);
      if (convenio == null)
        return false;

      convenio.Status = false;
      convenio.UpdatedAt = DateTime.UtcNow;

      await _convenioRepo.UpdateAsync(convenio);

      await _createSystemLog.ExecuteAsync(LogActionDescribe.Delete("Convenio", id));

      return true;
    }
  }
}
