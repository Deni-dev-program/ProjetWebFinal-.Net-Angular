namespace Infrastructure.Models;

public class ConsultationDb
{
    public int idConsultation { get; set; }
    public DateTime dateConsultation { get; set; }
    public string diagnostic { get; set; } = string.Empty;
    public decimal prix { get; set; }
    public int idDossier { get; set; }
    public int idMedecin { get; set; }
}
