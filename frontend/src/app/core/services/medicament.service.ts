import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Medicament } from '../models/medicament.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class MedicamentService {
  private readonly url = `${environment.apiUrl}/medicaments`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Medicament[]> {
    return this.http.get<Medicament[]>(this.url);
  }

  getById(id: number): Observable<Medicament> {
    return this.http.get<Medicament>(`${this.url}/${id}`);
  }

  create(med: Omit<Medicament, 'idMedicament'>): Observable<void> {
    return this.http.post<void>(this.url, med);
  }

  update(id: number, med: Omit<Medicament, 'idMedicament'>): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, med);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
