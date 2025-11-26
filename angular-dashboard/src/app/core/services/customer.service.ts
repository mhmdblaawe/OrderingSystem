import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer, CreateCustomer, CustomerPagedResponse } from '../models/customer.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getCustomersPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    name?: string,
    email?: string
  ): Observable<CustomerPagedResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (name) params = params.set('name', name);
    if (email) params = params.set('email', email);

    return this.http.get<CustomerPagedResponse>(`${this.apiUrl}/api/customers/GetCustomersPaged`, { params });
  }

  getCustomerById(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${this.apiUrl}/api/customers/GetCustomerById/${id}`);
  }

  createCustomer(customer: CreateCustomer): Observable<Customer> {
    return this.http.post<Customer>(`${this.apiUrl}/api/customers/CreateCustomer`, customer);
  }

  deleteCustomer(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/api/customers/DeleteCustomer/${id}`);
  }
}

