namespace Infrastructure.Models;

public class PatientDb
{
    public int idPatient { get; set; }
    public string nom { get; set; } = string.Empty;
    public string prenom { get; set; } = string.Empty;
    public DateTime dateNaissance { get; set; }
    public string sexe { get; set; } = string.Empty;
    public string telephone { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}
