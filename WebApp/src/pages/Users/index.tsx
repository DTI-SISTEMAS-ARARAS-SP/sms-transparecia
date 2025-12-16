import { useState } from 'react';
import { Container } from '@mui/material';
import {
  PageTitle,
  UserEditionModal,
  UserForm,
  UsersTable,
} from '../../components';
import type { UserFormValues, UserRead } from '../../interfaces';
import { useUsers, useSnackbar } from '../../hooks';
import { PermissionsMap } from '../../permissions';

export default function Users() {
  const { addUser, editUser, removeUser } = useUsers();
  const { showSnackbar } = useSnackbar();
  const [editingUser, setEditingUser] = useState<UserRead | null>(null);
  const [open, setOpen] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  async function handleCreate(user: UserFormValues) {
    try {
      await addUser(user);
      showSnackbar('Usuário cadastrado com sucesso!', 'success');
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao cadastrar usuário', 'error');
    }
  }

  async function handleUpdate(user: UserFormValues) {
    if (!editingUser) return;
    try {
      await editUser({ ...editingUser, ...user });
      showSnackbar('Usuário atualizado com sucesso!', 'success');
      setOpen(false);
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao atualizar usuário', 'error');
    }
  }

  async function handleDelete(id: number) {
    const confirmDelete = confirm(
      `Tem certeza que deseja excluir o usuário selecionado?`
    );
    if (!confirmDelete) return;

    try {
      await removeUser(id);
      showSnackbar('Usuário excluído com sucesso!', 'success');
      setRefreshTrigger(prev => prev + 1);
    } catch (err) {
      console.error(err);
      showSnackbar('Erro ao excluir usuário', 'error');
    }
  }

  function handleOpenEditionModal(user: UserRead) {
    setEditingUser(user);
    setOpen(true);
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
        icon={PermissionsMap.USERS}
        title="Gerenciamento de Usuários"
      />

      <UserForm onSubmit={handleCreate} />

      <UsersTable onEdit={handleOpenEditionModal} onDelete={handleDelete} refreshTrigger={refreshTrigger} />

      <UserEditionModal
        open={open}
        user={editingUser}
        onClose={() => setOpen(false)}
        onSubmit={handleUpdate}
      />
    </Container>
  );
}
