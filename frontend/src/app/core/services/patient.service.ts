import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Patient } from '../models/patient.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly url = `${environment.apiUrl}/patients`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.url);
  }

  getById(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.url}/${id}`);
  }

  create(patient: Omit<Patient, 'idPatient'>): Observable<void> {
    return this.http.post<void>(this.url, patient);
  }

  update(id: number, patient: Omit<Patient, 'idPatient'>): Observable<void> {
    return this.http.put<void>(`${this.url}/${id}`, patient);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
