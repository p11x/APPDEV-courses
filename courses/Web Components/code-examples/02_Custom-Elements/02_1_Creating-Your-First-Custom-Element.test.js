/**
 * @group unit
 * @group my-first-element
 */
import { expect, fixture, html } from '@open-wc/testing';
import './02_1_Creating-Your-First-Custom-Element.js';

describe('MyFirstElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<my-first-element></my-first-element>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should render hello world content', () => {
    const h1 = element.shadowRoot.querySelector('h1');
    expect(h1).to.exist;
    expect(h1.textContent).to.include('Hello World');
  });

  it('should have container with proper styling', () => {
    const container = element.shadowRoot.querySelector('.container');
    expect(container).to.exist;
  });

  it('should be defined in custom elements registry', () => {
    expect(customElements.get('my-first-element')).to.exist;
  });
});

describe('MyFirstButton', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<button is="my-first-button">Custom Button</button>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have button element inside', () => {
    const button = element.shadowRoot.querySelector('button');
    expect(button).to.exist;
  });

  it('should support slot content', () => {
    const slot = element.shadowRoot.querySelector('slot');
    expect(slot).to.exist;
  });

  it('should be defined as customized built-in', () => {
    expect(customElements.get('my-first-button')).to.exist;
  });
});
