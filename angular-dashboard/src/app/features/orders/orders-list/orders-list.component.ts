import { Component, OnInit, AfterViewInit, ViewChild, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { OrderService } from '../../../core/services/order.service';
import { CustomerService } from '../../../core/services/customer.service';
import { AlertService } from '../../../core/services/alert.service';
import { Order, ORDER_STATUSES, CreateOrder } from '../../../core/models/order.model';
import { Customer } from '../../../core/models/customer.model';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { DataTableComponent, TableColumn } from '../../../shared/components/data-table/data-table.component';
import { OrderDetailsDialogComponent } from '../order-details-dialog/order-details-dialog.component';

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DataTableComponent
  ],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.scss'
})
export class OrdersListComponent implements OnInit, AfterViewInit {
  @ViewChild('statusTemplate') statusTemplate!: TemplateRef<any>;
  @ViewChild('actionsTemplate') actionsTemplate!: TemplateRef<any>;
  @ViewChild('dateTemplate') dateTemplate!: TemplateRef<any>;
  @ViewChild('amountTemplate') amountTemplate!: TemplateRef<any>;

  columns: TableColumn[] = [];
  orders: Order[] = [];
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  loading = false;
  statusFilter: number | null = null;
  customerSearch: string = '';
  startDate: string = '';
  endDate: string = '';
  statuses = ORDER_STATUSES;
  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private router: Router,
    private alertService: AlertService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  ngAfterViewInit(): void {
    this.initializeColumns();
    this.loadOrders();
  }

  initializeColumns(): void {
    this.columns = [
      {
        key: 'orderId',
        label: 'Order ID',
        sortable: true,
        width: '120px',
        align: 'left'
      },
      {
        key: 'orderDate',
        label: 'Date',
        sortable: true,
        width: '150px',
        align: 'left',
        template: this.dateTemplate
      },
      {
        key: 'customerName',
        label: 'Customer',
        sortable: true,
        align: 'left'
      },
      {
        key: 'totalAmount',
        label: 'Total',
        sortable: true,
        width: '120px',
        align: 'left',
        template: this.amountTemplate
      },
      {
        key: 'statusDesc',
        label: 'Status',
        sortable: true,
        width: '150px',
        align: 'left',
        template: this.statusTemplate
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

  loadCustomers(): void {
    this.customerService.getCustomersPaged(1, 1000).subscribe({
      next: (response) => {
        this.customers = response.items;
        this.filteredCustomers = response.items;
      },
      error: () => {
        this.alertService.error('Error loading customers');
      }
    });
  }

  onCustomerSearchChange(): void {
    if (!this.customerSearch || this.customerSearch.trim() === '') {
      this.filteredCustomers = this.customers;
      this.onFilterChange();
      return;
    }

    const searchTerm = this.customerSearch.toLowerCase().trim();
    this.filteredCustomers = this.customers.filter(customer =>
      customer.name.toLowerCase().includes(searchTerm) ||
      customer.email.toLowerCase().includes(searchTerm) ||
      customer.phone.includes(searchTerm)
    );

    // Only filter orders if there's an exact match with a single customer
    const exactMatch = this.customers.find(customer =>
      customer.name.toLowerCase() === searchTerm ||
      customer.email.toLowerCase() === searchTerm ||
      customer.phone === searchTerm
    );

    if (exactMatch) {
      this.onFilterChange();
    }
  }

  loadOrders(): void {
    this.loading = true;
    const startDateObj = this.startDate ? new Date(this.startDate) : undefined;
    const endDateObj = this.endDate ? new Date(this.endDate) : undefined;

    // If customer search matches exactly one customer, use its ID for filtering
    let customerId: number | undefined = undefined;
    if (this.customerSearch && this.customerSearch.trim() !== '') {
      const searchTerm = this.customerSearch.toLowerCase().trim();
      const matchedCustomer = this.customers.find(customer =>
        customer.name.toLowerCase() === searchTerm ||
        customer.email.toLowerCase() === searchTerm ||
        customer.phone === searchTerm
      );
      if (matchedCustomer) {
        customerId = matchedCustomer.id;
      }
      // If multiple matches, don't filter by customer ID (show all matching orders)
    }

    this.orderService.getOrdersPaged(
      this.pageIndex + 1,
      this.pageSize,
      customerId,
      this.statusFilter || undefined,
      startDateObj,
      endDateObj
    ).subscribe({
      next: (response) => {
        this.orders = response.items;
        this.totalItems = response.totalCount;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        console.error('Error loading orders:', error);
        const message = error?.error?.message || 'Error loading orders';
        this.alertService.error(message);
      }
    });
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadOrders();
  }

  onSort(event: { column: string; direction: 'asc' | 'desc' }): void {
    this.loadOrders();
  }

  onFilterChange(): void {
    this.pageIndex = 0;
    this.loadOrders();
  }

  onStatusFilterChange(): void {
    this.onFilterChange();
  }

  onDateFilterChange(): void {
    this.onFilterChange();
  }

  clearFilters(): void {
    this.statusFilter = null;
    this.customerSearch = '';
    this.startDate = '';
    this.endDate = '';
    this.filteredCustomers = this.customers;
    this.onFilterChange();
  }

  canChangeStatus(statusId: number): boolean {
    // Cannot change status if order is Shipped (4) or Cancelled (3)
    return statusId !== 3 && statusId !== 4;
  }

  changeStatus(order: Order, newStatusId: number): void {
    // Prevent status change for Shipped or Cancelled orders
    if (!this.canChangeStatus(order.statusId)) {
      this.alertService.warning('Cannot change status for Shipped or Cancelled orders');
      return;
    }

    if (order.statusId === newStatusId) return;

    this.orderService.updateStatus(order.orderId, newStatusId).subscribe({
      next: () => {
        this.alertService.success('Order status updated successfully');
        this.loadOrders();
      },
      error: (error) => {
        const message = error?.error?.message || 'Error updating order status';
        this.alertService.error(message);
      }
    });
  }

  deleteOrder(order: Order): void {
    this.alertService.deleteConfirm(
      `Are you sure you want to delete order #${order.orderId}?`,
      'Delete Order'
    ).then((result) => {
      if (result.isConfirmed) {
        this.orderService.deleteOrder(order.orderId).subscribe({
          next: () => {
            this.alertService.success('Order deleted successfully');
            this.loadOrders();
          },
          error: (error) => {
            const message = error?.error?.message || 'Error deleting order';
            this.alertService.error(message);
          }
        });
      }
    });
  }

  viewOrderDetails(order: Order): void {
    this.orderService.getOrderDetails(order.orderId).subscribe({
      next: (orderDetails) => {
        this.dialog.open(OrderDetailsDialogComponent, {
          width: '90%',
          maxWidth: '800px',
          data: { orderDetails },
          panelClass: 'order-details-dialog-panel'
        });
      },
      error: () => {
        this.alertService.error('Error loading order details');
      }
    });
  }

  getStatusColor(statusId: number): string {
    const status = this.statuses.find(s => s.id === statusId);
    return status?.color || '#6b7280';
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }
}

