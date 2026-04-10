/**
 * Component Theming Systems - Advanced Theming for Web Components
 * @description CSS Custom Properties, theme switching, dark mode, dynamic themes, and design token systems
 * @module advanced/theming-systems
 * @version 1.0.0
 */

(function() {
  'use strict';

  const DesignTokens = {
    colors: {
      primary: { value: '#667eea', cssVar: '--color-primary' },
      secondary: { value: '#764ba2', cssVar: '--color-secondary' },
      success: { value: '#10b981', cssVar: '--color-success' },
      warning: { value: '#f59e0b', cssVar: '--color-warning' },
      error: { value: '#ef4444', cssVar: '--color-error' },
      info: { value: '#3b82f6', cssVar: '--color-info' },
      background: { value: '#ffffff', cssVar: '--color-background' },
      surface: { value: '#f9fafb', cssVar: '--color-surface' },
      text: { value: '#111827', cssVar: '--color-text' },
      'text-secondary': { value: '#6b7280', cssVar: '--color-text-secondary' },
      border: { value: '#e5e7eb', cssVar: '--color-border' }
    },
    spacing: {
      xs: { value: '4px', cssVar: '--spacing-xs' },
      sm: { value: '8px', cssVar: '--spacing-sm' },
      md: { value: '16px', cssVar: '--spacing-md' },
      lg: { value: '24px', cssVar: '--spacing-lg' },
      xl: { value: '32px', cssVar: '--spacing-xl' },
      xxl: { value: '48px', cssVar: '--spacing-xxl' }
    },
    typography: {
      'font-family': { value: 'system-ui, -apple-system, sans-serif', cssVar: '--font-family' },
      'font-size-xs': { value: '12px', cssVar: '--font-size-xs' },
      'font-size-sm': { value: '14px', cssVar: '--font-size-sm' },
      'font-size-base': { value: '16px', cssVar: '--font-size-base' },
      'font-size-lg': { value: '18px', cssVar: '--font-size-lg' },
      'font-size-xl': { value: '24px', cssVar: '--font-size-xl' },
      'font-size-2xl': { value: '30px', cssVar: '--font-size-2xl' },
      'font-weight-normal': { value: '400', cssVar: '--font-weight-normal' },
      'font-weight-medium': { value: '500', cssVar: '--font-weight-medium' },
      'font-weight-semibold': { value: '600', cssVar: '--font-weight-semibold' },
      'font-weight-bold': { value: '700', cssVar: '--font-weight-bold' },
      'line-height-tight': { value: '1.25', cssVar: '--line-height-tight' },
      'line-height-normal': { value: '1.5', cssVar: '--line-height-normal' },
      'line-height-relaxed': { value: '1.75', cssVar: '--line-height-relaxed' }
    },
    borderRadius: {
      none: { value: '0px', cssVar: '--border-radius-none' },
      sm: { value: '4px', cssVar: '--border-radius-sm' },
      md: { value: '6px', cssVar: '--border-radius-md' },
      lg: { value: '8px', cssVar: '--border-radius-lg' },
      xl: { value: '12px', cssVar: '--border-radius-xl' },
      full: { value: '9999px', cssVar: '--border-radius-full' }
    },
    shadows: {
      sm: { value: '0 1px 2px 0 rgba(0, 0, 0, 0.05)', cssVar: '--shadow-sm' },
      md: { value: '0 4px 6px -1px rgba(0, 0, 0, 0.1)', cssVar: '--shadow-md' },
      lg: { value: '0 10px 15px -3px rgba(0, 0, 0, 0.1)', cssVar: '--shadow-lg' },
      xl: { value: '0 20px 25px -5px rgba(0, 0, 0, 0.1)', cssVar: '--shadow-xl' }
    },
    transitions: {
      fast: { value: '150ms', cssVar: '--transition-fast' },
      normal: { value: '200ms', cssVar: '--transition-normal' },
      slow: { value: '300ms', cssVar: '--transition-slow' }
    },
    zIndex: {
      dropdown: { value: '1000', cssVar: '--z-index-dropdown' },
      sticky: { value: '1100', cssVar: '--z-index-sticky' },
      modal: { value: '1200', cssVar: '--z-index-modal' },
      popover: { value: '1300', cssVar: '--z-index-popover' },
      tooltip: { value: '1400', cssVar: '--z-index-tooltip' }
    }
  };

  const PRESET_THEMES = {
    light: {
      name: 'Light',
      colors: {
        background: '#ffffff',
        surface: '#f9fafb',
        text: '#111827',
        'text-secondary': '#6b7280',
        border: '#e5e7eb'
      }
    },
    dark: {
      name: 'Dark',
      colors: {
        background: '#111827',
        surface: '#1f2937',
        text: '#f9fafb',
        'text-secondary': '#9ca3af',
        border: '#374151'
      }
    },
    highContrast: {
      name: 'High Contrast',
      colors: {
        background: '#000000',
        surface: '#000000',
        text: '#ffffff',
        'text-secondary': '#e5e5e5',
        border: '#ffffff'
      }
    },
    sepia: {
      name: 'Sepia',
      colors: {
        background: '#f4ecd8',
        surface: '#ebe1ce',
        text: '#433422',
        'text-secondary': '#6b5c4c',
        border: '#d4c4b0'
      }
    }
  };

  class ThemeManager {
    constructor(options = {}) {
      this.themes = options.themes || PRESET_THEMES;
      this.currentTheme = options.defaultTheme || 'light';
      this.listeners = [];
      this.storageKey = options.storageKey || 'theme-manager-current';
      this.useSystemPreference = options.useSystemPreference !== false;
      this.cssVariablePrefix = options.cssVariablePrefix || '';
      this.loadSavedTheme();
      this.setupSystemThemeListener();
    }

    loadSavedTheme() {
      try {
        const saved = localStorage.getItem(this.storageKey);
        if (saved && this.themes[saved]) {
          this.currentTheme = saved;
        } else if (this.useSystemPreference) {
          this.currentTheme = this.getSystemTheme();
        }
      } catch (e) {
        console.warn('Failed to load saved theme:', e);
      }
    }

    setupSystemThemeListener() {
      if (!this.useSystemPreference) return;

      const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
      mediaQuery.addEventListener('change', (e) => {
        if (!localStorage.getItem(this.storageKey)) {
          this.setTheme(e.matches ? 'dark' : 'light', false);
        }
      });
    }

    getSystemTheme() {
      try {
        const isDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        return isDark ? 'dark' : 'light';
      } catch (e) {
        return 'light';
      }
    }

    getTheme(themeName) {
      return this.themes[themeName];
    }

    getCurrentTheme() {
      return this.themes[this.currentTheme];
    }

    setTheme(themeName, save = true) {
      if (!this.themes[themeName]) {
        console.warn(`Theme "${themeName}" not found`);
        return false;
      }

      const previousTheme = this.currentTheme;
      this.currentTheme = themeName;

      if (save) {
        try {
          localStorage.setItem(this.storageKey, themeName);
        } catch (e) {
          console.warn('Failed to save theme:', e);
        }
      }

      this.applyTheme(this.themes[themeName]);
      this.notifyListeners(themeName, previousTheme);

      return true;
    }

    applyTheme(theme) {
      const root = document.documentElement;

      for (const [category, tokens] of Object.entries(DesignTokens)) {
        for (const [key, token] of Object.entries(tokens)) {
          const cssVar = `${this.cssVariablePrefix}${token.cssVar}`;
          if (theme.colors && theme.colors[key] !== undefined) {
            root.style.setProperty(cssVar, theme.colors[key]);
          } else if (theme.colors && theme.colors[category]?.[key]) {
            root.style.setProperty(cssVar, theme.colors[category][key]);
          } else if (token.value) {
            root.style.setProperty(cssVar, token.value);
          }
        }
      }

      document.body?.setAttribute('data-theme', theme.name?.toLowerCase() || this.currentTheme);
    }

    registerTheme(name, theme) {
      this.themes[name] = theme;
    }

    addListener(callback) {
      this.listeners.push(callback);
      return () => {
        this.listeners = this.listeners.filter(l => l !== callback);
      };
    }

    notifyListeners(newTheme, oldTheme) {
      const event = { theme: newTheme, previousTheme: oldTheme };
      this.listeners.forEach(callback => callback(event));

      const customEvent = new CustomEvent('theme-change', {
        detail: event,
        bubbles: true,
        composed: true
      });
      document.dispatchEvent(customEvent);
    }

    getAvailableThemes() {
      return Object.entries(this.themes).map(([key, theme]) => ({
        id: key,
        name: theme.name || key
      }));
    }
  }

  class CSSVariableManager {
    constructor(prefix = '') {
      this.prefix = prefix;
      this.root = document.documentElement;
    }

    setVariable(name, value) {
      const varName = this.prefix + name;
      this.root.style.setProperty(varName, value);
    }

    getVariable(name) {
      const varName = this.prefix + name;
      return getComputedStyle(this.root).getPropertyValue(varName).trim();
    }

    removeVariable(name) {
      const varName = this.prefix + name;
      this.root.style.removeProperty(varName);
    }

    setVariables(tokens) {
      for (const [name, value] of Object.entries(tokens)) {
        this.setVariable(name, value);
      }
    }

    setTokenCategory(category) {
      const tokens = DesignTokens[category];
      if (!tokens) return;

      for (const [key, token] of Object.entries(tokens)) {
        if (token.cssVar) {
          this.setVariable(token.cssVar, token.value);
        }
      }
    }

    setAllTokens() {
      for (const category of Object.keys(DesignTokens)) {
        this.setTokenCategory(category);
      }
    }
  }

  class ThemeableComponent extends HTMLElement {
    static get observedAttributes() {
      return ['theme', 'theme-mode'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._theme = '';
      this._themeMode = 'auto';
    }

    static get observedAttributes() {
      return ['theme', 'theme-mode', 'no-theme'];
    }

    connectedCallback() {
      this._theme = this.getAttribute('theme') || '';
      this._themeMode = this.getAttribute('theme-mode') || 'auto';
      this.initialize();
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'theme') {
          this._theme = newValue;
        } else if (name === 'theme-mode') {
          this._themeMode = newValue;
        }
        this.applyThemeOverride();
      }
    }

    initialize() {
      window.addEventListener('theme-change', this.handleThemeChange.bind(this));
    }

    handleThemeChange(event) {
      if (this._themeMode === 'auto') {
        this.applyThemeOverride();
      }
    }

    applyThemeOverride() {
      if (this._theme) {
        this.style.setProperty('--component-theme', this._theme);
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
            --component-theme: var(--theme, inherit);
          }
          .container {
            background: var(--color-surface, var(--color-background));
            color: var(--color-text);
            border: 1px solid var(--color-border);
            border-radius: var(--border-radius-md, 6px);
            padding: var(--spacing-md, 16px);
          }
        </style>
        <div class="container">
          <slot></slot>
        </div>
      `;
    }
  }

  class ThemeSwitcher extends HTMLElement {
    static get observedAttributes() {
      return ['current-theme'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._manager = new ThemeManager();
      this._currentTheme = this._manager.currentTheme;
    }

    static get observedAttributes() {
      return ['current-theme', 'show-labels', 'position'];
    }

    connectedCallback() {
      this._manager = new ThemeManager();
      this._manager.addListener(({ theme }) => {
        this._currentTheme = theme;
        this.updateActiveState();
      });
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue && name === 'current-theme') {
        this._manager.setTheme(newValue);
      }
    }

    setTheme(themeName) {
      this._manager.setTheme(themeName);
    }

    updateActiveState() {
      const buttons = this.shadowRoot.querySelectorAll('.theme-button');
      buttons.forEach(button => {
        button.classList.toggle('active', button.dataset.theme === this._currentTheme);
      });
    }

    render() {
      const themes = this._manager.getAvailableThemes();
      const showLabels = this.hasAttribute('show-labels');

      this.shadowRoot.innerHTML = `
        <style>
          .theme-switcher {
            display: inline-flex;
            gap: 4px;
            padding: 4px;
            background: var(--color-surface, #f3f4f6);
            border-radius: var(--border-radius-lg, 8px);
          }
          .theme-button {
            padding: 8px 12px;
            border: none;
            background: transparent;
            border-radius: var(--border-radius-md, 6px);
            cursor: pointer;
            font-size: 14px;
            transition: all 0.2s;
          }
          .theme-button:hover {
            background: var(--color-background, #e5e7eb);
          }
          .theme-button.active {
            background: var(--color-primary, #667eea);
            color: white;
          }
          .theme-label {
            display: none;
          }
          :host([show-labels]) .theme-label {
            display: inline;
          }
        </style>
        <div class="theme-switcher">
          ${themes.map(theme => `
            <button class="theme-button" data-theme="${theme.id}" title="${theme.name}">
              ${theme.id}
            </button>
          `).join('')}
        </div>
      `;

      const buttons = this.shadowRoot.querySelectorAll('.theme-button');
      buttons.forEach(button => {
        button.addEventListener('click', () => {
          const theme = button.dataset.theme;
          this._manager.setTheme(theme);
        });
      });

      this.updateActiveState();
    }
  }

  class DarkModeToggle extends HTMLElement {
    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._manager = new ThemeManager();
    }

    connectedCallback() {
      this.render();
    }

    toggle() {
      const newTheme = this._manager.currentTheme === 'dark' ? 'light' : 'dark';
      this._manager.setTheme(newTheme);
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          button {
            padding: 8px 12px;
            border: 1px solid var(--color-border);
            background: var(--color-background);
            border-radius: var(--border-radius-md, 6px);
            cursor: pointer;
          }
          button:hover {
            background: var(--color-surface);
          }
        </style>
        <button class="toggle-button" type="button" aria-label="Toggle dark mode">
          🌙
        </button>
      `;

      const button = this.shadowRoot.querySelector('.toggle-button');
      button.addEventListener('click', this.toggle.bind(this));
    }
  }

  customElements.define('themeable-component', ThemeableComponent);
  customElements.define('theme-switcher', ThemeSwitcher);
  customElements.define('dark-mode-toggle', DarkModeToggle);

  if (typeof window !== 'undefined') {
    window.ComponentTheming = {
      DesignTokens,
      PRESET_THEMES,
      ThemeManager,
      CSSVariableManager,
      ThemeableComponent,
      ThemeSwitcher,
      DarkModeToggle
    };
  }

  export {
    DesignTokens,
    PRESET_THEMES,
    ThemeManager,
    CSSVariableManager,
    ThemeableComponent,
    ThemeSwitcher,
    DarkModeToggle
  };
})();