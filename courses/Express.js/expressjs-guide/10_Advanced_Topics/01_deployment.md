# Deploying Express.js Applications

## Preparing for Production

Before deploying your Express.js app, there are several important steps to ensure it runs smoothly in production.

## Environment Configuration

### Using Environment Variables

**Environment variables** are values stored outside your code that configure how your application runs. They're essential for:

- Keeping secrets out of source code
- Different settings for development vs production
- Easy deployment configuration

### Install dotenv

```bash
npm install dotenv
```

### Create .env Files

```bash
# .env - Development (local)
PORT=3000
NODE_ENV=development
DB_HOST=localhost
DB_USER=root
DB_PASSWORD=password
JWT_SECRET=devsecret123

# .env.production - Production (on server)
PORT=8080
NODE_ENV=production
DB_HOST=prod-db.example.com
DB_USER=produser
DB_PASSWORD=securepassword123!
JWT_SECRET=verylongandrandomsecret456!
```

### Load Environment Variables

```javascript
// server.js
import dotenv from 'dotenv';

// Load environment variables from .env file
// This should be at the very top of your main file
dotenv.config();

import express from 'express';

const app = express();

// Use environment variables
// process.env accesses environment variables set outside code
const PORT = process.env.PORT || 3000;
const NODE_ENV = process.env.NODE_ENV || 'development';

// Middleware
app.use(express.json());

// Routes
app.get('/', (req, res) => {
    res.json({
        message: 'Welcome to Express API',
        environment: NODE_ENV
    });
});

// Start server
app.listen(PORT, () => {
    console.log(`Server running in ${NODE_ENV} mode on port ${PORT}`);
});
```

## Production Best Practices

### 1. Use Process Managers

**Process managers** keep your app running and handle restarts.

```bash
# Install PM2 (popular process manager)
npm install -g pm2

# Start your app
pm2 start server.js

# List running processes
pm2 list

# View logs
pm2 logs

# Restart on changes
pm2 start server.js --watch
```

### 2. Enable Gzip Compression

```bash
npm install compression
```

```javascript
import compression from 'compression';

app.use(compression());
// Compresses responses sent to clients
```

### 3. Use Logging

```bash
npm install morgan
```

```javascript
import morgan from 'morgan';

if (NODE_ENV === 'production') {
    // In production, log to files
    app.use(morgan('combined'));
} else {
    // In development, log to console
    app.use(morgan('dev'));
}
```

### 4. Handle Graceful Shutdown

```javascript
// Handle uncaught exceptions
process.on('uncaughtException', (err) => {
    console.error('Uncaught Exception:', err);
    // Log and exit gracefully
    process.exit(1);
});

process.on('unhandledRejection', (reason) => {
    console.error('Unhandled Rejection:', reason);
});

// Graceful shutdown
process.on('SIGTERM', () => {
    console.log('SIGTERM received, shutting down gracefully');
    // Close database connections, etc.
    process.exit(0);
});
```

## Deploying to Platforms

### Railway

```bash
# Install Railway CLI
npm install -g @railway/cli

# Login
railway login

# Initialize project
railway init

# Set environment variables
railway variables set PORT=8080
railway variables set DB_URL=your_db_url

# Deploy
railway up
```

### Render

1. Connect your GitHub repository
2. Set build command: `npm install`
3. Set start command: `node server.js`
4. Add environment variables in dashboard

### Heroku

```bash
# Install Heroku CLI
# Create Procfile
echo "web: node server.js" > Procfile

# Deploy
git add .
git commit -m "Deploy"
git push heroku main
```

## Dockerfile for Containers

```dockerfile
# Use Node.js LTS version
FROM node:20-alpine

# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy application code
COPY . .

# Expose port from environment variable
EXPOSE $PORT

# Start application
CMD ["node", "server.js"]
```

## Health Checks

```javascript
// Health check endpoint - used by deployment platforms
app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        uptime: process.uptime(),
        timestamp: new Date().toISOString()
    });
});
```

## Production Checklist

| Task | Status |
|------|--------|
| Set NODE_ENV=production | ☐ |
| Use environment variables for secrets | ☐ |
| Enable gzip compression | ☐ |
| Set up logging | ☐ |
| Use process manager (PM2) | ☐ |
| Enable HTTPS | ☐ |
| Set up health checks | ☐ |
| Configure CORS properly | ☐ |
| Implement rate limiting | ☐ |
| Add graceful shutdown | ☐ |

## What's Next?

- **[Environment Configuration](./04_deployment/environment-configuration.md)** — Managing multiple environments
