using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class RendezVousGateway : IRendezVousGateway
{
    private readonly IRendezVousRepository _repo;

    public RendezVousGateway(IRendezVousRepository repo) => _repo = repo;

    public IEnumerable<RendezVous> GetAll() => _repo.GetAll().Select(Map);

    public IEnumerable<RendezVous> GetByPatientId(int patientId) =>
        _repo.GetByPatientId(patientId).Select(Map);

    public IEnumerable<RendezVous> GetByMedecinId(int medecinId) =>
        _repo.GetByMedecinId(medecinId).Select(Map);

    public RendezVous? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public void Add(RendezVous rdv) => _repo.Add(ToDb(rdv));

    public void Update(RendezVous rdv) => _repo.Update(ToDb(rdv));

    public void UpdateStatut(int id, string statut) => _repo.UpdateStatut(id, statut);

    public void Delete(int id) => _repo.Delete(id);

    private static RendezVous Map(RendezVousDb db) => new()
    {
        IdRDV = db.idRDV,
        DateHeure = db.dateHeure,
        Motif = db.motif,
        Statut = db.statut,
        IdPatient = db.idPatient,
        IdMedecin = db.idMedecin
    };

    private static RendezVousDb ToDb(RendezVous r) => new()
    {
        idRDV = r.IdRDV,
        dateHeure = r.DateHeure,
        motif = r.Motif,
        statut = r.Statut,
        idPatient = r.IdPatient,
        idMedecin = r.IdMedecin
    };
}
