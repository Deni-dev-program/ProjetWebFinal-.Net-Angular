using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IMedicamentRepository
{
    IEnumerable<MedicamentDb> GetAll();
    MedicamentDb? GetById(int id);
    void Add(MedicamentDb medicament);
    void Update(MedicamentDb medicament);
    void Delete(int id);
}
