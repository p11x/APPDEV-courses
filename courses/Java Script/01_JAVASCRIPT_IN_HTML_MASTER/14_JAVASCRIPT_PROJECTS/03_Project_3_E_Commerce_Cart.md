# 🛒 Project 3: E-Commerce Shopping Cart

## 📋 Project Overview

Build a fully functional shopping cart with product management, quantity controls, price calculations, and local storage persistence. This project demonstrates:
- State management
- DOM manipulation
- Price calculations
- LocalStorage integration
- Event delegation

---

## 🏗️ Architecture Overview

```
shopping-cart/
├── index.html
├── css/
│   └── styles.css
└── js/
    ├── app.js
    ├── cart.js
    └── products.js
```

---

## 🎯 Core Features

### Product Data

```javascript
const products = [
    { id: 1, name: 'Wireless Headphones', price: 99.99, image: '🎧', category: 'electronics' },
    { id: 2, name: 'Smart Watch', price: 199.99, image: '⌚', category: 'electronics' },
    { id: 3, name: 'Running Shoes', price: 79.99, image: '👟', category: 'clothing' },
    { id: 4, name: 'Laptop Backpack', price: 49.99, image: '🎒', category: 'accessories' },
    { id: 5, name: 'Bluetooth Speaker', price: 59.99, image: '🔊', category: 'electronics' },
    { id: 6, name: 'Water Bottle', price: 24.99, image: '🍶', category: 'accessories' }
];
```

### Cart Manager

```javascript
class CartManager {
    constructor() {
        this.items = [];
        this.loadFromStorage();
    }
    
    addItem(product, quantity = 1) {
        const existing = this.items.find(item => item.id === product.id);
        
        if (existing) {
            existing.quantity += quantity;
        } else {
            this.items.push({
                ...product,
                quantity
            });
        }
        
        this.saveToStorage();
        this.notifySubscribers();
    }
    
    removeItem(productId) {
        this.items = this.items.filter(item => item.id !== productId);
        this.saveToStorage();
        this.notifySubscribers();
    }
    
    updateQuantity(productId, quantity) {
        const item = this.items.find(item => item.id === productId);
        
        if (item) {
            if (quantity <= 0) {
                this.removeItem(productId);
            } else {
                item.quantity = quantity;
                this.saveToStorage();
                this.notifySubscribers();
            }
        }
    }
    
    getSubtotal() {
        return this.items.reduce((total, item) => 
            total + (item.price * item.quantity), 0
        );
    }
    
    getTax(rate = 0.08) {
        return this.getSubtotal() * rate;
    }
    
    getTotal() {
        return this.getSubtotal() + this.getTax();
    }
    
    getItemCount() {
        return this.items.reduce((count, item) => count + item.quantity, 0);
    }
    
    saveToStorage() {
        localStorage.setItem('cart', JSON.stringify(this.items));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('cart');
        if (stored) {
            try {
                this.items = JSON.parse(stored);
            } catch (e) {
                this.items = [];
            }
        }
    }
    
    subscribers = [];
    
    subscribe(callback) {
        this.subscribers.push(callback);
    }
    
    notifySubscribers() {
        this.subscribers.forEach(cb => cb(this));
    }
}
```

---

## 🎨 HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Shopping Cart</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="cart-app">
        <header class="cart-header">
            <h1>🛒 Shopping Cart</h1>
            <div class="cart-badge" id="cartBadge">0</div>
        </header>
        
        <div class="cart-container">
            <section class="products-section">
                <h2>Products</h2>
                <div class="products-grid" id="productsGrid"></div>
            </section>
            
            <section class="cart-section">
                <h2>Your Cart</h2>
                <div class="cart-items" id="cartItems"></div>
                
                <div class="cart-summary" id="cartSummary">
                    <div class="summary-row">
                        <span>Subtotal</span>
                        <span id="subtotal">$0.00</span>
                    </div>
                    <div class="summary-row">
                        <span>Tax (8%)</span>
                        <span id="tax">$0.00</span>
                    </div>
                    <div class="summary-row total">
                        <span>Total</span>
                        <span id="total">$0.00</span>
                    </div>
                </div>
                
                <button class="checkout-btn" id="checkoutBtn">
                    Proceed to Checkout
                </button>
                
                <button class="clear-btn" id="clearCartBtn">
                    Clear Cart
                </button>
            </section>
        </div>
    </div>
    
    <script src="js/products.js"></script>
    <script src="js/cart.js"></script>
    <script src="js/app.js"></script>
</body>
</html>
```

---

## 🎨 CSS Styling

```css
:root {
    --primary: #3498db;
    --secondary: #2c3e50;
    --accent: #e74c3c;
    --success: #27ae60;
    --background: #f5f6fa;
    --card-bg: white;
    --text-primary: #2c3e50;
    --text-secondary: #7f8c8d;
}

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', sans-serif;
    background: var(--background);
    padding: 2rem;
}

.cart-app {
    max-width: 1200px;
    margin: 0 auto;
}

.cart-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 2rem;
}

.cart-badge {
    background: var(--accent);
    color: white;
    padding: 0.5rem 1rem;
    border-radius: 20px;
    font-weight: bold;
}

.cart-container {
    display: grid;
    grid-template-columns: 1fr 350px;
    gap: 2rem;
}

.products-section {
    background: var(--card-bg);
    border-radius: 15px;
    padding: 1.5rem;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.products-section h2 {
    margin-bottom: 1rem;
    color: var(--secondary);
}

.products-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    gap: 1rem;
}

.product-card {
    border: 1px solid #eee;
    border-radius: 10px;
    padding: 1rem;
    text-align: center;
    transition: transform 0.2s, box-shadow 0.2s;
}

.product-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0,0,0,0.1);
}

.product-image {
    font-size: 3rem;
    margin-bottom: 0.5rem;
}

.product-name {
    font-weight: bold;
    margin-bottom: 0.5rem;
}

.product-price {
    color: var(--primary);
    font-size: 1.2rem;
    font-weight: bold;
    margin-bottom: 1rem;
}

.add-btn {
    background: var(--primary);
    color: white;
    border: none;
    padding: 0.5rem 1rem;
    border-radius: 5px;
    cursor: pointer;
    transition: background 0.2s;
}

.add-btn:hover {
    background: #2980b9;
}

.cart-section {
    background: var(--card-bg);
    border-radius: 15px;
    padding: 1.5rem;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
    position: sticky;
    top: 2rem;
    height: fit-content;
}

.cart-section h2 {
    margin-bottom: 1rem;
    color: var(--secondary);
}

.cart-items {
    max-height: 400px;
    overflow-y: auto;
    margin-bottom: 1rem;
}

.cart-item {
    display: flex;
    align-items: center;
    padding: 0.75rem;
    border-bottom: 1px solid #eee;
}

.cart-item-image {
    font-size: 1.5rem;
    margin-right: 0.75rem;
}

.cart-item-details {
    flex: 1;
}

.cart-item-name {
    font-size: 0.9rem;
    font-weight: bold;
}

.cart-item-price {
    color: var(--text-secondary);
    font-size: 0.85rem;
}

.quantity-controls {
    display: flex;
    align-items: center;
    gap: 0.25rem;
}

.qty-btn {
    width: 25px;
    height: 25px;
    border: 1px solid #ddd;
    background: white;
    border-radius: 3px;
    cursor: pointer;
}

.qty-value {
    width: 30px;
    text-align: center;
}

.remove-btn {
    background: var(--accent);
    color: white;
    border: none;
    padding: 0.25rem 0.5rem;
    border-radius: 3px;
    cursor: pointer;
    margin-left: 0.5rem;
}

.cart-summary {
    border-top: 2px solid #eee;
    padding-top: 1rem;
}

.summary-row {
    display: flex;
    justify-content: space-between;
    padding: 0.5rem 0;
}

.summary-row.total {
    font-size: 1.2rem;
    font-weight: bold;
    color: var(--primary);
    border-top: 2px solid #eee;
    padding-top: 1rem;
    margin-top: 0.5rem;
}

.checkout-btn {
    width: 100%;
    background: var(--success);
    color: white;
    border: none;
    padding: 1rem;
    border-radius: 10px;
    font-size: 1.1rem;
    font-weight: bold;
    cursor: pointer;
    margin-top: 1rem;
    transition: background 0.2s;
}

.checkout-btn:hover {
    background: #219a53;
}

.clear-btn {
    width: 100%;
    background: transparent;
    color: var(--accent);
    border: 1px solid var(--accent);
    padding: 0.75rem;
    border-radius: 10px;
    cursor: pointer;
    margin-top: 0.5rem;
    transition: all 0.2s;
}

.clear-btn:hover {
    background: var(--accent);
    color: white;
}

.empty-cart {
    text-align: center;
    padding: 2rem;
    color: var(--text-secondary);
}

@media (max-width: 768px) {
    .cart-container {
        grid-template-columns: 1fr;
    }
    
    .cart-section {
        position: static;
    }
}
```

---

## 📊 Features Summary

| Feature | Implementation |
|---------|----------------|
| Product Display | Grid layout with cards |
| Add to Cart | Add products to cart |
| Quantity Controls | Increment/decrement buttons |
| Remove Items | Remove single items |
| Price Calculation | Subtotal, tax, total |
| Local Storage | Persist cart between sessions |
| Item Count | Real-time cart badge |
| Clear Cart | Remove all items |

---

## 🔗 Related Topics

- [05_Element_Creation_and_Manipulation.md](../09_DOM_MANIPULATION/05_Element_Creation_and_Manipulation.md)
- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)
- [24_LocalStorage.md](../11_STORAGE_AND_APIS/24_LocalStorage.md)

---

**Projects Module Progress: 3/32 Complete** 🚀

To continue building more projects, let me know which you'd like next:
- Social Media Feed
- Real-Time Chat
- Finance Dashboard
- Or continue with other modules!