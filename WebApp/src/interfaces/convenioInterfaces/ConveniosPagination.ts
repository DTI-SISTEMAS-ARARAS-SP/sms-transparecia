import type { ConvenioRead } from './ConvenioRead';

export interface ConveniosPagination {
  totalItems: number;
  page: number;
  pageSize: number;
  totalPages: number;
  data: ConvenioRead[];
}
