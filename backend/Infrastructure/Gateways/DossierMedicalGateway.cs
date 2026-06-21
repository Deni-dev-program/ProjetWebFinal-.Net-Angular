using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class DossierMedicalGateway : IDossierMedicalGateway
{
    private readonly IDossierMedicalRepository _repo;

    public DossierMedicalGateway(IDossierMedicalRepository repo) => _repo = repo;

    public DossierMedical? GetByPatientId(int patientId)
    {
        var db = _repo.GetByPatientId(patientId);
        return db is null ? null : Map(db);
    }

    public void Add(DossierMedical dossier) => _repo.Add(ToDb(dossier));

    public void Update(DossierMedical dossier) => _repo.Update(ToDb(dossier));

    private static DossierMedical Map(DossierMedicalDb db) => new()
    {
        IdDossier = db.idDossier,
        GroupeSanguin = db.groupeSanguin,
        Antecedents = db.antecedents,
        Allergies = db.allergies,
        IdPatient = db.idPatient
    };

    private static DossierMedicalDb ToDb(DossierMedical d) => new()
    {
        idDossier = d.IdDossier,
        groupeSanguin = d.GroupeSanguin,
        antecedents = d.Antecedents,
        allergies = d.Allergies,
        idPatient = d.IdPatient
    };
}
