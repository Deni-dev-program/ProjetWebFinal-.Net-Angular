namespace Api.Models;

public record RegisterPatientRequest(
    string Nom,
    string Prenom,
    DateTime DateNaissance,
    char Sexe,
    string Telephone,
    string Email,
    string Password);
