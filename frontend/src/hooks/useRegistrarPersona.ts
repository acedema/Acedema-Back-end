import { ReqRegistrarPersona, ResRegistrarPersona } from "@/lib/types";
import { sendData } from "@/lib/api";

export async function registrarPersona(data: ReqRegistrarPersona["persona"]) {
  const payload: ReqRegistrarPersona = { persona: data };
  return await sendData<ResRegistrarPersona>(
    "Persona/registrarPersona",
    "POST",
    payload
  );
}

