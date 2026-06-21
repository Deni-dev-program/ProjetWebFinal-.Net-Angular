using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly DbConnectionFactory _factory;

    public PrescriptionRepository(DbConnectionFactory factory) => _factory = factory;

    public PrescriptionDb? GetByConsultationId(int consultationId)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<PrescriptionDb>(
            "SELECT * FROM prescription WHERE idConsultation = @consultationId", new { consultationId });
    }

    public int Add(PrescriptionDb prescription)
    {
        using var conn = _factory.CreateConnection();
        return conn.QuerySingle<int>(
            "INSERT INTO prescription (datePrescription, idConsultation) VALUES (@datePrescription, @idConsultation); SELECT LAST_INSERT_ID();",
            prescription);
    }

    public IEnumerable<LignePrescriptionDb> GetLignesByPrescriptionId(int prescriptionId)
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<LignePrescriptionDb>(
            "SELECT * FROM ligneprescription WHERE idPrescription = @prescriptionId", new { prescriptionId });
    }

    public void AddLigne(LignePrescriptionDb ligne)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO ligneprescription (quantite, posologie, idPrescription, idMedicament) VALUES (@quantite, @posologie, @idPrescription, @idMedicament)",
            ligne);
    }
}
