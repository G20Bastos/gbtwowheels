import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Motorcycle } from '../model/motorcycle.model';
import { Order } from '../model/order.model';
import { ToastrService } from 'ngx-toastr';

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
  years: number[] = [];

  moto: Motorcycle = { year: 2024, model: '', licensePlate: '', color: '', engineCapacity: '', isAvailable: true };
  order: Order = { addressOrder: '', userOrderCreationId: parseInt(localStorage.getItem('userId')!, 10), orderServiceValue: 0, orderCreationDate: new Date(), statusOrderId: 1, userDeliveryId: undefined, orderFinishDate: undefined };

  selectedMoto: Motorcycle | null = null;
  selectedOrder: Order | null = null;

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.fetchMotorcycles();
    this.fetchOrders();
    this.populateYears();
    this.moto.year = new Date().getFullYear();
  }

  populateYears(): void {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= 1990; year--) {
      this.years.push(year);
    }
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  createHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  fetchMotorcycles() {
    const headers = this.createHeaders();
    this.http.get<Motorcycle[]>(this.apiMotocycleUrl, { headers })
      .subscribe(motos => this.motorcycles = motos);
  }

  fetchOrders() {
    const headers = this.createHeaders();
    this.http.get<Order[]>(this.apiOrderUrl, { headers })
      .subscribe(orders => this.orders = orders);
  }

  addMoto() {
    const headers = this.createHeaders();
    if (this.moto.year && this.moto.model && this.moto.licensePlate && this.moto.color && this.moto.engineCapacity) {
      this.http.post(this.apiMotocycleUrl + '/addMotorcycle', this.moto, { headers })
        .subscribe(() => {
          this.fetchMotorcycles();
          this.moto = { year: 2024, model: '', licensePlate: '', color: '', engineCapacity: '', isAvailable: true };
          this.toastr.success('Moto cadastrada com sucesso');
        });
    } else {
      this.toastr.error('Preencha todos os campos');
    }
  }

  addOrder() {
    const headers = this.createHeaders();
    if (this.order.addressOrder && this.order.orderServiceValue) {
      this.http.post(this.apiOrderUrl + '/addOrder', this.order, { headers })
        .subscribe(() => {
          this.fetchOrders();
          this.order = { addressOrder: '', orderServiceValue: 0, orderCreationDate: new Date(), userOrderCreationId: parseInt(localStorage.getItem('userId')!, 10), statusOrderId: 1, userDeliveryId: undefined, orderFinishDate: undefined };
          this.toastr.success('Pedido cadastrado com sucesso');
        });
    } else {
      this.toastr.error('Preencha todos os campos');
    }
  }

  openMotoModal(moto: Motorcycle) {
    this.selectedMoto = { ...moto };
  }

  closeMotoModal() {
    this.selectedMoto = null;
  }

  saveMotoChanges() {
    const headers = this.createHeaders();
    if (this.selectedMoto && this.selectedMoto.year && this.selectedMoto.model && this.selectedMoto.licensePlate && this.selectedMoto.color && this.selectedMoto.engineCapacity) {
      this.editMoto(this.selectedMoto, headers);
      this.closeMotoModal();
      this.toastr.success('Alterações salvas com sucesso');
    } else {
      this.toastr.error('Preencha todos os campos');
    }
  }

  confirmDeleteMoto(id: number) {
    if (confirm('Deseja excluir o registro?')) {
      this.deleteMoto(id);
      this.closeMotoModal();
    }
  }

  openOrderModal(order: Order) {
    this.selectedOrder = { ...order };
  }

  closeOrderModal() {
    this.selectedOrder = null;
  }

  saveOrderChanges() {
    const headers = this.createHeaders();
    if (this.selectedOrder && this.selectedOrder.addressOrder && this.selectedOrder.orderServiceValue) {
      this.editOrder(this.selectedOrder, headers);
      this.closeOrderModal();
      this.toastr.success('Alterações salvas com sucesso');
    } else {
      this.toastr.error('Preencha todos os campos');
    }
  }

  confirmDeleteOrder(id: number) {
    if (confirm('Deseja excluir o registro?')) {
      this.deleteOrder(id);
    }
  }

  editMoto(moto: Motorcycle, headers: HttpHeaders) {
    this.http.put(this.apiMotocycleUrl + `/${moto.motorcycleId}`, moto, { headers })
      .subscribe(() => {
        this.fetchMotorcycles();
      });
  }

  deleteMoto(id: number) {
    const headers = this.createHeaders();
    this.http.delete(this.apiMotocycleUrl + `/${id}`, { headers })
      .subscribe(() => {
        this.fetchMotorcycles();
      });
  }

  editOrder(order: Order, headers: HttpHeaders) {
    this.http.put(this.apiOrderUrl + `/${order.orderId}`, order, { headers })
      .subscribe(() => {
        this.fetchOrders();
      });
  }

  deleteOrder(id: number) {
    const headers = this.createHeaders();
    this.http.delete(this.apiOrderUrl + `/${id}`, { headers })
      .subscribe(() => {
        this.fetchOrders();
      });
  }
}
