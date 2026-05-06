import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { AuthApiService } from './api/auth-api.service';
import { TokenService } from './token.service';
import { LoginRequest, RegisterRequest } from '../core/models/auth.model';
import { ApiResponse } from '../core/models/api-response.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenService = inject(TokenService);
  private readonly router = inject(Router);

  readonly isAuthenticated = signal(this.tokenService.isAuthenticated());
  readonly currentUsername = signal<string | null>(this.tokenService.getUsername());

  login(request: LoginRequest): Observable<unknown> {
    return this.authApi.login(request).pipe(
      tap((response) => {
        const { token, refreshToken, username } = response.data;
        this.tokenService.setTokens(token, refreshToken, username);
        this.isAuthenticated.set(true);
        this.currentUsername.set(username);
      })
    );
  }

  register(request: RegisterRequest): Observable<ApiResponse> {
    return this.authApi.register(request);
  }

  logout(): void {
    this.authApi.logout().subscribe({
      complete: () => this.clearAndRedirect(),
      error: () => this.clearAndRedirect(),
    });
  }

  clearAndRedirect(): void {
    this.tokenService.clearTokens();
    this.isAuthenticated.set(false);
    this.currentUsername.set(null);
    this.router.navigate(['/login']);
  }
}
