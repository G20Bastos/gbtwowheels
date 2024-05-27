export interface Rent {
  userId: number;
  motorcycleId: number;
  rentalPlanId: number;
  startRentDate?: Date;
  expectedEndRentDate?: Date;
  endRentDate?: Date;
  costAllDays: number;
  costByDayNotUsed: number; 
  costByAditionalDay: number; 
  totalCost: number; 
}
