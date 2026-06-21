using Core.Models;

namespace Core.IGateways;

public interface IRendezVousGateway
{
    IEnumerable<RendezVous> GetAll();
    IEnumerable<RendezVous> GetByPatientId(int patientId);
    IEnumerable<RendezVous> GetByMedecinId(int medecinId);
    RendezVous? GetById(int id);
    void Add(RendezVous rdv);
    void Update(RendezVous rdv);
    void UpdateStatut(int id, string statut);
    void Delete(int id);
}
