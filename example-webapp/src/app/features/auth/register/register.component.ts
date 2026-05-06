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
import { strongPasswordValidator, passwordMatchValidator } from '../../../core/validators/password.validator';

interface PasswordRequirement {
  key: string;
  label: string;
}

@Component({
  selector: 'app-register',
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
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  private readonly fb          = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router      = inject(Router);
  private readonly swal        = inject(SweetAlertService);

  loading = false;

  form = this.fb.group(
    {
      username:        ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      password:        ['', [Validators.required, strongPasswordValidator]],
      confirmPassword: ['', Validators.required],
    },
    { validators: passwordMatchValidator }
  );

  readonly requirements: PasswordRequirement[] = [
    { key: 'minLength',  label: 'อย่างน้อย 8 ตัวอักษร' },
    { key: 'uppercase',  label: 'ตัวพิมพ์ใหญ่ (A-Z)' },
    { key: 'lowercase',  label: 'ตัวพิมพ์เล็ก (a-z)' },
    { key: 'number',     label: 'ตัวเลข (0-9)' },
    { key: 'special',    label: 'อักขระพิเศษ (!@#$%^&*)' },
  ];

  get passwordValue(): string {
    return this.form.get('password')?.value ?? '';
  }

  isInvalid(field: string): boolean {
    const c = this.form.get(field);
    return !!(c?.invalid && c.touched);
  }

  isGroupError(error: string): boolean {
    return !!(this.form.hasError(error) && this.form.get('confirmPassword')?.touched);
  }

  pwHasError(key: string): boolean {
    return !!this.form.get('password')?.hasError(key);
  }

  getUsernameError(): string {
    const c = this.form.get('username');
    if (c?.hasError('required'))   return 'กรุณากรอกชื่อผู้ใช้งาน';
    if (c?.hasError('minlength'))  return 'ชื่อผู้ใช้งานต้องมีอย่างน้อย 3 ตัวอักษร';
    if (c?.hasError('maxlength'))  return 'ชื่อผู้ใช้งานต้องไม่เกิน 50 ตัวอักษร';
    return '';
  }

  onRegister(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid || this.loading) return;

    this.loading = true;
    const { username, password, confirmPassword } = this.form.getRawValue();

    this.authService.register({ username: username!, password: password!, confirmPassword: confirmPassword! }).subscribe({
      next: () => {
        this.swal.success('สมัครสมาชิกสำเร็จ', 'กรุณาเข้าสู่ระบบด้วยบัญชีที่สร้างใหม่').then(() => {
          this.router.navigate(['/login']);
        });
      },
      error: (err: HttpErrorResponse) => {
        this.loading = false;
        if (err.status === 0) return;
        const msg = err.error?.message || 'ไม่สามารถสมัครสมาชิกได้';
        this.swal.error('สมัครสมาชิกไม่สำเร็จ', msg);
      },
    });
  }
}
