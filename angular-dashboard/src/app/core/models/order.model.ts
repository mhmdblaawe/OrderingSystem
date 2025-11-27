export interface Order {
  orderId: number;
  orderDate: string;
  statusId: number;
  statusDesc: string;
  totalAmount: number;
  customerName: string;
  customerPhone: string;
}

export interface OrderPagedResponse {
  totalCount: number;
  items: Order[];
}

export interface UpdateOrderStatus {
  orderId: number;
  statusId: number;
}

export interface CreateOrderItem {
  productId: number;
  quantity: number;
}

export interface CreateOrder {
  customerId: number;
  items: CreateOrderItem[];
}

export interface OrderItem {
  orderItemId: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
}

export interface OrderDetails {
  orderId: number;
  customerId: number;
  customerName: string;
  customerPhone: string;
  customerEmail: string;
  orderDate: string;
  statusId: number;
  totalAmount: number;
  items: OrderItem[];
}

export const ORDER_STATUSES = [
  { id: 1, name: 'Pending', color: '#f59e0b' },
  { id: 2, name: 'Paid', color: '#10b981' },
  { id: 3, name: 'Cancelled', color: '#ef4444' },
  { id: 4, name: 'Shipped', color: '#3b82f6' }
];

