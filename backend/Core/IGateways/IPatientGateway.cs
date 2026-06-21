using Core.Models;

namespace Core.IGateways;

public interface IPatientGateway
{
    IEnumerable<Patient> GetAll();
    Patient? GetById(int id);
    void Add(Patient patient);
    void Update(Patient patient);
    void Delete(int id);
}
