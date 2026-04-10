import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/07_Forms/07_3_Data-Binding-Advanced-Methods.js';

describe('07_3_Data-Binding-Advanced-Methods', () => {
  describe('DataBindingForm', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<data-binding-form></data-binding-form>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders form element', () => {
      const form = el.shadowRoot.querySelector('#dataForm');
      expect(form).to.exist;
    });

    it('renders bound input fields', () => {
      expect(el.shadowRoot.querySelector('[data-bind="username"]')).to.exist;
      expect(el.shadowRoot.querySelector('[data-bind="email"]')).to.exist;
      expect(el.shadowRoot.querySelector('[data-bind="firstName"]')).to.exist;
      expect(el.shadowRoot.querySelector('[data-bind="lastName"]')).to.exist;
    });

    it('renders select field', () => {
      const country = el.shadowRoot.querySelector('[data-bind="country"]');
      expect(country).to.exist;
      expect(country.tagName).to.equal('SELECT');
    });

    it('renders checkbox fields', () => {
      expect(el.shadowRoot.querySelector('[data-bind="newsletter"]')).to.exist;
      expect(el.shadowRoot.querySelector('[data-bind="notifications"]')).to.exist;
      expect(el.shadowRoot.querySelector('[data-bind="publicProfile"]')).to.exist;
    });

    it('renders radio button fields', () => {
      expect(el.shadowRoot.querySelector('[data-bind="accountType"]')).to.exist;
    });

    it('renders action buttons', () => {
      expect(el.shadowRoot.querySelector('#saveBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#revertBtn')).to.exist;
      expect(el.shadowRoot.querySelector('#resetBtn')).to.exist;
    });

    it('renders data preview', () => {
      const preview = el.shadowRoot.querySelector('#dataPreview');
      expect(preview).to.exist;
    });

    it('renders dirty indicator', () => {
      const indicator = el.shadowRoot.querySelector('.dirty-indicator');
      expect(indicator).to.exist;
      expect(indicator.classList.contains('pristine')).to.be.true;
    });

    it('renders change indicator', () => {
      const indicator = el.shadowRoot.querySelector('.change-indicator');
      expect(indicator).to.exist;
    });

    it('gets data from form', () => {
      const data = el.getData();
      expect(data).to.be.an('object');
    });

    it('sets data to form', () => {
      el.setData({ username: 'testuser', email: 'test@example.com' });
      const data = el.getData();
      expect(data.username).to.equal('testuser');
    });

    it('gets individual property', () => {
      el.shadowRoot.querySelector('[data-bind="username"]').value = 'john';
      expect(el.getProperty('username')).to.equal('john');
    });

    it('sets individual property', () => {
      el.setProperty('email', 'jane@example.com');
      expect(el.getProperty('email')).to.equal('jane@example.com');
    });

    it('observes property changes', () => {
      let observedValue;
      el.observeProperty('username', (value) => {
        observedValue = value;
      });
      el.setProperty('username', 'observer-test');
      expect(observedValue).to.equal('observer-test');
    });

    it('unobserves property changes', () => {
      const observer = (value) => {};
      el.observeProperty('email', observer);
      el.unobserveProperty('email');
    });

    it('registers change callback', () => {
      let callbackTriggered = false;
      el.onChange((property, value) => {
        callbackTriggered = true;
      });
      el.setProperty('firstName', 'Test');
      expect(callbackTriggered).to.be.true;
    });

    it('detects dirty state', () => {
      expect(el.isDirty()).to.be.false;
      el.setProperty('username', 'changed');
      expect(el.isDirty()).to.be.true;
    });

    it('gets dirty fields', () => {
      el.setProperty('username', 'dirty');
      const dirtyFields = el.getDirtyFields();
      expect(dirtyFields).to.include('username');
    });

    it('reverts changes', () => {
      el.setProperty('username', 'changed');
      el.revert();
      expect(el.isDirty()).to.be.false;
    });

    it('saves changes', () => {
      el.setProperty('username', 'new-user');
      el.save();
      expect(el.isDirty()).to.be.false;
    });

    it('resets form', () => {
      el.setProperty('username', 'test');
      el.reset();
      const data = el.getData();
      expect(data.username).to.equal('');
    });

    it('validates form', () => {
      const errors = el.validate();
      expect(errors).to.be.an('array');
    });

    it('dispatches data-save event', async () => {
      const listener = oneEvent(el, 'data-save');
      el.save();
      const event = await listener;
      expect(event.detail).to.have.property('data');
      expect(event.detail).to.have.property('dirtyFields');
    });

    it('dispatches data-revert event', async () => {
      const listener = oneEvent(el, 'data-revert');
      el.setProperty('username', 'test');
      el.revert();
      const event = await listener;
      expect(event.detail).to.have.property('data');
    });

    it('dispatches data-reset event', async () => {
      const listener = oneEvent(el, 'data-reset');
      el.reset();
      const event = await listener;
      expect(event).to.exist;
    });

    it('updates dirty indicator on change', () => {
      el.setProperty('username', 'dirty-user');
      const indicator = el.shadowRoot.querySelector('.dirty-indicator');
      expect(indicator.classList.contains('dirty')).to.be.true;
    });
  });

  describe('Edge Cases', () => {
    it('handles unbound property', () => {
      const el = document.createElement('data-binding-form');
      expect(el.getProperty('nonexistent')).to.be.undefined;
    });

    it('handles setData with unknown properties', () => {
      const el = document.createElement('data-binding-form');
      el.setData({ unknownProp: 'value' });
    });
  });
});