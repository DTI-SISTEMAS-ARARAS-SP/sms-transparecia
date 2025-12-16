import { useState, useEffect } from 'react';
import { Box, TextField, Button, FormControlLabel, Switch, FormHelperText } from '@mui/material';
import type { ConvenioFormValues, ConvenioRead } from '../../interfaces';
import { cleanStates } from '../../helpers';

interface Props {
  onSubmit: (convenio: ConvenioFormValues) => void;
  convenio?: ConvenioRead;
}

export default function ConvenioForm({ onSubmit, convenio }: Props) {
  const [form, setForm] = useState(cleanStates.convenioForm);
  const [error, setError] = useState('');

  useEffect(() => {
    if (convenio) {
      setForm({
        numeroConvenio: convenio.numeroConvenio,
        titulo: convenio.titulo,
        descricao: convenio.descricao || '',
        orgaoConcedente: convenio.orgaoConcedente,
        dataPublicacaoDiario: convenio.dataPublicacaoDiario || '',
        dataVigenciaInicio: convenio.dataVigenciaInicio || '',
        dataVigenciaFim: convenio.dataVigenciaFim || '',
        status: convenio.status,
      });
    }
  }, [convenio]);

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === 'checkbox' ? checked : value });

    // Limpar erro ao mudar datas
    if ((name === 'dataVigenciaInicio' || name === 'dataVigenciaFim') && error) {
      setError('');
    }
  }

  function handleSubmit(e: React.FormEvent) {
    e.preventDefault();

    // Validar datas de vigência
    if (form.dataVigenciaInicio && form.dataVigenciaFim) {
      const dataInicio = new Date(form.dataVigenciaInicio);
      const dataFim = new Date(form.dataVigenciaFim);

      if (dataFim < dataInicio) {
        setError('A data de vigência fim não pode ser anterior à data de vigência início.');
        return;
      }
    }

    setError('');
    onSubmit(form);
    setForm(cleanStates.convenioForm);
  }

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        display: 'flex',
        flexDirection: 'column',
        gap: 2,
        maxWidth: 600,
        width: '100%',
      }}
    >
      <TextField
        label="Número do Convênio"
        name="numeroConvenio"
        value={form.numeroConvenio}
        onChange={handleChange}
        required
        fullWidth
      />

      <TextField
        label="Título"
        name="titulo"
        value={form.titulo}
        onChange={handleChange}
        required
        fullWidth
      />

      <TextField
        label="Descrição"
        name="descricao"
        value={form.descricao}
        onChange={handleChange}
        multiline
        rows={4}
        fullWidth
      />

      <TextField
        label="Órgão Concedente"
        name="orgaoConcedente"
        value={form.orgaoConcedente}
        onChange={handleChange}
        required
        fullWidth
      />

      <TextField
        label="Data de Publicação no Diário"
        name="dataPublicacaoDiario"
        type="date"
        value={form.dataPublicacaoDiario}
        onChange={handleChange}
        InputLabelProps={{ shrink: true }}
        fullWidth
      />

      <TextField
        label="Data Vigência Início"
        name="dataVigenciaInicio"
        type="date"
        value={form.dataVigenciaInicio}
        onChange={handleChange}
        InputLabelProps={{ shrink: true }}
        fullWidth
      />

      <TextField
        label="Data Vigência Fim"
        name="dataVigenciaFim"
        type="date"
        value={form.dataVigenciaFim}
        onChange={handleChange}
        InputLabelProps={{ shrink: true }}
        inputProps={{
          min: form.dataVigenciaInicio || undefined,
        }}
        fullWidth
        error={!!error}
      />

      {error && (
        <FormHelperText error sx={{ mt: -1 }}>
          {error}
        </FormHelperText>
      )}

      <FormControlLabel
        control={
          <Switch
            name="status"
            checked={form.status}
            onChange={handleChange}
          />
        }
        label={form.status ? 'Ativo' : 'Inativo'}
      />

      <Button variant="contained" type="submit" sx={{ mb: 3 }}>
        {convenio ? 'Atualizar' : 'Cadastrar'}
      </Button>
    </Box>
  );
}
