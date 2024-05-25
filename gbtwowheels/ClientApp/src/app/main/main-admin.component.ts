import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Motorcycle } from '../model/motorcycle.model';
import { Order } from '../model/order.model';

@Component({
  selector: 'app-main-admin',
  templateUrl: './main-admin.component.html',
  styleUrls: ['./main-admin.component.css']
})
export class MainAdminComponent implements OnInit {

  apiMotocycleUrl: string = 'https://localhost:7296/api/motorcycle';
  apiOrderUrl: string = 'https://localhost:7296/api/order';

  motorcycles: Motorcycle[] = [];
  orders: Order[] = [];

  moto: Motorcycle = { year: 2024, model: '', licensePlate: '', color: '', engineCapacity: '' };
  order: Order = {
    addressOrder: '', orderServiceValue: 0, orderCreationDate: new Date()
};

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
      this.fetchMotorcycles();
      this.fetchOrders();
    }

  fetchMotorcycles() {
    this.http.get<Motorcycle[]>(this.apiMotocycleUrl)
        .subscribe(motos => this.motorcycles = motos);
    }

  fetchOrders() {
    this.http.get<Order[]>(this.apiOrderUrl)
        .subscribe(orders => this.orders = orders);
    }

  addMoto() {
    this.http.post(this.apiMotocycleUrl + '/addMotorcycle', this.moto)
        .subscribe(() => {
          this.fetchMotorcycles();
          this.moto = { year: 2024, model: '', licensePlate: '', color: '', engineCapacity: '' };
        });
    }

  editMoto(moto: Motorcycle) {
    this.http.put(this.apiMotocycleUrl + `/${moto.id}`, moto)
        .subscribe(() => {
          this.fetchMotorcycles();
        });
    }

  deleteMoto(id: number) {
    this.http.delete(this.apiMotocycleUrl + `/${id}`)
        .subscribe(() => {
          this.fetchMotorcycles();
        });
    }

  addOrder() {
    this.http.post(this.apiOrderUrl, this.order)
        .subscribe(() => {
          this.fetchOrders();
          this.order = { addressOrder: '', orderServiceValue: 0, orderCreationDate: new Date() };
        });
    }


  editOrder(order: Order) {
    this.http.put(this.apiOrderUrl + `/${order.id}`, order)
        .subscribe(() => {
          this.fetchOrders();
        });
    }

  deleteOrder(id: number) {
    this.http.delete(this.apiOrderUrl + `/${id}`)
        .subscribe(() => {
          this.fetchOrders();
        });
  }
  
}


