namespace Infrastructure.Models;

public class RendezVousDb
{
    public int idRDV { get; set; }
    public DateTime dateHeure { get; set; }
    public string motif { get; set; } = string.Empty;
    public string statut { get; set; } = string.Empty;
    public int idPatient { get; set; }
    public int idMedecin { get; set; }
}
