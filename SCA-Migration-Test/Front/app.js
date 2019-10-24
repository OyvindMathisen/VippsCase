// Create a Stripe client.
const stripe = Stripe("pk_test_IHKCM3CTES4VMOFNa07X685b00z1e1ZBXP");

// Create an instance of Elements.
const elements = stripe.elements();

// Custom styling can be passed to options when creating an Element.
// (Note that this demo uses a wider set of styles than the guide below.)
const style = {
  base: {
    color: "#32325d",
    fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
    fontSmoothing: "antialiased",
    fontSize: "16px",
    "::placeholder": {
      color: "#aab7c4"
    }
  },
  invalid: {
    color: "#fa755a",
    iconColor: "#fa755a"
  }
};

// Create an instance of the card Element.
const card = elements.create("card", { style: style });

// Add an instance of the card Element into the `card-element` <div>.
card.mount("#card-element");

// Handle real-time validation errors from the card Element.
card.addEventListener("change", function(event) {
  var displayError = document.getElementById("card-errors");
  if (event.error) {
    displayError.textContent = event.error.message;
  } else {
    displayError.textContent = "";
  }
});

// Handle form submission.
var form = document.getElementById("payment-form");
form.addEventListener("submit", async function(event) {
  event.preventDefault();

  const { paymentMethod } = await stripe.createPaymentMethod('card', card);

  const response = await fetch('https://localhost:44321/stripe', {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({
      paymentMethodId: paymentMethod.id
    })
  });

  await handleServerResponse(await response.json());
});

async function handleServerResponse(response) {
  if (response.error) {
    // Show error from server on payment form
    const errorElement = document.getElementById("card-errors");
    errorElement.textContent = response.error;
  } else if (response.requires_action) {
    // Use Stripe.js to handle required card action
    await handleAction(response);
  } else {
    // Show success message
    alert("Success!");
  }
}

async function handleAction(response) {
  const { error: errorAction, paymentIntent } = await stripe.handleCardAction(response.payment_intent_client_secret);

  if (errorAction) {
    // Show error from Stripe.js in payment
    const errorElement = document.getElementById("card-errors");
    errorElement.textContent = errorAction.message;
  } else {
    const response = await fetch('https://localhost:44321/stripe', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        paymentIntentId: paymentIntent.id
      })
    });

    // Step 3: handle server response
    handleServerResponse(await response.json);
  }
}
