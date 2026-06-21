import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  registerForm: FormGroup;
  mode: 'login' | 'register' = 'login';
  errorMessage = '';
  loading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      nom: ['', Validators.required],
      prenom: ['', Validators.required],
      dateNaissance: ['', Validators.required],
      sexe: ['M', Validators.required],
      telephone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  login(): void {
    if (this.loginForm.invalid) return;
    this.loading = true;
    this.errorMessage = '';
    const { email, password } = this.loginForm.value;
    this.authService.login(email, password).subscribe({
      next: (res) => {
        this.loading = false;
        if (res.role === 'patient') this.router.navigate(['/patient-portal']);
        else if (res.role === 'medecin') this.router.navigate(['/medecin-portal']);
        else this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Email ou mot de passe incorrect.';
        this.cdr.detectChanges();
      }
    });
  }

  register(): void {
    if (this.registerForm.invalid) return;
    this.loading = true;
    this.errorMessage = '';
    this.authService.register(this.registerForm.value).subscribe({
      next: () => {
        this.loading = false;
        this.mode = 'login';
        this.loginForm.patchValue({ email: this.registerForm.value.email });
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Erreur lors de l\'inscription. Cet email est peut-être déjà utilisé.';
        this.cdr.detectChanges();
      }
    });
  }
}
