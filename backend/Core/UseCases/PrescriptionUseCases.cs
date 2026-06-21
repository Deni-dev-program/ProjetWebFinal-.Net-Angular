using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class PrescriptionUseCases : IPrescriptionUseCases
{
    private readonly IPrescriptionGateway _gateway;

    public PrescriptionUseCases(IPrescriptionGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public Prescription? GetByConsultationId(int consultationId) =>
        _gateway.GetByConsultationId(consultationId);

    public int Create(Prescription prescription)
    {
        ArgumentNullException.ThrowIfNull(prescription);
        return _gateway.Add(prescription);
    }

    public IEnumerable<LignePrescription> GetLignes(int prescriptionId) =>
        _gateway.GetLignesByPrescriptionId(prescriptionId);

    public void AddLigne(LignePrescription ligne)
    {
        ArgumentNullException.ThrowIfNull(ligne);
        if (ligne.Quantite <= 0) throw new ArgumentException("La quantité doit être positive.");
        _gateway.AddLigne(ligne);
    }
}
