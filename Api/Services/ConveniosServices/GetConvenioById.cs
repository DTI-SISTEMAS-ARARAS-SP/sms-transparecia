using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.ConveniosServices
{
  public class GetConvenioById
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;

    public GetConvenioById(IGenericRepository<Convenio> convenioRepo)
    {
      _convenioRepo = convenioRepo;
    }

    public async Task<ConvenioReadDto?> ExecuteAsync(int id)
    {
      var convenio = await _convenioRepo.Query()
        .Include(c => c.Documentos)
        .FirstOrDefaultAsync(c => c.Id == id);

      if (convenio == null)
        return null;

      return ConvenioMapper.MapToConvenioReadDto(convenio, includeDocumentos: true);
    }
  }
}
