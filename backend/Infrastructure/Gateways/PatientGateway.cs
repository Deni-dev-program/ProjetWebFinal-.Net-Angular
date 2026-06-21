using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class PatientGateway : IPatientGateway
{
    private readonly IPatientRepository _repo;

    public PatientGateway(IPatientRepository repo) => _repo = repo;

    public IEnumerable<Patient> GetAll() => _repo.GetAll().Select(Map);

    public Patient? GetById(int id)
    {
        var db = _repo.GetById(id);
        return db is null ? null : Map(db);
    }

    public void Add(Patient patient) => _repo.Add(ToDb(patient));

    public void Update(Patient patient) => _repo.Update(ToDb(patient));

    public void Delete(int id) => _repo.Delete(id);

    private static Patient Map(PatientDb db) => new()
    {
        IdPatient = db.idPatient,
        Nom = db.nom,
        Prenom = db.prenom,
        DateNaissance = db.dateNaissance,
        Sexe = db.sexe.Length > 0 ? db.sexe[0] : 'M',
        Telephone = db.telephone,
        Email = db.email
    };

    private static PatientDb ToDb(Patient p) => new()
    {
        idPatient = p.IdPatient,
        nom = p.Nom,
        prenom = p.Prenom,
        dateNaissance = p.DateNaissance,
        sexe = p.Sexe.ToString(),
        telephone = p.Telephone,
        email = p.Email
    };
}
