using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IDossierMedicalUseCases
{
    DossierMedical GetByPatientId(int patientId);
    void Create(DossierMedical dossier);
    void Update(DossierMedical dossier);
}
