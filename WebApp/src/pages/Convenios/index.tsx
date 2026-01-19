import { useState } from "react";
import { Container } from "@mui/material";
import PageTitle from "../../components/PageTitle";
import ConvenioForm from "../../components/ConvenioForm";
import ConveniosTable from "../../components/ConveniosTable";
import ConvenioEditionModal from "../../components/ConvenioEditionModal";
import ConvenioDocumentsModal from "../../components/ConvenioDocumentsModal";
import type { ConvenioFormValues, ConvenioRead } from "../../interfaces";
import { useConvenios, useSnackbar } from "../../hooks";
import { getErrorMessage } from "../../helpers";
import { PERMISSIONS } from "../../permissions";

export default function Convenios() {
  const { addConvenio, editConvenio, removeConvenio } = useConvenios();
  const { showSnackbar } = useSnackbar();
  const [editingConvenio, setEditingConvenio] = useState<ConvenioRead | null>(
    null,
  );
  const [openEditModal, setOpenEditModal] = useState(false);
  const [openDocsModal, setOpenDocsModal] = useState(false);
  const [selectedConvenioId, setSelectedConvenioId] = useState<number | null>(
    null,
  );
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  async function handleCreate(convenio: ConvenioFormValues) {
    try {
      await addConvenio(convenio);
      showSnackbar("Convênio cadastrado com sucesso!", "success");
      setRefreshTrigger((prev) => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar(getErrorMessage("Erro ao cadastrar convênio"), "error");
    }
  }

  async function handleUpdate(convenio: ConvenioFormValues) {
    if (!editingConvenio) return;
    try {
      await editConvenio(editingConvenio.id, convenio);
      showSnackbar("Convênio atualizado com sucesso!", "success");
      setOpenEditModal(false);
      setRefreshTrigger((prev) => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar(getErrorMessage("Erro ao atualizar convênio"), "error");
    }
  }

  async function handleDelete(id: number) {
    const confirmDelete = confirm(
      `Tem certeza que deseja excluir o convênio selecionado?`,
    );
    if (!confirmDelete) return;

    try {
      await removeConvenio(id);
      showSnackbar("Convênio excluído com sucesso!", "success");
      setRefreshTrigger((prev) => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar(getErrorMessage("Erro ao excluir convênio"), "error");
    }
  }

  function handleOpenEditModal(convenio: ConvenioRead) {
    setEditingConvenio(convenio);
    setOpenEditModal(true);
  }

  function handleOpenDocsModal(convenioId: number) {
    setSelectedConvenioId(convenioId);
    setOpenDocsModal(true);
  }

  return (
    <Container
      sx={{
        mt: 4,
        alignItems: "center",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        textAlign: "center",
      }}
    >
      <PageTitle
        icon={PERMISSIONS.CONVENIOS}
        title="Gerenciamento de Convênios"
      />

      <ConvenioForm onSubmit={handleCreate} />

      <ConveniosTable
        onEdit={handleOpenEditModal}
        onDelete={handleDelete}
        onManageDocs={handleOpenDocsModal}
        refreshTrigger={refreshTrigger}
      />

      <ConvenioEditionModal
        open={openEditModal}
        convenio={editingConvenio}
        onClose={() => setOpenEditModal(false)}
        onSubmit={handleUpdate}
      />

      <ConvenioDocumentsModal
        open={openDocsModal}
        convenioId={selectedConvenioId}
        onClose={() => setOpenDocsModal(false)}
      />
    </Container>
  );
}
