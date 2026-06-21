using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class RendezVousRepository : IRendezVousRepository
{
    private readonly DbConnectionFactory _factory;

    public RendezVousRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<RendezVousDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<RendezVousDb>("SELECT * FROM rendezvous");
    }

    public IEnumerable<RendezVousDb> GetByPatientId(int patientId)
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<RendezVousDb>(
            "SELECT * FROM rendezvous WHERE idPatient = @patientId", new { patientId });
    }

    public IEnumerable<RendezVousDb> GetByMedecinId(int medecinId)
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<RendezVousDb>(
            "SELECT * FROM rendezvous WHERE idMedecin = @medecinId", new { medecinId });
    }

    public RendezVousDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<RendezVousDb>(
            "SELECT * FROM rendezvous WHERE idRDV = @id", new { id });
    }

    public void Add(RendezVousDb rdv)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO rendezvous (dateHeure, motif, statut, idPatient, idMedecin) VALUES (@dateHeure, @motif, @statut, @idPatient, @idMedecin)",
            rdv);
    }

    public void Update(RendezVousDb rdv)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE rendezvous SET dateHeure=@dateHeure, motif=@motif, idPatient=@idPatient, idMedecin=@idMedecin WHERE idRDV=@idRDV",
            rdv);
    }

    public void UpdateStatut(int id, string statut)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("UPDATE rendezvous SET statut=@statut WHERE idRDV=@id", new { statut, id });
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM rendezvous WHERE idRDV = @id", new { id });
    }
}
