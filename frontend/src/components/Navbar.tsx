'use client';

import styles from './Navbar.module.css';
import { useEffect, useState } from 'react';
import Link from 'next/link';
import Image from 'next/image';

export default function Navbar() {
  const [scrolled, setScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setScrolled(window.scrollY > 8);
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  return (
    <nav className={`${styles.navbar} ${scrolled ? styles.scrollednav : ''}`}>
      <div className={styles.container}>
        <div className={styles.logo}>
          <Link href='/' className={styles.logoContainer}>
            <Image src="/logo.jpg" alt="Logo ACEDEMA" width={200} height={200} priority />
            <h1>ACEDEMA</h1>
          </Link>
        </div>
        <ul className={styles.links}>
          {[
            { href: '/', label: 'Inicio' },
            { href: '/nosotros', label: 'Nosotros' },
            { href: '/matricula', label: 'Matrícula' },
            { href: '/cursos', label: 'Cursos' },
            { href: '/contactenos', label: 'Contáctenos' },
            { href: '/login', label: 'Iniciar Sesión' },
          ].map((link) => (
            <li key={link.href} className={styles.linkItem}>
              <Link key={link.href} href={link.href} className={styles.link}>
                {link.label}
              </Link>
            </li>
          ))}
        </ul>
      </div>
    </nav>
  );
}

