import { useState, useCallback } from 'react';
import type { ConvenioRead, ConvenioFormValues } from '../interfaces';
import {
  listConvenios,
  createConvenio,
  updateConvenio,
  deleteConvenio,
  listConvenioById,
} from '../services';
import { getErrorMessage } from '../helpers';

export function useConvenios() {
  const [convenios, setConvenios] = useState<ConvenioRead[]>([]);
  const [pagination, setPagination] = useState({
    totalItems: 0,
    page: 1,
    pageSize: 10,
    totalPages: 1,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchConvenios = useCallback(
    async (
      page = pagination.page,
      pageSize = pagination.pageSize,
      searchKey = ''
    ) => {
      setLoading(true);
      setError(null);
      try {
        const data = await listConvenios(page, pageSize, searchKey);
        setConvenios(data.data);
        setPagination({
          totalItems: data.totalItems,
          page: data.page,
          pageSize: data.pageSize,
          totalPages: data.totalPages,
        });
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao listar convênios:', err);
      } finally {
        setLoading(false);
      }
    },
    [pagination.page, pagination.pageSize]
  );

  const searchConvenios = useCallback(
    async (key: string) => {
      setLoading(true);
      setError(null);
      try {
        const data = await listConvenios(1, pagination.pageSize, key);
        setConvenios(data.data);
        setPagination({
          totalItems: data.totalItems,
          page: data.page,
          pageSize: data.pageSize,
          totalPages: data.totalPages,
        });
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao buscar convênios:', err);
      } finally {
        setLoading(false);
      }
    },
    [pagination.pageSize]
  );

  const addConvenio = useCallback(
    async (convenio: ConvenioFormValues) => {
      setLoading(true);
      setError(null);
      try {
        await createConvenio(convenio);
        await fetchConvenios();
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao criar convênio:', err);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchConvenios]
  );

  const editConvenio = useCallback(
    async (id: number, convenio: ConvenioFormValues) => {
      setLoading(true);
      setError(null);
      try {
        await updateConvenio(id, convenio);
        await fetchConvenios();
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao atualizar convênio:', err);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchConvenios]
  );

  const removeConvenio = useCallback(
    async (id: number) => {
      setLoading(true);
      setError(null);
      try {
        await deleteConvenio(id);
        await fetchConvenios();
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao remover convênio:', err);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchConvenios]
  );

  const fetchConvenioById = useCallback(async (id: number) => {
    setError(null);
    try {
      return await listConvenioById(id);
    } catch (err) {
      setError(getErrorMessage(err));
      console.error('Erro ao buscar convênio:', err);
      return null;
    }
  }, []);

  return {
    convenios,
    pagination,
    loading,
    error,
    fetchConvenios,
    searchConvenios,
    addConvenio,
    editConvenio,
    removeConvenio,
    fetchConvenioById,
    setPagination,
  };
}
