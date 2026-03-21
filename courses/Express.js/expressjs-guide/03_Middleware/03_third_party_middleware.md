# Third-Party Middleware in Express.js

## Why Use Third-Party Middleware?

The Express ecosystem has thousands of community-built middleware packages that add useful features like logging, security, CORS, and file uploads. You don't have to build everything from scratch!

## Installing Middleware

```bash
npm install <package-name>
```

Then import and use in your app.

## Popular Middleware Packages

### 1. Morgan - HTTP Request Logging

**Morgan** automatically logs HTTP requests, making debugging easier.

```bash
npm install morgan
```

```javascript
// server.js
import express from 'express';
import morgan from 'morgan';

const app = express();
// 'app' is our Express application instance

// ============================================
// Middleware Table
// ============================================
// | Middleware    | Purpose                           |
// |--------------|-----------------------------------|
// | morgan       | HTTP request logging              |
// | cors        | Cross-origin resource sharing     |
// | helmet      | Security HTTP headers             |
// | multer      | File upload handling              |
// | compression | Response compression             |
// ============================================

// morgan('tiny') - minimal logging
// Other options: 'combined', 'common', 'dev', 'short'
app.use(morgan('tiny'));

// This will log every request, e.g.:
// GET / 200 13.234 ms - 12

app.get('/', (req, res) => {
    res.send('Check your terminal for the Morgan log!');
});

app.get('/api/data', (req, res) => {
    res.json({ message: 'Data endpoint' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### 2. CORS - Cross-Origin Resource Sharing

**CORS** allows browsers to make requests to your API from different domains.

```bash
npm install cors
```

```javascript
// server.js
import express from 'express';
import cors from 'cors';

const app = express();

// Enable CORS for all origins
// This allows any website to access your API
app.use(cors());

// Or configure CORS options:
const corsOptions = {
    origin: 'https://my-website.com',  // Only allow this domain
    methods: ['GET', 'POST', 'PUT'],   // Allowed methods
    allowedHeaders: ['Content-Type'],  // Allowed headers
};

app.use(cors(corsOptions));

// API routes
app.get('/api/data', (req, res) => {
    res.json({ message: 'CORS enabled!' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### 3. Helmet - Security Headers

**Helmet** sets various HTTP headers to protect your app from common web vulnerabilities.

```bash
npm install helmet
```

```javascript
// server.js
import express from 'express';
import helmet from 'helmet';

const app = express();

// Helmet sets security headers automatically
// - X-Content-Type-Options: nosniff
// - X-Frame-Options: SAMEORIGIN  
// - X-XSS-Protection
// - Strict-Transport-Security
app.use(helmet());

app.get('/', (req, res) => {
    res.send('Secure with Helmet!');
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### 4. Multer - File Uploads

**Multer** handles multipart/form-data (file uploads).

```bash
npm install multer
```

```javascript
// server.js
import express from 'express';
import multer from 'multer';

const app = express();
// 'app' is our Express application instance

// Configure storage
const storage = multer.diskStorage({
    destination: (req, file, cb) => {
        // Store files in 'uploads/' folder
        cb(null, 'uploads/');
    },
    filename: (req, file, cb) => {
        // Use original filename with timestamp
        const uniqueSuffix = Date.now() + '-' + Math.round(Math.random() * 1E9);
        cb(null, file.fieldname + '-' + uniqueSuffix);
    }
});

// Create upload middleware
// upload.single('avatar') expects a field named 'avatar'
const upload = multer({ storage });

// ============================================
// Route Table
// ============================================
// | Method | Path      | Handler         | Description         |
// |--------|-----------|-----------------|---------------------|
// | POST   | /upload   | uploadSingle    | Upload single file  |
// | POST   | /multiple | uploadMultiple  | Upload multiple    |
// ============================================

// Single file upload
app.post('/upload', upload.single('avatar'), (req, res) => {
    // req.file contains the uploaded file info
    res.json({
        message: 'File uploaded!',
        file: req.file
    });
});

// Multiple files
app.post('/multiple', upload.array('photos', 5), (req, res) => {
    // req.files contains array of files
    res.json({
        message: 'Files uploaded!',
        files: req.files
    });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

### 5. Compression - GZIP Compression

**Compression** compresses responses, making them smaller and faster.

```bash
npm install compression
```

```javascript
// server.js
import express from 'express';
import compression from 'compression';

const app = express();

// Compress all responses
// Makes responses smaller (faster for clients)
app.use(compression());

app.get('/large-data', (req, res) => {
    // This response will be compressed
    const largeData = { 
        items: Array(1000).fill({ message: 'Hello World!' }) 
    };
    res.json(largeData);
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Combining Multiple Middleware

```javascript
// server.js - Complete example with multiple middleware
import express from 'express';
import morgan from 'morgan';
import cors from 'cors';
import helmet from 'helmet';
import compression from 'compression';

const app = express();

// Order matters! 

// 1. Security
app.use(helmet());           // Security headers first
app.use(cors());             // Then CORS

// 2. Performance
app.use(compression());       // Compress responses
app.use(morgan('dev'));      // Log requests

// 3. Data parsing
app.use(express.json());     // Parse JSON bodies
app.use(express.urlencoded({ extended: true }));

// Routes
app.get('/api/users', (req, res) => {
    res.json([{ id: 1, name: 'Alice' }]);
});

app.post('/api/users', (req, res) => {
    res.status(201).json({ id: 2, ...req.body });
});

// Error handler
app.use((err, req, res, next) => {
    console.error(err.message);
    res.status(500).json({ error: 'Server error' });
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server on port ${PORT}`));
```

## Useful Middleware Table

| Package | Purpose | Install |
|---------|---------|---------|
| **morgan** | HTTP logging | `npm i morgan` |
| **cors** | Cross-origin requests | `npm i cors` |
| **helmet** | Security headers | `npm i helmet` |
| **multer** | File uploads | `npm i multer` |
| **compression** | GZIP compression | `npm i compression` |
| **cookie-parser** | Cookie parsing | `npm i cookie-parser` |
| **express-validator** | Input validation | `npm i express-validator` |
| **jsonwebtoken** | JWT authentication | `npm i jsonwebtoken` |
| **bcryptjs** | Password hashing | `npm i bcryptjs` |
| **dotenv** | Environment variables | `npm i dotenv` |

## Middleware Order Best Practices

Always use middleware in this order:

1. **Security** (helmet, cors)
2. **Performance** (compression)
3. **Logging** (morgan)
4. **Parsing** (express.json, express.urlencoded)
5. **Your custom middleware**
6. **Routes**
7. **Error handlers**

## What's Next?

- **[Request & Response](../04_Request_Response/01_request_object.md)** — Working with req and res objects
- **[Database Integration](../06_Database_Integration/01_mongodb.md)** — Connecting to databases
- **[Security](../08_Security/01_authentication.md)** — Authentication and authorization
