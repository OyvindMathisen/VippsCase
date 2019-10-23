import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { CartService } from 'src/app/services/cart.service';

@Component({
  selector: 'app-purchase-page',
  templateUrl: './purchase-page.component.html',
  styleUrls: ['./purchase-page.component.scss']
})
export class PurchasePageComponent implements OnInit {
  // Local Variables
  stripe: stripe.Stripe;
  card: any;
  items: any;

  constructor(private router: Router, private cartService: CartService) {
    this.initStripeElements();
  }

  ngOnInit() {
    // Get items here.
    this.cartService.getItem().subscribe((data) => {
      this.items = data;
    });
  }

  initStripeElements() {
    // Stripe Init
    this.stripe = Stripe(environment.stripeKey);

    // Stripe Elements Init
    const elements = this.stripe.elements();

    // Stripe Card Style Init
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

    // Stripe Card Init
    this.card = elements.create('card', { style, hidePostalCode: true });
  }

  onPurchaseSuccess(event: any) {
    this.router.navigate(['/confirmation']);
  }
}
