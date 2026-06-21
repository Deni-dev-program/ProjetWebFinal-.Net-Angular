import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RendezVous } from '../../core/models/rendezvous.model';
import { Patient } from '../../core/models/patient.model';
import { Medecin } from '../../core/models/medecin.model';
import { RendezVousService } from '../../core/services/rendezvous.service';
import { PatientService } from '../../core/services/patient.service';
import { MedecinService } from '../../core/services/medecin.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-patient-portal',
  standalone: false,
  templateUrl: './patient-portal.component.html',
  styleUrls: ['./patient-portal.component.css']
})
export class PatientPortalComponent implements OnInit {
  activeTab: 'rdv' | 'profil' = 'rdv';

  rdvList: RendezVous[] = [];
  medecins: Medecin[] = [];
  profil: Patient | null = null;

  showRdvForm = false;
  editingRdv: RendezVous | null = null;
  rdvForm: FormGroup;
  profilForm: FormGroup;
  profilEditMode = false;

  patientId: number;

  constructor(
    private fb: FormBuilder,
    private rdvSvc: RendezVousService,
    private patientSvc: PatientService,
    private medecinSvc: MedecinService,
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {
    this.patientId = this.authService.getIdRef();

    this.rdvForm = this.fb.group({
      dateHeure: ['', Validators.required],
      motif: ['', Validators.required],
      idMedecin: [null, Validators.required]
    });

    this.profilForm = this.fb.group({
      nom: ['', Validators.required],
      prenom: ['', Validators.required],
      dateNaissance: ['', Validators.required],
      sexe: ['M'],
      telephone: [''],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    this.loadRdv();
    this.medecinSvc.getAll().subscribe(m => { this.medecins = m; this.cdr.detectChanges(); });
    this.loadProfil();
  }

  loadRdv(): void {
    this.rdvSvc.getByPatient(this.patientId).subscribe(list => {
      this.rdvList = list.sort((a, b) =>
        new Date(b.dateHeure).getTime() - new Date(a.dateHeure).getTime()
      );
      this.cdr.detectChanges();
    });
  }

  loadProfil(): void {
    this.patientSvc.getById(this.patientId).subscribe(p => {
      this.profil = p;
      this.profilForm.patchValue({
        nom: p.nom,
        prenom: p.prenom,
        dateNaissance: p.dateNaissance?.toString().slice(0, 10),
        sexe: p.sexe,
        telephone: p.telephone,
        email: p.email
      });
      this.cdr.detectChanges();
    });
  }

  openNewRdv(): void {
    this.editingRdv = null;
    this.rdvForm.reset({ dateHeure: '', motif: '', idMedecin: null });
    this.showRdvForm = true;
    this.cdr.detectChanges();
  }

  openEditRdv(rdv: RendezVous): void {
    this.editingRdv = rdv;
    this.rdvForm.patchValue({
      dateHeure: new Date(rdv.dateHeure).toISOString().slice(0, 16),
      motif: rdv.motif,
      idMedecin: rdv.idMedecin
    });
    this.showRdvForm = true;
    this.cdr.detectChanges();
  }

  saveRdv(): void {
    if (this.rdvForm.invalid) return;
    const payload = { ...this.rdvForm.value, idPatient: this.patientId };
    if (this.editingRdv) {
      this.rdvSvc.update(this.editingRdv.idRDV, payload).subscribe(() => {
        this.showRdvForm = false;
        this.cdr.detectChanges();
        this.loadRdv();
      });
    } else {
      this.rdvSvc.create(payload).subscribe(() => {
        this.showRdvForm = false;
        this.cdr.detectChanges();
        this.loadRdv();
      });
    }
  }

  annulerRdv(rdv: RendezVous): void {
    if (confirm('Annuler ce rendez-vous ?')) {
      this.rdvSvc.updateStatut(rdv.idRDV, 'annulé').subscribe(() => this.loadRdv());
    }
  }

  saveProfil(): void {
    if (this.profilForm.invalid) return;
    this.patientSvc.update(this.patientId, this.profilForm.value).subscribe(() => {
      this.profilEditMode = false;
      this.cdr.detectChanges();
      this.loadProfil();
    });
  }

  getMedecinNom(id: number): string {
    const m = this.medecins.find(x => x.idMedecin === id);
    return m ? `Dr. ${m.nom} — ${m.specialite}` : `Médecin #${id}`;
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

  canModify(rdv: RendezVous): boolean {
    return rdv.statut === 'planifié' && new Date(rdv.dateHeure) > new Date();
  }
}
