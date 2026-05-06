import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { STORAGE_KEYS } from '../core/constants/api.constants';

@Injectable({ providedIn: 'root' })
export class TokenService {
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  setTokens(token: string, refreshToken: string, username: string): void {
    if (!this.isBrowser) return;
    localStorage.setItem(STORAGE_KEYS.ACCESS_TOKEN, token);
    localStorage.setItem(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
    localStorage.setItem(STORAGE_KEYS.USERNAME, username);
  }

  getAccessToken(): string | null {
    return this.isBrowser ? localStorage.getItem(STORAGE_KEYS.ACCESS_TOKEN) : null;
  }

  getRefreshToken(): string | null {
    return this.isBrowser ? localStorage.getItem(STORAGE_KEYS.REFRESH_TOKEN) : null;
  }

  getUsername(): string | null {
    return this.isBrowser ? localStorage.getItem(STORAGE_KEYS.USERNAME) : null;
  }

  clearTokens(): void {
    if (!this.isBrowser) return;
    localStorage.removeItem(STORAGE_KEYS.ACCESS_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USERNAME);
  }

  isTokenExpired(token: string): boolean {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return Date.now() >= payload.exp * 1000;
    } catch {
      return true;
    }
  }

  isAuthenticated(): boolean {
    const token = this.getAccessToken();
    if (!token) return false;
    return !this.isTokenExpired(token);
  }
}
