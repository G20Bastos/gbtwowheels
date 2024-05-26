import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Order } from '../model/order.model';
import { Motorcycle } from '../model/motorcycle.model';
import { StatusOrder } from '../model/status-order.model';
import { OrdersService } from '../services/orders.service';
import { MotorcyclesService } from '../services/motorcycles.service';
import { MotorcycleFilter } from '../filters/motorcycle.filter';
import { OrderFilter } from '../filters/order.filter';

@Component({
  selector: 'app-main-admin',
  templateUrl: './main-admin.component.html',
  styleUrls: ['./main-admin.component.css']
})
export class MainAdminComponent implements OnInit {
  motos: Motorcycle[] = [];
  filteredMotorcycles: Motorcycle[] = [];
  orders: Order[] = [];
  filteredOrders: Order[] = [];
  years: number[] = [];
  statusList: { key: number; value: string }[] = [];
  statusAvailabeMotoList: { key: boolean; value: string }[] = [];


  moto: Motorcycle = this.createEmptyMoto();
  order: Order = this.createEmptyOrder();
  selectedMoto: Motorcycle | null = null;
  selectedOrder: Order | null = null;
  isMotoCreateModalOpen = false;
  isOrderCreateModalOpen = false;
  filterMoto: MotorcycleFilter = { year: 0, color: '', engineCapacity: 0, licensePlate: '', model: '', isAvailable: true };
  filterOrder: OrderFilter = { startDate: new Date(), endDate: new Date(), addressOrder: '', orderServiceValue: 0, statusOrderId: 1 };

  constructor(
    private motorcyclesService: MotorcyclesService,
    private ordersService: OrdersService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.loadMotorcycles();
    this.loadOrders();
    this.years = this.generateYears();
    this.statusList = [
      { key: 1, value: 'Disponível' },
      { key: 2, value: 'Aceito' },
      { key: 3, value: 'Entregue' }
    ];
    this.statusAvailabeMotoList = [
      { key: false, value: 'Não' },
      { key: true, value: 'Sim' }
    ];
  }

  generateYears(): number[] {
    const currentYear = new Date().getFullYear();
    const years = [];
    for (let year = 1980; year <= currentYear; year++) {
      years.push(year);
    }
    return years;
  }

  getStatusName(statusId: number): string {
    const status = this.statusList.find(option => option.key === statusId);
    return status ? status.value : 'Desconhecido';
  }

  createEmptyMoto(): Motorcycle {
    return {
      year: 0,
      model: '',
      licensePlate: '',
      color: '',
      engineCapacity: '0',
      isAvailable: true
    };
  }

  createEmptyOrder(): Order {
    return {
      userOrderCreationId: parseInt(localStorage.getItem('userId')!, 10),
      addressOrder: '',
      orderServiceValue: 0,
      orderCreationDate: new Date(),
      statusOrderId: 0
    };
  }

  loadMotorcycles() {
    this.motorcyclesService.getAllMotorcycles().subscribe(motos => {
      this.motos = motos;
      this.applyMotoFilters();
    });
  }

  loadOrders() {
    this.ordersService.getAllOrders().subscribe(orders => {
      this.orders = orders;
      this.applyOrderFilters();
    });
  }

  openMotoCreateModal() {
    this.moto = this.createEmptyMoto();
    this.isMotoCreateModalOpen = true;
  }

  closeMotoCreateModal() {
    this.isMotoCreateModalOpen = false;
  }

  openMotoEditModal(moto: Motorcycle) {
    this.selectedMoto = { ...moto };
  }

  closeMotoEditModal() {
    this.selectedMoto = null;
  }


  addMoto() {
    if (this.moto.year && this.moto.model && this.moto.licensePlate && this.moto.color && this.moto.engineCapacity) {
    this.motorcyclesService.createMotorcycle(this.moto).subscribe({
      next: (response) => {
        if (response.success) {
          this.loadMotorcycles();
          this.toastr.success(response.message);
          this.closeMotoCreateModal();
        } else {
          this.toastr.error(response.message);
        }
      },
      error: (error) => {
        if (error.error && error.error.message) {
          this.toastr.error(error.error.message);
        } else {
          this.toastr.error('Ocorreu um erro ao cadastrar a moto.');
        }
        console.error(error);
      }
    });
  
  } else {
  this.toastr.error('Preencha todos os campos');
}
}
  




  saveMoto() {
    if (this.selectedMoto && this.selectedMoto.motorcycleId) {
      this.motorcyclesService.updateMotorcycle(this.selectedMoto).subscribe(() => {
        this.loadMotorcycles();
        this.toastr.success('Moto atualizada com sucesso!');
        this.closeMotoEditModal();
      }, error => {
        this.toastr.error('Erro ao atualizar moto.');
      });
    }
  }


  confirmDeleteMoto(motorcycleId: number) {
    if (confirm('Tem certeza que deseja excluir esta moto?')) {
      this.motorcyclesService.deleteMotorcycle(motorcycleId).subscribe(() => {
        this.loadMotorcycles();
        this.toastr.success('Moto excluída com sucesso!');
      }, error => {
        this.toastr.error('Erro ao excluir moto.');
      });
    }
  }

  applyMotoFilters() {
    const filters: MotorcycleFilter = {
      year: this.filterMoto.year,
      model: this.filterMoto.model,
      licensePlate: this.filterMoto.licensePlate,
      color: this.filterMoto.color,
      engineCapacity: this.filterMoto.engineCapacity,
      isAvailable: this.filterMoto.isAvailable
    };
    this.motorcyclesService.getMotorcyclesByFilter(filters).subscribe({
      next: (motorcycles) => {
        this.filteredMotorcycles = motorcycles;
      },
      error: (error) => {
        this.toastr.error('Resultados não encontrados para o filtro informado.');
      }
    });
  }


  resetMotoFilters() {
    this.filterMoto = { year: 0, color: '', engineCapacity: 0, licensePlate: '', model: '', isAvailable: true };
    this.applyMotoFilters();
  }

  openOrderCreateModal() {
    this.order = this.createEmptyOrder();
    this.isOrderCreateModalOpen = true;
  }

  closeOrderCreateModal() {
    this.isOrderCreateModalOpen = false;
  }

  openOrderEditModal(order: Order) {
    this.selectedOrder = { ...order };
  }

  closeOrderEditModal() {
    this.selectedOrder = null;
  }

  addOrder() {
    this.ordersService.createOrder(this.order).subscribe(() => {
      this.loadOrders();
      this.toastr.success('Pedido adicionado com sucesso!');
      this.closeOrderCreateModal();
    }, error => {
      this.toastr.error('Erro ao adicionar pedido.');
    });
  }

  saveOrder() {
    if (this.selectedOrder && this.selectedOrder.orderId) {
      this.ordersService.updateOrder(this.selectedOrder).subscribe(() => {
        this.loadOrders();
        this.toastr.success('Pedido atualizado com sucesso!');
        this.closeOrderEditModal();
      }, error => {
        this.toastr.error('Erro ao atualizar pedido.');
      });
    }
  }

  confirmDeleteOrder(orderId: number) {
    if (confirm('Tem certeza que deseja excluir este pedido?')) {
      this.ordersService.deleteOrder(orderId).subscribe(() => {
        this.loadOrders();
        this.toastr.success('Pedido excluído com sucesso!');
      }, error => {
        this.toastr.error('Erro ao excluir pedido.');
      });
    }
  }

  applyOrderFilters() {
    const filters: OrderFilter = {
      startDate: this.filterOrder.startDate,
      endDate: this.filterOrder.endDate,
      addressOrder: this.filterOrder.addressOrder,
      orderServiceValue: this.filterOrder.orderServiceValue,
      statusOrderId: this.filterOrder.statusOrderId
    };
    this.ordersService.getOrdersByFilter(filters).subscribe({
      next: (orders) => {
        this.filteredOrders = orders;
      },
      error: (error) => {
        this.toastr.error('Resultados não encontrados para o filtro informado.');
      }
    });
  }

  resetOrderFilters() {
    this.filterOrder = { startDate: new Date(), endDate: new Date(), addressOrder: '', orderServiceValue: 0, statusOrderId: 1 };
    this.applyOrderFilters();
  }
}

