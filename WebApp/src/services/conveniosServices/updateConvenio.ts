import api from '../../api';
import type { ConvenioFormValues } from '../../interfaces';

export async function updateConvenio(id: number, convenio: ConvenioFormValues) {
  await api.put(`/convenios/${id}`, convenio);
}
