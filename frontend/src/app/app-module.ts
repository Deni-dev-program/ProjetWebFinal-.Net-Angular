import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { PatientsComponent } from './features/patients/patients.component';
import { MedecinsComponent } from './features/medecins/medecins.component';
import { RendezVousComponent } from './features/rendezvous/rendezvous.component';
import { ConsultationsComponent } from './features/consultations/consultations.component';
import { MedicamentsComponent } from './features/medicaments/medicaments.component';
import { LoginComponent } from './features/auth/login/login.component';
import { PatientPortalComponent } from './features/patient-portal/patient-portal.component';
import { MedecinPortalComponent } from './features/medecin-portal/medecin-portal.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

@NgModule({
  declarations: [
    App,
    DashboardComponent,
    PatientsComponent,
    MedecinsComponent,
    RendezVousComponent,
    ConsultationsComponent,
    MedicamentsComponent,
    LoginComponent,
    PatientPortalComponent,
    MedecinPortalComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [App]
})
export class AppModule {}
