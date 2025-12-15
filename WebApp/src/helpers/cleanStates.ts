import type {
  ConvenioFormValues,
  DocConvenioFormValues,
  SystemResource,
  UserFormValues,
} from '../interfaces';

interface FormStates {
  userForm: UserFormValues;
  systemResource: SystemResource;
  convenioForm: ConvenioFormValues;
  docConvenioForm: DocConvenioFormValues;
}

export const cleanStates: FormStates = {
  userForm: {
    username: '',
    email: '',
    fullName: '',
    password: '',
    permissions: [],
  },
  systemResource: {
    name: '',
    exhibitionName: '',
  },
  convenioForm: {
    numeroConvenio: '',
    titulo: '',
    descricao: '',
    orgaoConcedente: '',
    dataPublicacaoDiario: '',
    dataVigenciaInicio: '',
    dataVigenciaFim: '',
    status: true,
  },
  docConvenioForm: {
    convenioId: 0,
    tipoDocumento: '',
    descricao: '',
  },
};
