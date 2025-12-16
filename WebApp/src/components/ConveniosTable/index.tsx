import { useState, useEffect } from "react";
import {
  Box,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  TextField,
  Typography,
  IconButton,
} from "@mui/material";
import { Edit, Delete, AttachFile, Search } from "@mui/icons-material";
import { useConvenios } from "../../hooks";
import { formatDate, formatStatus } from "../../helpers";

interface Props {
  onEdit: (convenio: any) => void;
  onDelete: (id: number) => void;
  onManageDocs: (convenioId: number) => void;
  refreshTrigger?: number;
}

export default function ConveniosTable({
  onEdit,
  onDelete,
  onManageDocs,
  refreshTrigger,
}: Props) {
  const [searchKey, setSearchKey] = useState("");
  const { convenios, pagination, loading, fetchConvenios, setPagination } =
    useConvenios();

  useEffect(() => {
    fetchConvenios(pagination.page, pagination.pageSize, searchKey);
  }, [
    fetchConvenios,
    pagination.page,
    pagination.pageSize,
    searchKey,
    refreshTrigger,
  ]);

  function handleSearchSubmit(e: React.FormEvent) {
    e.preventDefault();
    setPagination((prev) => ({ ...prev, page: 1 }));
    fetchConvenios(1, pagination.pageSize, searchKey);
  }

  function handleChangePage(_: unknown, newPage: number) {
    setPagination((prev) => ({ ...prev, page: newPage + 1 }));
  }

  function handleChangeRowsPerPage(e: React.ChangeEvent<HTMLInputElement>) {
    setPagination({
      ...pagination,
      pageSize: parseInt(e.target.value, 10),
      page: 1,
    });
  }

  return (
    <Paper sx={{ p: 2, width: "100%", maxWidth: 1200 }}>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          mb: 2,
        }}
      >
        <Typography variant="h6">Convênios cadastrados</Typography>
        <Box
          component="form"
          onSubmit={handleSearchSubmit}
          sx={{ display: "flex", alignItems: "center" }}
        >
          <TextField
            size="small"
            placeholder="Buscar convênio..."
            value={searchKey}
            onChange={(e) => setSearchKey(e.target.value)}
            slotProps={{
              input: {
                endAdornment: (
                  <IconButton type="submit" size="small">
                    <Search />
                  </IconButton>
                ),
              },
            }}
          />
        </Box>
      </Box>

      <TableContainer>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Número</TableCell>
              <TableCell>Título</TableCell>
              <TableCell>Órgão Concedente</TableCell>
              <TableCell>Vigência Início</TableCell>
              <TableCell>Vigência Fim</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="center">Documentos</TableCell>
              <TableCell align="right">Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {loading ? (
              <TableRow>
                <TableCell colSpan={9} align="center">
                  Carregando...
                </TableCell>
              </TableRow>
            ) : convenios.length > 0 ? (
              convenios.map((convenio) => (
                <TableRow key={convenio.id} hover>
                  <TableCell>{convenio.id}</TableCell>
                  <TableCell>{convenio.numeroConvenio}</TableCell>
                  <TableCell>{convenio.titulo}</TableCell>
                  <TableCell>{convenio.orgaoConcedente}</TableCell>
                  <TableCell>
                    {formatDate(convenio.dataVigenciaInicio)}
                  </TableCell>
                  <TableCell>{formatDate(convenio.dataVigenciaFim)}</TableCell>
                  <TableCell>{formatStatus(convenio.status)}</TableCell>
                  <TableCell align="center">
                    {convenio.totalDocumentos}
                  </TableCell>
                  <TableCell align="right">
                    <IconButton
                      color="primary"
                      onClick={() => onEdit(convenio)}
                      size="small"
                    >
                      <Edit />
                    </IconButton>
                    <IconButton
                      color="primary"
                      onClick={() => onManageDocs(convenio.id)}
                      size="small"
                    >
                      <AttachFile />
                    </IconButton>
                    <IconButton
                      color="error"
                      onClick={() => onDelete(convenio.id)}
                      size="small"
                    >
                      <Delete />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={9} align="center">
                  Nenhum convênio encontrado
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <TablePagination
        component="div"
        count={pagination.totalItems}
        page={pagination.page - 1}
        onPageChange={handleChangePage}
        rowsPerPage={pagination.pageSize}
        onRowsPerPageChange={handleChangeRowsPerPage}
        labelRowsPerPage="Itens por página:"
        rowsPerPageOptions={[5, 10, 25]}
      />
    </Paper>
  );
}
