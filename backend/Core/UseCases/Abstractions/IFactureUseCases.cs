using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IFactureUseCases
{
    IEnumerable<Facture> GetAll();
    Facture GetByConsultationId(int consultationId);
    void Create(Facture facture);
    void UpdateStatut(int idFacture, string statut);
}
