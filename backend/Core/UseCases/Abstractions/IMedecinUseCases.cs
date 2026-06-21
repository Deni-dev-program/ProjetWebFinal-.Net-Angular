using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IMedecinUseCases
{
    IEnumerable<Medecin> GetAll();
    Medecin GetById(int id);
    void Create(Medecin medecin);
    void Update(Medecin medecin);
    void Delete(int id);
}
