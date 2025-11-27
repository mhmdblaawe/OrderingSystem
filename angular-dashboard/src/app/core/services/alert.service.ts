import { Injectable } from '@angular/core';
import Swal, { SweetAlertOptions, SweetAlertResult } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  
  success(message: string, title: string = 'Success'): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'success',
      title: title,
      text: message,
      confirmButtonColor: '#667eea',
      confirmButtonText: 'OK',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button'
      }
    });
  }

  error(message: string, title: string = 'Error'): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'error',
      title: title,
      text: message,
      confirmButtonColor: '#ef4444',
      confirmButtonText: 'OK',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button'
      }
    });
  }

  warning(message: string, title: string = 'Warning'): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'warning',
      title: title,
      text: message,
      confirmButtonColor: '#f59e0b',
      confirmButtonText: 'OK',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button'
      }
    });
  }

  info(message: string, title: string = 'Information'): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'info',
      title: title,
      text: message,
      confirmButtonColor: '#3b82f6',
      confirmButtonText: 'OK',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button'
      }
    });
  }

  confirm(
    message: string,
    title: string = 'Are you sure?',
    confirmText: string = 'Yes, proceed',
    cancelText: string = 'Cancel'
  ): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'question',
      title: title,
      text: message,
      showCancelButton: true,
      confirmButtonColor: '#667eea',
      cancelButtonColor: '#6b7280',
      confirmButtonText: confirmText,
      cancelButtonText: cancelText,
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button',
        cancelButton: 'swal-cancel-button'
      }
    });
  }

  deleteConfirm(
    message: string,
    title: string = 'Delete Confirmation'
  ): Promise<SweetAlertResult> {
    return Swal.fire({
      icon: 'warning',
      title: title,
      text: message,
      showCancelButton: true,
      confirmButtonColor: '#ef4444',
      cancelButtonColor: '#6b7280',
      confirmButtonText: 'Yes, delete',
      cancelButtonText: 'Cancel',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button',
        cancelButton: 'swal-cancel-button'
      }
    });
  }

  custom(options: SweetAlertOptions): Promise<SweetAlertResult> {
    return Swal.fire({
      confirmButtonColor: '#667eea',
      buttonsStyling: true,
      customClass: {
        confirmButton: 'swal-confirm-button'
      },
      ...options
    });
  }
}

