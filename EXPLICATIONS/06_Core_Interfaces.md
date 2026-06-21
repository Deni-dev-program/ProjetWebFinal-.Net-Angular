# Interfaces Core — IGateways et IUseCases Abstractions

**Fichiers :**
- `backend/Core/IGateways/I*Gateway.cs`
- `backend/Core/UseCases/Abstractions/I*UseCases.cs`

**Exemples :** `IPatientGateway.cs`, `IPatientUseCases.cs`

## Rôle

Les interfaces définissent des **contrats** : elles décrivent ce qu'un objet peut faire, sans dire comment. Elles sont le mécanisme central de la Clean Architecture pour inverser les dépendances.

## IPatientGateway.cs

```csharp
public interface IPatientGateway
{
    IEnumerable<Patient> GetAll();
    Patient? GetById(int id);
    void Add(Patient patient);
    void Update(Patient patient);
    void Delete(int id);
}
```

**Rôle :** contrat que l'Infrastructure doit respecter pour fournir les données au Core. Le Core ne sait pas s'il y a MySQL, PostgreSQL ou un fichier JSON derrière — il appelle juste cette interface.

---

## IPatientUseCases.cs

```csharp
public interface IPatientUseCases
{
    IEnumerable<Patient> GetAll();
    Patient GetById(int id);
    void Create(Patient patient);
    void Update(Patient patient);
    void Delete(int id);
}
```

**Rôle :** contrat que l'Api peut utiliser pour appeler la logique métier. L'Api ne sait pas comment les données sont validées ou stockées — elle appelle juste cette interface.

---

## Différence entre IPatientGateway et IPatientUseCases

| `IPatientGateway` | `IPatientUseCases` |
|-------------------|--------------------|
| Dans `Core/IGateways/` | Dans `Core/UseCases/Abstractions/` |
| Implémentée par Infrastructure | Implémentée par Core |
| Représente l'accès aux données | Représente la logique métier |
| Appelée par les UseCases | Appelée par les EndPoints |
| `GetById` retourne `Patient?` (peut être null) | `GetById` retourne `Patient` (jamais null, lance une exception) |

---

## Pourquoi utiliser des interfaces ?

### 1. Inversion des dépendances (principe SOLID)
Sans interface :
```
UseCases → PatientGateway (classe concrète d'Infrastructure)
```
Core dépendrait d'Infrastructure → violation de la Clean Architecture.

Avec interface :
```
UseCases → IPatientGateway (interface dans Core)
PatientGateway (Infrastructure) → implémente IPatientGateway
```
Infrastructure dépend de Core, pas l'inverse.

### 2. Testabilité
Pour tester `PatientUseCases` sans base de données, on peut créer un faux gateway :
```csharp
class FakePatientGateway : IPatientGateway {
    public IEnumerable<Patient> GetAll() => new List<Patient>();
    // ...
}
```

### 3. Lisibilité
Les interfaces servent de **documentation vivante**. En lisant `IPatientUseCases`, on comprend immédiatement tout ce qu'on peut faire avec les patients.

---

## Comment l'injection de dépendances connecte tout

Dans `Program.cs` :
```csharp
// Core enregistre : "quand quelqu'un demande IPatientUseCases, donne lui PatientUseCases"
services.AddTransient<IPatientUseCases, PatientUseCases>();

// Infrastructure enregistre : "quand quelqu'un demande IPatientGateway, donne lui PatientGateway"
services.AddTransient<IPatientGateway, PatientGateway>();
```

ASP.NET Core crée automatiquement les bons objets et les injecte là où ils sont demandés.
