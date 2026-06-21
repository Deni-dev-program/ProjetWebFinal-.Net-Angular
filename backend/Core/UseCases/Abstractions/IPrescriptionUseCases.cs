using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IPrescriptionUseCases
{
    Prescription? GetByConsultationId(int consultationId);
    int Create(Prescription prescription);
    IEnumerable<LignePrescription> GetLignes(int prescriptionId);
    void AddLigne(LignePrescription ligne);
}
