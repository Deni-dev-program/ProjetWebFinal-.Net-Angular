using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class FactureRoutes
{
    public static void MapFactureRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/factures").WithTags("Factures");

        group.MapGet("/", (IFactureUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/consultation/{consultationId:int}", (int consultationId, IFactureUseCases useCases) =>
            Results.Ok(useCases.GetByConsultationId(consultationId)));

        group.MapPost("/", (FactureRequest req, IFactureUseCases useCases) =>
        {
            var facture = new Facture
            {
                DateFacture = DateTime.UtcNow,
                MontantTotal = req.MontantTotal,
                StatutPaiement = req.StatutPaiement,
                IdConsultation = req.IdConsultation
            };
            useCases.Create(facture);
            return Results.Created();
        });

        group.MapPatch("/{id:int}/statut", (int id, UpdateStatutRequest req, IFactureUseCases useCases) =>
        {
            useCases.UpdateStatut(id, req.Statut);
            return Results.NoContent();
        });
    }
}
