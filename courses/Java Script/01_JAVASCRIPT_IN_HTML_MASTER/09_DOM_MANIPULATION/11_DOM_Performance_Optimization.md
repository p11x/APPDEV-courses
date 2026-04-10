# ⚡ DOM Performance Optimization

## 📋 Overview

DOM operations can be expensive. Understanding how to optimize them is crucial for building smooth, responsive web applications.

---

## 🎯 Common Performance Issues

### Layout Thrashing

```javascript
// ❌ Bad: Multiple reads after writes cause reflows
element.style.width = '100px';
console.log(element.offsetWidth); // Forces reflow!
element.style.height = '100px';
console.log(element.offsetHeight); // Forces reflow!
element.style.color = 'red';

// ✅ Good: Batch reads, then writes
const width = element.offsetWidth; // Read (cached)
element.style.width = '100px';      // Write
element.style.height = '100px';     // Write
element.style.color = 'red';        // Write
```

### Repeated DOM Queries

```javascript
// ❌ Bad: Query DOM repeatedly
function updateItems() {
    document.querySelectorAll('.item').forEach(item => {
        item.classList.add('processed');
    });
    // Called multiple times = repeated queries
}

// ✅ Good: Cache DOM references
const items = document.querySelectorAll('.item');
function updateItems() {
    items.forEach(item => item.classList.add('processed'));
}
```

---

## 🎯 Optimization Techniques

### DocumentFragment

```javascript
// ❌ Bad: Multiple reflows
const container = document.querySelector('#list');
for (let i = 0; i < 100; i++) {
    const li = document.createElement('li');
    li.textContent = `Item ${i}`;
    container.appendChild(li); // Reflow each time!
}

// ✅ Good: Single reflow with DocumentFragment
const fragment = document.createDocumentFragment();
for (let i = 0; i < 100; i++) {
    const li = document.createElement('li');
    li.textContent = `Item ${i}`;
    fragment.appendChild(li);
}
container.appendChild(fragment); // Single reflow!
```

### requestAnimationFrame

```javascript
// ❌ Bad: Synchronous updates in loop
function animate() {
    for (let i = 0; i < 1000; i++) {
        element.style.transform = `translateX(${i}px)`;
    }
}

// ✅ Good: Use requestAnimationFrame
function animate() {
    let position = 0;
    
    function step() {
        position += 5;
        element.style.transform = `translateX(${position}px)`;
        
        if (position < 1000) {
            requestAnimationFrame(step);
        }
    }
    
    requestAnimationFrame(step);
}
```

---

## 🎯 Best Practices Summary

| Technique | Benefit |
|-----------|---------|
| Cache DOM queries | Avoid repeated lookups |
| Batch reads, then writes | Minimize reflows |
| Use DocumentFragment | Single reflow for multiple elements |
| Use requestAnimationFrame | Sync with browser paint cycle |
| Use event delegation | Fewer listeners |

---

## 🔗 Related Topics

- [08_Event_Delegation_Patterns.md](./08_Event_Delegation_Patterns.md)
- [09_DOM_Traversal_Techniques.md](./09_DOM_Traversal_Techniques.md)

---

**Next: Learn about [DOM Accessibility Best Practices](./14_DOM_Accessibility_Best_Practices.md)**