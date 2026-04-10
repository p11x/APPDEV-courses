/**
 * @group unit
 * @group custom-elements-defining
 */
import { expect, fixture, html } from '@open-wc/testing';
import './02_4_Defining-Custom-Elements.js';

describe('BasicElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<basic-element></basic-element>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should be defined', () => {
    expect(customElements.get('basic-element')).to.exist;
  });
});

describe('ExtendedButton', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<button is="extended-button">Extended</button>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have button element in shadow DOM', () => {
    const button = element.shadowRoot.querySelector('button');
    expect(button).to.exist;
  });

  it('should handle variant attribute', async () => {
    element.setAttribute('variant', 'success');
    await element.updateComplete;
  });

  it('should be defined as customized built-in', () => {
    expect(customElements.get('extended-button')).to.exist;
  });
});

describe('ComponentRegistry', () => {
  it('should create registry instance', () => {
    expect(registry).to.exist;
  });

  it('should have register method', () => {
    expect(registry.register).to.be.a('function');
  });

  it('should have get method', () => {
    expect(registry.get).to.be.a('function');
  });

  it('should have isDefined method', () => {
    expect(registry.isDefined).to.be.a('function');
  });

  it('should have whenDefined method', () => {
    expect(registry.whenDefined).to.be.a('function');
  });

  it('should check if element is defined', () => {
    expect(registry.isDefined('basic-element')).to.be.true;
  });

  it('should wait for element definition', async () => {
    const result = await registry.whenDefined('basic-element');
    expect(result).to.exist;
  });
});

describe('createComponent', () => {
  it('should be a function', () => {
    expect(createComponent).to.be.a('function');
  });

  it('should create factory button', () => {
    expect(customElements.get('factory-button')).to.exist;
  });
});

describe('LazyDefine', () => {
  it('should have observe method', () => {
    expect(LazyDefine.observe).to.be.a('function');
  });
});
