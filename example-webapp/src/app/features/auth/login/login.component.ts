import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { DividerModule } from 'primeng/divider';

import { AuthService } from '../../../services/auth.service';
import { SweetAlertService } from '../../../services/sweet-alert.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    IconFieldModule,
    InputIconModule,
    DividerModule,
  ],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  private readonly fb           = inject(FormBuilder);
  private readonly authService  = inject(AuthService);
  private readonly router       = inject(Router);
  private readonly swal         = inject(SweetAlertService);

  loading = false;

  form = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]],
  });

  isInvalid(field: string): boolean {
    const c = this.form.get(field);
    return !!(c?.invalid && c.touched);
  }

  onLogin(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.loading) return;

    this.loading = true;

    this.authService.login(this.form.getRawValue() as { username: string; password: string }).subscribe({
      next: () => this.router.navigate(['/welcome']),
      error: (err: HttpErrorResponse) => {
        this.loading = false;
        if (err.status === 0) return;
        const msg = err.error?.message || 'ชื่อผู้ใช้งานหรือรหัสผ่านไม่ถูกต้อง';
        this.swal.error('เข้าสู่ระบบไม่สำเร็จ', msg);
      },
    });
  }
}
