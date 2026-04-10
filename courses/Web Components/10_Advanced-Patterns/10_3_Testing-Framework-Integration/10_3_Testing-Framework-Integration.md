# Testing Framework Integration

## OVERVIEW

Testing Web Components requires special handling for Shadow DOM and custom elements. This guide covers unit tests, integration tests, and testing utilities.

## IMPLEMENTATION DETAILS

### Basic Testing

```javascript
import { test, expect } from '@open-wc/testing';

test('element renders', async () => {
  const el = document.createElement('test-element');
  document.body.appendChild(el);
  
  await el.updateComplete;
  
  expect(el.shadowRoot.querySelector('.content')).to.exist;
  expect(el.value).to.equal('test');
  
  el.remove();
});
```

### Lifecycle Testing

```javascript
test('connectedCallback fires', async () => {
  const el = document.createElement('test-element');
  
  let connected = false;
  el.addEventListener('connected', () => { connected = true; });
  
  document.body.appendChild(el);
  
  expect(connected).to.be.true;
  el.remove();
});
```

### Shadow DOM Testing

```javascript
test('shadow DOM content', async () => {
  const el = document.createElement('shadow-element');
  document.body.appendChild(el);
  
  const shadowContent = el.shadowRoot.querySelector('.inner');
  expect(shadowContent).to.exist;
  expect(shadowContent.textContent).to.equal('Content');
  
  el.remove();
});
```

## NEXT STEPS

Proceed to **10_Advanced-Patterns/10_4_Security-Best-Practices**.