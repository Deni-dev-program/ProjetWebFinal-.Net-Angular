import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { DossierMedicalService } from '../../core/services/dossier-medical.service';
import { DossierMedical } from '../../core/models/dossier-medical.model';

@Component({
  selector: 'app-patients',
  standalone: false,
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.css']
})
export class PatientsComponent implements OnInit {
  patients: Patient[] = [];
  showForm = false;
  showDossier = false;
  editingId: number | null = null;
  selectedPatient: Patient | null = null;
  dossier: DossierMedical | null = null;
  dossierError = '';
  patientForm: FormGroup;

  constructor(
    private patientSvc: PatientService,
    private dossierSvc: DossierMedicalService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.patientForm = this.fb.group({
      nom: ['', Validators.required],
      prenom: ['', Validators.required],
      dateNaissance: ['', Validators.required],
      sexe: ['M', Validators.required],
      telephone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.patientSvc.getAll().subscribe(p => { this.patients = p; this.cdr.detectChanges(); });
  }

  openCreate(): void {
    this.editingId = null;
    this.patientForm.reset({ sexe: 'M' });
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(p: Patient): void {
    this.editingId = p.idPatient;
    this.patientForm.patchValue({
      nom: p.nom,
      prenom: p.prenom,
      dateNaissance: p.dateNaissance.substring(0, 10),
      sexe: p.sexe,
      telephone: p.telephone,
      email: p.email
    });
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.patientForm.invalid) return;
    const data = this.patientForm.value;
    if (this.editingId) {
      this.patientSvc.update(this.editingId, data).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.load();
        },
        error: (e) => console.error('Update patient error:', e)
      });
    } else {
      this.patientSvc.create(data).subscribe({
        next: () => {
          this.showForm = false;
          this.cdr.detectChanges();
          this.load();
        },
        error: (e) => console.error('Create patient error:', e)
      });
    }
  }

  delete(id: number): void {
    if (confirm('Supprimer ce patient ?')) {
      this.patientSvc.delete(id).subscribe(() => this.load());
    }
  }

  viewDossier(p: Patient): void {
    this.selectedPatient = p;
    this.dossier = null;
    this.dossierError = '';
    this.dossierSvc.getByPatient(p.idPatient).subscribe({
      next: d => { this.dossier = d; this.cdr.detectChanges(); },
      error: () => { this.dossierError = 'Aucun dossier médical trouvé.'; this.cdr.detectChanges(); }
    });
    this.showDossier = true;
    this.cdr.detectChanges();
  }

  closeModal(): void {
    this.showForm = false;
    this.showDossier = false;
    this.cdr.detectChanges();
  }

  age(dateNaissance: string): number {
    const today = new Date();
    const birth = new Date(dateNaissance);
    return today.getFullYear() - birth.getFullYear();
  }
}
