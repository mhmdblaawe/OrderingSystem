import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Order, OrderPagedResponse, UpdateOrderStatus, CreateOrder, OrderDetails } from '../models/order.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getOrdersPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    customerId?: number,
    status?: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<OrderPagedResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (customerId) params = params.set('customerId', customerId.toString());
    if (status) params = params.set('status', status.toString());
    if (startDate) params = params.set('startDate', startDate.toISOString());
    if (endDate) params = params.set('endDate', endDate.toISOString());

    return this.http.get<{ TotalCount?: number; Items?: any[]; totalCount?: number; items?: any[] }>(`${this.apiUrl}/api/orders/GetOrdersPaged`, { params })
      .pipe(
        map(response => {
          // Handle case where response or Items might be undefined
          if (!response) {
            console.warn('Order service: Empty response received');
            return { totalCount: 0, items: [] };
          }

          const items = response.Items || response.items || [];
          const totalCount = response.TotalCount || response.totalCount || 0;

          return {
            totalCount: totalCount,
            items: items.map(item => ({
              orderId: item.OrderId || item.orderId,
              orderDate: item.OrderDate || item.orderDate,
              statusId: item.StatusId || item.statusId,
              statusDesc: item.StatusDesc || item.statusDesc,
              totalAmount: item.TotalAmount || item.totalAmount,
              customerName: item.CustomerName || item.customerName,
              customerPhone: item.CustomerPhone || item.customerPhone
            }))
          };
        }),
        catchError(error => {
          console.error('Order service error:', error);
          // Return empty response on error
          return of({ totalCount: 0, items: [] });
        })
      );
  }

  getOrderDetails(id: number): Observable<OrderDetails> {
    return this.http.get<any>(`${this.apiUrl}/api/orders/GetOrderDetails/${id}`).pipe(
      map(response => ({
        orderId: response.orderId || response.OrderId,
        customerId: response.customerId || response.CustomerId,
        customerName: response.customerName || response.CustomerName || '',
        customerPhone: response.customerPhone || response.CustomerPhone || '',
        customerEmail: response.customerEmail || response.CustomerEmail || '',
        orderDate: response.orderDate || response.OrderDate,
        statusId: response.statusId || response.StatusId,
        totalAmount: response.totalAmount || response.TotalAmount,
        items: (response.items || response.Items || []).map((item: any) => ({
          orderItemId: item.orderItemId || item.OrderItemId,
          productId: item.productId || item.ProductId,
          productName: item.productName || item.ProductName || '',
          quantity: item.quantity || item.Quantity,
          unitPrice: item.unitPrice || item.UnitPrice,
          lineTotal: item.lineTotal || item.LineTotal
        }))
      })),
      catchError(error => {
        console.error('Error fetching order details:', error);
        throw error;
      })
    );
  }

  createOrder(order: CreateOrder): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/orders/CreateOrder`, order);
  }

  updateStatus(orderId: number, statusId: number): Observable<any> {
    const dto: UpdateOrderStatus = { orderId, statusId };
    return this.http.put(`${this.apiUrl}/api/orders/UpdateStatus/${orderId}`, dto);
  }

  deleteOrder(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/orders/${id}`);
  }
}

