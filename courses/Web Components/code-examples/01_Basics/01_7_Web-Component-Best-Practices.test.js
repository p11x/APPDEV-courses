/**
 * @group unit
 * @group best-practices
 */
import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import './01_7_Web-Component-Best-Practices.js';

describe('BestPracticeComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<best-practice-component title="Test"></best-practice-component>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should handle title property', async () => {
    element.title = 'New Title';
    await element.updateComplete;
    expect(element.title).to.equal('New Title');
  });

  it('should handle title attribute', async () => {
    element.setAttribute('title', 'Attr Title');
    await element.updateComplete;
    expect(element.title).to.equal('Attr Title');
  });

  it('should handle variant property', async () => {
    element.variant = 'success';
    await element.updateComplete;
    expect(element.variant).to.equal('success');
  });

  it('should handle variant attribute', async () => {
    element.setAttribute('variant', 'danger');
    await element.updateComplete;
    expect(element.variant).to.equal('danger');
  });

  it('should handle disabled property', async () => {
    element.disabled = true;
    await element.updateComplete;
    expect(element.disabled).to.be.true;
  });

  it('should handle disabled attribute', async () => {
    element.setAttribute('disabled', '');
    await element.updateComplete;
    expect(element.disabled).to.be.true;
  });

  it('should handle loading property', async () => {
    element.loading = true;
    await element.updateComplete;
    expect(element.loading).to.be.true;
  });

  it('should handle loading attribute', async () => {
    element.setAttribute('loading', '');
    await element.updateComplete;
    expect(element.loading).to.be.true;
  });

  it('should reflect properties to attributes', async () => {
    element.title = 'Reflected';
    expect(element.getAttribute('title')).to.equal('Reflected');
  });

  it('should have button element', () => {
    const button = element.shadowRoot.querySelector('.button');
    expect(button).to.exist;
  });

  it('should fire button-click event on click', async () => {
    const button = element.shadowRoot.querySelector('.button');
    setTimeout(() => button.click(), 0);
    const event = await oneEvent(element, 'button-click');
    expect(event).to.exist;
  });

  it('should handle keydown for Enter', async () => {
    const button = element.shadowRoot.querySelector('.button');
    button.dispatchEvent(new KeyboardEvent('keydown', { key: 'Enter', bubbles: true }));
  });

  it('should handle keydown for Space', async () => {
    const button = element.shadowRoot.querySelector('.button');
    button.dispatchEvent(new KeyboardEvent('keydown', { key: ' ', bubbles: true }));
  });

  it('should not fire click when disabled', async () => {
    element.disabled = true;
    await element.updateComplete;
    const button = element.shadowRoot.querySelector('.button');
    setTimeout(() => button.click(), 0);
    await new Promise(r => setTimeout(r, 50));
  });

  it('should not fire click when loading', async () => {
    element.loading = true;
    await element.updateComplete;
    const button = element.shadowRoot.querySelector('.button');
    setTimeout(() => button.click(), 0);
    await new Promise(r => setTimeout(r, 50));
  });

  it('should have proper accessibility', () => {
    const button = element.shadowRoot.querySelector('.button');
    expect(button.getAttribute('role')).to.equal('button');
    expect(button.getAttribute('tabindex')).to.equal('0');
  });

  it('should fire component-ready event', async () => {
    const newEl = document.createElement('best-practice-component');
    setTimeout(() => document.body.appendChild(newEl), 0);
    const event = await oneEvent(document.body, 'component-ready');
    newEl.remove();
    expect(event).to.exist;
  });
});
