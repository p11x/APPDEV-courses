/**
 * @group unit
 * @group js-fundamentals
 */
import { expect, fixture, html } from '@open-wc/testing';
import './01_4_JavaScript-Fundamentals-for-Web-Components.js';

describe('ButtonComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<wc-button>Click Me</wc-button>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
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

  it('should handle size property', async () => {
    element.size = 'large';
    await element.updateComplete;
    expect(element.size).to.equal('large');
  });

  it('should handle size attribute', async () => {
    element.setAttribute('size', 'small');
    await element.updateComplete;
    expect(element.size).to.equal('small');
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

  it('should reflect property to attribute', async () => {
    element.variant = 'secondary';
    expect(element.getAttribute('variant')).to.equal('secondary');
  });

  it('should have default variant', () => {
    expect(element.variant).to.equal('primary');
  });

  it('should have default size', () => {
    expect(element.size).to.equal('medium');
  });
});

describe('ComponentRegistry', () => {
  it('should create registry instance', () => {
    expect(registry).to.exist;
  });

  it('should register components', () => {
    registry.register('test-component', class extends HTMLElement {});
    expect(registry.get('test-component')).to.exist;
  });

  it('should get registered component', () => {
    const result = registry.get('test-component');
    expect(result).to.exist;
  });
});

describe('createComponent', () => {
  it('should be a function', () => {
    expect(createComponent).to.be.a('function');
  });
});
