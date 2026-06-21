using Core.Models;

namespace Core.UseCases.Abstractions;

public interface IAuthUseCases
{
    LoginResult Login(string email, string password);
    void RegisterPatient(Patient patient, string password);
}
