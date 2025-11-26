export interface Product {
  id: number;
  name: string;
  sku: string;
  price: number;
  stockQuantity: number;
}

export interface CreateProduct {
  name: string;
  sku: string;
  price: number;
  stockQuantity: number;
}

export interface UpdateProduct {
  name: string;
  sku: string;
  price: number;
  stockQuantity: number;
}

export interface ProductPagedResponse {
  total: number;
  items: Product[];
}

