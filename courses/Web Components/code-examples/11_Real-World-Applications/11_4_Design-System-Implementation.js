/**
 * Design System Implementation - Comprehensive design system with typography, colors, spacing, and components
 * @module real-world/11_4_Design-System-Implementation
 * @version 1.0.0
 * @example <design-button></design-button>
 */

class DesignTokens {
  static get colors() {
    return {
      primary: {
        50: '#eef2ff',
        100: '#e0e7ff',
        200: '#c7d2fe',
        300: '#a5b4fc',
        400: '#818cf8',
        500: '#667eea',
        600: '#5a67d8',
        700: '#4c1d95',
        800: '#3730a3',
        900: '#312e81',
      },
      secondary: {
        50: '#faf5ff',
        100: '#f3e8ff',
        200: '#e9d5ff',
        300: '#d8b4fe',
        400: '#c084fc',
        500: '#a855f7',
        600: '#9333ea',
        700: '#7e22ce',
        800: '#6b21a8',
        900: '#581c87',
      },
      success: {
        50: '#f0fdf4',
        100: '#dcfce7',
        200: '#bbf7d0',
        300: '#86efac',
        400: '#4ade80',
        500: '#22c55e',
        600: '#16a34a',
        700: '#15803d',
        800: '#166534',
        900: '#14532d',
      },
      warning: {
        50: '#fefce8',
        100: '#fef9c3',
        200: '#fef08a',
        300: '#fde047',
        400: '#facc15',
        500: '#eab308',
        600: '#ca8a04',
        700: '#a16207',
        800: '#854d0e',
        900: '#713f12',
      },
      error: {
        50: '#fef2f2',
        100: '#fee2e2',
        200: '#fecaca',
        300: '#fca5a5',
        400: '#f87171',
        500: '#ef4444',
        600: '#dc2626',
        700: '#b91c1c',
        800: '#991b1b',
        900: '#7f1d1d',
      },
      neutral: {
        50: '#fafafa',
        100: '#f4f4f5',
        200: '#e4e4e7',
        300: '#d4d4d8',
        400: '#a1a1aa',
        500: '#71717a',
        600: '#52525b',
        700: '#3f3f46',
        800: '#27272a',
        900: '#18181b',
      },
    };
  }

  static get spacing() {
    return {
      0: '0',
      1: '0.25rem',
      2: '0.5rem',
      3: '0.75rem',
      4: '1rem',
      5: '1.25rem',
      6: '1.5rem',
      8: '2rem',
      10: '2.5rem',
      12: '3rem',
      16: '4rem',
      20: '5rem',
    };
  }

  static get typography() {
    return {
      fontFamily: {
        sans: "'Poppins', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif",
        mono: "'Fira Code', 'JetBrains Mono', monospace",
        display: "'Playfair Display', serif",
      },
      fontSize: {
        xs: '0.75rem',
        sm: '0.875rem',
        base: '1rem',
        lg: '1.125rem',
        xl: '1.25rem',
        '2xl': '1.5rem',
        '3xl': '1.875rem',
        '4xl': '2.25rem',
        '5xl': '3rem',
      },
      fontWeight: {
        normal: '400',
        medium: '500',
        semibold: '600',
        bold: '700',
      },
      lineHeight: {
        tight: 1.25,
        normal: 1.5,
        relaxed: 1.75,
      },
    };
  }

  static get borders() {
    return {
      none: 'none',
      sm: '1px',
      DEFAULT: '2px',
      lg: '4px',
    };
  }

  static get radii() {
    return {
      none: '0',
      sm: '0.25rem',
      DEFAULT: '0.5rem',
      md: '0.75rem',
      lg: '1rem',
      xl: '1.5rem',
      '2xl': '2rem',
      full: '9999px',
    };
  }

  static get shadows() {
    return {
      sm: '0 1px 2px 0 rgba(0, 0, 0, 0.05)',
      DEFAULT: '0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)',
      md: '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
      lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
      xl: '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
      '2xl': '0 25px 50px -12px rgba(0, 0, 0, 0.25)',
    };
  }

  static get transitions() {
    return {
      fast: '150ms',
      normal: '200ms',
      slow: '300ms',
      slower: '500ms',
    };
  }
}

class DesignButton extends HTMLElement {
  constructor() {
    super();
    this.variant = 'primary';
    this.size = 'md';
    this.disabled = false;
    this.loading = false;
  }

  static get observedAttributes() {
    return ['variant', 'size', 'disabled', 'loading', 'icon'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: inline-block;
      }
      .btn {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        font-family: ${tokens.typography.fontFamily.sans};
        font-weight: ${tokens.typography.fontWeight.semibold};
        border: none;
        border-radius: ${tokens.radii.lg};
        cursor: pointer;
        transition: all ${tokens.transitions.normal};
        text-decoration: none;
        white-space: nowrap;
      }
      .btn:focus {
        outline: none;
        box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.3);
      }
      .btn:disabled {
        opacity: 0.6;
        cursor: not-allowed;
      }
      
      /* Sizes */
      .btn.sm {
        padding: ${tokens.spacing[2]} ${tokens.spacing[3]};
        font-size: ${tokens.typography.fontSize.sm};
      }
      .btn.md {
        padding: ${tokens.spacing[3]} ${tokens.spacing[5]};
        font-size: ${tokens.typography.fontSize.base};
      }
      .btn.lg {
        padding: ${tokens.spacing[4]} ${tokens.spacing[6]};
        font-size: ${tokens.typography.fontSize.lg};
      }
      
      /* Variants */
      .btn.primary {
        background: linear-gradient(135deg, ${tokens.colors.primary[500]} 0%, ${tokens.colors.primary[600]} 100%);
        color: white;
      }
      .btn.primary:hover:not(:disabled) {
        transform: translateY(-2px);
        box-shadow: ${tokens.shadows.lg};
      }
      .btn.primary:active:not(:disabled) {
        transform: translateY(0);
      }
      
      .btn.secondary {
        background: linear-gradient(135deg, ${tokens.colors.secondary[500]} 0%, ${tokens.colors.secondary[600]} 100%);
        color: white;
      }
      .btn.secondary:hover:not(:disabled) {
        transform: translateY(-2px);
        box-shadow: ${tokens.shadows.lg};
      }
      
      .btn.outline {
        background: transparent;
        border: 2px solid ${tokens.colors.primary[500]};
        color: ${tokens.colors.primary[500]};
      }
      .btn.outline:hover:not(:disabled) {
        background: ${tokens.colors.primary[50]};
      }
      
      .btn.ghost {
        background: transparent;
        color: ${tokens.colors.primary[500]};
      }
      .btn.ghost:hover:not(:disabled) {
        background: ${tokens.colors.primary[50]};
      }
      
      .btn.success {
        background: linear-gradient(135deg, ${tokens.colors.success[500]} 0%, ${tokens.colors.success[600]} 100%);
        color: white;
      }
      
      .btn.warning {
        background: linear-gradient(135deg, ${tokens.colors.warning[500]} 0%, ${tokens.colors.warning[600]} 100%);
        color: ${tokens.colors.neutral[900]};
      }
      
      .btn.danger {
        background: linear-gradient(135deg, ${tokens.colors.error[500]} 0%, ${tokens.colors.error[600]} 100%);
        color: white;
      }
      
      .btn.full {
        width: 100%;
      }
      
      .spinner {
        width: 16px;
        height: 16px;
        border: 2px solid currentColor;
        border-top-color: transparent;
        border-radius: 50%;
        animation: spin 0.8s linear infinite;
      }
      @keyframes spin {
        to { transform: rotate(360deg); }
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

  render() {
    const icon = this.getAttribute('icon') || '';
    const fullWidth = this.hasAttribute('full');

    this.shadowRoot.innerHTML = `
      <style>${DesignButton.styles}</style>
      <button class="btn ${this.variant} ${this.size} ${fullWidth ? 'full' : ''}"
        ${this.disabled || this.loading ? 'disabled' : ''}>
        ${this.loading ? '<span class="spinner"></span>' : icon ? `<span>${icon}</span>` : ''}
        <slot></slot>
      </button>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const btn = this.shadowRoot.querySelector('.btn');
    btn?.addEventListener('click', () => {
      if (!this.disabled && !this.loading) {
        this.dispatchEvent(new CustomEvent('click', {
          bubbles: true,
          composed: true,
        }));
      }
    });
  }
}

class DesignCard extends HTMLElement {
  constructor() {
    super();
    this.variant = 'default';
    this.hoverable = false;
  }

  static get observedAttributes() {
    return ['variant', 'hoverable', 'elevation'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: block;
      }
      .card {
        background: white;
        border-radius: ${tokens.radii.xl};
        box-shadow: ${tokens.shadows.DEFAULT};
        overflow: hidden;
        transition: all ${tokens.transitions.normal};
      }
      .card.hoverable:hover {
        transform: translateY(-4px);
        box-shadow: ${tokens.shadows.xl};
      }
      .card.outlined {
        border: 1px solid ${tokens.neutral[200]};
        box-shadow: none;
      }
      .card.elevated {
        box-shadow: ${tokens.shadows.xl};
      }
      .card-header {
        padding: ${tokens.spacing[5]};
        border-bottom: 1px solid ${tokens.neutral[100]};
      }
      .card-body {
        padding: ${tokens.spacing[5]};
      }
      .card-footer {
        padding: ${tokens.spacing[4]} ${tokens.spacing[5]};
        border-top: 1px solid ${tokens.neutral[100]};
        background: ${tokens.neutral[50]};
      }
      .card-title {
        font-size: ${tokens.typography.fontSize.xl};
        font-weight: ${tokens.typography.fontWeight.semibold};
        color: ${tokens.neutral[900]};
        margin: 0;
      }
      .card-subtitle {
        font-size: ${tokens.typography.fontSize.sm};
        color: ${tokens.neutral[500]};
        margin: 4px 0 0;
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

  render() {
    this.shadowRoot.innerHTML = `
      <style>${DesignCard.styles}</style>
      <div class="card ${this.variant} ${this.hoverable ? 'hoverable' : ''}">
        <div class="card-header">
          <slot name="header">
            <h3 class="card-title"><slot name="title"></slot></h3>
            <p class="card-subtitle"><slot name="subtitle"></slot></p>
          </slot>
        </div>
        <div class="card-body">
          <slot></slot>
        </div>
        <div class="card-footer" style="display: none;">
          <slot name="footer"></slot>
        </div>
      </div>
    `;

    this.checkSlot('footer');
  }

  checkSlot(slotName) {
    const slot = this.querySelector(`[slot="${slotName}"]`);
    const footer = this.shadowRoot.querySelector('.card-footer');
    if (slot) {
      footer.style.display = 'block';
    }
  }
}

class DesignBadge extends HTMLElement {
  constructor() {
    super();
    this.variant = 'primary';
    this.size = 'md';
    this.dot = false;
  }

  static get observedAttributes() {
    return ['variant', 'size', 'dot'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: inline-block;
      }
      .badge {
        display: inline-flex;
        align-items: center;
        gap: 6px;
        font-family: ${tokens.typography.fontFamily.sans};
        font-weight: ${tokens.typography.fontWeight.medium};
        border-radius: ${tokens.radii.full};
      }
      .badge.sm {
        padding: 2px 8px;
        font-size: ${tokens.typography.fontSize.xs};
      }
      .badge.md {
        padding: 4px 12px;
        font-size: ${tokens.typography.fontSize.sm};
      }
      .badge.lg {
        padding: 6px 16px;
        font-size: ${tokens.typography.fontSize.base};
      }
      .badge.primary {
        background: ${tokens.colors.primary[100]};
        color: ${tokens.colors.primary[700]};
      }
      .badge.secondary {
        background: ${tokens.colors.secondary[100]};
        color: ${tokens.colors.secondary[700]};
      }
      .badge.success {
        background: ${tokens.colors.success[100]};
        color: ${tokens.colors.success[700]};
      }
      .badge.warning {
        background: ${tokens.colors.warning[100]};
        color: ${tokens.colors.warning[700]};
      }
      .badge.error {
        background: ${tokens.colors.error[100]};
        color: ${tokens.colors.error[700]};
      }
      .badge.neutral {
        background: ${tokens.colors.neutral[100]};
        color: ${tokens.colors.neutral[700]};
      }
      .dot {
        width: 6px;
        height: 6px;
        border-radius: 50%;
        background: currentColor;
      }
      .dot.pulse {
        animation: pulse 2s infinite;
      }
      @keyframes pulse {
        0%, 100% { opacity: 1; }
        50% { opacity: 0.5; }
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

  render() {
    this.shadowRoot.innerHTML = `
      <style>${DesignBadge.styles}</style>
      <span class="badge ${this.variant} ${this.size}">
        ${this.dot ? `<span class="dot ${this.hasAttribute('pulse') ? 'pulse' : ''}"></span>` : ''}
        <slot></slot>
      </span>
    `;
  }
}

class DesignInput extends HTMLElement {
  constructor() {
    super();
    this.type = 'text';
    this.size = 'md';
    this.label = '';
    this.placeholder = '';
    this.value = '';
    this.disabled = false;
    this.error = '';
    this.hint = '';
  }

  static get observedAttributes() {
    return ['type', 'size', 'label', 'placeholder', 'disabled', 'error', 'hint'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: block;
      }
      .input-wrapper {
        display: flex;
        flex-direction: column;
        gap: 6px;
      }
      .input-label {
        font-family: ${tokens.typography.fontFamily.sans};
        font-size: ${tokens.typography.fontSize.sm};
        font-weight: ${tokens.typography.fontWeight.medium};
        color: ${tokens.neutral[700]};
      }
      .input-wrapper.has-error .input-label {
        color: ${tokens.error[600]};
      }
      .input-field {
        font-family: ${tokens.typography.fontFamily.sans};
        border: 2px solid ${tokens.neutral[200]};
        border-radius: ${tokens.radii.lg};
        background: white;
        transition: all ${tokens.transitions.fast};
      }
      .input-field:focus {
        outline: none;
        border-color: ${tokens.primary[500]};
        box-shadow: 0 0 0 3px ${tokens.primary[100]};
      }
      .input-wrapper.has-error .input-field {
        border-color: ${tokens.error[500]};
      }
      .input-wrapper.has-error .input-field:focus {
        box-shadow: 0 0 0 3px ${tokens.error[100]};
      }
      .input-field:disabled {
        background: ${tokens.neutral[100]};
        cursor: not-allowed;
      }
      .input-field.sm {
        padding: ${tokens.spacing[2]} ${tokens.spacing[3]};
        font-size: ${tokens.typography.fontSize.sm};
      }
      .input-field.md {
        padding: ${tokens.spacing[3]} ${tokens.spacing[4]};
        font-size: ${tokens.typography.fontSize.base};
      }
      .input-field.lg {
        padding: ${tokens.spacing[4]} ${tokens.spacing[5]};
        font-size: ${tokens.typography.fontSize.lg};
      }
      .input-hint {
        font-size: ${tokens.typography.fontSize.xs};
        color: ${tokens.neutral[500]};
      }
      .input-error {
        font-size: ${tokens.typography.fontSize.xs};
        color: ${tokens.error[600]};
      }
      .input-group {
        position: relative;
      }
      .input-icon {
        position: absolute;
        left: ${tokens.spacing[3]};
        top: 50%;
        transform: translateY(-50%);
        color: ${tokens.neutral[400]};
      }
      .input-field.has-icon {
        padding-left: 40px;
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

  render() {
    const hasError = this.error && this.error.length > 0;
    const hasIcon = this.hasAttribute('icon');

    this.shadowRoot.innerHTML = `
      <style>${DesignInput.styles}</style>
      <div class="input-wrapper ${hasError ? 'has-error' : ''}">
        ${this.label ? `<label class="input-label">${this.label}</label>` : ''}
        <div class="input-group">
          ${hasIcon ? `<span class="input-icon">${this.getAttribute('icon')}</span>` : ''}
          <input type="${this.type}" class="input-field ${this.size} ${hasIcon ? 'has-icon' : ''}"
            placeholder="${this.placeholder}" value="${this.value}" ${this.disabled ? 'disabled' : ''}>
        </div>
        ${this.hint ? `<div class="input-hint">${this.hint}</div>` : ''}
        ${hasError ? `<div class="input-error">${this.error}</div>` : ''}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const input = this.shadowRoot.querySelector('input');
    input?.addEventListener('input', () => {
      this.dispatchEvent(new CustomEvent('input', {
        detail: { value: input.value },
        bubbles: true,
        composed: true,
      }));
    });

    input?.addEventListener('change', () => {
      this.dispatchEvent(new CustomEvent('change', {
        detail: { value: input.value },
        bubbles: true,
        composed: true,
      }));
    });
  }
}

class DesignAvatar extends HTMLElement {
  constructor() {
    super();
    this.size = 'md';
    this.src = '';
    this.name = '';
    this.status = '';
  }

  static get observedAttributes() {
    return ['size', 'src', 'name', 'status'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: inline-block;
        position: relative;
      }
      .avatar {
        border-radius: ${tokens.radii.full};
        overflow: hidden;
        background: linear-gradient(135deg, ${tokens.primary[400]} 0%, ${tokens.secondary[400]} 100%);
        display: flex;
        align-items: center;
        justify-content: center;
        color: white;
        font-weight: ${tokens.typography.fontWeight.semibold};
        text-transform: uppercase;
      }
      .avatar.sm {
        width: 32px;
        height: 32px;
        font-size: ${tokens.typography.fontSize.xs};
      }
      .avatar.md {
        width: 40px;
        height: 40px;
        font-size: ${tokens.typography.fontSize.sm};
      }
      .avatar.lg {
        width: 56px;
        height: 56px;
        font-size: ${tokens.typography.fontSize.lg};
      }
      .avatar.xl {
        width: 80px;
        height: 80px;
        font-size: ${tokens.typography.fontSize['2xl']};
      }
      .avatar-img {
        width: 100%;
        height: 100%;
        object-fit: cover;
      }
      .status-dot {
        position: absolute;
        bottom: 0;
        right: 0;
        width: 12px;
        height: 12px;
        border-radius: 50%;
        border: 2px solid white;
      }
      .status-dot.online {
        background: ${tokens.success[500]};
      }
      .status-dot.offline {
        background: ${tokens.neutral[400]};
      }
      .status-dot.busy {
        background: ${tokens.error[500]};
      }
      .status-dot.away {
        background: ${tokens.warning[500]};
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

  getInitials(name) {
    if (!name) return '?';
    const parts = name.split(' ');
    if (parts.length >= 2) {
      return parts[0][0] + parts[1][0];
    }
    return name.substring(0, 2);
  }

  render() {
    const initials = this.getInitials(this.name);
    const hasStatus = this.status && this.status.length > 0;

    this.shadowRoot.innerHTML = `
      <style>${DesignAvatar.styles}</style>
      <div class="avatar ${this.size}">
        ${this.src ? `<img class="avatar-img" src="${this.src}" alt="${this.name}">` : initials}
      </div>
      ${hasStatus ? `<span class="status-dot ${this.status}"></span>` : ''}
    `;
  }
}

class DesignProgress extends HTMLElement {
  constructor() {
    super();
    this.value = 0;
    this.max = 100;
    this.size = 'md';
    this.showLabel = true;
  }

  static get observedAttributes() {
    return ['value', 'max', 'size', 'show-label'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: block;
      }
      .progress-wrapper {
        display: flex;
        align-items: center;
        gap: 12px;
      }
      .progress-bar {
        flex: 1;
        background: ${tokens.neutral[200]};
        border-radius: ${tokens.radii.full};
        overflow: hidden;
      }
      .progress-bar.sm {
        height: 4px;
      }
      .progress-bar.md {
        height: 8px;
      }
      .progress-bar.lg {
        height: 12px;
      }
      .progress-fill {
        height: 100%;
        background: linear-gradient(90deg, ${tokens.primary[500]} 0%, ${tokens.secondary[500]} 100%);
        border-radius: ${tokens.radii.full};
        transition: width ${tokens.transitions.slow};
      }
      .progress-label {
        font-family: ${tokens.typography.fontFamily.sans};
        font-size: ${tokens.typography.fontSize.sm};
        font-weight: ${tokens.typography.fontWeight.medium};
        color: ${tokens.neutral[600]};
        min-width: 40px;
        text-align: right;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = name === 'show-label' ? newValue !== null : newValue;
      this.render();
    }
  }

  render() {
    const percentage = Math.min(100, Math.max(0, (this.value / this.max) * 100));

    this.shadowRoot.innerHTML = `
      <style>${DesignProgress.styles}</style>
      <div class="progress-wrapper">
        <div class="progress-bar ${this.size}">
          <div class="progress-fill" style="width: ${percentage}%"></div>
        </div>
        ${this.showLabel ? `<span class="progress-label">${Math.round(percentage)}%</span>` : ''}
      </div>
    `;
  }
}

class DesignTooltip extends HTMLElement {
  constructor() {
    super();
    this.position = 'top';
    this.text = '';
  }

  static get observedAttributes() {
    return ['position', 'text'];
  }

  static get styles() {
    const tokens = DesignTokens;
    return `
      :host {
        display: inline-block;
        position: relative;
      }
      .tooltip-trigger {
        cursor: pointer;
      }
      .tooltip-content {
        position: absolute;
        background: ${tokens.neutral[800]};
        color: white;
        padding: ${tokens.spacing[2]} ${tokens.spacing[3]};
        border-radius: ${tokens.radii.md};
        font-size: ${tokens.typography.fontSize.xs};
        white-space: nowrap;
        opacity: 0;
        visibility: hidden;
        transition: all ${tokens.transitions.fast};
        z-index: 1000;
      }
      :host([position="top"]) .tooltip-content {
        bottom: 100%;
        left: 50%;
        transform: translateX(-50%) translateY(-8px);
      }
      :host([position="bottom"]) .tooltip-content {
        top: 100%;
        left: 50%;
        transform: translateX(-50%) translateY(8px);
      }
      :host([position="left"]) .tooltip-content {
        right: 100%;
        top: 50%;
        transform: translateY(-50%) translateX(-8px);
      }
      :host([position="right"]) .tooltip-content {
        left: 100%;
        top: 50%;
        transform: translateY(-50%) translateX(8px);
      }
      :host(:hover) .tooltip-content {
        opacity: 1;
        visibility: visible;
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

  render() {
    this.shadowRoot.innerHTML = `
      <style>${DesignTooltip.styles}</style>
      <span class="tooltip-trigger">
        <slot></slot>
      </span>
      ${this.text ? `<span class="tooltip-content">${this.text}</span>` : ''}
    `;
  }
}

class DesignThemeProvider extends HTMLElement {
  constructor() {
    super();
    this.theme = 'light';
  }

  static get observedAttributes() {
    return ['theme'];
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

  render() {
    const tokenStyles = this.generateTokenStyles();
    
    this.shadowRoot.innerHTML = `
      <style>
        ${tokenStyles}
        :host {
          display: contents;
        }
        * {
          box-sizing: border-box;
        }
      </style>
      <slot></slot>
    `;
  }

  generateTokenStyles() {
    const tokens = DesignTokens;
    let css = ':root {';
    
    for (const [category, values] of Object.entries(tokens)) {
      if (typeof values === 'object') {
        for (const [key, value] of Object.entries(values)) {
          if (typeof value === 'object') {
            for (const [subKey, subValue] of Object.entries(value)) {
              css += `--${category}-${key}-${subKey}: ${subValue}; `;
            }
          } else if (typeof value === 'string') {
            css += `--${category}-${key}: ${value}; `;
          }
        }
      }
    }
    
    css += '}';
    return css;
  }
}

export { DesignTokens, DesignButton, DesignCard, DesignBadge, DesignInput, DesignAvatar, DesignProgress, DesignTooltip, DesignThemeProvider };