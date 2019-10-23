import { Component, OnInit, Input, Output, EventEmitter, ElementRef, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-person-information',
  templateUrl: './person-information.component.html',
  styleUrls: ['./person-information.component.scss']
})
export class PersonInformationComponent implements OnInit {
  // Inputs
  @Input() card: any;
  @Input() stripe: stripe.Stripe;
  @Input() stripeError: string;
  @Input() disablePurchaseButton: boolean;

  // Outputs
  @Output() confirmPurchaseDetails: EventEmitter<any>;

  // Equivalent of using document.getElement()
  @ViewChild('cardErrors', { static: false }) cardErrors: ElementRef;

  // Local Variables
  personDetails: FormGroup;

  constructor() {
    this.confirmPurchaseDetails = new EventEmitter();
  }

  ngOnInit() {
    // Initializing the FormGroup for input and validation.
    this.initForm();

    // Initializing the Stripe card element
    this.initStripe();

    // Initializing user details in the form if we're signed in
    const userId: number = parseInt(localStorage.getItem('user_id'), 10);
    const token: string = localStorage.getItem('id_token');

    if (isFinite(userId)) {
      this.initUserDetails(token);
    }
  }

  onPurchaseClicked() {
    // Check if the stripe component has been filled out fully!

    // Everything is okay, we'll submit the data!
    this.disablePurchaseButton = true;

    // Server responded okay, we'll tell our parent this!
    this.confirmPurchaseDetails.emit(this.personDetails.value);
  }

  initForm() {
    this.personDetails = new FormGroup({
      fullName: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
      addressLineOne: new FormControl('', [Validators.required]),
      addressLineTwo: new FormControl(''),
      county: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
      postalCode: new FormControl('', [Validators.required, Validators.pattern('\\d{4}')]),
      city: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
      country: new FormControl('', [Validators.required, Validators.pattern('^[a-zA-Z ]*$')]),
      phoneNumber: new FormControl('', [Validators.required, Validators.pattern('(?:\\d{2} ?){4}|\\d{3} ?\\d{2} ?\\d{3}')]),
      email: new FormControl('', [Validators.required, Validators.email])
    });
  }

  initStripe() {
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

  initUserDetails(token: string) {
    console.log(atob(token.split('.')[1]));
    const userInfo: any = JSON.parse(atob(token.split('.')[1]));

    // Set values of person-information form
    this.personDetails.controls.fullName.setValue(userInfo.Name);
    this.personDetails.controls.addressLineOne.setValue(userInfo.AddressLineOne);
    this.personDetails.controls.addressLineTwo.setValue(userInfo.AddressLineTwo);
    this.personDetails.controls.county.setValue(userInfo.County);
    this.personDetails.controls.postalCode.setValue(userInfo.PostalCode);
    this.personDetails.controls.city.setValue(userInfo.City);
    this.personDetails.controls.country.setValue(userInfo.Country);
    this.personDetails.controls.phoneNumber.setValue(userInfo.PhoneNumber);
    this.personDetails.controls.email.setValue(userInfo.Email);
  }

  // FormGroup validation methods.
  getFullName() {
    return this.personDetails.controls.fullName;
  }

  getAddressLineOne() {
    return this.personDetails.controls.addressLineOne;
  }

  getAddressLineTwo() {
    return this.personDetails.controls.addressLineTwo;
  }

  getCounty() {
    return this.personDetails.controls.county;
  }

  getPostalCode() {
    return this.personDetails.controls.postalCode;
  }

  getCity() {
    return this.personDetails.controls.city;
  }

  getCountry() {
    return this.personDetails.controls.country;
  }

  getPhoneNumber() {
    return this.personDetails.controls.phoneNumber;
  }

  getEmail() {
    return this.personDetails.controls.email;
  }
}
