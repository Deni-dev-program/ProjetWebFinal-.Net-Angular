# Architecture générale du projet

## Vue d'ensemble

Ce projet suit la **Clean Architecture** (architecture propre), divisée en 3 grandes couches backend + 1 frontend :

```
┌──────────────────────────────────────────────────────────────┐
│  FRONTEND (Angular)                                          │
│  Components → Services → HTTP → API REST                     │
└───────────────────────────┬──────────────────────────────────┘
                            │ HTTP/JSON
┌───────────────────────────▼──────────────────────────────────┐
│  COUCHE API (.NET 9)                                         │
│  Program.cs + EndPoints + Middleware + DTOs (Models)         │
└───────────────────────────┬──────────────────────────────────┘
                            │ appelle les interfaces
┌───────────────────────────▼──────────────────────────────────┐
│  COUCHE CORE (logique métier)                                │
│  Models + IGateways + UseCases + Abstractions                │
└───────────────────────────┬──────────────────────────────────┘
                            │ implémente les interfaces
┌───────────────────────────▼──────────────────────────────────┐
│  COUCHE INFRASTRUCTURE (accès données)                       │
│  Gateways + Repositories + Models Db + DbConnectionFactory   │
│                    │                                         │
│                    ▼ Dapper + SQL                            │
│              Base de données MySQL (clinique_db)             │
└──────────────────────────────────────────────────────────────┘
```

## Principe fondamental : la dépendance est toujours vers l'intérieur

- **Core** ne connaît personne (aucun import d'Infrastructure ou d'Api)
- **Infrastructure** connaît Core (pour implémenter ses interfaces)
- **Api** connaît Core et Infrastructure (pour les enregistrer dans le DI)
- **Frontend** ne connaît que l'URL de l'API

## Flux d'une requête HTTP (exemple : GET /api/patients)

```
Angular Component
    → PatientService.getAll()
        → GET http://localhost:5000/api/patients
            → PatientRoutes (EndPoint)
                → IPatientUseCases.GetAll()
                    → PatientUseCases.GetAll()
                        → IPatientGateway.GetAll()
                            → PatientGateway.GetAll()
                                → IPatientRepository.GetAll()
                                    → PatientRepository.GetAll()
                                        → Dapper: SELECT * FROM patient
                                            → MySQL (clinique_db)
                                        ← List<PatientDb>
                                    ← List<PatientDb>
                                ← PatientGateway.Map() → List<Patient>
                            ← List<Patient>
                        ← List<Patient>
                    ← List<Patient>
                ← Results.Ok(List<Patient>)
            ← JSON 200 OK
        ← Observable<Patient[]>
    ← affiche la liste
```

## Pourquoi cette architecture ?

| Avantage | Explication |
|----------|-------------|
| **Testabilité** | On peut tester PatientUseCases sans base de données (injection de faux gateway) |
| **Séparation des responsabilités** | Chaque couche a un rôle précis |
| **Évolutivité** | On peut changer MySQL pour PostgreSQL sans toucher Core ou Api |
| **Lisibilité** | Un développeur qui rejoint le projet sait où chercher quoi |

## Technologies utilisées

| Couche | Technologie |
|--------|-------------|
| Frontend | Angular 21, TypeScript, HttpClient, ReactiveFormsModule |
| Api | ASP.NET Core 9 Minimal APIs |
| Core | C# 13, interfaces, logique métier |
| Infrastructure | Dapper (micro-ORM), MySqlConnector |
| Base de données | MySQL, base `clinique_db` |

---

# Flux détaillés du projet (explication pour l'examen)

## 1. Flux de connexion (Login + JWT)

Le login est le premier flux à comprendre car il conditionne tous les autres. L'utilisateur saisit son email et son mot de passe dans le composant `LoginComponent`. Angular appelle `AuthService.login()` qui envoie une requête `POST /api/auth/login` avec un JSON `{ email, password }`.

Côté backend, `AuthRoutes` reçoit la requête, désérialise le body en `LoginRequest` (DTO record C#), et délègue à `IAuthUseCases.Login()`. Le Use Case vérifie que l'utilisateur existe en base via le Gateway, compare le mot de passe hashé (bcrypt), et retourne un objet `LoginResult` avec le rôle, le nom, l'email et l'id de référence (idRef = id du patient ou du médecin selon le rôle).

L'endpoint génère ensuite un **token JWT** signé avec une clé secrète (configurée dans `appsettings.json`). Ce token contient les **claims** : `sub` (id utilisateur), `email`, `role`, `idRef`, `nom`, `prenom`. Il expire après 24 heures.

L'API répond avec `200 OK` et un JSON contenant le token. Angular le stocke dans le `localStorage` via `AuthService`, qui conserve aussi le rôle et l'idRef en mémoire. À partir de là, **chaque requête suivante inclut automatiquement ce token** grâce à l'intercepteur HTTP.

```
LoginComponent (saisie email + password)
    → AuthService.login(email, password)
        → POST /api/auth/login  { email, password }
            → AuthRoutes → IAuthUseCases.Login()
                → vérifie email en base (via Gateway → Repository → Dapper)
                → compare mot de passe hashé
                → retourne LoginResult (role, idRef, nom, prenom, email)
            → génère token JWT (claims : sub, email, role, idRef, nom, prenom)
            ← 200 OK  { token, role, idRef, nom, prenom, email }
        ← Observable<LoginResponse>
    ← AuthService stocke token + infos dans localStorage
    → Angular Router redirige vers /dashboard (secrétaire) ou /patient-portal (patient) ou /medecin-portal (médecin)
```

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/features/auth/login/login.component.ts` → méthode `login()` ligne 41 : l'utilisateur soumet le formulaire, on récupère `email` + `password` et on appelle `this.authService.login(email, password).subscribe(...)`
2. `frontend/src/app/core/services/auth.service.ts` → méthode `login()` ligne 14 : on fait `http.post('/api/auth/login', { email, password })` et on stocke le token dans le `localStorage` avec `tap(res => localStorage.setItem(this.TOKEN_KEY, res.token))`
3. `backend/Api/Models/LoginRequest.cs` → le DTO `record LoginRequest(Email, Password)` : ASP.NET Core désérialise automatiquement le JSON du body dans ce record C#
4. `backend/Api/EndPoints/AuthRoutes.cs` → `MapPost("/login", ...)` ligne 17 : reçoit la requête, appelle `useCases.Login()` pour vérifier les credentials, puis `GenerateJwt()` pour construire le token
5. `backend/Core/UseCases/AuthUseCases.cs` → méthode `Login()` : vérifie que l'email existe en base, compare le mot de passe bcrypt, retourne `LoginResult` avec tous les infos du compte
6. `backend/Infrastructure/Gateways/AuthGateway.cs` → implémentation concrète : exécute les requêtes SQL via Dapper pour lire les tables `utilisateur` et `patient`/`medecin`/`secretaire`
7. `backend/Api/EndPoints/AuthRoutes.cs` → méthode privée `GenerateJwt()` ligne 55 : construit les claims (`sub`, `email`, `role`, `idRef`, `nom`, `prenom`) et signe le JWT avec la clé secrète de `appsettings.json`

---

## 2. Le mécanisme JWT : comment le token est transmis automatiquement

Après le login, chaque appel HTTP doit prouver l'identité de l'utilisateur. C'est le rôle de l'**intercepteur HTTP** (`AuthInterceptor`). Angular intercepte **toutes** les requêtes sortantes et ajoute automatiquement l'en-tête :

```
Authorization: Bearer <token JWT>
```

Côté backend, le middleware d'authentification ASP.NET Core lit cet en-tête, vérifie la signature du token, et injecte les claims dans le contexte de la requête. L'endpoint peut alors vérifier le rôle avec `.RequireAuthorization()`.

Ce mécanisme est **stateless** : le serveur ne garde aucune session en mémoire. Il se fie uniquement à ce que contient le token, ce qui est une caractéristique fondamentale des API REST.

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/core/interceptors/auth.interceptor.ts` → méthode `intercept()` ligne 10 : lit le token via `authService.getToken()`, clone la requête et ajoute le header `Authorization: Bearer <token>` — ce code s'exécute automatiquement avant **chaque** requête HTTP
2. `frontend/src/app/core/services/auth.service.ts` → méthode `getToken()` ligne 28 : lit `clinique_token` dans le `localStorage`; méthode `isLoggedIn()` ligne 32 : décode le JWT côté Angular (sans appel serveur) et vérifie `payload.exp * 1000 > Date.now()`
3. `backend/Api/Program.cs` → ligne 16 : `builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)` avec `TokenValidationParameters` — le backend vérifie la signature, l'émetteur, l'audience et la durée de vie du token à chaque requête

---

## 3. Flux de lecture — GET (liste et détail)

**Lire la liste complète des patients (GET /api/patients) :**

L'utilisateur navigue vers la page "Patients". Angular instancie `PatientsComponent`, dont le cycle de vie (`ngOnInit`) déclenche `patientSvc.getAll()`. Celui-ci envoie `GET /api/patients` avec le token JWT dans le header (ajouté par l'intercepteur).

Côté backend, `PatientRoutes` reçoit la requête et appelle `IPatientUseCases.GetAll()`. La couche **Core** (Use Cases) applique les règles métier si nécessaire, puis appelle `IPatientGateway.GetAll()`. La couche **Infrastructure** (Gateway) délègue à `IPatientRepository.GetAll()` qui exécute via Dapper :

```sql
SELECT * FROM patient
```

Dapper mappe chaque ligne SQL en objet `PatientDb` (modèle infrastructure). Le Gateway convertit ensuite chaque `PatientDb` en `Patient` (modèle Core), qui remonte jusqu'à l'endpoint. L'API sérialise la liste en JSON et répond `200 OK`.

Angular reçoit le tableau JSON, le désérialise en `Patient[]` TypeScript, et le composant l'affiche avec `@for`.

```
PatientsComponent (ngOnInit)
    → patientSvc.getAll()
        → GET /api/patients  [Authorization: Bearer <token>]
            → PatientRoutes → IPatientUseCases.GetAll()
                → IPatientGateway.GetAll()
                    → IPatientRepository.GetAll()
                        → Dapper: SELECT * FROM patient
                        ← List<PatientDb>
                    ← PatientGateway.Map() → List<Patient>
                ← List<Patient>
            ← Results.Ok(List<Patient>)
            ← 200 OK  [{ idPatient, nom, prenom, ... }, ...]
        ← Observable<Patient[]>
    ← affiche la liste avec @for
```

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/features/patients/patients.component.ts` → `ngOnInit()` ligne 40 appelle `load()` ligne 44 qui fait `patientSvc.getAll().subscribe(p => { this.patients = p; ... })` — c'est le point de départ Angular
2. `frontend/src/app/core/services/patient.service.ts` → `getAll()` ligne 13 : `return this.http.get<Patient[]>(this.url)` — une seule ligne qui lance la requête HTTP GET
3. `frontend/src/app/core/interceptors/auth.interceptor.ts` → `intercept()` ligne 10 : s'exécute automatiquement, ajoute le header JWT avant que la requête parte vers le backend
4. `backend/Api/EndPoints/PatientRoutes.cs` → `MapGet("/", ...)` ligne 13 : une ligne `Results.Ok(useCases.GetAll())` — reçoit la requête et délègue immédiatement
5. `backend/Core/UseCases/PatientUseCases.cs` → `GetAll()` ligne 14 : `return _gateway.GetAll()` — couche de pass-through, c'est ici qu'on ajouterait des règles métier si nécessaire
6. `backend/Infrastructure/Gateways/PatientGateway.cs` → `GetAll()` ligne 14 : `_repo.GetAll().Select(Map)` — c'est ici que `PatientDb` (modèle BDD) est converti en `Patient` (modèle Core) via `Map()`
7. `backend/Infrastructure/Repositories/PatientRepository.cs` → `GetAll()` ligne 14 : `conn.Query<PatientDb>("SELECT * FROM patient")` avec Dapper — la seule ligne qui touche vraiment la base de données

**Lire un seul patient (GET /api/patients/3) :**

Même flux mais avec l'id dans l'URL. Dapper exécute `SELECT * FROM patient WHERE idPatient = @id`. Si l'id n'existe pas, le Use Case lève une exception que le **middleware global** intercepte et transforme en `404 Not Found`.

---

## 4. Flux de création — POST

L'utilisateur remplit le formulaire (Reactive Form Angular) et clique "Enregistrer". Angular valide le formulaire côté client (champs requis, formats). Si valide, `patientSvc.create(formData)` envoie `POST /api/patients` avec le JSON du patient dans le body.

L'endpoint désérialise le body en `PatientRequest` (DTO C#), construit un objet `Patient` Core (sans id, car la base l'auto-génère), et appelle `IPatientUseCases.Create()`. Le Use Case peut appliquer des règles métier (ex. : vérifier que le nom n'est pas vide), puis appelle le Gateway qui appelle le Repository. Dapper exécute :

```sql
INSERT INTO patient (nom, prenom, dateNaissance, sexe, telephone, email)
VALUES (@nom, @prenom, @dateNaissance, @sexe, @telephone, @email)
```

L'API répond `201 Created` (sans body). Angular reçoit le succès, ferme le formulaire, et recharge la liste avec un nouveau `GET`.

```
PatientsComponent (save())
    → patientSvc.create({ nom, prenom, ... })
        → POST /api/patients  body: { nom, prenom, dateNaissance, sexe, telephone, email }
            → PatientRoutes → construit Patient Core → IPatientUseCases.Create()
                → règles métier (nom et prénom obligatoires)
                → IPatientGateway.Add()
                    → IPatientRepository.Add()
                        → Dapper: INSERT INTO patient ...
            ← Results.Created()
            ← 201 Created
        ← Observable<void>
    ← ferme le formulaire + recharge la liste (load())
```

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/features/patients/patients.component.ts` → méthode `save()` ligne 69 : si `editingId === null`, appelle `patientSvc.create(data).subscribe({ next: () => { this.showForm = false; this.load(); } })` — après succès, ferme le formulaire et recharge
2. `frontend/src/app/core/services/patient.service.ts` → `create()` ligne 21 : `return this.http.post<void>(this.url, patient)` — envoie le body JSON en POST
3. `backend/Api/Models/PatientRequest.cs` → `record PatientRequest(Nom, Prenom, DateNaissance, Sexe, Telephone, Email)` : ASP.NET Core désérialise automatiquement le JSON du body dans ce record C# — pas besoin d'écrire de code de désérialisation
4. `backend/Api/EndPoints/PatientRoutes.cs` → `MapPost("/", ...)` ligne 19 : reçoit le DTO, construit un objet `Patient` Core à partir des champs du DTO, et appelle `useCases.Create(patient)`
5. `backend/Core/UseCases/PatientUseCases.cs` → `Create()` ligne 23 : valide que `Nom` et `Prenom` ne sont pas vides (`ArgumentException` sinon), puis `_gateway.Add(patient)` — c'est ici que vit la logique métier
6. `backend/Infrastructure/Gateways/PatientGateway.cs` → `Add()` ligne 22 : `_repo.Add(ToDb(patient))` — convertit `Patient` (Core) → `PatientDb` (Infrastructure) avant d'écrire en base
7. `backend/Infrastructure/Repositories/PatientRepository.cs` → `Add()` ligne 27 : exécute `conn.Execute("INSERT INTO patient (...) VALUES (...)", patient)` avec Dapper

---

## 5. Flux de modification complète — PUT

L'utilisateur clique "Modifier" sur un patient, le formulaire se pré-remplit avec les données existantes. Il modifie et soumet. Angular envoie `PUT /api/patients/3` avec **toutes** les données du patient dans le body.

Le `PUT` remplace l'intégralité de la ressource. Dapper exécute :

```sql
UPDATE patient SET nom=@nom, prenom=@prenom, ... WHERE idPatient=@idPatient
```

L'API répond `204 No Content`. Angular recharge la liste.

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/features/patients/patients.component.ts` → méthode `openEdit(p)` ligne 55 : pré-remplit le formulaire avec `patientForm.patchValue(...)` et met `editingId = p.idPatient`; méthode `save()` ligne 72 : si `editingId !== null`, appelle `patientSvc.update(editingId, data)`
2. `frontend/src/app/core/services/patient.service.ts` → `update()` ligne 25 : `return this.http.put<void>(\`${this.url}/${id}\`, patient)` — l'id est dans l'URL, toutes les données sont dans le body
3. `backend/Api/EndPoints/PatientRoutes.cs` → `MapPut("/{id:int}", ...)` ligne 34 : récupère l'id depuis l'URL, construit un `Patient` avec cet id et les données du DTO, appelle `useCases.Update(patient)`
4. `backend/Core/UseCases/PatientUseCases.cs` → `Update()` ligne 31 : appelle d'abord `GetById(patient.IdPatient)` pour vérifier que le patient existe (lève `KeyNotFoundException` sinon), puis `_gateway.Update(patient)`
5. `backend/Infrastructure/Repositories/PatientRepository.cs` → `Update()` ligne 35 : `UPDATE patient SET nom=@nom, prenom=@prenom, ... WHERE idPatient=@idPatient`

---

## 6. Flux de modification partielle — PATCH

Pour changer uniquement le statut d'un rendez-vous, on utilise `PATCH` plutôt que `PUT` car on ne veut modifier **qu'un seul champ**, sans envoyer toutes les données.

`PATCH /api/rendezvous/5/statut` avec le body `{ "statut": "confirmé" }`.

Dapper exécute :

```sql
UPDATE rendezvous SET statut=@Statut WHERE idRDV=@Id
```

L'API répond `204 No Content`. C'est plus efficace qu'un `PUT` complet et plus sémantiquement correct : on cible la sous-ressource `statut`.

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/core/services/rendezvous.service.ts` → `updateStatut()` ligne 33 : `return this.http.patch<void>(\`${this.url}/${id}/statut\`, { statut })` — on n'envoie que le champ `statut`, pas tout l'objet
2. `backend/Api/EndPoints/RendezVousRoutes.cs` → `MapPatch("/{id:int}/statut", ...)` ligne 52 : reçoit l'id et le body `UpdateStatutRequest`, appelle `useCases.UpdateStatut(id, req.Statut)`
3. `backend/Core/UseCases/RendezVousUseCases.cs` → `UpdateStatut()` ligne 43 : vérifie que le rendez-vous existe via `GetById(id)`, valide que le statut n'est pas vide, puis `_gateway.UpdateStatut(id, statut)`
4. `backend/Infrastructure/Gateways/RendezVousGateway.cs` → `UpdateStatut()` : délègue au repository avec seulement l'id et le statut — c'est ici que la conversion Gateway→Repository se fait

---

## 7. Flux de suppression — DELETE

L'utilisateur clique "Supprimer" et confirme. Angular envoie `DELETE /api/patients/3` (sans body). Dapper exécute :

```sql
DELETE FROM patient WHERE idPatient=@id
```

L'API répond `204 No Content`. Angular retire l'élément de la liste locale (ou recharge).

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/features/patients/patients.component.ts` → méthode `delete(id)` ligne 93 : affiche une confirmation `confirm('Supprimer ce patient ?')`, puis `patientSvc.delete(id).subscribe(() => this.load())` — recharge la liste après suppression
2. `frontend/src/app/core/services/patient.service.ts` → `delete()` ligne 29 : `return this.http.delete<void>(\`${this.url}/${id}\`)` — requête DELETE sans body, l'id est dans l'URL
3. `backend/Api/EndPoints/PatientRoutes.cs` → `MapDelete("/{id:int}", ...)` ligne 50 : `useCases.Delete(id)` et `Results.NoContent()`
4. `backend/Core/UseCases/PatientUseCases.cs` → `Delete()` ligne 37 : appelle d'abord `GetById(id)` pour vérifier que le patient existe (protection contre suppression fantôme), puis `_gateway.Delete(id)`
5. `backend/Infrastructure/Repositories/PatientRepository.cs` → `Delete()` ligne 43 : `conn.Execute("DELETE FROM patient WHERE idPatient = @id", new { id })`

---

## 8. Gestion des erreurs — le Middleware global

Toutes les exceptions non gérées dans le code backend sont interceptées par le **middleware d'exception** (`GlobalExceptionMiddleware`) avant d'atteindre le client. Il traduit :

- `KeyNotFoundException` → `404 Not Found`
- `ArgumentException` → `400 Bad Request`
- Toute autre exception → `500 Internal Server Error`

Chaque réponse d'erreur est un JSON structuré `{ "error": "message" }`, jamais une page HTML ou une stack trace. Côté Angular, les services gèrent ces erreurs avec `catchError` (RxJS) pour afficher un message à l'utilisateur.

**Pour le montrer à l'examen, fichier par fichier :**

1. `backend/Api/Program.cs` → ligne 63 : `app.UseMiddleware<GlobalExceptionMiddleware>()` — enregistré **en premier** dans le pipeline pour intercepter toutes les exceptions de tous les endpoints
2. `backend/Api/Middleware/GlobalExceptionMiddleware.cs` → `InvokeAsync()` ligne 17 : wrappe `await _next(context)` dans un `try/catch` avec 3 blocs : `KeyNotFoundException` → 404, `ArgumentException` → 400, `Exception` → 500; chaque branche appelle `WriteError()` qui sérialise `{ "error": message }` en JSON
3. `backend/Core/UseCases/PatientUseCases.cs` → `GetById()` ligne 17 : `throw new KeyNotFoundException($"Patient {id} introuvable.")` → le middleware l'attrape et retourne `{ "error": "Patient 99 introuvable." }` avec code 404
4. `backend/Core/UseCases/PatientUseCases.cs` → `Create()` ligne 25 : `throw new ArgumentException("Le nom est obligatoire.")` → le middleware retourne `{ "error": "Le nom est obligatoire." }` avec code 400

---

## 9. Les Guards Angular — protection des routes

Certaines routes Angular sont protégées par des **guards** :

- `AuthGuard` : vérifie qu'un token JWT valide est présent dans le localStorage. Si l'utilisateur n'est pas connecté, il est redirigé vers `/login`.
- Guards de rôle : vérifie que le rôle stocké correspond à la route. Un patient ne peut pas accéder au dashboard secrétaire.

Ce système de protection existe **en plus** des vérifications côté backend. Les guards Angular protègent la navigation (UX), mais les endpoints backend vérifient toujours le JWT indépendamment — un utilisateur malveillant qui bypasse les guards Angular sera bloqué par le backend de toute façon.

**Pour le montrer à l'examen, fichier par fichier :**

1. `frontend/src/app/app-routing-module.ts` → lignes 22-23 : `canActivate: [AuthGuard], data: { roles: ['patient'] }` — Angular consulte le guard avant d'activer le composant; même chose lignes 34-38 pour `medecin` et lignes 41-46 pour `secretaire`
2. `frontend/src/app/core/guards/auth.guard.ts` → `canActivate()` ligne 9 : vérifie d'abord `this.authService.isLoggedIn()` — si `false`, redirige vers `/login`; ensuite lit `route.data?.['roles']` et vérifie que `getRole()` est dans la liste — si pas autorisé, redirige aussi vers `/login`
3. `frontend/src/app/core/services/auth.service.ts` → `isLoggedIn()` ligne 32 : décode le JWT localement (sans appel serveur) et compare `payload.exp * 1000 > Date.now()` — le token expire automatiquement après 24h

---

## 10. Résumé : qui fait quoi dans chaque flux

| Responsabilité | Où |
|---|---|
| Affichage et interaction utilisateur | Component Angular |
| Appels HTTP (get/post/put/patch/delete) | Service Angular |
| Ajout automatique du token JWT | Intercepteur HTTP Angular |
| Protection des routes (navigation) | Guards Angular |
| Réception de la requête HTTP et routage | EndPoint (Minimal API) |
| Désérialisation du body JSON | DTO (record C#) automatiquement |
| Validation et règles métier | Use Case (couche Core) |
| Conversion modèle DB ↔ modèle Core | Gateway (couche Infrastructure) |
| Exécution SQL | Repository via Dapper |
| Gestion globale des erreurs HTTP | Middleware d'exception |
| Authentification stateless | Token JWT (header Authorization)
