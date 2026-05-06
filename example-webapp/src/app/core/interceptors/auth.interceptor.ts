import { inject } from '@angular/core';
import { HttpInterceptorFn, HttpRequest, HttpHandlerFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

import { TokenService } from '../../services/token.service';
import { API_ENDPOINTS } from '../constants/api.constants';

const PUBLIC_ENDPOINTS = [API_ENDPOINTS.AUTH.LOGIN, API_ENDPOINTS.AUTH.REGISTER];

export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const tokenService = inject(TokenService);
  const router       = inject(Router);

  const token   = tokenService.getAccessToken();
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      const isPublic = PUBLIC_ENDPOINTS.some((ep) => req.url.includes(ep));

      if (error.status === 401 && !isPublic) {
        tokenService.clearTokens();
        router.navigate(['/login']);
        Swal.fire({ icon: 'warning', title: 'Session หมดอายุ', text: 'กรุณาเข้าสู่ระบบใหม่', confirmButtonText: 'ตกลง' });
      } else if (error.status === 0) {
        Swal.fire({ icon: 'error', title: 'เชื่อมต่อไม่ได้', text: 'ไม่สามารถเชื่อมต่อกับเซิร์ฟเวอร์ได้', confirmButtonText: 'ตกลง' });
      }

      return throwError(() => error);
    })
  );
};
