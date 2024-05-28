import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderNotification } from '../model/order-notification.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OrderNotificationsService {
  private apiUrl = 'https://localhost/api/orderNotification';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }


  getAllOrderNotificationByUser(orderNotificationId: number): Observable<OrderNotification[]> {
    const headers = this.createHeaders();
    return this.http.get<OrderNotification[]>(`${this.apiUrl}/${orderNotificationId}`, { headers });
  }

  getAllOrderNotification(): Observable<OrderNotification[]> {
    const headers = this.createHeaders();
    return this.http.get<OrderNotification[]>(`${this.apiUrl}/getAllOrderNotification`, { headers });
  }

 

}
