using Api.Middlewares;
using System.Net;
using System.Text.RegularExpressions;

namespace Api.Services
{
    public class FileStorageService
    {
        private readonly string _basePath;
        private readonly long _maxFileSizeBytes;
        private readonly string[] _allowedExtensions;

        public FileStorageService(IConfiguration configuration)
        {
            _basePath = configuration["FileStorage:BasePath"]
                ?? throw new InvalidOperationException("FileStorage:BasePath não configurado.");

            var maxFileSizeMB = configuration.GetValue<int?>("FileStorage:MaxFileSizeMB") ?? 10;
            _maxFileSizeBytes = maxFileSizeMB * 1024 * 1024;

            _allowedExtensions = configuration.GetSection("FileStorage:AllowedExtensions")
                .Get<string[]>() ?? new[] { ".pdf" };

            // Criar diretório base se não existir
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }

        /// <summary>
        /// Salva um arquivo no sistema de arquivos
        /// </summary>
        /// <param name="file">Arquivo a ser salvo</param>
        /// <param name="subfolder">Subpasta adicional (opcional)</param>
        /// <returns>Caminho relativo do arquivo salvo</returns>
        public async Task<(string CaminhoRelativo, string NomeArquivoSalvo, long TamanhoBytes)> SaveFileAsync(
            IFormFile file,
            string subfolder = "")
        {
            // Validações
            ValidateFile(file);

            // Estrutura: /2024/12/[subfolder]/guid-nome.pdf
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            var folderPath = string.IsNullOrWhiteSpace(subfolder)
                ? Path.Combine(_basePath, year.ToString(), month.ToString("00"))
                : Path.Combine(_basePath, year.ToString(), month.ToString("00"), subfolder);

            // Criar diretório se não existir
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Gerar nome único
            var sanitizedFileName = SanitizeFileName(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}-{sanitizedFileName}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            // Salvar arquivo
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Retornar caminho relativo
            var relativePath = string.IsNullOrWhiteSpace(subfolder)
                ? Path.Combine(year.ToString(), month.ToString("00"), uniqueFileName)
                : Path.Combine(year.ToString(), month.ToString("00"), subfolder, uniqueFileName);

            return (relativePath, uniqueFileName, file.Length);
        }

        /// <summary>
        /// Lê um arquivo do sistema de arquivos
        /// </summary>
        /// <param name="relativePath">Caminho relativo do arquivo</param>
        /// <returns>Bytes do arquivo</returns>
        public async Task<byte[]> ReadFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_basePath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new AppException("Arquivo não encontrado.", (int)HttpStatusCode.NotFound);
            }

            return await File.ReadAllBytesAsync(fullPath);
        }

        /// <summary>
        /// Verifica se um arquivo existe
        /// </summary>
        /// <param name="relativePath">Caminho relativo do arquivo</param>
        /// <returns>True se existe, False caso contrário</returns>
        public bool FileExists(string relativePath)
        {
            var fullPath = Path.Combine(_basePath, relativePath);
            return File.Exists(fullPath);
        }

        /// <summary>
        /// Deleta um arquivo do sistema de arquivos
        /// </summary>
        /// <param name="relativePath">Caminho relativo do arquivo</param>
        public void DeleteFile(string relativePath)
        {
            var fullPath = Path.Combine(_basePath, relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// Valida se o arquivo atende aos requisitos
        /// </summary>
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new AppException("Arquivo inválido ou vazio.", (int)HttpStatusCode.BadRequest);
            }

            // Validar tamanho
            if (file.Length > _maxFileSizeBytes)
            {
                var maxSizeMB = _maxFileSizeBytes / (1024 * 1024);
                throw new AppException(
                    $"Arquivo muito grande. Tamanho máximo permitido: {maxSizeMB}MB.",
                    (int)HttpStatusCode.BadRequest);
            }

            // Validar extensão
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                throw new AppException(
                    $"Tipo de arquivo não permitido. Apenas {string.Join(", ", _allowedExtensions)} são aceitos.",
                    (int)HttpStatusCode.BadRequest);
            }

            // Validar MIME type (segurança adicional)
            var allowedMimeTypes = new[] { "application/pdf" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                throw new AppException(
                    "Tipo de conteúdo não permitido. Apenas PDF é aceito.",
                    (int)HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Sanitiza o nome do arquivo para evitar problemas de segurança
        /// </summary>
        private string SanitizeFileName(string fileName)
        {
            // Remove caracteres especiais e espaços
            var sanitized = Regex.Replace(fileName, @"[^a-zA-Z0-9_\-\.]", "-");

            // Remove múltiplos hífens consecutivos
            sanitized = Regex.Replace(sanitized, @"-+", "-");

            // Limita o tamanho do nome (mantém extensão)
            var extension = Path.GetExtension(sanitized);
            var nameWithoutExtension = Path.GetFileNameWithoutExtension(sanitized);

            if (nameWithoutExtension.Length > 100)
            {
                nameWithoutExtension = nameWithoutExtension.Substring(0, 100);
            }

            return nameWithoutExtension + extension;
        }
    }
}
