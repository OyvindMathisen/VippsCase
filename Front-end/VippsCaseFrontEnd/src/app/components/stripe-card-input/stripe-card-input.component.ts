import { Component, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'app-stripe-card-input',
  templateUrl: './stripe-card-input.component.html',
  styleUrls: ['./stripe-card-input.component.scss']
})
export class StripeCardInputComponent implements OnInit {
  @Input() stripe: stripe.Stripe;
  @Input() @Output() card: any;

  constructor() {
  }

  ngOnInit() {
    // Display error handling to the user
    this.card.on('change', (event) => {
      const displayError = document.getElementById('card-errors');
      if (event.error) {
        displayError.textContent = event.error.message;
      } else {
        displayError.textContent = '';
      }
    });

    // Add the card to the component
    this.card.mount('#card-element');
  }
}
