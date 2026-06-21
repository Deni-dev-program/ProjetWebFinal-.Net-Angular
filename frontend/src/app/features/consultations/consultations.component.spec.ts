import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { ConsultationsComponent } from './consultations.component';
import { ConsultationService } from '../../core/services/consultation.service';
import { MedecinService } from '../../core/services/medecin.service';
import { PatientService } from '../../core/services/patient.service';
import { DossierMedicalService } from '../../core/services/dossier-medical.service';

describe('ConsultationsComponent', () => {
  let component: ConsultationsComponent;
  let fixture: ComponentFixture<ConsultationsComponent>;

  const consultationServiceMock = { getAll: () => of([]), create: () => of({}), delete: () => of({}) };
  const medecinServiceMock = { getAll: () => of([]) };
  const patientServiceMock = { getAll: () => of([]) };
  const dossierServiceMock = { getByPatient: () => of({}) };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ConsultationsComponent],
      imports: [ReactiveFormsModule],
      providers: [
        { provide: ConsultationService, useValue: consultationServiceMock },
        { provide: MedecinService, useValue: medecinServiceMock },
        { provide: PatientService, useValue: patientServiceMock },
        { provide: DossierMedicalService, useValue: dossierServiceMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ConsultationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
