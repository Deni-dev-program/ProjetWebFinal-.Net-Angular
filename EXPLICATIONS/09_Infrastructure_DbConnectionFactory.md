# DbConnectionFactory — Fabrique de connexions MySQL

**Fichier :** `backend/Infrastructure/Utils/DbConnectionFactory.cs`

## Rôle

La `DbConnectionFactory` est la seule classe du projet qui connaît MySQL. Elle lit la chaîne de connexion depuis la configuration et crée des connexions à la demande.

## Code

```csharp
public class DbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}
```

## Explication

### Lecture de la connexion

```csharp
configuration.GetConnectionString("DefaultConnection")
```

Lit la valeur `DefaultConnection` depuis `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=clinique_db;User=root;Password=..."
  }
}
```

`?? throw new InvalidOperationException(...)` lance une erreur explicite si la clé est absente — plutôt qu'une `NullReferenceException` mystérieuse plus tard.

---

### Création d'une connexion

```csharp
public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
```

Retourne `IDbConnection` (interface standard .NET), pas `MySqlConnection` (classe concrète). Ainsi, les Repositories dépendent de l'interface standard, pas de MySQL directement.

---

## Pourquoi une fabrique et pas une connexion unique ?

Les connexions de base de données ne doivent **pas** être partagées entre requêtes (problèmes de thread-safety). Chaque Repository crée une connexion, l'utilise, et la ferme :

```csharp
using var conn = _factory.CreateConnection();
return conn.Query<PatientDb>("SELECT * FROM patient");
// connexion fermée automatiquement à la fin du bloc using
```

Le `using` garantit que la connexion est toujours fermée, même en cas d'exception.

---

## Durée de vie Singleton

Dans `Infrastructure/ServiceCollectionExtension.cs` :
```csharp
services.AddSingleton<DbConnectionFactory>();
```

`DbConnectionFactory` est un **Singleton** car elle ne fait que stocker la chaîne de connexion (pas d'état mutable). Une seule instance suffit pour toute l'application. Les connexions elles-mêmes (`IDbConnection`) sont créées à la demande et détruites après usage.

---

## Résumé

```
appsettings.json (chaîne de connexion)
    → DbConnectionFactory (lit une fois, garde en mémoire)
        → CreateConnection() → nouvelle MySqlConnection à chaque appel
            → Repository utilise → ferme avec using
```
