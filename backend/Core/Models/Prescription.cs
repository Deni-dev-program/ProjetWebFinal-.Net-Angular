namespace Core.Models;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime DatePrescription { get; set; }
    public int IdConsultation { get; set; }
}
