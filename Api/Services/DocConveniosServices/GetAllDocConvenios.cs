using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.DocConveniosServices
{
  public class GetAllDocConvenios
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;

    public GetAllDocConvenios(IGenericRepository<DocConvenio> docConvenioRepo)
    {
      _docConvenioRepo = docConvenioRepo;
    }

    public async Task<PaginatedResult<DocConvenioReadDto>> ExecuteAsync(int page = 1, int pageSize = 10)
    {
      var query = _docConvenioRepo.Query()
        .OrderByDescending(d => d.CreatedAt)
        .Select(d => DocConvenioMapper.MapToDocConvenioReadDto(d));

      var paginatedDocs = await ApplyPagination.PaginateAsync(query, page, pageSize);
      return paginatedDocs;
    }
  }
}
