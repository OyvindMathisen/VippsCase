import { Component, OnInit, Input, Output, ViewChild, ElementRef, EventEmitter, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-stripe-card-input',
  templateUrl: './stripe-card-input.component.html',
  styleUrls: ['./stripe-card-input.component.scss']
})
export class StripeCardInputComponent implements OnInit, AfterViewInit {
  @Input() card: any;
  @Output() cardInitialized: EventEmitter<any>;

  // Equivalent of using document.getElement()
  @ViewChild('cardErrors', {static: false}) cardErrors: ElementRef;

  constructor() {
    this.cardInitialized = new EventEmitter();
  }

  ngOnInit() {
    // Display error handling to the user
    this.card.on('change', (event: any) => {
      // Remember to use nativeElement to get the DOM element
      const displayError = this.cardErrors.nativeElement;

      // Error check!
      if (event.error) {
        displayError.textContent = event.error.message;
      } else {
        displayError.textContent = '';
      }
    });

    // Add the card to the component
    this.card.mount('#card-element');
  }

  // Send the element to the parent for access to the cardErrors div, if other errors occures.
  ngAfterViewInit() {
    this.cardInitialized.emit(this.cardErrors);
  }
}