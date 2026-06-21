namespace Api.Models;

public record PrescriptionRequest(int IdConsultation);

public record LignePrescriptionRequest(
    int Quantite,
    string Posologie,
    int IdPrescription,
    int IdMedicament
);
