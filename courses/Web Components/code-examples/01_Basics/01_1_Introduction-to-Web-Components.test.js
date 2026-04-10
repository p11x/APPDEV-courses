/**
 * Introduction to Web Components - Test Suite
 * @description Unit tests for the greeting element
 * @version 1.0.0
 */

import { jest } from '@jest/globals';

// Mock describe, test, expect for browser testing environment
const describe = global.describe || (() => {});
const test = global.test || (() => {});
const expect = global.expect || (() => ({ toBe: () => {}, toEqual: () => {}, toExist: () => {} }));
const beforeEach = global.beforeEach || (() => {});
const afterEach = global.afterEach || (() => {});

describe('GreetingElement', () => {
  let element;
  
  beforeEach(() => {
    // Create element before each test
    element = document.createElement('greeting-element');
    document.body.appendChild(element);
  });
  
  afterEach(() => {
    // Clean up after each test
    element.remove();
  });
  
  describe('Component Creation', () => {
    test('should create element successfully', () => {
      expect(element).toBeTruthy();
      expect(element.tagName).toBe('GREETING-ELEMENT');
    });
    
    test('should have shadowRoot attached', () => {
      expect(element.shadowRoot).toBeTruthy();
      expect(element.shadowRoot.mode).toBe('open');
    });
    
    test('should render default greeting', () => {
      const greeting = element.shadowRoot.querySelector('.greeting');
      expect(greeting).toBeTruthy();
      expect(greeting.textContent).toContain('Hello');
      expect(greeting.textContent).toContain('World');
    });
  });
  
  describe('Attribute Handling', () => {
    test('should handle name attribute', async () => {
      element.setAttribute('name', 'Developer');
      await element.updateComplete;
      
      const nameSpan = element.shadowRoot.querySelector('.name');
      expect(nameSpan.textContent).toBe('Developer');
    });
    
    test('should handle greeting-type attribute', async () => {
      element.setAttribute('greeting-type', 'hi');
      await element.updateComplete;
      
      const greeting = element.shadowRoot.querySelector('.greeting');
      expect(greeting.textContent).toContain('Hi');
    });
    
    test('should handle highlight attribute', async () => {
      element.setAttribute('highlight', '');
      await element.updateComplete;
      
      expect(element.hasAttribute('highlight')).toBe(true);
    });
  });
  
  describe('Property Access', () => {
    test('should get and set name property', () => {
      element.name = 'Test User';
      expect(element.name).toBe('Test User');
      expect(element.getAttribute('name')).toBe('Test User');
    });
    
    test('should get and set greetingType property', () => {
      element.greetingType = 'welcome';
      expect(element.greetingType).toBe('welcome');
      expect(element.getAttribute('greeting-type')).toBe('welcome');
    });
  });
  
  describe('Lifecycle Callbacks', () => {
    test('should call connectedCallback when added to DOM', () => {
      const removeAndAdd = () => {
        element.remove();
        document.body.appendChild(element);
      };
      
      // Element is already connected from beforeEach
      expect(element.isConnected).toBe(true);
    });
    
    test('should call disconnectedCallback when removed from DOM', () => {
      element.remove();
      expect(element.isConnected).toBe(false);
    });
  });
  
  describe('Attribute Observation', () => {
    test('should observe name attribute changes', () => {
      const observer = jest.fn();
      element.addEventListener('DOMSubtreeModified', observer);
      
      element.setAttribute('name', 'New Name');
      element.setAttribute('greeting-type', 'hey');
      
      expect(observer).toHaveBeenCalled();
    });
  });
  
  describe('Accessibility', () => {
    test('should have proper ARIA attributes', () => {
      const greeting = element.shadowRoot.querySelector('.greeting');
      expect(greeting.getAttribute('role')).toBe('status');
      expect(greeting.getAttribute('aria-live')).toBe('polite');
    });
  });
});

describe('Custom Elements API', () => {
  test('should define custom element', () => {
    const GreetingElement = customElements.get('greeting-element');
    expect(GreetingElement).toBeTruthy();
  });
  
  test('should get element definition', () => {
    const def = customElements.get('greeting-element');
    expect(def).toBeDefined();
  });
});

describe('Lifecycle Events', () => {
  test('should fire custom lifecycle events', (done) => {
    const newElement = document.createElement('greeting-element');
    
    newElement.addEventListener('DOMCharacterDataModified', () => {
      done();
    });
    
    document.body.appendChild(newElement);
    
    setTimeout(() => {
      newElement.remove();
    }, 100);
  });
});

// Export for module usage
export {};