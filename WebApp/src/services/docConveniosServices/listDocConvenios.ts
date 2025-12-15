import api from '../../api';
import type { DocConvenioRead } from '../../interfaces';

export async function listDocConveniosByConvenioId(convenioId: number) {
  const { data } = await api.get<DocConvenioRead[]>(`/docconvenios/convenio/${convenioId}`);
  return data;
}

export async function listDocConvenioById(id: number) {
  const { data } = await api.get<DocConvenioRead>(`/docconvenios/${id}`);
  return data;
}
