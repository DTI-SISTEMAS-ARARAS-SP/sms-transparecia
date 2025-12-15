import api from '../../api';
import type { DocConvenioRead } from '../../interfaces';

export async function uploadDocConvenio(
  convenioId: number,
  file: File,
  tipoDocumento: string,
  descricao?: string
) {
  const formData = new FormData();
  formData.append('file', file);
  formData.append('tipoDocumento', tipoDocumento);
  if (descricao) {
    formData.append('descricao', descricao);
  }

  const { data } = await api.post<DocConvenioRead>(
    `/docconvenios/convenio/${convenioId}/upload`,
    formData,
    {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    }
  );
  return data;
}
