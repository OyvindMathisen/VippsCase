// Create a Stripe client.StripeElement
const stripe = Stripe('pk_test_IHKCM3CTES4VMOFNa07X685b00z1e1ZBXP');

// Create an instance of elements
const elements = stripe.elements();

// Custom styling can be passed to options when creating an Element
const style = {
    base: {
        color: '#32325d',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            color: '#aab7c4'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};

// Create an instance of the card Element
const card = elements.create('card', {style: style, hidePostalCode: true});
// const card = elements.create('card', {hidePostalCode: true});

// Add an instance of the card Element into the 'card-element' <div>.
card.mount('#card-element');

card.addEventListener('change', (event) => {
    const displayError = document.getElementById('card-errors');
    if (event.error) {
        displayError.textContent = event.error.message;
    } else {
        displayError.textContent = '';
    }
});

// Handle form submission
const form = document.getElementById('payment-form');
form.addEventListener('submit', (event) => {
    event.preventDefault();
    // Get the cost of the items here.
    const cost = 500;

    stripe.createToken(card).then((result) => {
        if (result.error) {
            // Inform the user if there was an error
            const errorElement = document.getElementById('card-errors');
            errorElement.textContent = result.error.message;
        } else {
            // Send the token to your server
            stripeTokenHandler(result.token, cost);
        }
    });
});

const customerForm = document.getElementById('customer-form');
customerForm.addEventListener('submit', (event) => {
    event.preventDefault();

    const name = document.getElementById('name').value;
    const addressLine1 = document.getElementById('address-1').value;
    const addressLine2 = document.getElementById('address-2').value;
    const postCode = document.getElementById('postal-code').value;
    const city = document.getElementById('city').value;
    const country = document.getElementById('country').value;
    const email = document.getElementById('email').value;
    const phone = document.getElementById('phone').value;

    customerTokenHandler(JSON.stringify({
        name: name,
        addressLineOne: addressLine1,
        addressLineTwo: addressLine2,
        postalCode: postCode,
        city: city,
        country: country,
        email: email,
        phone: phone
    }));
});

async function stripeTokenHandler(token, cost) {
    try {
        const response = await fetch('https://localhost:44369/charge', {
            method: 'POST',
            body: JSON.stringify({
                stripeToken: token.id,
                totalCost: cost * 100
            }),
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const json = await response.json();
        console.log('Success:', JSON.stringify(json));
    } catch (error) {
        console.error('Error:', error);
    }
}

async function customerTokenHandler(customer) {
    try {
        const response = await fetch('https://localhost:44369/add-customer', {
            method: 'POST',
            body: customer,
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        const json = await response.json();
        console.log('Success: ', JSON.stringify(json));
    } catch (error) {
        console.error('Error: ', error);
    }
}