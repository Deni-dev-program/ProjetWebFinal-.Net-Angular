namespace Infrastructure.Models;

public class PrescriptionDb
{
    public int idPrescription { get; set; }
    public DateTime datePrescription { get; set; }
    public int idConsultation { get; set; }
}
