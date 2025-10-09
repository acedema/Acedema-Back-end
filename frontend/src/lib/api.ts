const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5069/api";

// GET
export async function fetchData<T>(endpoint: string): Promise<T> {
  const res = await fetch(`${API_BASE_URL}/${endpoint}`, {
    headers: { "Content-Type": "application/json" },
    cache: "no-store",
  });
  if (!res.ok) throw new Error(`Error fetching ${endpoint}: ${res.statusText}`);
  return res.json();
}

// POST/PUT/DELETE
export async function sendData<T>(
  endpoint: string,
  method: "POST" | "PUT" | "DELETE",
  body?: unknown
): Promise<T> {
  const res = await fetch(`${API_BASE_URL}/${endpoint}`, {
    method,
    headers: { "Content-Type": "application/json" },
    body: body ? JSON.stringify(body) : undefined,
  });
  if (!res.ok) throw new Error(`Error ${method} ${endpoint}: ${res.statusText}`);
  return res.json();
}

