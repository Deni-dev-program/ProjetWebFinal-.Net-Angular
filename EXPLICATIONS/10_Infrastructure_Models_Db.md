# Infrastructure Models (Db) — Modèles de base de données

**Fichiers :** `backend/Infrastructure/Models/*Db.cs`  
**Exemple :** `PatientDb.cs`

## Rôle

Les modèles `*Db` sont des classes miroirs des **tables MySQL**. Leurs propriétés correspondent exactement aux colonnes de la base de données, ce qui permet à **Dapper** de mapper automatiquement les résultats des requêtes SQL.

## PatientDb.cs

```csharp
public class PatientDb
{
    public int idPatient { get; set; }
    public string nom { get; set; } = string.Empty;
    public string prenom { get; set; } = string.Empty;
    public DateTime dateNaissance { get; set; }
    public string sexe { get; set; } = string.Empty;
    public string telephone { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}
```

## Différences avec Patient (Core)

| Propriété | `Patient` (Core) | `PatientDb` (Infrastructure) |
|-----------|------------------|------------------------------|
| Nommage | `IdPatient` (PascalCase C#) | `idPatient` (camelCase = nom colonne SQL) |
| Sexe | `char Sexe` | `string sexe` (MySQL n'a pas de type `char` unique) |
| Rôle | Logique métier | Représentation SQL |

## Comment Dapper fait le mapping

Quand on exécute :
```csharp
conn.Query<PatientDb>("SELECT * FROM patient");
```

Dapper lit les colonnes SQL retournées (`idPatient`, `nom`, `prenom`, ...) et cherche des propriétés **du même nom** dans `PatientDb`. C'est pourquoi les noms doivent correspondre exactement aux colonnes MySQL.

## Pourquoi deux modèles séparés (PatientDb et Patient) ?

1. **Indépendance du Core** : le Core ne connaît pas la structure SQL. Si on renomme une colonne SQL, on change seulement `PatientDb`, pas `Patient`.

2. **Conversion de types** : MySQL stocke `sexe` en `VARCHAR(1)`, mais le Core préfère un `char`. La conversion se fait dans le `Gateway`.

3. **Tables complexes** : parfois un modèle de BDD (avec clés étrangères et colonnes techniques) n'a pas la même forme qu'un modèle métier.

## Convention de nommage

Les propriétés des modèles `*Db` sont en **camelCase** (`idPatient`, `nom`) pour correspondre aux colonnes MySQL qui sont aussi en camelCase. C'est délibéré et facilite le mapping Dapper sans configuration supplémentaire.
