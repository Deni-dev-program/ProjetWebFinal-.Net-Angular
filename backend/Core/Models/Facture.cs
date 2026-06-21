namespace Core.Models;

public class Facture
{
    public int IdFacture { get; set; }
    public DateTime DateFacture { get; set; }
    public decimal MontantTotal { get; set; }
    public string StatutPaiement { get; set; } = string.Empty;
    public int IdConsultation { get; set; }
}
