using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class DossierMedicalRepository : IDossierMedicalRepository
{
    private readonly DbConnectionFactory _factory;

    public DossierMedicalRepository(DbConnectionFactory factory) => _factory = factory;

    public DossierMedicalDb? GetByPatientId(int patientId)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<DossierMedicalDb>(
            "SELECT * FROM dossiermedical WHERE idPatient = @patientId", new { patientId });
    }

    public void Add(DossierMedicalDb dossier)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO dossiermedical (groupeSanguin, antecedents, allergies, idPatient) VALUES (@groupeSanguin, @antecedents, @allergies, @idPatient)",
            dossier);
    }

    public void Update(DossierMedicalDb dossier)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE dossiermedical SET groupeSanguin=@groupeSanguin, antecedents=@antecedents, allergies=@allergies WHERE idDossier=@idDossier",
            dossier);
    }
}
