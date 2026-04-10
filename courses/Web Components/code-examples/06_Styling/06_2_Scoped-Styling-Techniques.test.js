import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_2_Scoped-Styling-Techniques.js';

describe('06_2_Scoped-Styling-Techniques', () => {
  describe('ScopedPanel', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<scoped-panel></scoped-panel>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders panel structure', () => {
      const panel = el.shadowRoot.querySelector('.panel');
      expect(panel).to.exist;
    });

    it('has default collapsed state', () => {
      expect(el.collapsed).to.be.false;
    });

    it('toggles collapsed state', async () => {
      el.collapsed = true;
      await el.updateComplete;
      expect(el.collapsed).to.be.true;
      expect(el.hasAttribute('collapsed')).to.be.true;
    });

    it('applies variant attribute', async () => {
      el.setAttribute('variant', 'primary');
      await el.updateComplete;
      expect(el.getAttribute('variant')).to.equal('primary');
    });

    it('applies no-padding attribute', async () => {
      el.setAttribute('no-padding', '');
      await el.updateComplete;
      expect(el.hasAttribute('no-padding')).to.be.true;
    });

    it('dispatches panel-collapse-change event', async () => {
      const listener = oneEvent(el, 'panel-collapse-change');
      el.collapsed = true;
      const event = await listener;
      expect(event.detail.collapsed).to.be.true;
    });

    it('dispatches panel-variant-change event', async () => {
      const listener = oneEvent(el, 'panel-variant-change');
      el.setAttribute('variant', 'danger');
      const event = await listener;
      expect(event.detail.variant).to.equal('danger');
    });

    it('calls toggle method', async () => {
      const result = el.toggle();
      expect(result).to.be.true;
      expect(el.collapsed).to.be.true;
    });

    it('calls expand method', async () => {
      el.collapsed = true;
      el.expand();
      expect(el.collapsed).to.be.false;
    });

    it('calls collapse method', async () => {
      el.collapse();
      expect(el.collapsed).to.be.true;
    });

    it('has form-associated support', () => {
      expect(el.form).to.be.null;
    });

    it('has accessibility attributes', () => {
      const toggle = el.shadowRoot.querySelector('.panel-toggle');
      expect(toggle).to.have.attribute('aria-expanded');
    });

    it('handles multiple variants', async () => {
      el.setAttribute('variant', 'success');
      await el.updateComplete;
      expect(el.variant).to.equal('success');

      el.setAttribute('variant', 'outline');
      await el.updateComplete;
      expect(el.variant).to.equal('outline');
    });

    it('handles toggle via keyboard', async () => {
      const toggle = el.shadowRoot.querySelector('.panel-toggle');
      toggle.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter' }));
      expect(el.collapsed).to.be.true;
    });

    it('applies role attribute', () => {
      expect(el).to.have.attribute('role');
      expect(el.getAttribute('role')).to.equal('region');
    });
  });

  describe('ScopedButton', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<scoped-button>Click me</scoped-button>`);
    });

    it('renders button with slot content', () => {
      const button = el.shadowRoot.querySelector('.button');
      expect(button).to.exist;
      expect(button.textContent).to.equal('Click me');
    });

    it('has default variant', () => {
      expect(el.variant).to.equal('primary');
    });

    it('applies variant attribute', async () => {
      el.setAttribute('variant', 'success');
      await el.updateComplete;
      expect(el.variant).to.equal('success');
    });

    it('applies size attribute', async () => {
      el.setAttribute('size', 'lg');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('lg');
    });

    it('applies full-width attribute', async () => {
      el.setAttribute('full-width', '');
      await el.updateComplete;
      expect(el.hasAttribute('full-width')).to.be.true;
    });

    it('handles disabled state', async () => {
      el.disabled = true;
      await el.updateComplete;
      expect(el.disabled).to.be.true;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('dispatches button-click event', async () => {
      const listener = oneEvent(el, 'button-click');
      el.click();
      const event = await listener;
      expect(event.detail).to.have.property('originalEvent');
    });

    it('dispatches button-focus and button-blur events', async () => {
      const focusListener = oneEvent(el, 'button-focus');
      el.focus();
      await focusListener;
      
      const blurListener = oneEvent(el, 'button-blur');
      el.blur();
      await blurListener;
    });

    it('handles keyboard activation', async () => {
      const listener = oneEvent(el, 'button-click');
      el.dispatchEvent(new KeyboardEvent('keydown', { key: ' ' }));
      await listener;
    });

    it('prevents click when disabled', async () => {
      el.disabled = true;
      let clicked = false;
      el.addEventListener('button-click', () => { clicked = true; });
      el.click();
      expect(clicked).to.be.false;
    });

    it('has proper role and tabindex', () => {
      expect(el).to.have.attribute('role');
      expect(el.getAttribute('role')).to.equal('button');
      expect(el).to.have.attribute('tabindex');
    });

    it('supports multiple variants', async () => {
      const variants = ['secondary', 'danger', 'warning', 'outline-primary', 'ghost'];
      for (const variant of variants) {
        el.setAttribute('variant', variant);
        await el.updateComplete;
        expect(el.variant).to.equal(variant);
      }
    });

    it('handles focus and blur methods', () => {
      el.focus();
      expect(document.activeElement).to.equal(el);
      el.blur();
    });
  });

  describe('StyleEncapsulationHelper', () => {
    it('creates scoped styles', () => {
      const sheet = StyleEncapsulationHelper.createScopedStyles(':host { color: red; }', ':host');
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
    });

    it('creates layer styles', () => {
      const sheet = StyleEncapsulationHelper.createLayerStyles('.class { color: blue; }', 'test');
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
    });

    it('creates styles with media queries', () => {
      const sheet = StyleEncapsulationHelper.createWithMediaQueries({
        '(min-width: 768px)': '.class { font-size: 16px; }'
      });
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
    });

    it('injects global styles', () => {
      StyleEncapsulationHelper.injectGlobalStyle('.test { color: green; }', 'test-style-id');
      const style = document.getElementById('test-style-id');
      expect(style).to.exist;
      style.remove();
    });

    it('removes global styles', () => {
      StyleEncapsulationHelper.injectGlobalStyle('.test { color: green; }', 'remove-test-style');
      StyleEncapsulationHelper.removeGlobalStyle('remove-test-style');
      const style = document.getElementById('remove-test-style');
      expect(style).to.be.null;
    });
  });

  describe('Edge Cases', () => {
    it('handles panel without content', async () => {
      const el = await fixture(html`<scoped-panel></scoped-panel>`);
      const content = el.shadowRoot.querySelector('.panel-content');
      expect(content).to.exist;
    });

    it('handles button with empty content', async () => {
      const el = await fixture(html`<scoped-button></scoped-button>`);
      const button = el.shadowRoot.querySelector('.button');
      expect(button.textContent).to.equal('');
    });

    it('handles rapid toggle operations', async () => {
      const el = await fixture(html`<scoped-panel></scoped-panel>`);
      el.toggle();
      el.toggle();
      el.toggle();
      expect(el.collapsed).to.be.true;
    });

    it('handles variant change after collapsed', async () => {
      const el = await fixture(html`<scoped-panel collapsed></scoped-panel>`);
      el.setAttribute('variant', 'danger');
      expect(el.collapsed).to.be.true;
    });

    it('handles null variant value', async () => {
      const el = await fixture(html`<scoped-button></scoped-button>`);
      el.variant = null;
      expect(el.variant).to.equal('default');
    });
  });
});