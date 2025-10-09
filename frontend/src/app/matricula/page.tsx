'use client';

import styles from './matricula.module.css';
import Navbar from '@/components/Navbar';
import Header from '@/components/Header';
import Link from 'next/link';

export default function Matricula() {
  return (
    <div>
      <Navbar />
      <div className={styles.container}>
        <Header
          title="Matrícula Abierta"
          subtitle="Tu viaje musical comienza aquí. Descubre cómo unirte a nuestra comunidad y transformar tu pasión en arte."
          imageUrl="/principal.jpg"
          imageAlt="Proceso de matrícula"
          textBlock="En ACEDEMA, creemos que la música es para todos. Ofrecemos un proceso de matrícula continuo y flexible, diseñado para que puedas empezar a aprender en cualquier momento del año, sin importar tu nivel de experiencia."
          achievements={[
            { title: 'Modalidades', description: 'Clases virtuales y presenciales' },
            { title: 'Inscripción', description: 'Abierta todo el año' },
            { title: 'Niveles', description: 'Para principiantes y avanzados' },
          ]}
        />

        <section className={styles.contentSection}>
          <div className={styles.textBlock}>
            <h2>¿Cómo es el proceso?</h2>
            <ul>
              <li><strong>Paso 1: Contacto Inicial:</strong> Completa nuestro formulario de matrícula para que
                podamos conocerte.
              </li>
              <li><strong>Paso 2: Entrevista y Nivelación:</strong> Si ya tienes conocimientos, coordinaremos una
                breve entrevista virtual para asignarte el nivel perfecto para ti.
              </li>
              <li><strong>Paso 3: ¡A Tocar!:</strong> Una vez definido tu curso y horario, estarás listo para
                empezar tus clases y sumergirte en el mundo de la música.
              </li>
            </ul>
          </div>

          <div className={`${styles.textBlock} ${styles.infoBlock}`}>
            <h2>Información Clave</h2>
            <p><strong>¿Quiénes pueden matricularse?</strong> ¡Todos! Nuestra academia recibe a personas de todas las edades y niveles.</p>
            <p><strong>Fechas de Ingreso:</strong> La matrícula está abierta todo el año. Aunque tenemos ciclos semestrales (febrero-junio y julio-noviembre), puedes incorporarte en cualquier momento.</p>
            <p><strong>Modalidades:</strong> Ofrecemos clases tanto presenciales como virtuales.</p>
            <p><strong>Requisitos:</strong> Ninguno. Solo necesitas ganas de aprender.</p>
            <p><strong>Formas de Pago:</strong> Aceptamos transferencia bancaria, SINPE móvil y depósito.</p>
          </div>
        </section>

        <section id="cta" className={styles.ctaSection}>
          <div className={styles.ctaOverlay}></div>

          <div className={styles.formButtonSection}>
            <h2 className={styles.formPrompt}>¿Listo para empezar?</h2>
            <p className={styles.formSubtitle}>
              El primer paso hacia tu futuro musical está a un solo clic. Completa el formulario y nos pondremos en contacto para guiarte.
            </p>
            <Link href="/matricula/formulario" className={styles.formButton}>
              Iniciar Matrícula
            </Link>
          </div>
        </section>
      </div>
    </div>
  );
}
