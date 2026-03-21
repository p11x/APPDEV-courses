# Environment Configuration and Deployment

## 📌 What You'll Learn
- How to manage different environments (development, production)
- Using environment variables properly
- Deployment strategies for Express apps
- Best practices for production configuration

## 🧠 Concept Explained (Plain English)

When you develop a web application, you typically have multiple "environments":

- **Development**: Where you write and test code
- **Staging**: Where you test before production
- **Production**: Where real users access your app

Each environment needs different configurations:
- Development: Show error details, use local database, enable debugging
- Production: Hide errors, use production database, optimize performance

**Environment variables** are the standard way to manage these differences. They're like settings that change based on where your code is running.

Think of it like a house:
- Development is like having the front door unlocked and lights on
- Production is like having multiple locks, security systems, and controlled access

Environment variables keep your sensitive configuration (passwords, API keys) separate from your code, which is important for security.

## 💻 Code Example

```javascript
// ES Module - Environment Configuration

import express from 'express';

const app = express();

/*
// ========================================
// WHY USE process.env?
// ========================================

Environment variables (process.env) are used instead of hardcoded values because:

1. SECURITY: Keep secrets out of source code
   - Database passwords, API keys, JWT secrets
   - Never commit these to version control!

2. FLEXIBILITY: Different settings for different environments
   - Development: localhost database
   - Production: cloud database
   - Test: in-memory database

3. DEPLOYMENT: Easy to change without modifying code
   - Container platforms (Docker, Kubernetes)
   - Cloud platforms (Heroku, AWS)
   - CI/CD pipelines

4. PORTABILITY: Works across different systems
   - Windows, Linux, macOS
   - Different hosting providers
*/

// ========================================
// ENVIRONMENT CONFIGURATION
// ========================================

// Define which environment we're in
const NODE_ENV = process.env.NODE_ENV || 'development';

// Determine if we're in production
const isProduction = NODE_ENV === 'production';

// Basic configuration based on environment
const config = {
    // Server
    port: parseInt(process.env.PORT) || 3000,
    env: NODE_ENV,
    
    // Database
    database: {
        host: process.env.DB_HOST || 'localhost',
        port: parseInt(process.env.DB_PORT) || 5432,
        name: process.env.DB_NAME || 'myapp',
        // These should ALWAYS come from environment in production!
        user: process.env.DB_USER,
        password: process.env.DB_PASSWORD
    },
    
    // Security
    jwtSecret: process.env.JWT_SECRET || 'dev-secret-change-in-prod',
    
    // Logging
    logLevel: isProduction ? 'info' : 'debug'
};

console.log('Environment:', config.env);
console.log('Running in production:', isProduction);

// ========================================
// MIDDLEWARE FOR ENVIRONMENT
// ========================================

// Request logging middleware
app.use((req, res, next) => {
    // In production, you might use morgan or winston for logging
    if (!isProduction) {
        console.log(`${req.method} ${req.url}`);
    }
    next();
});

// Security middleware for production
if (isProduction) {
    // Import production-specific security packages
    // import helmet from 'helmet';
    // import cors from 'cors';
    // app.use(helmet());
    // app.use(cors());
}

// ========================================
// SAMPLE ROUTES WITH ENVIRONMENT AWARENESS
// ========================================

app.get('/', (req, res) => {
    res.json({
        message: 'Welcome to the API',
        environment: config.env,
        // Don't expose sensitive info in production!
        debugInfo: isProduction ? undefined : {
            version: '1.0.0',
            database: config.database.host
        }
    });
});

// Health check endpoint (important for deployment)
app.get('/health', (req, res) => {
    res.json({
        status: 'healthy',
        timestamp: new Date().toISOString(),
        environment: config.env,
        uptime: process.uptime()
    });
});

// ========================================
// PRODUCTION OPTIMIZATIONS
// ========================================

// In production:
if (isProduction) {
    // Trust proxy (for reverse proxies like nginx, Heroku, etc.)
    app.set('trust proxy', 1);
    
    // Disable powered-by header for security
    app.disable('x-powered-by');
    
    // Enable gzip compression (would use compression package)
    // app.use(compression());
}

// ========================================
// ERROR HANDLING FOR DIFFERENT ENVIRONMENTS
// ========================================

// eslint-disable-next-line no-unused-vars
app.use((err, req, res, next) => {
    console.error('Error:', err.message);
    
    // In development, show detailed error
    // In production, hide details
    res.status(err.statusCode || 500).json({
        error: isProduction 
            ? 'Internal server error'  // Safe message
            : err.message              // Detailed for debugging
    });
});

// ========================================
// STARTING THE SERVER
// ========================================

const PORT = config.port;

app.listen(PORT, () => {
    console.log(`
==========================================
Server Configuration
==========================================
Environment: ${config.env}
Port: ${PORT}
Database: ${config.database.host}:${config.database.port}
==========================================
    `);
});

/*
// ========================================
// ENVIRONMENT FILES (.env)
// ========================================

# Create a .env file in your project root:
# (Add this file to .gitignore!)

# Server
PORT=3000
NODE_ENV=development

# Database
DB_HOST=localhost
DB_PORT=5432
DB_NAME=myapp
DB_USER=postgres
DB_PASSWORD=secretpassword

# Security
JWT_SECRET=your-super-secret-jwt-key

# API Keys
API_KEY=your-api-key-here


// ========================================
// USING DOTENV PACKAGE
// ========================================

// Install: npm install dotenv

// At the top of your main app file:
import dotenv from 'dotenv';
dotenv.config(); // Loads variables from .env file


// ========================================
// PRODUCTION CHECKLIST
// ========================================

1. Set NODE_ENV=production
2. Use environment variables for all secrets
3. Enable security middleware (helmet, cors)
4. Disable verbose error messages
5. Enable request logging
6. Use compression
7. Set up monitoring and alerts
8. Configure reverse proxy (nginx)
9. Set up SSL/HTTPS
10. Use process managers (PM2, forever)


// ========================================
// DEPLOYMENT OPTIONS
// ========================================

1. Heroku
   - git push heroku main
   - Set config vars in dashboard

2. AWS (EC2, Lambda)
   - Use EB (Elastic Beanstalk)
   - Or containerize with Docker

3. Docker
   - Create Dockerfile
   - docker build -t myapp .
   - docker run -p 3000:3000 myapp

4. DigitalOcean
   - Use App Platform or Droplets

5. Vercel/Netlify
   - Serverless deployment
*/
```

## 🔍 Environment Configuration

| Variable | Purpose | Example |
|----------|---------|---------|
| `NODE_ENV` | Current environment | development, production, test |
| `PORT` | Server port | 3000 |
| `DB_HOST` | Database server | localhost, 123.45.67.89 |
| `DB_PASSWORD` | Database password | (should be secret!) |
| `JWT_SECRET` | JWT signing key | (should be secret!) |
| `API_KEY` | External API keys | (should be secret!) |

## ⚠️ Common Mistakes

**1. Hardcoding secrets in source code**
Never put passwords, API keys, or secrets directly in your code!

**2. Not using .gitignore**
Make sure .env files are not committed to version control.

**3. Not setting NODE_ENV**
Always set NODE_ENV=production in production. It enables optimizations!

**4. Showing error details in production**
Detailed error messages help attackers. Hide them in production.

**5. Not having a health check**
Deployment platforms need health checks to know if your app is running.

## ✅ Quick Recap

- Use environment variables for all configuration
- Never commit secrets to version control
- Set NODE_ENV appropriately (development, production, test)
- Different environments need different configurations
- Use .env files for local development
- Hide detailed errors in production
- Set up health checks for deployment platforms
- Use process managers (PM2) for production

---

✅ Guide complete. All 42 files written across 10 folders.
