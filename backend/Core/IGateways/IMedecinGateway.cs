using Core.Models;

namespace Core.IGateways;

public interface IMedecinGateway
{
    IEnumerable<Medecin> GetAll();
    Medecin? GetById(int id);
    void Add(Medecin medecin);
    void Update(Medecin medecin);
    void Delete(int id);
}
