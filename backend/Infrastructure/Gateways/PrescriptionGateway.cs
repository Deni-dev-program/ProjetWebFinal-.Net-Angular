using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class PrescriptionGateway : IPrescriptionGateway
{
    private readonly IPrescriptionRepository _repo;

    public PrescriptionGateway(IPrescriptionRepository repo) => _repo = repo;

    public Prescription? GetByConsultationId(int consultationId)
    {
        var db = _repo.GetByConsultationId(consultationId);
        return db is null ? null : Map(db);
    }

    public int Add(Prescription prescription) => _repo.Add(ToDb(prescription));

    public IEnumerable<LignePrescription> GetLignesByPrescriptionId(int prescriptionId) =>
        _repo.GetLignesByPrescriptionId(prescriptionId).Select(MapLigne);

    public void AddLigne(LignePrescription ligne) => _repo.AddLigne(ToDbLigne(ligne));

    private static Prescription Map(PrescriptionDb db) => new()
    {
        IdPrescription = db.idPrescription,
        DatePrescription = db.datePrescription,
        IdConsultation = db.idConsultation
    };

    private static PrescriptionDb ToDb(Prescription p) => new()
    {
        idPrescription = p.IdPrescription,
        datePrescription = p.DatePrescription,
        idConsultation = p.IdConsultation
    };

    private static LignePrescription MapLigne(LignePrescriptionDb db) => new()
    {
        IdLigne = db.idLigne,
        Quantite = db.quantite,
        Posologie = db.posologie,
        IdPrescription = db.idPrescription,
        IdMedicament = db.idMedicament
    };

    private static LignePrescriptionDb ToDbLigne(LignePrescription l) => new()
    {
        idLigne = l.IdLigne,
        quantite = l.Quantite,
        posologie = l.Posologie,
        idPrescription = l.IdPrescription,
        idMedicament = l.IdMedicament
    };
}
