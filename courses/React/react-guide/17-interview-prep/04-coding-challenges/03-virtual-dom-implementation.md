# Virtual DOM Implementation

## Overview
Understanding how Virtual DOM works by implementing a simplified version helps you understand React's core principles.

## Prerequisites
- JavaScript fundamentals
- DOM knowledge

## Core Concepts

### What is Virtual DOM?

The Virtual DOM is a JavaScript representation of the real DOM. React creates a virtual tree, compares it with the previous version, and only updates what's changed in the real DOM.

### Simple Implementation

```typescript
// [File: src/vdom/virtualDom.ts]
// VNode represents a virtual DOM node
interface VNode {
  type: string;
  props: Record<string, any>;
  children: VNode[];
}

// Create a virtual node
function createElement(
  type: string,
  props: Record<string, any>,
  ...children: VNode[]
): VNode {
  return {
    type,
    props: props || {},
    children: children.flat()
  };
}

// Render virtual DOM to real DOM
function render(vnode: VNode): HTMLElement | Text {
  // Handle text nodes
  if (typeof vnode === 'string' || typeof vnode === 'number') {
    return document.createTextNode(String(vnode));
  }

  // Create element
  const element = document.createElement(vnode.type);

  // Set props (except children)
  Object.entries(vnode.props).forEach(([key, value]) => {
    if (key === 'className') {
      element.className = value;
    } else if (key.startsWith('on')) {
      const event = key.slice(2).toLowerCase();
      element.addEventListener(event, value);
    } else {
      element.setAttribute(key, value);
    }
  });

  // Render children recursively
  vnode.children.forEach(child => {
    element.appendChild(render(child));
  });

  return element;
}
```

### Diffing Algorithm

```typescript
// [File: src/vdom/diff.ts]
import { render } from './virtualDom';

interface VNode {
  type: string;
  props: Record<string, any>;
  children: VNode[];
  key?: string;
}

// Compare two virtual nodes
function diff(oldVNode: VNode, newVNode: VNode): any {
  // Different types = replace entirely
  if (oldVNode.type !== newVNode.type) {
    return newVNode;
  }

  // Same type = update props
  const updatedProps = diffProps(oldVNode.props, newVNode.props);

  // Recursively diff children
  const updatedChildren = diffChildren(oldVNode.children, newVNode.children);

  // Return updated node if anything changed
  if (Object.keys(updatedProps).length > 0 || updatedChildren.some(Boolean)) {
    return { ...newVNode, props: updatedProps, children: updatedChildren };
  }

  return oldVNode;
}

function diffProps(oldProps: any, newProps: any): any {
  const updates: any = {};

  // Add or update new props
  Object.keys(newProps).forEach(key => {
    if (oldProps[key] !== newProps[key]) {
      updates[key] = newProps[key];
    }
  });

  // Remove old props
  Object.keys(oldProps).forEach(key => {
    if (!newProps[key]) {
      updates[key] = null;
    }
  });

  return updates;
}

function diffChildren(oldChildren: VNode[], newChildren: VNode[]): any[] {
  const updates: any[] = [];

  const maxLength = Math.max(oldChildren.length, newChildren.length);
  for (let i = 0; i < maxLength; i++) {
    updates.push(diff(oldChildren[i] || createPlaceholder(), newChildren[i]));
  }

  return updates;
}

function createPlaceholder(): VNode {
  return { type: '', props: {}, children: [] };
}
```

### Patch Function

```typescript
// [File: src/vdom/patch.ts]
// Apply changes to real DOM
function patch(parent: HTMLElement, oldVNode: VNode, newVNode: VNode, index = 0) {
  if (!oldVNode) {
    // New node
    parent.appendChild(render(newVNode));
    return;
  }

  if (!newVNode) {
    // Node removed
    parent.childNodes[index].remove();
    return;
  }

  if (oldVNode.type !== newVNode.type) {
    // Replace
    parent.childNodes[index].replaceWith(render(newVNode));
    return;
  }

  // Update element
  const element = parent.childNodes[index] as HTMLElement;
  updateProps(element, oldVNode.props, newVNode.props);

  // Update children
  const maxLength = Math.max(oldVNode.children.length, newVNode.children.length);
  for (let i = 0; i < maxLength; i++) {
    patch(element, oldVNode.children[i], newVNode.children[i], i);
  }
}

function updateProps(element: HTMLElement, oldProps: any, newProps: any) {
  // Remove old props
  Object.keys(oldProps).forEach(key => {
    if (!newProps[key]) {
      element.removeAttribute(key);
    }
  });

  // Set new props
  Object.entries(newProps).forEach(([key, value]) => {
    if (key === 'className') {
      element.className = value as string;
    } else if (key.startsWith('on')) {
      // Handle events
    } else {
      element.setAttribute(key, value as string);
    }
  });
}
```

## Key Takeaways
- Virtual DOM is a JavaScript representation of DOM
- Diffing compares old and new trees
- Only changed elements update in real DOM

## What's Next
This completes the Interview Prep module. Now let's move to [Storybook Setup](18-ecosystem/01-development-tools/01-storybook-setup.md) in the Ecosystem module.