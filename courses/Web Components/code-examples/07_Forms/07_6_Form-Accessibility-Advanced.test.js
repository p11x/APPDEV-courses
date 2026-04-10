import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_6_Form-Accessibility-Advanced.js';

describe('07_6_Form-Accessibility-Advanced', () => {
  describe('AdvancedAccessibleForm', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<advanced-accessible-form></advanced-accessible-form>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders form element', () => {
      const form = el.shadowRoot.querySelector('#advancedForm');
      expect(form).to.exist;
    });

    it('renders progress steps', () => {
      const steps = el.shadowRoot.querySelectorAll('.step');
      expect(steps.length).to.equal(3);
    });

    it('renders step indicator with role tablist', () => {
      const stepList = el.shadowRoot.querySelector('[role="tablist"]');
      expect(stepList).to.exist;
    });

    it('renders progress bar with role progressbar', () => {
      const progressbar = el.shadowRoot.querySelector('[role="progressbar"]');
      expect(progressbar).to.exist;
      expect(progressbar).to.have.attribute('aria-valuenow');
      expect(progressbar).to.have.attribute('aria-valuemin');
      expect(progressbar).to.have.attribute('aria-valuemax');
    });

    it('renders form title and description', () => {
      const title = el.shadowRoot.querySelector('.form-title');
      const desc = el.shadowRoot.querySelector('.form-description');
      expect(title).to.exist;
      expect(desc).to.exist;
    });

    it('renders first step as active', () => {
      const activeStep = el.shadowRoot.querySelector('.step.active');
      expect(activeStep).to.exist;
    });

    it('renders tabpanel for each step', () => {
      const step1 = el.shadowRoot.querySelector('#step1');
      const step2 = el.shadowRoot.querySelector('#step2');
      const step3 = el.shadowRoot.querySelector('#step3');
      expect(step1).to.exist;
      expect(step2).to.exist;
      expect(step3).to.exist;
    });

    it('renders form fields with accessibility attributes', () => {
      const username = el.shadowRoot.querySelector('#username');
      expect(username).to.have.attribute('aria-required');
      expect(username).to.have.attribute('aria-describedby');
    });

    it('renders error containers with role alert', () => {
      const errorContainer = el.shadowRoot.querySelector('#usernameError');
      expect(errorContainer).to.have.attribute('role');
      expect(errorContainer.getAttribute('role')).to.equal('alert');
    });

    it('renders error containers with aria-live', () => {
      const errorContainer = el.shadowRoot.querySelector('#usernameError');
      expect(errorContainer).to.have.attribute('aria-live');
    });

    it('renders live region for announcements', () => {
      const liveRegion = el.shadowRoot.querySelector('#liveRegion');
      expect(liveRegion).to.have.attribute('aria-live');
      expect(liveRegion.getAttribute('aria-live')).to.equal('polite');
    });

    it('renders assertive region for errors', () => {
      const assertiveRegion = el.shadowRoot.querySelector('#assertiveRegion');
      expect(assertiveRegion).to.have.attribute('aria-live');
      expect(assertiveRegion.getAttribute('aria-live')).to.equal('assertive');
    });

    it('renders navigation buttons', () => {
      const prevBtn = el.shadowRoot.querySelector('#prevBtn');
      const nextBtn = el.shadowRoot.querySelector('#nextBtn');
      const submitBtn = el.shadowRoot.querySelector('#submitBtn');
      expect(prevBtn).to.exist;
      expect(nextBtn).to.exist;
      expect(submitBtn).to.exist;
    });

    it('renders summary area with aria-live', () => {
      const summary = el.shadowRoot.querySelector('#summary');
      expect(summary).to.have.attribute('aria-live');
    });

    it('shows first step and hides others', () => {
      const step1 = el.shadowRoot.querySelector('#step1');
      const step2 = el.shadowRoot.querySelector('#step2');
      expect(step1.hidden).to.be.false;
      expect(step2.hidden).to.be.true;
    });

    it('disables previous button on first step', () => {
      const prevBtn = el.shadowRoot.querySelector('#prevBtn');
      expect(prevBtn.disabled).to.be.true;
    });

    it('dispatches form-submit event on valid submit', async () => {
      el.shadowRoot.querySelector('#username').value = 'testuser';
      el.shadowRoot.querySelector('#email').value = 'test@example.com';
      el.shadowRoot.querySelector('#password').value = 'password123';
      
      const nextBtn = el.shadowRoot.querySelector('#nextBtn');
      nextBtn.click();
      
      await new Promise(r => setTimeout(r, 150));
      
      const step2 = el.shadowRoot.querySelector('#step2');
      step2.querySelector('#fullName').value = 'Test User';
      step2.querySelector('#birthDate').value = '1990-01-01';
      
      nextBtn.click();
      
      await new Promise(r => setTimeout(r, 150));
      
      const submitBtn = el.shadowRoot.querySelector('#submitBtn');
      const listener = oneEvent(el, 'form-submit');
      submitBtn.click();
      
      const event = await listener;
      expect(event).to.exist;
    });

    it('validates required fields on next step', async () => {
      const nextBtn = el.shadowRoot.querySelector('#nextBtn');
      nextBtn.click();
      
      await new Promise(r => setTimeout(r, 50));
      
      const usernameError = el.shadowRoot.querySelector('#usernameError');
      expect(usernameError.hidden).to.be.false;
    });

    it('clears error on input', async () => {
      const nextBtn = el.shadowRoot.querySelector('#nextBtn');
      nextBtn.click();
      
      await new Promise(r => setTimeout(r, 50));
      
      const username = el.shadowRoot.querySelector('#username');
      username.value = 'test';
      username.dispatchEvent(new Event('input'));
      
      await new Promise(r => setTimeout(r, 50));
      
      const usernameError = el.shadowRoot.querySelector('#usernameError');
      expect(usernameError.hidden).to.be.true;
    });
  });

  describe('Edge Cases', () => {
    it('handles empty form', () => {
      const el = document.createElement('advanced-accessible-form');
      expect(el).to.exist;
    });

    it('handles rapid step navigation', async () => {
      const el = await fixture(html`<advanced-accessible-form></advanced-accessible-form>`);
      const nextBtn = el.shadowRoot.querySelector('#nextBtn');
      
      el.shadowRoot.querySelector('#username').value = 'testuser';
      el.shadowRoot.querySelector('#email').value = 'test@example.com';
      el.shadowRoot.querySelector('#password').value = 'password123';
      
      nextBtn.click();
      await new Promise(r => setTimeout(r, 200));
      
      el.shadowRoot.querySelector('#fullName').value = 'Test User';
      el.shadowRoot.querySelector('#birthDate').value = '1990-01-01';
      
      nextBtn.click();
      await new Promise(r => setTimeout(r, 200));
      
      const step3 = el.shadowRoot.querySelector('#step3');
      expect(step3.hidden).to.be.false;
    });
  });
});