'use client'; // Indica que este componente se renderizará en el cliente (navegador)

import Link from 'next/link'; // Importa el componente Link de Next.js para la navegación
import Image from 'next/image'; // Importa el componente Image de Next.js para la optimización de imágenes
import { Grid, Container, Typography, Box, Button } from '@mui/material'; // Importa componentes de Material UI para la interfaz de usuario
import React, { useState } from 'react'; // Importa React y el hook useState para manejar el estado del componente

export default function MediosPago() {
  const [selectedFile, setSelectedFile] = useState<File | null>(null); // Declara el estado para el archivo seleccionado, inicializado como null
  const [comments, setComments] = useState<string>(''); // Declara el estado para los comentarios, inicializado como una cadena vacía

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => { // Función para manejar el cambio en el input de archivo
    setSelectedFile(event.target.files?.[0] || null); // Actualiza el estado con el archivo seleccionado o null si no hay archivo
  };

  const handleCommentsChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => { // Función para manejar el cambio en el textarea de comentarios
    setComments(event.target.value); // Actualiza el estado con el valor del textarea
  };

  const handleSubmit = () => { // Función para manejar el envío del formulario
    // Aquí puedes agregar la lógica para enviar el archivo y los comentarios al servidor
    console.log('Archivo seleccionado:', selectedFile); // Muestra el archivo seleccionado en la consola
    console.log('Comentarios:', comments); // Muestra los comentarios en la consola
  };

  const handleRemoveFile = () => { // Función para eliminar el archivo seleccionado
    setSelectedFile(null); // Establece el estado del archivo seleccionado como null
  };


  return (
    <div style={{ backgroundColor: '#424242', minHeight: '100vh', display: 'flex', flexDirection: 'column' }}> {/* Contenedor principal con fondo gris, altura mínima de la pantalla y disposición en columna */}

      <nav style={{ position: 'fixed', top: 0, width: '100%', zIndex: 100 }} className="bg-[#001636] p-1 border-b-4 border-[#424242]"> {/* Barra de navegación fija en la parte superior */}
        <div className="max-w-7xl mx-auto"> {/* Contenedor con ancho máximo y centrado horizontalmente */}
          <div className="flexs-center space-x-2"> {/* Contenedor con elementos alineados horizontalmente y espacio entre ellos */}
            <Image
              src="/logo.jpg" // Ruta de la imagen del logo
              alt="Logo ACEDEMA" // Texto alternativo para la imagen
              width={80} // Ancho de la imagen
              height={80} // Alto de la imagen
              className="object-contain rounded-full" // Clases CSS para la imagen
              priority // Indica que la imagen es de alta prioridad
            />
            <h1 className="text-white text-3xl font-bold">ACEDEMA</h1> {/* Título de la página */}
          </div>
          <div className="flex space-x-1 mt-140"> {/* Contenedor con enlaces de navegación */}
            <Link href="/" className="text-white hover:bg-[#c38611] px-4 py-2 rounded-md"> {/* Enlace a la página de inicio */}
              Inicio
            </Link>
          </div>
        </div>
      </nav>

      <div style={{ paddingTop: '100px', flex: 1, overflowY: 'auto' }}> {/* Contenedor para el contenido principal con relleno superior y scroll vertical */}

        <Container maxWidth="xl" sx={{ position: 'relative', marginTop: '2rem' }}> {/* Contenedor con ancho máximo y margen superior */}
          <Grid container spacing={3}> {/* Contenedor de cuadrícula con espaciado entre elementos */}
            <Grid> {/* Elemento de cuadrícula que ocupa todo el ancho en pantallas pequeñas */}
              <Box sx={{ backgroundColor: 'white', padding: '2rem', borderRadius: '8px' }}> {/* Contenedor con fondo blanco, relleno y bordes redondeados */}
                <Typography variant="h5" sx={{ fontWeight: 'bold', mb: 2, textAlign: 'left', color: '#001636' }}> {/* Título de la sección */}
                  Medios de Pago
                </Typography>
                <Typography variant="body1" sx={{ textAlign: 'left', color: '#001636' }}> {/* Descripción de la sección */}
                  Acá va a haber información referente a los medios de pago que el usuario puede realizar:
                </Typography>
                <ol> {/* Lista ordenada de medios de pago */}
                  <li>Transferencia bancaria al siguiente número de cuenta:</li>
                  <li>Sinpe móvil al siguiente número:</li>
                </ol>

                <div className="file-upload" style={{ border: '2px dashed #ccc', padding: '20px', textAlign: 'center', marginTop: '20px', cursor: 'pointer' }}> {/* Contenedor para la carga de archivos */}
                  <input type="file" id="file-input" accept=".png, .pdf" style={{ display: 'none' }} onChange={handleFileChange} /> {/* Input de tipo archivo oculto */}
                  <label htmlFor="file-input"> {/* Etiqueta para el input de archivo */}
                    <Typography variant="body1" sx={{ color: '#555' }}> {/* Texto descriptivo para la carga de archivos */}
                      Cargar archivos únicamente formatos png y pdf.
                    </Typography>
                    <Typography variant="body1" sx={{ color: '#555' }}> {/* Texto descriptivo para la carga de archivos */}
                      Soltar archivos o hacer clic para seleccionar
                    </Typography>
                  </label>
                  {selectedFile && ( // Muestra el nombre del archivo seleccionado y el botón de eliminar si hay un archivo seleccionado
                    <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', marginTop: '10px' }}>
                      <Typography variant="body2" sx={{ marginRight: '10px' }}>
                        Archivo seleccionado: {selectedFile.name}
                      </Typography>
                      <Button variant="outlined" color="error" size="small" onClick={handleRemoveFile}>
                        Eliminar
                      </Button>
                    </div>
                  )}
                </div>

                <div className="comments" style={{ marginTop: '20px' }}> {/* Contenedor para los comentarios */}
                  <textarea
                    placeholder="Comentarios" // Texto de marcador de posición para el textarea
                    value={comments} // Valor del textarea
                    onChange={handleCommentsChange} // Función para manejar el cambio en el textarea
                    style={{ width: '100%', padding: '10px', border: '1px solid #ccc', borderRadius: '4px', resize: 'vertical' }} // Estilos CSS para el textarea
                  ></textarea>
                </div>

                <div className="buttons" style={{ display: 'flex', justifyContent: 'center', marginTop: '20px' }}> {/* Contenedor para los botones de enviar y cancelar */}
                  <button className="submit" onClick={handleSubmit} style={{ padding: '10px 20px', marginLeft: '10px', border: 'none', borderRadius: '4px', cursor: 'pointer', backgroundColor: '#c38611', color: '#fff' }}> {/* Botón de enviar */}
                    Enviar
                  </button>
                  <button className="cancel" style={{ padding: '10px 20px', marginLeft: '10px', border: 'none', borderRadius: '4px', cursor: 'pointer', backgroundColor: '#c38611', color: '#fff' }}> {/* Botón de cancelar */}
                    Cancelar
                  </button>
                </div>
              </Box>
            </Grid>
          </Grid>
        </Container>
      </div>
    </div>
  );
}