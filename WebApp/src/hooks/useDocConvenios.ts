import { useState, useCallback } from 'react';
import type { DocConvenioRead } from '../interfaces';
import {
  listDocConveniosByConvenioId,
  listDocConvenioById,
  uploadDocConvenio,
  downloadDocConvenio,
  deleteDocConvenio,
} from '../services';
import { getErrorMessage } from '../helpers';

export function useDocConvenios() {
  const [documentos, setDocumentos] = useState<DocConvenioRead[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchDocumentosByConvenio = useCallback(async (convenioId: number) => {
    setLoading(true);
    setError(null);
    try {
      const data = await listDocConveniosByConvenioId(convenioId);
      setDocumentos(data);
    } catch (err) {
      setError(getErrorMessage(err));
      console.error('Erro ao listar documentos:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  const fetchDocumentoById = useCallback(async (id: number) => {
    setError(null);
    try {
      return await listDocConvenioById(id);
    } catch (err) {
      setError(getErrorMessage(err));
      console.error('Erro ao buscar documento:', err);
      return null;
    }
  }, []);

  const uploadDocumento = useCallback(
    async (
      convenioId: number,
      file: File,
      tipoDocumento: string,
      descricao?: string
    ) => {
      setLoading(true);
      setError(null);
      try {
        const newDoc = await uploadDocConvenio(
          convenioId,
          file,
          tipoDocumento,
          descricao
        );
        await fetchDocumentosByConvenio(convenioId);
        return newDoc;
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao fazer upload de documento:', err);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchDocumentosByConvenio]
  );

  const downloadDocumento = useCallback(async (id: number) => {
    setError(null);
    try {
      await downloadDocConvenio(id);
    } catch (err) {
      setError(getErrorMessage(err));
      console.error('Erro ao baixar documento:', err);
      throw err;
    }
  }, []);

  const removeDocumento = useCallback(
    async (id: number, convenioId: number) => {
      setLoading(true);
      setError(null);
      try {
        await deleteDocConvenio(id);
        await fetchDocumentosByConvenio(convenioId);
      } catch (err) {
        setError(getErrorMessage(err));
        console.error('Erro ao remover documento:', err);
        throw err;
      } finally {
        setLoading(false);
      }
    },
    [fetchDocumentosByConvenio]
  );

  return {
    documentos,
    loading,
    error,
    fetchDocumentosByConvenio,
    fetchDocumentoById,
    uploadDocumento,
    downloadDocumento,
    removeDocumento,
  };
}
