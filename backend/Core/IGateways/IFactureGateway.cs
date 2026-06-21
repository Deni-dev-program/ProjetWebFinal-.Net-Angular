using Core.Models;

namespace Core.IGateways;

public interface IFactureGateway
{
    IEnumerable<Facture> GetAll();
    Facture? GetByConsultationId(int consultationId);
    void Add(Facture facture);
    void UpdateStatut(int idFacture, string statut);
}
