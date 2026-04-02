# Express Router Internals and Implementation

## What You'll Learn

- Router internal implementation
- Route matching algorithm
- Router composition patterns
- Performance optimization for routing

## Router Internal Implementation

```javascript
// Simplified Express Router implementation
class Router {
    constructor() {
        this.stack = []; // Layers (middleware + routes)
    }

    // Add route
    get(path, ...handlers) {
        this.route(path).get(...handlers);
    }

    post(path, ...handlers) {
        this.route(path).post(...handlers);
    }

    // Create Route object
    route(path) {
        const route = new Route(path);
        this.stack.push(route);
        return route;
    }

    // Add middleware
    use(path, ...handlers) {
        if (typeof path === 'function') {
            handlers.unshift(path);
            path = '/';
        }

        for (const handler of handlers) {
            this.stack.push({
                type: 'middleware',
                path,
                handle: handler
            });
        }
    }

    // Match request to routes
    match(method, url) {
        const matches = [];

        for (const layer of this.stack) {
            if (layer.type === 'middleware') {
                if (url.startsWith(layer.path)) {
                    matches.push(layer);
                }
            } else if (layer.type === 'route') {
                if (layer.methods[method] && layer.match(url)) {
                    matches.push(layer);
                }
            }
        }

        return matches;
    }
}
```

## Route Composition

```javascript
// Modular route organization
// routes/users.js
import { Router } from 'express';
const router = Router();

router.get('/', listUsers);
router.get('/:id', getUser);
router.post('/', createUser);
router.put('/:id', updateUser);
router.delete('/:id', deleteUser);

export default router;

// routes/products.js
import { Router } from 'express';
const router = Router();

router.get('/', listProducts);
router.get('/:id', getProduct);
router.post('/', createProduct);

export default router;

// app.js
import userRoutes from './routes/users.js';
import productRoutes from './routes/products.js';

app.use('/api/users', userRoutes);
app.use('/api/products', productRoutes);
```

## Router-Level Middleware

```javascript
import { Router } from 'express';

const adminRouter = Router();

// Middleware applied to all admin routes
adminRouter.use(authenticate);
adminRouter.use(authorize('admin'));

adminRouter.get('/dashboard', getDashboard);
adminRouter.get('/users', listAllUsers);
adminRouter.post('/settings', updateSettings);

app.use('/admin', adminRouter);
```

## Best Practices Checklist

- [ ] Use Router for modular route organization
- [ ] Apply middleware at router level when possible
- [ ] Keep route definitions clean and readable
- [ ] Use route composition for complex APIs
- [ ] Test route matching thoroughly

## Cross-References

- See [Lifecycle](./01-lifecycle-deep-dive.md) for request flow
- See [Advanced Routing](../02-advanced-routing/01-route-parameters.md) for routing patterns
- See [Middleware](../03-middleware-guide/01-custom-middleware.md) for middleware

## Next Steps

Continue to [Advanced Routing](../02-advanced-routing/01-route-parameters.md) for routing patterns.
