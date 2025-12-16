import { useState } from 'react';
import { Container } from '@mui/material';
import PageTitle from '../../components/PageTitle';
import ConvenioForm from '../../components/ConvenioForm';
import ConveniosTable from '../../components/ConveniosTable';
import ConvenioEditionModal from '../../components/ConvenioEditionModal';
import ConvenioDocumentsModal from '../../components/ConvenioDocumentsModal';
import type { ConvenioFormValues, ConvenioRead } from '../../interfaces';
import { useConvenios } from '../../hooks';
import { PermissionsMap } from '../../permissions';

export default function Convenios() {
  const { fetchConvenios, addConvenio, editConvenio, removeConvenio } = useConvenios();
  const [editingConvenio, setEditingConvenio] = useState<ConvenioRead | null>(null);
  const [openEditModal, setOpenEditModal] = useState(false);
  const [openDocsModal, setOpenDocsModal] = useState(false);
  const [selectedConvenioId, setSelectedConvenioId] = useState<number | null>(null);

  async function handleCreate(convenio: ConvenioFormValues) {
    try {
      await addConvenio(convenio);
      alert('Convênio cadastrado com sucesso!');
      fetchConvenios();
    } catch (err) {
      console.error(err);
      alert('Erro ao cadastrar convênio');
    }
  }

  async function handleUpdate(convenio: ConvenioFormValues) {
    if (!editingConvenio) return;
    try {
      await editConvenio(editingConvenio.id, convenio);
      alert('Convênio atualizado com sucesso!');
      setOpenEditModal(false);
      fetchConvenios();
    } catch (err) {
      console.error(err);
      alert('Erro ao atualizar convênio');
    }
  }

  async function handleDelete(id: number) {
    const confirmDelete = confirm(
      `Tem certeza que deseja excluir o convênio selecionado?`
    );
    if (!confirmDelete) return;

    try {
      await removeConvenio(id);
      alert('Convênio excluído com sucesso!');
      fetchConvenios();
    } catch (err) {
      console.error(err);
      alert('Erro ao excluir convênio');
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
        alignItems: 'center',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        textAlign: 'center',
      }}
    >
      <PageTitle
        icon={PermissionsMap.CONVENIOS}
        title="Gerenciamento de Convênios"
      />

      <ConvenioForm onSubmit={handleCreate} />

      <ConveniosTable
        onEdit={handleOpenEditModal}
        onDelete={handleDelete}
        onManageDocs={handleOpenDocsModal}
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
