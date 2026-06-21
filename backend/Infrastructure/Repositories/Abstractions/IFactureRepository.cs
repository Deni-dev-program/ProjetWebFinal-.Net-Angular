using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IFactureRepository
{
    IEnumerable<FactureDb> GetAll();
    FactureDb? GetByConsultationId(int consultationId);
    void Add(FactureDb facture);
    void UpdateStatut(int idFacture, string statut);
}
