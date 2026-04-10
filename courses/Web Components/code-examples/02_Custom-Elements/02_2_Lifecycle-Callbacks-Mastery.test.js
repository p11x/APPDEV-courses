/**
 * @group unit
 * @group full-lifecycle
 */
import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import './02_2_Lifecycle-Callbacks-Mastery.js';

describe('FullLifecycleComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<full-lifecycle data-value="test"></full-lifecycle>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should handle data-value attribute', async () => {
    expect(element.value).to.equal('test');
  });

  it('should handle disabled attribute', async () => {
    element.setAttribute('disabled', '');
    await element.updateComplete;
    expect(element.hasAttribute('disabled')).to.be.true;
  });

  it('should handle mode attribute', async () => {
    element.setAttribute('mode', 'edit');
    await element.updateComplete;
    expect(element.mode).to.equal('edit');
  });

  it('should reflect value property to attribute', async () => {
    element.value = 'new-value';
    expect(element.getAttribute('data-value')).to.equal('new-value');
  });

  it('should have input element', () => {
    const input = element.shadowRoot.querySelector('input');
    expect(input).to.exist;
  });

  it('should have container', () => {
    const container = element.shadowRoot.querySelector('.container');
    expect(container).to.exist;
  });

  it('should show mode display', () => {
    const modeEl = element.shadowRoot.querySelector('.mode');
    expect(modeEl).to.exist;
  });

  it('should show value display', () => {
    const valueEl = element.shadowRoot.querySelector('.value');
    expect(valueEl).to.exist;
  });

  it('should fire component-ready event', async () => {
    const newEl = document.createElement('full-lifecycle');
    setTimeout(() => document.body.appendChild(newEl), 0);
    const event = await oneEvent(document.body, 'component-ready');
    newEl.remove();
    expect(event).to.exist;
  });

  it('should handle attribute changes', async () => {
    element.setAttribute('mode', 'view');
    await element.updateComplete;
    expect(element.mode).to.equal('view');
  });

  it('should have reset method', () => {
    expect(element.reset).to.be.a('function');
  });

  it('should reset value', async () => {
    element.value = 'some-value';
    await element.updateComplete;
    element.reset();
    expect(element.value).to.be.null;
  });

  it('should handle disabled state in container', async () => {
    element.setAttribute('disabled', '');
    await element.updateComplete;
    const container = element.shadowRoot.querySelector('.container');
    expect(container.classList.contains('disabled')).to.be.true;
  });

  it('should dispatch element-click event', async () => {
    const container = element.shadowRoot.querySelector('.container');
    setTimeout(() => container.click(), 0);
    const event = await oneEvent(element, 'element-click');
    expect(event).to.exist;
  });
});
