using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IMedicamentUseCases
{
    IEnumerable<Medicament> GetAll();
    Medicament GetById(int id);
    void Create(Medicament medicament);
    void Update(Medicament medicament);
    void Delete(int id);
}
