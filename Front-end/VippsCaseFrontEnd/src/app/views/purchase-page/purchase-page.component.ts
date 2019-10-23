import { Component, Input, OnInit } from '@angular/core';
import { StripeService } from 'src/app/services/stripe.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { StripeCharge } from 'src/app/shared/models/stripe-charge.model';
import { CartService } from 'src/app/services/cart.service';
import { StripeCustomer } from 'src/app/shared/models/stripe-customer.model';

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
    // Cart to InProgress
    this.cartService.changeOrderStatus(parseInt(localStorage.getItem('order_id'), 10), 0).subscribe((data) => {
      console.log('Cart to "InProgress"' + data);
    });

    // Resetting the error message from stripe-backend
    this.stripeError = '';

    // TODO: Replace this with the totalCost from the shoppingCart
    // NOTE: Multiply the sum with 100, as Stripe calculates from the lowest denominator, which is "øre" in nok.
    const cost = 1000 * 100;

    const { paymentMethod } = await this.stripe.createPaymentMethod('card', this.card);
    const userId = parseInt(localStorage.getItem('user_id'), 10);
    const cartId = parseInt(localStorage.getItem('order_id'), 10);

    const charge = {} as StripeCharge;
    charge.paymentMethodId = paymentMethod.id;
    charge.totalCost = cost;
    charge.userId = isNaN(userId) ? -1 : userId;
    charge.cartId = isNaN(cartId) ? -1 : cartId;
    charge.customerDetails = event;

    this.stripeService.addCharge(charge).subscribe(
      (response) => {
        this.handleServerResponse(response);
      },
      (error) => {
        // TODO: Replace with generic "Unable to conact our server" message
        this.stripeError = error.message;
      }
    );
  }

  async handleServerResponse(response: any) {
    console.log(response);
    if (response.error) {
      // Show error from server on payment form
      this.stripeError = response.error;
    } else if (response.data != null && response.data.requires_action) {
      await this.handleAction(response.data);
    } else {
      // Show success message
      alert('Success!');
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
      this.stripeError = errorAction.message;
    } else {
      this.stripeService.addCharge(charge).subscribe(
        (response: any) => {
          this.handleServerResponse(response);
        },
        (error: any) => {
          this.stripeError = error.message;
        }
      );
    }
  }

  // Received from the stripe-card-element after initialization for us to handle error messages
  // related to the stripe element on submit.
  storeCard(event: any) {
    this.cardError = event.nativeElement;
  }
}
