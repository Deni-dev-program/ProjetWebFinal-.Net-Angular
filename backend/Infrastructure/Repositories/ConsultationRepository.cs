using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class ConsultationRepository : IConsultationRepository
{
    private readonly DbConnectionFactory _factory;

    public ConsultationRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<ConsultationDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<ConsultationDb>("SELECT * FROM consultation");
    }

    public IEnumerable<ConsultationDb> GetByDossierId(int dossierId)
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<ConsultationDb>(
            "SELECT * FROM consultation WHERE idDossier = @dossierId", new { dossierId });
    }

    public ConsultationDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<ConsultationDb>(
            "SELECT * FROM consultation WHERE idConsultation = @id", new { id });
    }

    public int Add(ConsultationDb consultation)
    {
        using var conn = _factory.CreateConnection();
        return conn.QuerySingle<int>(
            "INSERT INTO consultation (dateConsultation, diagnostic, prix, idDossier, idMedecin) VALUES (@dateConsultation, @diagnostic, @prix, @idDossier, @idMedecin); SELECT LAST_INSERT_ID();",
            consultation);
    }

    public void Update(ConsultationDb consultation)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE consultation SET dateConsultation=@dateConsultation, diagnostic=@diagnostic, prix=@prix, idDossier=@idDossier, idMedecin=@idMedecin WHERE idConsultation=@idConsultation",
            consultation);
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM consultation WHERE idConsultation = @id", new { id });
    }
}
