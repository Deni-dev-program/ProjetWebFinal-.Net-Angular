import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { PatientsComponent } from './features/patients/patients.component';
import { MedecinsComponent } from './features/medecins/medecins.component';
import { RendezVousComponent } from './features/rendezvous/rendezvous.component';
import { ConsultationsComponent } from './features/consultations/consultations.component';
import { MedicamentsComponent } from './features/medicaments/medicaments.component';
import { LoginComponent } from './features/auth/login/login.component';
import { PatientPortalComponent } from './features/patient-portal/patient-portal.component';
import { MedecinPortalComponent } from './features/medecin-portal/medecin-portal.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  // Portail patient
  {
    path: 'patient-portal',
    component: PatientPortalComponent,
    canActivate: [AuthGuard],
    data: { roles: ['patient'] }
  },

  // Portail médecin
  {
    path: 'medecin-portal',
    component: MedecinPortalComponent,
    canActivate: [AuthGuard],
    data: { roles: ['medecin'] }
  },

  // Espace secrétaire
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },
  {
    path: 'patients',
    component: PatientsComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },
  {
    path: 'medecins',
    component: MedecinsComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },
  {
    path: 'rendezvous',
    component: RendezVousComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },
  {
    path: 'consultations',
    component: ConsultationsComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },
  {
    path: 'medicaments',
    component: MedicamentsComponent,
    canActivate: [AuthGuard],
    data: { roles: ['secretaire'] }
  },

  { path: '**', redirectTo: 'login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
