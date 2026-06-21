using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class MedecinUseCases : IMedecinUseCases
{
    private readonly IMedecinGateway _gateway;

    public MedecinUseCases(IMedecinGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Medecin> GetAll() => _gateway.GetAll();

    public Medecin GetById(int id)
    {
        var medecin = _gateway.GetById(id);
        if (medecin is null) throw new KeyNotFoundException($"Médecin {id} introuvable.");
        return medecin;
    }

    public void Create(Medecin medecin)
    {
        ArgumentNullException.ThrowIfNull(medecin);
        if (string.IsNullOrWhiteSpace(medecin.Nom)) throw new ArgumentException("Le nom est obligatoire.");
        _gateway.Add(medecin);
    }

    public void Update(Medecin medecin)
    {
        ArgumentNullException.ThrowIfNull(medecin);
        GetById(medecin.IdMedecin);
        _gateway.Update(medecin);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
