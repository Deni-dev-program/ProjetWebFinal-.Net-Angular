namespace Infrastructure.Models;

public class SecretaireDb
{
    public int idSecretaire { get; set; }
    public string nom { get; set; } = string.Empty;
    public string prenom { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}
