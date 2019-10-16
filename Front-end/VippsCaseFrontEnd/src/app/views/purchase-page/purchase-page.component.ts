import { Component, Output, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { environment } from '../../../environments/environment.prod';
import { StripeService } from '../../services/stripe.service';
import { StripeCharge } from '../../shared/models/stripe-charge.model';

@Component({
    selector: 'app-purchase-page',
    templateUrl: './purchase-page.component.html',
    styleUrls: ['./purchase-page.component.scss']
})
export class PurchasePageComponent implements AfterViewInit {
    card: any;
    cardError: any;
    @Output() stripe: stripe.Stripe;
    @ViewChild('purchaseDetails', {static: false}) purchaseDetails: ElementRef;

    constructor(private stripeService: StripeService) {
        this.stripe = Stripe(environment.stripeKey);
        const elements = this.stripe.elements();
        this.card = elements.create('card', {hidePostalCode: true});
    }

    // Needs to be after the view has been initialized, if not we do not have access to the purchaseDetails form.
    ngAfterViewInit() {
        const form = this.purchaseDetails.nativeElement;
        form.addEventListener('submit', (event: any) => {
            event.preventDefault();

            // TODO: Replace this with the totalCost from the shoppingCart
            // NOTE: Multiply the sum with 100, as Stripe calculates from the lowest denominator, which is "øre" in nok.
            const cost = 1000 * 100;

            this.stripe.createToken(this.card).then((result) => {
                if (result.error) {
                    // Inform the user if there was an error
                    const errorElement = this.cardError.nativeElement;
                    errorElement.textContent = result.error.message;
                } else {
                    // Send the token to the server
                    this.stripeTokenHandler(result.token.id, cost);
                }
            });
        });
    }

    onCardChanged(event: any) {
        this.card = event.card;
        this.cardError = event.cardErrors;
    }

    stripeTokenHandler(token: string, cost: number) {
        // Generates the charge object, addign the values and handing the response.
        const charge = {} as StripeCharge;
        charge.stripeToken = token;
        charge.totalCost = cost;
        // TODO: Change success to a redirect to the success page, and handle other database actions.
        this.stripeService.addCharge(charge).subscribe(
            (data) => console.log(data),
            (error) => console.log(error)
        );
    }

}
