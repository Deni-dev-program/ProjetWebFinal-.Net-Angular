using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly DbConnectionFactory _factory;

    public AuthRepository(DbConnectionFactory factory) => _factory = factory;

    public UtilisateurDb? GetByEmail(string email)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<UtilisateurDb>(
            "SELECT * FROM utilisateur WHERE email = @email", new { email });
    }

    public void AddUtilisateur(UtilisateurDb utilisateur)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO utilisateur (email, passwordHash, role, idRef) VALUES (@email, @passwordHash, @role, @idRef)",
            utilisateur);
    }

    public int AddPatientAndGetId(string nom, string prenom, DateTime dateNaissance, char sexe, string telephone, string email)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO patient (nom, prenom, dateNaissance, sexe, telephone, email) VALUES (@nom, @prenom, @dateNaissance, @sexe, @telephone, @email)",
            new { nom, prenom, dateNaissance, sexe = sexe.ToString(), telephone, email });
        return conn.QueryFirst<int>("SELECT LAST_INSERT_ID()");
    }
}
