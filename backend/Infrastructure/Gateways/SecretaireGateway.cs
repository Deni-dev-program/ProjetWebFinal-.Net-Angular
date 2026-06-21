using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class SecretaireGateway : ISecretaireGateway
{
    private readonly ISecretaireRepository _repo;

    public SecretaireGateway(ISecretaireRepository repo) => _repo = repo;

    public IEnumerable<Secretaire> GetAll() => _repo.GetAll().Select(Map);

    public Secretaire? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public void Add(Secretaire secretaire) => _repo.Add(ToDb(secretaire));

    public void Update(Secretaire secretaire) => _repo.Update(ToDb(secretaire));

    public void Delete(int id) => _repo.Delete(id);

    private static Secretaire Map(SecretaireDb db) => new()
    {
        IdSecretaire = db.idSecretaire,
        Nom = db.nom,
        Prenom = db.prenom,
        Email = db.email
    };

    private static SecretaireDb ToDb(Secretaire s) => new()
    {
        idSecretaire = s.IdSecretaire,
        nom = s.Nom,
        prenom = s.Prenom,
        email = s.Email
    };
}
