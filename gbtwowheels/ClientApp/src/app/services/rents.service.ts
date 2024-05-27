import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Rent } from '../model/rent.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RentsService {
  private apiUrl = 'https://localhost:7296/api/rent';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }


  getAllRents(): Observable<Rent[]> {
    const headers = this.createHeaders();
    return this.http.get<Rent[]>(this.apiUrl, { headers });
  }

  createRent(moto: Rent): Observable<{ success: boolean, message: string, data: Rent }> {
    const headers = this.createHeaders();
    return this.http.post<{ success: boolean, message: string, data: Rent }>(this.apiUrl + '/addRent', moto, { headers });
  }

  


}
