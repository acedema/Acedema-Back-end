'use client';

import Link from 'next/link';
import Image from 'next/image';
import { Container, Typography, Grid, Box, IconButton, Button } from '@mui/material';

export default function Contactenos() {
  return (
    <div style={{ backgroundColor: '#424242', minHeight: '100vh', paddingTop: '100px' }}>
      {/* Barra de navegación */}
      <nav style={{ position: 'fixed', top: 0, width: '100%', zIndex: 100 }} className="bg-[#001636] p-1 border-b-4 border-[#424242]">
        <div className="max-w-7xl mx-auto">
          <div className="flexs-center space-x-2">
            <Image
              src="/logo.jpg"
              alt="Logo ACEDEMA"
              width={80}
              height={80}
              className="object-contain rounded-full"
              priority
            />
            <h1 className="text-white text-3xl font-bold">ACEDEMA</h1>
          </div>
          <div className="flex space-x-4 mt-2">
            <Link href="/" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Inicio
            </Link>
            <Link href="/cursos" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Cursos
            </Link>
            <Link href="/matricula" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Matrícula
            </Link>
            <Link href="/informacion" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Información
            </Link>
            <Link href="/solfeo" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Solfeo
            </Link>
            <Link href="/Acedema/Contactenos" className="text-white hover:bg-[#c38611]  px-4 py-2 rounded-md">
              Contáctenos
            </Link>
            <Link href="/Acedema/Foro" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md flexs-center">
            <Image
              src="/notificacion.png"
              alt="Foro"
              width={25} 
              height={25} 
              style={{ filter: 'brightness(0) invert(1)' }} // Cambia el color a blanco
            />
          </Link>

          </div>
        </div>
      </nav>
      <Container
        maxWidth="lg"
        style={{
          marginTop: '20px',
          backgroundColor: 'white',
          padding: '40px',
          borderRadius: '8px',
          backgroundImage: 'url("/")', // Reemplaza con la ruta de tu imagen
          backgroundSize: 'cover',
          backgroundRepeat: 'no-repeat',
        }}
      >
        <Typography variant="h4" gutterBottom align="center" color='#001636'>Contáctenos</Typography>
        <Grid container spacing={4} justifyContent="center">
          {/* Contenedor de Teléfono */}
          <Grid>
            <Box style={{ border: '1px solid #e0e0e0', borderRadius: '8px', padding: '20px', textAlign: 'center' }}>
              <Image src="/whatsApp.png" alt="WhatsApp" width={54} height={54} />
              <Typography variant="h6" className="mt-2">Teléfono</Typography>
              <Typography variant="body1">+506 8669 6541</Typography>
            </Box>
          </Grid>
          {/* Contenedor de Correo Electrónico */}
          <Grid>
            <Box style={{ border: '1px solid #e0e0e0', borderRadius: '8px', padding: '20px', textAlign: 'center' }}>
              <Image src="/gmail.png" alt="Correo Electrónico" width={54} height={54} />
              <Typography variant="h6" className="mt-2">Correo Electrónico</Typography>
              <Typography variant="body1">
                <a href="mailto:acedema.sr@gmail.com" style={{ color: 'blue', textDecoration: 'underline' }}>
                  acedema.sr@gmail.com
                </a>
              </Typography>
            </Box>
          </Grid>
          {/* Contenedor de Ubicación */}
          <Grid>
            <a href="https://www.google.com/maps/dir//Academia+M%C3%BAsica+Activa,+Heredia,+Heredia,+Costa+Rica/data=!4m9!4m8!1m0!1m5!1m1!19sChIJRWIx_Fz7oI8RIvGHaDAeQkA!2m2!1d-84.117143!2d9.9970483999999988!3e0" target="_blank" rel="noopener noreferrer" style={{ textDecoration: 'none', color: 'inherit' }}>
              <Box style={{ border: '1px solid #e0e0e0', borderRadius: '8px', padding: '20px', textAlign: 'center', cursor: 'pointer' }}>
                <Image src="/mapas-de-google.png" alt="Ubicación" width={54} height={54} />
                <Typography variant="h6" className="mt-2">Ubicación</Typography>
                <Typography variant="body1">Heredia, San Rafael</Typography>
              </Box>
            </a>
          </Grid>
          {/* Redes Sociales */}
          <Grid sx={{ marginTop: '-20px' }}>
            <Box style={{ border: '1px solid #e0e0e0', borderRadius: '8px', padding: '20px', textAlign: 'center' }}>
              <Typography variant="h6" gutterBottom>Redes Sociales</Typography>
              <IconButton aria-label="facebook" href="https://www.facebook.com/share/1BZZGTNCFZ/" target="_blank" rel="noopener noreferrer" className="mr-4">
                <Image src="/facebook.png" alt="Facebook" width={40} height={40} />
              </IconButton>
              <IconButton aria-label="instagram" href="https://www.instagram.com/acedemasanrafael?igsh=MTI2dXQ0aXJkczVrNA==" target="_blank" rel="noopener noreferrer">
                <Image src="/instagram.png" alt="Instagram" width={40} height={40} />
              </IconButton>
            </Box>
          </Grid>
        </Grid>
        <Grid container justifyContent="center" sx={{ marginTop: '20px' }}> 
  <Grid container spacing={4} justifyContent="center">
    <Grid>
      <Button
        variant="contained"
        sx={{
          backgroundColor: '#c38611',
          color: 'white',
          textTransform: 'none',
          padding: '0.5rem 2rem',
          fontSize: '1rem',
        }}
      >
        Editar
      </Button>
    </Grid>
    <Grid>
      <Button
        variant="contained"
        sx={{
          backgroundColor: '#c38611',
          color: 'white',
          textTransform: 'none',
          padding: '0.5rem 2rem',
          fontSize: '1rem',
        }}
      >
        Guardar
      </Button>
    </Grid>
  </Grid>
</Grid>
      </Container>
    </div>
  );
}