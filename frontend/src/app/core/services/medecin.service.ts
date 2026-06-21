import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Medecin } from '../models/medecin.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class MedecinService {
  private readonly url = `${environment.apiUrl}/medecins`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Medecin[]> {
    return this.http.get<Medecin[]>(this.url);
  }

  getById(id: number): Observable<Medecin> {
    return this.http.get<Medecin>(`${this.url}/${id}`);
  }

  create(medecin: Omit<Medecin, 'idMedecin'>): Observable<void> {
    return this.http.post<void>(this.url, medecin);
  }

  update(id: number, medecin: Omit<Medecin, 'idMedecin'>): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, medecin);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
