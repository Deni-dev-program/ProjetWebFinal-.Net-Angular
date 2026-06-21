using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IRendezVousUseCases
{
    IEnumerable<RendezVous> GetAll();
    IEnumerable<RendezVous> GetByPatientId(int patientId);
    IEnumerable<RendezVous> GetByMedecinId(int medecinId);
    RendezVous GetById(int id);
    void Create(RendezVous rdv);
    void Update(RendezVous rdv);
    void UpdateStatut(int id, string statut);
    void Delete(int id);
}
