export interface Customer {
  id: number;
  name: string;
  email: string;
  phone: string;
}

export interface CreateCustomer {
  name: string;
  email: string;
  phone: string;
}

export interface CustomerPagedResponse {
  total: number;
  items: Customer[];
}

