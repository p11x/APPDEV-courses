# Express.js Enterprise Authentication Patterns

## What You'll Learn

- OAuth 2.0 implementation
- JWT authentication
- Role-based access control
- Multi-factor authentication

## JWT Authentication

```bash
npm install jsonwebtoken bcrypt
```

```javascript
import jwt from 'jsonwebtoken';
import bcrypt from 'bcrypt';

const JWT_SECRET = process.env.JWT_SECRET;
const JWT_EXPIRES = '15m';
const REFRESH_EXPIRES = '7d';

function generateTokens(user) {
    const accessToken = jwt.sign(
        { sub: user.id, role: user.role },
        JWT_SECRET,
        { expiresIn: JWT_EXPIRES }
    );

    const refreshToken = jwt.sign(
        { sub: user.id, type: 'refresh' },
        JWT_SECRET,
        { expiresIn: REFRESH_EXPIRES }
    );

    return { accessToken, refreshToken };
}

function authenticate(req, res, next) {
    const token = req.headers.authorization?.split(' ')[1];
    if (!token) return res.status(401).json({ error: 'No token' });

    try {
        req.user = jwt.verify(token, JWT_SECRET);
        next();
    } catch {
        res.status(401).json({ error: 'Invalid token' });
    }
}

function authorize(...roles) {
    return (req, res, next) => {
        if (!roles.includes(req.user.role)) {
            return res.status(403).json({ error: 'Forbidden' });
        }
        next();
    };
}
```

## Role-Based Access Control

```javascript
const ROLES = {
    ADMIN: 'admin',
    MANAGER: 'manager',
    USER: 'user',
};

app.get('/admin/users', authenticate, authorize(ROLES.ADMIN), listUsers);
app.get('/manager/reports', authenticate, authorize(ROLES.ADMIN, ROLES.MANAGER), getReports);
app.get('/profile', authenticate, getProfile);
```

## Best Practices Checklist

- [ ] Use short-lived access tokens
- [ ] Implement refresh token rotation
- [ ] Hash passwords with bcrypt (cost 12+)
- [ ] Implement rate limiting on auth endpoints
- [ ] Use HTTPS for all auth endpoints

## Cross-References

- See [Security](../05-security-implementation/01-helmet-cors.md) for security headers
- See [Middleware](../03-middleware-guide/01-custom-middleware.md) for middleware
- See [Error Handling](../08-error-handling/01-centralized-errors.md) for errors

## Next Steps

Continue to [Deployment Operations](../16-deployment-operations/01-production-deployment.md) for deployment.
