/**
 * @group unit
 * @group inheritance-composition
 */
import { expect, fixture, html } from '@open-wc/testing';
import './02_3_Inheritance-and-Composition-Patterns.js';

describe('BaseComponent', () => {
  it('should be a function', () => {
    expect(BaseComponent).to.be.a('function');
  });
});

describe('ButtonComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<my-button>Click</my-button>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should handle variant attribute', async () => {
    element.setAttribute('variant', 'success');
    await element.updateComplete;
  });

  it('should handle size attribute', async () => {
    element.setAttribute('size', 'large');
    await element.updateComplete;
  });

  it('should have button element', () => {
    const button = element.shadowRoot.querySelector('button');
    expect(button).to.exist;
  });
});

describe('ReactiveMixin', () => {
  it('should be a function', () => {
    expect(ReactiveMixin).to.be.a('function');
  });

  it('should return a class', () => {
    const MixedClass = ReactiveMixin(HTMLElement);
    expect(MixedClass).to.be.a('function');
  });
});

describe('ValidationMixin', () => {
  it('should be a function', () => {
    expect(ValidationMixin).to.be.a('function');
  });
});

describe('FormMixin', () => {
  it('should be a function', () => {
    expect(FormMixin).to.be.a('function');
  });
});

describe('ComposedComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<my-card>
      <span slot="header">Card Header</span>
      <div>Card Content</div>
      <span slot="footer">Card Footer</span>
    </my-card>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have header slot', () => {
    const slot = element.shadowRoot.querySelector('slot[name="header"]');
    expect(slot).to.exist;
  });

  it('should have default slot', () => {
    const slot = element.shadowRoot.querySelector('slot:not([name])');
    expect(slot).to.exist;
  });

  it('should have footer slot', () => {
    const slot = element.shadowRoot.querySelector('slot[name="footer"]');
    expect(slot).to.exist;
  });

  it('should have addChild method', () => {
    expect(element.addChild).to.be.a('function');
  });
});

describe('FormInput', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<form-input></form-input>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have input element', () => {
    const input = element.shadowRoot.querySelector('input');
    expect(input).to.exist;
  });

  it('should have addValidator method', () => {
    expect(element.addValidator).to.be.a('function');
  });

  it('should have validate method', () => {
    expect(element.validate).to.be.a('function');
  });

  it('should validate input', () => {
    element.addValidator(v => ({ valid: v.length > 0, message: 'Required' }));
    const result = element.validate('test');
    expect(result.valid).to.be.true;
  });
});
