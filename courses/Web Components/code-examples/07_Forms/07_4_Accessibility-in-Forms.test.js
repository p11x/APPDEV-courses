import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_4_Accessibility-in-Forms.js';

describe('07_4_Accessibility-in-Forms', () => {
  describe('AccessibleForm', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<accessible-form></accessible-form>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders form element', () => {
      const form = el.shadowRoot.querySelector('#accessibleForm');
      expect(form).to.exist;
    });

    it('renders with proper role', () => {
      const wrapper = el.shadowRoot.querySelector('[role="form"]');
      expect(wrapper).to.exist;
    });

    it('has aria-labelledby', () => {
      const wrapper = el.shadowRoot.querySelector('[aria-labelledby="formTitle"]');
      expect(wrapper).to.exist;
    });

    it('has aria-describedby for description', () => {
      const wrapper = el.shadowRoot.querySelector('[aria-describedby="formDesc"]');
      expect(wrapper).to.exist;
    });

    it('renders skip link', () => {
      const skipLink = el.shadowRoot.querySelector('.skip-link');
      expect(skipLink).to.exist;
      expect(skipLink.getAttribute('href')).to.equal('#firstField');
    });

    it('renders required fields with aria-required', () => {
      const fullName = el.shadowRoot.querySelector('#fullName');
      expect(fullName).to.have.attribute('aria-required');
      expect(fullName).to.have.attribute('required');
    });

    it('renders fields with aria-describedby', () => {
      const email = el.shadowRoot.querySelector('#email');
      expect(email).to.have.attribute('aria-describedby');
    });

    it('renders error messages with role alert', () => {
      const errorElement = el.shadowRoot.querySelector('#fullNameError');
      expect(errorElement).to.have.attribute('role');
      expect(errorElement.getAttribute('role')).to.equal('alert');
    });

    it('renders radiogroup with role', () => {
      const radioGroup = el.shadowRoot.querySelector('[role="radiogroup"]');
      expect(radioGroup).to.exist;
    });

    it('renders fieldset with legend', () => {
      const fieldset = el.shadowRoot.querySelector('.fieldset');
      const legend = el.shadowRoot.querySelector('.fieldset-legend');
      expect(fieldset).to.exist;
      expect(legend).to.exist;
    });

    it('renders visually hidden class for screen readers', () => {
      const hiddenElements = el.shadowRoot.querySelectorAll('.visually-hidden');
      expect(hiddenElements.length).to.be.greaterThan(0);
    });

    it('renders live region for announcements', () => {
      const liveRegion = el.shadowRoot.querySelector('[aria-live="polite"]');
      expect(liveRegion).to.exist;
      expect(liveRegion.id).to.equal('formStatus');
    });

    it('renders submit and reset buttons', () => {
      const buttons = el.shadowRoot.querySelectorAll('button');
      expect(buttons.length).to.equal(2);
      expect(buttons[0]).to.have.attribute('type', 'submit');
      expect(buttons[1]).to.have.attribute('type', 'reset');
    });

    it('focuses on first field', () => {
      el.focus();
      expect(document.activeElement.id).to.equal('firstField');
    });

    it('gets form reference', () => {
      expect(el.form).to.exist;
    });

    it('dispatches form-success event on valid submit', async () => {
      const listener = oneEvent(el, 'form-success');
      
      el.shadowRoot.querySelector('#fullName').value = 'John Doe';
      el.shadowRoot.querySelector('#email').value = 'john@example.com';
      el.shadowRoot.querySelector('#inquiryType').value = 'support';
      el.shadowRoot.querySelector('#message').value = 'Test message';
      
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      const event = await listener;
      expect(event.detail.message).to.equal('Form submitted successfully');
    });

    it('dispatches form-error event on invalid submit', async () => {
      const listener = oneEvent(el, 'form-error');
      
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      const event = await listener;
      expect(event.detail.message).to.include('errors');
    });

    it('sets aria-invalid on invalid field', async () => {
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      await new Promise(r => setTimeout(r, 50));
      
      const fullName = el.shadowRoot.querySelector('#fullName');
      expect(fullName).to.have.attribute('aria-invalid');
    });

    it('removes aria-invalid on valid input', async () => {
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      await new Promise(r => setTimeout(r, 50));
      
      const fullName = el.shadowRoot.querySelector('#fullName');
      fullName.value = 'John';
      
      const inputEvent = new Event('input');
      fullName.dispatchEvent(inputEvent);
      
      await new Promise(r => setTimeout(r, 50));
      
      expect(fullName.hasAttribute('aria-invalid')).to.be.false;
    });

    it('shows error message on invalid', async () => {
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      
      await new Promise(r => setTimeout(r, 50));
      
      const errorElement = el.shadowRoot.querySelector('#fullNameError');
      expect(errorElement.hidden).to.be.false;
      expect(errorElement.textContent).to.not.equal('');
    });
  });

  describe('Edge Cases', () => {
    it('handles form without inputs', () => {
      const el = document.createElement('accessible-form');
      expect(el).to.exist;
    });

    it('handles empty required field', async () => {
      const el = await fixture(html`<accessible-form></accessible-form>`);
      const form = el.shadowRoot.querySelector('#accessibleForm');
      form.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
    });
  });
});