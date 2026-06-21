using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    private readonly DbConnectionFactory _factory;

    public MedicamentRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<MedicamentDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<MedicamentDb>("SELECT * FROM medicament");
    }

    public MedicamentDb? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<MedicamentDb>(
            "SELECT * FROM medicament WHERE idMedicament = @id", new { id });
    }

    public void Add(MedicamentDb medicament)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO medicament (nomCommercial, principeActif, dosage) VALUES (@nomCommercial, @principeActif, @dosage)",
            medicament);
    }

    public void Update(MedicamentDb medicament)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE medicament SET nomCommercial=@nomCommercial, principeActif=@principeActif, dosage=@dosage WHERE idMedicament=@idMedicament",
            medicament);
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute("DELETE FROM medicament WHERE idMedicament = @id", new { id });
    }
}
