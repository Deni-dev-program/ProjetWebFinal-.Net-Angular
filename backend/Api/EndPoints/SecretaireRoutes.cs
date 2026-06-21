using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class SecretaireRoutes
{
    public static void MapSecretaireRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/secretaires").WithTags("Secrétaires");

        group.MapGet("/", (ISecretaireUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, ISecretaireUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapPost("/", (SecretaireRequest req, ISecretaireUseCases useCases) =>
        {
            var s = new Secretaire { Nom = req.Nom, Prenom = req.Prenom, Email = req.Email };
            useCases.Create(s);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, SecretaireRequest req, ISecretaireUseCases useCases) =>
        {
            var s = new Secretaire { IdSecretaire = id, Nom = req.Nom, Prenom = req.Prenom, Email = req.Email };
            useCases.Update(s);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, ISecretaireUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
