namespace Core.Models;

public class Medicament
{
    public int IdMedicament { get; set; }
    public string NomCommercial { get; set; } = string.Empty;
    public string PrincipeActif { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
}
