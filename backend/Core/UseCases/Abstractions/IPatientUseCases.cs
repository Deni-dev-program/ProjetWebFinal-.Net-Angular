using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IPatientUseCases
{
    IEnumerable<Patient> GetAll();
    Patient GetById(int id);
    void Create(Patient patient);
    void Update(Patient patient);
    void Delete(int id);
}
