import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_4_Dynamic-Styling-Methods.js';

describe('06_4_Dynamic-Styling-Methods', () => {
  describe('DynamicStylist', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<dynamic-stylist></dynamic-stylist>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders control buttons', () => {
      expect(el.shadowRoot.querySelector('#increase-size')).to.exist;
      expect(el.shadowRoot.querySelector('#decrease-size')).to.exist;
      expect(el.shadowRoot.querySelector('#toggle-mode')).to.exist;
      expect(el.shadowRoot.querySelector('#reset-styles')).to.exist;
    });

    it('applies mode attribute', async () => {
      el.setAttribute('mode', 'dark');
      await el.updateComplete;
      expect(el.getAttribute('mode')).to.equal('dark');
    });

    it('applies scale attribute', async () => {
      el.setAttribute('scale', '1.5');
      await el.updateComplete;
      expect(el.getAttribute('scale')).to.equal('1.5');
    });

    it('applies debug attribute', async () => {
      el.setAttribute('debug', '');
      await el.updateComplete;
      expect(el.hasAttribute('debug')).to.be.true;
    });

    it('sets and gets properties', async () => {
      el.setProperty('--custom-prop', 'blue');
      expect(el.getProperty('--custom-prop')).to.equal('blue');
    });

    it('sets property without prefix', async () => {
      el.setProperty('custom-var', 'red');
      expect(el.getProperty('custom-var')).to.equal('red');
    });

    it('removes properties', async () => {
      el.setProperty('--test-var', 'value');
      el.removeProperty('--test-var');
      expect(el.getProperty('--test-var')).to.equal('');
    });

    it('adds custom rules', async () => {
      el.addRule('.custom-class', { background: 'red', color: 'white' });
      expect(el.allRules).to.have.property('.custom-class');
    });

    it('removes rules', async () => {
      el.addRule('.temp-class', { background: 'blue' });
      el.removeRule('.temp-class');
      expect(el.allRules).not.to.have.property('.temp-class');
    });

    it('updates existing rules', async () => {
      el.addRule('.test-rule', { color: 'black' });
      el.updateRule('.test-rule', 'color', 'white');
      expect(el.allRules['.test-rule'].color).to.equal('white');
    });

    it('handles scale increase button', () => {
      const btn = el.shadowRoot.querySelector('#increase-size');
      btn.click();
    });

    it('handles scale decrease button', () => {
      const btn = el.shadowRoot.querySelector('#decrease-size');
      btn.click();
    });

    it('handles mode toggle button', () => {
      const btn = el.shadowRoot.querySelector('#toggle-mode');
      btn.click();
    });

    it('handles reset button', () => {
      const btn = el.shadowRoot.querySelector('#reset-styles');
      btn.click();
      expect(el.getAttribute('mode')).to.equal('light');
    });

    it('returns all rules', () => {
      const rules = el.allRules;
      expect(rules).to.be.an('object');
    });
  });

  describe('RuntimeStyleInjector', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<runtime-style-injector></runtime-style-injector>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('accepts styles attribute', async () => {
      const injector = await fixture(html`<runtime-style-injector styles=".test { color: red; }"></runtime-style-injector>`);
      expect(injector.getInjectedStyles()).to.be.an('object');
    });

    it('injects CSS programmatically', async () => {
      const id = el.injectCSS('.injected { font-size: 14px; }', 'test-style-id');
      expect(id).to.exist;
      expect(el.shadowRoot.querySelector('#test-style-id')).to.exist;
    });

    injects with priority', async () => {
      el.setAttribute('priority', 'important');
      el.setAttribute('styles', '.priority { color: blue !important; }');
      await el.updateComplete;
      const styles = el.getInjectedStyles();
      expect(Object.values(styles)[0]).to.include('!important');
    });

    it('removes injected styles', async () => {
      el.injectCSS('.remove { display: none; }', 'remove-me');
      el.removeStyle('remove-me');
      expect(el.shadowRoot.querySelector('#remove-me')).to.not.exist;
    });

    it('returns all injected styles', async () => {
      el.injectCSS('.style1 { color: red; }', 'style1');
      el.injectCSS('.style2 { color: blue; }', 'style2');
      const styles = el.getInjectedStyles();
      expect(Object.keys(styles).length).to.equal(2);
    });
  });

  describe('InteractiveStyleModifier', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<interactive-style-modifier>Content</interactive-style-modifier>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('transitions to new styles', async () => {
      const transition = el.transitionTo({ backgroundColor: 'blue', color: 'white' }, 100);
      expect(transition).to.be.instanceOf(Promise);
      await transition;
    });

    it('animates keyframes', async () => {
      const keyframes = [{ opacity: 0 }, { opacity: 1 }];
      const animation = el.animateKeyframes(keyframes, { duration: 100 });
      expect(animation).to.be.instanceOf(Promise);
    });

    it('sets custom transitions', () => {
      el.setTransition('opacity', 200, 'ease-in-out');
    });

    it('clears transitions', () => {
      el.setTransition('opacity', 100);
      el.clearTransitions();
    });

    it('handles animate attribute', async () => {
      el.setAttribute('animate', 'true');
      await el.updateComplete;
      expect(el.hasAttribute('animate')).to.be.true;
    });

    it('handles duration attribute', async () => {
      el.setAttribute('duration', '500');
      await el.updateComplete;
      expect(el.getAttribute('duration')).to.equal('500');
    });

    it('handles easing attribute', async () => {
      el.setAttribute('easing', 'linear');
      await el.updateComplete;
      expect(el.getAttribute('easing')).to.equal('linear');
    });
  });

  describe('StyleCalculator', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<style-calculator>Test content</style-calculator>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('calculates layout metrics', () => {
      const metrics = el.calculateLayoutMetrics();
      expect(metrics).to.have.property('width');
      expect(metrics).to.have.property('height');
      expect(metrics).to.have.property('offsetWidth');
      expect(metrics).to.have.property('offsetHeight');
    });

    it('resolves CSS values', () => {
      const resolved = el.resolveCSSValue('color', 'red');
      expect(resolved).to.be.a('string');
    });
  });

  describe('Edge Cases', () => {
    it('handles null property value', async () => {
      const el = await fixture(html`<dynamic-stylist></dynamic-stylist>`);
      el.setProperty('--null-test', 'value');
      el.setProperty('--null-test', null);
      expect(el.getProperty('--null-test')).to.equal('');
    });

    it('handles empty rule selector', async () => {
      const el = await fixture(html`<dynamic-stylist></dynamic-stylist>`);
      el.updateRule('', 'color', 'black');
    });
  });
});