import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { TokenService } from '../../services/token.service';

/** Protects routes that require a valid JWT. */
export const authGuard: CanActivateFn = () => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  if (tokenService.isAuthenticated()) return true;

  router.navigate(['/login']);
  return false;
};

/** Redirects already-authenticated users away from login / register. */
export const guestGuard: CanActivateFn = () => {
  const tokenService = inject(TokenService);
  const router = inject(Router);

  if (!tokenService.isAuthenticated()) return true;

  router.navigate(['/welcome']);
  return false;
};
