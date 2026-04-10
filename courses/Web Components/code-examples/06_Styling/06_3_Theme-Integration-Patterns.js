/**
 * Theme Integration Patterns - Demonstrates dynamic theming systems for Web Components
 * including theme switching, preference detection, CSS custom property theming, and persistence
 * @module styling/06_3_Theme-Integration-Patterns
 * @version 1.0.0
 * @example <theme-provider theme="dark"></theme-provider>
 */

const DEFAULT_THEME_DATA = {
  light: {
    '--theme-bg': '#ffffff',
    '--theme-bg-secondary': '#f5f5f5',
    '--theme-text': '#333333',
    '--theme-text-secondary': '#666666',
    '--theme-border': '#e0e0e0',
    '--theme-primary': '#007bff',
    '--theme-primary-hover': '#0056b3',
    '--theme-success': '#28a745',
    '--theme-warning': '#ffc107',
    '--theme-danger': '#dc3545',
    '--theme-shadow': 'rgba(0, 0, 0, 0.1)',
    '--theme-input-bg': '#ffffff',
    '--theme-input-border': '#cccccc',
    '--theme-scrollbar': '#c1c1c1',
    '--theme-scrollbar-hover': '#a8a8a8'
  },
  dark: {
    '--theme-bg': '#1a1a1a',
    '--theme-bg-secondary': '#2d2d2d',
    '--theme-text': '#e0e0e0',
    '--theme-text-secondary': '#b0b0b0',
    '--theme-border': '#404040',
    '--theme-primary': '#4dabf7',
    '--theme-primary-hover': '#74c0fc',
    '--theme-success': '#51cf66',
    '--theme-warning': '#fcc419',
    '--theme-danger': '#ff6b6b',
    '--theme-shadow': 'rgba(0, 0, 0, 0.5)',
    '--theme-input-bg': '#2d2d2d',
    '--theme-input-border': '#555555',
    '--theme-scrollbar': '#555555',
    '--theme-scrollbar-hover': '#777777'
  },
  ocean: {
    '--theme-bg': '#e3f2fd',
    '--theme-bg-secondary': '#bbdefb',
    '--theme-text': '#0d47a1',
    '--theme-text-secondary': '#1976d2',
    '--theme-border': '#90caf9',
    '--theme-primary': '#0288d1',
    '--theme-primary-hover': '#03a9f4',
    '--theme-success': '#00acc1',
    '--theme-warning': '#ffb300',
    '--theme-danger': '#f4511e',
    '--theme-shadow': 'rgba(33, 150, 243, 0.2)',
    '--theme-input-bg': '#ffffff',
    '--theme-input-border': '#64b5f6',
    '--theme-scrollbar': '#42a5f5',
    '--theme-scrollbar-hover': '#1e88e5'
  },
  forest: {
    '--theme-bg': '#e8f5e9',
    '--theme-bg-secondary': '#c8e6c9',
    '--theme-text': '#1b5e20',
    '--theme-text-secondary': '#388e3c',
    '--theme-border': '#a5d6a7',
    '--theme-primary': '#43a047',
    '--theme-primary-hover': '#66bb6a',
    '--theme-success': '#2e7d32',
    '--theme-warning': '#ffa000',
    '--theme-danger': '#d32f2f',
    '--theme-shadow': 'rgba(56, 142, 60, 0.2)',
    '--theme-input-bg': '#ffffff',
    '--theme-input-border': '#81c784',
    '--theme-scrollbar': '#66bb6a',
    '--theme-scrollbar-hover': '#43a047'
  },
  nord: {
    '--theme-bg': '#2e3440',
    '--theme-bg-secondary': '#3b4252',
    '--theme-text': '#eceff4',
    '--theme-text-secondary': '#d8dee9',
    '--theme-border': '#4c566a',
    '--theme-primary': '#88c0d0',
    '--theme-primary-hover': '#5e81ac',
    '--theme-success': '#a3be8c',
    '--theme-warning': '#ebcb8b',
    '--theme-danger': '#bf616a',
    '--theme-shadow': 'rgba(0, 0, 0, 0.3)',
    '--theme-input-bg': '#3b4252',
    '--theme-input-border': '#4c566a',
    '--theme-scrollbar': '#4c566a',
    '--theme-scrollbar-hover': '#5e81ac'
  }
};

class ThemeProvider extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #currentTheme;
  #themeData;
  #preferenceObserver;
  #storageKey;

  static get observedAttributes() {
    return ['theme', 'storage-key', 'persist', 'respect-preference'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#currentTheme = 'light';
    this.#themeData = { ...DEFAULT_THEME_DATA };
    this.#preferenceObserver = null;
    this.#storageKey = 'theme-provider-data';
  }

  connectedCallback() {
    this.#setupStorageKey();
    this.#initializeTheme();
    this.#render();
    this.#setupPreferenceObserver();
  }

  disconnectedCallback() {
    this.#preferenceObserver?.disconnect();
  }

  #setupStorageKey() {
    const customKey = this.getAttribute('storage-key');
    if (customKey) {
      this.#storageKey = customKey;
    }
  }

  #initializeTheme() {
    const persist = this.hasAttribute('persist');
    const storedTheme = persist ? localStorage.getItem(this.#storageKey) : null;

    if (storedTheme && this.#themeData[storedTheme]) {
      this.#currentTheme = storedTheme;
    } else if (this.hasAttribute('respect-preference')) {
      this.#currentTheme = this.#getSystemPreference();
    } else {
      const defaultTheme = this.getAttribute('theme') || 'light';
      this.#currentTheme = this.#themeData[defaultTheme] ? defaultTheme : 'light';
    }
  }

  #getSystemPreference() {
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
      return 'dark';
    }
    return 'light';
  }

  #render() {
    this.#styleSheet.replaceSync(`
      :host {
        display: contents;
      }
    `);

    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    this.#applyTheme();
  }

  #applyTheme() {
    const themeVars = this.#themeData[this.#currentTheme];
    if (!themeVars) return;

    const root = document.documentElement;
    for (const [property, value] of Object.entries(themeVars)) {
      root.style.setProperty(property, value);
    }

    root.setAttribute('data-theme', this.#currentTheme);
  }

  #setupPreferenceObserver() {
    if (!this.hasAttribute('respect-preference')) return;

    if (window.matchMedia) {
      this.#preferenceObserver = window.matchMedia('(prefers-color-scheme: dark)');
      this.#preferenceObserver.addEventListener('change', this.#handlePreferenceChange.bind(this));
    }
  }

  #handlePreferenceChange(event) {
    if (!this.hasAttribute('respect-preference')) return;

    const newTheme = event.matches ? 'dark' : 'light';
    if (this.#themeData[newTheme]) {
      this.setAttribute('theme', newTheme);
    }
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;

    switch (name) {
      case 'theme':
        this.#handleThemeChange(newValue);
        break;
      case 'persist':
        this.#handlePersistChange();
        break;
      case 'respect-preference':
        this.#handlePreferenceSettingChange();
        break;
    }
  }

  #handleThemeChange(theme) {
    if (!theme || !this.#themeData[theme]) {
      theme = 'light';
    }

    this.#currentTheme = theme;
    this.#applyTheme();

    if (this.hasAttribute('persist')) {
      localStorage.setItem(this.#storageKey, theme);
    }

    this.dispatchEvent(
      new CustomEvent('theme-change', {
        bubbles: true,
        composed: true,
        detail: { theme, previousTheme: oldValue }
      })
    );
  }

  #handlePersistChange() {
    if (this.hasAttribute('persist')) {
      localStorage.setItem(this.#storageKey, this.#currentTheme);
    } else {
      localStorage.removeItem(this.#storageKey);
    }
  }

  #handlePreferenceSettingChange() {
    if (this.hasAttribute('respect-preference')) {
      this.#setupPreferenceObserver();
      const systemTheme = this.#getSystemPreference();
      if (!this.hasAttribute('theme')) {
        this.setAttribute('theme', systemTheme);
      }
    }
  }

  get theme() {
    return this.#currentTheme;
  }

  set theme(value) {
    this.setAttribute('theme', value);
  }

  get availableThemes() {
    return Object.keys(this.#themeData);
  }

  get registeredThemes() {
    return [...Object.keys(this.#themeData)];
  }

  registerTheme(name, variables) {
    if (typeof variables !== 'object' || !variables) {
      throw new Error('Theme variables must be an object');
    }

    this.#themeData[name] = { ...variables };

    this.dispatchEvent(
      new CustomEvent('theme-registered', {
        bubbles: true,
        composed: true,
        detail: { name, variables }
      })
    );
  }

  unregisterTheme(name) {
    if (DEFAULT_THEME_DATA[name]) {
      console.warn(`Cannot unregister built-in theme: ${name}`);
      return false;
    }

    if (this.#themeData[name]) {
      delete this.#themeData[name];
      return true;
    }

    return false;
  }

  getThemeVariables(themeName) {
    return this.#themeData[themeName] ? { ...this.#themeData[themeName] } : null;
  }

  applyThemeVariables(variables) {
    const root = document.documentElement;
    for (const [property, value] of Object.entries(variables)) {
      root.style.setProperty(property, value);
    }
  }

  resetToDefault() {
    const defaultTheme = this.getAttribute('theme') || 'light';
    this.#currentTheme = defaultTheme;
    this.#applyTheme();

    if (this.hasAttribute('persist')) {
      localStorage.setItem(this.#storageKey, defaultTheme);
    }
  }

  isValidTheme(themeName) {
    return Boolean(this.#themeData[themeName]);
  }
}

class ThemeableComponent extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #fallbackVariables;

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#fallbackVariables = {};
  }

  connectedCallback() {
    this.#setupStyles();
    this.#render();
    this.#observeThemeChanges();
  }

  #setupStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
        background: var(--theme-bg, var(--fallback-bg, #ffffff));
        color: var(--theme-text, var(--fallback-text, #333333));
        border: 1px solid var(--theme-border, var(--fallback-border, #e0e0e0));
        border-radius: 4px;
        padding: 16px;
        transition: background-color 0.3s ease, color 0.3s ease, border-color 0.3s ease;
      }

      .title {
        color: var(--theme-primary, var(--fallback-primary, #007bff));
        font-size: 1.25rem;
        font-weight: 600;
        margin-bottom: 12px;
      }

      .content {
        color: var(--theme-text-secondary, var(--fallback-text-secondary, #666666));
        line-height: 1.6;
      }
    `);
  }

  #render() {
    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    this.#shadowRoot.innerHTML = `
      <div class="title">
        <slot name="title">Themed Content</slot>
      </div>
      <div class="content">
        <slot></slot>
      </div>
    `;
  }

  #observeThemeChanges() {
    const observer = new MutationObserver((mutations) => {
      for (const mutation of mutations) {
        if (mutation.type === 'attributes' && mutation.attributeName === 'data-theme') {
          this.#handleThemeChange();
        }
      }
    });

    observer.observe(document.documentElement, {
      attributes: true,
      attributeFilter: ['data-theme']
    });
  }

  #handleThemeChange() {
    this.dispatchEvent(
      new CustomEvent('component-theme-update', {
        bubbles: true,
        composed: true
      })
    );
  }

  setFallbackVariables(variables) {
    this.#fallbackVariables = { ...variables };
    this.#styleSheet.replaceSync(`
      :host {
        display: block;
        background: var(--theme-bg, var(--fallback-bg, ${this.#fallbackVariables.bg || '#ffffff'}));
        color: var(--theme-text, var(--fallback-text, ${this.#fallbackVariables.text || '#333333'}));
        border: 1px solid var(--theme-border, var(--fallback-border, ${this.#fallbackVariables.border || '#e0e0e0'}));
        border-radius: 4px;
        padding: 16px;
        transition: all 0.3s ease;
      }
    `);
  }
}

class ThemeSwitcher extends HTMLElement {
  #shadowRoot;
  #styleSheet;
  #buttons;

  static get observedAttributes() {
    return ['themes', 'default'];
  }

  constructor() {
    super();
    this.#shadowRoot = this.attachShadow({ mode: 'open' });
    this.#styleSheet = new CSSStyleSheet();
    this.#buttons = [];
  }

  connectedCallback() {
    this.#setupStyles();
    this.#render();
    this.#setupEventListeners();
    this.#highlightCurrentTheme();
  }

  #setupStyles() {
    this.#styleSheet.replaceSync(`
      :host {
        display: flex;
        gap: 8px;
        flex-wrap: wrap;
      }

      .theme-btn {
        background: var(--theme-bg-secondary, #f5f5f5);
        border: 2px solid var(--theme-border, #e0e0e0);
        border-radius: 4px;
        color: var(--theme-text, #333333);
        cursor: pointer;
        font-family: inherit;
        font-size: 0.875rem;
        padding: 8px 12px;
        transition: all 0.2s ease;
      }

      .theme-btn:hover {
        background: var(--theme-primary, #007bff);
        color: white;
        border-color: var(--theme-primary, #007bff);
      }

      .theme-btn[aria-selected="true"] {
        background: var(--theme-primary, #007bff);
        border-color: var(--theme-primary, #007bff);
        color: white;
      }
    `);
  }

  #render() {
    const themesAttr = this.getAttribute('themes') || 'light,dark';
    const themes = themesAttr.split(',').map(t => t.trim());

    this.#shadowRoot.adoptedStyleSheets = [this.#styleSheet];
    this.#shadowRoot.innerHTML = themes
      .map(name => `<button type="button" class="theme-btn" data-theme="${name}" aria-label="Switch to ${name} theme">${name}</button>`)
      .join('');
  }

  #setupEventListeners() {
    const buttons = this.#shadowRoot.querySelectorAll('.theme-btn');
    buttons.forEach(btn => {
      btn.addEventListener('click', this.#handleThemeButtonClick.bind(this));
    });
  }

  #handleThemeButtonClick(event) {
    const theme = event.target.dataset.theme;
    if (theme) {
      this.#switchTheme(theme);
    }
  }

  #switchTheme(theme) {
    const provider = document.querySelector('theme-provider');
    if (provider) {
      provider.theme = theme;
    } else {
      document.documentElement.setAttribute('data-theme', theme);
    }

    this.#highlightCurrentTheme();
  }

  #highlightCurrentTheme() {
    const currentTheme = document.documentElement.getAttribute('data-theme') || 'light';
    const buttons = this.#shadowRoot.querySelectorAll('.theme-btn');

    buttons.forEach(btn => {
      const isSelected = btn.dataset.theme === currentTheme;
      btn.setAttribute('aria-selected', String(isSelected));
    });
  }
}

window.customElements.define('theme-provider', ThemeProvider);
window.customElements.define('themeable-component', ThemeableComponent);
window.customElements.define('theme-switcher', ThemeSwitcher);

export { ThemeProvider, ThemeableComponent, ThemeSwitcher };