using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IPatientRepository
{
    IEnumerable<PatientDb> GetAll();
    PatientDb? GetById(int id);
    void Add(PatientDb patient);
    void Update(PatientDb patient);
    void Delete(int id);
}
