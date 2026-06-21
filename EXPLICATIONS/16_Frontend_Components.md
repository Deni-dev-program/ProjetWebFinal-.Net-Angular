# Frontend Components — Composants Angular

**Fichiers :** `frontend/src/app/features/**/*.component.ts`  
**Exemple :** `patients.component.ts`

## Rôle

Les composants contrôlent ce que l'utilisateur voit et fait. Chaque composant = une "page" ou une section de l'interface. Il gère l'état local (variables), réagit aux actions (clics, formulaires), et délègue la communication HTTP au service.

## PatientsComponent — structure

```typescript
@Component({
  selector: 'app-patients',
  standalone: false,
  templateUrl: './patients.component.html'
})
export class PatientsComponent implements OnInit {
```

- `selector: 'app-patients'` : balise HTML utilisée dans le template parent (`<app-patients>`)
- `standalone: false` : composant **module-based** (déclaré dans `AppModule`, pas standalone)
- `templateUrl` : fichier HTML séparé qui affiche les données

---

## État du composant (variables)

```typescript
patients: Patient[] = [];          // liste affichée
showForm = false;                  // formulaire visible ?
showDossier = false;               // panneau dossier visible ?
editingId: number | null = null;   // null = création, id = édition
selectedPatient: Patient | null = null;
dossier: DossierMedical | null = null;
dossierError = '';
patientForm: FormGroup;            // formulaire réactif
```

Ces variables sont liées au template HTML via le binding Angular. Quand elles changent, l'affichage se met à jour automatiquement.

---

## Injection de dépendances dans le constructeur

```typescript
constructor(
  private patientSvc: PatientService,
  private dossierSvc: DossierMedicalService,
  private fb: FormBuilder
) {
  this.patientForm = this.fb.group({
    nom: ['', Validators.required],
    prenom: ['', Validators.required],
    dateNaissance: ['', Validators.required],
    sexe: ['M', Validators.required],
    telephone: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]]
  });
}
```

`FormBuilder.group()` crée un **Reactive Form** : chaque champ a une valeur initiale et des validateurs. Angular vérifie automatiquement les règles (`required`, `email`).

---

## Cycle de vie

```typescript
ngOnInit(): void {
  this.load();
}
```

`ngOnInit` est un **lifecycle hook** appelé par Angular après la création du composant. On charge la liste des patients ici (pas dans le constructeur, car les dépendances ne sont pas encore prêtes).

---

## Méthodes CRUD

```typescript
load(): void {
  this.patientSvc.getAll().subscribe(p => this.patients = p);
}
```
Charge la liste. Angular met à jour le `@for` dans le template automatiquement.

---

```typescript
openCreate(): void {
  this.editingId = null;          // mode création
  this.patientForm.reset({ sexe: 'M' });  // réinitialise le formulaire
  this.showForm = true;
}

openEdit(p: Patient): void {
  this.editingId = p.idPatient;   // mode édition
  this.patientForm.patchValue({ ... });   // remplit le formulaire
  this.showForm = true;
}
```

Un seul formulaire pour créer ET modifier. `editingId === null` → création, sinon → modification.

---

```typescript
save(): void {
  if (this.patientForm.invalid) return;   // stop si validation échoue
  const data = this.patientForm.value;
  if (this.editingId) {
    this.patientSvc.update(this.editingId, data).subscribe(() => {
      this.showForm = false;
      this.load();                         // recharger la liste
    });
  } else {
    this.patientSvc.create(data).subscribe(() => {
      this.showForm = false;
      this.load();
    });
  }
}
```

---

```typescript
delete(id: number): void {
  if (confirm('Supprimer ce patient ?')) {   // confirmation native du navigateur
    this.patientSvc.delete(id).subscribe(() => this.load());
  }
}
```

---

## viewDossier — fonctionnalité bonus

```typescript
viewDossier(p: Patient): void {
  this.selectedPatient = p;
  this.dossierSvc.getByPatient(p.idPatient).subscribe({
    next: d => this.dossier = d,
    error: () => this.dossierError = 'Aucun dossier médical trouvé.'
  });
  this.showDossier = true;
}
```

Charge le dossier médical lié au patient sélectionné. Utilise la syntaxe `{ next, error }` pour gérer les deux cas (succès et 404).

---

## Méthode utilitaire

```typescript
age(dateNaissance: string): number {
  const today = new Date();
  const birth = new Date(dateNaissance);
  return today.getFullYear() - birth.getFullYear();
}
```

Calcule l'âge à partir de la date de naissance. Appelée directement dans le template HTML.

---

## Pattern général de tous les composants

```
ngOnInit → load() → service.getAll() → Observable → subscribe → [données]
openCreate/openEdit → showForm = true → formulaire visible
save() → service.create/update → subscribe → reload
delete(id) → confirm → service.delete → subscribe → reload
closeModal() → showForm/showDossier = false
```
