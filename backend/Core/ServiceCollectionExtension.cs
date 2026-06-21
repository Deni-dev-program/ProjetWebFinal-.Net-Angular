using Core.UseCases;
using Core.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddTransient<IPatientUseCases, PatientUseCases>();
        services.AddTransient<IMedecinUseCases, MedecinUseCases>();
        services.AddTransient<IDossierMedicalUseCases, DossierMedicalUseCases>();
        services.AddTransient<IConsultationUseCases, ConsultationUseCases>();
        services.AddTransient<IFactureUseCases, FactureUseCases>();
        services.AddTransient<IMedicamentUseCases, MedicamentUseCases>();
        services.AddTransient<IPrescriptionUseCases, PrescriptionUseCases>();
        services.AddTransient<IRendezVousUseCases, RendezVousUseCases>();
        services.AddTransient<ISecretaireUseCases, SecretaireUseCases>();
        services.AddTransient<IAuthUseCases, AuthUseCases>();
        return services;
    }
}
