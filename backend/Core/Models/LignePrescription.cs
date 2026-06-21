namespace Core.Models;

public class LignePrescription
{
    public int IdLigne { get; set; }
    public int Quantite { get; set; }
    public string Posologie { get; set; } = string.Empty;
    public int IdPrescription { get; set; }
    public int IdMedicament { get; set; }
}
