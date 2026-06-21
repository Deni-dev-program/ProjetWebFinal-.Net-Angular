using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class DossierMedicalRoutes
{
    public static void MapDossierMedicalRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/dossiers").WithTags("Dossiers médicaux");

        group.MapGet("/patient/{patientId:int}", (int patientId, IDossierMedicalUseCases useCases) =>
            Results.Ok(useCases.GetByPatientId(patientId)));

        group.MapPost("/", (DossierMedicalRequest req, IDossierMedicalUseCases useCases) =>
        {
            var dossier = new DossierMedical
            {
                GroupeSanguin = req.GroupeSanguin,
                Antecedents = req.Antecedents,
                Allergies = req.Allergies,
                IdPatient = req.IdPatient
            };
            useCases.Create(dossier);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, DossierMedicalRequest req, IDossierMedicalUseCases useCases) =>
        {
            var dossier = new DossierMedical
            {
                IdDossier = id,
                GroupeSanguin = req.GroupeSanguin,
                Antecedents = req.Antecedents,
                Allergies = req.Allergies,
                IdPatient = req.IdPatient
            };
            useCases.Update(dossier);
            return Results.NoContent();
        });
    }
}
