using Infrastructure.Models;

namespace Infrastructure.Repositories.Abstractions;

public interface IAuthRepository
{
    UtilisateurDb? GetByEmail(string email);
    void AddUtilisateur(UtilisateurDb utilisateur);
    int AddPatientAndGetId(string nom, string prenom, DateTime dateNaissance, char sexe, string telephone, string email);
}
