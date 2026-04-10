# 🆕 Modern DOM APIs

## 📋 Overview

Modern browsers provide many powerful DOM APIs that make development easier. This guide covers APIs introduced in recent years.

---

## 🎯 New Element Methods

### replaceChildren

```javascript
const container = document.querySelector('#container');

// Replace all children at once
container.replaceChildren(
    document.createElement('div'),
    document.createElement('span')
);
```

### toggleAttribute

```javascript
const button = document.querySelector('button');

// Toggle boolean attributes
button.toggleAttribute('disabled');
button.toggleAttribute('readonly');
```

---

## 🎯 CSS.supports

```javascript
// Check if CSS feature is supported
if (CSS.supports('display: grid')) {
    // Use grid layout
}

if (CSS.supports('position: sticky')) {
    // Use sticky positioning
}
```

---

## 🎯 matchMedia

```javascript
// Check media query in JavaScript
const isMobile = window.matchMedia('(max-width: 768px)').matches;

// Listen for changes
const mq = window.matchMedia('(max-width: 768px)');
mq.addEventListener('change', (e) => {
    console.log('Screen is mobile:', e.matches);
});
```

---

## 🎯 Element.requestIdleCallback

```javascript
// Schedule non-critical work during idle time
function nonCriticalTask() {
    console.log('Doing non-critical work');
}

window.requestIdleCallback(nonCriticalTask, { timeout: 2000 });
```

---

## 🔗 Related Topics

- [12_Virtual_DOM_Concepts.md](./12_Virtual_DOM_Concepts.md)
- [11_DOM_Performance_Optimization.md](./11_DOM_Performance_Optimization.md)

---

**Next: Learn about [DOM Accessibility Best Practices](./14_DOM_Accessibility_Best_Practices.md)**