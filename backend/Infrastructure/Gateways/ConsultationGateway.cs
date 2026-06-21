using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class ConsultationGateway : IConsultationGateway
{
    private readonly IConsultationRepository _repo;

    public ConsultationGateway(IConsultationRepository repo) => _repo = repo;

    public IEnumerable<Consultation> GetAll() => _repo.GetAll().Select(Map);

    public IEnumerable<Consultation> GetByDossierId(int dossierId) =>
        _repo.GetByDossierId(dossierId).Select(Map);

    public Consultation? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public int Add(Consultation consultation) => _repo.Add(ToDb(consultation));

    public void Update(Consultation consultation) => _repo.Update(ToDb(consultation));

    public void Delete(int id) => _repo.Delete(id);

    private static Consultation Map(ConsultationDb db) => new()
    {
        IdConsultation = db.idConsultation,
        DateConsultation = db.dateConsultation,
        Diagnostic = db.diagnostic,
        Prix = db.prix,
        IdDossier = db.idDossier,
        IdMedecin = db.idMedecin
    };

    private static ConsultationDb ToDb(Consultation c) => new()
    {
        idConsultation = c.IdConsultation,
        dateConsultation = c.DateConsultation,
        diagnostic = c.Diagnostic,
        prix = c.Prix,
        idDossier = c.IdDossier,
        idMedecin = c.IdMedecin
    };
}
