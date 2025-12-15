import api from '../../api';

export async function deleteDocConvenio(id: number) {
  await api.delete(`/docconvenios/${id}`);
}
