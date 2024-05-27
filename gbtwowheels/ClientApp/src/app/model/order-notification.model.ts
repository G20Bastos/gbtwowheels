import { User } from '../model/user.model';

export interface OrderNotification {
orderNotificationId: number,
orderId: number,
userId: number,
message: string,
createdAt: Date,
user?: User
}
