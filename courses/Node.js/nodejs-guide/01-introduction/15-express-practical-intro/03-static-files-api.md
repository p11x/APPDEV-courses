# Static File Serving and API Endpoint Creation

## What You'll Learn

- Serving static files with Express
- Building RESTful API endpoints
- File upload handling
- API documentation with OpenAPI

## Static File Serving

### Basic Static Files

```javascript
// src/server.js — Serve static files

import express from 'express';
import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';

const __dirname = dirname(fileURLToPath(import.meta.url));
const app = express();

// Serve static files from 'public' directory
app.use(express.static(join(__dirname, '..', 'public')));

// With options
app.use('/assets', express.static(join(__dirname, '..', 'assets'), {
    maxAge: '1d',                    // Cache for 1 day
    etag: true,                      // Enable ETag headers
    lastModified: true,              // Enable Last-Modified header
    index: false,                    // Don't serve index.html
    dotfiles: 'ignore',              // Ignore .dotfiles
    extensions: ['html', 'htm'],     // Auto-resolve extensions
}));

app.listen(3000);
```

### Directory Structure

```
project/
├── public/
│   ├── index.html
│   ├── css/
│   │   └── style.css
│   ├── js/
│   │   └── app.js
│   ├── images/
│   │   └── logo.png
│   └── favicon.ico
├── assets/
│   └── documents/
└── src/
    └── server.js
```

### SPA (Single Page Application) Support

```javascript
// Serve SPA — all non-API routes return index.html

import { readFile } from 'node:fs/promises';
import { join } from 'node:path';

const indexPath = join(__dirname, '..', 'public', 'index.html');

// API routes first
app.use('/api', apiRouter);

// Static assets
app.use(express.static(join(__dirname, '..', 'public')));

// SPA fallback — all other routes serve index.html
app.get('*', async (req, res) => {
    const html = await readFile(indexPath, 'utf-8');
    res.type('html').send(html);
});
```

## RESTful API Design

### Resource-Based Routes

```javascript
// src/routes/products.js — Full CRUD API

import { Router } from 'express';
import { authenticate, authorize } from '../middleware/auth.js';
import { validateBody } from '../middleware/validate.js';

const router = Router();

// GET /api/products — List with pagination
router.get('/', async (req, res) => {
    const { page = 1, limit = 20, sort = 'createdAt', order = 'desc' } = req.query;
    const offset = (page - 1) * limit;
    
    const products = await db.products.findMany({
        skip: offset,
        take: +limit,
        orderBy: { [sort]: order },
    });
    
    const total = await db.products.count();
    
    res.json({
        data: products,
        meta: {
            page: +page,
            limit: +limit,
            total,
            totalPages: Math.ceil(total / limit),
        },
    });
});

// GET /api/products/:id — Single resource
router.get('/:id', async (req, res) => {
    const product = await db.products.findUnique({ where: { id: req.params.id } });
    
    if (!product) {
        return res.status(404).json({ error: 'Product not found' });
    }
    
    res.json({ data: product });
});

// POST /api/products — Create
router.post('/',
    authenticate,
    validateBody({
        name: { required: true, type: 'string', minLength: 1 },
        price: { required: true, type: 'number', min: 0 },
        description: { type: 'string' },
    }),
    async (req, res) => {
        const product = await db.products.create({
            data: { ...req.body, createdBy: req.user.id },
        });
        
        res.status(201)
            .location(`/api/products/${product.id}`)
            .json({ data: product });
    }
);

// PUT /api/products/:id — Full update
router.put('/:id', authenticate, async (req, res) => {
    const existing = await db.products.findUnique({ where: { id: req.params.id } });
    
    if (!existing) {
        return res.status(404).json({ error: 'Product not found' });
    }
    
    const product = await db.products.update({
        where: { id: req.params.id },
        data: req.body,
    });
    
    res.json({ data: product });
});

// PATCH /api/products/:id — Partial update
router.patch('/:id', authenticate, async (req, res) => {
    const product = await db.products.update({
        where: { id: req.params.id },
        data: req.body,
    });
    
    res.json({ data: product });
});

// DELETE /api/products/:id — Delete
router.delete('/:id', authenticate, authorize('admin'), async (req, res) => {
    await db.products.delete({ where: { id: req.params.id } });
    res.status(204).send();
});

export default router;
```

### File Upload Handling

```bash
npm install multer
```

```javascript
// src/routes/uploads.js — File upload handling

import { Router } from 'express';
import multer from 'multer';
import { randomUUID } from 'node:crypto';
import { join } from 'node:path';

const router = Router();

// Configure storage
const storage = multer.diskStorage({
    destination: 'uploads/',
    filename: (req, file, cb) => {
        const ext = file.originalname.split('.').pop();
        cb(null, `${randomUUID()}.${ext}`);
    },
});

// File filter
const fileFilter = (req, file, cb) => {
    const allowed = ['image/jpeg', 'image/png', 'image/webp', 'application/pdf'];
    if (allowed.includes(file.mimetype)) {
        cb(null, true);
    } else {
        cb(new Error('File type not allowed'), false);
    }
};

const upload = multer({
    storage,
    fileFilter,
    limits: {
        fileSize: 10 * 1024 * 1024, // 10MB
        files: 5,
    },
});

// Single file upload
router.post('/single', upload.single('file'), (req, res) => {
    if (!req.file) {
        return res.status(400).json({ error: 'No file uploaded' });
    }
    
    res.status(201).json({
        data: {
            filename: req.file.filename,
            size: req.file.size,
            mimetype: req.file.mimetype,
            url: `/uploads/${req.file.filename}`,
        },
    });
});

// Multiple files upload
router.post('/multiple', upload.array('files', 5), (req, res) => {
    const files = req.files.map(f => ({
        filename: f.filename,
        size: f.size,
        mimetype: f.mimetype,
        url: `/uploads/${f.filename}`,
    }));
    
    res.status(201).json({ data: files });
});

export default router;
```

## API Response Standards

```javascript
// src/utils/response.js — Consistent API responses

export function success(res, data, meta = {}) {
    res.json({ data, meta });
}

export function created(res, data, location) {
    if (location) res.location(location);
    res.status(201).json({ data });
}

export function noContent(res) {
    res.status(204).send();
}

export function error(res, status, message, code = 'ERROR') {
    res.status(status).json({ error: { message, code } });
}

export function paginated(res, data, { page, limit, total }) {
    res.json({
        data,
        meta: {
            page,
            limit,
            total,
            totalPages: Math.ceil(total / limit),
            hasNext: page * limit < total,
            hasPrev: page > 1,
        },
    });
}
```

## API Documentation with JSDoc

```javascript
// src/routes/users.js — Documented endpoints

/**
 * @route GET /api/users
 * @description List all users with pagination
 * @param {number} [page=1] - Page number
 * @param {number} [limit=20] - Items per page
 * @param {string} [sort=createdAt] - Sort field
 * @param {string} [order=desc] - Sort order (asc/desc)
 * @returns {Object} { data: User[], meta: PaginationMeta }
 */
router.get('/', async (req, res) => {
    // ... implementation
});

/**
 * @route POST /api/users
 * @description Create a new user
 * @body {string} name - User's full name (required)
 * @body {string} email - User's email (required)
 * @body {string} [role=user] - User role
 * @returns {Object} { data: User }
 */
router.post('/', async (req, res) => {
    // ... implementation
});
```

## Testing API Endpoints

```javascript
// src/__tests__/users.test.js — API tests

import { describe, it, before, after } from 'node:test';
import assert from 'node:assert';
import { createServer } from 'node:http';
import app from '../server.js';

describe('Users API', () => {
    let server;
    let baseURL;
    
    before(async () => {
        server = createServer(app);
        await new Promise(resolve => server.listen(0, resolve));
        const { port } = server.address();
        baseURL = `http://localhost:${port}`;
    });
    
    after(() => server.close());
    
    it('GET /api/users returns list', async () => {
        const res = await fetch(`${baseURL}/api/users`);
        assert.strictEqual(res.status, 200);
        const body = await res.json();
        assert.ok(Array.isArray(body.data));
    });
    
    it('POST /api/users creates user', async () => {
        const res = await fetch(`${baseURL}/api/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: 'Test', email: 'test@example.com' }),
        });
        assert.strictEqual(res.status, 201);
        const body = await res.json();
        assert.strictEqual(body.data.name, 'Test');
    });
    
    it('POST /api/users validates input', async () => {
        const res = await fetch(`${baseURL}/api/users`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name: '' }),
        });
        assert.strictEqual(res.status, 400);
    });
});
```

## Best Practices Checklist

- [ ] Use `express.static()` with caching headers
- [ ] Implement consistent API response format
- [ ] Use proper HTTP status codes (200, 201, 204, 400, 401, 403, 404, 500)
- [ ] Validate file uploads (type, size)
- [ ] Document all API endpoints
- [ ] Write integration tests for endpoints
- [ ] Use proper REST conventions (GET=read, POST=create, PUT=replace, PATCH=update, DELETE=remove)

## Cross-References

- See [Basic Server](./01-basic-server.md) for Express setup
- See [Routing and Middleware](./02-routing-middleware.md) for advanced patterns
- See [Security Best Practices](../21-security-modern/01-security-headers-deps.md) for API security
- See [File Uploads](../../../18-file-uploads/) for advanced upload handling

## Next Steps

Continue to [Package Management Hands-On](../16-package-management-hands-on/01-npm-hands-on.md) for npm workflows.
