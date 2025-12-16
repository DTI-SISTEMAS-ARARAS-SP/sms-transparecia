import { Modal, Box, Typography, Button } from '@mui/material';
import type { ConvenioRead, ConvenioFormValues } from '../../interfaces';
import ConvenioForm from '../ConvenioForm';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClose } from '@fortawesome/free-solid-svg-icons';

interface Props {
  open: boolean;
  convenio: ConvenioRead | null;
  onClose: () => void;
  onSubmit: (data: ConvenioFormValues) => void;
}

export default function ConvenioEditionModal({
  open,
  convenio,
  onClose,
  onSubmit,
}: Props) {
  if (!convenio) return null;

  return (
    <Modal open={open} onClose={onClose}>
      <Box
        sx={{
          bgcolor: 'background.paper',
          p: 4,
          borderRadius: 2,
          m: 'auto',
          mt: '5vh',
          mb: '5vh',
          width: 600,
          maxWidth: '90vw',
          maxHeight: '90vh',
          overflow: 'auto',
        }}
      >
        <Box display="flex" justifyContent="flex-end">
          <Button onClick={onClose}>
            <FontAwesomeIcon icon={faClose} />
          </Button>
        </Box>

        <Typography variant="h6" gutterBottom>
          Editar Convênio
        </Typography>

        <ConvenioForm convenio={convenio} onSubmit={onSubmit} />
      </Box>
    </Modal>
  );
}
