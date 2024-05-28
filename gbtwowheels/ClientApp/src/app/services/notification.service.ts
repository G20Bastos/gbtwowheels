import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'https://localhost/api/notifications';

  constructor(private http: HttpClient) { }

  getNotifications(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
