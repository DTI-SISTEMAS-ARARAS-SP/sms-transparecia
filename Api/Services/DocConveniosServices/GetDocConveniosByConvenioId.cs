using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.DocConveniosServices
{
  public class GetDocConveniosByConvenioId
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;

    public GetDocConveniosByConvenioId(IGenericRepository<DocConvenio> docConvenioRepo)
    {
      _docConvenioRepo = docConvenioRepo;
    }

    public async Task<IEnumerable<DocConvenioReadDto>> ExecuteAsync(int convenioId)
    {
      var docs = await _docConvenioRepo.Query()
        .Where(d => d.ConvenioId == convenioId)
        .OrderByDescending(d => d.CreatedAt)
        .Select(d => DocConvenioMapper.MapToDocConvenioReadDto(d))
        .ToListAsync();

      return docs;
    }
  }
}
