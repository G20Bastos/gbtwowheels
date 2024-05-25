export interface Order {
  orderId?: number;
  userOrderCreationId: number;
  userDeliveryId?: number;
  orderCreationDate: Date;
  addressOrder: string;
  orderServiceValue: number;
  statusOrderId: number;
  orderFinishDate?: Date;
}
