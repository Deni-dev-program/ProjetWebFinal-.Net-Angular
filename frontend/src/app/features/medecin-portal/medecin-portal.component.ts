import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RendezVous } from '../../core/models/rendezvous.model';
import { Patient } from '../../core/models/patient.model';
import { RendezVousService } from '../../core/services/rendezvous.service';
import { PatientService } from '../../core/services/patient.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-medecin-portal',
  standalone: false,
  templateUrl: './medecin-portal.component.html',
  styleUrls: ['./medecin-portal.component.css']
})
export class MedecinPortalComponent implements OnInit {
  rdvList: RendezVous[] = [];
  patients: Patient[] = [];
  medecinId: number;
  today = new Date().toDateString();

  readonly statuts = ['planifié', 'confirmé', 'terminé', 'annulé'];

  constructor(
    private rdvSvc: RendezVousService,
    private patientSvc: PatientService,
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {
    this.medecinId = this.authService.getIdRef();
  }

  ngOnInit(): void {
    this.rdvSvc.getByMedecin(this.medecinId).subscribe(list => {
      this.rdvList = list.sort((a, b) =>
        new Date(a.dateHeure).getTime() - new Date(b.dateHeure).getTime()
      );
      this.cdr.detectChanges();
    });
    this.patientSvc.getAll().subscribe(p => { this.patients = p; this.cdr.detectChanges(); });
  }

  get rdvAujourdhui(): RendezVous[] {
    return this.rdvList.filter(r =>
      new Date(r.dateHeure).toDateString() === this.today &&
      r.statut !== 'annulé'
    );
  }

  get rdvAVenir(): RendezVous[] {
    return this.rdvList.filter(r =>
      new Date(r.dateHeure) > new Date() && r.statut !== 'annulé'
    );
  }

  changerStatut(rdv: RendezVous, statut: string): void {
    this.rdvSvc.updateStatut(rdv.idRDV, statut).subscribe(() => {
      rdv.statut = statut;
      this.cdr.detectChanges();
    });
  }

  getPatientNom(id: number): string {
    const p = this.patients.find(x => x.idPatient === id);
    return p ? `${p.prenom} ${p.nom}` : `Patient #${id}`;
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
