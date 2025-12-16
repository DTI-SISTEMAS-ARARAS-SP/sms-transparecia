using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.ConveniosServices
{
  public class GetAllConvenios
  {
    private readonly IGenericRepository<Convenio> _convenioRepo;

    public GetAllConvenios(IGenericRepository<Convenio> convenioRepo)
    {
      _convenioRepo = convenioRepo;
    }

    public async Task<PaginatedResult<ConvenioReadDto>> ExecuteAsync(int page = 1, int pageSize = 10)
    {
      var query = _convenioRepo.Query()
        .Include(c => c.Documentos)
        .OrderByDescending(c => c.CreatedAt);

      var paginatedConvenios = await ApplyPagination.PaginateAsync(query, page, pageSize);

      return new PaginatedResult<ConvenioReadDto>
      {
        Data = paginatedConvenios.Data.Select(c => ConvenioMapper.MapToConvenioReadDto(c)).ToList(),
        Page = paginatedConvenios.Page,
        PageSize = paginatedConvenios.PageSize,
        TotalItems = paginatedConvenios.TotalItems
      };
    }
  }
}
