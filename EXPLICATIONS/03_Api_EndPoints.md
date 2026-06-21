# EndPoints — Routes de l'API (Minimal APIs)

**Fichiers :** `backend/Api/EndPoints/*.cs`  
**Exemple :** `PatientRoutes.cs`

## Rôle

Les fichiers EndPoints définissent les routes HTTP de l'API (les URLs que le frontend peut appeler). Ce projet utilise les **Minimal APIs** d'ASP.NET Core 9 : une façon concise de définir des routes sans contrôleur.

## Structure type : PatientRoutes.cs

```csharp
public static class PatientRoutes
{
    public static void MapPatientRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/patients").WithTags("Patients");
        // ...
    }
}
```

- `static` : la classe ne s'instancie pas, elle fournit une méthode utilitaire
- `this WebApplication app` : c'est une **méthode d'extension** sur `WebApplication`. On peut donc l'appeler dans `Program.cs` comme `app.MapPatientRoutes()`
- `MapGroup("/api/patients")` : toutes les routes du groupe commencent par `/api/patients`
- `WithTags("Patients")` : regroupe les routes dans la documentation Scalar/Swagger

## Les 5 routes CRUD

```csharp
// GET /api/patients  → liste tous les patients
group.MapGet("/", (IPatientUseCases useCases) =>
    Results.Ok(useCases.GetAll()));
```

```csharp
// GET /api/patients/3  → récupère le patient avec id=3
group.MapGet("/{id:int}", (int id, IPatientUseCases useCases) =>
    Results.Ok(useCases.GetById(id)));
```

```csharp
// POST /api/patients  → crée un nouveau patient
group.MapPost("/", (PatientRequest req, IPatientUseCases useCases) =>
{
    var patient = new Patient { Nom = req.Nom, ... };
    useCases.Create(patient);
    return Results.Created();
});
```

```csharp
// PUT /api/patients/3  → modifie le patient avec id=3
group.MapPut("/{id:int}", (int id, PatientRequest req, IPatientUseCases useCases) =>
{
    var patient = new Patient { IdPatient = id, Nom = req.Nom, ... };
    useCases.Update(patient);
    return Results.NoContent();
});
```

```csharp
// DELETE /api/patients/3  → supprime le patient avec id=3
group.MapDelete("/{id:int}", (int id, IPatientUseCases useCases) =>
{
    useCases.Delete(id);
    return Results.NoContent();
});
```

## Injection de dépendances automatique

ASP.NET Core injecte automatiquement `IPatientUseCases` dans les paramètres des lambdas. C'est possible parce que `PatientUseCases` a été enregistré dans `Program.cs` via `AddCoreServices()`.

## Codes de retour HTTP utilisés

| Code | Méthode | Signification |
|------|---------|---------------|
| 200 OK | `Results.Ok(data)` | Succès avec données |
| 201 Created | `Results.Created()` | Création réussie |
| 204 No Content | `Results.NoContent()` | Succès sans données à retourner |
| 400 Bad Request | via Middleware | Données invalides |
| 404 Not Found | via Middleware | Ressource introuvable |

## Conversion DTO → Modèle métier

L'endpoint reçoit un `PatientRequest` (DTO) et le convertit en `Patient` (modèle Core) :
```csharp
var patient = new Patient
{
    Nom = req.Nom,
    Prenom = req.Prenom,
    // ...
};
```
Cela maintient la séparation : le Core ne connaît pas la forme exacte des requêtes HTTP.
