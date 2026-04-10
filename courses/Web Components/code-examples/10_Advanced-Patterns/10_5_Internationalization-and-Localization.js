/**
 * Internationalization and Localization - i18n patterns for Web Components with
 * translations, pluralization, date/time formatting, and RTL support
 * @module advanced-patterns/10_5_Internationalization-and-Localization
 * @version 1.0.0
 * @example <i18n-component></i18n-component>
 */

class I18nManager {
  constructor(defaultLocale = 'en') {
    this.defaultLocale = defaultLocale;
    this.currentLocale = defaultLocale;
    this.translations = new Map();
    this.fallbacks = new Map();
    this.pluralRules = new Map();
    this.formatters = new Map();
  }

  setLocale(locale) {
    const oldLocale = this.currentLocale;
    this.currentLocale = locale;
    this.initPluralRule(locale);
    this.initFormatters(locale);
    return { oldLocale, newLocale: locale };
  }

  getLocale() {
    return this.currentLocale;
  }

  addTranslation(locale, key, value, context = {}) {
    if (!this.translations.has(locale)) {
      this.translations.set(locale, new Map());
    }
    this.translations.get(locale).set(key, { value, ...context });
  }

  addTranslations(locale, translations) {
    for (const [key, value] of Object.entries(translations)) {
      if (typeof value === 'string') {
        this.addTranslation(locale, key, value);
      } else {
        this.addTranslation(locale, key, value.value, value);
      }
    }
  }

  t(key, params = {}, options = {}) {
    const { locale = this.currentLocale, fallback = key } = options;
    let translation = this.findTranslation(locale, key);

    if (!translation && locale !== this.defaultLocale) {
      translation = this.findTranslation(this.defaultLocale, key);
    }

    if (!translation) {
      return fallback;
    }

    return this.interpolate(translation, params);
  }

  findTranslation(locale, key) {
    const localeTrans = this.translations.get(locale);
    if (localeTrans) {
      const trans = localeTrans.get(key);
      if (trans) return trans.value;
    }

    const fallbackLocale = this.fallbacks.get(locale);
    if (fallbackLocale) {
      return this.findTranslation(fallbackLocale, key);
    }

    return null;
  }

  interpolate(template, params) {
    return template.replace(/\{(\w+)\}/g, (match, key) => {
      return params[key] !== undefined ? params[key] : match;
    });
  }

  setFallback(fromLocale, toLocale) {
    this.fallbacks.set(fromLocale, toLocale);
  }

  getTranslations(locale = this.currentLocale) {
    return this.translations.get(locale) || new Map();
  }

  initPluralRule(locale) {
    try {
      this.pluralRules.set(locale, new Intl.PluralRules(locale));
    } catch (e) {
      this.pluralRules.set(locale, new Intl.PluralRules('en'));
    }
  }

  plural(count, options) {
    const rule = this.pluralRules.get(this.currentLocale);
    if (!rule) return options.other || options.one;

    const pluralForm = rule.select(count);
    return options[pluralForm] || options.other;
  }
}

class DateTimeFormatter {
  constructor(locale = 'en') {
    this.locale = locale;
    this.formatters = {
      full: new Intl.DateTimeFormat(locale, {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      }),
      long: new Intl.DateTimeFormat(locale, {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
      }),
      medium: new Intl.DateTimeFormat(locale, {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
      }),
      short: new Intl.DateTimeFormat(locale, {
        month: 'short',
        day: 'numeric',
      }),
      time: new Intl.DateTimeFormat(locale, {
        hour: '2-digit',
        minute: '2-digit',
      }),
    };
  }

  format(date, style = 'medium') {
    const formatter = this.formatters[style];
    if (!formatter) return String(date);
    return formatter.format(date);
  }

  formatRange(start, end, style = 'medium') {
    const formatter = this.formatters[style];
    if (!formatter) return `${start} - ${end}`;
    return formatter.formatRange(start, end);
  }

  formatRelative(date, baseDate = new Date()) {
    const diff = baseDate.getTime() - date.getTime();
    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (seconds < 60) return 'just now';
    if (minutes < 60) return `${minutes}m ago`;
    if (hours < 24) return `${hours}h ago`;
    if (days < 7) return `${days}d ago`;
    return this.format(date, 'short');
  }

  setLocale(locale) {
    this.locale = locale;
    this.initFormatters();
  }
}

class NumberFormatter {
  constructor(locale = 'en', options = {}) {
    this.locale = locale;
    this.options = options;
    this.initFormatters();
  }

  initFormatters() {
    this.formatters = {
      decimal: new Intl.NumberFormat(this.locale, {
        style: 'decimal',
        ...this.options,
      }),
      currency: new Intl.NumberFormat(this.locale, {
        style: 'currency',
        currency: this.options.currency || 'USD',
        ...this.options,
      }),
      percent: new Intl.NumberFormat(this.locale, {
        style: 'percent',
        ...this.options,
      }),
      unit: new Intl.NumberFormat(this.locale, {
        style: 'unit',
        unit: this.options.unit || 'byte',
        ...this.options,
      }),
    };
  }

  format(number, style = 'decimal') {
    const formatter = this.formatters[style];
    if (!formatter) return String(number);
    return formatter.format(number);
  }

  parse(value) {
    const formatter = new Intl.NumberFormat(this.locale);
    return formatter.formatToParts(value).map(p => p.value).join('');
  }

  setCurrency(currency) {
    this.options.currency = currency;
    this.initFormatters();
  }

  setLocale(locale) {
    this.locale = locale;
    this.initFormatters();
  }
}

class RTLManager {
  constructor() {
    this.rtlLocales = new Set([
      'ar', 'he', 'fa', 'ur', 'yi', 'ps', 'sd', 'ku', 'azb', 'haz',
    ]);
    this.currentLocale = 'en';
  }

  isRTL(locale = this.currentLocale) {
    return this.rtlLocales.has(locale);
  }

  addRTLLocale(locale) {
    this.rtlLocales.add(locale);
  }

  setLocale(locale) {
    this.currentLocale = locale;
  }

  getDirection(locale = this.currentLocale) {
    return this.isRTL(locale) ? 'rtl' : 'ltr';
  }

  applyDirection(element, locale = this.currentLocale) {
    const dir = this.getDirection(locale);
    element.setAttribute('dir', dir);
    element.style.direction = dir;
    element.style.textAlign = dir === 'rtl' ? 'right' : 'left';
  }
}

class LocaleDetector {
  constructor() {
    this.supportedLocales = new Set(['en']);
    this.storageKey = 'app-locale';
  }

  detect() {
    const stored = this.getStoredLocale();
    if (stored && this.isSupported(stored)) {
      return stored;
    }

    const browserLocale = this.getBrowserLocale();
    if (this.isSupported(browserLocale)) {
      return browserLocale;
    }

    const base = browserLocale.split('-')[0];
    if (this.isSupported(base)) {
      return base;
    }

    return 'en';
  }

  getBrowserLocale() {
    return navigator.language || navigator.userLanguage || 'en';
  }

  getStoredLocale() {
    try {
      return localStorage.getItem(this.storageKey);
    } catch (e) {
      return null;
    }
  }

  storeLocale(locale) {
    try {
      localStorage.setItem(this.storageKey, locale);
    } catch (e) {
      console.warn('Could not store locale:', e);
    }
  }

  setSupportedLocales(locales) {
    this.supportedLocales.clear();
    locales.forEach(l => this.supportedLocales.add(l));
  }

  isSupported(locale) {
    if (this.supportedLocales.has(locale)) return true;
    const base = locale.split('-')[0];
    return this.supportedLocales.has(base);
  }
}

const i18nManager = new I18nManager();
const dateFormatter = new DateTimeFormatter();
const numberFormatter = new NumberFormatter();
const rtlManager = new RTLManager();
const localeDetector = new LocaleDetector();

export { I18nManager, DateTimeFormatter, NumberFormatter, RTLManager, LocaleDetector };
export { i18nManager, dateFormatter, numberFormatter, rtlManager, localeDetector };

export default i18nManager;