using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class FactureGateway : IFactureGateway
{
    private readonly IFactureRepository _repo;

    public FactureGateway(IFactureRepository repo) => _repo = repo;

    public IEnumerable<Facture> GetAll() => _repo.GetAll().Select(Map);

    public Facture? GetByConsultationId(int consultationId)
    {
        var db = _repo.GetByConsultationId(consultationId);
        return db is null ? null : Map(db);
    }

    public void Add(Facture facture) => _repo.Add(ToDb(facture));

    public void UpdateStatut(int idFacture, string statut) => _repo.UpdateStatut(idFacture, statut);

    private static Facture Map(FactureDb db) => new()
    {
        IdFacture = db.idFacture,
        DateFacture = db.dateFacture,
        MontantTotal = db.montantTotal,
        StatutPaiement = db.statutPaiement,
        IdConsultation = db.idConsultation
    };

    private static FactureDb ToDb(Facture f) => new()
    {
        idFacture = f.IdFacture,
        dateFacture = f.DateFacture,
        montantTotal = f.MontantTotal,
        statutPaiement = f.StatutPaiement,
        idConsultation = f.IdConsultation
    };
}
