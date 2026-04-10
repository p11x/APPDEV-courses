# E-Commerce Component Suite

## OVERVIEW

Building a complete e-commerce component suite demonstrates real-world Web Component architecture. This guide covers product cards, cart management, checkout forms, and integration patterns.

## Component Suite Architecture

### Product Card

```javascript
class ProductCard extends HTMLElement {
  static get observedAttributes() { 
    return ['title', 'price', 'image', 'rating', 'stock']; 
  }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
  }
  
  attributeChangedCallback() {
    this.render();
  }
  
  get template() {
    return `
      <style>
        :host { display: block; }
        .card {
          border: 1px solid #e0e0e0;
          border-radius: 8px;
          overflow: hidden;
          transition: box-shadow 0.2s;
        }
        .card:hover {
          box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        .image {
          width: 100%;
          aspect-ratio: 1;
          object-fit: cover;
        }
        .content { padding: 16px; }
        .title {
          font-size: 16px;
          font-weight: 600;
          margin: 0 0 8px;
        }
        .price {
          font-size: 20px;
          color: #007bff;
          font-weight: bold;
        }
        .stock {
          font-size: 12px;
          color: ${this.getAttribute('stock') > 10 ? 'green' : 'orange'};
        }
        button {
          width: 100%;
          padding: 12px;
          background: #007bff;
          color: white;
          border: none;
          border-radius: 4px;
          cursor: pointer;
          margin-top: 12px;
        }
      </style>
      <div class="card">
        <img class="image" src="${this.getAttribute('image') || ''}" alt="${this.getAttribute('title') || ''}" />
        <div class="content">
          <h3 class="title">${this.getAttribute('title') || 'Product'}</h3>
          <div class="price">$${this.getAttribute('price') || '0.00'}</div>
          <div class="stock">${this.getAttribute('stock') || 0} in stock</div>
          <button @click="this.addToCart()">Add to Cart</button>
        </div>
      </div>
    `;
  }
  
  addToCart() {
    this.dispatchEvent(new CustomEvent('add-to-cart', {
      bubbles: true,
      composed: true,
      detail: {
        id: this.getAttribute('data-id'),
        title: this.getAttribute('title'),
        price: this.getAttribute('price')
      }
    }));
  }
  
  render() {
    this.shadowRoot.innerHTML = this.template;
    
    // Bind click
    this.shadowRoot.querySelector('button')
      .addEventListener('click', () => this.addToCart());
  }
}
customElements.define('product-card', ProductCard);
```

### Shopping Cart

```javascript
class ShoppingCart extends HTMLElement {
  #items = [];
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.addEventListener('add-to-cart', this.#handleAdd.bind(this));
    this.render();
  }
  
  #handleAdd(e) {
    this.#items.push(e.detail);
    this.render();
    this.#updateTotal();
  }
  
  #removeItem(index) {
    this.#items.splice(index, 1);
    this.render();
    this.#updateTotal();
  }
  
  #updateTotal() {
    const total = this.#items.reduce((sum, item) => sum + parseFloat(item.price), 0);
    this.dispatchEvent(new CustomEvent('cart-updated', {
      bubbles: true,
      composed: true,
      detail: { total, count: this.#items.length }
    }));
  }
  
  render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host { display: block; }
        .cart { border: 1px solid #ddd; border-radius: 8px; padding: 16px; }
        .item { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #eee; }
        .total { font-size: 20px; font-weight: bold; text-align: right; margin-top: 16px; }
      </style>
      <div class="cart">
        <h2>Shopping Cart</h2>
        ${this.#items.length === 0 ? '<p>Cart is empty</p>' : 
          this.#items.map((item, i) => `
            <div class="item">
              <span>${item.title}</span>
              <span>$${item.price} <button data-index="${i}">×</button></span>
            </div>
          `).join('')
        }
        <div class="total">Total: $${this.#items.reduce((s, i) => s + parseFloat(i.price), 0).toFixed(2)}</div>
      </div>
    `;
    
    // Bind remove buttons
    this.shadowRoot.querySelectorAll('button[data-index]').forEach(btn => {
      btn.addEventListener('click', () => this.#removeItem(parseInt(btn.dataset.index)));
    });
  }
}
customElements.define('shopping-cart', ShoppingCart);
```

## NEXT STEPS

Proceed to **11_Real-World-Applications/11_2_Dashboard-Widget-System**.