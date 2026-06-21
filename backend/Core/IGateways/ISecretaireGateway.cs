using Core.Models;

namespace Core.IGateways;

public interface ISecretaireGateway
{
    IEnumerable<Secretaire> GetAll();
    Secretaire? GetById(int id);
    void Add(Secretaire secretaire);
    void Update(Secretaire secretaire);
    void Delete(int id);
}
