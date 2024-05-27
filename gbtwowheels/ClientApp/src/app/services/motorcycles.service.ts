import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Motorcycle } from '../model/motorcycle.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MotorcycleFilter } from '../filters/motorcycle.filter';

@Injectable({
  providedIn: 'root'
})
export class MotorcyclesService {
  private apiUrl = 'https://localhost:7296/api/motorcycle';

  constructor(private http: HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }


  getAllMotorcycles(): Observable<Motorcycle[]> {
    const headers = this.createHeaders();
    return this.http.get<Motorcycle[]>(this.apiUrl, { headers });
  }

  createMotorcycle(moto: Motorcycle): Observable<{ success: boolean, message: string, data: Motorcycle }> {
    const headers = this.createHeaders();
    return this.http.post<{ success: boolean, message: string, data: Motorcycle }>(this.apiUrl + '/addMotorcycle', moto, { headers });
  }

  updateMotorcycle(moto: Motorcycle): Observable<{ success: boolean, message: string, data: Motorcycle }> {
    return this.http.put<{ success: boolean, message: string, data: Motorcycle }>(`${this.apiUrl}/${moto.motorcycleId}`, moto);
  }

  deleteMotorcycle(motorcycleId: number): Observable<{ success: boolean, message: string, data: Motorcycle }>  {
    return this.http.delete<{ success: boolean, message: string, data: Motorcycle }>(`${this.apiUrl}/${motorcycleId}`);
  }

  getMotorcyclesByFilter(filters: MotorcycleFilter): Observable<Motorcycle[]> {
    const headers = this.createHeaders();
    return this.http.post<Motorcycle[]>(`${this.apiUrl}/getByFilter`, filters, { headers });
  }

  getMotorcycleAvailable(): Observable<Motorcycle> {
    const headers = this.createHeaders();
    return this.http.get<Motorcycle>(`${this.apiUrl}/getMotorcycleAvailable`, { headers });
  }


}
