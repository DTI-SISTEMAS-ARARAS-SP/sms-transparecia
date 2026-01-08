using Api.Dtos;
using Api.Services.ConveniosServices;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ConveniosController : ControllerBase
  {
    private readonly CreateConvenio _createConvenio;
    private readonly GetAllConvenios _getAllConvenios;
    private readonly GetConvenioById _getConvenioById;
    private readonly UpdateConvenio _updateConvenio;
    private readonly DeleteConvenio _deleteConvenio;
    private readonly SearchConvenios _searchConvenios;
    private readonly GetConveniosAtivos _getConveniosAtivos;

    public ConveniosController(
      CreateConvenio createConvenio,
      GetAllConvenios getAllConvenios,
      GetConvenioById getConvenioById,
      UpdateConvenio updateConvenio,
      DeleteConvenio deleteConvenio,
      SearchConvenios searchConvenios,
      GetConveniosAtivos getConveniosAtivos)
    {
      _createConvenio = createConvenio;
      _getAllConvenios = getAllConvenios;
      _getConvenioById = getConvenioById;
      _updateConvenio = updateConvenio;
      _deleteConvenio = deleteConvenio;
      _searchConvenios = searchConvenios;
      _getConveniosAtivos = getConveniosAtivos;
    }

    // POST: api/convenios
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ConvenioCreateDto dto)
    {
      if (dto == null) return BadRequest("Payload inválido.");

      var created = await _createConvenio.ExecuteAsync(dto);
      if (created == null)
        return BadRequest("Falha ao criar convênio.");

      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET: api/convenios?page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      var allConvenios = await _getAllConvenios.ExecuteAsync(page, pageSize);
      return Ok(allConvenios);
    }

    // GET: api/convenios/ativos
    [HttpGet("ativos")]
    public async Task<IActionResult> GetAtivos()
    {
      var convenios = await _getConveniosAtivos.ExecuteAsync();
      return Ok(convenios);
    }

    // GET: api/convenios/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      var convenio = await _getConvenioById.ExecuteAsync(id);
      if (convenio == null) return NotFound();
      return Ok(convenio);
    }

    // PUT: api/convenios/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ConvenioUpdateDto dto)
    {
      if (dto == null) return BadRequest("Payload inválido.");

      var updated = await _updateConvenio.ExecuteAsync(id, dto);
      if (updated == null) return NotFound();

      return Ok(updated);
    }

    // DELETE: api/convenios/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var deleted = await _deleteConvenio.ExecuteAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }

    // GET: api/convenios/search?key=abc&page=1&pageSize=10
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string key, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      if (string.IsNullOrWhiteSpace(key))
        return BadRequest("A chave de pesquisa é obrigatória.");

      var conveniosFound = await _searchConvenios.ExecuteAsync(key, page, pageSize);
      return Ok(conveniosFound);
    }
  }
}
