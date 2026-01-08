using Api.Dtos;
using Api.Services.DocConveniosServices;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DocConveniosController : ControllerBase
  {
    private readonly CreateDocConvenio _createDocConvenio;
    private readonly GetAllDocConvenios _getAllDocConvenios;
    private readonly GetDocConvenioById _getDocConvenioById;
    private readonly GetDocConveniosByConvenioId _getDocConveniosByConvenioId;
    private readonly UpdateDocConvenio _updateDocConvenio;
    private readonly DeleteDocConvenio _deleteDocConvenio;
    private readonly UploadDocConvenio _uploadDocConvenio;
    private readonly DownloadDocConvenio _downloadDocConvenio;

    public DocConveniosController(
      CreateDocConvenio createDocConvenio,
      GetAllDocConvenios getAllDocConvenios,
      GetDocConvenioById getDocConvenioById,
      GetDocConveniosByConvenioId getDocConveniosByConvenioId,
      UpdateDocConvenio updateDocConvenio,
      DeleteDocConvenio deleteDocConvenio,
      UploadDocConvenio uploadDocConvenio,
      DownloadDocConvenio downloadDocConvenio)
    {
      _createDocConvenio = createDocConvenio;
      _getAllDocConvenios = getAllDocConvenios;
      _getDocConvenioById = getDocConvenioById;
      _getDocConveniosByConvenioId = getDocConveniosByConvenioId;
      _updateDocConvenio = updateDocConvenio;
      _deleteDocConvenio = deleteDocConvenio;
      _uploadDocConvenio = uploadDocConvenio;
      _downloadDocConvenio = downloadDocConvenio;
    }

    // POST: api/docconvenios
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DocConvenioCreateDto dto)
    {
      if (dto == null) return BadRequest("Payload inválido.");

      var created = await _createDocConvenio.ExecuteAsync(dto);
      if (created == null)
        return BadRequest("Falha ao criar documento.");

      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // GET: api/docconvenios?page=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
      var allDocs = await _getAllDocConvenios.ExecuteAsync(page, pageSize);
      return Ok(allDocs);
    }

    // GET: api/docconvenios/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      var doc = await _getDocConvenioById.ExecuteAsync(id);
      if (doc == null) return NotFound();
      return Ok(doc);
    }

    // GET: api/docconvenios/convenio/{convenioId}
    [HttpGet("convenio/{convenioId:int}")]
    public async Task<IActionResult> GetByConvenioId(int convenioId)
    {
      var docs = await _getDocConveniosByConvenioId.ExecuteAsync(convenioId);
      return Ok(docs);
    }

    // PUT: api/docconvenios/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DocConvenioUpdateDto dto)
    {
      if (dto == null) return BadRequest("Payload inválido.");

      var updated = await _updateDocConvenio.ExecuteAsync(id, dto);
      if (updated == null) return NotFound();

      return Ok(updated);
    }

    // DELETE: api/docconvenios/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var deleted = await _deleteDocConvenio.ExecuteAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }

    // POST: api/docconvenios/convenio/{convenioId}/upload
    [HttpPost("convenio/{convenioId}/upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
      int convenioId,
      IFormFile file,
      [FromForm] string tipoDocumento,
      [FromForm] string? descricao = null)
    {
      if (file == null || file.Length == 0)
        return BadRequest("Nenhum arquivo foi enviado.");

      var uploaded = await _uploadDocConvenio.ExecuteAsync(convenioId, file, tipoDocumento, descricao);

      return CreatedAtAction(nameof(GetById), new { id = uploaded.Id }, uploaded);
    }

    // GET: api/docconvenios/{id}/download

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id)
    {
      var (fileBytes, fileName, contentType) = await _downloadDocConvenio.ExecuteAsync(id);

      return File(fileBytes, contentType, fileName);
    }
  }
}
