using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Models;

namespace Api.Services.DocConveniosServices
{
  public class GetDocConvenioById
  {
    private readonly IGenericRepository<DocConvenio> _docConvenioRepo;

    public GetDocConvenioById(IGenericRepository<DocConvenio> docConvenioRepo)
    {
      _docConvenioRepo = docConvenioRepo;
    }

    public async Task<DocConvenioReadDto?> ExecuteAsync(int id)
    {
      var docConvenio = await _docConvenioRepo.GetByIdAsync(id);
      if (docConvenio == null)
        return null;

      return DocConvenioMapper.MapToDocConvenioReadDto(docConvenio);
    }
  }
}
