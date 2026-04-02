# Advanced Security Implementation for NodeMark

## What You'll Build In This File

OWASP-compliant security hardening, comprehensive input validation, security monitoring, and vulnerability prevention.

## Security Headers and Helmet

```javascript
// src/middleware/security.js — Comprehensive security middleware
import helmet from 'helmet';
import cors from 'cors';
import { config } from '../config/index.js';

// Helmet — sets security headers
export const securityHeaders = helmet({
    contentSecurityPolicy: {
        directives: {
            defaultSrc: ["'self'"],
            scriptSrc: ["'self'"],
            styleSrc: ["'self'", "'unsafe-inline'"],
            imgSrc: ["'self'", 'data:', 'https:'],
            connectSrc: ["'self'"],
            fontSrc: ["'self'"],
            objectSrc: ["'none'"],
            frameSrc: ["'none'"],
        },
    },
    hsts: { maxAge: 31536000, includeSubDomains: true },
    referrerPolicy: { policy: 'strict-origin-when-cross-origin' },
});

// CORS configuration
export const corsConfig = cors({
    origin: config.cors.origin || 'http://localhost:5173',
    methods: ['GET', 'POST', 'PUT', 'DELETE'],
    allowedHeaders: ['Content-Type', 'Authorization', 'X-MFA-Token'],
    credentials: true,
    maxAge: 86400,
});

// Request size limiter
export function requestSizeLimit(maxSize = '1mb') {
    return (req, res, next) => {
        const contentLength = parseInt(req.headers['content-length'] || '0');
        const maxBytes = parseSize(maxSize);

        if (contentLength > maxBytes) {
            return res.status(413).json({ error: 'Request too large' });
        }
        next();
    };
}
```

## Comprehensive Input Validation

```javascript
// src/middleware/validation.js — Input validation and sanitization
import { z } from 'zod';

// Sanitize string input
function sanitizeString(str) {
    return str
        .replace(/[<>]/g, '')           // Remove HTML brackets
        .replace(/javascript:/gi, '')   // Remove JS protocol
        .replace(/on\w+=/gi, '')        // Remove event handlers
        .trim();
}

// Sanitize object recursively
function sanitizeObject(obj) {
    if (typeof obj === 'string') return sanitizeString(obj);
    if (Array.isArray(obj)) return obj.map(sanitizeObject);
    if (obj && typeof obj === 'object') {
        const sanitized = {};
        for (const [key, value] of Object.entries(obj)) {
            sanitized[key] = sanitizeObject(value);
        }
        return sanitized;
    }
    return obj;
}

// Validation middleware factory
export function validate(schema) {
    return (req, res, next) => {
        // Sanitize input first
        req.body = sanitizeObject(req.body);
        req.query = sanitizeObject(req.query);

        // Validate
        const result = schema.safeParse({
            body: req.body,
            query: req.query,
            params: req.params,
        });

        if (!result.success) {
            return res.status(400).json({
                error: 'Validation Error',
                issues: result.error.issues.map(i => ({
                    field: i.path.join('.'),
                    message: i.message,
                })),
            });
        }

        // Replace with validated data
        req.body = result.data.body || req.body;
        req.query = result.data.query || req.query;
        next();
    };
}
```

## SQL Injection Prevention

```javascript
// All database queries use parameterized queries
// src/db/index.js

// SAFE — parameterized query
async function findUser(email) {
    const { rows } = await pool.query(
        'SELECT * FROM users WHERE email = $1',
        [email]  // Parameterized — safe from SQL injection
    );
    return rows[0];
}

// DANGEROUS — string interpolation (NEVER do this)
// const query = `SELECT * FROM users WHERE email = '${email}'`;

// Safe dynamic query builder
function buildBookmarkQuery(userId, filters) {
    let sql = 'SELECT * FROM bookmarks WHERE user_id = $1';
    const params = [userId];
    let paramIndex = 2;

    if (filters.tag) {
        sql += ` AND id IN (SELECT bookmark_id FROM bookmark_tags bt JOIN tags t ON t.id = bt.tag_id WHERE t.name = $${paramIndex})`;
        params.push(filters.tag);
        paramIndex++;
    }

    if (filters.search) {
        sql += ` AND (title ILIKE $${paramIndex} OR description ILIKE $${paramIndex})`;
        params.push(`%${filters.search}%`);
        paramIndex++;
    }

    sql += ` ORDER BY created_at DESC LIMIT $${paramIndex} OFFSET $${paramIndex + 1}`;
    params.push(filters.limit || 20, filters.offset || 0);

    return { sql, params };
}
```

## Security Audit Logging

```javascript
// src/services/audit.js — Security audit logging
import { query } from '../db/index.js';

export class AuditService {
    static async log(event) {
        await query(
            `INSERT INTO audit_log 
             (event_type, user_id, ip_address, user_agent, details, created_at)
             VALUES ($1, $2, $3, $4, $5, NOW())`,
            [
                event.type,
                event.userId,
                event.ip,
                event.userAgent,
                JSON.stringify(event.details || {}),
            ]
        );
    }

    static async getSuspiciousActivity() {
        const { rows } = await query(`
            SELECT ip_address, COUNT(*) as failed_count
            FROM audit_log
            WHERE event_type = 'login_failed'
              AND created_at > NOW() - INTERVAL '1 hour'
            GROUP BY ip_address
            HAVING COUNT(*) > 10
            ORDER BY failed_count DESC
        `);
        return rows;
    }
}

// Audit middleware
export function auditMiddleware(eventType) {
    return async (req, res, next) => {
        const originalEnd = res.end;

        res.end = function (...args) {
            if (res.statusCode >= 400) {
                AuditService.log({
                    type: `${eventType}_failed`,
                    userId: req.user?.userId,
                    ip: req.ip,
                    userAgent: req.headers['user-agent'],
                    details: { path: req.path, status: res.statusCode },
                }).catch(console.error);
            }
            originalEnd.apply(res, args);
        };

        next();
    };
}

// Login audit
app.post('/auth/login',
    auditMiddleware('login'),
    rateLimit({ max: 5, windowMs: 900000 }),
    loginHandler
);
```

## How It Connects

- Security follows [19-security-rate-limiting](../../../19-security-rate-limiting/)
- OWASP follows security best practices from [08-authentication/06-authentication-security/](../../../08-authentication/06-authentication-security/)
- Audit logging follows [21-logging-monitoring](../../../21-logging-monitoring/)

## Common Mistakes

- Not sanitizing input before validation
- Using string interpolation in SQL queries
- Not logging failed authentication attempts
- Missing CORS configuration for production

## Try It Yourself

### Exercise 1: Test SQL Injection
Try injecting SQL through the search parameter.

### Exercise 2: Test XSS
Try injecting script tags through bookmark titles.

### Exercise 3: Audit Review
Review the audit log after failed login attempts.

## Next Steps

Continue to [18-performance-optimization/01-caching-implementation.md](../18-performance-optimization/01-caching-implementation.md).
