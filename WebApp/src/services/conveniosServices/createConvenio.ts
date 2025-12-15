import api from '../../api';
import type { ConvenioFormValues } from '../../interfaces';

export async function createConvenio(convenio: ConvenioFormValues) {
  await api.post('/convenios', convenio);
}
