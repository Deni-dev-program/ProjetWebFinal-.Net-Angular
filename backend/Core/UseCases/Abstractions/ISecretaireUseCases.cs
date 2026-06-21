using Core.Models;

namespace Core.UseCases.Abstractions;

public interface ISecretaireUseCases
{
    IEnumerable<Secretaire> GetAll();
    Secretaire GetById(int id);
    void Create(Secretaire secretaire);
    void Update(Secretaire secretaire);
    void Delete(int id);
}
