using Api.Interfaces;
using Api.Middlewares;
using Api.Models;
using System.Net;

namespace Api.Services.DocConveniosServices
{
    public class DownloadDocConvenio
    {
        private readonly IGenericRepository<DocConvenio> _docConvenioRepo;
        private readonly FileStorageService _fileStorageService;

        public DownloadDocConvenio(
            IGenericRepository<DocConvenio> docConvenioRepo,
            FileStorageService fileStorageService)
        {
            _docConvenioRepo = docConvenioRepo;
            _fileStorageService = fileStorageService;
        }

        public async Task<(byte[] FileBytes, string FileName, string ContentType)> ExecuteAsync(int documentoId)
        {
            // Buscar documento no banco
            var documento = await _docConvenioRepo.GetByIdAsync(documentoId);

            if (documento == null)
            {
                throw new AppException("Documento não encontrado.", (int)HttpStatusCode.NotFound);
            }

            // Verificar se arquivo existe
            if (!_fileStorageService.FileExists(documento.CaminhoArquivo))
            {
                throw new AppException(
                    "Arquivo físico não encontrado no servidor.",
                    (int)HttpStatusCode.NotFound);
            }

            // Ler arquivo
            var fileBytes = await _fileStorageService.ReadFileAsync(documento.CaminhoArquivo);

            // Retornar bytes, nome original e content type
            return (fileBytes, documento.NomeArquivoOriginal, "application/pdf");
        }
    }
}
