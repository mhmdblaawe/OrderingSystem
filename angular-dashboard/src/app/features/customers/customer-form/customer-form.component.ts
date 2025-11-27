import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomerService } from '../../../core/services/customer.service';
import { AlertService } from '../../../core/services/alert.service';
import { LoadingService } from '../../../core/services/loading.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-customer-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './customer-form.component.html',
  styleUrl: './customer-form.component.scss'
})
export class CustomerFormComponent {
  customerForm: FormGroup;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private router: Router,
    private alertService: AlertService,
    private loadingService: LoadingService
  ) {
    this.customerForm = this.fb.group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.customerForm.valid) {
      this.loading = true;
      this.loadingService.show();
      this.customerService.createCustomer(this.customerForm.value).subscribe({
        next: () => {
          this.loadingService.hide();
          this.alertService.success('Customer created successfully').then(() => {
            this.router.navigate(['/customers']);
          });
        },
        error: () => {
          this.loading = false;
          this.loadingService.hide();
          this.alertService.error('Error creating customer');
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/customers']);
  }
}

