namespace Infrastructure.Models;

public class UtilisateurDb
{
    public int idUtilisateur { get; set; }
    public string email { get; set; } = string.Empty;
    public string passwordHash { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public int idRef { get; set; }
}
