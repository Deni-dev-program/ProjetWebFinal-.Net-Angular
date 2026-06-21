using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Api.EndPoints;

public static class MedicamentRoutes
{
    public static void MapMedicamentRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/medicaments").WithTags("Médicaments");

        group.MapGet("/", (IMedicamentUseCases useCases) =>
            Results.Ok(useCases.GetAll()));

        group.MapGet("/{id:int}", (int id, IMedicamentUseCases useCases) =>
            Results.Ok(useCases.GetById(id)));

        group.MapPost("/", (MedicamentRequest req, IMedicamentUseCases useCases) =>
        {
            var med = new Medicament { NomCommercial = req.NomCommercial, PrincipeActif = req.PrincipeActif, Dosage = req.Dosage };
            useCases.Create(med);
            return Results.Created();
        });

        group.MapPut("/{id:int}", (int id, MedicamentRequest req, IMedicamentUseCases useCases) =>
        {
            var med = new Medicament { IdMedicament = id, NomCommercial = req.NomCommercial, PrincipeActif = req.PrincipeActif, Dosage = req.Dosage };
            useCases.Update(med);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", (int id, IMedicamentUseCases useCases) =>
        {
            useCases.Delete(id);
            return Results.NoContent();
        });
    }
}
