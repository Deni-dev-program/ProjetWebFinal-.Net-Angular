using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class PatientUseCases : IPatientUseCases
{
    private readonly IPatientGateway _gateway;

    public PatientUseCases(IPatientGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Patient> GetAll() => _gateway.GetAll();

    public Patient GetById(int id)
    {
        var patient = _gateway.GetById(id);
        if (patient is null) throw new KeyNotFoundException($"Patient {id} introuvable.");
        return patient;
    }

    public void Create(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient);
        if (string.IsNullOrWhiteSpace(patient.Nom)) throw new ArgumentException("Le nom est obligatoire.");
        if (string.IsNullOrWhiteSpace(patient.Prenom)) throw new ArgumentException("Le prénom est obligatoire.");
        _gateway.Add(patient);
    }

    public void Update(Patient patient)
    {
        ArgumentNullException.ThrowIfNull(patient);
        GetById(patient.IdPatient);
        _gateway.Update(patient);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
