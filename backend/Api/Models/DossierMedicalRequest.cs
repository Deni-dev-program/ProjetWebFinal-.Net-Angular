namespace Api.Models;

public record DossierMedicalRequest(
    string GroupeSanguin,
    string Antecedents,
    string Allergies,
    int IdPatient
);
