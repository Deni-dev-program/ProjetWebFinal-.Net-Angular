namespace Api.Models;

public record PatientRequest(
    string Nom,
    string Prenom,
    DateTime DateNaissance,
    char Sexe,
    string Telephone,
    string Email
);
