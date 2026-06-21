using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class FactureUseCases : IFactureUseCases
{
    private readonly IFactureGateway _gateway;

    public FactureUseCases(IFactureGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Facture> GetAll() => _gateway.GetAll();

    public Facture GetByConsultationId(int consultationId)
    {
        var f = _gateway.GetByConsultationId(consultationId);
        if (f is null) throw new KeyNotFoundException($"Facture pour la consultation {consultationId} introuvable.");
        return f;
    }

    public void Create(Facture facture)
    {
        ArgumentNullException.ThrowIfNull(facture);
        _gateway.Add(facture);
    }

    public void UpdateStatut(int idFacture, string statut)
    {
        if (string.IsNullOrWhiteSpace(statut)) throw new ArgumentException("Le statut est obligatoire.");
        _gateway.UpdateStatut(idFacture, statut);
    }
}
