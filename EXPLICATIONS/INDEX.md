# Index des explications

## Point d'entrée
- [../README.md](../README.md) — Guide d'installation, comptes de test, structure du projet, fonctionnalités complètes

## Vue globale
- [00_ARCHITECTURE.md](00_ARCHITECTURE.md) — Architecture Clean Architecture complète, flux d'une requête, schéma des couches

## Couche API (.NET)
- [01_Program.md](01_Program.md) — Point d'entrée, configuration DI, CORS, pipeline middlewares
- [02_Middleware.md](02_Middleware.md) — Gestion globale des exceptions → réponses JSON propres
- [03_Api_EndPoints.md](03_Api_EndPoints.md) — Routes HTTP (Minimal APIs), codes de retour, injection DI
- [04_Api_Models_DTOs.md](04_Api_Models_DTOs.md) — DTOs (records), désérialisation JSON automatique

## Couche Core (logique métier)
- [05_Core_Models.md](05_Core_Models.md) — Entités du domaine (Patient, Medecin, etc.)
- [06_Core_Interfaces.md](06_Core_Interfaces.md) — IGateway + IUseCases : contrats, inversion de dépendances
- [07_Core_UseCases.md](07_Core_UseCases.md) — Validation, règles métier, coordination (le "cerveau")
- [08_Core_ServiceCollection.md](08_Core_ServiceCollection.md) — Enregistrement DI des UseCases (AddTransient)

## Couche Infrastructure (accès données)
- [09_Infrastructure_DbConnectionFactory.md](09_Infrastructure_DbConnectionFactory.md) — Connexion MySQL, chaîne de connexion, pattern Singleton
- [10_Infrastructure_Models_Db.md](10_Infrastructure_Models_Db.md) — Modèles miroirs des tables SQL, mapping Dapper
- [11_Infrastructure_Repositories.md](11_Infrastructure_Repositories.md) — Requêtes SQL avec Dapper, paramètres nommés, sécurité
- [12_Infrastructure_Gateways.md](12_Infrastructure_Gateways.md) — Pont Core/Infrastructure, conversion Patient ↔ PatientDb
- [13_Infrastructure_ServiceCollection.md](13_Infrastructure_ServiceCollection.md) — Enregistrement DI des Repositories et Gateways

## Frontend Angular
- [14_Frontend_Models.md](14_Frontend_Models.md) — Interfaces TypeScript, correspondance avec le C#
- [15_Frontend_Services.md](15_Frontend_Services.md) — HttpClient, Observables, appels REST
- [16_Frontend_Components.md](16_Frontend_Components.md) — Reactive Forms, CRUD, lifecycle hooks, état local
- [17_Frontend_AppModule_Routing.md](17_Frontend_AppModule_Routing.md) — NgModule, déclarations, imports, routes Angular
