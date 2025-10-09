'use client';

import NextLink from 'next/link';
import Image from 'next/image';
import { Grid, Container, Typography, Box, Popover, List, ListItem, ListItemText, Link } from '@mui/material';
import { useState } from 'react';

export default function EstudianteRegistrado() {
  // Estado y funciones para el submenú de Cursos
  const [anchorElCursos, setAnchorElCursos] = useState<HTMLElement | null>(null);

  const handlePopoverOpenCursos = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElCursos(event.currentTarget);
  };

  const handlePopoverCloseCursos = () => {
    setAnchorElCursos(null);
  };

  const openCursos = Boolean(anchorElCursos);

  // Estado y funciones para el submenú de Matrícula
  const [anchorElMatricula, setAnchorElMatricula] = useState<HTMLElement | null>(null);

  const handlePopoverOpenMatricula = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElMatricula(event.currentTarget);
  };

  const handlePopoverCloseMatricula = () => {
    setAnchorElMatricula(null);
  };

  const openMatricula = Boolean(anchorElMatricula);

  return (
    <div style={{ backgroundColor: '#424242', minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
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
            <NextLink href="/" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Inicio
            </NextLink>
            <div
              className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md"
              onMouseEnter={handlePopoverOpenCursos}
              onMouseLeave={handlePopoverCloseCursos}
            >
              Cursos
              <Popover
                open={openCursos}
                anchorEl={anchorElCursos}
                anchorOrigin={{
                  vertical: 'bottom',
                  horizontal: 'left',
                }}
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'left',
                }}
                onClose={handlePopoverCloseCursos}
                disableRestoreFocus
                PaperProps={{
                  style: {
                    border: 'none',
                    borderRadius: '8px',
                  },
                }}
              >
                <List>
                  <ListItem component="div" style={{ borderBottom: 'none', borderRadius: '8px' }}>
                    <Link
                      href="/cursos/guitarra"
                      sx={{
                        textDecoration: 'none',
                        color: 'black',
                        display: 'block',
                        width: '150px',
                        padding: '8px 16px',
                        borderRadius: '8px',
                        border: '1px solid #c38611',
                        backgroundColor: 'white',
                        '&:hover': { backgroundColor: '#c38611', color: 'white' },
                      }}
                    >
                      <ListItemText primary="Cursos" />
                    </Link>
                  </ListItem>
                  <ListItem component="div" style={{ borderBottom: 'none', borderRadius: '8px' }}>
                    <Link
                      href="/cursos/piano"
                      sx={{
                        textDecoration: 'none',
                        color: 'black',
                        display: 'block',
                        width: '150px',
                        padding: '8px 16px',
                        borderRadius: '8px',
                        border: '1px solid #c38611',
                        backgroundColor: 'white',
                        '&:hover': { backgroundColor: '#c38611', color: 'white' },
                      }}
                    >
                      <ListItemText primary="Mis Cursos" />
                    </Link>
                  </ListItem>
                </List>
              </Popover>
            </div>
            <div
              className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md"
              onMouseEnter={handlePopoverOpenMatricula}
              onMouseLeave={handlePopoverCloseMatricula}
            >
              Matrícula
              <Popover
                open={openMatricula}
                anchorEl={anchorElMatricula}
                anchorOrigin={{
                  vertical: 'bottom',
                  horizontal: 'left',
                }}
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'left',
                }}
                onClose={handlePopoverCloseMatricula}
                disableRestoreFocus
                PaperProps={{
                  style: {
                    border: 'none',
                    borderRadius: '8px',
                  },
                }}
              >
                <List>
                  <ListItem component="div" style={{ borderBottom: 'none', borderRadius: '8px' }}>
                    <Link
                      href="/matricula/matricula"
                      sx={{
                        textDecoration: 'none',
                        color: 'black',
                        display: 'block',
                        width: '150px',
                        padding: '8px 16px',
                        borderRadius: '8px',
                        border: '1px solid #c38611',
                        backgroundColor: 'white',
                        '&:hover': { backgroundColor: '#c38611', color: 'white' },
                      }}
                    >
                      <ListItemText primary="Matrícula" />
                    </Link>
                  </ListItem>
                  <ListItem component="div" style={{ borderBottom: 'none', borderRadius: '8px' }}>
                    <Link
                      href="/matricula/pagos"
                      sx={{
                        textDecoration: 'none',
                        color: 'black',
                        display: 'block',
                        width: '150px',
                        padding: '8px 16px',
                        borderRadius: '8px',
                        border: '1px solid #c38611',
                        backgroundColor: 'white',
                        '&:hover': { backgroundColor: '#c38611', color: 'white' },
                      }}
                    >
                      <ListItemText primary="Pagos" />
                    </Link>
                  </ListItem>
                </List>
              </Popover>
            </div>
            <NextLink href="/informacion" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Información
            </NextLink>
            <NextLink href="/Acedema/Contactenos" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Contáctenos
            </NextLink>
            <NextLink href="/Acedema/Foro" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md flexs-center">
              <Image
                src="/notificacion.png"
                alt="Foro"
                width={25}
                height={25}
                style={{ filter: 'brightness(0) invert(1)' }}
              />
            </NextLink>
            <NextLink href="/Acedema/login" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md">
              Iniciar Sesion
            </NextLink>
          </div>
        </div>
      </nav>

      <div style={{ paddingTop: '100px', flex: 1, overflowY: 'auto' }}>
        <Container maxWidth="xl" sx={{ position: 'relative', marginTop: '2rem' }}>
          <Grid container spacing={3}>
            <Grid sx={{ display: 'flex', alignItems: 'center' }}>
              <Box sx={{ position: 'relative', width: '100%', height: '500px' }}>
                <Image
                  src="/banda.jpg"
                  alt="Comunidad ACEDEMA"
                  fill
                  sizes="(max-width: 1280px) 100vw, 1280px"
                  style={{ objectFit: "cover" }}
                  className="object-cover"
                />
              </Box>
            </Grid>

            <Grid sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <Typography variant="h4" sx={{ fontWeight: 'bold', mb: 3, color: 'white', fontSize: '40px' }}>
                ¿Quiénes somos?
              </Typography>
              <Typography variant="body1" sx={{ textAlign: 'left', color: 'white' }}>
                La Asociación Centro de Desarrollo de la Música y el Arte (ACEDEMA), con cédula jurídica 3-002-687784, es una institución educativa sin fines de lucro dedicada a la promoción y enseñanza de la música y las artes. Fundada hace más de una década en el pintoresco San Rafael de Heredia, ACEDEMA ha crecido hasta convertirse en un pilar fundamental para la comunidad local, ofreciendo un espacio donde la pasión por la música puede florecer.
              </Typography>
              <Typography variant="h5" sx={{ fontWeight: 'bold', mt: 2, color: 'white', textAlign: 'left' }}>
                Misión
              </Typography>
              <Typography variant="body1" sx={{ textAlign: 'left', color: 'white' }}>
                Nuestra misión es proporcionar una educación musical de alta calidad a personas de todas las edades y niveles de habilidad, fomentando el amor por la música y el desarrollo personal a través de la expresión artística. Creemos firmemente en el poder transformador de la música y nos esforzamos por hacerla accesible a todos.
              </Typography>
              <Typography variant="h5" sx={{ fontWeight: 'bold', mt: 2, color: 'white', textAlign: 'left' }}>
                Nuestro Equipo de Profesionales
              </Typography>
              <Typography variant="body1" sx={{ textAlign: 'left', color: 'white' }}>
                En ACEDEMA, contamos con un equipo de cinco profesores altamente calificados y apasionados, cada uno experto en su área de especialización. Nuestros profesores no solo son músicos talentosos, sino también educadores dedicados que se comprometen a guiar a sus estudiantes en su viaje musical.
              </Typography>
            </Grid>

            <Grid>
              <Box sx={{ backgroundColor: 'white', padding: '2rem', marginTop: '2rem' }}>
                <Typography variant="h5" sx={{ fontWeight: 'bold', mb: 2, textAlign: 'left', color: '#001636' }}>
                  Atención Personalizada
                </Typography>
                <Typography variant="body1" sx={{ textAlign: 'left', color: '#001636' }}>
                  Entendemos que cada estudiante es único, por lo que ofrecemos clases en grupos pequeños de máximo cinco personas, así como clases individuales. Esta atención personalizada permite a nuestros profesores adaptar sus métodos de enseñanza a las necesidades específicas de cada estudiante, asegurando un progreso significativo y una experiencia de aprendizaje enriquecedora.
                </Typography>

                <Typography variant="h5" sx={{ fontWeight: 'bold', mt: 3, mb: 2, textAlign: 'left', color: '#001636' }}>
                  Nuestra Junta Directiva
                </Typography>
                <Typography variant="body1" sx={{ textAlign: 'left', color: '#001636' }}>
                  ACEDEMA se enorgullece de contar con una junta directiva compuesta por siete miembros dedicados. Esta junta desempeña un papel crucial en la toma de decisiones estratégicas y en la dirección general de la institución, asegurando que ACEDEMA continúe cumpliendo su misión y sirviendo a la comunidad.
                </Typography>

                <Typography variant="h5" sx={{ fontWeight: 'bold', mt: 3, mb: 2, textAlign: 'left', color: '#001636' }}>
                  Nuestra Comunidad
                </Typography>
                <Typography variant="body1" sx={{ textAlign: 'left', color: '#001636' }}>
                  En ACEDEMA, valoramos la creación de una comunidad musical inclusiva y de apoyo. Organizamos regularmente conciertos, talleres y eventos especiales para brindar a nuestros estudiantes la oportunidad de mostrar su talento y conectarse con otros amantes de la música.
                </Typography>
              </Box>
            </Grid>

            <Grid sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
              <Typography variant="h4" sx={{ fontWeight: 'bold', mb: 3, color: 'white', fontSize: '40px' }}>
                Solfeo
              </Typography>

              <Typography variant="body1" sx={{ textAlign: 'left', color: 'white' }}>
                El solfeo, esa disciplina a menudo vista como el cimiento de la educación musical, es mucho más que la simple lectura de notas en un pentagrama. Se trata de un entrenamiento auditivo y rítmico que capacita al individuo para comprender y ejecutar la música escrita con precisión y sensibilidad. Al dominar el solfeo, uno adquiere la habilidad de descifrar partituras, entonar melodías con afinación exacta y mantener un ritmo constante, destrezas esenciales tanto para vocalistas como para instrumentistas. Pero su influencia trasciende el ámbito de la interpretación; el solfeo agudiza el oído, permitiendo distinguir intervalos y acordes, y fomenta una comunicación fluida entre músicos. Incluso para aquellos sin experiencia previa, el solfeo abre un mundo de posibilidades: revela talentos ocultos, profundiza la apreciación musical, estimula el desarrollo cognitivo y brinda la satisfacción de crear música. En esencia, el solfeo es una herramienta transformadora que enriquece la vida de quien se aventura a explorarla.
              </Typography>
            </Grid>

            <Grid sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
              <Box sx={{ position: 'relative', width: '100%', height: '500px' }}>
                <Image
                  src="/principal.jpg"
                  alt="Comunidad ACEDEMA"
                  fill
                  sizes="(max-width: 1280px) 100vw, 1280px"
                  style={{ objectFit: "cover" }}
                  className="object-cover"
                />
              </Box>
            </Grid>
          </Grid>
        </Container>

        <Box sx={{ backgroundColor: '#001636', padding: '2rem', marginTop: '2rem' }}>
          <Typography variant="h6" sx={{ textAlign: 'center', color: 'white' }}>
            Nosotros
          </Typography>
          <div style={{ display: 'flex', justifyContent: 'center', marginTop: '1rem' }}>
            <a href="https://www.facebook.com/share/1BZZGTNCFZ/" target="_blank" rel="noopener noreferrer">
              <Image src="/facebook.png" alt="Facebook" width={30} height={30} style={{ margin: '0 10px' }} />
            </a>
            <a href="https://www.instagram.com/acedemasanrafael?igsh=MTI2dXQ0aXJkczVrNA==" target="_blank" rel="noopener noreferrer">
              <Image src="/instagram.png" alt="Instagram" width={30} height={30} style={{ margin: '0 10px' }} />
            </a>
            <a href="" target="_blank" rel="noopener noreferrer">
              <Image src="/whatsApp.png" alt="Instagram" width={30} height={30} style={{ margin: '0 10px' }} />
            </a>
            <a href="" target="_blank" rel="noopener noreferrer">
              <Image src="/mapas-de-google.png" alt="Instagram" width={30} height={30} style={{ margin: '0 10px' }} />
            </a>
            <a href="" target="_blank" rel="noopener noreferrer">
              <Image src="/gmail.png" alt="Instagram" width={30} height={30} style={{ margin: '0 10px' }} />
            </a>
          </div>
        </Box>
      </div>
    </div>
  );
}
