namespace Infrastructure.Models;

public class LignePrescriptionDb
{
    public int idLigne { get; set; }
    public int quantite { get; set; }
    public string posologie { get; set; } = string.Empty;
    public int idPrescription { get; set; }
    public int idMedicament { get; set; }
}
