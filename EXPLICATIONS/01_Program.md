# Program.cs — Point d'entrée de l'API

**Fichier :** `backend/Api/Program.cs`

## Rôle

C'est le fichier de démarrage de l'application ASP.NET Core. Il configure tous les services (injection de dépendances) et définit le pipeline HTTP.

## Explication ligne par ligne

```csharp
var builder = WebApplication.CreateBuilder(args);
```
Crée le "constructeur" de l'application. C'est ici qu'on configure les services avant que l'app démarre.

---

```csharp
builder.Services.AddOpenApi();
```
Active la documentation automatique de l'API (format OpenAPI/Swagger). Disponible uniquement en mode développement.

---

```csharp
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices();
```
Ces deux lignes enregistrent **tous les services** de notre application dans le conteneur d'injection de dépendances.
- `AddCoreServices()` → enregistre tous les `UseCases` (logique métier)
- `AddInfrastructureServices()` → enregistre tous les `Repositories`, `Gateways`, et `DbConnectionFactory`

Ces méthodes sont définies dans `Core/ServiceCollectionExtension.cs` et `Infrastructure/ServiceCollectionExtension.cs`.

---

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
```
Configure le **CORS** (Cross-Origin Resource Sharing). Sans ça, le navigateur bloquerait les requêtes Angular (port 4200) vers l'API (port 5000/7000) car ce sont des origines différentes. On autorise ici toutes les méthodes HTTP et tous les headers venant de `localhost:4200`.

---

```csharp
var app = builder.Build();
```
Construit l'application avec tous les services configurés.

---

```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
```
En mode développement seulement : active l'endpoint OpenAPI et l'interface Scalar (une alternative à Swagger UI) pour tester l'API visuellement.

---

```csharp
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
```
Configure le **pipeline de middlewares**. L'ordre est important :
1. `GlobalExceptionMiddleware` — attrape toutes les exceptions non gérées
2. `UseHttpsRedirection` — redirige HTTP vers HTTPS
3. `UseCors` — applique la politique CORS définie plus haut

---

```csharp
app.MapPatientRoutes();
app.MapMedecinRoutes();
// ... etc
```
Enregistre toutes les routes de l'API. Chaque `Map...Routes()` est une méthode d'extension définie dans `EndPoints/`.

---

```csharp
app.Run();
```
Démarre le serveur HTTP et attend les requêtes.

## À retenir

Le fichier `Program.cs` ne contient **aucune logique métier**. Son seul rôle est de "câbler" tous les morceaux de l'application ensemble.
