namespace Api.Models;

public record RendezVousRequest(
    DateTime DateHeure,
    string Motif,
    int IdPatient,
    int IdMedecin
);

public record UpdateStatutRequest(string Statut);
