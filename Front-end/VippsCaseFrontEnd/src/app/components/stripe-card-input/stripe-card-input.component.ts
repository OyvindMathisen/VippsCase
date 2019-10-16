import { Component, OnInit, Input, Output, ViewChild, ElementRef, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-stripe-card-input',
  templateUrl: './stripe-card-input.component.html',
  styleUrls: ['./stripe-card-input.component.scss']
})
export class StripeCardInputComponent implements OnInit {
  @Input() stripe: stripe.Stripe;
  @Input() card: any;
  @Output() cardChanged: EventEmitter<any>;
  @Output() errorContainer: EventEmitter<any>;

  // Equivalent of using document.getElement()
  @ViewChild('cardErrors', {static: false}) cardErrors: ElementRef;

  constructor() {
    this.cardChanged = new EventEmitter();
  }

  ngOnInit() {
    // Display error handling to the user
    this.card.on('change', (event: any) => {
      // Remember to use nativeElement to get the DOM element
      const displayError = this.cardErrors.nativeElement;
      if (event.error) {
        displayError.textContent = event.error.message;
      } else {
        displayError.textContent = '';
      }
      // Sending details to our parent object to ensure the content is as up-to-date as possible.
      this.cardChanged.emit({card: this.card, cardErrors: this.cardErrors});
    });

    // Add the card to the component
    this.card.mount('#card-element');
  }
}
