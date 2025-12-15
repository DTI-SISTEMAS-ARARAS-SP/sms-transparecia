export interface ConvenioFormValues {
  id?: number;
  numeroConvenio: string;
  titulo: string;
  descricao?: string;
  orgaoConcedente: string;
  dataPublicacaoDiario?: string;
  dataVigenciaInicio?: string;
  dataVigenciaFim?: string;
  status: boolean;
}
