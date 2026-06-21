using Core.Models;

namespace Core.IGateways;

public interface IDossierMedicalGateway
{
    DossierMedical? GetByPatientId(int patientId);
    void Add(DossierMedical dossier);
    void Update(DossierMedical dossier);
}
