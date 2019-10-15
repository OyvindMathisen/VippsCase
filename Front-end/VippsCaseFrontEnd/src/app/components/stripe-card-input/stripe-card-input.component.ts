import { Component, OnInit } from '@angular/core';
import { StripeService } from '../../services/stripe.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-stripe-card-input',
  templateUrl: './stripe-card-input.component.html',
  styleUrls: ['./stripe-card-input.component.scss']
})
export class StripeCardInputComponent implements OnInit {
  private card: stripe.elements.Element;

  constructor(private stripeService: StripeService) {
    // Stripe content to let us create the input
    const stripe = Stripe(environment.stripeKey);
    const elements = stripe.elements();
    this.card = elements.create('card', {hidePostalCode: true});

    // Display error handling to the user
    this.card.on('change', (event) => {
      const displayError = document.getElementById('card-errors');
      if (event.error) {
        displayError.textContent = event.error.message;
      } else {
        displayError.textContent = '';
      }
    });
  }

  ngOnInit() {
    // Add the card to the component
    this.card.mount('#card-element');
  }

}
