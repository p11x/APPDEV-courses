/**
 * @group unit
 * @group shadow-dom
 */
import { expect, fixture, html, oneEvent } from '@open-wc/testing';
import './02_5_Shadow-DOM-Integration.js';

describe('ShadowDOMElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<shadow-element></shadow-element>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have shadow DOM in open mode', () => {
    expect(element.shadowRoot.mode).to.equal('open');
  });

  it('should handle highlight attribute', async () => {
    element.setAttribute('highlight', '');
    await element.updateComplete;
    expect(element.hasAttribute('highlight')).to.be.true;
  });

  it('should handle variant attribute', async () => {
    element.setAttribute('variant', 'primary');
    await element.updateComplete;
  });

  it('should have container element', () => {
    const container = element.shadowRoot.querySelector('.container');
    expect(container).to.exist;
  });

  it('should have h2 element', () => {
    const h2 = element.shadowRoot.querySelector('h2');
    expect(h2).to.exist;
  });

  it('should have default slot', () => {
    const slot = element.shadowRoot.querySelector('slot:not([name])');
    expect(slot).to.exist;
  });

  it('should have named title slot', () => {
    const slot = element.shadowRoot.querySelector('slot[name="title"]');
    expect(slot).to.exist;
  });

  it('should have footer slot', () => {
    const slot = element.shadowRoot.querySelector('slot[name="footer"]');
    expect(slot).to.exist;
  });
});

describe('OpenShadowElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<open-shadow-element></open-shadow-element>`);
  });

  it('should have accessible shadow root', () => {
    expect(element.shadowRoot).to.exist;
  });
});

describe('ClosedShadowElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<closed-shadow-element></closed-shadow-element>`);
  });

  it('should not have accessible shadow root', () => {
    expect(element.shadowRoot).to.be.null;
  });
});

describe('CardWithSlots', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<card-with-slots>
      <span slot="header">Custom Header</span>
      <div>Content</div>
      <button slot="footer">Action</button>
    </card-with-slots>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have card element', () => {
    const card = element.shadowRoot.querySelector('.card');
    expect(card).to.exist;
  });

  it('should have header element', () => {
    const header = element.shadowRoot.querySelector('.header');
    expect(header).to.exist;
  });

  it('should have body element', () => {
    const body = element.shadowRoot.querySelector('.body');
    expect(body).to.exist;
  });

  it('should have footer element', () => {
    const footer = element.shadowRoot.querySelector('.footer');
    expect(footer).to.exist;
  });
});

describe('ShadowEventElement', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<shadow-event></shadow-event>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have button', () => {
    const btn = element.shadowRoot.getElementById('btn');
    expect(btn).to.exist;
  });

  it('should dispatch internal-click event', async () => {
    const btn = element.shadowRoot.getElementById('btn');
    setTimeout(() => btn.click(), 0);
    const event = await oneEvent(element, 'internal-click');
    expect(event).to.exist;
  });

  it('should include timestamp in event detail', async () => {
    const btn = element.shadowRoot.getElementById('btn');
    setTimeout(() => btn.click(), 0);
    const event = await oneEvent(element, 'internal-click');
    expect(event.detail).to.have.property('timestamp');
  });
});
