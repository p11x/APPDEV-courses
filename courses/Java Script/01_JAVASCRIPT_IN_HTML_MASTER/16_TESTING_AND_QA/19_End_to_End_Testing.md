# 🧭 End to End Testing

## 📋 Overview

End-to-End (E2E) testing verifies that an application works as expected from start to finish, simulating real user interactions in a browser environment.

---

## 🎯 E2E Testing Tools

### Popular Tools Comparison

| Tool | Pros | Cons |
|------|------|------|
| **Cypress** | Great API, good docs | No multi-tab support |
| **Playwright** | Cross-browser, fast | Newer, smaller community |
| **Puppeteer** | Google support, flexible | Manual waiting |
| **Selenium** | Cross-browser, mature | Slower, complex setup |

---

## 🎯 Cypress Basics

### Installation

```bash
npm install cypress --save-dev
npx cypress open
```

### First Test

```javascript
// cypress/integration/todo.spec.js

describe('Todo App E2E', () => {
    beforeEach(() => {
        cy.visit('http://localhost:3000');
    });
    
    it('should add a new todo', () => {
        // Type in input
        cy.get('[data-testid="todo-input"]')
            .type('Learn Cypress');
        
        // Submit form
        cy.get('[data-testid="add-button"]')
            .click();
        
        // Verify todo was added
        cy.get('[data-testid="todo-list"]')
            .should('contain', 'Learn Cypress');
    });
    
    it('should mark todo as complete', () => {
        // Add todo first
        cy.get('[data-testid="todo-input"]').type('Test todo');
        cy.get('[data-testid="add-button"]').click();
        
        // Click checkbox
        cy.get('[data-testid="todo-checkbox"]')
            .first()
            .click();
        
        // Verify completed style
        cy.get('[data-testid="todo-item"]')
            .first()
            .should('have.class', 'completed');
    });
    
    it('should delete a todo', () => {
        // Add todo
        cy.get('[data-testid="todo-input"]').type('Delete me');
        cy.get('[data-testid="add-button"]').click();
        
        // Click delete button
        cy.get('[data-testid="delete-button"]')
            .first()
            .click();
        
        // Verify deleted
        cy.get('[data-testid="todo-list"]')
            .should('not.contain', 'Delete me');
    });
});
```

---

## 🎯 Playwright Basics

### Installation

```bash
npm install @playwright/test --save-dev
npx playwright install chromium
```

### First Test

```javascript
// tests/example.spec.js
const { test, expect } = require('@playwright/test');

test('has title', async ({ page }) => {
    await page.goto('http://localhost:3000');
    await expect(page).toHaveTitle(/Todo App/);
});

test('can add todo', async ({ page }) => {
    await page.goto('http://localhost:3000');
    
    // Fill input
    await page.fill('[data-testid="todo-input"]', 'Playwright test');
    
    // Click add button
    await page.click('[data-testid="add-button"]');
    
    // Verify added
    await expect(page.locator('[data-testid="todo-list"]'))
        .toContainText('Playwright test');
});
```

---

## 🎯 Advanced Interactions

### Waiting for Elements

```javascript
// Cypress - explicit wait
cy.get('.loading').should('not.exist');

// Cypress - wait for API call
cy.intercept('GET', '/api/todos').as('getTodos');
cy.visit('/');
cy.wait('@getTodos');

// Playwright - wait for navigation
await page.goto('/page');
await page.waitForLoadState('networkidle');

// Playwright - wait for element
await page.waitForSelector('.loaded-content');
```

### Handling Forms

```javascript
// Cypress form testing
cy.get('#email').type('test@example.com');
cy.get('#password').type('password123');
cy.get('form').submit();
cy.url().should('include', '/dashboard');

// Playwright form testing
await page.fill('#email', 'test@example.com');
await page.fill('#password', 'password123');
await page.click('button[type="submit"]');
await expect(page).toHaveURL(/dashboard/);
```

### Handling Dialogs

```javascript
// Cypress
cy.on('window:confirm', () => true);
cy.get('.delete-btn').click();

// Playwright
page.on('dialog', dialog => dialog.accept());
await page.click('.delete-btn');
```

---

## 🎯 Assertions

### Common Assertions

```javascript
// Text content
cy.contains('Hello World');
cy.get('h1').should('have.text', 'Title');

// Visibility
cy.get('.hidden').should('not.be.visible');
cy.get('.visible').should('be.visible');

// Classes
cy.get('.item').should('have.class', 'active');
cy.get('.item').should('not.have.class', 'disabled');

// Attributes
cy.get('input').should('have.attr', 'type', 'text');
cy.get('a').should('have.attr', 'href', '/about');

// Count
cy.get('.items').should('have.length', 5);
```

---

## 🔗 Related Topics

- [01_Automated_Testing_Framework.md](./01_Automated_Testing_Framework.md)
- [18_Integration_Testing.md](./18_Integration_Testing.md)

---

**Next: [Test Driven Development](./20_Test_Driven_Development.md)**