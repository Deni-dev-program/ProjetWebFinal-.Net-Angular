# GlobalExceptionMiddleware — Gestion globale des erreurs

**Fichier :** `backend/Api/Middleware/GlobalExceptionMiddleware.cs`

## Rôle

Un **middleware** est un composant qui s'intercale dans le pipeline HTTP. Chaque requête passe *à travers* lui avant d'atteindre l'endpoint, et chaque réponse passe *à travers* lui au retour.

Ce middleware particulier intercepte toutes les exceptions non gérées et retourne une réponse JSON propre au lieu de planter l'application.

## Sans vs Avec ce middleware

```
SANS middleware :
Exception → ASP.NET Core → Réponse HTML d'erreur (500, page blanche)
                           Angular reçoit du HTML → bug d'affichage

AVEC middleware :
Exception → Middleware → Réponse JSON { "error": "message" }
                         Angular reçoit du JSON → peut afficher l'erreur
```

## Explication du code

```csharp
private readonly RequestDelegate _next;
```
`RequestDelegate` représente **la suite du pipeline**. Quand on appelle `_next(context)`, on passe la requête au middleware suivant (ou à l'endpoint final).

---

```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);  // laisse la requête continuer
    }
    catch (KeyNotFoundException ex)
    {
        await WriteError(context, HttpStatusCode.NotFound, ex.Message);
    }
    catch (ArgumentException ex)
    {
        await WriteError(context, HttpStatusCode.BadRequest, ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erreur inattendue");
        await WriteError(context, HttpStatusCode.InternalServerError, "Une erreur interne est survenue.");
    }
}
```

Le `try/catch` entoure toute la suite du pipeline. Si n'importe quel endpoint lance une exception :

| Exception | Code HTTP | Explication |
|-----------|-----------|-------------|
| `KeyNotFoundException` | 404 Not Found | Ressource introuvable (lancée dans UseCases) |
| `ArgumentException` | 400 Bad Request | Données invalides (lancée dans UseCases) |
| Tout le reste | 500 Internal Server Error | Erreur imprévue |

---

```csharp
private static async Task WriteError(HttpContext context, HttpStatusCode status, string message)
{
    context.Response.StatusCode = (int)status;
    context.Response.ContentType = "application/json";
    var body = JsonSerializer.Serialize(new { error = message });
    await context.Response.WriteAsync(body);
}
```

Écrit une réponse JSON standardisée :
```json
{ "error": "Patient 5 introuvable." }
```

## Connexion avec les UseCases

Les `UseCases` lancent des exceptions métier (ex: `throw new KeyNotFoundException("Patient 5 introuvable.")`). Le middleware les attrape ici et les convertit en réponses HTTP appropriées. Cela évite de mettre des `try/catch` dans chaque endpoint.

## Enregistrement dans le pipeline

Dans `Program.cs` :
```csharp
app.UseMiddleware<GlobalExceptionMiddleware>();
```
Il est enregistré **en premier**, pour intercepter les erreurs de tous les autres middlewares et endpoints.
