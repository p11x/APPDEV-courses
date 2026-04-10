import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_7_Form-State-Management.js';

describe('07_7_Form-State-Management', () => {
  describe('ComplexStateForm', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<complex-state-form></complex-state-form>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders account type selection', () => {
      const personal = el.shadowRoot.querySelector('#typePersonal');
      const business = el.shadowRoot.querySelector('#typeBusiness');
      const enterprise = el.shadowRoot.querySelector('#typeEnterprise');
      expect(personal).to.exist;
      expect(business).to.exist;
      expect(enterprise).to.exist;
    });

    it('renders conditional sections', () => {
      const personalSection = el.shadowRoot.querySelector('#personalSection');
      const businessSection = el.shadowRoot.querySelector('#businessSection');
      const enterpriseSection = el.shadowRoot.querySelector('#enterpriseSection');
      expect(personalSection).to.exist;
      expect(businessSection).to.exist;
      expect(enterpriseSection).to.exist;
    });

    it('shows personal section by default', () => {
      const personalSection = el.shadowRoot.querySelector('#personalSection');
      expect(personalSection.classList.contains('hidden')).to.be.false;
    });

    it('hides business and enterprise sections by default', () => {
      const businessSection = el.shadowRoot.querySelector('#businessSection');
      const enterpriseSection = el.shadowRoot.querySelector('#enterpriseSection');
      expect(businessSection.classList.contains('hidden')).to.be.true;
      expect(enterpriseSection.classList.contains('hidden')).to.be.true;
    });

    it('switches sections on account type change', async () => {
      const businessType = el.shadowRoot.querySelector('#typeBusiness');
      businessType.checked = true;
      businessType.dispatchEvent(new Event('change'));
      
      await new Promise(r => setTimeout(r, 50));
      
      const personalSection = el.shadowRoot.querySelector('#personalSection');
      const businessSection = el.shadowRoot.querySelector('#businessSection');
      
      expect(personalSection.classList.contains('hidden')).to.be.true;
      expect(businessSection.classList.contains('hidden')).to.be.false;
    });

    it('shows SSO requirement for enterprise', async () => {
      const enterpriseType = el.shadowRoot.querySelector('#typeEnterprise');
      enterpriseType.checked = true;
      enterpriseType.dispatchEvent(new Event('change'));
      
      await new Promise(r => setTimeout(r, 50));
      
      const ssoSection = el.shadowRoot.querySelector('#ssorequirement');
      expect(ssoSection.classList.contains('hidden')).to.be.false;
    });

    it('renders action buttons', () => {
      expect(el.shadowRoot.querySelector('#undoBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#redoBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#historyBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#saveBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#resetBtn')).to.exist;
    });

    it('renders submit and cancel buttons', () => {
      expect(el.shadowRoot.querySelector('#submitBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#cancelBtn')).to.exist;
    });

    it('renders state indicator', () => {
      const stateDot = el.shadowRoot.querySelector('#stateDot');
      const stateText = el.shadowRoot.querySelector('#stateText');
      expect(stateDot).to.exist;
      expect(stateText).to.exist;
    });

    it('shows saved state initially', () => {
      const stateDot = el.shadowRoot.querySelector('#stateDot');
      const stateText = el.shadowRoot.querySelector('#stateText');
      expect(stateDot.classList.contains('saved')).to.be.true;
      expect(stateText.textContent).to.equal('Saved');
    });

    it('marks as modified on field change', async () => {
      const firstName = el.shadowRoot.querySelector('#firstName');
      firstName.value = 'John';
      firstName.dispatchEvent(new Event('input'));
      
      await new Promise(r => setTimeout(r, 50));
      
      const stateDot = el.shadowRoot.querySelector('#stateDot');
      const stateText = el.shadowRoot.querySelector('#stateText');
      expect(stateDot.classList.contains('modified')).to.be.true;
      expect(stateText.textContent).to.equal('Modified');
    });

    it('gets form data', () => {
      const data = el.getFormData();
      expect(data).to.be.an('object');
      expect(data).to.have.property('accountType');
    });

    it('sets form data', () => {
      el.setFormData({ firstName: 'Jane', lastName: 'Doe' });
      const firstName = el.shadowRoot.querySelector('#firstName');
      expect(firstName.value).to.equal('Jane');
    });

    it('resets form', () => {
      el.setFormData({ firstName: 'Test' });
      el.resetForm();
      const firstName = el.shadowRoot.querySelector('#firstName');
      expect(firstName.value).to.equal('');
    });

    it('dispatches form-save event', async () => {
      const listener = oneEvent(el, 'form-save');
      const saveBtn = el.shadowRoot.querySelector('#saveBtn');
      saveBtn.click();
      const event = await listener;
      expect(event.detail).to.have.property('state');
    });

    it('dispatches form-submit event', async () => {
      el.shadowRoot.querySelector('#firstName').value = 'John';
      el.shadowRoot.querySelector('#lastName').value = 'Doe';
      el.shadowRoot.querySelector('#personalEmail').value = 'john@example.com';
      
      const listener = oneEvent(el, 'form-submit');
      const submitBtn = el.shadowRoot.querySelector('#submitBtn');
      submitBtn.click();
      
      const event = await listener;
      expect(event.detail).to.have.property('state');
    });

    it('disables undo button when stack is empty', () => {
      const undoBtn = el.shadowRoot.querySelector('#undoBtn');
      expect(undoBtn.disabled).to.be.true;
    });

    it('disables redo button when stack is empty', () => {
      const redoBtn = el.shadowRoot.querySelector('#redoBtn');
      expect(redoBtn.disabled).to.be.true;
    });

    it('toggles history panel', () => {
      const historyBtn = el.shadowRoot.querySelector('#historyBtn');
      const historyPanel = el.shadowRoot.querySelector('#historyPanel');
      
      historyBtn.click();
      expect(historyPanel.classList.contains('hidden')).to.be.false;
      
      historyBtn.click();
      expect(historyPanel.classList.contains('hidden')).to.be.true;
    });

    it('renders history list', () => {
      const historyBtn = el.shadowRoot.querySelector('#historyBtn');
      historyBtn.click();
      
      const historyList = el.shadowRoot.querySelector('#historyList');
      expect(historyList).to.exist;
    });

    it('validates required fields on submit', () => {
      const submitBtn = el.shadowRoot.querySelector('#submitBtn');
      submitBtn.click();
      
      const firstName = el.shadowRoot.querySelector('#firstName');
      expect(firstName.hasAttribute('aria-invalid')).to.be.true;
    });

    it('renders language preference select', () => {
      const language = el.shadowRoot.querySelector('#language');
      expect(language).to.exist;
    });

    it('renders newsletter checkbox', () => {
      const newsletter = el.shadowRoot.querySelector('#newsletter');
      expect(newsletter).to.exist;
    });
  });

  describe('Edge Cases', () => {
    it('handles form without input', () => {
      const el = document.createElement('complex-state-form');
      expect(el).to.exist;
    });

    it('handles rapid account type changes', async () => {
      const el = await fixture(html`<complex-state-form></complex-state-form>`);
      
      el.shadowRoot.querySelector('#typeBusiness').checked = true;
      el.shadowRoot.querySelector('#typeBusiness').dispatchEvent(new Event('change'));
      
      el.shadowRoot.querySelector('#typeEnterprise').checked = true;
      el.shadowRoot.querySelector('#typeEnterprise').dispatchEvent(new Event('change'));
      
      el.shadowRoot.querySelector('#typePersonal').checked = true;
      el.shadowRoot.querySelector('#typePersonal').dispatchEvent(new Event('change'));
      
      await new Promise(r => setTimeout(r, 50));
      
      const personalSection = el.shadowRoot.querySelector('#personalSection');
      expect(personalSection.classList.contains('hidden')).to.be.false;
    });
  });
});