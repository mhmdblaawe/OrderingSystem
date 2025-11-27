import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface TableColumn {
  key: string;
  label: string;
  sortable?: boolean;
  width?: string;
  align?: 'left' | 'center' | 'right';
  template?: TemplateRef<any>;
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-table.component.html',
  styleUrl: './data-table.component.scss'
})
export class DataTableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() loading: boolean = false;
  @Input() totalItems: number = 0;
  @Input() pageSize: number = 10;
  @Input() pageIndex: number = 0;
  @Input() pageSizeOptions: number[] = [5, 10, 20, 50];
  @Input() emptyMessage: string = 'No data available';

  @Output() pageChange = new EventEmitter<{ pageIndex: number; pageSize: number }>();
  @Output() sortChange = new EventEmitter<{ column: string; direction: 'asc' | 'desc' }>();

  sortColumn: string = '';
  sortDirection: 'asc' | 'desc' | '' = '';

  onSort(column: TableColumn): void {
    if (!column.sortable) return;

    if (this.sortColumn === column.key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column.key;
      this.sortDirection = 'asc';
    }

    this.sortChange.emit({ column: this.sortColumn, direction: this.sortDirection });
  }

  onPageChange(pageIndex: number): void {
    this.pageIndex = pageIndex;
    this.pageChange.emit({ pageIndex, pageSize: this.pageSize });
  }

  onPageSizeChange(pageSize: number): void {
    this.pageSize = pageSize;
    this.pageIndex = 0;
    this.pageChange.emit({ pageIndex: 0, pageSize });
  }

  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  get startIndex(): number {
    return this.pageIndex * this.pageSize + 1;
  }

  get endIndex(): number {
    return Math.min((this.pageIndex + 1) * this.pageSize, this.totalItems);
  }

  getPageNumbers(): (number | string)[] {
    const total = this.totalPages;
    const current = this.pageIndex + 1;
    const pages: (number | string)[] = [];

    if (total <= 7) {
      for (let i = 1; i <= total; i++) {
        pages.push(i);
      }
    } else {
      if (current <= 3) {
        for (let i = 1; i <= 4; i++) pages.push(i);
        pages.push('...');
        pages.push(total);
      } else if (current >= total - 2) {
        pages.push(1);
        pages.push('...');
        for (let i = total - 3; i <= total; i++) pages.push(i);
      } else {
        pages.push(1);
        pages.push('...');
        for (let i = current - 1; i <= current + 1; i++) pages.push(i);
        pages.push('...');
        pages.push(total);
      }
    }

    return pages;
  }

  isPageNumber(page: number | string): page is number {
    return typeof page === 'number';
  }

  getPageNumber(page: number | string): number {
    return typeof page === 'number' ? page : 0;
  }

  isActivePage(page: number | string): boolean {
    if (typeof page === 'number') {
      return this.pageIndex === page - 1;
    }
    return false;
  }

  goToPage(page: number | string): void {
    if (typeof page === 'number') {
      this.onPageChange(page - 1);
    }
  }
}

