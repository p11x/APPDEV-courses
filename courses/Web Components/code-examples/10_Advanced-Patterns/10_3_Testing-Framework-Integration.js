/**
 * Testing Framework Integration - Jest and Cypress integration patterns for
 * Web Components with unit tests, integration tests, and E2E testing
 * @module advanced-patterns/10_3_Testing-Framework-Integration
 * @version 1.0.0
 * @example <test-harness></test-harness>
 */

class TestHarness {
  constructor() {
    this.components = new Map();
    this.fixtures = new Map();
    this.spy = new Map();
    this.clock = null;
  }

  create(tagName, props = {}) {
    const element = document.createElement(tagName);
    document.body.appendChild(element);

    for (const [key, value] of Object.entries(props)) {
      element[key] = value;
    }

    this.components.set(element, { tagName, props });
    return element;
  }

  destroy(element) {
    if (element.parentNode) {
      element.parentNode.removeChild(element);
    }
    this.components.delete(element);
  }

  destroyAll() {
    for (const [element] of this.components) {
      this.destroy(element);
    }
  }

  fixture(name, data) {
    this.fixtures.set(name, data);
  }

  getFixture(name) {
    return this.fixtures.get(name);
  }

  spyOn(obj, method) {
    const original = obj[method];
    const calls = [];

    const spy = (...args) => {
      calls.push({ args, timestamp: Date.now() });
      return original.apply(obj, args);
    };

    spy.mock = { calls, original };
    this.spy.set(spy, { obj, method, original });
    return spy;
  }

  useFakeTimers() {
    if (typeof jest !== 'undefined') {
      jest.useFakeTimers();
      this.clock = 'jest';
    }
  }

  useRealTimers() {
    if (typeof jest !== 'undefined') {
      jest.useRealTimers();
      this.clock = null;
    }
  }

  advanceTimersByTime(ms) {
    if (this.clock) {
      jest.advanceTimersByTime(ms);
    }
  }

  flush() {
    return new Promise(resolve => setTimeout(resolve, 0));
  }

  whenDefined(tagName) {
    return customElements.whenDefined(tagName);
  }
}

class ComponentTester {
  constructor(componentClass, tagName) {
    this.ComponentClass = componentClass;
    this.tagName = tagName;
    this.harness = new TestHarness();
    this.events = [];
    this.listeners = new Map();
  }

  setup() {
    return this.harness.whenDefined(this.tagName);
  }

  mount(props = {}) {
    return this.harness.create(this.tagName, props);
  }

  unmount(element) {
    this.harness.destroy(element);
  }

  trigger(element, eventType, detail = {}) {
    const event = new CustomEvent(eventType, {
      bubbles: true,
      composed: true,
      detail,
    });
    element.dispatchEvent(event);
    this.events.push({ type: eventType, detail, timestamp: Date.now() });
    return event;
  }

  listen(element, eventType, handler) {
    const wrappedHandler = (e) => handler(e.detail);
    element.addEventListener(eventType, wrappedHandler);
    this.listeners.set(handler, { element, eventType, wrappedHandler });
  }

  getEvents(eventType) {
    return this.events.filter(e => e.type === eventType);
  }

  simulateInput(element, value) {
    element.value = value;
    this.trigger(element, 'input', { value });
    this.trigger(element, 'change', { value });
  }

  simulateClick(element) {
    element.click();
    this.trigger(element, 'click');
  }

  wait(ms = 0) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}

class MockShadowRoot {
  constructor() {
    this.innerHTML = '';
    this.adoptedStyleSheets = [];
    this.elements = new Set();
  }

  get mode() {
    return 'open';
  }

  get host() {
    return this._host;
  }

  set host(element) {
    this._host = element;
  }

  appendChild(node) {
    this.elements.add(node);
    if (node.tagName) {
      this.innerHTML += node.outerHTML;
    }
    return node;
  }

  insertBefore(node, ref) {
    this.elements.add(node);
    return node;
  }

  removeChild(node) {
    this.elements.delete(node);
    return node;
  }

  querySelector(selector) {
    for (const el of this.elements) {
      if (el.matches && el.matches(selector)) {
        return el;
      }
    }
    return null;
  }

  querySelectorAll(selector) {
    return Array.from(this.elements).filter(el => el.matches && el.matches(selector));
  }
}

class MockCustomElement {
  constructor(tagName) {
    this.tagName = tagName;
    this.attributes = new Map();
    this.children = [];
    this.shadowRoot = new MockShadowRoot();
    this.listeners = new Map();
    this.props = {};
  }

  static get observedAttributes() {
    return [];
  }

  static get version() {
    return '1.0.0';
  }

  get id() {
    return this.attributes.get('id')?.value;
  }

  set id(val) {
    this.attributes.set('id', { value: val });
  }

  get className() {
    return this.attributes.get('class')?.value;
  }

  set className(val) {
    this.attributes.set('class', { value: val });
  }

  getAttribute(name) {
    return this.attributes.get(name)?.value;
  }

  setAttribute(name, value) {
    this.attributes.set(name, { value: String(value) });
  }

  removeAttribute(name) {
    this.attributes.delete(name);
  }

  hasAttribute(name) {
    return this.attributes.has(name);
  }

  addEventListener(type, handler) {
    if (!this.listeners.has(type)) {
      this.listeners.set(type, new Set());
    }
    this.listeners.get(type).add(handler);
  }

  removeEventListener(type, handler) {
    const handlers = this.listeners.get(type);
    if (handlers) {
      handlers.delete(handler);
    }
  }

  dispatchEvent(event) {
    const handlers = this.listeners.get(event.type);
    if (handlers) {
      for (const handler of handlers) {
        handler(event);
      }
    }
    return true;
  }

  appendChild(node) {
    this.children.push(node);
  }

  removeChild(node) {
    const idx = this.children.indexOf(node);
    if (idx >= 0) {
      this.children.splice(idx, 1);
    }
    return node;
  }

  querySelector(selector) {
    return this.shadowRoot.querySelector(selector);
  }

  querySelectorAll(selector) {
    return this.shadowRoot.querySelectorAll(selector);
  }

  get innerHTML() {
    return this.shadowRoot.innerHTML;
  }

  set innerHTML(html) {
    this.shadowRoot.innerHTML = html;
  }
}

class CypressComponentCommands {
  constructor() {
    this.commands = {};
  }

  register() {
    if (typeof cy === 'undefined') return;

    cy.Commands.add('mountComponent', (tagName, props = {}) => {
      return cy.window().then(win => {
        const el = win.document.createElement(tagName);
        Object.assign(el, props);
        win.document.body.appendChild(el);
        return cy.wrap(el);
      });
    });

    cy.Commands.add('getShadow', { prevSubject: true }, (subject, selector) => {
      const shadow = subject.shadowRoot || subject.attachShadow;
      if (selector) {
        return cy.wrap(shadow.querySelector(selector));
      }
      return cy.wrap(shadow);
    });

    cy.Commands.add('dispatchEvent', { prevSubject: true }, (subject, eventType, detail) => {
      const event = new CustomEvent(eventType, { detail, bubbles: true });
      subject.dispatchEvent(event);
      return cy.wrap(subject);
    });

    cy.Commands.add('triggerSlotChange', { prevSubject: true }, (subject) => {
      subject.dispatchEvent(new CustomEvent('slotchange'));
      return cy.wrap(subject);
    });
  }
}

class JestMatchers {
  constructor() {
    this.matchers = {};
  }

  toHaveAttribute(element, name, value) {
    const actual = element.getAttribute(name);
    if (value !== undefined) {
      return actual === value;
    }
    return actual !== null;
  }

  toHaveShadow(element, selector) {
    const shadow = element.shadowRoot;
    if (!shadow) return false;
    if (selector) {
      return shadow.querySelector(selector) !== null;
    }
    return true;
  }

  toHaveSlot(element, slotName = '') {
    const slot = element.shadowRoot?.querySelector(`slot[name="${slotName}"]`);
    return slot !== null;
  }

  toEmit(element, eventType) {
    return this._events.some(e => e.type === eventType);
  }

  toHaveVersion(element, version) {
    return element.constructor.version === version;
  }
}

const testHarness = new TestHarness();
const cypressCommands = new CypressComponentCommands();
const jestMatchers = new JestMatchers();

export { TestHarness, ComponentTester, MockShadowRoot, MockCustomElement };
export { CypressComponentCommands, JestMatchers };
export { testHarness, cypressCommands, jestMatchers };

export default testHarness;