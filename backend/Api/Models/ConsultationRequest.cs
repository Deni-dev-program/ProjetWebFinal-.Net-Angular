namespace Api.Models;

public record ConsultationRequest(
    DateTime DateConsultation,
    string Diagnostic,
    decimal Prix,
    int IdDossier,
    int IdMedecin
);
