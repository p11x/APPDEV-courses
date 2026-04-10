import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_5_Custom-Input-Element-Development.js';

describe('07_5_Custom-Input-Element-Development', () => {
  describe('CustomInput', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<custom-input label="Name" name="name"></custom-input>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders label', () => {
      const label = el.shadowRoot.querySelector('.label');
      expect(label.textContent).to.equal('Name');
    });

    it('renders input element', () => {
      const input = el.shadowRoot.querySelector('#input');
      expect(input).to.exist;
    });

    it('applies type attribute', async () => {
      el.setAttribute('type', 'email');
      await el.updateComplete;
      expect(el.getAttribute('type')).to.equal('email');
    });

    it('applies name attribute', () => {
      expect(el.getAttribute('name')).to.equal('name');
    });

    it('applies value attribute', async () => {
      el.setAttribute('value', 'test-value');
      await el.updateComplete;
      expect(el.value).to.equal('test-value');
    });

    it('applies placeholder attribute', async () => {
      el.setAttribute('placeholder', 'Enter name');
      await el.updateComplete;
      expect(el.shadowRoot.querySelector('#input').placeholder).to.equal('Enter name');
    });

    it('applies required attribute', async () => {
      el.setAttribute('required', '');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.required).to.be.true;
    });

    it('applies disabled attribute', async () => {
      el.setAttribute('disabled', '');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.disabled).to.be.true;
    });

    it('applies readonly attribute', async () => {
      el.setAttribute('readonly', '');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.readOnly).to.be.true;
    });

    it('applies min and max attributes', async () => {
      el.setAttribute('min', '0');
      el.setAttribute('max', '100');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.min).to.equal('0');
      expect(input.max).to.equal('100');
    });

    it('applies minlength and maxlength attributes', async () => {
      el.setAttribute('minlength', '3');
      el.setAttribute('maxlength', '10');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.minLength).to.equal(3);
      expect(input.maxLength).to.equal(10);
    });

    it('applies pattern attribute', async () => {
      el.setAttribute('pattern', '[A-Za-z]+');
      await el.updateComplete;
      expect(el.getAttribute('pattern')).to.equal('[A-Za-z]+');
    });

    it('applies prefix attribute', async () => {
      el.setAttribute('prefix', '$');
      await el.updateComplete;
      const prefix = el.shadowRoot.querySelector('#prefix');
      expect(prefix.textContent).to.equal('$');
      expect(prefix.hidden).to.be.false;
    });

    it('applies suffix attribute', async () => {
      el.setAttribute('suffix', 'px');
      await el.updateComplete;
      const suffix = el.shadowRoot.querySelector('#suffix');
      expect(suffix.textContent).to.equal('px');
      expect(suffix.hidden).to.be.false;
    });

    it('applies hint attribute', async () => {
      el.setAttribute('hint', 'Enter your name');
      await el.updateComplete;
      const hint = el.shadowRoot.querySelector('#hint');
      expect(hint.textContent).to.equal('Enter your name');
      expect(hint.hidden).to.be.false;
    });

    it('applies invalid attribute', async () => {
      el.setAttribute('invalid', 'Invalid value');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input).to.have.attribute('aria-invalid');
    });

    it('applies autocomplete attribute', async () => {
      el.setAttribute('autocomplete', 'name');
      await el.updateComplete;
      const input = el.shadowRoot.querySelector('#input');
      expect(input.autocomplete).to.equal('name');
    });

    it('gets and sets value', async () => {
      el.value = 'new-value';
      expect(el.value).to.equal('new-value');
    });

    it('gets validity', () => {
      const validity = el.validity;
      expect(validity).to.be.an('object');
    });

    it('gets validationMessage', () => {
      const message = el.validationMessage;
      expect(message).to.be.a('string');
    });

    it('checks validity', () => {
      const result = el.checkValidity();
      expect(result).to.be.a('boolean');
    });

    it('reports validity', () => {
      const result = el.reportValidity();
      expect(result).to.be.a('boolean');
    });

    it('focuses input', () => {
      el.focus();
      expect(document.activeElement).to.equal(el.shadowRoot.querySelector('#input'));
    });

    it('blurs input', () => {
      el.focus();
      el.blur();
    });

    it('resets input', async () => {
      el.value = 'test';
      el.reset();
      expect(el.value).to.equal('');
    });

    it('sets custom validity', async () => {
      el.setCustomValidity('Custom error');
      expect(el.validationMessage).to.equal('Custom error');
    });

    it('dispatches input-change event', async () => {
      const listener = oneEvent(el, 'input-change');
      el.value = 'changed';
      const event = await listener;
      expect(event.detail.value).to.equal('changed');
    });

    it('dispatches input-blur event', async () => {
      const input = el.shadowRoot.querySelector('#input');
      const listener = oneEvent(el, 'input-blur');
      input.dispatchEvent(new Event('blur'));
      const event = await listener;
      expect(event.detail).to.have.property('value');
    });

    it('dispatches input-focus event', async () => {
      const input = el.shadowRoot.querySelector('#input');
      const listener = oneEvent(el, 'input-focus');
      input.dispatchEvent(new Event('focus'));
      const event = await listener;
      expect(event.detail).to.have.property('value');
    });

    it('dispatches input-submit event on Enter', async () => {
      const input = el.shadowRoot.querySelector('#input');
      const listener = oneEvent(el, 'input-submit');
      input.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter' }));
      const event = await listener;
      expect(event.detail.value).to.equal('');
    });

    it('gets input element reference', () => {
      expect(el.inputElement).to.exist;
    });

    it('handles maxlength counter', async () => {
      el.setAttribute('maxlength', '10');
      await el.updateComplete;
      el.value = '12345678901';
      const counter = el.shadowRoot.querySelector('#counter');
      expect(counter.classList.contains('danger')).to.be.true;
    });
  });

  describe('CurrencyInput', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<currency-input label="Amount" name="amount"></currency-input>`);
    });

    it('renders with default currency prefix', () => {
      const prefix = el.shadowRoot.querySelector('#prefix');
      expect(prefix.textContent).to.equal('$');
    });

    it('applies currency attribute', async () => {
      el.setAttribute('currency', 'EUR');
      await el.updateComplete;
      const prefix = el.shadowRoot.querySelector('#prefix');
      expect(prefix.textContent).to.equal('€');
    });
  });

  describe('PercentageInput', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<percentage-input label="Rate" name="rate"></percentage-input>`);
    });

    it('renders with percentage suffix', () => {
      const suffix = el.shadowRoot.querySelector('#suffix');
      expect(suffix.textContent).to.equal('%');
    });

    it('constrains value between 0 and 100', async () => {
      el.value = '150';
      expect(el.value).to.equal('100');
    });
  });

  describe('Edge Cases', () => {
    it('handles input without attributes', () => {
      const el = document.createElement('custom-input');
      expect(el.value).to.equal('');
    });

    it('handles invalid type', async () => {
      const el = await fixture(html`<custom-input type="invalid"></custom-input>`);
      expect(el.shadowRoot.querySelector('#input').type).to.equal('text');
    });
  });
});