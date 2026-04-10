# ⏱️ Timeout and Debouncing

## 📋 Overview

Timeout and debouncing are essential techniques for controlling async behavior, especially for handling user input, network requests, and rate limiting.

---

## ⏳ setTimeout

### Basic Usage

```javascript
// Execute after delay
setTimeout(() => {
    console.log('Executed after 1 second');
}, 1000);

// With parameters
function greet(name, greeting) {
    console.log(`${greeting}, ${name}!`);
}

setTimeout(greet, 1000, 'John', 'Hello');
```

### Canceling Timeout

```javascript
const timerId = setTimeout(() => {
    console.log('This will not run');
}, 5000);

// Cancel before it executes
clearTimeout(timerId);
```

---

## 🎯 Debouncing

### What is Debouncing?

Debouncing ensures a function is only called after a specified wait period has elapsed since the last call. Useful for search inputs, window resizing, etc.

### Implementation

```javascript
function debounce(func, wait) {
    let timeout;
    
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Usage
const searchInput = document.querySelector('#search');

const handleSearch = debounce((query) => {
    console.log('Searching for:', query);
    // API call here
}, 300);

searchInput.addEventListener('input', (e) => {
    handleSearch(e.target.value);
});
```

### Leading/Trailing Debounce

```javascript
function debounce(func, wait, options = {}) {
    let timeout;
    
    return function executedFunction(...args) {
        const context = this;
        
        const later = () => {
            timeout = null;
            if (!options.leading) func.apply(context, args);
        };
        
        const callNow = options.leading && !timeout;
        
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        
        if (callNow) func.apply(context, args);
    };
}

// Leading edge (fires immediately on first call)
const onLeading = debounce(handler, 300, { leading: true });
```

---

## 🌍 Real-World Examples

### Search Autocomplete

```javascript
class SearchAutocomplete {
    constructor(inputElement) {
        this.input = inputElement;
        this.setupDebounce();
    }
    
    setupDebounce() {
        this.handleInput = debounce((query) => {
            if (query.length < 2) return;
            this.fetchSuggestions(query);
        }, 300);
        
        this.input.addEventListener('input', (e) => {
            this.handleInput(e.target.value);
        });
    }
    
    async fetchSuggestions(query) {
        try {
            const response = await fetch(
                `/api/search?q=${encodeURIComponent(query)}`
            );
            const suggestions = await response.json();
            this.displaySuggestions(suggestions);
        } catch (error) {
            console.error('Search error:', error);
        }
    }
    
    displaySuggestions(suggestions) {
        // Render suggestions
    }
}
```

### Window Resize Handler

```javascript
// Debounce resize events
const handleResize = debounce(() => {
    console.log('Window resized:', window.innerWidth, window.innerHeight);
    // Recalculate layout
}, 250);

window.addEventListener('resize', handleResize);
```

---

## 🔗 Related Topics

- [03_Async_Await_Master_Class.md](./03_Async_Await_Master_Class.md)
- [09_Throttling_and_Performance.md](./09_Throttling_and_Performance.md)

---

**Next: Learn about [Throttling and Performance](./09_Throttling_and_Performance.md)**