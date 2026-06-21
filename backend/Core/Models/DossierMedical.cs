namespace Core.Models;

public class DossierMedical
{
    public int IdDossier { get; set; }
    public string GroupeSanguin { get; set; } = string.Empty;
    public string Antecedents { get; set; } = string.Empty;
    public string Allergies { get; set; } = string.Empty;
    public int IdPatient { get; set; }
}
