/**
 * Framework-Neutral Patterns - Universal Web Component implementations
 * @module interoperability/08_1_Framework-Neutral-Patterns
 * @version 1.0.0
 * @example <universal-card></universal-card>
 */

class UniversalCard extends HTMLElement {
    static get observedAttributes() {
        return ['title', 'description', 'image', 'variant', 'disabled', 'loading'];
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this._data = null;
        this._loading = false;
        this._disabled = false;
        this._variant = 'default';
        this._rendered = false;
    }

    connectedCallback() {
        if (!this._rendered) {
            this._render();
            this._rendered = true;
        }
        this._setupEventListeners();
        this._updateFromAttributes();
    }

    disconnectedCallback() {
        this._removeEventListeners();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue === newValue) return;
        
        switch (name) {
            case 'title':
            case 'description':
            case 'image':
                this._updateContent();
                break;
            case 'variant':
                this._variant = newValue || 'default';
                this._updateVariant();
                break;
            case 'disabled':
                this._disabled = this.hasAttribute('disabled');
                this._updateDisabledState();
                break;
            case 'loading':
                this._loading = this.hasAttribute('loading');
                this._updateLoadingState();
                break;
        }
    }

    _render() {
        this.shadowRoot.innerHTML = `
            <style>
                :host {
                    display: block;
                    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                    --card-bg: #ffffff;
                    --card-border: #e0e0e0;
                    --card-text: #333333;
                    --card-accent: #007bff;
                    --card-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
                    --card-radius: 8px;
                    --card-padding: 16px;
                }

                :host([variant="outlined"]) {
                    --card-bg: transparent;
                    --card-border: 1px solid var(--card-border);
                    --card-shadow: none;
                }

                :host([variant="elevated"]) {
                    --card-shadow: 0 4px 16px rgba(0, 0, 0, 0.15);
                }

                :host([variant="flat"]) {
                    --card-shadow: none;
                    --card-border: none;
                }

                :host([disabled]) {
                    opacity: 0.5;
                    pointer-events: none;
                }

                .card {
                    background: var(--card-bg);
                    border: var(--card-border);
                    border-radius: var(--card-radius);
                    box-shadow: var(--card-shadow);
                    overflow: hidden;
                    transition: transform 0.2s ease, box-shadow 0.2s ease;
                }

                .card:hover {
                    transform: translateY(-2px);
                    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.15);
                }

                .card-image {
                    width: 100%;
                    height: 200px;
                    object-fit: cover;
                    display: none;
                }

                .card-image[src] {
                    display: block;
                }

                .card-content {
                    padding: var(--card-padding);
                }

                .card-title {
                    margin: 0 0 8px;
                    font-size: 1.25rem;
                    font-weight: 600;
                    color: var(--card-text);
                }

                .card-description {
                    margin: 0;
                    font-size: 0.875rem;
                    color: #666;
                    line-height: 1.5;
                }

                .card-actions {
                    padding: var(--card-padding);
                    padding-top: 0;
                    display: flex;
                    gap: 8px;
                }

                button {
                    padding: 8px 16px;
                    border: none;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 0.875rem;
                    transition: background 0.2s ease;
                }

                button.primary {
                    background: var(--card-accent);
                    color: white;
                }

                button.secondary {
                    background: #f0f0f0;
                    color: var(--card-text);
                }

                button:hover {
                    filter: brightness(1.1);
                }

                button:active {
                    transform: scale(0.98);
                }

                .loading-overlay {
                    position: absolute;
                    inset: 0;
                    background: rgba(255, 255, 255, 0.9);
                    display: none;
                    align-items: center;
                    justify-content: center;
                    z-index: 10;
                }

                :host([loading]) .loading-overlay {
                    display: flex;
                }

                .spinner {
                    width: 32px;
                    height: 32px;
                    border: 3px solid #f0f0f0;
                    border-top-color: var(--card-accent);
                    border-radius: 50%;
                    animation: spin 0.8s linear infinite;
                }

                @keyframes spin {
                    to { transform: rotate(360deg); }
                }
            </style>
            <div class="card">
                <img class="card-image" alt="" />
                <div class="card-content">
                    <h3 class="card-title"></h3>
                    <p class="card-description"></p>
                </div>
                <div class="card-actions">
                    <button class="primary">Accept</button>
                    <button class="secondary">Decline</button>
                </div>
                <div class="loading-overlay">
                    <div class="spinner"></div>
                </div>
            </div>
        `;
    }

    _setupEventListeners() {
        const primaryBtn = this.shadowRoot.querySelector('button.primary');
        const secondaryBtn = this.shadowRoot.querySelector('button.secondary');

        primaryBtn?.addEventListener('click', () => this._handleAction('accept'));
        secondaryBtn?.addEventListener('click', () => this._handleAction('decline'));
    }

    _removeEventListeners() {
        const primaryBtn = this.shadowRoot.querySelector('button.primary');
        const secondaryBtn = this.shadowRoot.querySelector('button.secondary');

        primaryBtn?.removeEventListener('click', () => this._handleAction('accept'));
        secondaryBtn?.removeEventListener('click', () => this._handleAction('decline'));
    }

    _handleAction(action) {
        if (this._disabled) return;

        const event = new CustomEvent(`card:${action}`, {
            bubbles: true,
            composed: true,
            detail: {
                action,
                data: this._data,
                timestamp: Date.now()
            }
        });

        this.dispatchEvent(event);
    }

    _updateFromAttributes() {
        this._updateContent();
        this._updateVariant();
        this._updateDisabledState();
        this._updateLoadingState();
    }

    _updateContent() {
        const titleEl = this.shadowRoot.querySelector('.card-title');
        const descEl = this.shadowRoot.querySelector('.card-description');
        const imgEl = this.shadowRoot.querySelector('.card-image');

        if (titleEl) {
            titleEl.textContent = this.getAttribute('title') || '';
        }
        if (descEl) {
            descEl.textContent = this.getAttribute('description') || '';
        }
        if (imgEl) {
            const src = this.getAttribute('image');
            if (src) {
                imgEl.src = src;
                imgEl.alt = this.getAttribute('title') || '';
            }
        }
    }

    _updateVariant() {
        const card = this.shadowRoot.querySelector('.card');
        if (card) {
            card.dataset.variant = this._variant;
        }
    }

    _updateDisabledState() {
        const buttons = this.shadowRoot.querySelectorAll('button');
        buttons.forEach(btn => {
            btn.disabled = this._disabled;
        });
    }

    _updateLoadingState() {
        // Loading state handled via attribute and CSS
    }

    get data() {
        return this._data;
    }

    set data(value) {
        this._data = value;
        if (value) {
            if (value.title) this.setAttribute('title', value.title);
            if (value.description) this.setAttribute('description', value.description);
            if (value.image) this.setAttribute('image', value.image);
            if (value.variant) this.setAttribute('variant', value.variant);
        }
    }

    get loading() {
        return this._loading;
    }

    set loading(value) {
        this._loading = value;
        if (value) {
            this.setAttribute('loading', '');
        } else {
            this.removeAttribute('loading');
        }
    }

    get disabled() {
        return this._disabled;
    }

    set disabled(value) {
        this._disabled = value;
        if (value) {
            this.setAttribute('disabled', '');
        } else {
            this.removeAttribute('disabled');
        }
    }

    accept() {
        this._handleAction('accept');
    }

    decline() {
        this._handleAction('decline');
    }

    focus() {
        const primaryBtn = this.shadowRoot.querySelector('button.primary');
        primaryBtn?.focus();
    }
}

class FrameworkNeutralModal extends HTMLElement {
    static get observedAttributes() {
        return ['open', 'title', 'size', 'closable'];
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this._isOpen = false;
        this._previousActiveElement = null;
    }

    connectedCallback() {
        this._render();
        this._setupKeyboardListeners();
    }

    disconnectedCallback() {
        this._removeKeyboardListeners();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (oldValue === newValue) return;

        switch (name) {
            case 'open':
                this._isOpen = this.hasAttribute('open');
                this._updateVisibility();
                break;
            case 'title':
                this._updateTitle();
                break;
            case 'size':
                this._updateSize();
                break;
            case 'closable':
                this._updateClosable();
                break;
        }
    }

    _render() {
        this.shadowRoot.innerHTML = `
            <style>
                :host {
                    --modal-overlay-bg: rgba(0, 0, 0, 0.5);
                    --modal-content-bg: #ffffff;
                    --modal-border-radius: 8px;
                    --modal-shadow: 0 4px 24px rgba(0, 0, 0, 0.2);
                    --modal-sm: 320px;
                    --modal-md: 480px;
                    --modal-lg: 720px;
                    font-family: inherit;
                }

                .overlay {
                    position: fixed;
                    inset: 0;
                    background: var(--modal-overlay-bg);
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    z-index: 9999;
                    opacity: 0;
                    visibility: hidden;
                    transition: opacity 0.3s ease, visibility 0.3s ease;
                }

                :host([open]) .overlay {
                    opacity: 1;
                    visibility: visible;
                }

                .modal {
                    background: var(--modal-content-bg);
                    border-radius: var(--modal-border-radius);
                    box-shadow: var(--modal-shadow);
                    width: 100%;
                    max-width: var(--modal-md);
                    max-height: 90vh;
                    overflow: hidden;
                    display: flex;
                    flex-direction: column;
                    transform: scale(0.9);
                    transition: transform 0.3s ease;
                }

                :host([open]) .modal {
                    transform: scale(1);
                }

                :host([size="small"]) .modal {
                    max-width: var(--modal-sm);
                }

                :host([size="large"]) .modal {
                    max-width: var(--modal-lg);
                }

                .header {
                    padding: 16px 20px;
                    border-bottom: 1px solid #e0e0e0;
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                }

                .title {
                    margin: 0;
                    font-size: 1.125rem;
                    font-weight: 600;
                }

                .close-btn {
                    background: none;
                    border: none;
                    font-size: 1.5rem;
                    cursor: pointer;
                    padding: 0;
                    line-height: 1;
                    color: #666;
                }

                .close-btn:hover {
                    color: #000;
                }

                .body {
                    padding: 20px;
                    overflow-y: auto;
                    flex: 1;
                }

                .footer {
                    padding: 16px 20px;
                    border-top: 1px solid #e0e0e0;
                    display: flex;
                    justify-content: flex-end;
                    gap: 8px;
                }

                button {
                    padding: 8px 16px;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 0.875rem;
                }

                button.primary {
                    background: #007bff;
                    color: white;
                    border: none;
                }

                button.secondary {
                    background: #f0f0f0;
                    border: 1px solid #d0d0d0;
                }
            </style>
            <div class="overlay" part="overlay">
                <div class="modal" part="modal">
                    <div class="header" part="header">
                        <h2 class="title" part="title"></h2>
                        <button class="close-btn" part="close-btn" aria-label="Close">&times;</button>
                    </div>
                    <div class="body" part="body">
                        <slot></slot>
                    </div>
                    <div class="footer" part="footer">
                        <button class="secondary">Cancel</button>
                        <button class="primary">Confirm</button>
                    </div>
                </div>
            </div>
        `;

        this._setupEventListeners();
    }

    _setupEventListeners() {
        const overlay = this.shadowRoot.querySelector('.overlay');
        const closeBtn = this.shadowRoot.querySelector('.close-btn');
        const cancelBtn = this.shadowRoot.querySelector('button.secondary');
        const confirmBtn = this.shadowRoot.querySelector('button.primary');

        overlay?.addEventListener('click', (e) => {
            if (e.target === overlay && this.hasAttribute('closable')) {
                this.close();
            }
        });

        closeBtn?.addEventListener('click', () => this.close());
        cancelBtn?.addEventListener('click', () => {
            this._emitEvent('cancel');
            this.close();
        });
        confirmBtn?.addEventListener('click', () => {
            this._emitEvent('confirm');
            this.close();
        });
    }

    _setupKeyboardListeners() {
        this._keydownHandler = (e) => {
            if (!this._isOpen) return;
            if (e.key === 'Escape' && this.hasAttribute('closable')) {
                this.close();
            }
            if (e.key === 'Tab') {
                this._trapFocus(e);
            }
        };
        document.addEventListener('keydown', this._keydownHandler);
    }

    _removeKeyboardListeners() {
        if (this._keydownHandler) {
            document.removeEventListener('keydown', this._keydownHandler);
        }
    }

    _trapFocus(e) {
        const focusableElements = this.shadowRoot.querySelectorAll(
            'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
        );
        const firstEl = focusableElements[0];
        const lastEl = focusableElements[focusableElements.length - 1];

        if (e.shiftKey && document.activeElement === firstEl) {
            e.preventDefault();
            lastEl?.focus();
        } else if (!e.shiftKey && document.activeElement === lastEl) {
            e.preventDefault();
            firstEl?.focus();
        }
    }

    _updateVisibility() {
        if (this._isOpen) {
            this._previousActiveElement = document.activeElement;
            document.body.style.overflow = 'hidden';
            setTimeout(() => {
                this.shadowRoot.querySelector('.close-btn')?.focus();
            }, 100);
        } else {
            document.body.style.overflow = '';
            this._previousActiveElement?.focus();
        }
    }

    _updateTitle() {
        const titleEl = this.shadowRoot.querySelector('.title');
        if (titleEl) {
            titleEl.textContent = this.getAttribute('title') || '';
        }
    }

    _updateSize() {
        // Size handled via CSS and attribute
    }

    _updateClosable() {
        // Closable behavior handled in event listeners
    }

    _emitEvent(name) {
        this.dispatchEvent(new CustomEvent(name, {
            bubbles: true,
            composed: true,
            detail: { timestamp: Date.now() }
        }));
    }

    open() {
        this.setAttribute('open', '');
    }

    close() {
        if (!this.hasAttribute('closable')) return;
        this.removeAttribute('open');
    }

    toggle() {
        if (this._isOpen) {
            this.close();
        } else {
            this.open();
        }
    }
}

customElements.define('universal-card', UniversalCard);
customElements.define('framework-neutral-modal', FrameworkNeutralModal);

export { UniversalCard, FrameworkNeutralModal };
