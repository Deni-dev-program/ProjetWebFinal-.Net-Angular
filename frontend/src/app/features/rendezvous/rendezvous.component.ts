import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RendezVous } from '../../core/models/rendezvous.model';
import { Patient } from '../../core/models/patient.model';
import { Medecin } from '../../core/models/medecin.model';
import { RendezVousService } from '../../core/services/rendezvous.service';
import { PatientService } from '../../core/services/patient.service';
import { MedecinService } from '../../core/services/medecin.service';

@Component({
  selector: 'app-rendezvous',
  standalone: false,
  templateUrl: './rendezvous.component.html',
  styleUrls: ['./rendezvous.component.css']
})
export class RendezVousComponent implements OnInit {
  rdvList: RendezVous[] = [];
  patients: Patient[] = [];
  medecins: Medecin[] = [];
  showForm = false;
  rdvForm: FormGroup;

  readonly statuts = ['planifié', 'confirmé', 'annulé', 'terminé'];

  constructor(
    private rdvSvc: RendezVousService,
    private patientSvc: PatientService,
    private medecinSvc: MedecinService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.rdvForm = this.fb.group({
      dateHeure: ['', Validators.required],
      motif: ['', Validators.required],
      idPatient: [null, Validators.required],
      idMedecin: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.load();
    this.patientSvc.getAll().subscribe(p => { this.patients = p; this.cdr.detectChanges(); });
    this.medecinSvc.getAll().subscribe(m => { this.medecins = m; this.cdr.detectChanges(); });
  }

  load(): void {
    this.rdvSvc.getAll().subscribe(r => { this.rdvList = r; this.cdr.detectChanges(); });
  }

  openCreate(): void {
    this.rdvForm.reset();
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.rdvForm.invalid) return;
    this.rdvSvc.create(this.rdvForm.value).subscribe(() => {
      this.showForm = false;
      this.cdr.detectChanges();
      this.load();
    });
  }

  updateStatut(rdv: RendezVous, statut: string): void {
    this.rdvSvc.updateStatut(rdv.idRDV, statut).subscribe(() => this.load());
  }

  delete(id: number): void {
    if (confirm('Annuler ce rendez-vous ?')) {
      this.rdvSvc.delete(id).subscribe(() => this.load());
    }
  }

  closeModal(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }

  getPatientNom(id: number): string {
    const p = this.patients.find(x => x.idPatient === id);
    return p ? `${p.prenom} ${p.nom}` : `Patient #${id}`;
  }

  getMedecinNom(id: number): string {
    const m = this.medecins.find(x => x.idMedecin === id);
    return m ? `Dr. ${m.nom}` : `Médecin #${id}`;
  }

  badgeClass(statut: string): string {
    const map: Record<string, string> = {
      'planifié': 'badge-info',
      'confirmé': 'badge-success',
      'annulé': 'badge-danger',
      'terminé': 'badge-warning'
    };
    return `badge ${map[statut] ?? 'badge-info'}`;
  }
}
