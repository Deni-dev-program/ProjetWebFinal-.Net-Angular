using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class RendezVousUseCases : IRendezVousUseCases
{
    private readonly IRendezVousGateway _gateway;

    public RendezVousUseCases(IRendezVousGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<RendezVous> GetAll() => _gateway.GetAll();

    public IEnumerable<RendezVous> GetByPatientId(int patientId) => _gateway.GetByPatientId(patientId);

    public IEnumerable<RendezVous> GetByMedecinId(int medecinId) => _gateway.GetByMedecinId(medecinId);

    public RendezVous GetById(int id)
    {
        var rdv = _gateway.GetById(id);
        if (rdv is null) throw new KeyNotFoundException($"Rendez-vous {id} introuvable.");
        return rdv;
    }

    public void Create(RendezVous rdv)
    {
        ArgumentNullException.ThrowIfNull(rdv);
        if (string.IsNullOrWhiteSpace(rdv.Motif)) throw new ArgumentException("Le motif est obligatoire.");
        rdv.Statut = "planifié";
        _gateway.Add(rdv);
    }

    public void Update(RendezVous rdv)
    {
        ArgumentNullException.ThrowIfNull(rdv);
        GetById(rdv.IdRDV);
        if (string.IsNullOrWhiteSpace(rdv.Motif)) throw new ArgumentException("Le motif est obligatoire.");
        _gateway.Update(rdv);
    }

    public void UpdateStatut(int id, string statut)
    {
        GetById(id);
        if (string.IsNullOrWhiteSpace(statut)) throw new ArgumentException("Le statut est obligatoire.");
        _gateway.UpdateStatut(id, statut);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
