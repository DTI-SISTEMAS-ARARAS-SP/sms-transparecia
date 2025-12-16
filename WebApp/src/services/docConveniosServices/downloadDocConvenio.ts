import api from '../../api';

export async function downloadDocConvenio(id: number) {
  const response = await api.get(`/docconvenios/${id}/download`, {
    responseType: 'blob',
  });

  // Extrai o nome do arquivo do header Content-Disposition
  const contentDisposition = response.headers['content-disposition'];
  let filename = 'documento.pdf';

  if (contentDisposition) {
    // Tenta vários formatos de Content-Disposition
    // Formato 1: filename="arquivo.pdf"
    // Formato 2: filename=arquivo.pdf
    // Formato 3: attachment; filename="arquivo.pdf"
    // Formato 4: filename*=UTF-8''arquivo.pdf

    const filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
    const matches = filenameRegex.exec(contentDisposition);

    if (matches != null && matches[1]) {
      filename = matches[1].replace(/['"]/g, '');
    } else {
      // Fallback: tenta pegar qualquer coisa após filename=
      const simpleMatch = contentDisposition.match(/filename[*]?=['"]?([^;\r\n"']*)/);
      if (simpleMatch && simpleMatch[1]) {
        filename = decodeURIComponent(simpleMatch[1]);
      }
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
