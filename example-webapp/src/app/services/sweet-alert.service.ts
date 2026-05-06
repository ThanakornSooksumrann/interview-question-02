import { Injectable } from '@angular/core';
import Swal, { SweetAlertResult } from 'sweetalert2';

@Injectable({ providedIn: 'root' })
export class SweetAlertService {
  success(title: string, text?: string): Promise<SweetAlertResult> {
    return Swal.fire({ icon: 'success', title, text, confirmButtonText: 'ตกลง', confirmButtonColor: '#22c55e' });
  }

  error(title: string, text?: string): Promise<SweetAlertResult> {
    return Swal.fire({ icon: 'error', title, text, confirmButtonText: 'ตกลง', confirmButtonColor: '#ef4444' });
  }

  warning(title: string, text?: string): Promise<SweetAlertResult> {
    return Swal.fire({ icon: 'warning', title, text, confirmButtonText: 'ตกลง', confirmButtonColor: '#f59e0b' });
  }
}
