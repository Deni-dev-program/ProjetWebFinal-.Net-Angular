using Core.Models;

namespace Core.IGateways;

public interface IMedicamentGateway
{
    IEnumerable<Medicament> GetAll();
    Medicament? GetById(int id);
    void Add(Medicament medicament);
    void Update(Medicament medicament);
    void Delete(int id);
}
