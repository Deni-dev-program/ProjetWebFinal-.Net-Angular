import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Consultation } from '../../core/models/consultation.model';
import { Medecin } from '../../core/models/medecin.model';
import { Patient } from '../../core/models/patient.model';
import { DossierMedical } from '../../core/models/dossier-medical.model';
import { ConsultationService } from '../../core/services/consultation.service';
import { MedecinService } from '../../core/services/medecin.service';
import { PatientService } from '../../core/services/patient.service';
import { DossierMedicalService } from '../../core/services/dossier-medical.service';

@Component({
  selector: 'app-consultations',
  standalone: false,
  templateUrl: './consultations.component.html',
  styleUrls: ['./consultations.component.css']
})
export class ConsultationsComponent implements OnInit {
  consultations: Consultation[] = [];
  medecins: Medecin[] = [];
  patients: Patient[] = [];
  dossiers: Map<number, DossierMedical> = new Map();
  showForm = false;
  consultForm: FormGroup;

  constructor(
    private consultSvc: ConsultationService,
    private medecinSvc: MedecinService,
    private patientSvc: PatientService,
    private dossierSvc: DossierMedicalService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.consultForm = this.fb.group({
      idPatient: [null, Validators.required],
      idMedecin: [null, Validators.required],
      dateConsultation: ['', Validators.required],
      diagnostic: ['', Validators.required],
      prix: [null, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.medecinSvc.getAll().subscribe(m => { this.medecins = m; this.cdr.detectChanges(); });
    this.patientSvc.getAll().subscribe(p => {
      this.patients = p;
      p.forEach(pat => {
        this.dossierSvc.getByPatient(pat.idPatient).subscribe({
          next: d => this.dossiers.set(pat.idPatient, d)
        });
      });
      this.cdr.detectChanges();
    });
    this.load();
  }

  load(): void {
    this.consultSvc.getAll().subscribe(c => { this.consultations = c; this.cdr.detectChanges(); });
  }

  openCreate(): void {
    this.consultForm.reset();
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.consultForm.invalid) return;
    const val = this.consultForm.value;
    const patientId = val.idPatient;
    const dossier = this.dossiers.get(Number(patientId));
    if (!dossier) {
      alert('Ce patient n\'a pas de dossier médical.');
      return;
    }
    const payload = {
      dateConsultation: val.dateConsultation,
      diagnostic: val.diagnostic,
      prix: val.prix,
      idDossier: dossier.idDossier,
      idMedecin: Number(val.idMedecin)
    };
    this.consultSvc.create(payload).subscribe(() => {
      this.showForm = false;
      this.cdr.detectChanges();
      this.load();
    });
  }

  delete(id: number): void {
    if (confirm('Supprimer cette consultation ?')) {
      this.consultSvc.delete(id).subscribe(() => this.load());
    }
  }

  closeModal(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }

  getMedecinNom(id: number): string {
    const m = this.medecins.find(x => x.idMedecin === id);
    return m ? `Dr. ${m.nom}` : `#${id}`;
  }
}
