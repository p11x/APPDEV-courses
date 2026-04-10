# 🛒 Project 23: Marketplace Platform

## E-Commerce with Vendor Management

---

## Table of Contents

1. [Product Management](#product-management)
2. [Shopping Cart](#shopping-cart)
3. [Checkout System](#checkout-system)
4. [Vendor Dashboard](#vendor-dashboard)

---

## Product Management

### Product Schema

```javascript
const productSchema = {
  id: 'prod-001',
  vendorId: 'vendor-001',
  name: 'Wireless Headphones',
  description: 'High-quality wireless headphones',
  price: 99.99,
  comparePrice: 149.99,
  category: 'electronics',
  images: ['/images/headphones1.jpg', '/images/headphones2.jpg'],
  stock: 50,
  sku: 'WH-001',
  tags: ['wireless', 'bluetooth'],
  attributes: {
    color: 'black',
    brand: 'AudioTech'
  },
  rating: 4.5,
  reviews: 128
};
```

### Product Card

```javascript
class ProductCard {
  constructor(product) {
    this.product = product;
  }
  
  render() {
    const discount = this.product.comparePrice 
      ? Math.round((1 - this.product.price / this.product.comparePrice) * 100)
      : 0;
    
    return `
      <div class="product-card">
        <img src="${this.product.images[0]}" alt="${this.product.name}">
        <h3>${this.product.name}</h3>
        <div class="price">
          <span class="current">$${this.product.price}</span>
          ${discount > 0 ? `<span class="original">$${this.product.comparePrice}</span>
          <span class="discount">-${discount}%</span>` : ''}
        </div>
        <div class="rating">${'★'.repeat(Math.floor(this.product.rating))}</div>
        <button data-action="add-to-cart" data-product="${this.product.id}">
          Add to Cart
        </button>
      </div>
    `;
  }
}
```

---

## Shopping Cart

### Cart Manager

```javascript
class CartManager {
  constructor() {
    this.items = [];
    this.load();
  }
  
  add(product, quantity = 1) {
    const existing = this.items.find(item => item.product.id === product.id);
    
    if (existing) {
      existing.quantity += quantity;
    } else {
      this.items.push({ product, quantity });
    }
    
    this.save();
    this.updateUI();
  }
  
  remove(productId) {
    this.items = this.items.filter(item => item.product.id !== productId);
    this.save();
    this.updateUI();
  }
  
  getTotal() {
    return this.items.reduce((sum, item) => 
      sum + (item.product.price * item.quantity), 0
    );
  }
  
  getItemCount() {
    return this.items.reduce((sum, item) => sum + item.quantity, 0);
  }
}
```

---

## Checkout System

### Checkout Flow

```javascript
class Checkout {
  constructor(cart) {
    this.cart = cart;
    this.step = 'shipping';
    this.data = {
      shipping: {},
      payment: {},
      billing: {}
    };
  }
  
  async process() {
    switch (this.step) {
      case 'shipping':
        await this.collectShipping();
        break;
      case 'payment':
        await this.collectPayment();
        break;
      case 'review':
        await this.reviewOrder();
        break;
      case 'confirmation':
        return await this.createOrder();
    }
  }
  
  async createOrder() {
    const order = {
      items: this.cart.items,
      total: this.cart.getTotal(),
      shipping: this.data.shipping,
      payment: this.data.payment,
      status: 'pending',
      createdAt: new Date()
    };
    
    return await API.createOrder(order);
  }
}
```

---

## Vendor Dashboard

### Vendor Product Management

```javascript
class VendorDashboard {
  constructor(vendorId) {
    this.vendorId = vendorId;
  }
  
  async getStats() {
    return {
      totalSales: await API.getVendorSales(this.vendorId),
      orders: await API.getVendorOrders(this.vendorId),
      products: await API.getVendorProducts(this.vendorId),
      rating: await API.getVendorRating(this.vendorId)
    };
  }
  
  async updateInventory(productId, stock) {
    await API.updateProduct(productId, { stock });
  }
}
```

---

## Summary

### Key Takeaways

1. **Products**: Structured data
2. **Cart**: State management
3. **Checkout**: Multi-step flow

### Next Steps

- Continue with: [09_PROJECT_TRAVEL_BOOKING.md](09_PROJECT_TRAVEL_BOOKING.md)
- Add reviews system
- Implement search

---

## Cross-References

- **Previous**: [07_PROJECT_WEATHER_STATION.md](07_PROJECT_WEATHER_STATION.md)
- **Next**: [09_PROJECT_TRAVEL_BOOKING.md](09_PROJECT_TRAVEL_BOOKING.md)

---

*Last updated: 2024*