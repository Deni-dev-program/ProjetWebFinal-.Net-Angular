import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Consultation } from '../models/consultation.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ConsultationService {
  private readonly url = `${environment.apiUrl}/consultations`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Consultation[]> {
    return this.http.get<Consultation[]>(this.url);
  }

  getByDossier(dossierId: number): Observable<Consultation[]> {
    return this.http.get<Consultation[]>(`${this.url}/dossier/${dossierId}`);
  }

  getById(id: number): Observable<Consultation> {
    return this.http.get<Consultation>(`${this.url}/${id}`);
  }

  create(c: Omit<Consultation, 'idConsultation'>): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(this.url, c);
  }

  update(id: number, c: Omit<Consultation, 'idConsultation'>): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, c);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
