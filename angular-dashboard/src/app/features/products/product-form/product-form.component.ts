import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { AlertService } from '../../../core/services/alert.service';
import { LoadingService } from '../../../core/services/loading.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-product-form',
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
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  isEditMode = false;
  productId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private loadingService: LoadingService
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      sku: ['', [Validators.required]],
      price: [0, [Validators.required, Validators.min(0)]],
      stockQuantity: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.productId = +id;
      this.loadProduct();
    }
  }

  loadProduct(): void {
    if (this.productId) {
      this.productService.getProductById(this.productId).subscribe({
        next: (product) => {
          this.productForm.patchValue({
            name: product.name,
            sku: product.sku,
            price: product.price,
            stockQuantity: product.stockQuantity
          });
        },
        error: () => {
          this.alertService.error('Error loading product').then(() => {
            this.router.navigate(['/products']);
          });
        }
      });
    }
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.loading = true;
      this.loadingService.show();
      const productData = this.productForm.value;

      if (this.isEditMode && this.productId) {
        this.productService.updateProduct(this.productId, productData).subscribe({
          next: () => {
            this.loadingService.hide();
            this.alertService.success('Product updated successfully').then(() => {
              this.router.navigate(['/products']);
            });
          },
          error: () => {
            this.loading = false;
            this.loadingService.hide();
            this.alertService.error('Error updating product');
          }
        });
      } else {
        this.productService.createProduct(productData).subscribe({
          next: () => {
            this.loadingService.hide();
            this.alertService.success('Product created successfully').then(() => {
              this.router.navigate(['/products']);
            });
          },
          error: () => {
            this.loading = false;
            this.loadingService.hide();
            this.alertService.error('Error creating product');
          }
        });
      }
    }
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }
}

