# Infrastructure Gateways — Pont entre Core et Infrastructure

**Fichiers :** `backend/Infrastructure/Gateways/*.cs`  
**Exemple :** `PatientGateway.cs`

## Rôle

Les Gateways sont le **pont** entre la couche Core et la couche Infrastructure. Ils implémentent les interfaces `IPatientGateway` (définies dans Core) et traduisent entre les modèles Core (`Patient`) et les modèles de BDD (`PatientDb`).

## PatientGateway.cs — expliqué

```csharp
public class PatientGateway : IPatientGateway
{
    private readonly IPatientRepository _repo;

    public PatientGateway(IPatientRepository repo) => _repo = repo;
```

Implémente `IPatientGateway` (Core). Reçoit `IPatientRepository` par injection.

---

### Méthodes de délégation + conversion

```csharp
public IEnumerable<Patient> GetAll() => _repo.GetAll().Select(Map);
```

1. `_repo.GetAll()` → liste de `PatientDb` (données brutes SQL)
2. `.Select(Map)` → convertit chaque `PatientDb` en `Patient` (modèle Core)

---

```csharp
public Patient? GetById(int id)
{
    var db = _repo.GetById(id);
    return db is null ? null : Map(db);
}
```

Retourne `null` si pas trouvé (le UseCase gèrera la conversion en exception).

---

```csharp
public void Add(Patient patient) => _repo.Add(ToDb(patient));
public void Update(Patient patient) => _repo.Update(ToDb(patient));
public void Delete(int id) => _repo.Delete(id);
```

Pour les écritures : convertit `Patient` → `PatientDb` avec `ToDb()`, puis délègue au repository.

---

### Méthodes de conversion

```csharp
private static Patient Map(PatientDb db) => new()
{
    IdPatient = db.idPatient,
    Nom = db.nom,
    Prenom = db.prenom,
    DateNaissance = db.dateNaissance,
    Sexe = db.sexe.Length > 0 ? db.sexe[0] : 'M',  // string → char
    Telephone = db.telephone,
    Email = db.email
};
```

**Map** : `PatientDb` (Infrastructure) → `Patient` (Core)  
Ligne importante : `db.sexe[0]` extrait le premier caractère du string SQL pour en faire un `char`.

---

```csharp
private static PatientDb ToDb(Patient p) => new()
{
    idPatient = p.IdPatient,
    nom = p.Nom,
    prenom = p.Prenom,
    dateNaissance = p.DateNaissance,
    sexe = p.Sexe.ToString(),  // char → string
    telephone = p.Telephone,
    email = p.Email
};
```

**ToDb** : `Patient` (Core) → `PatientDb` (Infrastructure)  
`p.Sexe.ToString()` convertit le `char` en `string` pour MySQL.

---

## Pourquoi une couche Gateway en plus du Repository ?

```
Core.IPatientGateway  ←→  PatientGateway  ←→  IPatientRepository  ←→  PatientRepository
   (interface Core)        (conversion)         (interface Infra)        (SQL Dapper)
```

Le Gateway sépare deux responsabilités :
- **Repository** : sait parler à MySQL (SQL, Dapper)
- **Gateway** : sait parler au Core (conversion des types)

Si on change de BDD (ex: PostgreSQL), on change les Repositories. La conversion des modèles dans les Gateways reste identique.

## Résumé du flux

```
UseCases appelle IPatientGateway.GetAll()
    → PatientGateway.GetAll()
        → IPatientRepository.GetAll() → SQL → List<PatientDb>
        ← List<PatientDb>
    → .Select(Map) → List<Patient>
← List<Patient>
```
