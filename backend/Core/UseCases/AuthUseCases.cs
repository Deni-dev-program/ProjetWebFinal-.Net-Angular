using Core.IGateways;
using Core.Models;
using Core.UseCases.Abstractions;

namespace Core.UseCases;

public class AuthUseCases : IAuthUseCases
{
    private readonly IAuthGateway _gateway;
    private readonly IPatientGateway _patientGateway;

    public AuthUseCases(IAuthGateway gateway, IPatientGateway patientGateway)
    {
        _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        _patientGateway = patientGateway ?? throw new ArgumentNullException(nameof(patientGateway));
    }

    public LoginResult Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("L'email est obligatoire.");
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Le mot de passe est obligatoire.");

        var result = _gateway.VerifyLogin(email, password);
        if (result is null) throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
        return result;
    }

    public void RegisterPatient(Patient patient, string password)
    {
        ArgumentNullException.ThrowIfNull(patient);
        if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Le mot de passe est obligatoire.");
        if (string.IsNullOrWhiteSpace(patient.Nom)) throw new ArgumentException("Le nom est obligatoire.");
        if (string.IsNullOrWhiteSpace(patient.Email)) throw new ArgumentException("L'email est obligatoire.");
        _gateway.RegisterPatient(patient, password);
    }
}
