using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class PatientRoutes
{
    public static void MapPatientRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/patients").WithTags("Patients");

        group.MapGet("/", (IPatientUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, IPatientUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapPost("/", (PatientRequest req, IPatientUseCases useCases) =>
        {
            var patient = new Patient
            {
                Nom = req.Nom,
                Prenom = req.Prenom,
                DateNaissance = req.DateNaissance,
                Sexe = req.Sexe,
                Telephone = req.Telephone,
                Email = req.Email
            };
            useCases.Create(patient);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, PatientRequest req, IPatientUseCases useCases) =>
        {
            var patient = new Patient
            {
                IdPatient = id,
                Nom = req.Nom,
                Prenom = req.Prenom,
                DateNaissance = req.DateNaissance,
                Sexe = req.Sexe,
                Telephone = req.Telephone,
                Email = req.Email
            };
            useCases.Update(patient);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IPatientUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
