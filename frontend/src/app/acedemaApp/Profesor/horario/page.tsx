import Link from 'next/link';
import Image from 'next/image';
import { Container, Box, Typography } from '@mui/material';

export default function horarioProfesor() {
  return (
    <div>
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
          <div className="flex space-x-1 mt-140">
            <Link href="/" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Inicio
            </Link>
            <Link href="/cursos" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Cursos
            </Link>
            <Link href="/matricula" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Matrícula
            </Link>
            <Link href="/informacion" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Información
            </Link>
            <Link href="/Acedema/Contactenos" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Contáctenos
            </Link>
            <Link href="/Acedema/Foro" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md flexs-center">
              <Image
                src="/notificacion.png"
                alt="Foro"
                width={25}
                height={25}
                style={{ filter: 'brightness(0) invert(1)' }}
              />
            </Link>
            <Link href="/Acedema/login" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Iniciar Sesion
            </Link>
          </div>
        </div>
      </nav>
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        minHeight="calc(100vh - 100px)" // Ajusta la altura para que no se superponga con la barra de navegación
        padding="20px"
      >
        <Container
          maxWidth="lg"
          style={{
            backgroundColor: '#eeeeee',
            marginTop: '150px',
            padding: '20px', // Reducido a 20px
            borderRadius: '8px',
            backgroundSize: 'cover',
            backgroundRepeat: 'no-repeat',
            width: '100%',
            maxWidth: '1200px',
            border: '1px solid #001636',
          }}
        >
                      <Link href="/"> {/* Enlace a otra página */}
                    <Image
                      src="/devolver.png"
                      alt="Return Icon"
                      width={30}
                      height={30}
                      style={{ marginRight: '1rem', cursor: 'pointer' }} // Agrega cursor: pointer
                    />
            </Link>
          <Typography variant="h4" gutterBottom align="center" fontWeight="bold" color="#001636">Horarios</Typography>
          <Typography variant="h6" gutterBottom align="left" color="#001636">Nivel:</Typography>
          <Typography variant="h6" gutterBottom align="left" color="#001636">Dia:</Typography>
          <Typography variant="h6" gutterBottom align="left" color="#001636">Hora de Inicio:</Typography>
          <Typography variant="h6" gutterBottom align="left" color="#001636">Hora de finalización:</Typography>
          <Typography variant="h6" gutterBottom align="left" color="#001636">Modalidad:</Typography>
          
        </Container>
      </Box>
    </div>
  );
}