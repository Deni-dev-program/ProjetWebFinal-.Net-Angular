import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DossierMedical } from '../models/dossier-medical.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class DossierMedicalService {
  private readonly url = `${environment.apiUrl}/dossiers`;

  constructor(private http: HttpClient) {}

  getByPatient(patientId: number): Observable<DossierMedical> {
    return this.http.get<DossierMedical>(`${this.url}/patient/${patientId}`);
  }

  create(dossier: Omit<DossierMedical, 'idDossier'>): Observable<void> {
    return this.http.post<void>(this.url, dossier);
  }

  update(id: number, dossier: DossierMedical): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, dossier);
  }
}
