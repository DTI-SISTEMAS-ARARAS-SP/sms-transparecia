import { useEffect } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Typography,
} from '@mui/material';
import { Download, Delete } from '@mui/icons-material';
import { useDocConvenios } from '../../hooks';
import { formatFileSize, formatDateTime } from '../../helpers';

interface Props {
  convenioId: number;
}

export default function DocConveniosTable({ convenioId }: Props) {
  const { documentos, loading, fetchDocumentosByConvenio, downloadDocumento, removeDocumento } =
    useDocConvenios();

  useEffect(() => {
    if (convenioId) {
      fetchDocumentosByConvenio(convenioId);
    }
  }, [convenioId, fetchDocumentosByConvenio]);

  async function handleDelete(id: number) {
    const confirmDelete = confirm(
      'Tem certeza que deseja excluir este documento?'
    );
    if (!confirmDelete) return;

    try {
      await removeDocumento(id, convenioId);
      alert('Documento excluído com sucesso!');
    } catch (err) {
      console.error(err);
      alert('Erro ao excluir documento');
    }
  }

  async function handleDownload(id: number) {
    try {
      await downloadDocumento(id);
    } catch (err) {
      console.error(err);
      alert('Erro ao baixar documento');
    }
  }

  return (
    <>
      <Typography variant="h6" sx={{ mb: 2 }}>
        Documentos Anexados
      </Typography>

      <TableContainer>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Tipo</TableCell>
              <TableCell>Nome do Arquivo</TableCell>
              <TableCell>Tamanho</TableCell>
              <TableCell>Data de Upload</TableCell>
              <TableCell align="right">Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={5} align="center">
                  Carregando documentos...
                </TableCell>
              </TableRow>
            ) : documentos.length > 0 ? (
              documentos.map((doc) => (
                <TableRow key={doc.id} hover>
                  <TableCell>{doc.tipoDocumento}</TableCell>
                  <TableCell>{doc.nomeArquivoOriginal}</TableCell>
                  <TableCell>{formatFileSize(doc.tamanhoBytes)}</TableCell>
                  <TableCell>{formatDateTime(doc.createdAt)}</TableCell>
                  <TableCell align="right">
                    <IconButton
                      color="primary"
                      onClick={() => handleDownload(doc.id)}
                      size="small"
                    >
                      <Download />
                    </IconButton>
                    <IconButton
                      color="error"
                      onClick={() => handleDelete(doc.id)}
                      size="small"
                    >
                      <Delete />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={5} align="center">
                  Nenhum documento anexado
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
}
