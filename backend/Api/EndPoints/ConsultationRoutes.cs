using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class ConsultationRoutes
{
    public static void MapConsultationRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/consultations").WithTags("Consultations");

        group.MapGet("/", (IConsultationUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, IConsultationUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapGet("/dossier/{dossierId:int}", (int dossierId, IConsultationUseCases useCases) =>
            Results.Ok(useCases.GetByDossierId(dossierId)));

        group.MapPost("/", (ConsultationRequest req, IConsultationUseCases useCases) =>
        {
            var consultation = new Consultation
            {
                DateConsultation = req.DateConsultation,
                Diagnostic = req.Diagnostic,
                Prix = req.Prix,
                IdDossier = req.IdDossier,
                IdMedecin = req.IdMedecin
            };
            var id = useCases.Create(consultation);
            return Results.Created($"/api/consultations/{id}", new { id });
        });

        group.MapPut("/{id:int}", (int id, ConsultationRequest req, IConsultationUseCases useCases) =>
        {
            var consultation = new Consultation
            {
                IdConsultation = id,
                DateConsultation = req.DateConsultation,
                Diagnostic = req.Diagnostic,
                Prix = req.Prix,
                IdDossier = req.IdDossier,
                IdMedecin = req.IdMedecin
            };
            useCases.Update(consultation);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IConsultationUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
