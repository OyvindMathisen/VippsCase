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
  total: number;

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

    this.card = elements.create('card', {style, hidePostalCode: true });
  }

  ngOnInit() {
    // Get items here.
    this.cartService.getItem().subscribe((data) => {
      this.items = data;
    });
  }

  createCharge(event: any) {
    // Cart to InProgress
    this.cartService.changeOrderStatus(parseInt(localStorage.getItem('order_id'), 10), 0).subscribe((data) => {
      console.log('Cart to "InProgress"' + data);
    });

    // Resetting the error message from stripe-backend
    this.stripeError = '';

    // NOTE: Multiply the sum with 100, as Stripe calculates from the lowest denominator, which is "øre" in nok.
    const cost = this.total * 100;

    this.stripe.createToken(this.card).then((result) => {
      if (result.error) {
        // Inform the user if there was an error
        this.cardError.textContent = result.error.message;
        this.disablePurchaseButton = false;
      } else {
        // Send the token to the server
        this.stripeTokenHandler(result.token.id, cost, event);
      }
    });
  }

  stripeTokenHandler(token: string, cost: number, customerData: StripeCustomer) {
    // Obtaining values from our localStorage
    const userId = parseInt(localStorage.getItem('user_id'), 10);
    const cartId = parseInt(localStorage.getItem('order_id'), 10);

    // Generates the charge object, adding the values and handing the response.
    const charge = {} as StripeCharge;
    charge.stripeToken = token;
    charge.totalCost = cost;
    charge.userId = isNaN(userId) ? -1 : userId;
    charge.cartId = isNaN(cartId) ? -1 : cartId;
    charge.customerDetails = customerData;

    // Redirects the user to the confirmation page if everything went as expected.
    const order = parseInt(localStorage.getItem('order_id'), 10);
    this.stripeService.addCharge(charge).subscribe(
      (data) => {
        console.log(data);
        // Cart to Accepted
        if (data.successful) {
          this.cartService.changeOrderStatus(order, 1).subscribe();
          this.router.navigate(['/confirmation']);
        } else {
          // Cart to Declined
          this.cartService.changeOrderStatus(order, 2).subscribe();
          this.stripeError = data.errorMessage;
        }
      },
      (error) => {
        console.error(error);
        // Cart to Declined
        this.cartService.changeOrderStatus(order, 0).subscribe();
        this.stripeError = error.message;
      }
    );

    this.disablePurchaseButton = false;
  }

  // Received from the stripe-card-element after initialization for us to handle error messages
  // related to the stripe element on submit.
  storeCard(event: any) {
    this.cardError = event.nativeElement;
  }

  getCart() {
    let userId = 0;

    if (isFinite(parseInt(localStorage.getItem('user_id'), 10))){
      userId = parseInt(localStorage.getItem('user_id'), 10);
    }

    // Get new cart:
    this.cartService.newCart(userId).subscribe((data) => {
      localStorage.setItem('order_id', data.orderId);
      this.items = data.items;

      this.total = 0;

      this.items.forEach((item: any) => {
        this.total += item.price;
      });

      console.log(this.total);
    });
  }
}
