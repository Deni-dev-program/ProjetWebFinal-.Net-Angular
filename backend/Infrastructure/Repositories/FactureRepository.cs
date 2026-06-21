using Dapper;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;

namespace Infrastructure.Repositories;

public class FactureRepository : IFactureRepository
{
    private readonly DbConnectionFactory _factory;

    public FactureRepository(DbConnectionFactory factory) => _factory = factory;

    public IEnumerable<FactureDb> GetAll()
    {
        using var conn = _factory.CreateConnection();
        return conn.Query<FactureDb>("SELECT * FROM facture");
    }

    public FactureDb? GetByConsultationId(int consultationId)
    {
        using var conn = _factory.CreateConnection();
        return conn.QueryFirstOrDefault<FactureDb>(
            "SELECT * FROM facture WHERE idConsultation = @consultationId", new { consultationId });
    }

    public void Add(FactureDb facture)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "INSERT INTO facture (dateFacture, montantTotal, statutPaiement, idConsultation) VALUES (@dateFacture, @montantTotal, @statutPaiement, @idConsultation)",
            facture);
    }

    public void UpdateStatut(int idFacture, string statut)
    {
        using var conn = _factory.CreateConnection();
        conn.Execute(
            "UPDATE facture SET statutPaiement=@statut WHERE idFacture=@idFacture",
            new { statut, idFacture });
    }
}
