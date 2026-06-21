using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IDossierMedicalRepository
{
    DossierMedicalDb? GetByPatientId(int patientId);
    void Add(DossierMedicalDb dossier);
    void Update(DossierMedicalDb dossier);
}
