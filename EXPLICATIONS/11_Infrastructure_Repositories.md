# Infrastructure Repositories — Accès à la base de données

**Fichiers :** `backend/Infrastructure/Repositories/*.cs`  
**Exemple :** `PatientRepository.cs`

## Rôle

Les Repositories sont responsables des **requêtes SQL**. Ils utilisent **Dapper** pour exécuter des requêtes paramétrées et mapper les résultats en objets `*Db`.

## PatientRepository.cs — expliqué

```csharp
public class PatientRepository : IPatientRepository
{
    private readonly DbConnectionFactory _factory;

    public PatientRepository(DbConnectionFactory factory) => _factory = factory;
```

Le repository reçoit `DbConnectionFactory` par injection. Il l'utilise pour créer des connexions à la demande.

---

### GetAll

```csharp
public IEnumerable<PatientDb> GetAll()
{
    using var conn = _factory.CreateConnection();
    return conn.Query<PatientDb>("SELECT * FROM patient");
}
```

- `using var conn` : la connexion est ouverte puis **automatiquement fermée** à la fin du bloc
- `conn.Query<PatientDb>(sql)` : Dapper exécute le SQL et mappe chaque ligne en `PatientDb`

---

### GetById

```csharp
public PatientDb? GetById(int id)
{
    using var conn = _factory.CreateConnection();
    return conn.QueryFirstOrDefault<PatientDb>(
        "SELECT * FROM patient WHERE idPatient = @id", new { id });
}
```

- `@id` : **paramètre nommé** dans le SQL — protège contre les injections SQL
- `new { id }` : objet anonyme qui fournit la valeur du paramètre
- `QueryFirstOrDefault` : retourne `null` si aucune ligne trouvée

---

### Add

```csharp
public void Add(PatientDb patient)
{
    using var conn = _factory.CreateConnection();
    conn.Execute(
        "INSERT INTO patient (nom, prenom, dateNaissance, sexe, telephone, email) VALUES (@nom, @prenom, @dateNaissance, @sexe, @telephone, @email)",
        patient);
}
```

- `conn.Execute(sql, objet)` : Dapper mappe les propriétés de `patient` sur les `@paramètres` du SQL
- Pas de `SELECT` → on utilise `Execute` (pour les INSERT/UPDATE/DELETE)

---

### Update

```csharp
public void Update(PatientDb patient)
{
    using var conn = _factory.CreateConnection();
    conn.Execute(
        "UPDATE patient SET nom=@nom, prenom=@prenom, ... WHERE idPatient=@idPatient",
        patient);
}
```

Même principe : les `@paramètres` sont remplacés par les valeurs de l'objet `patient`.

---

### Delete

```csharp
public void Delete(int id)
{
    using var conn = _factory.CreateConnection();
    conn.Execute("DELETE FROM patient WHERE idPatient = @id", new { id });
}
```

---

## Dapper vs Entity Framework

| | **Dapper** (ce projet) | **Entity Framework** |
|---|------------------------|---------------------|
| Style | Micro-ORM : SQL manuel | ORM complet : génère le SQL |
| Contrôle | Total sur le SQL | Abstrait, SQL automatique |
| Performance | Très rapide | Overhead supplémentaire |
| Apprentissage | Connaissance SQL requise | Moins de SQL requis |
| Configuration | Minimale | Plus complexe (migrations) |

## Protection contre les injections SQL

Dapper utilise des **requêtes paramétrées** :
```csharp
// CORRECT — paramètre @id remplacé de façon sécurisée
"WHERE idPatient = @id", new { id }

// DANGEREUX — ne jamais faire ça
$"WHERE idPatient = {id}"  // vulnérable aux injections SQL
```

Les paramètres sont toujours traités comme des valeurs, jamais comme du code SQL.
