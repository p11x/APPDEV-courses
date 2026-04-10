# 🔄 Virtual DOM Concepts

## 📋 Overview

The Virtual DOM is a concept used by frameworks like React where a lightweight JavaScript representation of the real DOM is maintained. Understanding this helps when working with modern frameworks.

---

## 🏗️ How Virtual DOM Works

```javascript
// Simple Virtual Node representation
class VNode {
    constructor(tag, props, children) {
        this.tag = tag;
        this.props = props;
        this.children = children;
    }
}

// Create virtual nodes
const vNode = new VNode('div', { class: 'container' }, [
    new VNode('h1', {}, ['Hello']),
    new VNode('p', {}, ['World'])
]);
```

---

## 🎯 Reconciliation

```javascript
// Diff algorithm - compare old and new virtual trees
function reconcile(oldNode, newNode) {
    // If same node type, update props
    if (oldNode.tag === newNode.tag) {
        updateProps(oldNode, newNode);
    }
    
    // If different, replace entire subtree
    else {
        replaceNode(oldNode, newNode);
    }
}
```

---

## 🔗 Related Topics

- [11_DOM_Performance_Optimization.md](./11_DOM_Performance_Optimization.md)
- [13_Modern_DOM_APIs.md](./13_Modern_DOM_APIs.md)

---

**Next: Learn about [Modern DOM APIs](./13_Modern_DOM_APIs.md)**