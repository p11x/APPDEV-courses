# ⏳ Async/Await Master Class

## 📋 Overview

Async/await is syntactic sugar built on top of Promises that makes asynchronous code look and behave more like synchronous code. It was introduced in ES2017 and is now the standard way to handle async operations.

---

## 🎯 Basic Syntax

### async Function Declaration

```javascript
// async function declaration
async function fetchData() {
    return 'data';
}

// async function expression
const fetchData = async function() {
    return 'data';
};

// async arrow function
const fetchData = async () => {
    return 'data';
};

// async class method
class Api {
    async getData() {
        return 'data';
    }
}
```

### await Keyword

```javascript
// Without async/await (Promise chain)
function getData() {
    fetch('/api/data')
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error(error));
}

// With async/await (much cleaner!)
async function getData() {
    try {
        const response = await fetch('/api/data');
        const data = await response.json();
        console.log(data);
    } catch (error) {
        console.error(error);
    }
}
```

---

## 🌍 Real-World Applications

### Scenario 1: User Profile Loader

```javascript
class UserProfileLoader {
    constructor() {
        this.baseURL = 'https://api.example.com';
    }
    
    async loadUserProfile(userId) {
        try {
            // Sequential calls (one after another)
            const user = await this.fetchUser(userId);
            const posts = await this.fetchUserPosts(userId);
            const friends = await this.fetchUserFriends(userId);
            
            return {
                user,
                posts,
                friends,
                loadedAt: new Date().toISOString()
            };
        } catch (error) {
            console.error('Failed to load profile:', error);
            throw error;
        }
    }
    
    async fetchUser(id) {
        const response = await fetch(`${this.baseURL}/users/${id}`);
        if (!response.ok) throw new Error('User not found');
        return response.json();
    }
    
    async fetchUserPosts(userId) {
        const response = await fetch(`${this.baseURL}/users/${userId}/posts`);
        return response.json();
    }
    
    async fetchUserFriends(userId) {
        const response = await fetch(`${this.baseURL}/users/${userId}/friends`);
        return response.json();
    }
}

// Usage
const loader = new UserProfileLoader();
loader.loadUserProfile(123)
    .then(profile => {
        console.log(`User: ${profile.user.name}`);
        console.log(`Posts: ${profile.posts.length}`);
        console.log(`Friends: ${profile.friends.length}`);
    })
    .catch(error => console.error(error));
```

### Scenario 2: Shopping Cart Checkout

```javascript
class CheckoutService {
    async processCheckout(cart, paymentInfo) {
        try {
            // Validate cart
            const validatedCart = await this.validateCart(cart);
            
            // Process payment
            const paymentResult = await this.processPayment(
                validatedCart.total,
                paymentInfo
            );
            
            // Create order
            const order = await this.createOrder({
                cart: validatedCart,
                payment: paymentResult
            });
            
            // Send confirmation
            await this.sendConfirmation(order);
            
            return { success: true, order };
        } catch (error) {
            await this.handleCheckoutError(error, cart);
            throw error;
        }
    }
    
    async validateCart(cart) {
        // Simulate validation
        await this.delay(500);
        if (cart.items.length === 0) {
            throw new Error('Cart is empty');
        }
        return { ...cart, valid: true };
    }
    
    async processPayment(amount, paymentInfo) {
        await this.delay(1000);
        return { transactionId: 'TXN-' + Date.now(), amount };
    }
    
    async createOrder(orderData) {
        await this.delay(500);
        return { orderId: 'ORD-' + Date.now(), ...orderData };
    }
    
    async sendConfirmation(order) {
        await this.delay(300);
        console.log('Confirmation sent!');
    }
    
    delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    
    async handleCheckoutError(error, cart) {
        console.error('Checkout failed:', error.message);
        // Could implement rollback logic here
    }
}
```

---

## ⚡ Parallel Execution

### Promise.all with async/await

```javascript
// ❌ Sequential (slow - each waits for previous)
async function getSlowData() {
    const user = await fetch('/api/user').then(r => r.json());
    const posts = await fetch('/api/posts').then(r => r.json());
    const settings = await fetch('/api/settings').then(r => r.json());
    return { user, posts, settings };
}

// ✅ Parallel (fast - all requests start together)
async function getFastData() {
    const [userResponse, postsResponse, settingsResponse] = await Promise.all([
        fetch('/api/user'),
        fetch('/api/posts'),
        fetch('/api/settings')
    ]);
    
    const user = await userResponse.json();
    const posts = await postsResponse.json();
    const settings = await settingsResponse.json();
    
    return { user, posts, settings };
}
```

### Promise.allSettled

```javascript
async function loadAllWidgets() {
    const results = await Promise.allSettled([
        fetch('/api/widget/1').then(r => r.json()),
        fetch('/api/widget/2').then(r => r.json()),
        fetch('/api/widget/3').then(r => r.json())
    ]);
    
    const successful = results
        .filter(r => r.status === 'fulfilled')
        .map(r => r.value);
    
    const failed = results
        .filter(r => r.status === 'rejected')
        .map(r => r.reason);
    
    console.log(`Loaded: ${successful.length}, Failed: ${failed.length}`);
    
    return successful;
}
```

---

## 🔄 Error Handling

### try...catch Pattern

```javascript
async function fetchWithErrorHandling(url) {
    try {
        const response = await fetch(url);
        
        if (!response.ok) {
            // Handle HTTP errors
            throw new Error(`HTTP ${response.status}: ${response.statusText}`);
        }
        
        const data = await response.json();
        return data;
        
    } catch (error) {
        // Network errors, JSON parse errors, etc.
        if (error.name === 'TypeError') {
            console.error('Network error:', error.message);
        } else {
            console.error('Error:', error.message);
        }
        throw error; // Re-throw to caller
    }
}
```

### Error Boundaries

```javascript
async function safeAsyncOperation(operation, fallback = null) {
    try {
        return await operation();
    } catch (error) {
        console.warn('Async operation failed:', error.message);
        return fallback;
    }
}

// Usage
const user = await safeAsyncOperation(
    () => fetchUser(123),
    { name: 'Guest', id: null }
);
```

---

## 🎯 Best Practices

### Always use try...catch

```javascript
// ❌ Bad - unhandled promise rejection
async function bad() {
    const data = await fetch('/api/data'); // Could throw!
    return data;
}

// ✅ Good - proper error handling
async function good() {
    try {
        const data = await fetch('/api/data');
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
```

### Don't forget await

```javascript
// ❌ Bug - returns promise, not data!
async function getData() {
    return fetch('/api/data').then(r => r.json());
}

const data = getData(); // data is a Promise, not the actual data!

// ✅ Correct
async function getData() {
    return await fetch('/api/data').then(r => r.json());
}

const data = await getData(); // data is the actual data
```

---

## 🎯 Practice Exercise

### Async Data Pipeline

```javascript
class DataPipeline {
    constructor() {
        this.steps = [];
    }
    
    addStep(name, fn) {
        this.steps.push({ name, fn });
        return this;
    }
    
    async execute(initialData) {
        let data = initialData;
        
        for (const step of this.steps) {
            console.log(`Executing: ${step.name}`);
            try {
                data = await step.fn(data);
            } catch (error) {
                throw new Error(`Step "${step.name}" failed: ${error.message}`);
            }
        }
        
        return data;
    }
}

// Usage
const pipeline = new DataPipeline()
    .addStep('Fetch', async data => {
        const response = await fetch(data.url);
        return response.json();
    })
    .addStep('Transform', async data => {
        return data.map(item => ({ ...item, processed: true }));
    })
    .addStep('Validate', async data => {
        if (!data.length) throw new Error('No data');
        return data;
    });

const result = await pipeline.execute({ url: '/api/items' });
console.log('Processed:', result.length, 'items');
```

---

## 🔗 Related Topics

- [02_Promises_Complete_Guide.md](./02_Promises_Complete_Guide.md)
- [04_Promise_Chaining_Techniques.md](./04_Promise_Chaining_Techniques.md)

---

**Next: Learn about [Promise Chaining Techniques](./04_Promise_Chaining_Techniques.md)**