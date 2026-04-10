import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_2_Validation-Framework-Integration.js';

describe('07_2_Validation-Framework-Integration', () => {
  describe('ValidationForm', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<validation-form></validation-form>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders form element', () => {
      const form = el.shadowRoot.querySelector('#validationForm');
      expect(form).to.exist;
    });

    it('renders form fields', () => {
      expect(el.shadowRoot.querySelector('#email')).to.exist;
      expect(el.shadowRoot.querySelector('#password')).to.exist;
      expect(el.shadowRoot.querySelector('#confirm-password')).to.exist;
      expect(el.shadowRoot.querySelector('#url')).to.exist;
      expect(el.shadowRoot.querySelector('#age')).to.exist;
      expect(el.shadowRoot.querySelector('#terms')).to.exist;
    });

    it('renders submit and reset buttons', () => {
      const buttons = el.shadowRoot.querySelectorAll('button');
      expect(buttons.length).to.equal(2);
      expect(buttons[0]).to.have.attribute('type', 'submit');
      expect(buttons[1]).to.have.attribute('type', 'reset');
    });

    it('renders error messages', () => {
      expect(el.shadowRoot.querySelector('#email-error')).to.exist;
      expect(el.shadowRoot.querySelector('#password-error')).to.exist;
      expect(el.shadowRoot.querySelector('#url-error')).to.exist;
    });

    it('renders validity indicators', () => {
      const indicators = el.shadowRoot.querySelectorAll('.validity-indicator');
      expect(indicators.length).to.be.greaterThan(0);
    });

    it('renders hint messages', () => {
      expect(el.shadowRoot.querySelector('#password-hint')).to.exist;
      expect(el.shadowRoot.querySelector('#age-hint')).to.exist;
    });

    it('applies disabled attribute', async () => {
      el.setAttribute('disabled', '');
      await el.updateComplete;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('applies novalidate attribute', async () => {
      el.setAttribute('novalidate', '');
      await el.updateComplete;
      expect(el.hasAttribute('novalidate')).to.be.true;
    });

    it('gets form reference', () => {
      expect(el.form).to.exist;
    });

    it('validates form', () => {
      const result = el.validate();
      expect(result).to.be.a('boolean');
    });

    it('resets form', () => {
      el.reset();
    });

    it('gets form data', () => {
      const data = el.getData();
      expect(data).to.be.an('object');
    });

    it('sets field error programmatically', () => {
      el.setFieldError('email', 'Custom error message');
      const emailInput = el.shadowRoot.querySelector('#email');
      expect(emailInput.validity.customError).to.be.true;
    });

    it('clears field error programmatically', () => {
      el.setFieldError('email', 'Error');
      el.clearFieldError('email');
      const emailInput = el.shadowRoot.querySelector('#email');
      expect(emailInput.validity.customError).to.be.false;
    });

    it('adds custom validator', () => {
      const emailInput = el.shadowRoot.querySelector('#email');
      el.addCustomValidator('custom', () => ({ valid: true }), ['#email']);
    });

    it('removes custom validator', () => {
      el.addCustomValidator('test-validator', () => ({ valid: true }), ['#email']);
      el.removeCustomValidator('test-validator');
    });

    it('dispatches form-valid event on valid submit', async () => {
      const listener = oneEvent(el, 'form-valid');
      
      el.shadowRoot.querySelector('#email').value = 'test@example.com';
      el.shadowRoot.querySelector('#password').value = 'password123';
      el.shadowRoot.querySelector('#confirm-password').value = 'password123';
      el.shadowRoot.querySelector('#url').value = 'https://example.com';
      el.shadowRoot.querySelector('#age').value = '25';
      el.shadowRoot.querySelector('#terms').checked = true;
      
      const form = el.shadowRoot.querySelector('#validationForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      const event = await listener;
      expect(event.detail).to.have.property('data');
    });

    it('dispatches form-invalid event on invalid submit', async () => {
      const listener = oneEvent(el, 'form-invalid');
      
      const form = el.shadowRoot.querySelector('#validationForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      await listener;
    });

    it('dispatches form-reset event', async () => {
      const listener = oneEvent(el, 'form-reset');
      const form = el.shadowRoot.querySelector('#validationForm');
      form.dispatchEvent(new Event('reset'));
      await listener;
    });
  });

  describe('Edge Cases', () => {
    it('handles form with no fields', () => {
      const el = document.createElement('validation-form');
      expect(el).to.exist;
    });

    it('handles invalid field name for setFieldError', () => {
      const el = document.createElement('validation-form');
      el.setFieldError('nonexistent', 'Error');
    });
  });
});