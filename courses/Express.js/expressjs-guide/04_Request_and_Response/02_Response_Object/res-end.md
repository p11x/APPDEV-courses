# res.end() - Ending the Response

## 📌 What You'll Learn
- When to use res.end()
- Differences from res.send()
- Basic usage

## 🧠 Concept Explained (Plain English)

The `res.end()` method ends the response process without sending any data. It's like hanging up a phone call - you end the connection without saying anything more.

Use this when:
- You just want to acknowledge receipt
- You're using streaming responses
- You need minimal overhead

## 💻 Code Example

```javascript
// ES Module - Using res.end()

import express from 'express';

const app = express();

// Simple acknowledgment
app.get('/ping', (req, res) => {
    // req = request, res = response
    res.status(200).end();
});

// With status but no body
app.post('/data', (req, res) => {
    // Process data...
    res.status(202).end();
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown

| Line | Code | What It Does |
|------|------|--------------|
| 12 | `res.status(200).end();` | End response with 200 status |

## ✅ Quick Recap

- Use res.end() to end response without data
- Useful for simple acknowledgments
- Often used with status()

## 🔗 What's Next

Learn about [res.type()](./res-type.md)