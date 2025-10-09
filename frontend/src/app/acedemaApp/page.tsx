'use client';

import { useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { getCurrentUser } from '@/lib/auth';

export default function AcedemaInicio() {
  const router = useRouter();
  const sesion = getCurrentUser(); //

  useEffect(() => {
    if (!sesion) {
      router.push('/login');
    }
  }, [sesion]);

  return (
    <div>
      <h1>Bienvenido a Acedema</h1>
      <p>Hola {sesion?.nombre}, estás logueado como <strong>{sesion?.rol}</strong>.</p>
    </div>
  );
}

