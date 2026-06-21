using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class MedicamentUseCases : IMedicamentUseCases
{
    private readonly IMedicamentGateway _gateway;

    public MedicamentUseCases(IMedicamentGateway gateway) =>
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));

    public IEnumerable<Medicament> GetAll() => _gateway.GetAll();

    public Medicament GetById(int id)
    {
        var m = _gateway.GetById(id);
        if (m is null) throw new KeyNotFoundException($"Médicament {id} introuvable.");
        return m;
    }

    public void Create(Medicament medicament)
    {
        ArgumentNullException.ThrowIfNull(medicament);
        if (string.IsNullOrWhiteSpace(medicament.NomCommercial)) throw new ArgumentException("Le nom commercial est obligatoire.");
        _gateway.Add(medicament);
    }

    public void Update(Medicament medicament)
    {
        ArgumentNullException.ThrowIfNull(medicament);
        GetById(medicament.IdMedicament);
        _gateway.Update(medicament);
    }

    public void Delete(int id)
    {
        GetById(id);
        _gateway.Delete(id);
    }
}
