using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class DossierMedicalUseCases : IDossierMedicalUseCases
{
    private readonly IDossierMedicalGateway _gateway;

    public DossierMedicalUseCases(IDossierMedicalGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public DossierMedical GetByPatientId(int patientId)
    {
        var dossier = _gateway.GetByPatientId(patientId);
        if (dossier is null) throw new KeyNotFoundException($"Dossier médical du patient {patientId} introuvable.");
        return dossier;
    }

    public void Create(DossierMedical dossier)
    {
        ArgumentNullException.ThrowIfNull(dossier);
        _gateway.Add(dossier);
    }

    public void Update(DossierMedical dossier)
    {
        ArgumentNullException.ThrowIfNull(dossier);
        _gateway.Update(dossier);
    }
}
