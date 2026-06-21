import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginResponse, TokenPayload } from '../models/utilisateur.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly url = `${environment.apiUrl}/auth`;
  private readonly TOKEN_KEY = 'clinique_token';

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.url}/login`, { email, password }).pipe(
      tap(res => localStorage.setItem(this.TOKEN_KEY, res.token))
    );
  }

  register(data: any): Observable<void> {
    return this.http.post<void>(`${this.url}/register`, data);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    const payload = this.decodeToken();
    if (!payload) return false;
    return payload.exp * 1000 > Date.now();
  }

  getRole(): string {
    return this.decodeToken()?.role ?? '';
  }

  getIdRef(): number {
    return parseInt(this.decodeToken()?.idRef ?? '0', 10);
  }

  getNom(): string {
    return this.decodeToken()?.nom ?? '';
  }

  getPrenom(): string {
    return this.decodeToken()?.prenom ?? '';
  }

  getEmail(): string {
    return this.decodeToken()?.email ?? '';
  }

  private decodeToken(): TokenPayload | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload)) as TokenPayload;
    } catch {
      return null;
    }
  }
}
