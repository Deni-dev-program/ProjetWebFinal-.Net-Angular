using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class SecretaireUseCases : ISecretaireUseCases
{
    private readonly ISecretaireGateway _gateway;

    public SecretaireUseCases(ISecretaireGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Secretaire> GetAll() => _gateway.GetAll();

    public Secretaire GetById(int id)
    {
        var s = _gateway.GetById(id);
        if (s is null) throw new KeyNotFoundException($"Secrétaire {id} introuvable.");
        return s;
    }

    public void Create(Secretaire secretaire)
    {
        ArgumentNullException.ThrowIfNull(secretaire);
        if (string.IsNullOrWhiteSpace(secretaire.Nom)) throw new ArgumentException("Le nom est obligatoire.");
        if (string.IsNullOrWhiteSpace(secretaire.Email)) throw new ArgumentException("L'email est obligatoire.");
        _gateway.Add(secretaire);
    }

    public void Update(Secretaire secretaire)
    {
        ArgumentNullException.ThrowIfNull(secretaire);
        GetById(secretaire.IdSecretaire);
        _gateway.Update(secretaire);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
