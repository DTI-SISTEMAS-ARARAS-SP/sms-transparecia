using Api.Dtos;
using Api.Helpers;
using Api.Interfaces;
using Api.Middlewares;
using Api.Models;
using Api.Validations;
using System.Net;

namespace Api.Services.DocConveniosServices
{
    public class UploadDocConvenio
    {
        private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
        private readonly IGenericRepository<Convenio> _convenioRepo;
        private readonly FileStorageService _fileStorageService;
        private readonly CreateSystemLog _createSystemLog;
        private readonly CurrentAuthUser _currentAuthUser;

        public UploadDocConvenio(
            IGenericRepository<DocConvenio> docConvenioRepo,
            IGenericRepository<Convenio> convenioRepo,
            FileStorageService fileStorageService,
            CreateSystemLog createSystemLog,
            CurrentAuthUser currentAuthUser)
        {
            _docConvenioRepo = docConvenioRepo;
            _convenioRepo = convenioRepo;
            _fileStorageService = fileStorageService;
            _createSystemLog = createSystemLog;
            _currentAuthUser = currentAuthUser;
        }

        public async Task<DocConvenioReadDto> ExecuteAsync(
            int convenioId,
            IFormFile file,
            string tipoDocumento,
            string? descricao = null)
        {
            // Validar se convênio existe
            await ValidateEntity.EnsureEntityExistsAsync(_convenioRepo, convenioId, "Convênio");

            // Validar tipo de documento
            if (string.IsNullOrWhiteSpace(tipoDocumento))
            {
                throw new AppException("Tipo de documento é obrigatório.", (int)HttpStatusCode.BadRequest);
            }

            // Salvar arquivo no sistema de arquivos
            var (caminhoRelativo, nomeArquivoSalvo, tamanhoBytes) =
                await _fileStorageService.SaveFileAsync(file, subfolder: convenioId.ToString());

            // Criar registro no banco
            var docConvenio = new DocConvenio
            {
                ConvenioId = convenioId,
                TipoDocumento = tipoDocumento,
                NomeArquivoOriginal = file.FileName,
                NomeArquivoSalvo = nomeArquivoSalvo,
                CaminhoArquivo = caminhoRelativo,
                TamanhoBytes = tamanhoBytes,
                Descricao = descricao,
                UploadedByUserId = _currentAuthUser.GetId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _docConvenioRepo.CreateAsync(docConvenio);

            await _createSystemLog.ExecuteAsync(
                LogActionDescribe.Create("DocConvenio", docConvenio.Id));

            return DocConvenioMapper.MapToDocConvenioReadDto(docConvenio);
        }
    }
}
