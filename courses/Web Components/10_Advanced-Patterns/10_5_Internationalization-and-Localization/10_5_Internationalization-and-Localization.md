# Internationalization and Localization

## OVERVIEW

Internationalization (i18n) enables Web Components to support multiple languages and regions. This guide covers i18n patterns, translation loading, and locale-aware formatting.

## IMPLEMENTATION DETAILS

### Basic i18n Pattern

```javascript
class I18nElement extends HTMLElement {
  #locale = 'en';
  #translations = {};
  
  static get observedAttributes() { return ['locale']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this.#loadTranslations(this.#locale);
  }
  
  attributeChangedCallback(name, newVal) {
    if (name === 'locale') {
      this.#loadTranslations(newVal);
      this.render();
    }
  }
  
  async #loadTranslations(locale) {
    const translations = await import(`./locales/${locale}.js`);
    this.#translations = translations.default || translations;
  }
  
  t(key) {
    return this.#translations[key] || key;
  }
  
  render() {
    this.shadowRoot.innerHTML = `<div>${this.t('greeting')}</div>`;
  }
}
```

### Locale-Aware Formatting

```javascript
class FormattedElement extends HTMLElement {
  formatNumber(value, locale = 'en-US') {
    return new Intl.NumberFormat(locale).format(value);
  }
  
  formatDate(date, locale = 'en-US') {
    return new Intl.DateTimeFormat(locale).format(date);
  }
  
  formatCurrency(value, currency, locale = 'en-US') {
    return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency
    }).format(value);
  }
}
```

## NEXT STEPS

Proceed to **10_Advanced-Patterns/10_6_Enterprise-Architecture-Patterns**.