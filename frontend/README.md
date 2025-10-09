**📘 Documento: Integración de Información - Proyecto ACEDEMA**

---

### 🔹 Objetivo General

Centralizar y estructurar toda la información funcional y visual del sitio web ACEDEMA según la arquitectura actual del proyecto y el flujo de usuario previsto.

---

## 📂 Estructura del Proyecto

```
acedemaApp/
├── dashboard/                # Panel administrativo solo para el admin
├── layout.tsx               # Layout base para la sección privada
├── page.tsx                 # Vista principal tras iniciar sesión (Home privado)
├── menu/                    # Posible componente de navegación interna privada
contactenos/
└── page.tsx                 # Página de contacto para el público general
cursos/
└── page.tsx                 # Página informativa de cursos (pública)
favicon.ico
globals.css
layout.tsx                  # Layout general (landing)
login/
├── login.module.css         # Estilos para el login
└── page.tsx                 # Página de login (acceso para admin, estudiante y profesor)
matricula/
└── page.tsx                 # Instrucciones de matrícula (pública)
nosotros/
└── page.tsx                 # Información sobre ACEDEMA
page.module.css
page.tsx                    # Landing page principal con navbar
```

---

## 🚀 Flujo del Usuario

### 1. Página de Inicio (Landing Page)

- Navbar con enlaces a:

  - **Inicio**
  - **Cursos**: `/cursos/page.tsx` ✔
  - **Matrícula**: `/matricula/page.tsx` ✔
  - **Información**: `/nosotros/page.tsx` ✔
  - **Contáctenos**: `/contactenos/page.tsx` ✔
  - **Login**: `/login/page.tsx`

---

### 2. Login

- Ruta: `/login/page.tsx`
- Autenticación para:

  - **Administrador**
  - **Profesor**
  - **Estudiante**

> ✉ Los usuarios tienen cuentas preexistentes. No se registran desde la web.

---

### 3. Sección Privada (Post-login)

#### Para Profesores y Estudiantes

Ruta: `/acedemaApp/`

- `page.tsx`: Home privado
- `menu/`: Posiblemente contiene menús para navegar entre vistas privadas

##### Contenidos

- **Mis Cursos** (`/acedemaApp/miscursos`):

  - Guitarra, Piano, Violín (por niveles)

- **Horarios** (`/acedemaApp/horarioProfesor`):

  - Nivel, Día, Hora de Inicio, Finalización, Modalidad

- **Perfil**:

  - Avatar, Nombre, Información personal (Correo, Dirección, Teléfono, etc.)

- **Foro** (`/acedemaApp/Foro`):

  - Sección social/comunitaria

#### Para Administrador

Ruta: `/acedemaApp/dashboard`

- Acceso exclusivo del admin para:

  - Ver usuarios
  - Editar cursos
  - Agregar profesores/estudiantes
  - Revisar matrículas, horarios, etc.

---

## 📚 Contenido Recolectado Integrado

### ✍️ Matrícula (contenido de `/matricula/page.tsx`)

- Proceso en 5 pasos claros
- Contacto vía WhatsApp
- Cursos disponibles:

  - Viento: Oboe, Fagot, Acordeón, Flauta, Clarinete, Trompeta...
  - Percusión: Xilófono, Marimba, Caja...
  - Cuerdas: Violín, Viola, Piano vertical...

### 🎓 Perfil

- Imagen y datos:

  - Nombre, Fecha Nacimiento, Género
  - Correo, Teléfono, Dirección

### ⏰ Horarios

- Información de clases:

  - Nivel, Día, Hora inicio y fin, Modalidad

### 📄 Mis Cursos

- Lista de cursos con nombre y descripción del contenido
- Estructura tipo acordeón

---

## ❓ Faltante o Por Confirmar

### Sobre Usuarios

- ✉ ¿Cómo se crean las cuentas de estudiante/profesor? ¿Manual desde el admin?
- ✉ ¿Hay posibilidad de que un profesor vea solo sus cursos y horarios?
- ✉ ¿El foro es global o filtrado por curso?

### Sobre Cursos

- ✉ ¿Hay un sistema para inscribirse desde la vista privada?
- ✉ ¿Cuál es la relación entre el estudiante y el curso? (niveles, profesor, horario...)

### Sobre Administrador

- ✉ ¿El dashboard tiene control CRUD completo sobre todos los elementos?
- ✉ ¿El admin también puede generar reportes? (ej. lista de estudiantes por curso)

### Sobre Seguridad

- ✉ ¿Autenticación basada en JWT, sessions, cookies?
- ✉ ¿Hay roles protegidos con middleware?

---

## 📄 Siguiente Paso

1. Revisar con ACEDEMA los puntos ❓ marcados como "Por Confirmar".
2. Completar información faltante sobre flujos y privilegios.

