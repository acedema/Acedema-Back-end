'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { isAuthenticated } from '@/lib/auth';

export default function AcedemaLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [cargando, setCargando] = useState(true);

  useEffect(() => {
    if (!isAuthenticated()) {
      router.push('/login');
    } else {
      setCargando(false);
    }
  }, []);

  if (cargando) return null; // o un loader

  return <>{children}</>;
}

