namespace Infrastructure.Models;

public class DossierMedicalDb
{
    public int idDossier { get; set; }
    public string groupeSanguin { get; set; } = string.Empty;
    public string antecedents { get; set; } = string.Empty;
    public string allergies { get; set; } = string.Empty;
    public int idPatient { get; set; }
}
