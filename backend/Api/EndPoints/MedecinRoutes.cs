using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class MedecinRoutes
{
    public static void MapMedecinRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/medecins").WithTags("Médecins");

        group.MapGet("/", (IMedecinUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, IMedecinUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapPost("/", (MedecinRequest req, IMedecinUseCases useCases) =>
        {
            var medecin = new Medecin { Nom = req.Nom, Specialite = req.Specialite, EmailPro = req.EmailPro };
            useCases.Create(medecin);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, MedecinRequest req, IMedecinUseCases useCases) =>
        {
            var medecin = new Medecin { IdMedecin = id, Nom = req.Nom, Specialite = req.Specialite, EmailPro = req.EmailPro };
            useCases.Update(medecin);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IMedecinUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
