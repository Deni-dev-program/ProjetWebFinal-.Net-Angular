# Infrastructure ServiceCollectionExtension — Injection de dépendances Infrastructure

**Fichier :** `backend/Infrastructure/ServiceCollectionExtension.cs`

## Rôle

Enregistre tous les composants de la couche Infrastructure dans le conteneur d'injection de dépendances : `DbConnectionFactory`, Repositories, et Gateways.

## Code

```csharp
public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
{
    services.AddSingleton<DbConnectionFactory>();

    services.AddTransient<IPatientRepository, PatientRepository>();
    services.AddTransient<IMedecinRepository, MedecinRepository>();
    // ... tous les repositories

    services.AddTransient<IPatientGateway, PatientGateway>();
    services.AddTransient<IMedecinGateway, MedecinGateway>();
    // ... tous les gateways

    return services;
}
```

## Choix des durées de vie

### DbConnectionFactory → Singleton
```csharp
services.AddSingleton<DbConnectionFactory>();
```
Une seule instance pour toute l'application. Elle ne fait que stocker la chaîne de connexion (chaîne immuable). Il serait inutile et coûteux de la recréer à chaque requête.

### Repositories et Gateways → Transient
```csharp
services.AddTransient<IPatientRepository, PatientRepository>();
services.AddTransient<IPatientGateway, PatientGateway>();
```
Nouvelle instance à chaque injection. Ces objets sont légers et sans état persistant entre requêtes. `Transient` est le choix sûr par défaut.

## Chaîne de résolution automatique

Quand ASP.NET Core doit créer un `PatientRoutes` handler :
1. Voit que l'endpoint a besoin de `IPatientUseCases`
2. Cherche dans le DI → crée `PatientUseCases`
3. `PatientUseCases` a besoin de `IPatientGateway`
4. Cherche dans le DI → crée `PatientGateway`
5. `PatientGateway` a besoin de `IPatientRepository`
6. Cherche dans le DI → crée `PatientRepository`
7. `PatientRepository` a besoin de `DbConnectionFactory`
8. Cherche dans le DI → retourne le **singleton existant**

Tout ça est automatique grâce aux enregistrements dans ces deux `ServiceCollectionExtension`.

## Séparation Core vs Infrastructure

| Fichier | Enregistre |
|---------|-----------|
| `Core/ServiceCollectionExtension.cs` | `IPatientUseCases → PatientUseCases` (8 UsesCases) |
| `Infrastructure/ServiceCollectionExtension.cs` | `DbConnectionFactory` + 8 Repositories + 8 Gateways |

Cette séparation respecte le principe : le Core ne sait pas qu'il y a une Infrastructure. Chaque couche enregistre ses propres services.
