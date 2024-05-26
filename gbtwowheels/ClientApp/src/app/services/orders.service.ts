import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../model/order.model';
import { OrderFilter } from '../filters/order.filter';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl = 'https://localhost:7296/api/order';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getAllOrders(): Observable<Order[]> {
    const headers = this.createHeaders();
    return this.http.get<Order[]>(this.apiUrl, { headers });
  }

  createOrder(order: Order): Observable<Order> {
    const headers = this.createHeaders();
    return this.http.post<Order>(this.apiUrl + '/addOrder', order, { headers });
  }

  updateOrder(order: Order): Observable<void> {
    const headers = this.createHeaders();
    return this.http.put<void>(`${this.apiUrl}/${order.orderId}`, order, { headers });
  }

  deleteOrder(orderId: number): Observable<void> {
    const headers = this.createHeaders();
    return this.http.delete<void>(`${this.apiUrl}/${orderId}`, { headers });
  }

  getOrdersByFilter(filters: OrderFilter): Observable<Order[]> {
    const headers = this.createHeaders();
    return this.http.post<Order[]>(`${this.apiUrl}/getByFilter`, filters, { headers });
  }
}
