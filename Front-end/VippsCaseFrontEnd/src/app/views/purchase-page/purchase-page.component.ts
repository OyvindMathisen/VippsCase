import { Component, Input, OnInit } from '@angular/core';
import { StripeService } from 'src/app/services/stripe.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { StripeCharge } from 'src/app/shared/models/stripe-charge.model';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-purchase-page',
  templateUrl: './purchase-page.component.html',
  styleUrls: ['./purchase-page.component.scss']
})
export class PurchasePageComponent implements OnInit {
  // Inputs
  @Input() purchaseDetails: any;

  // Local Variables
  stripe: stripe.Stripe;
  stripeError: string;
  cardError: any;
  card: any;
  items: any;
  disablePurchaseButton: boolean;

  constructor(private stripeService: StripeService, private router: Router, private cartService: CartService) {
    // Stripe Init
    this.stripe = Stripe(environment.stripeKey);
    const elements = this.stripe.elements();
    // Stripe Card Init
    const style = {
      base: {
        color: '#000000',
        fontFamily: '"Trebuchet MS", "Lucida Sans Unicode", "Lucida Grande", "Lucida Sans", Arial, sans-serif',
        fontSmoothing: 'antialiased',
        iconColor: '#000000',
        fontSize: '16px', '::placeholder': {
          color: '#c4c4c4'
        },
      },
      invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
      }
    };

    this.card = elements.create('card', { style, hidePostalCode: true });
  }

  ngOnInit() {
    // Get items here.
    this.cartService.getItem().subscribe((data) => {
      this.items = data;
    });
  }

  async createCharge(event: any) {
    // Resetting the error message from stripe-backend
    this.stripeError = '';

    // TODO: Replace this with the totalCost from the shoppingCart
    // NOTE: Multiply the sum with 100, as Stripe calculates from the lowest denominator, which is "øre" in nok.
    const inputCost = 1000;
    const cost = inputCost * 100;

    const { paymentMethod } = await this.stripe.createPaymentMethod('card', this.card);
    const userId = parseInt(localStorage.getItem('user_id'), 10);
    const cartId = parseInt(localStorage.getItem('order_id'), 10);

    const charge = {} as StripeCharge;
    charge.paymentMethodId = paymentMethod.id;
    charge.totalCost = cost;
    charge.userId = isNaN(userId) ? -1 : userId;
    charge.cartId = isNaN(cartId) ? -1 : cartId;
    charge.customerDetails = event;

    try {
      const response = await this.stripeService.addCharge(charge);
      console.log(response);
      // this.handleServerResponse(response, cartId);
    } catch (error) {
      // 'We were unable to contact our server to make the transaction. Please try again later.'
      this.handleErrorResponse(error, 0, cartId);
    }
  }

  async handleServerResponse(response: any, cartId: number) {
    if (response.error) {
      // Show error from server on payment form
      this.handleErrorResponse(response.error, 2, cartId);
    } else if (response.data != null && response.data.requires_action) {
      await this.handleAction(response.data);
    } else {
      this.cartService.changeOrderStatus(cartId, 1).subscribe();
      this.router.navigate(['/confirmation']);
    }
  }

  async handleAction(input: any) {
    const { error: errorAction, paymentIntent } = await this.stripe.handleCardAction(input.payment_intent_client_secret);
    const userId = parseInt(localStorage.getItem('user_id'), 10);
    const cartId = parseInt(localStorage.getItem('order_id'), 10);

    const charge = {} as StripeCharge;
    charge.paymentIntentId = paymentIntent.id;
    charge.userId = userId;
    charge.cartId = cartId;

    if (errorAction) {
      // Show error from Stripe.js in payment
      this.handleErrorResponse(errorAction, 2, cartId);
    } else {
      try {
        const resp: any = await this.stripeService.addCharge(charge);
        this.handleServerResponse(resp, cartId);
      } catch (error) {
        this.handleErrorResponse(error, 0, cartId);
      }

    }
  }

  handleErrorResponse(error: any, cartState: number, cartId = -1) {
    if (cartId !== -1) {
      this.cartService.changeOrderStatus(cartId, cartState).subscribe();
    }
    this.stripeError = error.message;
    this.disablePurchaseButton = false;
  }
}
