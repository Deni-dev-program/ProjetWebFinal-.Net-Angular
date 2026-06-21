using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class ConsultationUseCases : IConsultationUseCases
{
    private readonly IConsultationGateway _gateway;

    public ConsultationUseCases(IConsultationGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Consultation> GetAll() => _gateway.GetAll();

    public IEnumerable<Consultation> GetByDossierId(int dossierId) => _gateway.GetByDossierId(dossierId);

    public Consultation GetById(int id)
    {
        var c = _gateway.GetById(id);
        if (c is null) throw new KeyNotFoundException($"Consultation {id} introuvable.");
        return c;
    }

    public int Create(Consultation consultation)
    {
        ArgumentNullException.ThrowIfNull(consultation);
        return _gateway.Add(consultation);
    }

    public void Update(Consultation consultation)
    {
        ArgumentNullException.ThrowIfNull(consultation);
        GetById(consultation.IdConsultation);
        _gateway.Update(consultation);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
