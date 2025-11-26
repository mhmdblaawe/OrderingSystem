import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product, CreateProduct, UpdateProduct, ProductPagedResponse } from '../models/product.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getProductsPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    search?: string,
    name?: string,
    sku?: string
  ): Observable<ProductPagedResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (search) params = params.set('search', search);
    if (name) params = params.set('name', name);
    if (sku) params = params.set('sku', sku);

    return this.http.get<ProductPagedResponse>(`${this.apiUrl}/api/products/GetProductsPaged`, { params });
  }

  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/api/products/GetProductById/${id}`);
  }

  createProduct(product: CreateProduct): Observable<Product> {
    return this.http.post<Product>(`${this.apiUrl}/api/products/CreateProduct`, product);
  }

  updateProduct(id: number, product: UpdateProduct): Observable<Product> {
    return this.http.put<Product>(`${this.apiUrl}/api/products/UpdateProduct/${id}`, product);
  }

  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/products/DeleteProduct/${id}`);
  }
}

