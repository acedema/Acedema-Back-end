'use client';

import Navbar from '@/components/Navbar';
import Header from '@/components/Header';
import styles from './cursos.module.css';

export default function Cursos() {
  return (
    <div>
      <Navbar />
      <div className={styles.container}>
        <Header
          title="Nuestros Cursos"
          subtitle="Explora la variedad de instrumentos que puedes aprender a tocar con nosotros. Encuentra tu pasión musical."
          imageUrl="/bandabg.jpg"
          imageAlt="Nuestros Cursos"
          textBlock="Ofrecemos una amplia gama de cursos para todos los niveles e intereses. Nuestros profesores expertos te guiarán en cada paso de tu aprendizaje, desde los fundamentos teóricos hasta la ejecución avanzada."
        />

        <section className={styles.questionsSection}>
          <h2 className={styles.questionsTitle}>Preguntas para completar la página</h2>
          <ul className={styles.questionsList}>
            <li>¿Cuál es la lista completa de instrumentos que se enseñan?</li>
            <li>¿Hay diferentes niveles para cada curso (ej. principiante, intermedio, avanzado)?</li>
            <li>¿Cuál es la duración de cada curso o módulo?</li>
            <li>¿Se ofrece alguna clase de teoría musical o solfeo de forma independiente?</li>
            <li>¿Podrían proporcionar una breve descripción para cada curso?</li>
            <li>¿Hay requisitos de edad o previos para algún curso en específico?</li>
          </ul>
        </section>
      </div>
    </div>
  );
}
