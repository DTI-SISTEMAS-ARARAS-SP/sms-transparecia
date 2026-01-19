import type {
  ConvenioFormValues,
  DocConvenioFormValues,
  PaginatedResponse,
  SystemLogFiltersPayload,
  SystemResource,
  UserFormValues,
} from "../interfaces";
import { PERMISSIONS, type ValidPermission } from "../permissions";

interface FormStates {
  initialPermissionsMap: Record<string, ValidPermission>;
  logsReportFilters: Omit<SystemLogFiltersPayload, "page" | "pageSize">;
  userForm: UserFormValues;
  systemResource: SystemResource;
  convenioForm: ConvenioFormValues;
  tablePagination: Omit<PaginatedResponse<unknown>, "data">;
  docConvenioForm: DocConvenioFormValues;
}

export const cleanStates: FormStates = {
  initialPermissionsMap: PERMISSIONS,
  logsReportFilters: {
    startDate: undefined,
    endDate: undefined,
    userId: undefined,
    action: "",
  },
  userForm: {
    username: "",
    email: "",
    fullName: "",
    password: "",
    permissions: [],
  },
  systemResource: {
    name: "",
    exhibitionName: "",
  },
  tablePagination: {
    totalItems: 0,
    page: 1,
    pageSize: 10,
    totalPages: 1,
  },
  convenioForm: {
    numeroConvenio: "",
    titulo: "",
    descricao: "",
    orgaoConcedente: "",
    dataPublicacaoDiario: "",
    dataVigenciaInicio: "",
    dataVigenciaFim: "",
    status: true,
  },
  docConvenioForm: {
    convenioId: 0,
    tipoDocumento: "",
    descricao: "",
  },
};
