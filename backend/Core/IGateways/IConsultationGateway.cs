using Core.Models;

namespace Core.IGateways;

public interface IConsultationGateway
{
    IEnumerable<Consultation> GetAll();
    IEnumerable<Consultation> GetByDossierId(int dossierId);
    Consultation? GetById(int id);
    int Add(Consultation consultation);
    void Update(Consultation consultation);
    void Delete(int id);
}
