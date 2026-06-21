export interface LoginResponse {
  token: string;
  role: string;
  idRef: number;
  nom: string;
  prenom: string;
  email: string;
}

export interface TokenPayload {
  sub: string;
  email: string;
  role: string;
  idRef: string;
  nom: string;
  prenom: string;
  exp: number;
}
