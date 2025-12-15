import api from '../../api';

export async function deleteConvenio(id: number) {
  await api.delete(`/convenios/${id}`);
}
