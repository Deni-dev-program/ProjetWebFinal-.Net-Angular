using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class RendezVousRoutes
{
    public static void MapRendezVousRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/rendezvous").WithTags("Rendez-vous");

        group.MapGet("/", (IRendezVousUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, IRendezVousUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapGet("/patient/{patientId:int}", (int patientId, IRendezVousUseCases useCases) =>
            Results.Ok(useCases.GetByPatientId(patientId)));

        group.MapGet("/medecin/{medecinId:int}", (int medecinId, IRendezVousUseCases useCases) =>
            Results.Ok(useCases.GetByMedecinId(medecinId)));

        group.MapPost("/", (RendezVousRequest req, IRendezVousUseCases useCases) =>
        {
            var rdv = new RendezVous
            {
                DateHeure = req.DateHeure,
                Motif = req.Motif,
                IdPatient = req.IdPatient,
                IdMedecin = req.IdMedecin
            };
            useCases.Create(rdv);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, RendezVousRequest req, IRendezVousUseCases useCases) =>
        {
            var rdv = new RendezVous
            {
                IdRDV = id,
                DateHeure = req.DateHeure,
                Motif = req.Motif,
                IdPatient = req.IdPatient,
                IdMedecin = req.IdMedecin
            };
            useCases.Update(rdv);
            return Results.NoContent();
        });

        group.MapPatch("/{id:int}/statut", (int id, UpdateStatutRequest req, IRendezVousUseCases useCases) =>
        {
            useCases.UpdateStatut(id, req.Statut);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IRendezVousUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
