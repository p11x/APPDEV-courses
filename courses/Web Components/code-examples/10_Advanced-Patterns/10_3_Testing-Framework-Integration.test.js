/**
 * @group unit
 * @group advanced-patterns
 */
import { expect, fixture, html } from '@open-wc/testing';
import './10_3_Testing-Framework-Integration.js';

describe('TestHarness', () => {
  let harness;

  beforeEach(() => {
    harness = new TestHarness();
  });

  it('should create elements', async () => {
    const el = document.createElement('div');
    document.body.appendChild(el);
    harness.components.set(el, { tagName: 'div', props: {} });
    expect(harness.components.size).to.equal(1);
    document.body.removeChild(el);
  });

  it('should store fixtures', () => {
    harness.fixture('test-fixture', { data: 'test' });
    expect(harness.getFixture('test-fixture')).to.deep.equal({ data: 'test' });
  });

  it('should create spy on methods', () => {
    const obj = { method: () => 'result' };
    const spy = harness.spyOn(obj, 'method');
    const result = spy();
    expect(result).to.equal('result');
    expect(spy.mock.calls.length).to.equal(1);
  });

  it('should flush promises', async () => {
    let resolved = false;
    harness.flush().then(() => { resolved = true; });
    await new Promise(resolve => setTimeout(resolve, 10));
    expect(resolved).to.be.true;
  });
});

describe('ComponentTester', () => {
  let tester;

  beforeEach(() => {
    class TestComponent extends HTMLElement {
      connectedCallback() {
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.innerHTML = '<div>Test</div>';
      }
    }
    customElements.define('test-component', TestComponent);
    tester = new ComponentTester(TestComponent, 'test-component');
  });

  it('should mount component', async () => {
    const element = document.createElement('test-component');
    document.body.appendChild(element);
    expect(element.shadowRoot).to.exist;
    document.body.removeChild(element);
  });

  it('should trigger events', () => {
    const element = { 
      dispatchEvent: (event) => true 
    };
    const event = tester.trigger(element, 'custom-event', { data: 'test' });
    expect(event.type).to.equal('custom-event');
    expect(tester.events.length).to.equal(1);
  });

  it('should get events by type', () => {
    const element = { dispatchEvent: () => true };
    tester.trigger(element, 'click');
    tester.trigger(element, 'click');
    tester.trigger(element, 'change');
    const clicks = tester.getEvents('click');
    expect(clicks.length).to.equal(2);
  });
});

describe('MockShadowRoot', () => {
  let mockShadow;

  beforeEach(() => {
    mockShadow = new MockShadowRoot();
  });

  it('should have open mode', () => {
    expect(mockShadow.mode).to.equal('open');
  });

  it('should append children', () => {
    const child = document.createElement('div');
    mockShadow.appendChild(child);
    expect(mockShadow.elements.size).to.equal(1);
  });

  it('should remove children', () => {
    const child = document.createElement('div');
    mockShadow.appendChild(child);
    mockShadow.removeChild(child);
    expect(mockShadow.elements.size).to.equal(0);
  });

  it('should query selector', () => {
    const child = document.createElement('div');
    child.className = 'test-class';
    mockShadow.appendChild(child);
    const found = mockShadow.querySelector('.test-class');
    expect(found).to.equal(child);
  });

  it('should query selector all', () => {
    const child1 = document.createElement('div');
    child1.className = 'test-class';
    const child2 = document.createElement('div');
    child2.className = 'test-class';
    mockShadow.appendChild(child1);
    mockShadow.appendChild(child2);
    const found = mockShadow.querySelectorAll('.test-class');
    expect(found.length).to.equal(2);
  });
});

describe('MockCustomElement', () => {
  let mockElement;

  beforeEach(() => {
    mockElement = new MockCustomElement('test-element');
  });

  it('should have tag name', () => {
    expect(mockElement.tagName).to.equal('test-element');
  });

  it('should set and get attributes', () => {
    mockElement.setAttribute('id', 'test-id');
    expect(mockElement.getAttribute('id')).to.equal('test-id');
  });

  it('should check attributes', () => {
    mockElement.setAttribute('disabled', 'true');
    expect(mockElement.hasAttribute('disabled')).to.be.true;
  });

  it('should remove attributes', () => {
    mockElement.setAttribute('test', 'value');
    mockElement.removeAttribute('test');
    expect(mockElement.hasAttribute('test')).to.be.false;
  });

  it('should add and remove event listeners', () => {
    const handler = () => {};
    mockElement.addEventListener('click', handler);
    expect(mockElement.listeners.has('click')).to.be.true;
    mockElement.removeEventListener('click', handler);
    expect(mockElement.listeners.get('click').size).to.equal(0);
  });

  it('should dispatch events', () => {
    const handler = { call: false };
    mockElement.addEventListener('test', () => { handler.call = true; });
    const event = new CustomEvent('test');
    mockElement.dispatchEvent(event);
    expect(handler.call).to.be.true;
  });

  it('should have default version', () => {
    expect(mockElement.constructor.version).to.equal('1.0.0');
  });

  it('should get and set innerHTML', () => {
    mockElement.innerHTML = '<span>Test</span>';
    expect(mockElement.innerHTML).to.include('Test');
  });
});

describe('CypressComponentCommands', () => {
  let commands;

  beforeEach(() => {
    commands = new CypressComponentCommands();
  });

  it('should initialize commands object', () => {
    expect(commands.commands).to.deep.equal({});
  });
});

describe('JestMatchers', () => {
  let matchers;

  beforeEach(() => {
    matchers = new JestMatchers();
  });

  it('should have attribute matcher', () => {
    const element = { getAttribute: () => 'value' };
    expect(matchers.toHaveAttribute(element, 'class', 'value')).to.be.true;
  });

  it('should have shadow matcher', () => {
    const element = { shadowRoot: { querySelector: () => ({}) } };
    expect(matchers.toHaveShadow(element, 'div')).to.be.true;
  });

  it('should have version matcher', () => {
    const element = { constructor: { version: '1.0.0' } };
    expect(matchers.toHaveVersion(element, '1.0.0')).to.be.true;
  });
});

describe('Global Exports', () => {
  it('should export testHarness', () => {
    expect(testHarness).to.exist;
  });

  it('should export cypressCommands', () => {
    expect(cypressCommands).to.exist;
  });

  it('should export jestMatchers', () => {
    expect(jestMatchers).to.exist;
  });
});