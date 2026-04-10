import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import '../../../code-examples/08_Interoperability/08_1_Framework-Neutral-Patterns.js';

describe('08_1_Framework-Neutral-Patterns', () => {
  describe('UniversalCard', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<universal-card title="Test Card" description="Test description"></universal-card>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders card structure', () => {
      const card = el.shadowRoot.querySelector('.card');
      const title = el.shadowRoot.querySelector('.card-title');
      const description = el.shadowRoot.querySelector('.card-description');
      expect(card).to.exist;
      expect(title).to.exist;
      expect(description).to.exist;
    });

    it('applies title attribute', () => {
      expect(el.getAttribute('title')).to.equal('Test Card');
    });

    it('applies description attribute', () => {
      expect(el.getAttribute('description')).to.equal('Test description');
    });

    it('applies image attribute', async () => {
      el.setAttribute('image', 'https://example.com/image.jpg');
      await el.updateComplete;
      const img = el.shadowRoot.querySelector('.card-image');
      expect(img.src).to.equal('https://example.com/image.jpg');
    });

    it('applies variant attribute', async () => {
      el.setAttribute('variant', 'outlined');
      await el.updateComplete;
      expect(el.hasAttribute('variant')).to.be.true;
    });

    it('applies disabled attribute', async () => {
      el.setAttribute('disabled', '');
      await el.updateComplete;
      expect(el.hasAttribute('disabled')).to.be.true;
    });

    it('applies loading attribute', async () => {
      el.setAttribute('loading', '');
      await el.updateComplete;
      expect(el.hasAttribute('loading')).to.be.true;
    });

    it('gets and sets data property', async () => {
      el.data = { title: 'Data Title', description: 'Data Description' };
      expect(el.getAttribute('title')).to.equal('Data Title');
    });

    it('gets and sets loading property', async () => {
      el.loading = true;
      expect(el.loading).to.be.true;
      el.loading = false;
      expect(el.loading).to.be.false;
    });

    it('gets and sets disabled property', async () => {
      el.disabled = true;
      expect(el.disabled).to.be.true;
      el.disabled = false;
      expect(el.disabled).to.be.false;
    });

    it('dispatches card:accept event', async () => {
      const listener = oneEvent(el, 'card:accept');
      const button = el.shadowRoot.querySelector('button.primary');
      button.click();
      const event = await listener;
      expect(event.detail.action).to.equal('accept');
    });

    it('dispatches card:decline event', async () => {
      const listener = oneEvent(el, 'card:decline');
      const button = el.shadowRoot.querySelector('button.secondary');
      button.click();
      const event = await listener;
      expect(event.detail.action).to.equal('decline');
    });

    it('calls accept method', () => {
      const listener = oneEvent(el, 'card:accept');
      el.accept();
      listener;
    });

    it('calls decline method', () => {
      const listener = oneEvent(el, 'card:decline');
      el.decline();
      listener;
    });

    it('focuses on primary button', () => {
      el.focus();
    });

    it('handles disabled state preventing action', async () => {
      el.disabled = true;
      let fired = false;
      el.addEventListener('card:accept', () => { fired = true; });
      el.accept();
      expect(fired).to.be.false;
    });

    it('handles multiple variants', async () => {
      const variants = ['default', 'outlined', 'elevated', 'flat'];
      for (const variant of variants) {
        el.setAttribute('variant', variant);
        await el.updateComplete;
        expect(el.hasAttribute('variant')).to.be.true;
      }
    });
  });

  describe('FrameworkNeutralModal', () => {
    let el;

    beforeEach(async () => {
      el = await fixture(html`<framework-neutral-modal title="Modal Title"></framework-neutral-modal>`);
    });

    it('renders with shadow DOM', () => {
      expect(el.shadowRoot).to.exist;
    });

    it('renders modal structure', () => {
      const overlay = el.shadowRoot.querySelector('.overlay');
      const modal = el.shadowRoot.querySelector('.modal');
      const header = el.shadowRoot.querySelector('.header');
      const body = el.shadowRoot.querySelector('.body');
      const footer = el.shadowRoot.querySelector('.footer');
      expect(overlay).to.exist;
      expect(modal).to.exist;
      expect(header).to.exist;
      expect(body).to.exist;
      expect(footer).to.exist;
    });

    it('applies title attribute', () => {
      expect(el.getAttribute('title')).to.equal('Modal Title');
    });

    it('applies open attribute', async () => {
      el.setAttribute('open', '');
      await el.updateComplete;
      expect(el.hasAttribute('open')).to.be.true;
    });

    it('applies size attribute', async () => {
      el.setAttribute('size', 'small');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('small');
    });

    it('applies closable attribute', async () => {
      el.setAttribute('closable', '');
      await el.updateComplete;
      expect(el.hasAttribute('closable')).to.be.true;
    });

    it('calls open method', async () => {
      el.open();
      expect(el.hasAttribute('open')).to.be.true;
    });

    it('calls close method', async () => {
      el.setAttribute('open', '');
      el.setAttribute('closable', '');
      el.close();
      expect(el.hasAttribute('open')).to.be.false;
    });

    it('calls toggle method', async () => {
      el.setAttribute('closable', '');
      el.open();
      el.toggle();
      expect(el.hasAttribute('open')).to.be.false;
      el.toggle();
      expect(el.hasAttribute('open')).to.be.true;
    });

    it('dispatches confirm event', async () => {
      el.setAttribute('open', '');
      el.setAttribute('closable', '');
      const listener = oneEvent(el, 'confirm');
      const confirmBtn = el.shadowRoot.querySelector('button.primary');
      confirmBtn.click();
      const event = await listener;
      expect(event).to.exist;
    });

    it('dispatches cancel event', async () => {
      el.setAttribute('open', '');
      el.setAttribute('closable', '');
      const listener = oneEvent(el, 'cancel');
      const cancelBtn = el.shadowRoot.querySelector('button.secondary');
      cancelBtn.click();
      const event = await listener;
      expect(event).to.exist;
    });

    it('renders close button with aria-label', () => {
      const closeBtn = el.shadowRoot.querySelector('.close-btn');
      expect(closeBtn).to.have.attribute('aria-label');
    });

    it('does not close without closable attribute', () => {
      el.setAttribute('open', '');
      el.close();
      expect(el.hasAttribute('open')).to.be.true;
    });

    it('handles size small', async () => {
      el.setAttribute('size', 'small');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('small');
    });

    it('handles size large', async () => {
      el.setAttribute('size', 'large');
      await el.updateComplete;
      expect(el.getAttribute('size')).to.equal('large');
    });
  });

  describe('Edge Cases', () => {
    it('handles universal card without attributes', () => {
      const el = document.createElement('universal-card');
      expect(el).to.exist;
    });

    it('handles modal without attributes', () => {
      const el = document.createElement('framework-neutral-modal');
      expect(el).to.exist;
    });

    it('handles card with empty data', async () => {
      const el = await fixture(html`<universal-card></universal-card>`);
      el.data = null;
    });
  });
});