# Models (DTOs) — Objets de transfert de données

**Fichiers :** `backend/Api/Models/*.cs`  
**Exemple :** `PatientRequest.cs`

## Rôle

Les DTOs (Data Transfer Objects) définissent la **forme exacte** des données reçues dans le corps des requêtes HTTP. Ce sont les objets que le frontend envoie dans ses requêtes POST et PUT.

## PatientRequest.cs

```csharp
public record PatientRequest(
    string Nom,
    string Prenom,
    DateTime DateNaissance,
    char Sexe,
    string Telephone,
    string Email
);
```

## Pourquoi un `record` et pas une `class` ?

Un `record` en C# est une classe **immuable** (ses valeurs ne changent pas après création) conçue pour transporter des données. Il est parfait pour les DTOs car :
- Syntaxe plus courte
- Égalité basée sur les valeurs (pas sur la référence)
- Signale clairement l'intention : "cet objet ne fait que transporter des données"

## Pourquoi séparer PatientRequest de Patient (Core) ?

| `PatientRequest` (Api) | `Patient` (Core) |
|------------------------|------------------|
| Pas d'`IdPatient` | A un `IdPatient` |
| Reçu du frontend | Utilisé dans la logique métier |
| Peut avoir des annotations de validation | Pur modèle métier |
| Lié au protocole HTTP | Indépendant de HTTP |

Cela protège la couche Core des détails du protocole HTTP. Si demain on change le format de la requête, on ne touche pas au Core.

## Comment ASP.NET Core désérialise automatiquement

Quand Angular envoie :
```json
POST /api/patients
{
    "nom": "Dupont",
    "prenom": "Marie",
    "dateNaissance": "1990-05-15",
    "sexe": "F",
    "telephone": "0612345678",
    "email": "marie@example.com"
}
```

ASP.NET Core lit le JSON et le convertit automatiquement en `PatientRequest` grâce au nom des propriétés (insensible à la casse).

## Validation

La validation des champs obligatoires n'est **pas** faite ici avec des annotations `[Required]`. Elle est faite dans la couche `Core/UseCases` :
```csharp
if (string.IsNullOrWhiteSpace(patient.Nom))
    throw new ArgumentException("Le nom est obligatoire.");
```

Cette approche maintient la logique de validation dans le Core, indépendamment du protocole HTTP.
