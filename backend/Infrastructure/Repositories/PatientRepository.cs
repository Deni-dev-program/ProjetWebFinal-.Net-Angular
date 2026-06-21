using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly DbConnectionFactory _factory;

    public PatientRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<PatientDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<PatientDb>("SELECT * FROM patient");
    }

    public PatientDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<PatientDb>(
            "SELECT * FROM patient WHERE idPatient = @id", new { id });
    }

    public void Add(PatientDb patient)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO patient (nom, prenom, dateNaissance, sexe, telephone, email) VALUES (@nom, @prenom, @dateNaissance, @sexe, @telephone, @email)",
            patient);
    }

    public void Update(PatientDb patient)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE patient SET nom=@nom, prenom=@prenom, dateNaissance=@dateNaissance, sexe=@sexe, telephone=@telephone, email=@email WHERE idPatient=@idPatient",
            patient);
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM patient WHERE idPatient = @id", new { id });
    }
}
