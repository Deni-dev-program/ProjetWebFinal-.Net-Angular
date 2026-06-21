using Dapper;
using Infrastructure.Utils;

namespace Infrastructure;

public class DataSeeder
{
    private readonly DbConnectionFactory _factory;

    public DataSeeder(DbConnectionFactory factory) => _factory = factory;

    public void Seed()
    {
        using var conn = _factory.CreateConnection();

        var count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM utilisateur");
        if (count > 0) return;

        // Secrétaire par défaut
        conn.Execute("INSERT IGNORE INTO secretaire (nom, prenom, email) VALUES ('Admin', 'Secrétaire', 'secretaire@clinique.be')");
        var secId = conn.QueryFirst<int>("SELECT idSecretaire FROM secretaire WHERE email = 'secretaire@clinique.be'");
        conn.Execute(
            "INSERT INTO utilisateur (email, passwordHash, role, idRef) VALUES (@e, @h, @r, @id)",
            new { e = "secretaire@clinique.be", h = BCrypt.Net.BCrypt.HashPassword("admin1234"), r = "secretaire", id = secId });

        // Patients déjà seedés dans db.sql
        var patients = conn.Query("SELECT idPatient, email FROM patient");
        foreach (var p in patients)
        {
            conn.Execute(
                "INSERT IGNORE INTO utilisateur (email, passwordHash, role, idRef) VALUES (@e, @h, @r, @id)",
                new { e = (string)p.email, h = BCrypt.Net.BCrypt.HashPassword("patient1234"), r = "patient", id = (int)p.idPatient });
        }

        // Médecins déjà seedés dans db.sql
        var medecins = conn.Query("SELECT idMedecin, emailPro FROM medecin");
        foreach (var m in medecins)
        {
            conn.Execute(
                "INSERT IGNORE INTO utilisateur (email, passwordHash, role, idRef) VALUES (@e, @h, @r, @id)",
                new { e = (string)m.emailPro, h = BCrypt.Net.BCrypt.HashPassword("medecin1234"), r = "medecin", id = (int)m.idMedecin });
        }
    }
}
