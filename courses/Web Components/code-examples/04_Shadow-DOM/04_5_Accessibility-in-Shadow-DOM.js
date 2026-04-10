/**
 * Accessible Card Component - ARIA implementation in Shadow DOM
 * @module shadow-dom/04_5_Accessibility-in-Shadow-DOM
 * @version 1.0.0
 * @example <accessible-card></accessible-card>
 */

class AccessibleCard extends HTMLElement {
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._focusVisible = false;
    this._role = 'article';
  }

  static get observedAttributes() {
    return ['role', 'aria-label', 'aria-describedby', 'tabindex', 'disabled'];
  }

  connectedCallback() {
    this._render();
    this._attachEventListeners();
    this._applyRole();
  }

  disconnectedCallback() {
    this._removeEventListeners();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this._handleAttributeChange(name, newValue);
    }
  }

  _handleAttributeChange(name, value) {
    switch (name) {
      case 'role':
        this._role = value || 'article';
        this._applyRole();
        break;
      case 'aria-label':
        this._updateAriaLabel(value);
        break;
      case 'aria-describedby':
        this._updateAriaDescribedby(value);
        break;
      case 'tabindex':
        this._updateTabIndex(value);
        break;
      case 'disabled':
        this._updateDisabled(value !== null);
        break;
    }
  }

  _render() {
    const style = this._getStyles();
    const template = this._getTemplate();
    this.shadowRoot.innerHTML = `${style}${template}`;
    this._cacheElements();
  }

  _getStyles() {
    return `
      <style>
        :host {
          display: block;
          --card-bg: #ffffff;
          --card-border: 1px solid #e0e0e0;
          --card-radius: 8px;
          --card-shadow: 0 2px 8px rgba(0,0,0,0.1);
          --card-padding: 16px;
          --text-color: #333333;
          --title-color: #1a1a1a;
          --focus-ring: 2px solid #0066cc;
          --focus-offset: 2px;
        }

        :host([hidden]) {
          display: none;
        }

        :host([disabled]) {
          opacity: 0.6;
          pointer-events: none;
        }

        .card {
          background: var(--card-bg);
          border: var(--card-border);
          border-radius: var(--card-radius);
          box-shadow: var(--card-shadow);
          padding: var(--card-padding);
          transition: box-shadow 0.2s ease, transform 0.2s ease;
        }

        .card:focus {
          outline: none;
        }

        .card.focus-visible {
          box-shadow: 0 4px 16px rgba(0,0,0,0.15);
          transform: translateY(-2px);
        }

        .card-header {
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 12px;
        }

        .card-title {
          font-size: 1.25rem;
          font-weight: 600;
          color: var(--title-color);
          margin: 0;
        }

        .card-icon {
          width: 24px;
          height: 24px;
          fill: currentColor;
        }

        .card-content {
          color: var(--text-color);
          line-height: 1.6;
        }

        .card-actions {
          display: flex;
          gap: 8px;
          margin-top: 16px;
        }

        .action-button {
          padding: 8px 16px;
          border: 1px solid #ccc;
          border-radius: 4px;
          background: #f5f5f5;
          color: var(--text-color);
          cursor: pointer;
          font-size: 0.875rem;
          transition: background 0.15s ease;
        }

        .action-button:hover {
          background: #e8e8e8;
        }

        .action-button:focus {
          outline: var(--focus-ring);
          outline-offset: var(--focus-offset);
        }

        .action-button.primary {
          background: #0066cc;
          color: white;
          border-color: #0066cc;
        }

        .action-button.primary:hover {
          background: #0055aa;
        }

        .sr-only {
          position: absolute;
          width: 1px;
          height: 1px;
          padding: 0;
          margin: -1px;
          overflow: hidden;
          clip: rect(0, 0, 0, 0);
          white-space: nowrap;
          border: 0;
        }

        .status-indicator {
          display: inline-flex;
          align-items: center;
          gap: 6px;
          font-size: 0.75rem;
          color: #666;
          margin-top: 8px;
        }

        .status-dot {
          width: 8px;
          height: 8px;
          border-radius: 50%;
          background: #4caf50;
        }
      </style>
    `;
  }

  _getTemplate() {
    return `
      <div class="card" tabindex="0" role="article">
        <div class="card-header">
          <h2 class="card-title" id="card-title">Card Title</h2>
          <svg class="card-icon" viewBox="0 0 24 24" aria-hidden="true">
            <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
          </svg>
        </div>
        <div class="card-content">
          <p id="card-description">Default card content goes here.</p>
          <div class="status-indicator">
            <span class="status-dot" aria-hidden="true"></span>
            <span class="status-text">Active</span>
          </div>
        </div>
        <div class="card-actions" role="group" aria-label="Card actions">
          <button class="action-button primary" type="button" aria-label="Confirm action">Confirm</button>
          <button class="action-button" type="button" aria-label="Cancel action">Cancel</button>
        </div>
        <div id="live-region" class="sr-only" aria-live="polite" aria-atomic="true"></div>
      </div>
    `;
  }

  _cacheElements() {
    this._card = this.shadowRoot.querySelector('.card');
    this._title = this.shadowRoot.getElementById('card-title');
    this._description = this.shadowRoot.getElementById('card-description');
    this._liveRegion = this.shadowRoot.getElementById('live-region');
    this._actionButtons = this.shadowRoot.querySelectorAll('.action-button');
  }

  _attachEventListeners() {
    this._card.addEventListener('keydown', this._handleKeyDown.bind(this));
    this._card.addEventListener('focus', this._handleFocus.bind(this));
    this._card.addEventListener('blur', this._handleBlur.bind(this));

    this._actionButtons.forEach(button => {
      button.addEventListener('click', this._handleButtonClick.bind(this));
    });
  }

  _removeEventListeners() {
    if (this._card) {
      this._card.removeEventListener('keydown', this._handleKeyDown.bind(this));
      this._card.removeEventListener('focus', this._handleFocus.bind(this));
      this._card.removeEventListener('blur', this._handleBlur.bind(this));
    }

    if (this._actionButtons) {
      this._actionButtons.forEach(button => {
        button.removeEventListener('click', this._handleButtonClick.bind(this));
      });
    }
  }

  _handleKeyDown(event) {
    if (event.key === 'Enter' || event.key === ' ') {
      event.preventDefault();
      this._activateCard();
    }

    if (event.key === 'Escape') {
      this._card.blur();
    }

    if (event.key === 'ArrowRight' || event.key === 'ArrowLeft') {
      this._navigateActions(event.key === 'ArrowRight' ? 1 : -1);
    }
  }

  _handleFocus() {
    this._focusVisible = true;
    this._card.classList.add('focus-visible');
  }

  _handleBlur() {
    this._focusVisible = false;
    this._card.classList.remove('focus-visible');
  }

  _handleButtonClick(event) {
    const button = event.target;
    const action = button.textContent.trim();
    this._announceToScreenReader(`${action} clicked`);

    this.dispatchEvent(new CustomEvent('card-action', {
      detail: { action, button: button.textContent },
      bubbles: true,
      composed: true
    }));
  }

  _activateCard() {
    this._announceToScreenReader('Card activated');
    this.dispatchEvent(new CustomEvent('card-activate', {
      bubbles: true,
      composed: true
    }));
  }

  _navigateActions(direction) {
    const focusableButtons = Array.from(this._actionButtons);
    const currentIndex = focusableButtons.findIndex(btn => btn === document.activeElement);
    let newIndex = currentIndex + direction;

    if (newIndex < 0) newIndex = focusableButtons.length - 1;
    if (newIndex >= focusableButtons.length) newIndex = 0;

    focusableButtons[newIndex].focus();
  }

  _announceToScreenReader(message) {
    if (this._liveRegion) {
      this._liveRegion.textContent = message;
      setTimeout(() => {
        this._liveRegion.textContent = '';
      }, 1000);
    }
  }

  _applyRole() {
    if (this._card) {
      this._card.setAttribute('role', this._role);
    }
  }

  _updateAriaLabel(value) {
    if (this._card) {
      if (value) {
        this._card.setAttribute('aria-label', value);
      } else {
        this._card.removeAttribute('aria-label');
      }
    }
  }

  _updateAriaDescribedby(value) {
    if (this._card) {
      if (value) {
        this._card.setAttribute('aria-describedby', value);
      } else {
        this._card.removeAttribute('aria-describedby');
      }
    }
  }

  _updateTabIndex(value) {
    if (this._card) {
      this._card.setAttribute('tabindex', value || 0);
    }
  }

  _updateDisabled(disabled) {
    if (disabled) {
      this._card.setAttribute('aria-disabled', 'true');
      this._card.setAttribute('tabindex', '-1');
    } else {
      this._card.removeAttribute('aria-disabled');
      this._card.removeAttribute('tabindex');
    }
  }

  get title() {
    return this._title ? this._title.textContent : '';
  }

  set title(value) {
    if (this._title) {
      this._title.textContent = value;
    }
  }

  get content() {
    return this._description ? this._description.textContent : '';
  }

  set content(value) {
    if (this._description) {
      this._description.textContent = value;
      this._announceToScreenReader('Content updated');
    }
  }

  get status() {
    return this._statusText ? this._statusText.textContent : '';
  }

  set status(value) {
    const statusText = this.shadowRoot.querySelector('.status-text');
    if (statusText) {
      statusText.textContent = value;
    }
  }
}

customElements.define('accessible-card', AccessibleCard);

export { AccessibleCard };