namespace Core.Models;

public class LoginResult
{
    public int IdUtilisateur { get; set; }
    public int IdRef { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
}
