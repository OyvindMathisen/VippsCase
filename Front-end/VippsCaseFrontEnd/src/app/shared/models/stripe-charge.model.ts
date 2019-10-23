import { StripeCustomer } from './stripe-customer.model';

export interface StripeCharge {
    stripeToken: string;
    totalCost: number;
    userId: number;
    cartId: number;
    customerDetails: StripeCustomer;
}
