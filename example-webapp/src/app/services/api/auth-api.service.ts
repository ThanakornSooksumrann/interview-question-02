import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_BASE_URL, API_ENDPOINTS } from '../../core/constants/api.constants';
import { ApiResponse, ApiResponseWithData } from '../../core/models/api-response.model';
import { AuthResponse, LoginRequest, RegisterRequest } from '../../core/models/auth.model';
import { HttpService } from '../http.service';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpService);

  private url(path: string): string {
    return `${API_BASE_URL}${path}`;
  }

  register(body: RegisterRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.url(API_ENDPOINTS.AUTH.REGISTER), body);
  }

  login(body: LoginRequest): Observable<ApiResponseWithData<AuthResponse>> {
    return this.http.post<ApiResponseWithData<AuthResponse>>(this.url(API_ENDPOINTS.AUTH.LOGIN), body);
  }

  me(): Observable<ApiResponseWithData<{ username: string }>> {
    return this.http.get<ApiResponseWithData<{ username: string }>>(this.url(API_ENDPOINTS.AUTH.ME));
  }

  logout(): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.url(API_ENDPOINTS.AUTH.LOGOUT), {});
  }
}
