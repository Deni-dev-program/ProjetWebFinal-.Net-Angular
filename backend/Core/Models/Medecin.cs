namespace Core.Models;

public class Medecin
{
    public int IdMedecin { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty;
    public string EmailPro { get; set; } = string.Empty;
}
