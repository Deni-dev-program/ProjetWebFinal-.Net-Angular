import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RendezVous } from '../models/rendezvous.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class RendezVousService {
  private readonly url = `${environment.apiUrl}/rendezvous`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<RendezVous[]> {
    return this.http.get<RendezVous[]>(this.url);
  }

  getByPatient(patientId: number): Observable<RendezVous[]> {
    return this.http.get<RendezVous[]>(`${this.url}/patient/${patientId}`);
  }

  getByMedecin(medecinId: number): Observable<RendezVous[]> {
    return this.http.get<RendezVous[]>(`${this.url}/medecin/${medecinId}`);
  }

  create(rdv: Omit<RendezVous, 'idRDV' | 'statut'>): Observable<void> {
    return this.http.post<void>(this.url, rdv);
  }

  update(id: number, rdv: Omit<RendezVous, 'idRDV' | 'statut'>): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, rdv);
  }

  updateStatut(id: number, statut: string): Observable<void> {
    return this.http.patch<void>(`${this.url}/${id}/statut`, { statut });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
