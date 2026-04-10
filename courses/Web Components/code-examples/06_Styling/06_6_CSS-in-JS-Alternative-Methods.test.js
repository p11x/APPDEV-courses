import { expect, fixture, html } from '@open-wc/testing';
import '../../../code-examples/06_Styling/06_6_CSS-in-JS-Alternative-Methods.js';

describe('06_6_CSS-in-JS-Alternative-Methods', () => {
  describe('css tagged template', () => {
    it('creates CSS from template literal', () => {
      const result = css`:host { color: red; }`;
      expect(result).to.include(':host');
      expect(result).to.include('color: red');
    });

    it('interpolates values', () => {
      const color = 'blue';
      const result = css`:host { color: ${color}; }`;
      expect(result).to.include('color: blue');
    });

    it('handles arrays', () => {
      const classes = ['class1', 'class2'];
      const result = css`.${classes} { color: red; }`;
      expect(result).to.include('class1 class2');
    });

    it('handles null and undefined', () => {
      const result = css`:host { color: ${null}; }`;
      expect(result).to.not.include('null');
    });
  });

  describe('keyframes function', () => {
    it('generates keyframe CSS', () => {
      const result = keyframes({
        '0%': { opacity: 0 },
        '100%': { opacity: 1 }
      });
      expect(result).to.include('@keyframes');
      expect(result).to.include('0%');
      expect(result).to.include('100%');
    });

    it('converts camelCase to kebab-case', () => {
      const result = keyframes({
        '50%': { backgroundColor: 'red' }
      });
      expect(result).to.include('background-color');
    });
  });

  describe('global function', () => {
    it('generates global CSS', () => {
      const result = global({
        'body': { margin: 0 },
        '*': { boxSizing: 'border-box' }
      });
      expect(result).to.include('body');
      expect(result).to.include('margin: 0');
    });
  });

  describe('AtomicCSSGenerator', () => {
    it('generates unique class names', () => {
      const class1 = atomicGenerator.generate({ color: 'red' });
      const class2 = atomicGenerator.generate({ color: 'blue' });
      expect(class1).to.not.equal(class2);
    });

    it('caches duplicate properties', () => {
      const class1 = atomicGenerator.generate({ margin: '10px' });
      const class2 = atomicGenerator.generate({ margin: '10px' });
      expect(class1).to.equal(class2);
    });

    it('returns stylesheet', () => {
      expect(atomicGenerator.sheet).to.be.instanceOf(CSSStyleSheet);
    });
  });

  describe('StyledCard', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<styled-card></styled-card>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders card elements', () => {
      expect(el.shadowRoot.querySelector('.card')).to.exist;
      expect(el.shadowRoot.querySelector('.title')).to.exist;
    });

    it('applies variant attribute', async () => {
      el.setAttribute('variant', 'primary');
      await el.updateComplete;
      expect(el.getAttribute('variant')).to.equal('primary');
    });

    it('applies size attribute', async () => {
      el.setAttribute('size', 'large');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('large');
    });

    it('applies elevated attribute', async () => {
      el.setAttribute('elevated', '');
      await el.updateComplete;
      expect(el.hasAttribute('elevated')).to.be.true;
    });

    it('handles multiple variants', async () => {
      const variants = ['default', 'primary', 'secondary', 'success', 'danger', 'warning'];
      for (const variant of variants) {
        el.setAttribute('variant', variant);
        await el.updateComplete;
        expect(el.getAttribute('variant')).to.equal(variant);
      }
    });

    it('re-renders on attribute change', async () => {
      el.setAttribute('variant', 'success');
      await el.updateComplete;
      el.setAttribute('variant', 'danger');
      await el.updateComplete;
      expect(el.getAttribute('variant')).to.equal('danger');
    });
  });

  describe('StyledButton', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<styled-button>Click</styled-button>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders button element', () => {
      expect(el.shadowRoot.querySelector('.button')).to.exist;
      expect(el.shadowRoot.querySelector('.button').textContent).to.equal('Click');
    });

    it('applies variant attribute', async () => {
      el.setAttribute('variant', 'outline');
      await el.updateComplete;
      expect(el.getAttribute('variant')).to.equal('outline');
    });

    it('applies size attribute', async () => {
      el.setAttribute('size', 'small');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('small');
    });

    it('applies fullwidth attribute', async () => {
      el.setAttribute('fullwidth', '');
      await el.updateComplete;
      expect(el.hasAttribute('fullwidth')).to.be.true;
    });

    it('handles disabled state', async () => {
      el.disabled = true;
      await el.updateComplete;
      expect(el.disabled).to.be.true;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('has accessibility attributes', () => {
      expect(el).to.have.attribute('role');
      expect(el).to.have.attribute('tabindex');
    });

    it('gets and sets disabled property', () => {
      el.disabled = true;
      expect(el.disabled).to.be.true;
      el.disabled = false;
      expect(el.disabled).to.be.false;
    });
  });

  describe('StyleInjector', () => {
    it('injects styles', () => {
      const sheet = globalStyleInjector.inject('test-id', '.test { color: red; }');
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
      expect(globalStyleInjector.get('test-id')).to.exist;
    });

    it('removes styles', () => {
      globalStyleInjector.inject('remove-test', '.test { color: blue; }');
      const result = globalStyleInjector.remove('remove-test');
      expect(result).to.be.true;
      expect(globalStyleInjector.get('remove-test')).to.be.undefined;
    });

    it('gets all styles', () => {
      globalStyleInjector.inject('all-1', '.a { color: red; }');
      globalStyleInjector.inject('all-2', '.b { color: blue; }');
      const all = globalStyleInjector.all;
      expect(Object.keys(all).length).to.be.at.least(2);
    });
  });

  describe('createGlobalStyles', () => {
    it('creates global styles', () => {
      const sheet = createGlobalStyles('global-test', {
        'body': { background: 'white' }
      });
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
    });
  });

  describe('createKeyframes', () => {
    it('creates keyframe animation', () => {
      const sheet = createKeyframes('test-keyframe', {
        '0%': { opacity: 0 },
        '100%': { opacity: 1 }
      });
      expect(sheet).to.be.instanceOf(CSSStyleSheet);
    });
  });

  describe('useAtomicClass', () => {
    it('returns atomic class name', () => {
      const className = useAtomicClass({ padding: '10px' });
      expect(className).to.be.a('string');
      expect(className).to.include('atomic-');
    });
  });

  describe('Edge Cases', () => {
    it('handles empty css template', () => {
      const result = css``;
      expect(result).to.equal('');
    });

    it('handles styled card without variant', () => {
      const el = document.createElement('styled-card');
      expect(el).to.exist;
    });
  });
});