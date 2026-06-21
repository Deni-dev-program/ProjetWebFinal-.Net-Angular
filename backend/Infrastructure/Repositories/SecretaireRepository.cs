using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class SecretaireRepository : ISecretaireRepository
{
    private readonly DbConnectionFactory _factory;

    public SecretaireRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<SecretaireDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<SecretaireDb>("SELECT * FROM secretaire");
    }

    public SecretaireDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<SecretaireDb>(
            "SELECT * FROM secretaire WHERE idSecretaire = @id", new { id });
    }

    public int Add(SecretaireDb secretaire)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO secretaire (nom, prenom, email) VALUES (@nom, @prenom, @email)", secretaire);
        return conn.QueryFirst<int>("SELECT LAST_INSERT_ID()");
    }

    public void Update(SecretaireDb secretaire)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE secretaire SET nom=@nom, prenom=@prenom, email=@email WHERE idSecretaire=@idSecretaire",
            secretaire);
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM secretaire WHERE idSecretaire = @id", new { id });
    }
}
