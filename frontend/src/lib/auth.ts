export type User = {
  nombre: string;
  rol: 'estudiante' | 'admin' | 'profesor';
};

const SESSION_KEY = 'currentUser';

export function saveSession(user: User) {
  localStorage.setItem(SESSION_KEY, JSON.stringify(user));
}

export function getCurrentUser(): User | null {
  if (typeof window === 'undefined') return null;
  const data = localStorage.getItem(SESSION_KEY);
  return data ? JSON.parse(data) : null;
}

export function removeSession() {
  localStorage.removeItem(SESSION_KEY);
}

export function isAuthenticated(): boolean {
  return getCurrentUser() !== null;
}

