using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class PrescriptionRoutes
{
    public static void MapPrescriptionRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/prescriptions").WithTags("Prescriptions");

        group.MapGet("/consultation/{consultationId:int}", (int consultationId, IPrescriptionUseCases useCases) =>
            Results.Ok(useCases.GetByConsultationId(consultationId)));

        group.MapGet("/{id:int}/lignes", (int id, IPrescriptionUseCases useCases) =>
            Results.Ok(useCases.GetLignes(id)));

        group.MapPost("/", (PrescriptionRequest req, IPrescriptionUseCases useCases) =>
        {
            var prescription = new Prescription
            {
                DatePrescription = DateTime.UtcNow,
                IdConsultation = req.IdConsultation
            };
            var id = useCases.Create(prescription);
            return Results.Created($"/api/prescriptions/{id}", new { id });
        });

        group.MapPost("/lignes", (LignePrescriptionRequest req, IPrescriptionUseCases useCases) =>
        {
            var ligne = new LignePrescription
            {
                Quantite = req.Quantite,
                Posologie = req.Posologie,
                IdPrescription = req.IdPrescription,
                IdMedicament = req.IdMedicament
            };
            useCases.AddLigne(ligne);
            return Results.Created();
        });
    }
}
