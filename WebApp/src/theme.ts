import { createTheme } from '@mui/material/styles';

export const defineTheme = (mode: 'light' | 'dark') =>
  createTheme({
    palette: {
      mode,
      ...(mode === 'light'
        ? {
            // Cores do modo claro
            primary: { main: '#1976d2' },
            secondary: { main: '#2e7d32' },
            background: { default: '#f5f5f5', paper: '#fff' },
            text: { primary: '#000', secondary: '#555' },
          }
        : {
            // Cores do modo escuro
            primary: { main: '#90caf9' },
            secondary: { main: '#66bb6a' },
            background: { default: '#121212', paper: '#1e1e1e' },
            text: { primary: '#fff', secondary: '#aaa' },
          }),
    },
    typography: {
      fontFamily:
        'Roboto, system-ui, -apple-system, "Segoe UI", Helvetica, Arial, sans-serif',
    },
  });
