using Api.Models;
using Core.Models;
using Core.UseCases.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.EndPoints;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", (LoginRequest req, IAuthUseCases useCases, IConfiguration config) =>
        {
            try
            {
                var result = useCases.Login(req.Email, req.Password);
                var token = GenerateJwt(result, config);
                return Results.Ok(new
                {
                    token,
                    role = result.Role,
                    idRef = result.IdRef,
                    nom = result.Nom,
                    prenom = result.Prenom,
                    email = result.Email
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
        });

        group.MapPost("/register", (RegisterPatientRequest req, IAuthUseCases useCases) =>
        {
            var patient = new Patient
            {
                Nom = req.Nom,
                Prenom = req.Prenom,
                DateNaissance = req.DateNaissance,
                Sexe = req.Sexe,
                Telephone = req.Telephone,
                Email = req.Email
            };
            useCases.RegisterPatient(patient, req.Password);
            return Results.Created();
        });
    }

    private static string GenerateJwt(LoginResult result, IConfiguration config)
    {
        var claims = new[]
        {
            new Claim("sub", result.IdUtilisateur.ToString()),
            new Claim("email", result.Email),
            new Claim("role", result.Role),
            new Claim("idRef", result.IdRef.ToString()),
            new Claim("nom", result.Nom),
            new Claim("prenom", result.Prenom)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
