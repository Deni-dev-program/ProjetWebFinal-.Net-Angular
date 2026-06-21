# Core Models — Modèles du domaine métier

**Fichiers :** `backend/Core/Models/*.cs`  
**Exemple :** `Patient.cs`

## Rôle

Les modèles Core représentent les **entités métier** de la clinique. Ce sont les objets qui circulent entre les couches Api, Core et Infrastructure. Ils ne contiennent aucune dépendance externe.

## Patient.cs

```csharp
public class Patient
{
    public int IdPatient { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public DateTime DateNaissance { get; set; }
    public char Sexe { get; set; }
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

## Caractéristiques importantes

- **PascalCase** : les propriétés suivent la convention C# (`IdPatient`, `Nom`)
- **`= string.Empty`** : initialisation par défaut pour éviter les `null` sur les strings
- **`char Sexe`** : un seul caractère (`'M'` ou `'F'`)
- **Pas d'annotations** : aucune dépendance à Entity Framework, Dapper, ou HTTP

## Les 9 entités du projet

| Modèle | Description |
|--------|-------------|
| `Patient` | Un patient de la clinique |
| `Medecin` | Un médecin de la clinique |
| `RendezVous` | Un rendez-vous entre un patient et un médecin |
| `DossierMedical` | Le dossier médical d'un patient |
| `Consultation` | Une consultation médicale |
| `Facture` | Une facture liée à une consultation |
| `Medicament` | Un médicament référencé dans la base |
| `Prescription` | Une ordonnance liée à une consultation |
| `LignePrescription` | Une ligne d'une prescription (médicament + quantité) |

## Différence avec PatientDb (Infrastructure)

| `Patient` (Core) | `PatientDb` (Infrastructure) |
|------------------|------------------------------|
| `IdPatient` (PascalCase) | `idPatient` (camelCase = colonne SQL) |
| `char Sexe` | `string sexe` (MySQL stocke en VARCHAR) |
| Indépendant de la BDD | Mappé directement sur la table SQL |
| Utilisé dans la logique métier | Utilisé par Dapper pour les requêtes |

Cette séparation permet de changer le schéma de la base de données sans impacter les UseCases.

## Principe de la Clean Architecture

Le Core est la couche **la plus interne** et **la plus stable**. Elle ne dépend de rien d'autre dans le projet. C'est pourquoi elle ne contient que des classes C# pures.
