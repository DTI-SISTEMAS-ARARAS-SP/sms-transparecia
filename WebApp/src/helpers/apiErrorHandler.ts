import { AxiosError } from 'axios';

/**
 * Extrai mensagem de erro de uma resposta Axios
 * @param error - Erro capturado
 * @param fallbackMessage - Mensagem padrão caso não consiga extrair
 * @returns Mensagem de erro formatada
 */
export function getErrorMessage(error: unknown, fallbackMessage: string = 'Erro desconhecido'): string {
  if (error instanceof AxiosError) {
    // Erro 401 - Não autorizado (token inválido/expirado)
    if (error.response?.status === 401) {
      return 'Sessão expirada. Faça login novamente.';
    }

    // Erro 403 - Proibido (sem permissão)
    if (error.response?.status === 403) {
      const data = error.response?.data;
      if (data && typeof data === 'object' && 'requiredPermissions' in data && 'userPermissions' in data) {
        return `Você não tem permissão para realizar esta ação. Necessário: [${data.requiredPermissions}], Você tem: [${data.userPermissions}]`;
      }
      return error.response?.data?.message || 'Você não tem permissão para realizar esta ação.';
    }

    // Erro 404 - Não encontrado
    if (error.response?.status === 404) {
      return 'Recurso não encontrado.';
    }

    // Erro 409 - Conflito (duplicidade)
    if (error.response?.status === 409) {
      return error.response?.data?.message || 'Conflito de dados. Verifique se o registro já existe.';
    }

    // Erros 500+ - Erro do servidor
    if (error.response?.status && error.response.status >= 500) {
      return 'Erro no servidor. Tente novamente mais tarde.';
    }

    // Mensagem customizada da API
    if (error.response?.data?.message) {
      return error.response.data.message;
    }

    // Mensagem genérica do Axios
    if (error.message) {
      return error.message;
    }
  }

  // Erros não-Axios (network, etc)
  if (error instanceof Error) {
    return error.message;
  }

  return fallbackMessage;
}
