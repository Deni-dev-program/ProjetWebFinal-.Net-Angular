using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IRendezVousRepository
{
    IEnumerable<RendezVousDb> GetAll();
    IEnumerable<RendezVousDb> GetByPatientId(int patientId);
    IEnumerable<RendezVousDb> GetByMedecinId(int medecinId);
    RendezVousDb? GetById(int id);
    void Add(RendezVousDb rdv);
    void Update(RendezVousDb rdv);
    void UpdateStatut(int id, string statut);
    void Delete(int id);
}
