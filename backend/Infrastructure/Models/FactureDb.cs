namespace Infrastructure.Models;

public class FactureDb
{
    public int idFacture { get; set; }
    public DateTime dateFacture { get; set; }
    public decimal montantTotal { get; set; }
    public string statutPaiement { get; set; } = string.Empty;
    public int idConsultation { get; set; }
}
