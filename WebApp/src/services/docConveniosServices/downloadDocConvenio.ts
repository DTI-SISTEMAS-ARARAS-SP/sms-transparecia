import api from '../../api';

export async function downloadDocConvenio(id: number) {
  const response = await api.get(`/docconvenios/${id}/download`, {
    responseType: 'blob',
  });

  // Extrai o nome do arquivo do header Content-Disposition
  const contentDisposition = response.headers['content-disposition'];
  let filename = 'documento.pdf';

  if (contentDisposition) {
    const filenameMatch = contentDisposition.match(/filename="?(.+)"?/);
    if (filenameMatch && filenameMatch[1]) {
      filename = filenameMatch[1];
    }
  }

  // Cria um link temporário e dispara o download
  const url = window.URL.createObjectURL(new Blob([response.data]));
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', filename);
  document.body.appendChild(link);
  link.click();
  link.remove();
  window.URL.revokeObjectURL(url);

  return response;
}
