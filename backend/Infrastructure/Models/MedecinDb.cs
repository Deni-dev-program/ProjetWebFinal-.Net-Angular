namespace Infrastructure.Models;

public class MedecinDb
{
    public int idMedecin { get; set; }
    public string nom { get; set; } = string.Empty;
    public string specialite { get; set; } = string.Empty;
    public string emailPro { get; set; } = string.Empty;
}
