import { DocConvenioRead } from '../docConvenioInterfaces/DocConvenioRead';

export interface ConvenioRead {
  id: number;
  numeroConvenio: string;
  titulo: string;
  descricao?: string;
  orgaoConcedente: string;
  dataPublicacaoDiario?: string;
  dataVigenciaInicio?: string;
  dataVigenciaFim?: string;
  status: boolean;
  createdByUserId: number;
  createdAt: string;
  updatedAt: string;
  totalDocumentos: number;
  documentos?: DocConvenioRead[];
}
