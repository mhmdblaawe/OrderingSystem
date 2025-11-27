import { Component, OnInit, AfterViewInit, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CustomerService } from '../../../core/services/customer.service';
import { AlertService } from '../../../core/services/alert.service';
import { Customer } from '../../../core/models/customer.model';
import { FormsModule } from '@angular/forms';
import { DataTableComponent, TableColumn } from '../../../shared/components/data-table/data-table.component';

@Component({
  selector: 'app-customers-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DataTableComponent
  ],
  templateUrl: './customers-list.component.html',
  styleUrl: './customers-list.component.scss'
})
export class CustomersListComponent implements OnInit, AfterViewInit {
  @ViewChild('actionsTemplate') actionsTemplate!: TemplateRef<any>;

  columns: TableColumn[] = [];
  customers: Customer[] = [];
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  searchName = '';
  loading = false;

  constructor(
    private customerService: CustomerService,
    private router: Router,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    // Columns will be initialized after view init
  }

  ngAfterViewInit(): void {
    this.initializeColumns();
    this.loadCustomers();
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
        label: 'Customer Name',
        sortable: true,
        align: 'left'
      },
      {
        key: 'email',
        label: 'Email',
        sortable: true,
        align: 'left'
      },
      {
        key: 'phone',
        label: 'Phone',
        sortable: true,
        width: '150px',
        align: 'left'
      },
      {
        key: 'actions',
        label: 'Actions',
        sortable: false,
        width: '150px',
        align: 'left',
        template: this.actionsTemplate
      }
    ];
  }

  loadCustomers(): void {
    this.loading = true;
    this.customerService.getCustomersPaged(
      this.pageIndex + 1,
      this.pageSize,
      this.searchName || undefined
    ).subscribe({
      next: (response) => {
        this.customers = response.items;
        this.totalItems = response.total;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.alertService.error('Error loading customers');
      }
    });
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadCustomers();
  }

  onSort(event: { column: string; direction: 'asc' | 'desc' }): void {
    // Implement sorting logic if needed
    this.loadCustomers();
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.loadCustomers();
  }

  clearSearch(): void {
    this.searchName = '';
    this.pageIndex = 0;
    this.loadCustomers();
  }

  addCustomer(): void {
    this.router.navigate(['/customers/new']);
  }

  deleteCustomer(id: number): void {
    this.alertService.deleteConfirm(
      'Are you sure you want to delete this customer?',
      'Delete Customer'
    ).then((result) => {
      if (result.isConfirmed) {
        this.customerService.deleteCustomer(id).subscribe({
          next: () => {
            this.alertService.success('Customer deleted successfully');
            this.loadCustomers();
          },
          error: () => {
            this.alertService.error('Error deleting customer');
          }
        });
      }
    });
  }
}
