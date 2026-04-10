/**
 * @group unit
 * @group dom-manipulation
 */
import { expect, fixture, html } from '@open-wc/testing';
import './01_5_DOM-Manipulation-Mastery.js';

describe('EfficientListComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<efficient-list></efficient-list>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should handle items property', async () => {
    element.items = [
      { id: '1', label: 'Item 1' },
      { id: '2', label: 'Item 2' }
    ];
    await element.updateComplete;
    expect(element.items).to.have.lengthOf(2);
  });

  it('should render items', async () => {
    element.items = [{ id: '1', label: 'Test' }];
    await element.updateComplete;
    const listItems = element.shadowRoot.querySelectorAll('li');
    expect(listItems.length).to.be.greaterThan(0);
  });

  it('should update specific item', async () => {
    element.items = [{ id: '1', label: 'Original' }];
    await element.updateComplete;
    element.updateItem('1', { id: '1', label: 'Updated' });
    const item = element.shadowRoot.querySelector('[data-id="1"]');
    expect(item.textContent).to.equal('Updated');
  });

  it('should add item', async () => {
    element.items = [];
    await element.updateComplete;
    element.addItem({ id: '1', label: 'New Item' });
    expect(element.items).to.have.lengthOf(1);
  });

  it('should remove item', async () => {
    element.items = [{ id: '1', label: 'Item' }];
    await element.updateComplete;
    element.removeItem('1');
    expect(element.items).to.have.lengthOf(0);
  });

  it('should handle empty items', async () => {
    element.items = [];
    await element.updateComplete;
    const listItems = element.shadowRoot.querySelectorAll('li');
    expect(listItems.length).to.equal(0);
  });
});

describe('QueryHelper', () => {
  it('should create query helper with shadow root', () => {
    const div = document.createElement('div');
    div.attachShadow({ mode: 'open' });
    div.shadowRoot.innerHTML = '<div id="test"></div><div class="item"></div>';
    
    const helper = new QueryHelper(div.shadowRoot);
    
    expect(helper.$('div')).to.exist;
    expect(helper.$$('.item')).to.have.lengthOf(1);
    expect(helper.$id('test')).to.exist;
  });
});

describe('ObservableComponent', () => {
  let element;

  beforeEach(async () => {
    element = await fixture(html`<observable-component></observable-component>`);
  });

  it('should render', () => {
    expect(element.shadowRoot).to.exist;
  });

  it('should have content slot', () => {
    const slot = element.shadowRoot.querySelector('slot');
    expect(slot).to.exist;
  });
});
