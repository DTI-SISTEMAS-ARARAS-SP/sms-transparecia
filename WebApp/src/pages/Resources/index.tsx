import { useState } from 'react';
import { Container } from '@mui/material';
import type { SystemResource } from '../../interfaces';
import { useSystemResources, useSnackbar } from '../../hooks';
import {
  SystemResourceForm,
  SystemResourcesTable,
  SystemResourceEditionModal,
  PageTitle,
} from '../../components';
import { PermissionsMap } from '../../permissions';

export default function Resources() {
  const {
    addSystemResource,
    editSystemResource,
    removeSystemResource,
  } = useSystemResources();
  const { showSnackbar } = useSnackbar();

  const [editingResource, setEditingResource] = useState<SystemResource | null>(
    null
  );
  const [open, setOpen] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  async function handleCreate(resource: SystemResource) {
    try {
      await addSystemResource(resource);
      showSnackbar('Recurso criado com sucesso!', 'success');
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao criar recurso', 'error');
    }
  }

  async function handleUpdate(resource: SystemResource) {
    if (!editingResource) return;
    try {
      await editSystemResource({ ...editingResource, ...resource });
      showSnackbar('Recurso atualizado com sucesso!', 'success');
      setOpen(false);
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao atualizar recurso', 'error');
    }
  }

  async function handleDelete(id: number) {
    const confirmDelete = confirm(
      'Tem certeza que deseja excluir este recurso?'
    );
    if (!confirmDelete) return;

    try {
      await removeSystemResource(id.toString());
      showSnackbar('Recurso excluído com sucesso!', 'success');
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao excluir recurso', 'error');
    }
  }

  function handleOpenEditionModal(resource: SystemResource) {
    setEditingResource(resource);
    setOpen(true);
  }

  return (
    <Container
      sx={{
        mt: 4,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        textAlign: 'center',
      }}
    >
      <PageTitle
        icon={PermissionsMap.RESOURCES}
        title="Gerenciamento de Recursos"
      />

      <SystemResourceForm onSubmit={handleCreate} />

      <SystemResourcesTable
        onEdit={handleOpenEditionModal}
        onDelete={handleDelete}
        refreshTrigger={refreshTrigger}
      />

      <SystemResourceEditionModal
        open={open}
        resource={editingResource}
        onClose={() => setOpen(false)}
        onSubmit={handleUpdate}
      />
    </Container>
  );
}
