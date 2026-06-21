using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IConsultationUseCases
{
    IEnumerable<Consultation> GetAll();
    IEnumerable<Consultation> GetByDossierId(int dossierId);
    Consultation GetById(int id);
    int Create(Consultation consultation);
    void Update(Consultation consultation);
    void Delete(int id);
}
