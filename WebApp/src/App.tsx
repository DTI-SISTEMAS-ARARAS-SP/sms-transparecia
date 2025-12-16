import { RouterProvider } from 'react-router-dom';
import router from './routes';
import { AuthProvider, ThemeModeProvider, SnackbarProvider } from './contexts';

export default function App() {
  return (
    <ThemeModeProvider>
      <SnackbarProvider>
        <AuthProvider>
          <RouterProvider router={router} />
        </AuthProvider>
      </SnackbarProvider>
    </ThemeModeProvider>
  );
}
