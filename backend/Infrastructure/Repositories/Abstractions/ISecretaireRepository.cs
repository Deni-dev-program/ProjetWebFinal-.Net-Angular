using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface ISecretaireRepository
{
    IEnumerable<SecretaireDb> GetAll();
    SecretaireDb? GetById(int id);
    int Add(SecretaireDb secretaire);
    void Update(SecretaireDb secretaire);
    void Delete(int id);
}
