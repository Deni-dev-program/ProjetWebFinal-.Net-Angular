# Frontend Models — Interfaces TypeScript

**Fichiers :** `frontend/src/app/core/models/*.model.ts`  
**Exemple :** `patient.model.ts`

## Rôle

Les modèles TypeScript définissent la **forme des données** que le frontend reçoit de l'API et utilise dans les composants. Ce sont des `interface` TypeScript (pas des classes) — elles existent seulement à la compilation, pas à l'exécution.

## patient.model.ts

```typescript
export interface Patient {
  idPatient: number;
  nom: string;
  prenom: string;
  dateNaissance: string;
  sexe: string;
  telephone: string;
  email: string;
}
```

## Pourquoi une `interface` et pas une `class` ?

En TypeScript, une `interface` est suffisante pour typer des données JSON. Elle ne génère aucun code JavaScript, elle sert uniquement à la vérification de types à la compilation. Une `class` serait nécessaire seulement si on avait besoin de méthodes ou d'un constructeur.

## Correspondance avec le backend C#

| `Patient` (C# Core) | `Patient` (TypeScript) |
|---------------------|------------------------|
| `int IdPatient` | `number idPatient` |
| `string Nom` | `string nom` |
| `char Sexe` | `string sexe` |
| `DateTime DateNaissance` | `string dateNaissance` |

`dateNaissance` est un `string` en TypeScript car JSON n'a pas de type `Date`. La date arrive comme `"1990-05-15T00:00:00"` et on la manipule comme string.

`sexe` est un `string` (et non `char`) car TypeScript n'a pas de type `char`.

## Utilisation dans les composants et services

```typescript
// Dans le service
getAll(): Observable<Patient[]> { ... }

// Dans le composant
patients: Patient[] = [];
this.patientSvc.getAll().subscribe(p => this.patients = p);

// Dans le template
@for (p of patients; track p.idPatient) {
  <tr>{{ p.nom }}</tr>
}
```

TypeScript vérifie que `p.nom` existe bien dans l'interface `Patient`. Si on tape `p.Nom` (PascalCase), TypeScript signale une erreur de compilation.

## Les 8 modèles du frontend

| Fichier | Interface |
|---------|-----------|
| `patient.model.ts` | `Patient` |
| `medecin.model.ts` | `Medecin` |
| `rendezvous.model.ts` | `RendezVous` |
| `consultation.model.ts` | `Consultation` |
| `dossier-medical.model.ts` | `DossierMedical` |
| `facture.model.ts` | `Facture` |
| `medicament.model.ts` | `Medicament` |
| *(prescriptions via consultation)* | — |
