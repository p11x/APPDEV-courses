# 🔍 DOM Traversal Techniques

## 📋 Overview

DOM traversal allows you to navigate between elements in the document tree. Understanding these methods is essential for building complex interactions.

---

## 🎯 Parent Traversal

### parentElement / parentNode

```javascript
const element = document.querySelector('.child');

// Get parent element
const parent = element.parentElement;

// Get parent node (can be text node)
const parentNode = element.parentNode;

// Navigate up multiple levels
const grandparent = element.parentElement.parentElement;
```

---

## 👶 Child Traversal

### children vs childNodes

```javascript
const container = document.querySelector('#container');

// Element children only (no text nodes)
const children = container.children;

// All child nodes (includes text, comments)
const childNodes = container.childNodes;

// Get specific child
const firstChild = container.firstElementChild;
const lastChild = container.lastElementChild;
```

---

## 🔄 Sibling Traversal

### Next/Previous Siblings

```javascript
const element = document.querySelector('.item');

// Next/Previous element siblings
const next = element.nextElementSibling;
const prev = element.previousElementSibling;

// All siblings (excluding self)
const siblings = [...element.parentElement.children]
    .filter(el => el !== element);
```

---

## 🎯 Query Methods

### querySelector with Traversal

```javascript
const container = document.querySelector('#container');

// Find ancestor
const ancestor = container.querySelector('.ancestor-class');

// Find descendant
const descendant = container.querySelector('.descendant-class');

// Find closest (goes up the tree)
const closest = container.closest('.parent-class');
```

---

## 🔗 Related Topics

- [01_DOM_Introduction.md](./01_DOM_Introduction.md)
- [02_DOM_Selectors_Complete_Reference.md](./02_DOM_Selectors_Complete_Reference.md)

---

**Next: Learn about [DOM Performance Optimization](./11_DOM_Performance_Optimization.md)**