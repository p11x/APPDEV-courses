import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_3_Theme-Integration-Patterns.js';

describe('06_3_Theme-Integration-Patterns', () => {
  describe('ThemeProvider', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<theme-provider theme="light"></theme-provider>`);
    });

    it('renders and sets default theme', () => {
      expect(el.theme).to.equal('light');
    });

    it('applies theme to document', async () => {
      el.theme = 'dark';
      await el.updateComplete;
      expect(document.documentElement).to.have.attribute('data-theme');
    });

    it('persists theme to localStorage', async () => {
      el.setAttribute('persist', '');
      el.theme = 'ocean';
      await el.updateComplete;
      expect(localStorage.getItem('theme-provider-data')).to.equal('ocean');
    });

    it('respects system preference', async () => {
      el.setAttribute('respect-preference', '');
      expect(el.theme).to.be.oneOf(['light', 'dark']);
    });

    it('registers new theme', async () => {
      el.registerTheme('custom', { '--theme-bg': '#000000', '--theme-text': '#ffffff' });
      expect(el.availableThemes).to.include('custom');
    });

    it('unregisters custom theme', async () => {
      el.registerTheme('custom', { '--theme-bg': '#000000' });
      const result = el.unregisterTheme('custom');
      expect(result).to.be.true;
      expect(el.availableThemes).to.not.include('custom');
    });

    it('cannot unregister built-in themes', () => {
      const result = el.unregisterTheme('light');
      expect(result).to.be.false;
    });

    it('gets theme variables', async () => {
      const vars = el.getThemeVariables('dark');
      expect(vars).to.have.property('--theme-bg');
    });

    it('applies theme variables directly', async () => {
      el.applyThemeVariables({ '--custom-var': 'custom-value' });
      expect(document.documentElement.style.getPropertyValue('--custom-var')).to.equal('custom-value');
    });

    it('resets to default theme', async () => {
      el.theme = 'dark';
      el.resetToDefault();
      expect(el.theme).to.equal('light');
    });

    it('validates theme name', () => {
      expect(el.isValidTheme('dark')).to.be.true;
      expect(el.isValidTheme('invalid')).to.be.false;
    });

    it('dispatches theme-change event', async () => {
      const listener = oneEvent(el, 'theme-change');
      el.theme = 'forest';
      const event = await listener;
      expect(event.detail.theme).to.equal('forest');
    });

    it('dispatches theme-registered event', async () => {
      const listener = oneEvent(el, 'theme-registered');
      el.registerTheme('test-theme', { '--test': '#000' });
      const event = await listener;
      expect(event.detail.name).to.equal('test-theme');
    });

    it('handles invalid theme gracefully', async () => {
      el.theme = 'nonexistent';
      expect(el.theme).to.equal('light');
    });

    it('uses custom storage key', async () => {
      el.setAttribute('storage-key', 'my-theme-key');
      el.setAttribute('persist', '');
      el.theme = 'ocean';
      await el.updateComplete;
      expect(localStorage.getItem('my-theme-key')).to.equal('ocean');
    });

    it('returns available themes', () => {
      const themes = el.availableThemes;
      expect(themes).to.include('light');
      expect(themes).to.include('dark');
    });
  });

  describe('ThemeableComponent', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<themeable-component></themeable-component>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('contains title and content slots', () => {
      const title = el.shadowRoot.querySelector('.title');
      const content = el.shadowRoot.querySelector('.content');
      expect(title).to.exist;
      expect(content).to.exist;
    });

    it('accepts fallback variables', async () => {
      el.setFallbackVariables({ bg: '#cccccc', text: '#111111' });
      expect(el).to.exist;
    });

    it('dispatches theme update event on document theme change', async () => {
      document.documentElement.setAttribute('data-theme', 'dark');
      const listener = oneEvent(el, 'component-theme-update');
      await listener;
      document.documentElement.removeAttribute('data-theme');
    });
  });

  describe('ThemeSwitcher', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<theme-switcher themes="light,dark,ocean"></theme-switcher>`);
    });

    it('renders theme buttons', () => {
      const buttons = el.shadowRoot.querySelectorAll('.theme-btn');
      expect(buttons.length).to.equal(3);
    });

    it('applies themes attribute', async () => {
      expect(el.getAttribute('themes')).to.equal('light,dark,ocean');
    });

    it('switches theme on button click', async () => {
      const provider = await fixture(html`<theme-provider theme="light"></theme-provider>`);
      const switcher = await fixture(html`<theme-switcher themes="light,dark"></theme-switcher>`);
      const darkBtn = switcher.shadowRoot.querySelector('[data-theme="dark"]');
      darkBtn.click();
      expect(provider.theme).to.equal('dark');
    });

    it('highlights current theme', () => {
      const buttons = el.shadowRoot.querySelectorAll('.theme-btn');
      const selected = Array.from(buttons).find(btn => btn.hasAttribute('aria-selected'));
      expect(selected).to.exist;
    });
  });

  describe('Edge Cases', () => {
    it('handles theme registration with invalid variables', () => {
      const el = document.createElement('theme-provider');
      expect(() => el.registerTheme('test', null)).to.throw();
    });

    it('handles getting variables for invalid theme', () => {
      const el = document.createElement('theme-provider');
      expect(el.getThemeVariables('nonexistent')).to.be.null;
    });

    it('handles empty themes attribute', async () => {
      const el = await fixture(html`<theme-switcher themes=""></theme-switcher>`);
      const buttons = el.shadowRoot.querySelectorAll('.theme-btn');
      expect(buttons.length).to.equal(0);
    });
  });
});