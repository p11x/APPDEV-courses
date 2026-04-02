# E2E Testing with Cypress and User Journey Testing

## What You'll Learn

- Cypress setup and configuration
- User journey testing patterns
- Cross-browser testing strategies
- Component testing with Cypress

## Cypress Setup

```bash
npm install --save-dev cypress
npx cypress open
```

```javascript
// cypress.config.js
import { defineConfig } from 'cypress';

export default defineConfig({
    e2e: {
        baseUrl: 'http://localhost:3000',
        specPattern: 'cypress/e2e/**/*.cy.js',
        supportFile: 'cypress/support/e2e.js',
        viewportWidth: 1280,
        viewportHeight: 720,
        video: false,
        screenshotOnRunFailure: true,
        retries: {
            runMode: 2,
            openMode: 0,
        },
    },
});
```

## User Journey Tests

```javascript
// cypress/e2e/user-journey.cy.js
describe('User Registration and Login Journey', () => {
    beforeEach(() => {
        cy.task('db:seed');
    });

    it('registers, logs out, and logs back in', () => {
        // Step 1: Register
        cy.visit('/register');
        cy.get('[data-cy=name]').type('Test User');
        cy.get('[data-cy=email]').type('test@example.com');
        cy.get('[data-cy=password]').type('SecurePass123!');
        cy.get('[data-cy=confirm-password]').type('SecurePass123!');
        cy.get('[data-cy=submit]').click();

        cy.url().should('include', '/dashboard');
        cy.contains('Welcome, Test User');

        // Step 2: Logout
        cy.get('[data-cy=logout]').click();
        cy.url().should('include', '/login');

        // Step 3: Login
        cy.get('[data-cy=email]').type('test@example.com');
        cy.get('[data-cy=password]').type('SecurePass123!');
        cy.get('[data-cy=submit]').click();

        cy.url().should('include', '/dashboard');
        cy.contains('Welcome, Test User');
    });

    it('shows error for invalid login', () => {
        cy.visit('/login');
        cy.get('[data-cy=email]').type('wrong@example.com');
        cy.get('[data-cy=password]').type('WrongPass');
        cy.get('[data-cy=submit]').click();

        cy.get('[data-cy=error]').should('contain', 'Invalid credentials');
        cy.url().should('include', '/login');
    });
});

describe('Shopping Journey', () => {
    beforeEach(() => {
        cy.task('db:seed');
        cy.login('user@example.com', 'Pass123!');
    });

    it('completes full shopping flow', () => {
        // Browse products
        cy.visit('/products');
        cy.get('[data-cy=product-card]').should('have.length.greaterThan', 0);

        // Add to cart
        cy.get('[data-cy=add-to-cart]').first().click();
        cy.get('[data-cy=cart-count]').should('contain', '1');

        // View cart
        cy.get('[data-cy=cart-icon]').click();
        cy.url().should('include', '/cart');
        cy.get('[data-cy=cart-item]').should('have.length', 1);

        // Checkout
        cy.get('[data-cy=checkout]').click();
        cy.url().should('include', '/checkout');

        // Fill shipping
        cy.get('[data-cy=address]').type('123 Main St');
        cy.get('[data-cy=city]').type('New York');
        cy.get('[data-cy=zip]').type('10001');

        // Place order
        cy.get('[data-cy=place-order]').click();
        cy.get('[data-cy=order-success]').should('be.visible');
        cy.contains('Order confirmed');
    });
});
```

## Custom Commands

```javascript
// cypress/support/commands.js
Cypress.Commands.add('login', (email, password) => {
    cy.request('POST', '/auth/login', { email, password }).then((res) => {
        window.localStorage.setItem('token', res.body.token);
    });
});

Cypress.Commands.add('createUser', (overrides = {}) => {
    const user = {
        name: 'Test User',
        email: `user-${Date.now()}@test.com`,
        password: 'Pass123!',
        ...overrides,
    };
    return cy.request('POST', '/auth/register', user).then((res) => res.body);
});

Cypress.Commands.add('getByCy', (selector) => {
    return cy.get(`[data-cy=${selector}]`);
});
```

## Database Seeding

```javascript
// cypress/plugins/index.js
export default (on, config) => {
    on('task', {
        async 'db:seed'() {
            const { Pool } = require('pg');
            const pool = new Pool({ connectionString: config.env.DATABASE_URL });

            await pool.query('DELETE FROM orders');
            await pool.query('DELETE FROM users');
            await pool.query(`
                INSERT INTO users (name, email, password_hash) VALUES
                ('Test User', 'user@example.com', '$2b$10$...')
            `);
            await pool.query(`
                INSERT INTO products (name, price) VALUES
                ('Widget', 9.99),
                ('Gadget', 19.99)
            `);

            await pool.end();
            return null;
        },
    });
};
```

## Common Mistakes

- Not seeding database before tests
- Using cy.wait() instead of proper assertions
- Not testing error states
- Hardcoding test data instead of using fixtures

## Cross-References

- See [Playwright](./01-playwright-e2e.md) for Playwright comparison
- See [Testing Production](../11-testing-production/01-canary-feature-flags.md) for prod testing
- See [API Testing](../06-api-testing/01-rest-graphql.md) for API E2E

## Next Steps

Continue to [API Testing: WebSocket and Contract](../06-api-testing/02-websocket-contract.md).
