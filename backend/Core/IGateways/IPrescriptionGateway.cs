using Core.Models;

namespace Core.IGateways;

public interface IPrescriptionGateway
{
    Prescription? GetByConsultationId(int consultationId);
    int Add(Prescription prescription);
    IEnumerable<LignePrescription> GetLignesByPrescriptionId(int prescriptionId);
    void AddLigne(LignePrescription ligne);
}
