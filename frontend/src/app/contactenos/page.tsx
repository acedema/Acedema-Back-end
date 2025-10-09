'use client';

import Navbar from '@/components/Navbar';
import Header from '@/components/Header';
import styles from './contactenos.module.css';


export default function Contactenos() {
  return (
    <div>
      <Navbar />
      <div className={styles.container}>
        <Header
          title="Contáctenos"
          subtitle="Estamos aquí para ayudarte. No dudes in comunicarte con nosotros para cualquier consulta."
          imageUrl="/bandabg.jpg"
          imageAlt="Contáctenos"
          textBlock="Ya sea que tengas preguntas sobre nuestros cursos, horarios o proceso de matrícula, nuestro equipo está listo para darte toda la información que necesites. ¡Esperamos saber de ti!"
        />

        <section className={styles.questionsSection}>
          <h2 className={styles.questionsTitle}>Preguntas para completar la página</h2>
          <ul className={styles.questionsList}>
            <li>¿Cuál es la dirección física exacta de la academia?</li>
            <li>¿Cuál es el horario de atención al público?</li>
            <li>¿Hay un número de WhatsApp para consultas rápidas?</li>
            <li>¿Quieren incluir un mapa de Google Maps con la ubicación?</li>
            <li>¿Prefieren un formulario de contacto en la página o solo mostrar la información?</li>
          </ul>
        </section>
      </div>
    </div>
  );
}
