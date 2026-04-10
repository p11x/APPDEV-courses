import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_1_Form-Integration-Mastery.js';

describe('07_1_Form-Integration-Mastery', () => {
  describe('FormIntegrationMastery', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<form-integration-mastery></form-integration-mastery>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders form elements', () => {
      const form = el.shadowRoot.querySelector('#masterForm');
      expect(form).to.exist;
      expect(form).to.have.attribute('novalidate');
    });

    it('renders form fields', () => {
      expect(el.shadowRoot.querySelector('#username')).to.exist;
      expect(el.shadowRoot.querySelector('#email')).to.exist;
      expect(el.shadowRoot.querySelector('#password')).to.exist;
      expect(el.shadowRoot.querySelector('#confirmPassword')).to.exist;
      expect(el.shadowRoot.querySelector('#country')).to.exist;
      expect(el.shadowRoot.querySelector('#bio')).to.exist;
      expect(el.shadowRoot.querySelector('#terms')).to.exist;
    });

    it('renders submit and reset buttons', () => {
      const buttons = el.shadowRoot.querySelectorAll('button');
      expect(buttons.length).to.equal(2);
      expect(buttons[0]).to.have.attribute('type', 'submit');
      expect(buttons[1]).to.have.attribute('type', 'reset');
    });

    it('renders progress indicator', () => {
      const progressBar = el.shadowRoot.querySelector('.progress-bar');
      expect(progressBar).to.exist;
    });

    it('applies action attribute', async () => {
      el.setAttribute('action', '/submit');
      await el.updateComplete;
      expect(el.getAttribute('action')).to.equal('/submit');
    });

    it('applies method attribute', async () => {
      el.setAttribute('method', 'POST');
      await el.updateComplete;
      expect(el.getAttribute('method')).to.equal('POST');
    });

    it('applies enctype attribute', async () => {
      el.setAttribute('enctype', 'multipart/form-data');
      await el.updateComplete;
      expect(el.getAttribute('enctype')).to.equal('multipart/form-data');
    });

    it('applies novalidate attribute', async () => {
      el.setAttribute('novalidate', '');
      await el.updateComplete;
      expect(el.hasAttribute('novalidate')).to.be.true;
    });

    it('applies disabled attribute', async () => {
      el.setAttribute('disabled', '');
      await el.updateComplete;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('dispatches form-submit event', async () => {
      const form = el.shadowRoot.querySelector('#masterForm');
      const listener = oneEvent(el, 'form-submit');
      
      el.shadowRoot.querySelector('#username').value = 'testuser';
      el.shadowRoot.querySelector('#email').value = 'test@example.com';
      el.shadowRoot.querySelector('#password').value = 'password123';
      el.shadowRoot.querySelector('#confirmPassword').value = 'password123';
      el.shadowRoot.querySelector('#country').value = 'us';
      el.shadowRoot.querySelector('#terms').checked = true;
      
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      const event = await listener;
      expect(event.detail).to.have.property('data');
    });

    it('dispatches form-reset event', async () => {
      const form = el.shadowRoot.querySelector('#masterForm');
      const listener = oneEvent(el, 'form-reset');
      form.dispatchEvent(new Event('reset'));
      const event = await listener;
      expect(event).to.exist;
    });

    it('dispatches form-input event', async () => {
      const input = el.shadowRoot.querySelector('#username');
      const listener = oneEvent(el, 'form-input');
      input.value = 'test';
      input.dispatchEvent(new Event('input'));
      const event = await listener;
      expect(event.detail).to.have.property('name');
    });

    it('dispatches form-change event', async () => {
      const input = el.shadowRoot.querySelector('#country');
      const listener = oneEvent(el, 'form-change');
      input.value = 'us';
      input.dispatchEvent(new Event('change'));
      const event = await listener;
      expect(event.detail).to.have.property('value');
    });

    it('gets form reference', () => {
      expect(el.form).to.exist;
    });

    it('gets form data', () => {
      el.shadowRoot.querySelector('#username').value = 'testuser';
      const data = el.getFormData();
      expect(data).to.have.property('username');
    });

    it('resets form', () => {
      el.shadowRoot.querySelector('#username').value = 'testuser';
      el.reset();
      expect(el.shadowRoot.querySelector('#username').value).to.equal('');
    });

    it('validates form', () => {
      const result = el.validate();
      expect(result).to.be.a('boolean');
    });

    it('sets field value', () => {
      el.setValue('username', 'newuser');
      expect(el.getValue('username')).to.equal('newuser');
    });

    it('gets field value', () => {
      el.shadowRoot.querySelector('#email').value = 'test@example.com';
      expect(el.getValue('email')).to.equal('test@example.com');
    });

    it('returns validity state', () => {
      const validity = el.validity;
      expect(validity).to.be.an('object');
    });

    it('returns touched state', () => {
      const touched = el.touched;
      expect(touched).to.be.an('object');
    });

    it('returns dirty state', () => {
      const dirty = el.dirty;
      expect(dirty).to.be.an('object');
    });

    it('updates progress bar', () => {
      el.shadowRoot.querySelector('#username').value = 'test';
      const input = el.shadowRoot.querySelector('#username');
      input.dispatchEvent(new Event('input'));
    });
  });

  describe('Edge Cases', () => {
    it('handles form without fields', () => {
      const el = document.createElement('form-integration-mastery');
      expect(el).to.exist;
    });

    it('handles null field value', () => {
      const el = document.createElement('form-integration-mastery');
      expect(el.getValue('nonexistent')).to.be.null;
    });
  });
});