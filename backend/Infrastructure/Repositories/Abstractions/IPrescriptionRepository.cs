using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IPrescriptionRepository
{
    PrescriptionDb? GetByConsultationId(int consultationId);
    int Add(PrescriptionDb prescription);
    IEnumerable<LignePrescriptionDb> GetLignesByPrescriptionId(int prescriptionId);
    void AddLigne(LignePrescriptionDb ligne);
}
