/**
 * Payment Gateway Components - Razorpay, Paytm, UPI integration for Indian payment ecosystem
 * @module real-world/11_6_Payment-Gateway-Components
 * @version 1.0.0
 * @example <payment-button></payment-button>
 */

class UPIPayment extends HTMLElement {
  constructor() {
    super();
    this.amount = 0;
    this.merchantId = '';
    this.merchantName = '';
    this.callbackUrl = '';
  }

  static get observedAttributes() {
    return ['amount', 'merchant-id', 'merchant-name', 'callback-url'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .upi-apps {
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        gap: 12px;
      }
      .upi-app {
        display: flex;
        flex-direction: column;
        align-items: center;
        padding: 16px;
        border: 2px solid #e9ecef;
        border-radius: 12px;
        cursor: pointer;
        transition: all 0.2s;
        background: white;
      }
      .upi-app:hover {
        border-color: #667eea;
        transform: translateY(-2px);
      }
      .upi-app.selected {
        border-color: #667eea;
        background: #f0f4ff;
      }
      .app-icon {
        width: 48px;
        height: 48px;
        margin-bottom: 8px;
        border-radius: 12px;
      }
      .app-name {
        font-size: 0.75rem;
        font-weight: 500;
        color: #495057;
      }
      .upi-input-group {
        margin-top: 16px;
        padding: 16px;
        background: #f8f9fa;
        border-radius: 8px;
      }
      .upi-input-label {
        font-size: 0.75rem;
        color: #6c757d;
        margin-bottom: 8px;
      }
      .upi-input {
        width: 100%;
        padding: 12px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        font-size: 0.875rem;
      }
      .upi-input:focus {
        outline: none;
        border-color: #667eea;
      }
      @media (max-width: 480px) {
        .upi-apps {
          grid-template-columns: repeat(2, 1fr);
        }
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
      this.render();
    }
  }

  generateUPIUrl() {
    const params = new URLSearchParams({
      pa: this.merchantId,
      pn: this.merchantName,
      am: this.amount,
      cu: 'INR',
      tn: this.callbackUrl ? `txn_${Date.now()}` : '',
    }).toString();

    return `upi://pay?${params}`;
  }

  async initiatePayment(appName) {
    const upiUrl = this.generateUPIUrl();
    
    this.dispatchEvent(new CustomEvent('payment-initiate', {
      detail: { app: appName, amount: this.amount, upiUrl },
      bubbles: true,
      composed: true,
    }));

    if (appName === 'manual') {
      const upiId = this.shadowRoot.querySelector('.upi-input')?.value;
      if (!upiId) {
        this.showError('Please enter a UPI ID');
        return;
      }
      this.copyUPIUrl(upiId);
    } else {
      window.location.href = upiUrl;
    }
  }

  copyUPIUrl(upiId) {
    const url = new URL(this.generateUPIUrl());
    url.searchParams.set('pa', upiId);
    
    const textarea = document.createElement('textarea');
    textarea.value = url.toString();
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand('copy');
    document.body.removeChild(textarea);

    this.showSuccess('UPI payment link copied! Open any UPI app to pay');
  }

  showError(message) {
    this.dispatchEvent(new CustomEvent('error', {
      detail: { message },
      bubbles: true,
      composed: true,
    }));
  }

  showSuccess(message) {
    this.dispatchEvent(new CustomEvent('success', {
      detail: { message },
      bubbles: true,
      composed: true,
    }));
  }

  render() {
    const apps = [
      { name: 'Google Pay', icon: '📱', id: 'gpay' },
      { name: 'PhonePe', icon: '📲', id: 'phonepe' },
      { name: 'Paytm', icon: '💹', id: 'paytm' },
      { name: 'Amazon Pay', icon: '📦', id: 'amazon' },
      { name: 'BHIM', icon: '🏦', id: 'bhim' },
      { name: 'Others', icon: '📱', id: 'manual' },
    ];

    this.shadowRoot.innerHTML = `
      <style>${UPIPayment.styles}</style>
      <div>
        <div class="upi-apps">
          ${apps.map(app => `
            <div class="upi-app" data-app="${app.id}">
              <span class="app-icon" style="font-size: 2rem; display: flex; justify-content: center; align-items: center;">
                ${app.icon}
              </span>
              <span class="app-name">${app.name}</span>
            </div>
          `).join('')}
        </div>
        <div class="upi-input-group" style="display: none;">
          <div class="upi-input-label">Or enter any UPI ID:</div>
          <input type="text" class="upi-input" placeholder="username@upi">
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const appElements = this.shadowRoot.querySelectorAll('.upi-app');
    appElements.forEach(app => {
      app.addEventListener('click', () => {
        const appId = app.dataset.app;
        
        appElements.forEach(a => a.classList.remove('selected'));
        app.classList.add('selected');

        const manualGroup = this.shadowRoot.querySelector('.upi-input-group');
        if (appId === 'manual') {
          manualGroup.style.display = 'block';
        } else {
          manualGroup.style.display = 'none';
          this.initiatePayment(appId);
        }
      });
    });

    const payBtn = this.shadowRoot.querySelector('.upi-input');
    payBtn?.addEventListener('keypress', (e) => {
      if (e.key === 'Enter') {
        this.initiatePayment('manual');
      }
    });
  }
}

class RazorpayCheckout extends HTMLElement {
  constructor() {
    super();
    this.key = '';
    this.amount = 0;
    this.currency = 'INR';
    this.orderId = '';
    this.name = '';
    this.description = '';
    this.prefill = {};
    this.theme = {};
  }

  static get observedAttributes() {
    return ['key', 'amount', 'currency', 'order-id', 'name', 'description'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .checkout-btn {
        width: 100%;
        padding: 14px 24px;
        background: linear-gradient(135deg, #3399cc 0%, #2476a5 100%);
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        transition: all 0.2s;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
      }
      .checkout-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(51, 153, 204, 0.4);
      }
      .checkout-btn:disabled {
        background: #dee2e6;
        cursor: not-allowed;
        transform: none;
        box-shadow: none;
      }
      .options-modal {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 1000;
      }
      .options-content {
        background: white;
        border-radius: 16px;
        padding: 24px;
        max-width: 400px;
        width: 90%;
        max-height: 80vh;
        overflow-y: auto;
      }
      .option-item {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px;
        border: 2px solid #e9ecef;
        border-radius: 8px;
        margin-bottom: 8px;
        cursor: pointer;
        transition: all 0.2s;
      }
      .option-item:hover {
        border-color: #667eea;
      }
      .option-icon {
        font-size: 1.5rem;
      }
      .close-btn {
        position: absolute;
        top: 16px;
        right: 16px;
        background: none;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        color: #6c757d;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
    }
  }

  async open(options = {}) {
    const config = {
      key: this.key,
      amount: this.amount,
      currency: this.currency,
      name: this.name,
      description: this.description,
      order_id: this.orderId,
      prefill: { ...this.prefill, ...options.prefill },
      theme: { ...this.theme, ...options.theme },
      handler: (response) => {
        this.dispatchEvent(new CustomEvent('payment-success', {
          detail: response,
          bubbles: true,
          composed: true,
        }));
      },
    };

    if (window.Razorpay) {
      const rzp = new window.Razorpay(config);
      rzp.open();

      rzp.on('payment.failed', (response) => {
        this.dispatchEvent(new CustomEvent('payment-failed', {
          detail: response,
          bubbles: true,
          composed: true,
        }));
      });
    } else {
      this.showModal();
    }
  }

  showModal() {
    this.shadowRoot.innerHTML = `
      <style>${RazorpayCheckout.styles}</style>
      <div class="options-modal">
        <div class="options-content">
          <h3>💳 Select Payment Method</h3>
          <div class="option-item" data-method="card">
            <span class="option-icon">💳</span>
            <span>Credit / Debit Card</span>
          </div>
          <div class="option-item" data-method="netbanking">
            <span class="option-icon">🏦</span>
            <span>Net Banking</span>
          </div>
          <div class="option-item" data-method="wallet">
            <span class="option-icon">👛</span>
            <span>Wallet</span>
          </div>
          <div class="option-item" data-method="upi">
            <span class="option-icon">📱</span>
            <span>UPI</span>
          </div>
          <button class="close-btn">✕</button>
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  render() {
    const label = this.getAttribute('label') || 'Pay Now';

    this.shadowRoot.innerHTML = `
      <style>${RazorpayCheckout.styles}</style>
      <button class="checkout-btn">💳 ${label}</button>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const btn = this.shadowRoot.querySelector('.checkout-btn');
    btn?.addEventListener('click', () => this.open());

    const optionItems = this.shadowRoot.querySelectorAll('.option-item');
    optionItems.forEach(item => {
      item.addEventListener('click', () => {
        const method = item.dataset.method;
        this.dispatchEvent(new CustomEvent('method-selected', {
          detail: { method },
          bubbles: true,
          composed: true,
        }));
      });
    });

    const closeBtn = this.shadowRoot.querySelector('.close-btn');
    closeBtn?.addEventListener('click', () => this.render());
  }
}

class PaymentCardForm extends HTMLElement {
  constructor() {
    super();
    this.saveCard = false;
  }

  static get observedAttributes() {
    return ['save-card'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .card-form {
        padding: 20px;
        background: white;
        border-radius: 12px;
        border: 1px solid #e9ecef;
      }
      .form-group {
        margin-bottom: 16px;
      }
      .form-label {
        display: block;
        font-size: 0.75rem;
        color: #6c757d;
        margin-bottom: 6px;
      }
      .form-input {
        width: 100%;
        padding: 12px;
        border: 2px solid #e9ecef;
        border-radius: 8px;
        font-size: 0.875rem;
      }
      .form-input:focus {
        outline: none;
        border-color: #667eea;
      }
      .form-row {
        display: grid;
        grid-template-columns: 1fr 1fr;
        gap: 12px;
      }
      .card-icons {
        display: flex;
        gap: 8px;
        margin-bottom: 12px;
      }
      .card-icon {
        font-size: 1.5rem;
        opacity: 0.5;
      }
      .card-icon.active {
        opacity: 1;
      }
      .save-card {
        display: flex;
        align-items: center;
        gap: 8px;
      }
      .save-card input {
        width: 16px;
        height: 16px;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue !== null;
    }
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${PaymentCardForm.styles}</style>
      <div class="card-form">
        <div class="card-icons">
          <span class="card-icon">💳</span>
          <span class="card-icon">💳</span>
          <span class="card-icon">💳</span>
        </div>
        
        <div class="form-group">
          <label class="form-label">Card Number</label>
          <input type="text" class="form-input" placeholder="1234 5678 9012 3456" maxlength="19">
        </div>
        
        <div class="form-group">
          <label class="form-label">Cardholder Name</label>
          <input type="text" class="form-input" placeholder="Name on card">
        </div>
        
        <div class="form-row">
          <div class="form-group">
            <label class="form-label">Expiry Date</label>
            <input type="text" class="form-input" placeholder="MM/YY" maxlength="5">
          </div>
          <div class="form-group">
            <label class="form-label">CVV</label>
            <input type="text" class="form-input" placeholder="123" maxlength="4">
          </div>
        </div>
        
        <label class="save-card">
          <input type="checkbox" ${this.saveCard ? 'checked' : ''}>
          <span>Save card for future payments</span>
        </label>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const inputs = this.shadowRoot.querySelectorAll('.form-input');
    inputs.forEach(input => {
      input.addEventListener('input', () => {
        this.formatCardNumber();
        this.detectCardType();
      });
    });
  }

  formatCardNumber() {
    const input = this.shadowRoot.querySelector('input[placeholder*="3456"]');
    if (input) {
      let value = input.value.replace(/\D/g, '');
      value = value.replace(/(\d{4})/g, '$1 ').trim();
      input.value = value;
    }
  }

  detectCardType() {
    const input = this.shadowRoot.querySelector('input[placeholder*="3456"]');
    const icons = this.shadowRoot.querySelectorAll('.card-icon');
    const value = input?.value.replace(/\s/g, '');

    icons.forEach(icon => icon.classList.remove('active'));

    if (value?.startsWith('4')) {
      icons[0].classList.add('active');
    } else if (value?.startsWith('5')) {
      icons[1].classList.add('active');
    } else if (value?.startsWith('3')) {
      icons[2].classList.add('active');
    }
  }

  validate() {
    const inputs = this.shadowRoot.querySelectorAll('.form-input');
    let isValid = true;

    inputs.forEach(input => {
      if (!input.value.trim()) {
        input.style.borderColor = '#dc3545';
        isValid = false;
      } else {
        input.style.borderColor = '#28a745';
      }
    });

    return isValid;
  }

  getCardData() {
    const inputs = this.shadowRoot.querySelectorAll('.form-input');
    return {
      number: inputs[0]?.value,
      name: inputs[1]?.value,
      expiry: inputs[2]?.value,
      cvv: inputs[3]?.value,
      saveCard: this.saveCard,
    };
  }
}

class PaytmCheckout extends HTMLElement {
  constructor() {
    super();
    this.merchantId = '';
    this.amount = 0;
    this.orderId = '';
  }

  static get observedAttributes() {
    return ['merchant-id', 'amount', 'order-id'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .paytm-btn {
        width: 100%;
        padding: 14px 24px;
        background: #00b9f5;
        color: white;
        border: none;
        border-radius: 8px;
        font-size: 1rem;
        font-weight: 600;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
      }
      .paytm-btn:hover {
        background: #00a5db;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
    }
  }

  async initiate() {
    const formData = new FormData();
    formData.append('MID', this.merchantId);
    formData.append('ORDER_ID', this.orderId);
    formData.append('CUST_ID', `CUST_${Date.now()}`);
    formData.append('INDUSTRY_TYPE_ID', 'Retail');
    formData.append('CHANNEL_ID', 'WEB');
    formData.append('TXN_AMOUNT', this.amount);
    formData.append('EMAIL', '');
    formData.append('MOBILE_NO', '');

    this.dispatchEvent(new CustomEvent('payment-initiate', {
      detail: { formData, amount: this.amount },
      bubbles: true,
      composed: true,
    }));
  }

  render() {
    const label = this.getAttribute('label') || 'Pay with Paytm';

    this.shadowRoot.innerHTML = `
      <style>${PaytmCheckout.styles}</style>
      <button class="paytm-btn">💹 ${label}</button>
    `;

    const btn = this.shadowRoot.querySelector('.paytm-btn');
    btn?.addEventListener('click', () => this.initiate());
  }
}

class PaymentSummary extends HTMLElement {
  constructor() {
    super();
    this.amount = 0;
    this.currency = 'INR';
    this.gst = 0;
    this.shipping = 0;
  }

  static get observedAttributes() {
    return ['amount', 'currency', 'gst', 'shipping'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .summary {
        padding: 16px;
        background: #f8f9fa;
        border-radius: 12px;
      }
      .summary-row {
        display: flex;
        justify-content: space-between;
        padding: 8px 0;
        font-size: 0.875rem;
      }
      .summary-row.total {
        border-top: 2px solid #dee2e6;
        margin-top: 8px;
        padding-top: 12px;
        font-weight: 700;
        font-size: 1rem;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = parseFloat(newValue);
      this.render();
    }
  }

  formatAmount(amount) {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: this.currency,
    }).format(amount);
  }

  render() {
    const subtotal = this.amount;
    const gstAmount = subtotal * (this.gst / 100);
    const total = subtotal + gstAmount + (this.shipping || (subtotal >= 499 ? 0 : 49));

    this.shadowRoot.innerHTML = `
      <style>${PaymentSummary.styles}</style>
      <div class="summary">
        <div class="summary-row">
          <span>Subtotal</span>
          <span>${this.formatAmount(subtotal)}</span>
        </div>
        <div class="summary-row">
          <span>GST (${this.gst}%)</span>
          <span>${this.formatAmount(gstAmount)}</span>
        </div>
        <div class="summary-row">
          <span>Shipping</span>
          <span>${this.shipping || (subtotal >= 499 ? 'Free' : this.formatAmount(49))}</span>
        </div>
        <div class="summary-row total">
          <span>Total</span>
          <span>${this.formatAmount(total)}</span>
        </div>
      </div>
    `;
  }
}

class SavedPaymentMethods extends HTMLElement {
  constructor() {
    super();
    this.methods = [];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .methods {
        display: flex;
        flex-direction: column;
        gap: 8px;
      }
      .method {
        display: flex;
        align-items: center;
        gap: 12px;
        padding: 12px;
        border: 2px solid #e9ecef;
        border-radius: 8px;
        cursor: pointer;
        transition: all 0.2s;
      }
      .method:hover {
        border-color: #667eea;
      }
      .method.selected {
        border-color: #667eea;
        background: #f0f4ff;
      }
      .method-icon {
        font-size: 1.5rem;
      }
      .method-info {
        flex: 1;
      }
      .method-name {
        font-weight: 500;
        font-size: 0.875rem;
      }
      .method-detail {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .delete-btn {
        background: none;
        border: none;
        color: #dc3545;
        cursor: pointer;
        font-size: 0.875rem;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  addMethod(method) {
    this.methods.push(method);
    this.render();
  }

  removeMethod(id) {
    this.methods = this.methods.filter(m => m.id !== id);
    this.render();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${SavedPaymentMethods.styles}</style>
      <div class="methods">
        ${this.methods.length === 0 ? `
          <p style="color: #6c757d; text-align: center;">No saved payment methods</p>
        ` : this.methods.map((method, index) => `
          <div class="method" data-id="${method.id}">
            <span class="method-icon">💳</span>
            <div class="method-info">
              <div class="method-name">${method.name}</div>
              <div class="method-detail">**** **** **** ${method.last4}</div>
            </div>
            <button class="delete-btn" data-action="delete">🗑️</button>
          </div>
        `).join('')}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const methods = this.shadowRoot.querySelectorAll('.method');
    methods.forEach(method => {
      method.addEventListener('click', (e) => {
        if (e.target.dataset.action !== 'delete') {
          methods.forEach(m => m.classList.remove('selected'));
          method.classList.add('selected');
          
          const methodId = method.dataset.id;
          this.dispatchEvent(new CustomEvent('method-selected', {
            detail: { methodId },
            bubbles: true,
            composed: true,
          }));
        }
      });
    });

    const deleteBtns = this.shadowRoot.querySelectorAll('[data-action="delete"]');
    deleteBtns.forEach(btn => {
      btn.addEventListener('click', (e) => {
        e.stopPropagation();
        const method = btn.closest('.method');
        this.removeMethod(method.dataset.id);
      });
    });
  }
}

export { UPIPayment, RazorpayCheckout, PaymentCardForm, PaytmCheckout, PaymentSummary, SavedPaymentMethods };