using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IMedecinRepository
{
    IEnumerable<MedecinDb> GetAll();
    MedecinDb? GetById(int id);
    void Add(MedecinDb medecin);
    void Update(MedecinDb medecin);
    void Delete(int id);
}
