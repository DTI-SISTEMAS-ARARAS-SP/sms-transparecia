import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          // Bibliotecas principais do React
          vendor: ['react', 'react-dom', 'react-router-dom'],
          // Material-UI e dependências
          mui: [
            '@mui/material',
            '@mui/icons-material',
            '@emotion/react',
            '@emotion/styled',
          ],
          // Axios para chamadas HTTP
          axios: ['axios'],
          // FontAwesome (se usado)
          fontawesome: [
            '@fortawesome/react-fontawesome',
            '@fortawesome/free-solid-svg-icons',
          ],
        },
      },
    },
  },
})
