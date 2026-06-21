namespace Core.Models;

public class RendezVous
{
    public int IdRDV { get; set; }
    public DateTime DateHeure { get; set; }
    public string Motif { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public int IdPatient { get; set; }
    public int IdMedecin { get; set; }
}
