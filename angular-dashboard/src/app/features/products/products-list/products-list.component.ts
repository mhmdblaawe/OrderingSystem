import { Component, OnInit, AfterViewInit, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { AlertService } from '../../../core/services/alert.service';
import { Product } from '../../../core/models/product.model';
import { FormsModule } from '@angular/forms';
import { DataTableComponent, TableColumn } from '../../../shared/components/data-table/data-table.component';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DataTableComponent
  ],
  templateUrl: './products-list.component.html',
  styleUrl: './products-list.component.scss'
})
export class ProductsListComponent implements OnInit, AfterViewInit {
  @ViewChild('priceTemplate') priceTemplate!: TemplateRef<any>;
  @ViewChild('stockTemplate') stockTemplate!: TemplateRef<any>;
  @ViewChild('actionsTemplate') actionsTemplate!: TemplateRef<any>;

  columns: TableColumn[] = [];
  products: Product[] = [];
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  searchTerm = '';
  loading = false;

  constructor(
    private productService: ProductService,
    private router: Router,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    // Columns will be initialized after view init to access templates
  }

  ngAfterViewInit(): void {
    this.initializeColumns();
    this.loadProducts();
  }

  initializeColumns(): void {
    this.columns = [
      {
        key: 'id',
        label: 'ID',
        sortable: true,
        width: '100px',
        align: 'left'
      },
      {
        key: 'name',
        label: 'Product Name',
        sortable: true,
        align: 'left'
      },
      {
        key: 'sku',
        label: 'SKU',
        sortable: true,
        width: '180px',
        align: 'left'
      },
      {
        key: 'price',
        label: 'Price',
        sortable: true,
        width: '120px',
        align: 'left',
        template: this.priceTemplate
      },
      {
        key: 'stockQuantity',
        label: 'Stock',
        sortable: true,
        width: '120px',
        align: 'left',
        template: this.stockTemplate
      },
      {
        key: 'actions',
        label: 'Actions',
        sortable: false,
        width: '200px',
        align: 'left',
        template: this.actionsTemplate
      }
    ];
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getProductsPaged(
      this.pageIndex + 1,
      this.pageSize,
      this.searchTerm
    ).subscribe({
      next: (response) => {
        this.products = response.items;
        this.totalItems = response.total;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.alertService.error('Error loading products');
      }
    });
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadProducts();
  }

  onSort(event: { column: string; direction: 'asc' | 'desc' }): void {
    // Implement sorting logic if needed
    // For now, just reload with current settings
    this.loadProducts();
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.loadProducts();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.pageIndex = 0;
    this.loadProducts();
  }

  addProduct(): void {
    this.router.navigate(['/products/new']);
  }

  editProduct(id: number): void {
    this.router.navigate(['/products/edit', id]);
  }

  deleteProduct(id: number): void {
    this.alertService.deleteConfirm(
      'Are you sure you want to delete this product?',
      'Delete Product'
    ).then((result) => {
      if (result.isConfirmed) {
        this.productService.deleteProduct(id).subscribe({
          next: () => {
            this.alertService.success('Product deleted successfully');
            this.loadProducts();
          },
          error: () => {
            this.alertService.error('Error deleting product');
          }
        });
      }
    });
  }
}
