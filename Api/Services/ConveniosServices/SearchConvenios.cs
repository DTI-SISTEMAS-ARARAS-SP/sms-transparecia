using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.ConveniosServices
{
  public class SearchConvenios
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;

    public SearchConvenios(IGenericRepository<Convenio> convenioRepo)
    {
      _convenioRepo = convenioRepo;
    }

    public async Task<PaginatedResult<ConvenioReadDto>> ExecuteAsync(string key, int page = 1, int pageSize = 10)
    {
      var query = _convenioRepo.Query()
        .Where(c => c.Status &&
          (c.NumeroConvenio.Contains(key) ||
           c.Titulo.Contains(key) ||
           c.OrgaoConcedente.Contains(key) ||
           (c.Descricao != null && c.Descricao.Contains(key))))
        .OrderByDescending(c => c.CreatedAt)
        .Select(c => ConvenioMapper.MapToConvenioReadDto(c));

      var paginatedConvenios = await ApplyPagination.PaginateAsync(query, page, pageSize);
      return paginatedConvenios;
    }
  }
}
