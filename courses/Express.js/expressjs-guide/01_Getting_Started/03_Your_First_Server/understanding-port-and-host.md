# Understanding Port and Host

## 📌 What You'll Learn
- What ports and hosts are
- How to configure them in Express
- When to use different values

## 🧠 Concept Explained (Plain English)

When your Express server runs, it needs to know two things: **where** to listen (the host) and **which door** to use (the port). Together, these determine how clients can connect to your server.

Think of your computer as an apartment building. The **host** is the building's address, and the **port** is the apartment number. Just like you'd need both an address AND an apartment number to visit someone, a client needs both a host AND a port to reach your server.

**Port** is a number between 0 and 65535 that identifies a specific "door" on your computer. Different services traditionally use different ports — web servers often use 80 or 443, while development servers commonly use 3000.

**Host** is the network address your server binds to. It can be an IP address (like "127.0.0.1") or a hostname (like "localhost" or "0.0.0.0").

## 💻 Code Examples

### Basic Port Configuration

```javascript
// ES Module

import express from 'express';

const app = express();

// ========================================
// PORT CONFIGURATION
// ========================================

// Most common pattern - use environment variable with fallback
const PORT = process.env.PORT || 3000;

// Process.env stores environment variables
// These are set outside your code (in terminal, deployment platform, etc.)
// Using process.env.PORT lets deployment platforms set their own port
// The || 3000 provides a default for local development

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

### Host Configuration

```javascript
import express from 'express';

const app = express();
const PORT = process.env.PORT || 3000;
const HOST = process.env.HOST || '0.0.0.0';

// ========================================
// HOST OPTIONS
// ========================================

// '127.0.0.1' or 'localhost'
// Only accessible from THIS computer
// Good for local development
app.listen(PORT, '127.0.0.1', () => {
    console.log('Server only accessible from this machine');
});

// '0.0.0.0' 
// Accessible from ANY network interface
// Required when running in containers (Docker) or VMs
app.listen(PORT, '0.0.0.0', () => {
    console.log('Server accessible from outside');
});

// No host specified
// Defaults to all available interfaces
app.listen(PORT, () => {
    console.log('Server running');
});
```

### Full Example with Environment Variables

```javascript
// server.js

import express from 'express';

const app = express();

// Read from environment variables
// process.env lets you configure without changing code
const PORT = process.env.PORT || 3000;
const HOST = process.env.HOST || '0.0.0.0';
const NODE_ENV = process.env.NODE_ENV || 'development';

// Create different messages based on environment
const message = NODE_ENV === 'production' 
    ? 'Production Server Running'
    : 'Development Server Running';

app.get('/', (req, res) => {
    res.json({
        message,
        port: PORT,
        host: HOST,
        environment: NODE_ENV
    });
});

// Listen on specified host and port
app.listen(PORT, HOST, () => {
    console.log(`${message} at http://${HOST}:${PORT}`);
});
```

## Common Ports and Their Uses

| Port | Common Use |
|------|-------------|
| 80 | HTTP (production web) |
| 443 | HTTPS (secure web) |
| 3000 | Node.js development servers |
| 3001 | Alternative Node.js port |
| 5000 | Rails, alternative dev |
| 5432 | PostgreSQL database |
| 27017 | MongoDB database |
| 6379 | Redis |

## Environment Variables in Practice

### Setting Environment Variables

```bash
# In terminal (temporary)
export PORT=8080
export HOST=0.0.0.0

# In .env file (using dotenv)
# PORT=8080
# HOST=0.0.0.0
# NODE_ENV=production

# Using nodemon
nodemon --env PORT=4000 server.js
```

### Using dotenv Package

```bash
npm install dotenv
```

```javascript
import dotenv from 'dotenv';
dotenv.config();  // Loads .env file

const PORT = process.env.PORT || 3000;
```

## ⚠️ Common Mistakes

**1. Hardcoding ports**
Always use `process.env.PORT || 3000`. Production servers have different port requirements.

**2. Using wrong host**
For local development, localhost is fine. For production/dockers, use 0.0.0.0.

**3. Port conflicts**
If "EADDRINUSE" error appears, another process is using that port. Find and stop it, or use a different port.

## ✅ Quick Recap

- Port is like an apartment number (0-65535)
- Host is the building address
- Use `process.env.PORT || 3000` for flexibility
- Use `0.0.0.0` when accessible externally needed

## 🔗 What's Next

Now that you understand the basics, let's dive into routing!
