import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const strongPasswordValidator: ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
  const value: string = control.value ?? '';
  if (!value) return null;

  const errors: ValidationErrors = {};
  if (value.length < 8)                                          errors['minLength']  = true;
  if (!/[A-Z]/.test(value))                                     errors['uppercase']  = true;
  if (!/[a-z]/.test(value))                                     errors['lowercase']  = true;
  if (!/[0-9]/.test(value))                                     errors['number']     = true;
  if (!/[!@#$%^&*()\-_=+\[\]{};':"\\|,.<>/?`~]/.test(value))  errors['special']    = true;

  return Object.keys(errors).length ? errors : null;
};

export const passwordMatchValidator: ValidatorFn = (group: AbstractControl): ValidationErrors | null => {
  const pw  = group.get('password')?.value;
  const cpw = group.get('confirmPassword')?.value;
  return pw && cpw && pw !== cpw ? { passwordMismatch: true } : null;
};
