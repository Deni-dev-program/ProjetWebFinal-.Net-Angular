import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Medecin } from '../../core/models/medecin.model';
import { MedecinService } from '../../core/services/medecin.service';

@Component({
  selector: 'app-medecins',
  standalone: false,
  templateUrl: './medecins.component.html',
  styleUrls: ['./medecins.component.css']
})
export class MedecinsComponent implements OnInit {
  medecins: Medecin[] = [];
  showForm = false;
  editingId: number | null = null;
  medecinForm: FormGroup;

  constructor(private medecinSvc: MedecinService, private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.medecinForm = this.fb.group({
      nom: ['', Validators.required],
      specialite: ['', Validators.required],
      emailPro: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.medecinSvc.getAll().subscribe(m => { this.medecins = m; this.cdr.detectChanges(); });
  }

  openCreate(): void {
    this.editingId = null;
    this.medecinForm.reset();
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(m: Medecin): void {
    this.editingId = m.idMedecin;
    this.medecinForm.patchValue({ nom: m.nom, specialite: m.specialite, emailPro: m.emailPro });
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.medecinForm.invalid) return;
    const data = this.medecinForm.value;
    if (this.editingId) {
      this.medecinSvc.update(this.editingId, data).subscribe(() => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.load();
      });
    } else {
      this.medecinSvc.create(data).subscribe(() => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.load();
      });
    }
  }

  delete(id: number): void {
    if (confirm('Supprimer ce médecin ?')) {
      this.medecinSvc.delete(id).subscribe(() => this.load());
    }
  }

  closeModal(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
