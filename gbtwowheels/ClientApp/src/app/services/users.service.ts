import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../model/user.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private apiUrl = 'https://localhost/api/user';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }


  createUser(user: User, imageFile: File): Observable<{ success: boolean, message: string, data: User }> {
    const formData = new FormData();
    formData.append('imageFile', imageFile);
    Object.entries(user).forEach(([key, value]) => {
      formData.append(key, value);
    });

    return this.http.post<{ success: boolean, message: string, data: User }>(this.apiUrl + '/AddUser', formData);
  }



}
