using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class MedecinRepository : IMedecinRepository
{
    private readonly DbConnectionFactory _factory;

    public MedecinRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<MedecinDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<MedecinDb>("SELECT * FROM medecin");
    }

    public MedecinDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<MedecinDb>(
            "SELECT * FROM medecin WHERE idMedecin = @id", new { id });
    }

    public void Add(MedecinDb medecin)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO medecin (nom, specialite, emailPro) VALUES (@nom, @specialite, @emailPro)",
            medecin);
    }

    public void Update(MedecinDb medecin)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE medecin SET nom=@nom, specialite=@specialite, emailPro=@emailPro WHERE idMedecin=@idMedecin",
            medecin);
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM medecin WHERE idMedecin = @id", new { id });
    }
}
