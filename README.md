# Clinique Médicale — Projet Angular & .NET

Application de gestion de clinique médicale développée avec Angular (frontend) et ASP.NET Core (backend), suivant la Clean Architecture.

---

## Prérequis

| Outil | Version utilisée |
|-------|-----------------|
| .NET SDK | 9.0 |
| Node.js | 24.x |
| Angular CLI | 21.x |
| MySQL / MariaDB | 8.x / 10.x |

---

## Installation

### 1. Créer la base de données

Ouvrir un client MySQL (MySQL Workbench, DBeaver, HeidiSQL ou CLI) et exécuter le script :

```bash
mysql -u root -p < backend/Infrastructure/db.sql
```

Ou copier-coller le contenu du fichier `backend/Infrastructure/db.sql` dans votre client SQL.

La base de données `clinique_db` sera créée automatiquement avec les tables et données de démonstration.

### 2. Configurer la chaîne de connexion

Ouvrir le fichier `backend/Api/appsettings.json` et adapter la chaîne de connexion :

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=clinique_db;Uid=root;Pwd=VOTRE_MOT_DE_PASSE;"
}
```

Si MySQL tourne sans mot de passe (XAMPP par défaut), laisser `Pwd=;`.

### 3. Lancer le Backend

```bash
cd backend
dotnet run --project Api
```

L'API démarre sur `http://localhost:5096`.  
La documentation Scalar est disponible sur `http://localhost:5096/scalar/v1`.

### 4. Lancer le Frontend

```bash
cd frontend
npm install
npm start
```

L'application Angular démarre sur `http://localhost:4200`.

---

## Comptes de test

L'application utilise une authentification JWT. Les comptes sont créés automatiquement au premier démarrage du backend.

| Rôle | Email | Mot de passe | Accès |
|------|-------|-------------|-------|
| Secrétaire | `secretaire@clinique.be` | `admin1234` | Dashboard, Patients, Médecins, Rendez-vous, Consultations, Médicaments |
| Patient | `alice.dupont@mail.be` | `patient1234` | Portail patient (ses rendez-vous, son profil) |
| Patient | `marc.leroy@mail.be` | `patient1234` | Portail patient |
| Patient | `sophie.bernard@mail.be` | `patient1234` | Portail patient |
| Médecin | `martin@clinique.be` | `medecin1234` | Portail médecin (ses rendez-vous du jour) |
| Médecin | `dubois@clinique.be` | `medecin1234` | Portail médecin |
| Médecin | `lambert@clinique.be` | `medecin1234` | Portail médecin |

Un patient peut également créer son propre compte via la page de connexion ("Créer un compte").

---

## Structure du projet

```
ProjetFinalDevWeb/
├── backend/
│   ├── Api/              ← Endpoints, Middleware, Models (DTOs), Program.cs
│   ├── Core/             ← Models domaine, IGateways, UseCases
│   ├── Infrastructure/   ← Repositories (Dapper), Gateways, Utils, db.sql
│   └── Api.sln
└── frontend/
    └── src/
        └── app/
            ├── core/
            │   ├── models/    ← Interfaces TypeScript
            │   └── services/  ← Services Angular (HttpClient)
            └── features/
                ├── auth/
                ├── dashboard/
                ├── patients/
                ├── medecins/
                ├── rendezvous/
                ├── consultations/
                ├── medicaments/
                ├── patient-portal/
                └── medecin-portal/
```

## Fonctionnalités

- **Authentification** : Login / inscription avec JWT, rôles (secrétaire, médecin, patient)
- **Patients** : CRUD complet + consultation du dossier médical
- **Médecins** : CRUD complet
- **Rendez-vous** : Création, mise à jour du statut (planifié/confirmé/annulé/terminé), suppression
- **Consultations** : Création et suppression
- **Médicaments** : CRUD complet avec recherche en temps réel
- **Tableau de bord** : Statistiques globales (accessible secrétaire uniquement)
- **Portail patient** : Gestion de ses rendez-vous et son profil
- **Portail médecin** : Visualisation et gestion des statuts de ses rendez-vous

## Technologies

**Backend**
- ASP.NET Core 9 — Minimal API
- Clean Architecture (API / Core / Infrastructure)
- Dapper — accès base de données
- MySqlConnector — connecteur MySQL
- JWT Bearer — authentification
- Scalar — documentation API

**Frontend**
- Angular 21 — module-based (NgModule)
- Nouvelle syntaxe (@if, @for, @switch)
- Services Angular (gestion d'état)
- Reactive Forms
- Angular Router avec guards (AuthGuard, rôles)
- HTTP Interceptor (injection automatique du token JWT)
