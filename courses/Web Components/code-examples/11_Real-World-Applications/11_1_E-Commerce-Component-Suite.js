/**
 * E-Commerce Component Suite - Complete shopping cart, product display, and checkout components with Indian context (INR, UPI)
 * @module real-world/11_1_E-Commerce-Component-Suite
 * @version 1.0.0
 * @example <shopping-cart></shopping-cart>
 */

class ShoppingCart extends HTMLElement {
  constructor() {
    super();
    this.items = [];
    this.currency = 'INR';
    this.locale = 'en-IN';
  }

  static get observedAttributes() {
    return ['currency', 'show-summary'];
  }

  static get styles() {
    return `
      :host {
        display: block;
        font-family: 'Poppins', -apple-system, BlinkMacSystemFont, sans-serif;
      }
      .cart-container {
        background: #fff;
        border-radius: 12px;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
        overflow: hidden;
      }
      .cart-header {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 20px 24px;
        display: flex;
        justify-content: space-between;
        align-items: center;
      }
      .cart-header h2 {
        margin: 0;
        font-size: 1.5rem;
        font-weight: 600;
      }
      .item-count {
        background: rgba(255,255,255,0.2);
        padding: 4px 12px;
        border-radius: 20px;
        font-size: 0.875rem;
      }
      .cart-items {
        max-height: 400px;
        overflow-y: auto;
      }
      .cart-item {
        display: flex;
        padding: 16px 24px;
        border-bottom: 1px solid #f0f0f0;
        transition: background 0.2s;
      }
      .cart-item:hover {
        background: #f8f9fa;
      }
      .item-image {
        width: 80px;
        height: 80px;
        border-radius: 8px;
        object-fit: cover;
        background: #e9ecef;
      }
      .item-details {
        flex: 1;
        margin-left: 16px;
        display: flex;
        flex-direction: column;
        justify-content: center;
      }
      .item-name {
        font-weight: 600;
        color: #212529;
        margin-bottom: 4px;
      }
      .item-price {
        color: #6c757d;
        font-size: 0.875rem;
      }
      .item-quantity {
        display: flex;
        align-items: center;
        gap: 8px;
      }
      .quantity-btn {
        width: 32px;
        height: 32px;
        border: 1px solid #dee2e6;
        background: #fff;
        border-radius: 6px;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.2s;
      }
      .quantity-btn:hover {
        background: #667eea;
        color: white;
        border-color: #667eea;
      }
      .quantity-value {
        min-width: 32px;
        text-align: center;
        font-weight: 600;
      }
      .item-total {
        text-align: right;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: flex-end;
      }
      .item-total-price {
        font-weight: 700;
        color: #212529;
        font-size: 1.125rem;
      }
      .remove-btn {
        background: none;
        border: none;
        color: #dc3545;
        cursor: pointer;
        font-size: 0.875rem;
        margin-top: 4px;
      }
      .cart-summary {
        padding: 24px;
        background: #f8f9fa;
        border-top: 2px solid #dee2e6;
      }
      .summary-row {
        display: flex;
        justify-content: space-between;
        margin-bottom: 12px;
        color: #495057;
      }
      .summary-row.total {
        font-size: 1.25rem;
        font-weight: 700;
        color: #212529;
        padding-top: 12px;
        border-top: 2px solid #dee2e6;
      }
      .checkout-btn {
        width: 100%;
        padding: 16px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        margin-top: 16px;
        transition: transform 0.2s, box-shadow 0.2s;
      }
      .checkout-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
      }
      .checkout-btn:disabled {
        background: #dee2e6;
        cursor: not-allowed;
        transform: none;
        box-shadow: none;
      }
      .empty-cart {
        padding: 48px 24px;
        text-align: center;
        color: #6c757d;
      }
      .empty-cart svg {
        width: 64px;
        height: 64px;
        margin-bottom: 16px;
        fill: #dee2e6;
      }
      .payment-options {
        display: flex;
        gap: 8px;
        margin-top: 16px;
        flex-wrap: wrap;
      }
      .payment-option {
        flex: 1;
        min-width: 100px;
        padding: 12px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        transition: all 0.2s;
      }
      .payment-option.selected {
        border-color: #667eea;
        background: rgba(102, 126, 234, 0.1);
      }
      .payment-option:hover {
        border-color: #667eea;
      }
      .payment-icon {
        font-size: 1.5rem;
        margin-bottom: 4px;
      }
      .payment-label {
        font-size: 0.75rem;
        color: #495057;
      }
      @keyframes slideIn {
        from {
          opacity: 0;
          transform: translateY(10px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }
      .cart-item {
        animation: slideIn 0.3s ease-out;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'currency') {
        this.currency = newValue;
        this.render();
      }
    }
  }

  formatPrice(amount) {
    return new Intl.NumberFormat(this.locale, {
      style: 'currency',
      currency: this.currency,
    }).format(amount);
  }

  addItem(product) {
    const existingItem = this.items.find(item => item.id === product.id);
    if (existingItem) {
      existingItem.quantity += 1;
    } else {
      this.items.push({ ...product, quantity: 1 });
    }
    this.dispatchEvent(new CustomEvent('cart-updated', {
      detail: { items: this.items },
      bubbles: true,
      composed: true,
    }));
    this.render();
  }

  removeItem(productId) {
    this.items = this.items.filter(item => item.id !== productId);
    this.dispatchEvent(new CustomEvent('cart-updated', {
      detail: { items: this.items },
      bubbles: true,
      composed: true,
    }));
    this.render();
  }

  updateQuantity(productId, quantity) {
    const item = this.items.find(item => item.id === productId);
    if (item) {
      item.quantity = Math.max(1, quantity);
      this.render();
    }
  }

  getTotal() {
    return this.items.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  getItemCount() {
    return this.items.reduce((count, item) => count + item.quantity, 0);
  }

  render() {
    const showSummary = this.getAttribute('show-summary') !== 'false';
    const total = this.getTotal();
    const itemCount = this.getItemCount();

    this.shadowRoot.innerHTML = `
      <style>${ShoppingCart.styles}</style>
      <div class="cart-container">
        <div class="cart-header">
          <h2>🛒 Shopping Cart</h2>
          <span class="item-count">${itemCount} items</span>
        </div>
        ${this.items.length === 0 ? `
          <div class="empty-cart">
            <svg viewBox="0 0 24 24"><path d="M7 18c-1.1 0-1.99.9-1.99 2S5.9 22 7 22s2-.9 2-2-.9-2-2-2zM1 2v2h2l3.6 7.59-1.35 2.45c-.16.28-.25.61-.25.96 0 1.1.9 2 2 2h12v-2H7.42c-.14 0-.25-.11-.25-.25l.03-.12.9-1.63h7.45c.75 0 1.41-.41 1.75-1.03l3.58-6.49c.08-.14.12-.31.12-.48 0-.55-.45-1-1-1H5.21l-.94-2H1zm16 16c-1.1 0-1.99.9-1.99 2s.89 2 1.99 2 2-.9 2-2-.9-2-2-2z"/></svg>
            <p>Your cart is empty</p>
          </div>
        ` : `
          <div class="cart-items">
            ${this.items.map(item => `
              <div class="cart-item" data-id="${item.id}">
                <img class="item-image" src="${item.image || 'data:image/svg+xml,<svg xmlns=%22http://www.w3.org/2000/svg%22 viewBox=%220 0 100 100%22><rect fill=%22%23e9ecef%22 width=%22100%22 height=%22100%22/><text fill=%22%23adb5bd%22 x=%2250%22 y=%2255%22 text-anchor=%22middle%22>No Image</text></svg>'}" alt="${item.name}">
                <div class="item-details">
                  <div class="item-name">${item.name}</div>
                  <div class="item-price">${this.formatPrice(item.price)}</div>
                  <div class="item-quantity">
                    <button class="quantity-btn" data-action="decrease" data-id="${item.id}">−</button>
                    <span class="quantity-value">${item.quantity}</span>
                    <button class="quantity-btn" data-action="increase" data-id="${item.id}">+</button>
                  </div>
                </div>
                <div class="item-total">
                  <div class="item-total-price">${this.formatPrice(item.price * item.quantity)}</div>
                  <button class="remove-btn" data-action="remove" data-id="${item.id}">Remove</button>
                </div>
              </div>
            `).join('')}
          </div>
          ${showSummary ? `
            <div class="cart-summary">
              <div class="summary-row">
                <span>Subtotal</span>
                <span>${this.formatPrice(total)}</span>
              </div>
              <div class="summary-row">
                <span>GST (18%)</span>
                <span>${this.formatPrice(total * 0.18)}</span>
              </div>
              <div class="summary-row">
                <span>Shipping</span>
                <span>${total > 499 ? 'Free' : this.formatPrice(49)}</span>
              </div>
              <div class="summary-row total">
                <span>Total</span>
                <span>${this.formatPrice(total > 499 ? total * 1.18 : (total * 1.18) + 49)}</span>
              </div>
              <div class="payment-options">
                <div class="payment-option selected" data-method="upi">
                  <div class="payment-icon">📱</div>
                  <div class="payment-label">UPI</div>
                </div>
                <div class="payment-option" data-method="card">
                  <div class="payment-icon">💳</div>
                  <div class="payment-label">Card</div>
                </div>
                <div class="payment-option" data-method="cod">
                  <div class="payment-icon">💵</div>
                  <div class="payment-label">COD</div>
                </div>
              </div>
              <button class="checkout-btn" ${this.items.length === 0 ? 'disabled' : ''}>
                Proceed to Checkout →
              </button>
            </div>
          ` : ''}
        `}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const decreaseBtns = this.shadowRoot.querySelectorAll('[data-action="decrease"]');
    decreaseBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const id = e.target.dataset.id;
        const item = this.items.find(item => item.id === id);
        if (item) {
          this.updateQuantity(id, item.quantity - 1);
        }
      });
    });

    const increaseBtns = this.shadowRoot.querySelectorAll('[data-action="increase"]');
    increaseBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const id = e.target.dataset.id;
        const item = this.items.find(item => item.id === id);
        if (item) {
          this.updateQuantity(id, item.quantity + 1);
        }
      });
    });

    const removeBtns = this.shadowRoot.querySelectorAll('[data-action="remove"]');
    removeBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        const id = e.target.dataset.id;
        this.removeItem(id);
      });
    });

    const checkoutBtn = this.shadowRoot.querySelector('.checkout-btn');
    if (checkoutBtn) {
      checkoutBtn.addEventListener('click', () => {
        this.dispatchEvent(new CustomEvent('checkout', {
          detail: { items: this.items, total: this.getTotal() },
          bubbles: true,
          composed: true,
        }));
      });
    }

    const paymentOptions = this.shadowRoot.querySelectorAll('.payment-option');
    paymentOptions.forEach(option => {
      option.addEventListener('click', (e) => {
        paymentOptions.forEach(o => o.classList.remove('selected'));
        e.currentTarget.classList.add('selected');
        this.dispatchEvent(new CustomEvent('payment-method-changed', {
          detail: { method: e.currentTarget.dataset.method },
          bubbles: true,
          composed: true,
        }));
      });
    });
  }
}

class ProductCard extends HTMLElement {
  constructor() {
    super();
    this.product = null;
    this.currency = 'INR';
    this.locale = 'en-IN';
  }

  static get observedAttributes() {
    return ['product', 'currency'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .product-card {
        background: #fff;
        border-radius: 12px;
        overflow: hidden;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
        transition: transform 0.3s, box-shadow 0.3s;
        cursor: pointer;
      }
      .product-card:hover {
        transform: translateY(-4px);
        box-shadow: 0 12px 24px rgba(0,0,0,0.12);
      }
      .product-image {
        width: 100%;
        height: 200px;
        object-fit: cover;
        background: #f8f9fa;
      }
      .product-content {
        padding: 16px;
      }
      .product-category {
        font-size: 0.75rem;
        color: #6c757d;
        text-transform: uppercase;
        letter-spacing: 0.5px;
        margin-bottom: 4px;
      }
      .product-name {
        font-weight: 600;
        color: #212529;
        margin-bottom: 8px;
        font-size: 1rem;
      }
      .product-description {
        font-size: 0.875rem;
        color: #6c757d;
        margin-bottom: 12px;
        line-height: 1.5;
      }
      .product-price {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 12px;
      }
      .current-price {
        font-size: 1.25rem;
        font-weight: 700;
        color: #212529;
      }
      .original-price {
        font-size: 0.875rem;
        color: #6c757d;
        text-decoration: line-through;
      }
      .discount-badge {
        background: #28a745;
        color: white;
        padding: 2px 8px;
        border-radius: 4px;
        font-size: 0.75rem;
        font-weight: 600;
      }
      .product-rating {
        display: flex;
        align-items: center;
        gap: 4px;
        margin-bottom: 12px;
      }
      .stars {
        color: #ffc107;
      }
      .rating-count {
        font-size: 0.875rem;
        color: #6c757d;
      }
      .add-to-cart-btn {
        width: 100%;
        padding: 12px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 0.875rem;
        font-weight: 600;
        cursor: pointer;
        transition: transform 0.2s, box-shadow 0.2s;
      }
      .add-to-cart-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
      }
      .out-of-stock {
        background: #6c757d;
        cursor: not-allowed;
      }
      .out-of-stock:hover {
        transform: none;
        box-shadow: none;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    if (this.hasAttribute('product')) {
      this.product = JSON.parse(this.getAttribute('product'));
    }
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'product') {
        this.product = JSON.parse(newValue);
      } else if (name === 'currency') {
        this.currency = newValue;
      }
      this.render();
    }
  }

  formatPrice(amount) {
    return new Intl.NumberFormat(this.locale, {
      style: 'currency',
      currency: this.currency,
    }).format(amount);
  }

  render() {
    if (!this.product) {
      this.shadowRoot.innerHTML = '<p>No product data</p>';
      return;
    }

    const discount = this.product.originalPrice 
      ? Math.round(((this.product.originalPrice - this.product.price) / this.product.originalPrice) * 100)
      : 0;

    this.shadowRoot.innerHTML = `
      <style>${ProductCard.styles}</style>
      <div class="product-card">
        <img class="product-image" src="${this.product.image}" alt="${this.product.name}">
        <div class="product-content">
          <div class="product-category">${this.product.category || 'General'}</div>
          <div class="product-name">${this.product.name}</div>
          <div class="product-description">${this.product.description || ''}</div>
          <div class="product-price">
            <span class="current-price">${this.formatPrice(this.product.price)}</span>
            ${this.product.originalPrice ? `
              <span class="original-price">${this.formatPrice(this.product.originalPrice)}</span>
              <span class="discount-badge">-${discount}%</span>
            ` : ''}
          </div>
          <div class="product-rating">
            <span class="stars">★ ★ ★ ★ ☆</span>
            <span class="rating-count">(${this.product.rating || 0})</span>
          </div>
          <button class="add-to-cart-btn ${this.product.stock === 0 ? 'out-of-stock' : ''}">
            ${this.product.stock === 0 ? 'Out of Stock' : 'Add to Cart 🛒'}
          </button>
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const addToCartBtn = this.shadowRoot.querySelector('.add-to-cart-btn');
    addToCartBtn?.addEventListener('click', (e) => {
      e.stopPropagation();
      if (this.product.stock > 0) {
        this.dispatchEvent(new CustomEvent('add-to-cart', {
          detail: { product: this.product },
          bubbles: true,
          composed: true,
        }));
      }
    });

    const card = this.shadowRoot.querySelector('.product-card');
    card?.addEventListener('click', () => {
      this.dispatchEvent(new CustomEvent('product-click', {
        detail: { product: this.product },
        bubbles: true,
        composed: true,
      }));
    });
  }
}

class AddressForm extends HTMLElement {
  constructor() {
    super();
    this.address = null;
  }

  static get styles() {
    return `
      :host {
        display: block;
        font-family: 'Poppins', -apple-system, BlinkMacSystemFont, sans-serif;
      }
      .address-form {
        background: #fff;
        border-radius: 12px;
        padding: 24px;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
      }
      .form-title {
        font-size: 1.25rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 20px;
      }
      .form-group {
        margin-bottom: 16px;
      }
      .form-label {
        display: block;
        font-size: 0.875rem;
        font-weight: 500;
        color: #495057;
        margin-bottom: 6px;
      }
      .form-input {
        width: 100%;
        padding: 12px;
        border: 1px solid #dee2e6;
        border-radius: 8px;
        font-size: 0.875rem;
        transition: border-color 0.2s, box-shadow 0.2s;
      }
      .form-input:focus {
        outline: none;
        border-color: #667eea;
        box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
      }
      .form-row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 16px;
      }
      .address-type {
        display: flex;
        gap: 12px;
        margin-bottom: 16px;
      }
      .type-option {
        flex: 1;
        padding: 12px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        transition: all 0.2s;
      }
      .type-option.selected {
        border-color: #667eea;
        background: rgba(102, 126, 234, 0.1);
      }
      .type-option:hover {
        border-color: #667eea;
      }
      .type-icon {
        font-size: 1.5rem;
        margin-bottom: 4px;
      }
      .type-label {
        font-size: 0.875rem;
        font-weight: 500;
      }
      .save-btn {
        width: 100%;
        padding: 14px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        margin-top: 16px;
      }
      .state-city-row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 16px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    if (this.hasAttribute('address')) {
      this.address = JSON.parse(this.getAttribute('address'));
    }
    this.render();
  }

  static get IndianStates() {
    return [
      'Andhra Pradesh', 'Arunachal Pradesh', 'Assam', 'Bihar', 'Chhattisgarh',
      'Goa', 'Gujarat', 'Haryana', 'Himachal Pradesh', 'Jharkhand',
      'Karnataka', 'Kerala', 'Madhya Pradesh', 'Maharashtra', 'Manipur',
      'Meghalaya', 'Mizoram', 'Nagaland', 'Odisha', 'Punjab',
      'Rajasthan', 'Sikkim', 'Tamil Nadu', 'Telangana', 'Tripura',
      'Uttar Pradesh', 'Uttarakhand', 'West Bengal', 'Delhi', 'Jammu and Kashmir'
    ];
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${AddressForm.styles}</style>
      <div class="address-form">
        <div class="form-title">📍 Delivery Address</div>
        
        <div class="address-type">
          <div class="type-option selected" data-type="home">
            <div class="type-icon">🏠</div>
            <div class="type-label">Home</div>
          </div>
          <div class="type-option" data-type="office">
            <div class="type-icon">🏢</div>
            <div class="type-label">Office</div>
          </div>
          <div class="type-option" data-type="other">
            <div class="type-icon">📍</div>
            <div class="type-label">Other</div>
          </div>
        </div>

        <div class="form-group">
          <label class="form-label">Full Name *</label>
          <input type="text" class="form-input" name="name" value="${this.address?.name || ''}" placeholder="Enter your full name" required>
        </div>

        <div class="form-group">
          <label class="form-label">Phone Number *</label>
          <input type="tel" class="form-input" name="phone" value="${this.address?.phone || ''}" placeholder="10-digit mobile number" required>
        </div>

        <div class="form-group">
          <label class="form-label">Flat/House No., Building, Area *</label>
          <input type="text" class="form-input" name="addressLine1" value="${this.address?.addressLine1 || ''}" placeholder="Flat No., Building, Area" required>
        </div>

        <div class="form-group">
          <label class="form-label">Landmark (Optional)</label>
          <input type="text" class="form-input" name="landmark" value="${this.address?.landmark || ''}" placeholder="Near landmark">
        </div>

        <div class="form-group">
          <label class="form-label">Pincode *</label>
          <input type="text" class="form-input" name="pincode" value="${this.address?.pincode || ''}" placeholder="6-digit PIN code" maxlength="6" required>
        </div>

        <div class="state-city-row">
          <div class="form-group">
            <label class="form-label">City *</label>
            <input type="text" class="form-input" name="city" value="${this.address?.city || ''}" placeholder="City" required>
          </div>
          <div class="form-group">
            <label class="form-label">State *</label>
            <select class="form-input" name="state" required>
              <option value="">Select State</option>
              ${AddressForm.IndianStates.map(state => `
                <option value="${state}" ${this.address?.state === state ? 'selected' : ''}>${state}</option>
              `).join('')}
            </select>
          </div>
        </div>

        <button class="save-btn">Save Address</button>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const typeOptions = this.shadowRoot.querySelectorAll('.type-option');
    typeOptions.forEach(option => {
      option.addEventListener('click', () => {
        typeOptions.forEach(o => o.classList.remove('selected'));
        option.classList.add('selected');
      });
    });

    const saveBtn = this.shadowRoot.querySelector('.save-btn');
    saveBtn?.addEventListener('click', () => {
      const form = this.shadowRoot.querySelector('.address-form');
      const formData = new FormData(form);
      const address = {
        type: this.shadowRoot.querySelector('.type-option.selected')?.dataset.type || 'home',
        name: formData.get('name'),
        phone: formData.get('phone'),
        addressLine1: formData.get('addressLine1'),
        landmark: formData.get('landmark'),
        pincode: formData.get('pincode'),
        city: formData.get('city'),
        state: formData.get('state'),
      };

      this.dispatchEvent(new CustomEvent('address-save', {
        detail: { address },
        bubbles: true,
        composed: true,
      }));
    });
  }
}

class CheckoutComponent extends HTMLElement {
  constructor() {
    super();
    this.cart = null;
    this.address = null;
    this.selectedPayment = 'upi';
    this.currency = 'INR';
    this.locale = 'en-IN';
  }

  static get styles() {
    return `
      :host {
        display: block;
        font-family: 'Poppins', -apple-system, BlinkMacSystemFont, sans-serif;
      }
      .checkout-container {
        background: #fff;
        border-radius: 12px;
        overflow: hidden;
        box-shadow: 0 2px 12px rgba(0,0,0,0.08);
      }
      .checkout-header {
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        padding: 20px 24px;
      }
      .checkout-header h2 {
        margin: 0;
        font-size: 1.5rem;
      }
      .checkout-steps {
        display: flex;
        padding: 0 24px;
        background: #f8f9fa;
        border-bottom: 1px solid #dee2e6;
      }
      .step {
        flex: 1;
        padding: 16px;
        text-align: center;
        position: relative;
        color: #6c757d;
      }
      .step.active {
        color: #667eea;
        font-weight: 600;
      }
      .step.completed {
        color: #28a745;
      }
      .step-number {
        width: 28px;
        height: 28px;
        border-radius: 50%;
        background: #dee2e6;
        color: #6c757d;
        display: inline-flex;
        align-items: center;
        justify-content: center;
        margin-right: 8px;
        font-size: 0.875rem;
        font-weight: 600;
      }
      .step.active .step-number {
        background: #667eea;
        color: white;
      }
      .step.completed .step-number {
        background: #28a745;
        color: white;
      }
      .checkout-content {
        padding: 24px;
      }
      .section-title {
        font-size: 1.125rem;
        font-weight: 600;
        color: #212529;
        margin-bottom: 16px;
        padding-bottom: 12px;
        border-bottom: 2px solid #f0f0f0;
      }
      .order-item {
        display: flex;
        padding: 12px 0;
        border-bottom: 1px solid #f0f0f0;
      }
      .item-img {
        width: 60px;
        height: 60px;
        border-radius: 8px;
        object-fit: cover;
        background: #e9ecef;
      }
      .item-info {
        flex: 1;
        margin-left: 12px;
      }
      .item-name {
        font-weight: 500;
        color: #212529;
      }
      .item-qty {
        font-size: 0.875rem;
        color: #6c757d;
      }
      .item-price {
        font-weight: 600;
      }
      .summary-row {
        display: flex;
        justify-content: space-between;
        padding: 8px 0;
        color: #495057;
      }
      .summary-row.total {
        font-size: 1.125rem;
        font-weight: 700;
        color: #212529;
        border-top: 2px solid #dee2e6;
        margin-top: 8px;
        padding-top: 12px;
      }
      .upi-options {
        grid-template-columns: repeat(3, 1fr);
        gap: 12px;
        margin-bottom: 16px;
      }
      .upi-option {
        padding: 16px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        text-align: center;
        cursor: pointer;
        transition: all 0.2s;
      }
      .upi-option.selected {
        border-color: #667eea;
        background: rgba(102, 126, 234, 0.1);
      }
      .upi-icon {
        font-size: 2rem;
        margin-bottom: 8px;
      }
      .place-order-btn {
        width: 100%;
        padding: 16px;
        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1.125rem;
        font-weight: 600;
        cursor: pointer;
        margin-top: 24px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  formatPrice(amount) {
    return new Intl.NumberFormat(this.locale, {
      style: 'currency',
      currency: this.currency,
    }).format(amount);
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${CheckoutComponent.styles}</style>
      <div class="checkout-container">
        <div class="checkout-header">
          <h2>Secure Checkout 🔒</h2>
        </div>
        
        <div class="checkout-steps">
          <div class="step completed">
            <span class="step-number">✓</span>
            Cart
          </div>
          <div class="step active">
            <span class="step-number">2</span>
            Address
          </div>
          <div class="step">
            <span class="step-number">3</span>
            Payment
          </div>
        </div>

        <div class="checkout-content">
          <div class="section-title">📦 Delivery Address</div>
          <div class="address-display">
            <p><strong>Rajesh Kumar</strong></p>
            <p>Flat No. 123, Sector 15</p>
            <p>Near City Park, Koramangala</p>
            <p>Bangalore - 560034, Karnataka</p>
            <p>📱 +91 98765 43210</p>
          </div>

          <div class="section-title">🛒 Order Items</div>
          <div class="order-items">
            <div class="order-item">
              <img class="item-img" src="" alt="Product">
              <div class="item-info">
                <div class="item-name">Sony WH-1000XM4 Headphones</div>
                <div class="item-qty">Qty: 1</div>
              </div>
              <div class="item-price">₹24,990</div>
            </div>
          </div>

          <div class="section-title">💳 Payment Options</div>
          <div class="payment-section">
            <div class="upi-options">
              <div class="upi-option selected" data-method="gpay">
                <div class="upi-icon">📱</div>
                <div>Google Pay</div>
              </div>
              <div class="upi-option" data-method="phonepe">
                <div class="upi-icon">📲</div>
                <div>PhonePe</div>
              </div>
              <div class="upi-option" data-method="paytm">
                <div class="upi-icon">💹</div>
                <div>Paytm</div>
              </div>
            </div>
            <div class="other-options">
              <label><input type="radio" name="payment" value="card"> Credit/Debit Card</label>
              <label><input type="radio" name="payment" value="upi"> UPI ID</label>
              <label><input type="radio" name="payment" value="cod" checked> Cash on Delivery</label>
            </div>
          </div>

          <div class="order-summary">
            <div class="summary-row">
              <span>Subtotal</span>
              <span>₹24,990</span>
            </div>
            <div class="summary-row">
              <span>GST (18%)</span>
              <span>₹4,498</span>
            </div>
            <div class="summary-row">
              <span>Shipping</span>
              <span>Free</span>
            </div>
            <div class="summary-row total">
              <span>Total</span>
              <span>₹29,488</span>
            </div>
          </div>

          <button class="place-order-btn">Place Order →</button>
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const upiOptions = this.shadowRoot.querySelectorAll('.upi-option');
    upiOptions.forEach(option => {
      option.addEventListener('click', () => {
        upiOptions.forEach(o => o.classList.remove('selected'));
        option.classList.add('selected');
        this.selectedPayment = option.dataset.method;
      });
    });
  }
}

export { ShoppingCart, ProductCard, AddressForm, CheckoutComponent };