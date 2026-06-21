import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Facture } from '../models/facture.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FactureService {
  private readonly url = `${environment.apiUrl}/factures`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Facture[]> {
    return this.http.get<Facture[]>(this.url);
  }

  getByConsultation(consultationId: number): Observable<Facture> {
    return this.http.get<Facture>(`${this.url}/consultation/${consultationId}`);
  }

  create(facture: Omit<Facture, 'idFacture' | 'dateFacture'>): Observable<void> {
    return this.http.post<void>(this.url, facture);
  }

  updateStatut(id: number, statut: string): Observable<void> {
    return this.http.patch<void>(`${this.url}/${id}/statut`, { statut });
  }
}
