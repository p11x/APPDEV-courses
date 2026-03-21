# Stripe Integration

## 📌 What You'll Learn

- Using Stripe SDK
- Creating payment intents
- Handling webhooks

## 💻 Code Example

```js
import Stripe from 'stripe';

const stripe = new Stripe(process.env.STRIPE_SECRET_KEY);

app.post('/create-payment-intent', async (req, res) => {
  const { amount } = req.body;
  
  const intent = await stripe.paymentIntents.create({
    amount: amount * 100,
    currency: 'usd'
  });
  
  res.json({ clientSecret: intent.client_secret });
});
```
