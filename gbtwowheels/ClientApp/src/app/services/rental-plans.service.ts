import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RentalPlan } from '../model/rental-plan.model';

@Injectable({
  providedIn: 'root'
})
export class RentalPlansService {
  private apiUrl = 'https://localhost/api/rentalplan';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getAllRentalPlans(): Observable<RentalPlan[]> {
    const headers = this.createHeaders();
    return this.http.get<RentalPlan[]>(this.apiUrl, { headers });
  }

}
