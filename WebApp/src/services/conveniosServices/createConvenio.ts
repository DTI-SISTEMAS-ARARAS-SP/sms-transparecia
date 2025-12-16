import api from '../../api';
import type { ConvenioFormValues } from '../../interfaces';

export async function createConvenio(convenio: ConvenioFormValues) {
  const payload: any = {
    numeroConvenio: convenio.numeroConvenio,
    titulo: convenio.titulo,
    orgaoConcedente: convenio.orgaoConcedente,
    status: convenio.status,
  };

  if (convenio.descricao && convenio.descricao.trim() !== '') {
    payload.descricao = convenio.descricao;
  }

  if (convenio.dataPublicacaoDiario && convenio.dataPublicacaoDiario.trim() !== '') {
    payload.dataPublicacaoDiario = convenio.dataPublicacaoDiario;
  }

  if (convenio.dataVigenciaInicio && convenio.dataVigenciaInicio.trim() !== '') {
    payload.dataVigenciaInicio = convenio.dataVigenciaInicio;
  }

  if (convenio.dataVigenciaFim && convenio.dataVigenciaFim.trim() !== '') {
    payload.dataVigenciaFim = convenio.dataVigenciaFim;
  }

  await api.post('/convenios', payload);
}
