using Core.IGateways;
using Infrastructure.Gateways;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<DbConnectionFactory>();
        services.AddTransient<DataSeeder>();

        services.AddTransient<IPatientRepository, PatientRepository>();
        services.AddTransient<IMedecinRepository, MedecinRepository>();
        services.AddTransient<IDossierMedicalRepository, DossierMedicalRepository>();
        services.AddTransient<IConsultationRepository, ConsultationRepository>();
        services.AddTransient<IFactureRepository, FactureRepository>();
        services.AddTransient<IMedicamentRepository, MedicamentRepository>();
        services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();
        services.AddTransient<IRendezVousRepository, RendezVousRepository>();
        services.AddTransient<ISecretaireRepository, SecretaireRepository>();
        services.AddTransient<IAuthRepository, AuthRepository>();

        services.AddTransient<IPatientGateway, PatientGateway>();
        services.AddTransient<IMedecinGateway, MedecinGateway>();
        services.AddTransient<IDossierMedicalGateway, DossierMedicalGateway>();
        services.AddTransient<IConsultationGateway, ConsultationGateway>();
        services.AddTransient<IFactureGateway, FactureGateway>();
        services.AddTransient<IMedicamentGateway, MedicamentGateway>();
        services.AddTransient<IPrescriptionGateway, PrescriptionGateway>();
        services.AddTransient<IRendezVousGateway, RendezVousGateway>();
        services.AddTransient<ISecretaireGateway, SecretaireGateway>();
        services.AddTransient<IAuthGateway, AuthGateway>();

        return services;
    }
}
