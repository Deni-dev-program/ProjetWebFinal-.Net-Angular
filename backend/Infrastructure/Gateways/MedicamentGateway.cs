using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class MedicamentGateway : IMedicamentGateway
{
    private readonly IMedicamentRepository _repo;

    public MedicamentGateway(IMedicamentRepository repo) => _repo = repo;

    public IEnumerable<Medicament> GetAll() => _repo.GetAll().Select(Map);

    public Medicament? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public void Add(Medicament medicament) => _repo.Add(ToDb(medicament));

    public void Update(Medicament medicament) => _repo.Update(ToDb(medicament));

    public void Delete(int id) => _repo.Delete(id);

    private static Medicament Map(MedicamentDb db) => new()
    {
        IdMedicament = db.idMedicament,
        NomCommercial = db.nomCommercial,
        PrincipeActif = db.principeActif,
        Dosage = db.dosage
    };

    private static MedicamentDb ToDb(Medicament m) => new()
    {
        idMedicament = m.IdMedicament,
        nomCommercial = m.NomCommercial,
        principeActif = m.PrincipeActif,
        dosage = m.Dosage
    };
}
