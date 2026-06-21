# Frontend Services — Communication avec l'API

**Fichiers :** `frontend/src/app/core/services/*.service.ts`  
**Exemple :** `patient.service.ts`

## Rôle

Les services Angular encapsulent toute la **communication HTTP** avec l'API backend. Les composants ne font jamais de requêtes HTTP directement — ils passent toujours par un service.

## patient.service.ts — expliqué

```typescript
@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly url = `${environment.apiUrl}/patients`;

  constructor(private http: HttpClient) {}
```

### `@Injectable({ providedIn: 'root' })`

Ce décorateur fait du service un **singleton** accessible partout dans l'application (fourni au niveau racine). Pas besoin de l'enregistrer manuellement dans `AppModule`.

### `environment.apiUrl`

Lit l'URL de base depuis `environments/environment.ts` :
```typescript
export const environment = {
  apiUrl: 'http://localhost:5000/api'
};
```

Cela centralise l'URL de l'API — si elle change, on ne modifie qu'un seul fichier.

---

### getAll()

```typescript
getAll(): Observable<Patient[]> {
  return this.http.get<Patient[]>(this.url);
}
```

- `this.http.get<Patient[]>` : fait un GET HTTP et type la réponse en `Patient[]`
- Retourne un `Observable` (asynchrone) — rien ne se passe tant qu'on ne `.subscribe()` pas

---

### create()

```typescript
create(patient: Omit<Patient, 'idPatient'>): Observable<void> {
  return this.http.post<void>(this.url, patient);
}
```

`Omit<Patient, 'idPatient'>` : on envoie un patient **sans** `idPatient` car la BDD génère l'id automatiquement. TypeScript vérifiera que l'objet envoyé ne contient pas `idPatient`.

---

### update()

```typescript
update(id: number, patient: Omit<Patient, 'idPatient'>): Observable<void> {
  return this.http.put<void>(`${this.url}/${id}`, patient);
}
```

L'id est dans l'URL (`/api/patients/3`), pas dans le corps.

---

### delete()

```typescript
delete(id: number): Observable<void> {
  return this.http.delete<void>(`${this.url}/${id}`);
}
```

---

## Observable — le mécanisme asynchrone d'Angular

Un `Observable` est comme une "promesse" qui peut émettre plusieurs valeurs. Pour déclencher la requête HTTP et recevoir les données, on `.subscribe()` dans le composant :

```typescript
// MAUVAIS — la requête ne part pas !
const obs = this.patientSvc.getAll();

// CORRECT — la requête part quand on subscribe
this.patientSvc.getAll().subscribe(patients => {
  this.patients = patients;
});
```

## Correspondance avec les routes backend

| Méthode service | Appel HTTP | Endpoint backend |
|-----------------|------------|-----------------|
| `getAll()` | `GET /api/patients` | `MapGet("/")` |
| `getById(3)` | `GET /api/patients/3` | `MapGet("/{id:int}")` |
| `create(data)` | `POST /api/patients` | `MapPost("/")` |
| `update(3, data)` | `PUT /api/patients/3` | `MapPut("/{id:int}")` |
| `delete(3)` | `DELETE /api/patients/3` | `MapDelete("/{id:int}")` |

## HttpClient — enregistré dans AppModule

```typescript
// app-module.ts
imports: [
  HttpClientModule,  // nécessaire pour que HttpClient soit injectable
  // ...
]
```

Sans `HttpClientModule`, Angular ne saurait pas comment créer `HttpClient`.
