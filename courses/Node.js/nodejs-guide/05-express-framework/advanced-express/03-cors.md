# Handling CORS in Express

## What You'll Learn

- What CORS is
- Using the cors middleware
- Configuring CORS options

## What is CORS?

**CORS** (Cross-Origin Resource Sharing) is a security feature that restricts web pages from making requests to a different domain.

## Installing CORS

```bash
npm install cors
```

## Using cors Middleware

### Basic Usage

```javascript
import cors from 'cors';

app.use(cors());
```

### Specific Origins

```javascript
app.use(cors({
  origin: 'https://example.com'
}));
```

### Multiple Origins

```javascript
app.use(cors({
  origin: ['https://example.com', 'https://api.example.com']
}));
```

## Code Example

```javascript
// cors-demo.js - CORS demonstration

import express from 'express';
import cors from 'cors';

const app = express();

// Enable CORS for all routes
app.use(cors());

// Or configure specific options
app.use(cors({
  origin: 'http://localhost:3000',
  methods: ['GET', 'POST'],
  allowedHeaders: ['Content-Type']
}));

app.get('/api/data', (req, res) => {
  res.json({ message: 'Data from API' });
});

app.post('/api/users', (req, res) => {
  res.json({ message: 'User created' });
});

app.listen(3000);
```

## Try It Yourself

### Exercise 1: Enable CORS
Add CORS middleware to your Express app.

### Exercise 2: Configure Origins
Configure CORS to allow specific origins only.
