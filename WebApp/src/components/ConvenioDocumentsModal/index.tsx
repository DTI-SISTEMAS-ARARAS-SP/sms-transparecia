import { Modal, Box, Typography, Button } from '@mui/material';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClose } from '@fortawesome/free-solid-svg-icons';
import DocConvenioUpload from '../DocConvenioUpload';
import DocConveniosTable from '../DocConveniosTable';

interface Props {
  open: boolean;
  convenioId: number | null;
  onClose: () => void;
}

export default function ConvenioDocumentsModal({
  open,
  convenioId,
  onClose,
}: Props) {
  if (!convenioId) return null;

  function handleUploadSuccess() {
    // A tabela será atualizada automaticamente pelo hook
  }

  return (
    <Modal open={open} onClose={onClose}>
      <Box
        sx={{
          bgcolor: 'background.paper',
          p: 4,
          borderRadius: 2,
          m: 'auto',
          mt: '5vh',
          width: 800,
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
          Documentos do Convênio
        </Typography>

        <DocConvenioUpload
          convenioId={convenioId}
          onUploadSuccess={handleUploadSuccess}
        />

        <DocConveniosTable convenioId={convenioId} />
      </Box>
    </Modal>
  );
}
