import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return false;
    }
    const allowedRoles = route.data?.['roles'] as string[] | undefined;
    if (allowedRoles && !allowedRoles.includes(this.authService.getRole())) {
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}
