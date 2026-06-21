using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class MedecinGateway : IMedecinGateway
{
    private readonly IMedecinRepository _repo;

    public MedecinGateway(IMedecinRepository repo) => _repo = repo;

    public IEnumerable<Medecin> GetAll() => _repo.GetAll().Select(Map);

    public Medecin? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public void Add(Medecin medecin) => _repo.Add(ToDb(medecin));

    public void Update(Medecin medecin) => _repo.Update(ToDb(medecin));

    public void Delete(int id) => _repo.Delete(id);

    private static Medecin Map(MedecinDb db) => new()
    {
        IdMedecin = db.idMedecin,
        Nom = db.nom,
        Specialite = db.specialite,
        EmailPro = db.emailPro
    };

    private static MedecinDb ToDb(Medecin m) => new()
    {
        idMedecin = m.IdMedecin,
        nom = m.Nom,
        specialite = m.Specialite,
        emailPro = m.EmailPro
    };
}
