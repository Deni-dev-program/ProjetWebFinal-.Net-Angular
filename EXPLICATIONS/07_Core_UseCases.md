# Core UseCases — Logique métier

**Fichiers :** `backend/Core/UseCases/*.cs`  
**Exemple :** `PatientUseCases.cs`

## Rôle

Les UseCases contiennent la **logique métier** de l'application : validation des données, règles de gestion, coordination des opérations. C'est le "cerveau" du backend.

## PatientUseCases.cs — code complet expliqué

```csharp
public class PatientUseCases : IPatientUseCases
{
    private readonly IPatientGateway _gateway;

    public PatientUseCases(IPatientGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
```

**Injection de dépendance par constructeur.** `IPatientGateway` est une interface — ASP.NET Core injectera automatiquement un `PatientGateway` (Infrastructure) sans que le Core ne sache que c'est MySQL derrière.

La vérification `?? throw new ArgumentNullException(...)` s'assure qu'on ne reçoit pas un `null` (défense à l'entrée).

---

```csharp
public IEnumerable<Patient> GetAll() => _gateway.GetAll();
```

Simple délégation : le UseCase ne fait que transmettre au gateway. Ici il n'y a pas de logique métier particulière pour lister tous les patients.

---

```csharp
public Patient GetById(int id)
{
    var patient = _gateway.GetById(id);
    if (patient is null) throw new KeyNotFoundException($"Patient {id} introuvable.");
    return patient;
}
```

**Règle métier :** un patient introuvable est une erreur. L'exception `KeyNotFoundException` sera attrapée par le `GlobalExceptionMiddleware` et retournera un **404 Not Found** au frontend.

Le gateway retourne `Patient?` (nullable) — le UseCase transforme le `null` en exception explicite.

---

```csharp
public void Create(Patient patient)
{
    ArgumentNullException.ThrowIfNull(patient);
    if (string.IsNullOrWhiteSpace(patient.Nom)) throw new ArgumentException("Le nom est obligatoire.");
    if (string.IsNullOrWhiteSpace(patient.Prenom)) throw new ArgumentException("Le prénom est obligatoire.");
    _gateway.Add(patient);
}
```

**Validation métier :** avant d'insérer, on vérifie que les champs obligatoires sont remplis. Les `ArgumentException` seront attrapées par le Middleware et retourneront un **400 Bad Request**.

---

```csharp
public void Update(Patient patient)
{
    ArgumentNullException.ThrowIfNull(patient);
    GetById(patient.IdPatient);   // vérifie que le patient existe
    _gateway.Update(patient);
}
```

**Règle métier :** on ne peut pas mettre à jour un patient qui n'existe pas. `GetById()` lancera un `KeyNotFoundException` si l'id est introuvable (404).

---

```csharp
public void Delete(int id)
{
    GetById(id);   // vérifie que le patient existe
    _gateway.Delete(id);
}
```

Même principe : vérification de l'existence avant suppression.

---

## Résumé : responsabilités du UseCase

| Responsabilité | Exemple |
|----------------|---------|
| Validation des données | Nom et prénom obligatoires |
| Vérification de l'existence | Patient introuvable → 404 |
| Coordination | Update = vérifier + modifier |
| Protection contre null | `ArgumentNullException.ThrowIfNull` |

## Ce que le UseCase ne fait PAS

- Il ne connaît pas MySQL (délégué au Gateway)
- Il ne connaît pas HTTP (géré par les EndPoints)
- Il ne génère pas le JSON de réponse (fait par ASP.NET Core)
