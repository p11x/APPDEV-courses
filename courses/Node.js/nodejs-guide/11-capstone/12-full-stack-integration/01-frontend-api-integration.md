# Full-Stack Integration Patterns

## What You'll Build In This File

Complete integration patterns connecting the NodeMark API with frontend clients, file uploads, email notifications, and real-time features.

## Frontend API Client

```javascript
// Frontend: api/client.js — Reusable API client
class ApiClient {
    constructor(baseURL) {
        this.baseURL = baseURL;
        this.accessToken = localStorage.getItem('accessToken');
    }

    async request(method, path, body = null) {
        const headers = {
            'Content-Type': 'application/json',
        };

        if (this.accessToken) {
            headers['Authorization'] = `Bearer ${this.accessToken}`;
        }

        const response = await fetch(`${this.baseURL}${path}`, {
            method,
            headers,
            body: body ? JSON.stringify(body) : null,
        });

        // Handle token expiration
        if (response.status === 401) {
            const refreshed = await this.refreshToken();
            if (refreshed) {
                return this.request(method, path, body);
            }
            throw new Error('Authentication required');
        }

        if (!response.ok) {
            const error = await response.json();
            throw new Error(error.message || 'Request failed');
        }

        return response.json();
    }

    async refreshToken() {
        try {
            const refreshToken = localStorage.getItem('refreshToken');
            const response = await fetch(`${this.baseURL}/auth/refresh`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ refreshToken }),
            });

            if (response.ok) {
                const { accessToken } = await response.json();
                this.accessToken = accessToken;
                localStorage.setItem('accessToken', accessToken);
                return true;
            }
        } catch {
            // Refresh failed
        }

        this.logout();
        return false;
    }

    logout() {
        this.accessToken = null;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
    }

    // Bookmark methods
    getBookmarks(params = {}) {
        const query = new URLSearchParams(params).toString();
        return this.request('GET', `/bookmarks${query ? '?' + query : ''}`);
    }

    createBookmark(data) {
        return this.request('POST', '/bookmarks', data);
    }

    updateBookmark(id, data) {
        return this.request('PUT', `/bookmarks/${id}`, data);
    }

    deleteBookmark(id) {
        return this.request('DELETE', `/bookmarks/${id}`);
    }

    // Auth methods
    login(email, password) {
        return this.request('POST', '/auth/login', { email, password });
    }

    register(name, email, password) {
        return this.request('POST', '/auth/register', { name, email, password });
    }
}

const api = new ApiClient('http://localhost:3000');
```

## File Upload Handling

```javascript
// src/routes/uploads.js — File upload with validation
import { Router } from 'express';
import multer from 'multer';
import path from 'node:path';
import { randomUUID } from 'node:crypto';
import { authenticate } from '../middleware/auth.js';

const router = Router();
router.use(authenticate);

// Configure multer storage
const storage = multer.diskStorage({
    destination: 'uploads/',
    filename: (req, file, cb) => {
        const ext = path.extname(file.originalname);
        cb(null, `${randomUUID()}${ext}`);
    },
});

// File filter — only allow images
const fileFilter = (req, file, cb) => {
    const allowed = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
    if (allowed.includes(file.mimetype)) {
        cb(null, true);
    } else {
        cb(new Error('Only image files are allowed'), false);
    }
};

const upload = multer({
    storage,
    fileFilter,
    limits: { fileSize: 5 * 1024 * 1024 }, // 5MB max
});

// POST /uploads — Upload single file
router.post('/', upload.single('file'), async (req, res) => {
    if (!req.file) {
        return res.status(400).json({ error: 'No file provided' });
    }

    res.status(201).json({
        filename: req.file.filename,
        originalName: req.file.originalname,
        size: req.file.size,
        mimetype: req.file.mimetype,
        url: `/uploads/${req.file.filename}`,
    });
});

export { router as uploadsRouter };
```

## Email Notification Service

```javascript
// src/services/email.js — Email notification service
import nodemailer from 'nodemailer';
import { config } from '../config/index.js';

const transporter = nodemailer.createTransport({
    host: config.email.host,
    port: config.email.port,
    secure: config.email.port === 465,
    auth: {
        user: config.email.user,
        pass: config.email.pass,
    },
});

export class EmailService {
    static async sendWelcome(user) {
        await transporter.sendMail({
            from: config.email.from,
            to: user.email,
            subject: 'Welcome to NodeMark!',
            html: `
                <h1>Welcome, ${user.name}!</h1>
                <p>Your account has been created successfully.</p>
                <p>Start organizing your bookmarks today.</p>
            `,
        });
    }

    static async sendPasswordReset(user, resetToken) {
        const resetUrl = `${config.app.url}/reset-password?token=${resetToken}`;
        await transporter.sendMail({
            from: config.email.from,
            to: user.email,
            subject: 'Password Reset Request',
            html: `
                <h1>Password Reset</h1>
                <p>Click the link below to reset your password:</p>
                <a href="${resetUrl}">Reset Password</a>
                <p>This link expires in 1 hour.</p>
            `,
        });
    }
}
```

## Error Handling Middleware

```javascript
// src/middleware/error-handler.js — Centralized error handling
export function errorHandler(err, req, res, next) {
    // Log error
    console.error('Error:', {
        message: err.message,
        stack: err.stack,
        path: req.path,
        method: req.method,
    });

    // Zod validation errors
    if (err.name === 'ZodError') {
        return res.status(400).json({
            error: 'Validation Error',
            issues: err.issues.map(i => ({
                field: i.path.join('.'),
                message: i.message,
            })),
        });
    }

    // PostgreSQL errors
    if (err.code === '23505') {
        return res.status(409).json({
            error: 'Conflict',
            message: 'Resource already exists',
        });
    }

    // JWT errors
    if (err.name === 'JsonWebTokenError') {
        return res.status(401).json({ error: 'Invalid token' });
    }
    if (err.name === 'TokenExpiredError') {
        return res.status(401).json({ error: 'Token expired' });
    }

    // Multer errors
    if (err.code === 'LIMIT_FILE_SIZE') {
        return res.status(400).json({ error: 'File too large (max 5MB)' });
    }

    // Default
    const status = err.status || 500;
    res.status(status).json({
        error: status === 500 ? 'Internal Server Error' : err.message,
    });
}
```

## How It Connects

- API client follows [05-express-framework](../../../05-express-framework/) patterns
- Auth follows [08-authentication](../../../08-authentication/) JWT patterns
- File uploads follow [18-file-uploads](../../../18-file-uploads/) patterns

## Common Mistakes

- Not handling token refresh in the API client
- Not validating file types and sizes on the server
- Not implementing proper error boundaries in frontend
- Sending synchronous email in request path (should be async)

## Try It Yourself

### Exercise 1: Build the API Client
Implement the full `ApiClient` class and test it with curl.

### Exercise 2: Add File Upload
Add a profile picture upload endpoint to the auth routes.

### Exercise 3: Error Handling
Test all error scenarios (400, 401, 404, 409, 500).

## Next Steps

Continue to [13-advanced-database/01-connection-optimization.md](../13-advanced-database/01-connection-optimization.md).
