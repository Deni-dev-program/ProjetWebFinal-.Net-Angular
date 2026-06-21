using Core.Models;

namespace Core.IGateways;

public interface IAuthGateway
{
    LoginResult? VerifyLogin(string email, string password);
    void RegisterPatient(Patient patient, string password);
    void CreateCompteUtilisateur(string email, string password, string role, int idRef);
}
