# AppModule & AppRoutingModule — Configuration Angular

**Fichiers :**
- `frontend/src/app/app-module.ts`
- `frontend/src/app/app-routing-module.ts`

## AppModule — le registre de l'application

```typescript
@NgModule({
  declarations: [
    App,
    DashboardComponent,
    PatientsComponent,
    MedecinsComponent,
    RendezVousComponent,
    ConsultationsComponent,
    MedicamentsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule {}
```

### `declarations`
Liste tous les **composants** de l'application. Tout composant utilisé dans un template doit être déclaré ici (car `standalone: false`).

### `imports`
Modules Angular nécessaires :
| Module | Rôle |
|--------|------|
| `BrowserModule` | Rend l'app dans le navigateur (nécessaire une seule fois) |
| `HttpClientModule` | Active `HttpClient` (requêtes HTTP) |
| `ReactiveFormsModule` | Active `FormBuilder`, `FormGroup`, `Validators` |
| `FormsModule` | Active `[(ngModel)]` (binding two-way, si utilisé) |
| `AppRoutingModule` | Configure les routes de navigation |

### `bootstrap`
`App` est le **composant racine** — le premier composant chargé, qui contient la structure générale (`<router-outlet>`).

### `providers`
`provideBrowserGlobalErrorListeners()` : capture les erreurs JavaScript non gérées dans le navigateur (Angular 21+).

---

## AppRoutingModule — la navigation

```typescript
const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'patients', component: PatientsComponent },
  { path: 'medecins', component: MedecinsComponent },
  { path: 'rendezvous', component: RendezVousComponent },
  { path: 'consultations', component: ConsultationsComponent },
  { path: 'medicaments', component: MedicamentsComponent },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
```

### Fonctionnement des routes

| Route | Comportement |
|-------|-------------|
| `''` (vide) | Redirige vers `/dashboard` |
| `/dashboard` | Affiche `DashboardComponent` |
| `/patients` | Affiche `PatientsComponent` |
| `**` (wildcard) | Toute URL inconnue → redirige vers `/dashboard` |

### `pathMatch: 'full'`
Sur la route vide (`''`), `pathMatch: 'full'` signifie que la redirection ne s'applique que si **toute** l'URL est vide. Sans ça, `''` matcherait n'importe quelle URL (car toute URL commence par `''`).

### `RouterModule.forRoot(routes)`
`forRoot` configure le Router au niveau **racine** de l'application. Il y a aussi `forChild` pour les modules de fonctionnalités (pas utilisé ici car pas de lazy loading).

### `<router-outlet>`
Dans le template de `App`, la balise `<router-outlet>` est l'endroit où Angular affiche le composant correspondant à l'URL actuelle. Chaque navigation remplace le contenu de `<router-outlet>`.

---

## Différence avec les composants standalone (Angular 17+)

Ce projet utilise l'approche **module-based** (ancienne mais toujours valide) :
- Composants déclarés dans `AppModule`
- Routes centralisées dans `AppRoutingModule`

L'approche **standalone** (Angular 17+) :
- `standalone: true` dans chaque composant
- Chaque composant importe ses propres dépendances
- Pas de `NgModule` nécessaire

Les deux approches fonctionnent. Ce projet suit l'architecture module-based du professeur.
