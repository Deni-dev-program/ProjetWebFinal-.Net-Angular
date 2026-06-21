namespace Core.Models;

public class Consultation
{
    public int IdConsultation { get; set; }
    public DateTime DateConsultation { get; set; }
    public string Diagnostic { get; set; } = string.Empty;
    public decimal Prix { get; set; }
    public int IdDossier { get; set; }
    public int IdMedecin { get; set; }
}
