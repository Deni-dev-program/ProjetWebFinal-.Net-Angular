# Core ServiceCollectionExtension — Injection de dépendances du Core

**Fichier :** `backend/Core/ServiceCollectionExtension.cs`

## Rôle

Ce fichier enregistre tous les **UseCases** dans le conteneur d'injection de dépendances d'ASP.NET Core. Il centralise la configuration du Core en une seule méthode.

## Code

```csharp
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<IPatientUseCases, PatientUseCases>();
        services.AddTransient<IMedecinUseCases, MedecinUseCases>();
        services.AddTransient<IDossierMedicalUseCases, DossierMedicalUseCases>();
        services.AddTransient<IConsultationUseCases, ConsultationUseCases>();
        services.AddTransient<IFactureUseCases, FactureUseCases>();
        services.AddTransient<IMedicamentUseCases, MedicamentUseCases>();
        services.AddTransient<IPrescriptionUseCases, PrescriptionUseCases>();
        services.AddTransient<IRendezVousUseCases, RendezVousUseCases>();
        return services;
    }
}
```

## Méthode d'extension

`this IServiceCollection services` fait de `AddCoreServices` une **méthode d'extension** sur `IServiceCollection`. Cela permet de l'appeler dans `Program.cs` comme :
```csharp
builder.Services.AddCoreServices();
```

Au lieu de devoir tout copier-coller dans `Program.cs`, on a une seule ligne propre.

## AddTransient — durée de vie des services

`AddTransient<Interface, Implémentation>()` dit : "à chaque fois que quelqu'un demande `IPatientUseCases`, crée une **nouvelle instance** de `PatientUseCases`".

Il existe trois durées de vie :

| Méthode | Durée | Usage typique |
|---------|-------|---------------|
| `AddTransient` | Nouvelle instance à chaque injection | Services sans état (UseCases) |
| `AddScoped` | Une instance par requête HTTP | DbContext (Entity Framework) |
| `AddSingleton` | Une seule instance pour toute l'app | Caches, connexions (DbConnectionFactory) |

Les UseCases sont `Transient` car ils n'ont pas d'état persistant entre les requêtes.

## Connexion avec Program.cs

```csharp
// Program.cs
builder.Services.AddCoreServices();        // enregistre les UseCases (Core)
builder.Services.AddInfrastructureServices(); // enregistre Repos, Gateways, DbFactory (Infrastructure)
```

Ces deux appels configurent toutes les dépendances. ASP.NET Core sait alors :
- Quand un EndPoint demande `IPatientUseCases` → créer `PatientUseCases`
- Quand `PatientUseCases` demande `IPatientGateway` → créer `PatientGateway`
- Quand `PatientGateway` demande `IPatientRepository` → créer `PatientRepository`
- Quand `PatientRepository` demande `DbConnectionFactory` → retourner le singleton existant
