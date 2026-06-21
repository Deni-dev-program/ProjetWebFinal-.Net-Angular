namespace Core.Models;

public class Patient
{
    public int IdPatient { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public DateTime DateNaissance { get; set; }
    public char Sexe { get; set; }
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
