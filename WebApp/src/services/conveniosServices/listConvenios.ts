import api from '../../api';
import type { ConvenioRead, ConveniosPagination } from '../../interfaces';

export async function listConvenios(
  pageNumber = 1,
  pageSize = 10,
  searchKey: string = ''
) {
  const defaultParams = `page=${pageNumber}&pageSize=${pageSize}`;

  const endpoint = searchKey
    ? `/convenios/search?key=${encodeURIComponent(searchKey)}&${defaultParams}`
    : `/convenios?${defaultParams}`;

  const { data } = await api.get<ConveniosPagination>(endpoint);
  return data;
}

export async function listConvenioById(id: number) {
  const { data } = await api.get<ConvenioRead>(`/convenios/${id}`);
  return data;
}
