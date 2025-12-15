export interface DocConvenioRead {
  id: number;
  convenioId: number;
  tipoDocumento: string;
  nomeArquivoOriginal: string;
  nomeArquivoSalvo: string;
  caminhoArquivo: string;
  tamanhoBytes: number;
  descricao?: string;
  uploadedByUserId: number;
  createdAt: string;
  updatedAt: string;
}
