import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Medicament } from '../../core/models/medicament.model';
import { MedicamentService } from '../../core/services/medicament.service';

@Component({
  selector: 'app-medicaments',
  standalone: false,
  templateUrl: './medicaments.component.html',
  styleUrls: ['./medicaments.component.css']
})
export class MedicamentsComponent implements OnInit {
  medicaments: Medicament[] = [];
  filtered: Medicament[] = [];
  search = '';
  showForm = false;
  editingId: number | null = null;
  medicamentForm: FormGroup;

  constructor(private medSvc: MedicamentService, private fb: FormBuilder, private cdr: ChangeDetectorRef) {
    this.medicamentForm = this.fb.group({
      nomCommercial: ['', Validators.required],
      principeActif: ['', Validators.required],
      dosage: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.medSvc.getAll().subscribe(m => {
      this.medicaments = m;
      this.applySearch();
      this.cdr.detectChanges();
    });
  }

  applySearch(): void {
    const q = this.search.toLowerCase();
    this.filtered = q
      ? this.medicaments.filter(m =>
          m.nomCommercial.toLowerCase().includes(q) ||
          m.principeActif.toLowerCase().includes(q))
      : [...this.medicaments];
  }

  openCreate(): void {
    this.editingId = null;
    this.medicamentForm.reset();
    this.showForm = true;
    this.cdr.detectChanges();
  }

  openEdit(m: Medicament): void {
    this.editingId = m.idMedicament;
    this.medicamentForm.patchValue({ nomCommercial: m.nomCommercial, principeActif: m.principeActif, dosage: m.dosage });
    this.showForm = true;
    this.cdr.detectChanges();
  }

  save(): void {
    if (this.medicamentForm.invalid) return;
    const data = this.medicamentForm.value;
    if (this.editingId) {
      this.medSvc.update(this.editingId, data).subscribe(() => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.load();
      });
    } else {
      this.medSvc.create(data).subscribe(() => {
        this.showForm = false;
        this.cdr.detectChanges();
        this.load();
      });
    }
  }

  delete(id: number): void {
    if (confirm('Supprimer ce médicament ?')) {
      this.medSvc.delete(id).subscribe(() => this.load());
    }
  }

  closeModal(): void {
    this.showForm = false;
    this.cdr.detectChanges();
  }
}
