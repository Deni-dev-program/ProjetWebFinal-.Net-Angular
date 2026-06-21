namespace Api.Models;

public record FactureRequest(
    decimal MontantTotal,
    string StatutPaiement,
    int IdConsultation
);
