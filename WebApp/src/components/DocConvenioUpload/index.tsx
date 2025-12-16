import { useState, useRef } from 'react';
import { Box, TextField, Button, Typography, IconButton } from '@mui/material';
import { Close } from '@mui/icons-material';
import { useDocConvenios } from '../../hooks';
import { formatFileSize } from '../../helpers';

interface Props {
  convenioId: number;
  onUploadSuccess: () => void;
}

export default function DocConvenioUpload({ convenioId, onUploadSuccess }: Props) {
  const [file, setFile] = useState<File | null>(null);
  const [tipoDocumento, setTipoDocumento] = useState('');
  const [descricao, setDescricao] = useState('');
  const [uploading, setUploading] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);
  const { uploadDocumento } = useDocConvenios();

  async function handleUpload() {
    if (!file || !tipoDocumento) return;

    setUploading(true);
    try {
      await uploadDocumento(convenioId, file, tipoDocumento, descricao);
      alert('Documento enviado com sucesso!');
      setFile(null);
      setTipoDocumento('');
      setDescricao('');
      onUploadSuccess();
    } catch (err) {
      console.error(err);
      alert('Erro ao enviar documento');
    } finally {
      setUploading(false);
    }
  }

  function handleFileChange(e: React.ChangeEvent<HTMLInputElement>) {
    const selectedFile = e.target.files?.[0] || null;
    setFile(selectedFile);
  }

  return (
    <Box
      sx={{
        border: '1px solid #ccc',
        borderRadius: 1,
        p: 2,
        display: 'flex',
        flexDirection: 'column',
        gap: 2,
        mb: 3,
      }}
    >
      <Typography variant="h6">Adicionar Documento</Typography>

      <TextField
        label="Tipo de Documento"
        value={tipoDocumento}
        onChange={(e) => setTipoDocumento(e.target.value)}
        required
        fullWidth
        placeholder="Ex: Contrato, Aditivo, Prestação de Contas"
      />

      <TextField
        label="Descrição"
        value={descricao}
        onChange={(e) => setDescricao(e.target.value)}
        multiline
        rows={2}
        fullWidth
      />

      <Box>
        <input
          type="file"
          accept=".pdf"
          onChange={handleFileChange}
          style={{ display: 'none' }}
          ref={fileInputRef}
        />
        <Button
          variant="outlined"
          onClick={() => fileInputRef.current?.click()}
          disabled={uploading}
        >
          Selecionar PDF
        </Button>
      </Box>

      {file && (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <Typography>{file.name}</Typography>
          <Typography variant="caption" color="text.secondary">
            ({formatFileSize(file.size)})
          </Typography>
          <IconButton
            size="small"
            onClick={() => setFile(null)}
            disabled={uploading}
          >
            <Close />
          </IconButton>
        </Box>
      )}

      <Button
        variant="contained"
        onClick={handleUpload}
        disabled={!file || !tipoDocumento || uploading}
      >
        {uploading ? 'Enviando...' : 'Upload'}
      </Button>
    </Box>
  );
}
