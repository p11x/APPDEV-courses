# Attribute Change Detection

## OVERVIEW

Attribute change detection enables components to respond to HTML attribute changes through the attributeChangedCallback. This guide covers detection patterns, optimization, and handling various attribute types.

## IMPLEMENTATION DETAILS

### Basic Detection

```javascript
class DetectingElement extends HTMLElement {
  // MUST declare observed attributes
  static get observedAttributes() {
    return ['title', 'variant', 'disabled'];
  }
  
  // Called when any observed attribute changes
  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue === newValue) return;  // Skip if unchanged
    
    console.log(`${name} changed from ${oldValue} to ${newValue}`);
    this.handleAttributeChange(name, oldValue, newValue);
  }
  
  handleAttributeChange(name, oldVal, newVal) {
    switch (name) {
      case 'title':
        this.updateTitle(newVal);
        break;
      case 'variant':
        this.updateVariant(newVal);
        break;
      case 'disabled':
        this.updateDisabled(newVal !== null);
        break;
    }
  }
}
```

### Advanced Detection

```javascript
class AdvancedDetection extends HTMLElement {
  // Regex patterns for data-* attributes
  static get observedAttributes() {
    return ['value', 'data-id', 'data-items', 'data-config', 'disabled'];
  }
  
  attributeChangedCallback(name, oldValue, newValue) {
    // Debounce rapid changes
    clearTimeout(this.#debounceTimer);
    this.#debounceTimer = setTimeout(() => {
      this.#processAttributeChange(name, oldValue, newValue);
    }, 50);
  }
  
  #debounceTimer = null;
  
  #processAttributeChange(name, oldVal, newVal) {
    if (name.startsWith('data-')) {
      this.#handleDataAttribute(name, newVal);
    } else {
      this.#handleStandardAttribute(name, newVal);
    }
  }
  
  #handleDataAttribute(name, value) {
    if (name === 'data-items') {
      try {
        const items = JSON.parse(value);
        this.renderItems(items);
      } catch (e) { /* invalid JSON */ }
    }
  }
}
```

## NEXT STEPS

Proceed to **05_Data-Binding/05_3_Data-Flow-Architecture**.