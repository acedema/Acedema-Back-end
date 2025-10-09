import { Container, Grid, Avatar, Typography, Paper, TextField } from '@mui/material';

export default function PerfilPage() {
  return (
    <Container
      maxWidth="md"
      sx={{
        backgroundColor: 'white',
        padding: '2rem',
        borderRadius: '8px',
        boxShadow: 3,
        marginTop: '2rem'
      }}
    >
      <Grid container spacing={3} alignItems="center">
        
        {/*/Foto perfil y nombre */}
        <Grid sx={{ textAlign: 'center' }}>
          <Avatar 
            src="/profile.jpg"  // Imagen de perfil (cambiar por la real)
            sx={{ width: 120, height: 120, margin: 'auto' }} 
          />
          <Typography variant="h5" sx={{ marginTop: '1rem', fontWeight: 'bold' }}>
            Juan Pérez
          </Typography>
        </Grid>

        {/* Información Personal */}
        <Grid>
          <Paper elevation={3} sx={{ padding: '1.5rem', borderRadius: '8px' }}>
            <Typography variant="h6" sx={{ fontWeight: 'bold', marginBottom: '1rem' }}>
              Información Personal
            </Typography>

            <TextField label="Fecha de Nacimiento" variant="outlined" fullWidth value="10/05/1990" sx={{ marginBottom: 2 }} disabled />
            <TextField label="Género" variant="outlined" fullWidth value="Masculino" sx={{ marginBottom: 2 }} disabled />
            <TextField label="Correo" variant="outlined" fullWidth value="juanperez@email.com" sx={{ marginBottom: 2 }} disabled />
            <TextField label="Teléfono" variant="outlined" fullWidth value="+506 8888 8888" sx={{ marginBottom: 2 }} disabled />
            <TextField label="Dirección" variant="outlined" fullWidth value="San José, Costa Rica" sx={{ marginBottom: 2 }} disabled />
          </Paper>
        </Grid>

      </Grid>
    </Container>
  );
}
