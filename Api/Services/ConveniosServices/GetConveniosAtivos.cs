using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.ConveniosServices
{
  public class GetConveniosAtivos
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;

    public GetConveniosAtivos(IGenericRepository<Convenio> convenioRepo)
    {
      _convenioRepo = convenioRepo;
    }

    public async Task<List<ConvenioReadDto?>> ExecuteAsync()
    {
      var convenios = await _convenioRepo.Query()
        .Where(c => c.Status == true)
        .Include(c => c.Documentos)
        .ToListAsync();


      return convenios
        .Select(c => ConvenioMapper.MapToConvenioReadDto(c, includeDocumentos: true))
        .Where(dto => dto != null)
        .ToList()!;
    }
  }
}
