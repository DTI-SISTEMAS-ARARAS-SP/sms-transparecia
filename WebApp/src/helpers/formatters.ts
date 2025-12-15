/**
 * Formata uma data ISO string para o formato brasileiro (dd/mm/yyyy)
 * @param isoDate - Data em formato ISO
 * @returns Data formatada ou string vazia se inválida
 */
export function formatDate(isoDate?: string): string {
  if (!isoDate) return '';

  try {
    const date = new Date(isoDate);
    if (isNaN(date.getTime())) return '';

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();

    return `${day}/${month}/${year}`;
  } catch {
    return '';
  }
}

/**
 * Formata uma data ISO string para o formato brasileiro com hora (dd/mm/yyyy HH:mm)
 * @param isoDate - Data em formato ISO
 * @returns Data e hora formatadas ou string vazia se inválida
 */
export function formatDateTime(isoDate?: string): string {
  if (!isoDate) return '';

  try {
    const date = new Date(isoDate);
    if (isNaN(date.getTime())) return '';

    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}`;
  } catch {
    return '';
  }
}

/**
 * Formata tamanho de arquivo em bytes para formato legível (KB, MB, etc)
 * @param bytes - Tamanho em bytes
 * @returns Tamanho formatado
 */
export function formatFileSize(bytes: number): string {
  if (bytes === 0) return '0 Bytes';

  const k = 1024;
  const sizes = ['Bytes', 'KB', 'MB', 'GB'];
  const i = Math.floor(Math.log(bytes) / Math.log(k));

  return `${parseFloat((bytes / Math.pow(k, i)).toFixed(2))} ${sizes[i]}`;
}

/**
 * Formata status booleano para texto
 * @param status - Status booleano
 * @returns "Ativo" ou "Inativo"
 */
export function formatStatus(status: boolean): string {
  return status ? 'Ativo' : 'Inativo';
}

/**
 * Converte data do formato brasileiro (dd/mm/yyyy) para ISO string
 * @param brDate - Data no formato dd/mm/yyyy
 * @returns Data em formato ISO ou null se inválida
 */
export function brDateToISO(brDate: string): string | null {
  if (!brDate) return null;

  const parts = brDate.split('/');
  if (parts.length !== 3) return null;

  const [day, month, year] = parts;
  const date = new Date(Number(year), Number(month) - 1, Number(day));

  if (isNaN(date.getTime())) return null;

  return date.toISOString();
}
