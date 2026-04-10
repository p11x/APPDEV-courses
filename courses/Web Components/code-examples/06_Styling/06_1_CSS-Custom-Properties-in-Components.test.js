import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_1_CSS-Custom-Properties-in-Components.js';

describe('06_1_CSS-Custom-Properties-in-Components', () => {
  describe('ThemeableCard', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<themeable-card></themeable-card>`);
    });

    it('renders with default theme', () => {
      expect(el.theme).to.equal('default');
      expect(el.hasAttribute('theme')).to.be.false;
    });

    it('applies theme attribute', async () => {
      el.theme = 'dark';
      await el.updateComplete;
      expect(el.theme).to.equal('dark');
    });

    it('sets CSS custom properties via JavaScript API', async () => {
      el.setCSSCustomProperty('--card-bg', '#ff0000');
      const computed = getComputedStyle(el).getPropertyValue('--card-bg');
      expect(computed).to.include('#ff0000');
    });

    it('removes CSS custom properties', async () => {
      el.setCSSCustomProperty('--card-bg', '#ff0000');
      el.removeCSSCustomProperty('--card-bg');
      expect(el.getCSSCustomProperty('--card-bg')).to.equal('');
    });

    it('retrieves all CSS custom properties', async () => {
      el.style.setProperty('--test-prop', 'test-value');
      const props = el.getAllCSSCustomProperties();
      expect(props).to.be.an('object');
    });

    it('handles disabled state', async () => {
      el.disabled = true;
      await el.updateComplete;
      expect(el.disabled).to.be.true;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('dispatches theme-change event', async () => {
      const listener = oneEvent(el, 'theme-change');
      el.theme = 'ocean';
      const event = await listener;
      expect(event.detail.oldTheme).to.equal('default');
      expect(event.detail.newTheme).to.equal('ocean');
    });

    it('dispatches card-action event on button click', async () => {
      const listener = oneEvent(el, 'card-action');
      const button = el.shadowRoot.querySelector('#primary-action');
      button.click();
      const event = await listener;
      expect(event.detail).to.have.property('originalEvent');
    });

    it('respects no-hover attribute', async () => {
      el.setAttribute('no-hover', '');
      expect(el.hasAttribute('no-hover')).to.be.true;
      el.removeAttribute('no-hover');
      expect(el.hasAttribute('no-hover')).to.be.false;
    });

    it('animates CSS properties', async () => {
      const animation = el.animateCSSProperty('--card-bg', '#fff', '#000', 200);
      expect(animation).to.be.instanceOf(Promise);
    });

    it('supports form-associated behavior', () => {
      expect(el.form).to.be.null;
      expect(el.checkValidity()).to.be.true;
    });

    it('handles null theme value', async () => {
      el.theme = 'invalid-theme';
      expect(el.theme).to.equal('default');
    });

    it('handles invalid themes gracefully', async () => {
      el.setAttribute('theme', 'nonexistent');
      expect(el.theme).to.equal('default');
    });

    it('supports cardBackground getter/setter', async () => {
      el.cardBackground = '#cccccc';
      expect(el.cardBackground).to.equal('#cccccc');
    });

    it('supports cardAccent getter/setter', async () => {
      el.cardAccent = '#ff00ff';
      expect(el.cardAccent).to.equal('#ff00ff');
    });

    it('respects observedThemes static getter', () => {
      expect(ThemeableCard.observedThemes).to.include('dark');
      expect(ThemeableCard.observedThemes).to.include('ocean');
    });
  });

  describe('CSSPropertyWatcher', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<css-property-watcher property="--test"></css-property-watcher>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('accepts property attribute', () => {
      expect(el.getAttribute('property')).to.equal('--test');
    });

    it('accepts callback attribute', async () => {
      window.testCallback = () => {};
      el.setAttribute('callback', 'testCallback');
      expect(el.getAttribute('callback')).to.equal('testCallback');
      delete window.testCallback;
    });
  });

  describe('CustomPropertyProvider', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<custom-property-provider properties="--color: red;"></custom-property-provider>`);
    });

    it('parses properties from attribute', () => {
      expect(el.getProperty('--color')).to.equal('red');
    });

    it('sets properties programmatically', async () => {
      el.setProperty('--new-prop', 'blue');
      expect(el.getProperty('--new-prop')).to.equal('blue');
    });

    it('removes properties', async () => {
      el.setProperty('--to-remove', 'value');
      el.removeProperty('--to-remove');
      expect(el.getProperty('--to-remove')).to.be.undefined;
    });

    it('updates when properties attribute changes', async () => {
      el.setAttribute('properties', '--color: blue; --size: 10px;');
      expect(el.getProperty('--color')).to.equal('blue');
      expect(el.getProperty('--size')).to.equal('10px');
    });

    it('uses custom selector', async () => {
      const provider = await fixture(html`<custom-property-provider selector=".custom" properties="--test: value;"></custom-property-provider>`);
      expect(provider).to.exist;
    });
  });

  describe('Edge Cases', () => {
    it('handles component without shadow root gracefully', () => {
      const element = document.createElement('themeable-card');
      expect(element.theme).to.equal('default');
    });

    it('handles rapid theme changes', async () => {
      const el = await fixture(html`<themeable-card></themeable-card>`);
      el.theme = 'dark';
      el.theme = 'ocean';
      el.theme = 'forest';
      await el.updateComplete;
      expect(el.theme).to.equal('forest');
    });

    it('handles empty custom property values', async () => {
      const el = await fixture(html`<themeable-card></themeable-card>`);
      el.setCSSCustomProperty('--empty', '');
      expect(el.getCSSCustomProperty('--empty')).to.equal('');
    });

    it('handles CSS property name without prefix', async () => {
      const el = await fixture(html`<themeable-card></themeable-card>`);
      el.setCSSCustomProperty('card-bg', '#fff');
      expect(el.cardBackground).to.include('#fff');
    });

    it('handles animation with invalid property', async () => {
      const el = await fixture(html`<themeable-card></themeable-card>`);
      const animation = el.animateCSSProperty('invalid', 'value1', 'value2');
      expect(animation).to.be.instanceOf(Promise);
    });
  });
});