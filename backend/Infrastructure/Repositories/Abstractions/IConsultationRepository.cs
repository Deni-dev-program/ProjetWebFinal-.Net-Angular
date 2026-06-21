using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IConsultationRepository
{
    IEnumerable<ConsultationDb> GetAll();
    IEnumerable<ConsultationDb> GetByDossierId(int dossierId);
    ConsultationDb? GetById(int id);
    int Add(ConsultationDb consultation);
    void Update(ConsultationDb consultation);
    void Delete(int id);
}
