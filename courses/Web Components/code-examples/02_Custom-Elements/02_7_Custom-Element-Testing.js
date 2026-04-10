/**
 * Custom Element Testing - Test Suite Examples
 * @description Comprehensive testing patterns for Web Components including unit, integration, and E2E tests
 * @module custom-elements/testing
 * @version 1.0.0
 * @example <test-runner></test-runner>
 */

// ============================================
// Test Runner Component
// ============================================

/**
 * TestRunner - Component for running and displaying test results
 * 
 * Features:
 * - Unit test execution
 * - Integration test support
 * - E2E test automation
 * - Coverage reporting
 * 
 * Props:
 * - testSuite: string - Name of test suite to run
 * - verbose: boolean - Enable verbose output
 * - coverage: boolean - Generate coverage reports
 * 
 * Events:
 * - test-start: Fired when tests begin
 * - test-complete: Fired when tests finish
 * - test-fail: Fired when a test fails
 * 
 * Slots:
 * - results: Test results display area
 */
class TestRunner extends HTMLElement {
  static get observedAttributes() {
    return ['test-suite', 'verbose', 'coverage'];
  }

  static get is() {
    return 'test-runner';
  }

  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
    this._tests = [];
    this._results = {
      passed: 0,
      failed: 0,
      skipped: 0,
      total: 0,
      duration: 0,
      tests: []
    };
    this._isRunning = false;
    this._verbose = false;
    this._coverage = false;
  }

  connectedCallback() {
    this._render();
    this._setupEventListeners();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;
    
    switch (name) {
      case 'test-suite':
        this._testSuite = newValue;
        break;
      case 'verbose':
        this._verbose = newValue !== null;
        break;
      case 'coverage':
        this._coverage = newValue !== null;
        break;
    }
  }

  get testSuite() {
    return this._testSuite;
  }

  set testSuite(value) {
    this._testSuite = value;
    if (value) {
      this.setAttribute('test-suite', value);
    } else {
      this.removeAttribute('test-suite');
    }
  }

  get verbose() {
    return this._verbose;
  }

  set verbose(value) {
    this._verbose = Boolean(value);
    if (value) {
      this.setAttribute('verbose', '');
    } else {
      this.removeAttribute('verbose');
    }
  }

  get coverage() {
    return this._coverage;
  }

  set coverage(value) {
    this._coverage = Boolean(value);
    if (value) {
      this.setAttribute('coverage', '');
    } else {
      this.removeAttribute('coverage');
    }
  }

  /**
   * Register a test function
   * @param {string} name - Test name
   * @param {Function} fn - Test function
   * @param {Object} options - Test options
   */
  registerTest(name, fn, options = {}) {
    this._tests.push({ name, fn, options });
  }

  /**
   * Run all registered tests
   * @returns {Promise<Object>} Test results
   */
  async runTests() {
    if (this._isRunning) {
      throw new Error('Tests are already running');
    }

    this._isRunning = true;
    this._results = {
      passed: 0,
      failed: 0,
      skipped: 0,
      total: this._tests.length,
      duration: 0,
      tests: []
    };

    const startTime = performance.now();
    this.dispatchEvent(new CustomEvent('test-start', {
      bubbles: true,
      composed: true,
      detail: { total: this._tests.length }
    }));

    for (const test of this._tests) {
      const result = await this._runSingleTest(test);
      this._results.tests.push(result);
      
      if (result.status === 'passed') {
        this._results.passed++;
      } else if (result.status === 'failed') {
        this._results.failed++;
      } else {
        this._results.skipped++;
      }

      this._updateProgress();
    }

    this._results.duration = performance.now() - startTime;
    this._isRunning = false;

    this.dispatchEvent(new CustomEvent('test-complete', {
      bubbles: true,
      composed: true,
      detail: this._results
    }));

    return this._results;
  }

  async _runSingleTest(test) {
    const startTime = performance.now();
    const result = {
      name: test.name,
      status: 'pending',
      duration: 0,
      error: null,
      stack: null
    };

    try {
      if (test.options.skip) {
        result.status = 'skipped';
      } else {
        await test.fn();
        result.status = 'passed';
      }
    } catch (error) {
      result.status = 'failed';
      result.error = error.message;
      result.stack = error.stack;
      
      this.dispatchEvent(new CustomEvent('test-fail', {
        bubbles: true,
        composed: true,
        detail: { test: test.name, error }
      }));
    }

    result.duration = performance.now() - startTime;

    if (this._verbose) {
      console.log(`[${result.status}] ${test.name} (${result.duration.toFixed(2)}ms)`);
    }

    return result;
  }

  _updateProgress() {
    const progress = this.shadowRoot.querySelector('.progress');
    if (progress) {
      const percentage = (this._results.tests.length / this._results.total) * 100;
      progress.style.width = `${percentage}%`;
    }

    this._renderResults();
  }

  _setupEventListeners() {
    this.addEventListener('click', (e) => {
      if (e.target.classList.contains('run-button')) {
        this.runTests();
      }
      if (e.target.classList.contains('clear-button')) {
        this._clearResults();
      }
    });
  }

  _clearResults() {
    this._results = {
      passed: 0,
      failed: 0,
      skipped: 0,
      total: 0,
      duration: 0,
      tests: []
    };
    this._render();
  }

  _render() {
    this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
          --primary-color: #2563eb;
          --success-color: #22c55e;
          --error-color: #ef4444;
          --warning-color: #f59e0b;
          --bg-color: #ffffff;
          --border-color: #e5e7eb;
          --text-color: #1f2937;
        }

        .container {
          border: 1px solid var(--border-color);
          border-radius: 8px;
          overflow: hidden;
          background: var(--bg-color);
        }

        .header {
          display: flex;
          justify-content: space-between;
          align-items: center;
          padding: 16px;
          border-bottom: 1px solid var(--border-color);
          background: #f9fafb;
        }

        .title {
          font-size: 18px;
          font-weight: 600;
          color: var(--text-color);
          margin: 0;
        }

        .controls {
          display: flex;
          gap: 8px;
        }

        button {
          padding: 8px 16px;
          border: none;
          border-radius: 4px;
          font-size: 14px;
          cursor: pointer;
          transition: all 0.2s;
        }

        .run-button {
          background: var(--primary-color);
          color: white;
        }

        .run-button:hover {
          background: #1d4ed8;
        }

        .clear-button {
          background: var(--border-color);
          color: var(--text-color);
        }

        .clear-button:hover {
          background: #d1d5db;
        }

        .progress-bar {
          height: 4px;
          background: var(--border-color);
          overflow: hidden;
        }

        .progress {
          height: 100%;
          background: var(--primary-color);
          transition: width 0.3s ease;
          width: 0%;
        }

        .results {
          padding: 16px;
          max-height: 400px;
          overflow-y: auto;
        }

        .test-item {
          padding: 12px;
          margin-bottom: 8px;
          border-radius: 4px;
          background: #f9fafb;
        }

        .test-item.passed {
          border-left: 4px solid var(--success-color);
        }

        .test-item.failed {
          border-left: 4px solid var(--error-color);
        }

        .test-item.skipped {
          border-left: 4px solid var(--warning-color);
        }

        .test-name {
          font-weight: 500;
          color: var(--text-color);
        }

        .test-duration {
          font-size: 12px;
          color: #6b7280;
          margin-top: 4px;
        }

        .test-error {
          font-size: 12px;
          color: var(--error-color);
          margin-top: 8px;
          padding: 8px;
          background: #fef2f2;
          border-radius: 4px;
          font-family: monospace;
        }

        .summary {
          display: grid;
          grid-template-columns: repeat(4, 1fr);
          gap: 16px;
          padding: 16px;
          border-top: 1px solid var(--border-color);
          background: #f9fafb;
        }

        .summary-item {
          text-align: center;
        }

        .summary-value {
          font-size: 24px;
          font-weight: 600;
        }

        .summary-label {
          font-size: 12px;
          color: #6b7280;
          text-transform: uppercase;
        }

        .passed .summary-value {
          color: var(--success-color);
        }

        .failed .summary-value {
          color: var(--error-color);
        }
      </style>

      <div class="container">
        <div class="header">
          <h2 class="title">Test Runner</h2>
          <div class="controls">
            <button class="run-button">Run Tests</button>
            <button class="clear-button">Clear</button>
          </div>
        </div>
        <div class="progress-bar">
          <div class="progress"></div>
        </div>
        <div class="results">
          <slot name="results"></slot>
        </div>
        <div class="summary">
          <div class="summary-item passed">
            <div class="summary-value">${this._results.passed}</div>
            <div class="summary-label">Passed</div>
          </div>
          <div class="summary-item failed">
            <div class="summary-value">${this._results.failed}</div>
            <div class="summary-label">Failed</div>
          </div>
          <div class="summary-item">
            <div class="summary-value">${this._results.skipped}</div>
            <div class="summary-label">Skipped</div>
          </div>
          <div class="summary-item">
            <div class="summary-value">${this._results.duration.toFixed(2)}ms</div>
            <div class="summary-label">Duration</div>
          </div>
        </div>
      </div>
    `;
  }

  _renderResults() {
    const resultsContainer = this.shadowRoot.querySelector('.results');
    if (!resultsContainer) return;

    const html = this._results.tests.map(test => `
      <div class="test-item ${test.status}">
        <div class="test-name">${test.name}</div>
        <div class="test-duration">${test.duration.toFixed(2)}ms</div>
        ${test.error ? `<div class="test-error">${test.error}</div>` : ''}
      </div>
    `).join('');

    const slot = this.shadowRoot.querySelector('slot[name="results"]');
    if (slot) {
      slot.innerHTML = html;
    }
  }
}

customElements.define(TestRunner.is, TestRunner);

// ============================================
// Mock Element Factory
// ============================================

/**
 * Factory for creating mock Web Components for testing
 */
class MockElementFactory {
  /**
   * Create a mock custom element for testing
   * @param {string} tagName - Custom element tag name
   * @param {Object} options - Mock options
   * @returns {HTMLElement} Mock element
   */
  static createMockElement(tagName, options = {}) {
    const mock = document.createElement(tagName);
    
    if (options.attributes) {
      for (const [key, value] of Object.entries(options.attributes)) {
        mock.setAttribute(key, value);
      }
    }

    if (options.properties) {
      for (const [key, value] of Object.entries(options.properties)) {
        mock[key] = value;
      }
    }

    if (options.children) {
      mock.innerHTML = options.children;
    }

    return mock;
  }

  /**
   * Create a mock Shadow DOM for testing
   * @param {string} innerHTML - Inner HTML content
   * @returns {ShadowRoot} Mock shadow root
   */
  static createMockShadowRoot(innerHTML = '') {
    const container = document.createElement('div');
    container.attachShadow({ mode: 'open' });
    container.shadowRoot.innerHTML = innerHTML;
    return container.shadowRoot;
  }

  /**
   * Create mock custom element with lifecycle tracking
   * @param {string} tagName - Tag name
   * @returns {Object} Mock element with lifecycle hooks
   */
  static createLifecycleMock(tagName) {
    const lifecycle = {
      connected: false,
      disconnected: false,
      adopted: false,
      attributeChanged: [],
      capturedEvents: []
    };

    class LifecycleMock extends HTMLElement {
      static get observedAttributes() {
        return ['data-test', 'data-value'];
      }

      constructor() {
        super();
        this.attachShadow({ mode: 'open' });
      }

      connectedCallback() {
        lifecycle.connected = true;
        this._dispatchLifecycleEvent('connected');
      }

      disconnectedCallback() {
        lifecycle.disconnected = true;
        this._dispatchLifecycleEvent('disconnected');
      }

      adoptedCallback() {
        lifecycle.adopted = true;
        this._dispatchLifecycleEvent('adopted');
      }

      attributeChangedCallback(name, oldValue, newValue) {
        lifecycle.attributeChanged.push({ name, oldValue, newValue });
        this._dispatchLifecycleEvent('attributeChanged', { name, oldValue, newValue });
      }

      _dispatchLifecycleEvent(type, detail = {}) {
        this.dispatchEvent(new CustomEvent(`lifecycle:${type}`, {
          bubbles: true,
          composed: true,
          detail
        }));
      }
    }

    customElements.define(tagName, LifecycleMock);
    return { elementClass: LifecycleMock, lifecycle };
  }

  /**
   * Create a mock HTML element with specified behavior
   * @param {string} tagName - Tag name to mock
   * @param {Object} behavior - Behavior configuration
   * @returns {HTMLElement} Configured mock element
   */
  static createMockWithBehavior(tagName, behavior = {}) {
    const mock = document.createElement(tagName);
    
    if (behavior.click) {
      mock.click = behavior.click;
    }

    if (behavior.focus) {
      mock.focus = behavior.focus;
    }

    if (behavior.blur) {
      mock.blur = behavior.blur;
    }

    if (behavior.addEventListener) {
      mock.addEventListener = behavior.addEventListener;
    }

    if (behavior.removeEventListener) {
      mock.removeEventListener = behavior.removeEventListener;
    }

    if (behavior.dispatchEvent) {
      mock.dispatchEvent = behavior.dispatchEvent;
    }

    return mock;
  }

  /**
   * Create a mock event for testing
   * @param {string} type - Event type
   * @param {Object} options - Event options
   * @returns {Event} Mock event
   */
  static createMockEvent(type, options = {}) {
    return new CustomEvent(type, {
      bubbles: options.bubbles ?? true,
      cancelable: options.cancelable ?? false,
      composed: options.composed ?? true,
      detail: options.detail ?? {}
    });
  }

  /**
   * Create a mock document fragment
   * @param {string} html - HTML content
   * @returns {DocumentFragment} Mock fragment
   */
  static createMockFragment(html) {
    const template = document.createElement('template');
    template.innerHTML = html;
    return template.content.cloneNode(true);
  }
}

// ============================================
// Test Utilities
// ============================================

/**
 * Utility functions for testing Web Components
 */
const TestUtils = {
  /**
   * Wait for a condition to be true
   * @param {Function} condition - Condition function
   * @param {number} timeout - Timeout in milliseconds
   * @returns {Promise<boolean>} Whether condition was met
   */
  waitFor(condition, timeout = 1000) {
    return new Promise((resolve, reject) => {
      const startTime = Date.now();
      
      const check = () => {
        if (condition()) {
          resolve(true);
          return;
        }
        
        if (Date.now() - startTime >= timeout) {
          reject(new Error(`Timeout waiting for condition (${timeout}ms)`));
          return;
        }
        
        requestAnimationFrame(check);
      };
      
      check();
    });
  },

  /**
   * Wait for element to be defined
   * @param {string} tagName - Custom element tag name
   * @param {number} timeout - Timeout in milliseconds
   * @returns {Promise<HTMLElement>} Defined element
   */
  waitForDefinition(tagName, timeout = 1000) {
    return new Promise((resolve, reject) => {
      const startTime = Date.now();
      
      const check = () => {
        if (customElements.get(tagName)) {
          resolve(customElements.get(tagName));
          return;
        }
        
        if (Date.now() - startTime >= timeout) {
          reject(new Error(`Timeout waiting for custom element definition: ${tagName}`));
          return;
        }
        
        requestAnimationFrame(check);
      };
      
      check();
    });
  },

  /**
   * Wait for DOM updates
   * @param {number} timeout - Timeout in milliseconds
   * @returns {Promise<void>}
   */
  waitForDOMUpdate(timeout = 100) {
    return new Promise((resolve) => {
      setTimeout(resolve, timeout);
    });
  },

  /**
   * Flush all pending mutations
   * @returns {Promise<void>}
   */
  async flushMutations() {
    await new Promise(resolve => {
      if (typeof MutationObserver !== 'undefined') {
        const observer = new MutationObserver(resolve);
        observer.observe(document.body, { childList: true, subtree: true });
        requestAnimationFrame(() => {
          document.body.appendChild(document.createElement('div'));
        });
      } else {
        resolve();
      }
    });
  },

  /**
   * Simulate keyboard event
   * @param {HTMLElement} element - Target element
   * @param {string} key - Key to press
   * @param {Object} options - Event options
   */
  simulateKeyboard(element, key, options = {}) {
    const eventInit = {
      key,
      code: options.code ?? key,
      keyCode: options.keyCode ?? key.charCodeAt(0),
     bubbles: true,
      cancelable: true,
      ...options
    };
    
    element.dispatchEvent(new KeyboardEvent('keydown', eventInit));
    element.dispatchEvent(new KeyboardEvent('keypress', eventInit));
    element.dispatchEvent(new KeyboardEvent('keyup', eventInit));
  },

  /**
   * Simulate mouse event
   * @param {HTMLElement} element - Target element
   * @param {string} type - Event type
   * @param {Object} options - Event options
   */
  simulateMouseEvent(element, type, options = {}) {
    const rect = element.getBoundingClientRect();
    const eventInit = {
      type,
      bubbles: true,
      cancelable: true,
      view: window,
      clientX: options.x ?? rect.left + rect.width / 2,
      clientY: options.y ?? rect.top + rect.height / 2,
      screenX: options.screenX ?? rect.left,
      screenY: options.screenY ?? rect.top,
      ctrlKey: options.ctrlKey ?? false,
      altKey: options.altKey ?? false,
      shiftKey: options.shiftKey ?? false,
      metaKey: options.metaKey ?? false,
      button: options.button ?? 0,
      buttons: options.buttons ?? 1,
      ...options
    };
    
    element.dispatchEvent(new MouseEvent(type, eventInit));
  },

  /**
   * Simulate drag and drop
   * @param {HTMLElement} source - Source element
   * @param {HTMLElement} target - Target element
   */
  simulateDragAndDrop(source, target) {
    const dataTransfer = new DataTransfer();
    
    source.dispatchEvent(new DragEvent('dragstart', {
      bubbles: true,
      cancelable: true,
      dataTransfer
    }));

    const rect = target.getBoundingClientRect();
    target.dispatchEvent(new DragEvent('dragenter', {
      bubbles: true,
      cancelable: true,
      dataTransfer,
      clientX: rect.left + rect.width / 2,
      clientY: rect.top + rect.height / 2
    }));

    target.dispatchEvent(new DragEvent('dragover', {
      bubbles: true,
      cancelable: true,
      dataTransfer,
      clientX: rect.left + rect.width / 2,
      clientY: rect.top + rect.height / 2
    }));

    target.dispatchEvent(new DragEvent('drop', {
      bubbles: true,
      cancelable: true,
      dataTransfer,
      clientX: rect.left + rect.width / 2,
      clientY: rect.top + rect.height / 2
    }));

    source.dispatchEvent(new DragEvent('dragend', {
      bubbles: true,
      cancelable: true,
      dataTransfer
    }));
  },

  /**
   * Mock fetch response
   * @param {Object} data - Response data
   * @param {number} status - HTTP status
   * @returns {Response} Mocked response
   */
  mockFetchResponse(data, status = 200) {
    return {
      ok: status >= 200 && status < 300,
      status,
      statusText: status === 200 ? 'OK' : 'Error',
      json: () => Promise.resolve(data),
      text: () => Promise.resolve(JSON.stringify(data)),
      blob: () => Promise.resolve(new Blob([JSON.stringify(data)])),
      headers: new Map([['content-type', 'application/json']])
    };
  },

  /**
   * Create sandboxed iframe for testing
   * @returns {HTMLIFrameElement} Sandboxed iframe
   */
  createSandboxedIframe() {
    const iframe = document.createElement('iframe');
    iframe.sandbox.add('allow-scripts');
    iframe.sandbox.add('allow-same-origin');
    return iframe;
  },

  /**
   * Generate unique test ID
   * @returns {string} Unique ID
   */
  generateTestId() {
    return `test-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  },

  /**
   * Deep clone object for testing
   * @param {Object} obj - Object to clone
   * @returns {Object} Cloned object
   */
  deepClone(obj) {
    return JSON.parse(JSON.stringify(obj));
  },

  /**
   * Compare DOM structures
   * @param {HTMLElement} element1 - First element
   * @param {HTMLElement} element2 - Second element
   * @returns {boolean} Whether structures match
   */
  compareDOM(element1, element2) {
    if (element1.nodeType !== element2.nodeType) return false;
    if (element1.nodeName !== element2.nodeName) return false;
    if (element1.attributes?.length !== element2.attributes?.length) return false;
    
    for (let i = 0; i < element1.attributes?.length; i++) {
      const attr1 = element1.attributes[i];
      const attr2 = element2.attributes[attr1.name];
      if (attr1.value !== attr2?.value) return false;
    }

    if (element1.childNodes.length !== element2.childNodes.length) return false;

    for (let i = 0; i < element1.childNodes.length; i++) {
      if (!TestUtils.compareDOM(element1.childNodes[i], element2.childNodes[i])) {
        return false;
      }
    }

    return true;
  },

  /**
   * Get element text content recursively
   * @param {HTMLElement} element - Element to get text from
   * @returns {string} All text content
   */
  getTextContent(element) {
    return element.textContent?.trim() ?? '';
  },

  /**
   * Find elements by shadow DOM boundary
   * @param {HTMLElement} root - Root element
   * @param {string} selector - CSS selector
   * @returns {Array<Element>} Found elements
   */
  findInShadowDOM(root, selector) {
    const results = [];
    
    const search = (element) => {
      if (element.matches?.(selector)) {
        results.push(element);
      }
      
      if (element.shadowRoot) {
        search(element.shadowRoot);
      }
      
      for (const child of element.children ?? []) {
        search(child);
      }
    };
    
    search(root);
    return results;
  },

  /**
   * Assert element has expected properties
   * @param {HTMLElement} element - Element to check
   * @param {Object} expected - Expected properties
   * @throws {Error} If assertion fails
   */
  assertProperties(element, expected) {
    for (const [key, value] of Object.entries(expected)) {
      if (element[key] !== value) {
        throw new Error(`Property "${key}" mismatch: expected ${value}, got ${element[key]}`);
      }
    }
  },

  /**
   * Assert element has expected attributes
   * @param {HTMLElement} element - Element to check
   * @param {Object} expected - Expected attributes
   * @throws {Error} If assertion fails
   */
  assertAttributes(element, expected) {
    for (const [key, value] of Object.entries(expected)) {
      if (element.getAttribute(key) !== value) {
        throw new Error(`Attribute "${key}" mismatch: expected ${value}, got ${element.getAttribute(key)}`);
      }
    }
  },

  /**
   * Mock intersection observer
   * @param {Function} callback - Observer callback
   * @returns {Object} Mock observer
   */
  mockIntersectionObserver(callback) {
    return {
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
      trigger: (entries) => callback(entries, this)
    };
  },

  /**
   * Mock resize observer
   * @param {Function} callback - Observer callback
   * @returns {Object} Mock observer
   */
  mockResizeObserver(callback) {
    return {
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn(),
      trigger: (entries) => callback(entries, this)
    };
  },

  /**
   * Create mock mutation observer
   * @param {Function} callback - Observer callback
   * @returns {Object} Mock observer with trigger
   */
  mockMutationObserver(callback) {
    return {
      observe: jest.fn(),
      disconnect: jest.fn(),
      trigger: (mutations) => callback(mutations, this)
    };
  },

  /**
   * Mock performance entries
   * @param {Array} entries - Performance entries
   */
  mockPerformanceEntries(entries) {
    jest.spyOn(performance, 'getEntriesByType').mockReturnValue(entries);
    jest.spyOn(performance, 'getEntriesByName').mockReturnValue(entries);
  },

  /**
   * Create test fixture
   * @param {string} tagName - Element tag name
   * @param {Object} options - Fixture options
   * @returns {HTMLElement} Test element
   */
  createTestFixture(tagName, options = {}) {
    const element = document.createElement(tagName);
    
    if (options.attributes) {
      for (const [key, value] of Object.entries(options.attributes)) {
        element.setAttribute(key, value);
      }
    }

    if (options.properties) {
      Object.assign(element, options.properties);
    }

    document.body.appendChild(element);
    
    if (options.immediate !== false) {
      return element;
    }
    
    return new Promise(resolve => {
      requestAnimationFrame(() => resolve(element));
    });
  },

  /**
   * Cleanup test fixture
   * @param {HTMLElement} element - Element to remove
   */
  cleanupFixture(element) {
    if (element && element.parentNode) {
      element.parentNode.removeChild(element);
    }
  }
};

// ============================================
// Export
// ============================================

export { TestRunner, MockElementFactory, TestUtils };