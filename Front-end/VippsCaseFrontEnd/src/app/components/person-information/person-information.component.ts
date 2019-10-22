import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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

  // Outputs
  @Output() confirmPurchaseDetails: EventEmitter<any>;
  @Output() cardInitialized: EventEmitter<any>;

  // Local Variables
  personDetails: FormGroup;

  constructor() {
    this.confirmPurchaseDetails = new EventEmitter();
    this.cardInitialized = new EventEmitter();
  }

  ngOnInit() {
    // Initializing the FormGroup for input and validation.
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

  onPurchaseClicked() {
    // Purchase is validated and ready to be processed!
    this.confirmPurchaseDetails.emit(this.personDetails.value);
  }

  // Method only passes the mounted card details to the parent.
  stripeCardInited(event: any) {
    this.cardInitialized.emit(event);
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
