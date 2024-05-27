import { Component, OnInit } from '@angular/core';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { formatDate } from '@angular/common';
import { RentalPlan } from '../model/rental-plan.model';
import { OrderNotification } from '../model/order-notification.model';
import { RentalPlansService } from '../services/rental-plans.service';
import { Rent } from '../model/rent.model';
import { RentsService } from '../services/rents.service';
import { ToastrService } from 'ngx-toastr';
import { MotorcyclesService } from '../services/motorcycles.service';
import { OrderNotificationsService } from '../services/order-notifications.service';
import { OrdersService } from '../services/orders.service';
import { format } from 'date-fns';
import { differenceInDays } from 'date-fns';



@Component({
  selector: 'app-main-user',
  templateUrl: './main-user.component.html',
  styleUrls: ['./main-user.component.css']
})
export class MainUserComponent implements OnInit {
  startDate: Date = new Date();
  possibleEndDate: Date = new Date();
  endDate: Date = new Date();
  userEmail: string = '';
  rentalPlans: RentalPlan[] = [];
  isDisabled: boolean = true;
  selectedRentalPlan: RentalPlan = {
    rentalPlanId: 0,
    numberRentalDays: 0,
    valuePerRentalDay: 0,
    percentageOfFineLowerDate: 0,
    additionalValueLaterDate: 0
  };
  costAllDays: number = 0;
  costByDayNotUsed: number = 0;
  costByAditionalDay: number = 0
  totalCost: number = 0;
  rent: Rent = this.createEmptyRent();
  motorcycleAvailableId: number = 0;
  orderNotifications: OrderNotification[] = [];
  selectedOrderNotification: OrderNotification | null = null;


  constructor(private rentalPlansService: RentalPlansService, private rentsService: RentsService, private toastr: ToastrService, private motorcyclesService: MotorcyclesService, private orderNotificationsService: OrderNotificationsService,
    private ordersService: OrdersService) { }

  ngOnInit() {
    const today = new Date();
    this.startDate = new Date(today.setDate(today.getDate() + 1));
    this.loadRentalPlans();
              }

  loadRentalPlans(): void {
    this.rentalPlansService.getAllRentalPlans().subscribe(
      (plans: RentalPlan[]) => {
        const emptyPlan: RentalPlan = {
          rentalPlanId: 0,
          numberRentalDays: 0,
          valuePerRentalDay: 0,
          percentageOfFineLowerDate: 0,
          additionalValueLaterDate: 0
        };

        this.rentalPlans = [emptyPlan, ...plans];
      },
      (error) => {
        console.error('Erro ao carregar planos de aluguel:', error);
      }
    );
  }

  acceptOrder(notification: OrderNotification) {
    this.selectedOrderNotification = { ...notification };

    this.ordersService.acceptOrder(this.selectedOrderNotification.orderId, parseInt(localStorage.getItem('userId')!, 10)).subscribe(() => {
      
      this.toastr.success('Pedido aceito com sucesso!');
    }, error => {
      this.toastr.error('Erro ao aceitar o pedido.');
    });
  }

  finishOrder(notification: OrderNotification) {
    this.selectedOrderNotification = { ...notification };

    this.ordersService.finishOrder(this.selectedOrderNotification.orderId, parseInt(localStorage.getItem('userId')!, 10)).subscribe(() => {
      
      this.toastr.success('Pedido finalizado com sucesso!');
    }, error => {
      this.toastr.error('Erro ao finalizar o pedido.');
    });
  }

  searchNotifications() {

    this.orderNotificationsService.getAllOrderNotificationByUser(parseInt(localStorage.getItem('userId')!, 10)).subscribe({
      next: (orderNotificationss) => {
        this.orderNotifications = orderNotificationss!;
      },
      error: (error) => {
        this.toastr.error('Sem pedidos disponíveis no momento');
      }
    });
  }

createEmptyRent(): Rent {
  return {

    userId: 0,
    motorcycleId: 0,
    rentalPlanId: 0,
    startRentDate: new Date(),
    expectedEndRentDate: new Date(),
    endRentDate: new Date(),
    costAllDays: 0,
    costByDayNotUsed: 0,
    costByAditionalDay: 0,
    totalCost: 0,

    
  };
}

  onEndDateChange(newDate: string): void {
    const newEndDate = new Date(newDate);
    if (newEndDate < this.startDate) {
      this.endDate = this.startDate;
    } else {
      this.endDate = newEndDate;
    }
    this.compareDatesAndCalculateCosts();
  }


  onPlanSelected(event: Event): void {
    const planId = (event.target as HTMLSelectElement).value;
    const selectedPlan = this.rentalPlans.find(plan => plan.rentalPlanId === Number(planId));
    this.selectedRentalPlan = selectedPlan!;

    const today = new Date();
    this.possibleEndDate = new Date(today.setDate(today.getDate() + this.selectedRentalPlan?.numberRentalDays!));
    this.endDate = this.possibleEndDate;

    //costAllDays
    this.costAllDays = this.selectedRentalPlan.numberRentalDays * this.selectedRentalPlan.valuePerRentalDay

    this.compareDatesAndCalculateCosts();
  }

  compareDatesAndCalculateCosts(): void {

  

    if (this.endDate < this.possibleEndDate) {
      const diffDays = differenceInDays(this.endDate, this.possibleEndDate);
      console.log(diffDays);
      //costByDayNotUsed
      this.costByDayNotUsed = (Math.abs(diffDays) * this.selectedRentalPlan.valuePerRentalDay) * (this.selectedRentalPlan.percentageOfFineLowerDate / 100);
      this.costByAditionalDay = 0;
    

    } else if (this.endDate > this.possibleEndDate) {
      const diffDays = differenceInDays(this.possibleEndDate, this.endDate);
      console.log(diffDays);
      //costByAditionalDay
      this.costByAditionalDay = ((Math.abs(diffDays) + 1) * this.selectedRentalPlan.additionalValueLaterDate)
      this.costByDayNotUsed = 0;


    }

    this.totalCost = this.costAllDays + this.costByDayNotUsed + this.costByAditionalDay; 

  }


  addRent() {
    if (this.selectedRentalPlan.rentalPlanId !== 0) {

      this.getMotorcycleAvailable();

      if (this.motorcycleAvailableId !== 0) {


        this.rent.userId = parseInt(localStorage.getItem('userId')!, 10);
        this.rent.motorcycleId = this.motorcycleAvailableId;
        this.rent.rentalPlanId = this.selectedRentalPlan.rentalPlanId;
        this.rent.startRentDate = this.startDate;
        this.rent.expectedEndRentDate = this.possibleEndDate;
        this.rent.endRentDate = this.endDate;
        this.rent.costAllDays = this.costAllDays;
        this.rent.costByDayNotUsed = this.costByDayNotUsed;
        this.rent.costByAditionalDay = this.costByAditionalDay;
        this.rent.totalCost = this.totalCost;


        this.rentsService.createRent(this.rent).subscribe({
          next: (response) => {
            if (response.success) {
              this.toastr.success(response.message);
            } else {
              this.toastr.error(response.message);
            }
          },
          error: (error) => {
            if (error.error && error.error.message) {
              this.toastr.error(error.error.message);
            } else {
              this.toastr.error('Ocorreu um erro ao realizar a locação.');
            }
            console.error(error);
          }
        });

      }



    } else {
      this.toastr.error('Selecione um plano de locação');
    }
  }



  getMotorcycleAvailable() {

    this.motorcyclesService.getMotorcycleAvailable().subscribe({
      next: (motorcycle) => {
        if (motorcycle) {
          this.motorcycleAvailableId = motorcycle.motorcycleId!;
        } else {
          this.motorcycleAvailableId = 0;
          this.toastr.error('Infelizmente não temos motos disponíveis no momento, por favor, tente mais tarde');
          return;
        }
        
      },
      error: (error) => {
        this.motorcycleAvailableId = 0;
        console.error('Not found');
      }
    });



  }

}
  
  
  
