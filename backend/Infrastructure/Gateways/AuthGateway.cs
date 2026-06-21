using Core.IGateways;
using Core.Models;
using Infrastructure.Models;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Gateways;

public class AuthGateway : IAuthGateway
{
    private readonly IAuthRepository _authRepo;
    private readonly IPatientRepository _patientRepo;
    private readonly IMedecinRepository _medecinRepo;
    private readonly ISecretaireRepository _secretaireRepo;

    public AuthGateway(
        IAuthRepository authRepo,
        IPatientRepository patientRepo,
        IMedecinRepository medecinRepo,
        ISecretaireRepository secretaireRepo)
    {
        _authRepo = authRepo;
        _patientRepo = patientRepo;
        _medecinRepo = medecinRepo;
        _secretaireRepo = secretaireRepo;
    }

    public LoginResult? VerifyLogin(string email, string password)
    {
        var user = _authRepo.GetByEmail(email);
        if (user is null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.passwordHash)) return null;

        var (nom, prenom) = GetNomPrenom(user.role, user.idRef);

        return new LoginResult
        {
            IdUtilisateur = user.idUtilisateur,
            IdRef = user.idRef,
            Role = user.role,
            Email = user.email,
            Nom = nom,
            Prenom = prenom
        };
    }

    public void RegisterPatient(Patient patient, string password)
    {
        var idPatient = _authRepo.AddPatientAndGetId(
            patient.Nom, patient.Prenom, patient.DateNaissance,
            patient.Sexe, patient.Telephone, patient.Email);

        _authRepo.AddUtilisateur(new UtilisateurDb
        {
            email = patient.Email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password),
            role = "patient",
            idRef = idPatient
        });
    }

    public void CreateCompteUtilisateur(string email, string password, string role, int idRef)
    {
        _authRepo.AddUtilisateur(new UtilisateurDb
        {
            email = email,
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password),
            role = role,
            idRef = idRef
        });
    }

    private (string nom, string prenom) GetNomPrenom(string role, int idRef)
    {
        return role switch
        {
            "patient" => GetFromPatient(idRef),
            "medecin" => GetFromMedecin(idRef),
            "secretaire" => GetFromSecretaire(idRef),
            _ => ("", "")
        };
    }

    private (string, string) GetFromPatient(int id)
    {
        var p = _patientRepo.GetById(id);
        return p is null ? ("", "") : (p.nom, p.prenom);
    }

    private (string, string) GetFromMedecin(int id)
    {
        var m = _medecinRepo.GetById(id);
        return m is null ? ("", "") : (m.nom, "");
    }

    private (string, string) GetFromSecretaire(int id)
    {
        var s = _secretaireRepo.GetById(id);
        return s is null ? ("", "") : (s.nom, s.prenom);
    }
}
