export interface ReqRegistrarPersona {
  persona: {
    numCedula: number;
    fechaNacimiento: string;
    primerNombre: string;
    segundoNombre: string;
    correo: string;
    direccion: string;
    telefono1: number;
    telefono2: number;
    fechaRegistro: string;
    idRol: number;
    puesto: string;
    cedulaResponsable: number | null;
  };
}

/** Respuesta de registrarPersona */
export interface ResRegistrarPersona {
  resultado: boolean;
  listaDeErrores: string[];
}

