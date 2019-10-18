import { Component, Input } from '@angular/core';
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
export class PurchasePageComponent {
  // Inputs
  @Input() purchaseDetails: any;

  // Local Variables
  stripe: stripe.Stripe;
  cardError: any;
  card: any;
  items: any; 

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
    // TODO: Replace this with the totalCost from the shoppingCart
    // NOTE: Multiply the sum with 100, as Stripe calculates from the lowest denominator, which is "øre" in nok.
    const cost = 1000 * 100;

    this.stripe.createToken(this.card).then((result) => {
      if (result.error) {
        // Inform the user if there was an error
        this.cardError.textContent = result.error.message;
      } else {
        // Send the token to the server
        this.stripeTokenHandler(result.token.id, cost, event);
      }
    });
  }

  stripeTokenHandler(token: string, cost: number, customerData: any) {
    // Generates the charge object, adding the values and handing the response.
    const charge = {} as StripeCharge;
    charge.stripeToken = token;
    charge.totalCost = cost;
    charge.customerDetails = customerData;

    // Redirects the user to the confirmation page if everything went as expected.
    this.stripeService.addCharge(charge).subscribe(
      (data) => {
        console.log(data);

        if (data.successful) {
          this.router.navigate(['/confirmation']);
        }
      },
      (error) => {
        console.log(error);
      }
    );
  }

  // Received from the stripe-card-element after initialization for us to handle error messages
  // related to the stripe element on submit.
  storeCard(event: any) {
    this.cardError = event.nativeElement;
  }
}
