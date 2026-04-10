/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_5_Internationalization-and-Localization.js';

describe('I18nManager', () => {
  let i18n;

  beforeEach(() => {
    i18n = new I18nManager('en');
  });

  it('should set and get locale', () => {
    i18n.setLocale('es');
    expect(i18n.getLocale()).to.equal('es');
  });

  it('should add translation', () => {
    i18n.addTranslation('en', 'greeting', 'Hello');
    const result = i18n.t('greeting');
    expect(result).to.equal('Hello');
  });

  it('should add multiple translations', () => {
    i18n.addTranslations('en', {
      'hello': 'Hello',
      'goodbye': 'Goodbye',
    });
    expect(i18n.t('hello')).to.equal('Hello');
    expect(i18n.t('goodbye')).to.equal('Goodbye');
  });

  it('should interpolate parameters', () => {
    i18n.addTranslation('en', 'greeting', 'Hello {name}');
    const result = i18n.t('greeting', { name: 'World' });
    expect(result).to.equal('Hello World');
  });

  it('should fallback to default locale', () => {
    i18n.addTranslation('en', 'test', 'English');
    i18n.setLocale('es');
    const result = i18n.t('test');
    expect(result).to.equal('English');
  });

  it('should use fallback locale', () => {
    i18n.addTranslation('en', 'test', 'English');
    i18n.setFallback('fr', 'en');
    i18n.setLocale('fr');
    const result = i18n.t('test');
    expect(result).to.equal('English');
  });

  it('should return fallback key when no translation', () => {
    const result = i18n.t('missing-key', {}, { fallback: 'Missing' });
    expect(result).to.equal('Missing');
  });

  it('should handle pluralization', () => {
    i18n.initPluralRule('en');
    const result = i18n.plural(1, { one: 'One item', other: '{count} items' });
    expect(result).to.equal('One item');
  });
});

describe('DateTimeFormatter', () => {
  let formatter;

  beforeEach(() => {
    formatter = new DateTimeFormatter('en');
  });

  it('should format date with different styles', () => {
    const date = new Date('2024-01-15');
    const result = formatter.format(date, 'short');
    expect(result).to.include('Jan');
  });

  it('should format relative time', () => {
    const now = new Date();
    const past = new Date(now.getTime() - 60000);
    const result = formatter.formatRelative(past, now);
    expect(result).to.include('ago');
  });

  it('should set locale', () => {
    formatter.setLocale('de');
    expect(formatter.locale).to.equal('de');
  });
});

describe('NumberFormatter', () => {
  let formatter;

  beforeEach(() => {
    formatter = new NumberFormatter('en');
  });

  it('should format decimal', () => {
    const result = formatter.format(1234.56, 'decimal');
    expect(result).to.include('1');
  });

  it('should format currency', () => {
    const result = formatter.format(99.99, 'currency');
    expect(result).to.include('$');
  });

  it('should format percent', () => {
    const result = formatter.format(0.5, 'percent');
    expect(result).to.include('50');
  });

  it('should set currency', () => {
    formatter.setCurrency('EUR');
    expect(formatter.options.currency).to.equal('EUR');
  });
});

describe('RTLManager', () => {
  let rtl;

  beforeEach(() => {
    rtl = new RTLManager();
  });

  it('should detect RTL locales', () => {
    expect(rtl.isRTL('ar')).to.be.true;
    expect(rtl.isRTL('he')).to.be.true;
  });

  it('should detect non-RTL locales', () => {
    expect(rtl.isRTL('en')).to.be.false;
    expect(rtl.isRTL('fr')).to.be.false;
  });

  it('should add RTL locale', () => {
    rtl.addRTLLocale('xx');
    expect(rtl.isRTL('xx')).to.be.true;
  });

  it('should get direction', () => {
    expect(rtl.getDirection('ar')).to.equal('rtl');
    expect(rtl.getDirection('en')).to.equal('ltr');
  });
});

describe('LocaleDetector', () => {
  let detector;

  beforeEach(() => {
    detector = new LocaleDetector();
  });

  it('should detect stored locale', () => {
    detector.storeLocale('de');
    const detected = detector.detect();
    expect(detected).to.equal('de');
  });

  it('should check supported locales', () => {
    detector.setSupportedLocales(['en', 'de', 'fr']);
    expect(detector.isSupported('de')).to.be.true;
    expect(detector.isSupported('es')).to.be.false;
  });

  it('should support base locale', () => {
    detector.setSupportedLocales(['en']);
    expect(detector.isSupported('en-US')).to.be.true;
  });
});

describe('Global Exports', () => {
  it('should export i18nManager', () => {
    expect(i18nManager).to.exist;
  });

  it('should export dateFormatter', () => {
    expect(dateFormatter).to.exist;
  });

  it('should export numberFormatter', () => {
    expect(numberFormatter).to.exist;
  });

  it('should export rtlManager', () => {
    expect(rtlManager).to.exist;
  });

  it('should export localeDetector', () => {
    expect(localeDetector).to.exist;
  });
});