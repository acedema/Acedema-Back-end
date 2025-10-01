-- PostgreSQL Compatible Script

--------------------------------------------------------------------------------
--                              TABLE CREATION
--------------------------------------------------------------------------------

-- Table: Roles
CREATE TABLE Roles (
                       id_rol SERIAL PRIMARY KEY,
                       nombre VARCHAR(250)
);

-- Table: Persona (Note: num_cedula changed to VARCHAR to handle potential BIGINT issues from T-SQL, though INTEGER is used for consistency with T-SQL int)
CREATE TABLE Persona (
                         id_persona SERIAL PRIMARY KEY,
                         num_cedula INTEGER NOT NULL UNIQUE, -- Assuming int in T-SQL is sufficient for this ID
                         fecha_nacimiento DATE NOT NULL,
                         primer_nombre VARCHAR(250) NOT NULL,
                         segundo_nombre VARCHAR(250),
                         primer_apellido VARCHAR(250) NOT NULL,
                         segundo_apellido VARCHAR(250) NOT NULL,
                         correo VARCHAR(250) NOT NULL UNIQUE,
                         contraseña VARCHAR(250) NOT NULL,
                         direccion VARCHAR(250) NOT NULL,
                         telefono_1 INTEGER NOT NULL,
                         telefono_2 INTEGER,
                         fecha_registro DATE NOT NULL,
                         id_Rol INTEGER NOT NULL REFERENCES Roles(id_rol),
                         puesto VARCHAR(250),
                         cedula_responsable INTEGER
);

-- Table: Actas
CREATE TABLE Actas (
                       id_acta SERIAL PRIMARY KEY,
                       id_administrador INTEGER NOT NULL REFERENCES Persona(id_persona),
                       titulo VARCHAR(250) NOT NULL,
                       descripcion VARCHAR(250) NOT NULL,
                       fecha_registro DATE NOT NULL
);

-- Table: Actas_Roles
CREATE TABLE Actas_Roles (
                             id_registro SERIAL PRIMARY KEY,
                             id_acta INTEGER NOT NULL REFERENCES Actas(id_acta),
                             id_rol INTEGER NOT NULL REFERENCES Roles(id_rol)
);

-- Table: Archivos_Acta
CREATE TABLE Archivos_Acta (
                               id_archivo_acta SERIAL PRIMARY KEY,
                               id_acta INTEGER NOT NULL REFERENCES Actas(id_acta),
                               nombre VARCHAR(250) NOT NULL,
                               tipo VARCHAR(250) NOT NULL,
                               archivo BYTEA NOT NULL
);

-- Table: Archivos_Aplicar
CREATE TABLE Archivos_Aplicar (
                                  id_archivo_aplicar SERIAL PRIMARY KEY,
                                  nombre VARCHAR(250) NOT NULL,
                                  tipo VARCHAR(250) NOT NULL,
                                  archivo BYTEA NOT NULL
);

-- Table: Foro
CREATE TABLE Foro (
                      id_foro SERIAL PRIMARY KEY,
                      id_administrador INTEGER REFERENCES Persona(id_persona),
                      titulo VARCHAR(250),
                      descripcion VARCHAR(250),
                      fecha_registro DATE,
                      EsPublico BOOLEAN NOT NULL DEFAULT TRUE, -- bit to BOOLEAN, default 1 to TRUE
                      FechaUltimaModificacion TIMESTAMP WITHOUT TIME ZONE
);

-- Table: Archivos_Foro
CREATE TABLE Archivos_Foro (
                               id_archivo_foro SERIAL PRIMARY KEY,
                               id_foro INTEGER REFERENCES Foro(id_foro),
                               nombre VARCHAR(250) NOT NULL,
                               tipo VARCHAR(250) NOT NULL,
                               archivo BYTEA NOT NULL
);

-- Table: Clases
CREATE TABLE Clases (
                        id_clase SERIAL PRIMARY KEY,
                        nombre_clase VARCHAR(250),
                        estado BOOLEAN, -- binary(1) to BOOLEAN
                        modalidad VARCHAR(250)
);

-- Table: Clases_Docente
CREATE TABLE Clases_Docente (
                                id_clase_docente SERIAL PRIMARY KEY,
                                id_clase INTEGER NOT NULL REFERENCES Clases(id_clase),
                                id_docente INTEGER NOT NULL REFERENCES Persona(id_persona), -- Assumes id_docente is a persona id
                                fecha_inicio DATE NOT NULL,
                                fecha_fin DATE NOT NULL,
                                estado BOOLEAN NOT NULL, -- binary(1) to BOOLEAN
                                Capacidad INTEGER NOT NULL,
                                Miembros INTEGER NOT NULL
);

-- Table: Matricula
CREATE TABLE Matricula (
                           id_matricula SERIAL PRIMARY KEY,
                           id_estudiante INTEGER NOT NULL REFERENCES Persona(id_persona), -- Assumes id_estudiante is a persona id
                           fecha_inicio DATE NOT NULL,
                           fecha_fin DATE NOT NULL
);

-- Table: Clases_Estudiante
CREATE TABLE Clases_Estudiante (
                                   id_clase_estudiante SERIAL PRIMARY KEY,
                                   id_estudiante INTEGER NOT NULL REFERENCES Persona(id_persona),
                                   id_clase_docente INTEGER NOT NULL REFERENCES Clases_Docente(id_clase_docente),
                                   id_matricula INTEGER NOT NULL REFERENCES Matricula(id_matricula)
);

-- Table: Asistencia_Docente
CREATE TABLE Asistencia_Docente (
                                    id_asistencia_docente SERIAL PRIMARY KEY,
                                    id_clase_docente INTEGER NOT NULL REFERENCES Clases_Docente(id_clase_docente),
                                    asistencia VARCHAR(250),
                                    fecha_registro DATE NOT NULL
);

-- Table: Asistencia_Estudiante
CREATE TABLE Asistencia_Estudiante (
                                       id_asistencia_estudiante SERIAL PRIMARY KEY,
                                       id_clase_estudiante INTEGER NOT NULL REFERENCES Clases_Estudiante(id_clase_estudiante),
                                       asistencia VARCHAR(250) NOT NULL,
                                       fecha_registro DATE NOT NULL
);

-- Table: Comprobantes_Pago
CREATE TABLE Comprobantes_Pago (
                                   id_comprobante SERIAL PRIMARY KEY,
                                   nombre VARCHAR(250) NOT NULL,
                                   tipo VARCHAR(250) NOT NULL,
                                   archivo BYTEA NOT NULL,
                                   comentario VARCHAR(250)
);

-- Table: Pagos
CREATE TABLE Pagos (
                       id_pago SERIAL PRIMARY KEY,
                       id_estudiante INTEGER NOT NULL REFERENCES Persona(id_persona),
                       id_comprobante INTEGER NOT NULL REFERENCES Comprobantes_Pago(id_comprobante),
                       id_matricula INTEGER NOT NULL REFERENCES Matricula(id_matricula),
                       estado_pago VARCHAR(250) NOT NULL,
                       tipo_pago VARCHAR(250) NOT NULL,
                       id_administrador INTEGER NOT NULL REFERENCES Persona(id_persona),
                       fecha_registro DATE NOT NULL
);

-- Table: Cronograma
CREATE TABLE Cronograma (
                            id_cronograma SERIAL PRIMARY KEY,
                            link_teams VARCHAR(250),
                            id_clase_docente INTEGER NOT NULL REFERENCES Clases_Docente(id_clase_docente),
                            fecha_registro DATE NOT NULL
);

-- Table: Semana
CREATE TABLE Semana (
                        id_semana SERIAL PRIMARY KEY,
                        id_cronograma INTEGER NOT NULL REFERENCES Cronograma(id_cronograma),
                        fecha DATE NOT NULL,
                        contenido VARCHAR(250) NOT NULL
);

-- Table: Bitacora
CREATE TABLE Bitacora (
                          id_bitacora SERIAL PRIMARY KEY,
                          id_semana INTEGER NOT NULL REFERENCES Semana(id_semana),
                          comentario VARCHAR(250) NOT NULL,
                          estado VARCHAR(250) NOT NULL,
                          fecha_hora_registro DATE NOT NULL
);

-- Table: Foro_Roles
CREATE TABLE Foro_Roles (
                            id_registro SERIAL PRIMARY KEY,
                            id_foro INTEGER NOT NULL REFERENCES Foro(id_foro),
                            id_rol INTEGER NOT NULL REFERENCES Roles(id_rol)
);

-- Table: Horarios
CREATE TABLE Horarios (
                          id_horario SERIAL PRIMARY KEY,
                          id_clase_docente INTEGER NOT NULL REFERENCES Clases_Docente(id_clase_docente),
                          dia VARCHAR(50) NOT NULL,
                          hora_inicio TIME WITHOUT TIME ZONE NOT NULL, -- time(7) to TIME WITHOUT TIME ZONE
                          hora_final TIME WITHOUT TIME ZONE NOT NULL
);

-- Table: Instrumentos
CREATE TABLE Instrumentos (
                              id_instrumento SERIAL PRIMARY KEY,
                              nombre_instrumento VARCHAR(250) NOT NULL,
                              categoria_instrumento VARCHAR(250) NOT NULL,
                              estado_instrumento VARCHAR(250) NOT NULL,
                              fecha_registro DATE NOT NULL
);

-- Table: Prestamos_instrumentos
CREATE TABLE Prestamos_instrumentos (
                                        id_prestamo SERIAL PRIMARY KEY,
                                        id_instrumento INTEGER NOT NULL UNIQUE REFERENCES Instrumentos(id_instrumento),
                                        id_estudiante INTEGER NOT NULL REFERENCES Persona(id_persona),
                                        fecha_registro DATE NOT NULL,
                                        fecha_inicio DATE NOT NULL,
                                        fecha_fin DATE NOT NULL,
                                        estado_prestamo VARCHAR(250) NOT NULL,
                                        id_administrador INTEGER NOT NULL REFERENCES Persona(id_persona)
);


--------------------------------------------------------------------------------
--                              STORED PROCEDURES (Functions)
--------------------------------------------------------------------------------

-- StoredProcedure [dbo].[Actualizar_Contrasena]
CREATE OR REPLACE FUNCTION Actualizar_Contrasena(
    p_correo VARCHAR,
    p_contrasena_actual VARCHAR,
    p_nueva_contrasena VARCHAR
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    Resultado BOOLEAN
) AS $$
DECLARE
v_id_persona INTEGER;
    v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_Resultado BOOLEAN := FALSE;
BEGIN
    -- Buscar persona con correo y contraseña actual
SELECT id_persona INTO v_id_persona
FROM Persona
WHERE correo = p_correo AND contraseña = p_contrasena_actual;

IF v_id_persona IS NULL THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'Credenciales incorrectas.';
        v_Resultado := FALSE;
ELSE
        -- Actualizar contraseña
UPDATE Persona
SET contraseña = p_nueva_contrasena
WHERE id_persona = v_id_persona;

v_Resultado := TRUE;
END IF;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_Resultado;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_Resultado := FALSE;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_Resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[ActualizarForo]
CREATE OR REPLACE FUNCTION ActualizarForo(
    p_ForoId INTEGER,
    p_IdAdmin INTEGER,
    p_Titulo VARCHAR,
    p_Descripcion VARCHAR,
    p_EsPublico BOOLEAN
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    Resultado BOOLEAN
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_Resultado BOOLEAN := FALSE;
    v_rowcount INTEGER;
BEGIN
    -- Actualizar foro
UPDATE Foro
SET titulo = p_Titulo,
    descripcion = p_Descripcion,
    espublico = p_EsPublico,
    FechaUltimaModificacion = NOW()
WHERE id_foro = p_ForoId AND id_administrador = p_IdAdmin;

GET DIAGNOSTICS v_rowcount = ROW_COUNT;

IF v_rowcount = 0 THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'No se encontró el foro o no tiene permiso para actualizarlo.';
        v_Resultado := FALSE;
ELSE
        v_Resultado := TRUE;
END IF;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_Resultado;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_Resultado := FALSE;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_Resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[ActualizarForoRoles]
CREATE OR REPLACE FUNCTION ActualizarForoRoles(
    p_IdForo INTEGER,
    p_Roles TEXT
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_RolId INTEGER;
    v_role_text TEXT;
BEGIN
    -- Primero eliminar asignaciones actuales
DELETE FROM Foro_Roles WHERE id_foro = p_IdForo;

-- Insertar los nuevos roles uno por uno
FOR v_role_text IN SELECT TRIM(UNNEST(STRING_TO_ARRAY(p_Roles, ',')))
                              LOOP
BEGIN
            v_RolId := v_role_text::INTEGER;
INSERT INTO Foro_Roles (id_foro, id_rol) VALUES (p_IdForo, v_RolId);
EXCEPTION
            WHEN invalid_text_representation THEN
                -- Handle non-integer roles if necessary, though T-SQL uses INT
                CONTINUE;
END;
END LOOP;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[ActualizarPerfilPersona]
CREATE OR REPLACE FUNCTION ActualizarPerfilPersona(
    p_id_persona INTEGER,
    p_fecha_nacimiento DATE,
    p_primer_nombre VARCHAR,
    p_segundo_nombre VARCHAR,
    p_primer_apellido VARCHAR,
    p_segundo_apellido VARCHAR,
    p_correo VARCHAR,
    p_direccion VARCHAR,
    p_telefono_1 INTEGER,
    p_telefono_2 INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
BEGIN
UPDATE Persona
SET
    fecha_nacimiento = p_fecha_nacimiento,
    primer_nombre = p_primer_nombre,
    segundo_nombre = p_segundo_nombre,
    primer_apellido = p_primer_apellido,
    segundo_apellido = p_segundo_apellido,
    correo = p_correo,
    direccion = p_direccion,
    telefono_1 = p_telefono_1,
    telefono_2 = p_telefono_2
WHERE id_persona = p_id_persona;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Buscar_Matricula]
CREATE OR REPLACE FUNCTION Buscar_Matricula(
    p_id_persona INTEGER
)
RETURNS TABLE (
    id_matricula INTEGER,
    id_estudiante INTEGER,
    fecha_inicio DATE,
    fecha_fin DATE,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    resultado BOOLEAN
) AS $$
DECLARE
v_idReturn INTEGER := NULL;
    v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_resultado BOOLEAN := FALSE;
BEGIN
    -- Verificar si existe la matrícula vigente
SELECT m.id_matricula INTO v_idReturn
FROM Matricula m
WHERE m.id_estudiante = p_id_persona
  AND NOW()::DATE BETWEEN m.fecha_inicio AND m.fecha_fin
    LIMIT 1;

IF v_idReturn IS NOT NULL THEN
        -- Retornar matrícula y variables de éxito
        v_resultado := TRUE;
RETURN QUERY
SELECT m.id_matricula, m.id_estudiante, m.fecha_inicio, m.fecha_fin, v_ErrorOccurred, v_ErrorMensaje, v_resultado
FROM Matricula m
WHERE m.id_matricula = v_idReturn;
ELSE
        -- No hay matrícula
        v_ErrorMensaje := 'El usuario no posee ninguna matrícula.';
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::DATE, v_ErrorOccurred, v_ErrorMensaje, v_resultado;
END IF;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_resultado := FALSE;
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::DATE, v_ErrorOccurred, v_ErrorMensaje, v_resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Crear_Matricular]
CREATE OR REPLACE FUNCTION Crear_Matricular(
    p_id_persona INTEGER
)
RETURNS TABLE (
    MatriculaId INTEGER,
    IdEstudiante INTEGER,
    FechaInicio DATE,
    FechaFin DATE,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_idReturn INTEGER := NULL;
    v_fecha_fin DATE;
BEGIN
    -- Validar que la persona es un estudiante (Assuming id_Rol = 2 is for students)
    IF NOT EXISTS (
        SELECT 1
        FROM Persona
        WHERE id_persona = p_id_persona AND id_Rol = 2
    ) THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'El usuario no es un estudiante válido.';
RETURN QUERY SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::DATE, v_ErrorOccurred, v_ErrorMensaje;
RETURN;
END IF;

    -- Verificar si ya existe una matrícula activa
    IF EXISTS (
        SELECT 1
        FROM Matricula
        WHERE id_estudiante = p_id_persona
          AND fecha_fin >= CURRENT_DATE
          AND fecha_inicio <= CURRENT_DATE
    ) THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'El estudiante ya tiene una matrícula vigente.';
RETURN QUERY SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::DATE, v_ErrorOccurred, v_ErrorMensaje;
RETURN;
END IF;

    -- Calculate fecha_fin as end of current year
    v_fecha_fin := MAKE_DATE(EXTRACT(YEAR FROM CURRENT_DATE)::INTEGER, 12, 31);

    -- Insertar matrícula and get the new ID
INSERT INTO Matricula (id_estudiante, fecha_inicio, fecha_fin)
VALUES (p_id_persona, CURRENT_DATE, v_fecha_fin)
    RETURNING id_matricula INTO v_idReturn;

v_ErrorMensaje := 'Matrícula registrada exitosamente.';

    -- Devolver los datos completos de la matrícula insertada
RETURN QUERY
SELECT m.id_matricula, m.id_estudiante, m.fecha_inicio, m.fecha_fin, v_ErrorOccurred, v_ErrorMensaje
FROM Matricula m
WHERE m.id_matricula = v_idReturn;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
RETURN QUERY SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::DATE, v_ErrorOccurred, v_ErrorMensaje;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Cursos_Matricula]
CREATE OR REPLACE FUNCTION Cursos_Matricula(
    p_id_matricula INTEGER
)
RETURNS TABLE (
    nombre_clase VARCHAR,
    modalidad VARCHAR,
    estado BOOLEAN,
    primer_nombre VARCHAR,
    primer_apellido VARCHAR,
    fecha_inicio DATE,
    fecha_fin DATE,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_idReturn INTEGER := p_id_matricula;
BEGIN
    -- Selecciono cursos de la matricula
RETURN QUERY
SELECT c.nombre_clase, c.modalidad, c.estado, prof.primer_nombre, prof.primer_apellido, cd.fecha_inicio, cd.fecha_fin,
       v_ErrorOccurred, 'Búsqueda de cursos realizada correctamente.'::VARCHAR, v_idReturn
FROM Clases_Estudiante ce
         INNER JOIN Clases_Docente cd ON ce.id_clase_docente = cd.id_clase_docente
         INNER JOIN Clases c ON cd.id_clase = c.id_clase
         INNER JOIN Persona prof ON cd.id_docente = prof.id_persona
WHERE ce.id_matricula = p_id_matricula;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'Error interno: ' || SQLERRM;
        v_idReturn := NULL;
RETURN QUERY
SELECT NULL::VARCHAR, NULL::VARCHAR, NULL::BOOLEAN, NULL::VARCHAR, NULL::VARCHAR, NULL::DATE, NULL::DATE,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Eliminar_Persona]
CREATE OR REPLACE FUNCTION Eliminar_Persona(
    p_ID INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := 'Usuario eliminado exitosamente';
    v_rowcount INTEGER;
BEGIN
BEGIN
DELETE FROM Persona WHERE id_persona = p_ID;
GET DIAGNOSTICS v_rowcount = ROW_COUNT;

IF v_rowcount = 0 THEN
            v_ErrorOccurred := 1;
            v_ErrorMensaje := 'No se eliminó ningún registro.';
END IF;

EXCEPTION
        WHEN OTHERS THEN
            v_ErrorOccurred := 1;
            v_ErrorMensaje := 'Error interno: ' || SQLERRM;
END;

    IF v_ErrorOccurred = 1 THEN
        v_ErrorMensaje := 'Se produjo un error al eliminar al usuario. ' || v_ErrorMensaje;
END IF;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[EliminarForo]
CREATE OR REPLACE FUNCTION EliminarForo(
    p_ForoId INTEGER,
    p_IdAdmin INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    Resultado BOOLEAN
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_Resultado BOOLEAN := FALSE;
    v_rowcount INTEGER;
BEGIN
BEGIN
        -- Primero eliminar archivos relacionados
DELETE FROM Archivos_Foro WHERE id_foro = p_ForoId;

-- Luego eliminar roles asignados
DELETE FROM Foro_Roles WHERE id_foro = p_ForoId;

-- Finalmente eliminar el foro (solo si es administrador)
DELETE FROM Foro
WHERE id_foro = p_ForoId AND id_administrador = p_IdAdmin;

GET DIAGNOSTICS v_rowcount = ROW_COUNT;

IF v_rowcount = 0 THEN
            v_ErrorOccurred := 1;
            v_ErrorMensaje := 'No se encontró el foro o no tiene permiso para eliminarlo.';
            v_Resultado := FALSE;
ELSE
            v_Resultado := TRUE;
END IF;

EXCEPTION
        WHEN OTHERS THEN
            v_ErrorOccurred := 1;
            v_ErrorMensaje := SQLERRM;
            v_Resultado := FALSE;
END;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_Resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Horario_Matricula]
CREATE OR REPLACE FUNCTION Horario_Matricula(
    p_id_persona INTEGER
)
RETURNS TABLE (
    nombre_clase VARCHAR,
    dia VARCHAR,
    hora_inicio TIME WITHOUT TIME ZONE,
    hora_final TIME WITHOUT TIME ZONE,
    primer_nombre VARCHAR,
    primer_apellido VARCHAR,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER
) AS $$
DECLARE
v_matricula INTEGER;
    v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := 'Búsqueda de horario realizada correctamente.';
    v_idReturn INTEGER := NULL;
BEGIN
    -- Guardo matricula activa
SELECT id_matricula INTO v_matricula
FROM Matricula
WHERE id_estudiante = p_id_persona
  AND CURRENT_DATE BETWEEN fecha_inicio AND fecha_fin
    LIMIT 1;

IF v_matricula IS NOT NULL THEN
        v_idReturn := v_matricula;

RETURN QUERY
SELECT c.nombre_clase, h.dia, h.hora_inicio, h.hora_final, prof.primer_nombre, prof.primer_apellido,
       v_ErrorOccurred, v_ErrorMensaje, v_idReturn
FROM Clases_Estudiante ce
         INNER JOIN Clases_Docente cd ON ce.id_clase_docente = cd.id_clase_docente
         INNER JOIN Clases c ON cd.id_clase = c.id_clase
         INNER JOIN Persona prof ON cd.id_docente = prof.id_persona
         INNER JOIN Horarios h ON cd.id_clase_docente = h.id_clase_docente
WHERE ce.id_matricula = v_matricula;
ELSE
        -- No existe
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'El usuario no posee ninguna matricula';
RETURN QUERY
SELECT NULL::VARCHAR, NULL::VARCHAR, NULL::TIME, NULL::TIME, NULL::VARCHAR, NULL::VARCHAR,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
END IF;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'Error interno: ' || SQLERRM;
        v_idReturn := NULL;
RETURN QUERY
SELECT NULL::VARCHAR, NULL::VARCHAR, NULL::TIME, NULL::TIME, NULL::VARCHAR, NULL::VARCHAR,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Insertar_Curso_Matricula]
CREATE OR REPLACE FUNCTION Insertar_Curso_Matricula(
    p_id_persona INTEGER,
    p_id_clase_docente INTEGER,
    p_id_matricula INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := 'Matricula registrada exitosamente';
    v_idReturn INTEGER := NULL;
BEGIN
BEGIN
        -- Inserto matricula
INSERT INTO Clases_Estudiante(id_estudiante, id_clase_docente, id_matricula)
VALUES (p_id_persona, p_id_clase_docente, p_id_matricula)
    RETURNING id_clase_estudiante INTO v_idReturn;

EXCEPTION
        WHEN OTHERS THEN
            v_ErrorOccurred := 1;
            v_ErrorMensaje := 'Se produjo un error al registrar la matricula. Error interno: ' || SQLERRM;
            v_idReturn := NULL;
END;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[InsertarForo]
CREATE OR REPLACE FUNCTION InsertarForo(
    p_IdAdmin INTEGER,
    p_Titulo VARCHAR,
    p_Descripcion VARCHAR,
    p_FechaRegistro TIMESTAMP WITHOUT TIME ZONE,
    p_EsPublico BOOLEAN
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    NuevoIdForo INTEGER,
    Resultado BOOLEAN
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_NuevoIdForo INTEGER := NULL;
    v_Resultado BOOLEAN := FALSE;
BEGIN
INSERT INTO Foro (id_administrador, titulo, descripcion, fecha_registro, espublico)
VALUES (p_IdAdmin, p_Titulo, p_Descripcion, p_FechaRegistro, p_EsPublico)
    RETURNING id_foro INTO v_NuevoIdForo;

v_Resultado := TRUE;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_NuevoIdForo, v_Resultado;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_Resultado := FALSE;
        v_NuevoIdForo := NULL;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_NuevoIdForo, v_Resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[InsertarForoRoles]
CREATE OR REPLACE FUNCTION InsertarForoRoles(
    p_IdForo INTEGER,
    p_IdRol INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := 'Rol asignado exitosamente.';
BEGIN
    -- valida que no exista ya la asignación para evitar duplicados
    IF NOT EXISTS (SELECT 1 FROM Foro_Roles WHERE id_foro = p_IdForo AND id_rol = p_IdRol)
    THEN
        INSERT INTO Foro_Roles (id_foro, id_rol)
        VALUES (p_IdForo, p_IdRol);
ELSE
        v_ErrorMensaje := 'Ya está guardado para un rol existente.';
END IF;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Login_Persona]
CREATE OR REPLACE FUNCTION Login_Persona(
    p_email VARCHAR,
    p_password VARCHAR
)
RETURNS TABLE (
    id_persona INTEGER,
    num_cedula INTEGER,
    fecha_nacimiento DATE,
    primer_nombre VARCHAR,
    segundo_nombre VARCHAR,
    primer_apellido VARCHAR,
    segundo_apellido VARCHAR,
    correo VARCHAR,
    direccion VARCHAR,
    telefono_1 INTEGER,
    telefono_2 INTEGER,
    id_Rol INTEGER,
    nombre_rol VARCHAR,
    puesto VARCHAR,
    cedula_responsable INTEGER,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER,
    resultado BOOLEAN
) AS $$
DECLARE
v_idReturn INTEGER := NULL;
    v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_resultado BOOLEAN := FALSE;
BEGIN
    -- Buscar el id_persona que coincide con email y password
SELECT P.id_persona INTO v_idReturn
FROM Persona P
WHERE P.correo = p_email AND P.contraseña = p_password
    LIMIT 1;

IF v_idReturn IS NOT NULL THEN
        -- Login correcto: devolver datos del usuario
        v_resultado := TRUE;

RETURN QUERY
SELECT
    P.id_persona, P.num_cedula, P.fecha_nacimiento, P.primer_nombre, P.segundo_nombre,
    P.primer_apellido, P.segundo_apellido, P.correo, P.direccion, P.telefono_1, P.telefono_2,
    P.id_Rol, R.nombre, P.puesto, P.cedula_responsable,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado
FROM Persona P
         INNER JOIN Roles R ON P.id_Rol = R.id_rol
WHERE P.id_persona = v_idReturn;
ELSE
        -- Login fallido: credenciales incorrectas
        v_ErrorMensaje := 'Credenciales incorrectas.';
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::VARCHAR, NULL::VARCHAR,
    NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER, NULL::INTEGER,
    NULL::INTEGER, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado;
END IF;

EXCEPTION
    WHEN OTHERS THEN
        -- Capturar cualquier error inesperado
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_idReturn := NULL;
        v_resultado := FALSE;
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::VARCHAR, NULL::VARCHAR,
    NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER, NULL::INTEGER,
    NULL::INTEGER, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[ObtenerForosPorRol]
CREATE OR REPLACE FUNCTION ObtenerForosPorRol(
    p_IdRol INTEGER
)
RETURNS TABLE (
    id_foro INTEGER,
    id_administrador INTEGER,
    titulo VARCHAR,
    descripcion VARCHAR,
    fecha_registro DATE,
    FechaUltimaModificacion TIMESTAMP WITHOUT TIME ZONE,
    espublico BOOLEAN
) AS $$
BEGIN
RETURN QUERY
SELECT
    f.id_foro,
    f.id_administrador,
    f.titulo,
    f.descripcion,
    f.fecha_registro,
    f.FechaUltimaModificacion,
    f.espublico
FROM Foro f
         LEFT JOIN Foro_Roles fr ON f.id_foro = fr.id_foro
WHERE
    f.espublico = TRUE
   OR fr.id_rol = p_IdRol
GROUP BY
    f.id_foro,
    f.id_administrador,
    f.titulo,
    f.descripcion,
    f.fecha_registro,
    f.FechaUltimaModificacion,
    f.espublico
ORDER BY f.fecha_registro DESC;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[Registrar_Persona]
CREATE OR REPLACE FUNCTION Registrar_Persona(
    p_num_cedula INTEGER,
    p_fecha_nacimiento DATE,
    p_primer_nombre VARCHAR,
    p_segundo_nombre VARCHAR,
    p_primer_apellido VARCHAR,
    p_segundo_apellido VARCHAR,
    p_correo VARCHAR,
    p_contraseña VARCHAR,
    p_direccion VARCHAR,
    p_telefono_1 INTEGER,
    p_telefono_2 INTEGER,
    p_id_Rol INTEGER,
    p_puesto VARCHAR,
    p_cedula_responsable INTEGER
)
RETURNS TABLE (
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER
) AS $$
DECLARE
v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := 'Usuario registrado exitosamente';
    v_idReturn INTEGER := NULL;
    v_fechaRegistro DATE := CURRENT_DATE;
BEGIN
    -- Validar cédula duplicada
    IF EXISTS (SELECT 1 FROM Persona WHERE num_cedula = p_num_cedula) THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'La cédula ya está registrada.';
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
RETURN;
END IF;

    -- Validar correo duplicado
    IF EXISTS (SELECT 1 FROM Persona WHERE correo = p_correo) THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'El correo ya está registrado.';
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
RETURN;
END IF;

INSERT INTO Persona (
    num_cedula, fecha_nacimiento, primer_nombre, segundo_nombre,
    primer_apellido, segundo_apellido, correo, contraseña,
    direccion, telefono_1, telefono_2, fecha_registro,
    id_Rol, puesto, cedula_responsable
)
VALUES (
           p_num_cedula, p_fecha_nacimiento, p_primer_nombre, p_segundo_nombre,
           p_primer_apellido, p_segundo_apellido, p_correo, p_contraseña,
           p_direccion, p_telefono_1, p_telefono_2, v_fechaRegistro,
           p_id_Rol, p_puesto, p_cedula_responsable
       )
    RETURNING id_persona INTO v_idReturn;

RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_idReturn;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_idReturn := NULL;
RETURN QUERY SELECT v_ErrorOccurred, v_ErrorMensaje, v_idReturn;
END;
$$ LANGUAGE plpgsql;


---

-- StoredProcedure [dbo].[TraerInfoUsuario]
CREATE OR REPLACE FUNCTION TraerInfoUsuario(
    p_id_persona INTEGER DEFAULT NULL,
    p_correo VARCHAR DEFAULT NULL
)
RETURNS TABLE (
    id_persona INTEGER,
    num_cedula INTEGER,
    fecha_nacimiento DATE,
    primer_nombre VARCHAR,
    segundo_nombre VARCHAR,
    primer_apellido VARCHAR,
    segundo_apellido VARCHAR,
    correo VARCHAR,
    contraseña VARCHAR,
    direccion VARCHAR,
    telefono_1 INTEGER,
    telefono_2 INTEGER,
    id_Rol INTEGER,
    puesto VARCHAR,
    cedula_responsable INTEGER,
    nombre_rol VARCHAR,
    ErrorOccurred INTEGER,
    ErrorMensaje VARCHAR,
    idReturn INTEGER,
    resultado BOOLEAN
) AS $$
DECLARE
v_idReturn INTEGER := NULL;
    v_ErrorOccurred INTEGER := 0;
    v_ErrorMensaje VARCHAR := '';
    v_resultado BOOLEAN := FALSE;
BEGIN
    IF p_id_persona IS NOT NULL AND EXISTS (SELECT 1 FROM Persona WHERE id_persona = p_id_persona) THEN
        v_idReturn := p_id_persona;
        v_resultado := TRUE;

RETURN QUERY
SELECT
    p.id_persona, p.num_cedula, p.fecha_nacimiento, p.primer_nombre, p.segundo_nombre,
    p.primer_apellido, p.segundo_apellido, p.correo, p.contraseña, p.direccion, p.telefono_1, p.telefono_2,
    p.id_Rol, p.puesto, p.cedula_responsable,
    r.nombre AS nombre_rol,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado
FROM Persona p
         INNER JOIN Roles r ON p.id_Rol = r.id_rol
WHERE p.id_persona = p_id_persona;

ELSIF p_correo IS NOT NULL AND EXISTS (SELECT 1 FROM Persona WHERE correo = p_correo) THEN
SELECT p.id_persona INTO v_idReturn FROM Persona p WHERE p.correo = p_correo;
v_resultado := TRUE;

RETURN QUERY
SELECT
    p.id_persona, p.num_cedula, p.fecha_nacimiento, p.primer_nombre, p.segundo_nombre,
    p.primer_apellido, p.segundo_apellido, p.correo, p.contraseña, p.direccion, p.telefono_1, p.telefono_2,
    p.id_Rol, p.puesto, p.cedula_responsable,
    r.nombre AS nombre_rol,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado
FROM Persona p
         INNER JOIN Roles r ON p.id_Rol = r.id_rol
WHERE p.correo = p_correo;

ELSE
        v_ErrorOccurred := 1;
        v_ErrorMensaje := 'La persona no existe.';
        v_resultado := FALSE;
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::VARCHAR, NULL::VARCHAR,
    NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER, NULL::INTEGER,
    NULL::INTEGER, NULL::VARCHAR, NULL::INTEGER, NULL::VARCHAR,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado;
END IF;

EXCEPTION
    WHEN OTHERS THEN
        v_ErrorOccurred := 1;
        v_ErrorMensaje := SQLERRM;
        v_idReturn := NULL;
        v_resultado := FALSE;
RETURN QUERY
SELECT NULL::INTEGER, NULL::INTEGER, NULL::DATE, NULL::VARCHAR, NULL::VARCHAR,
    NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::VARCHAR, NULL::INTEGER, NULL::INTEGER,
    NULL::INTEGER, NULL::VARCHAR, NULL::INTEGER, NULL::VARCHAR,
    v_ErrorOccurred, v_ErrorMensaje, v_idReturn, v_resultado;
END;
$$ LANGUAGE plpgsql;