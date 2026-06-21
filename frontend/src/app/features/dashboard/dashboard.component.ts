import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { PatientService } from '../../core/services/patient.service';
import { MedecinService } from '../../core/services/medecin.service';
import { RendezVousService } from '../../core/services/rendezvous.service';
import { ConsultationService } from '../../core/services/consultation.service';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  totalPatients = 0;
  totalMedecins = 0;
  totalRdv = 0;
  totalConsultations = 0;

  constructor(
    private patientSvc: PatientService,
    private medecinSvc: MedecinService,
    private rdvSvc: RendezVousService,
    private consultSvc: ConsultationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.patientSvc.getAll().subscribe(p => { this.totalPatients = p.length; this.cdr.detectChanges(); });
    this.medecinSvc.getAll().subscribe(m => { this.totalMedecins = m.length; this.cdr.detectChanges(); });
    this.rdvSvc.getAll().subscribe(r => { this.totalRdv = r.length; this.cdr.detectChanges(); });
    this.consultSvc.getAll().subscribe(c => { this.totalConsultations = c.length; this.cdr.detectChanges(); });
  }
}
