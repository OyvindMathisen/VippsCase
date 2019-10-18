import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StripeCharge } from '../shared/models/stripe-charge.model';
import { StripeCustomer } from '../shared/models/stripe-customer.model';

@Injectable({
  providedIn: 'root'
})

export class StripeService {
  private baseUrl = 'https://localhost:44399/api/stripe/';
  private makeChargeEndpoint = this.baseUrl + 'charge';
  private addCustomerEndpoint = this.baseUrl + 'add-customer';
  private options = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  addCharge(charge: StripeCharge): Observable<any> {
    return this.http.post(this.makeChargeEndpoint, charge, this.options);
  }

  addCustomer(customer: StripeCustomer): Observable<any> {
    return this.http.post(this.addCustomerEndpoint, customer, this.options);
  }
}