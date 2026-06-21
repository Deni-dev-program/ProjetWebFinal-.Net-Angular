namespace Infrastructure.Models;

public class MedicamentDb
{
    public int idMedicament { get; set; }
    public string nomCommercial { get; set; } = string.Empty;
    public string principeActif { get; set; } = string.Empty;
    public string dosage { get; set; } = string.Empty;
}
