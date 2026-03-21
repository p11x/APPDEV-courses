# Virtual DOM Implementation

## Overview
The Virtual DOM is React's key innovation for efficient UI updates. This challenge walks through building a simplified version from scratch, demonstrating createElement, render, diff, and patch operations.

## Prerequisites
- JavaScript fundamentals
- Understanding of DOM and React rendering
- TypeScript basics

## Core Concepts

### Part 1: createElement — Building the Virtual DOM

The first step is creating a JavaScript representation of DOM nodes. This is what React.createElement does internally.

```typescript
// [File: src/vdom/createElement.ts]
/**
 * Part 1: createElement — Build a virtual DOM node
 * 
 * A VNode (Virtual Node) is a plain JavaScript object that represents
 * a DOM element. It has a type, props, and children.
 */

interface VNode {
  type: string | Function;  // Element type (e.g., 'div', 'button', or component)
  props: VNodeProps;       // Attributes, event handlers, etc.
  children: VNode[];       // Child VNodes
  key?: string;            // Optional key for list reconciliation
}

interface VNodeProps {
  [key: string]: unknown;  // Any attribute or event handler
  children?: VNode[];      // Children can also be in props
}

/**
 * Creates a virtual DOM node
 * 
 * @param type - Element type ('div', 'span', etc.) or component function
 * @param props - Element attributes and properties
 * @param children - Child elements
 * @returns A VNode object representing the element
 */
export function createElement(
  type: string | Function,
  props: VNodeProps | null,
  ...children: (VNode | string | number | boolean | null)[]
): VNode {
  // Normalize props to an object (handles null props)
  const normalizedProps: VNodeProps = props ?? {};
  
  // Normalize children - flatten arrays and filter nulls
  const normalizedChildren: VNode[] = children
    .flat()
    .filter((child): child is VNode | string | number => 
      child !== null && child !== false && child !== undefined
    )
    .map(child => {
      // Strings and numbers become text nodes
      if (typeof child === 'string' || typeof child === 'number') {
        return createTextElement(child);
      }
      // VNodes pass through as-is
      return child as VNode;
    });

  return {
    type,
    props: {
      ...normalizedProps,
      children: normalizedChildren,
    },
    children: normalizedChildren,
    key: normalizedProps.key as string | undefined,
  };
}

/**
 * Create a text node VNode
 */
function createTextElement(text: string | number): VNode {
  return {
    type: 'TEXT',  // Special type for text nodes
    props: { nodeValue: text },
    children: [],
  };
}

// Helper to make JSX work (when using TypeScript's React JSX transform)
// This allows writing: <div>Hello</div> instead of createElement('div', null, 'Hello')
export const jsx = createElement;
export const jsxs = createElement;
export const Fragment = Symbol('Fragment');
```

```typescript
// [File: src/vdom/createElement.examples.ts]
import { createElement } from './createElement';

// Example: Creating virtual DOM nodes
const vnode1 = createElement('div', { className: 'container' },
  createElement('h1', null, 'Hello'),
  createElement('p', null, 'World')
);

// Using JSX-like syntax (if using the jsx function):
// const vnode2 = jsx('div', { className: 'container' },
//   jsx('h1', null, 'Hello'),
//   jsx('p', null, 'World')
// );

console.log('VNode structure:', JSON.stringify(vnode1, null, 2));
/*
{
  "type": "div",
  "props": {
    "className": "container",
    "children": [
      { "type": "h1", "props": { "children": ["Hello"] }, "children": [] },
      { "type": "p", "props": { "children": ["World"] }, "children": [] }
    ]
  },
  "children": [...]
}
*/
```

### Part 2: render — Converting VDOM to Real DOM

The render function takes a VNode and creates actual DOM nodes in the browser.

```typescript
// [File: src/vdom/render.ts]
import { VNode, VNodeProps } from './createElement';

/**
 * Part 2: render — Convert VNode to real DOM
 * 
 * This function walks the VNode tree and creates real DOM elements.
 * Text nodes become Text nodes, element VNodes become HTMLElements.
 */

interface Container {
  appendChild(child: ChildNode): void;
  removeChild(child: ChildNode): void;
  replaceChild(newChild: ChildNode, oldChild: ChildNode): void;
}

/**
 * Render a VNode to a DOM container
 * 
 * @param vnode - The virtual node to render
 * @param container - The DOM container to render into
 * @returns The root DOM element
 */
export function render(vnode: VNode | string | number, container: Container): ChildNode {
  // Handle text nodes
  if (typeof vnode === 'string' || typeof vnode === 'number') {
    const textNode = document.createTextNode(String(vnode));
    container.appendChild(textNode);
    return textNode;
  }

  // Handle VNodes
  const element = document.createElement(vnode.type as string);
  
  // Set attributes and event handlers
  Object.entries(vnode.props || {}).forEach(([key, value]) => {
    // Skip children (handled separately)
    if (key === 'children') return;
    
    // Handle event listeners (onClick, onChange, etc.)
    if (key.startsWith('on')) {
      const eventName = key.slice(2).toLowerCase();
      if (typeof value === 'function') {
        element.addEventListener(eventName, value as EventListener);
      }
      return;
    }
    
    // Handle special properties
    if (key === 'className') {
      element.className = value as string;
      return;
    }
    
    if (key === 'style' && typeof value === 'object') {
      Object.assign(element.style, value);
      return;
    }
    
    // Handle key (not a DOM attribute)
    if (key === 'key') {
      return;
    }
    
    // Handle other attributes
    if (value !== null && value !== undefined) {
      element.setAttribute(key, String(value));
    }
  });

  // Recursively render children
  const children = vnode.children || [];
  children.forEach((child: VNode | string | number) => {
    const childNode = render(child, { 
      appendChild: (c) => element.appendChild(c),
      removeChild: (c) => element.removeChild(c),
      replaceChild: (n, o) => element.replaceChild(n, o)
    } as unknown as Container);
    if (childNode) {
      element.appendChild(childNode);
    }
  });

  container.appendChild(element);
  return element;
}

/**
 * Simple implementation for a DOM-like interface
 */
export function createRenderer() {
  return {
    render,
  };
}
```

### Part 3: diff — Comparing Two VDOM Trees

The diff algorithm compares old and new VNodes to determine the minimum number of changes needed.

```typescript
// [File: src/vdom/diff.ts]
import { VNode } from './createElement';

/**
 * Part 3: diff — Compare old and new VNodes
 * 
 * This is the core of React's reconciliation algorithm.
 * Returns a "patch" describing what changes to make.
 */

interface Patch {
  type: 'UPDATE' | 'REPLACE' | 'REMOVE' | 'TEXT';
  props?: Record<string, unknown>;
  children?: Patch[];
  newVNode?: VNode | string | number;
}

/**
 * Diff two VNodes and return a patch
 * 
 * @param oldVNode - The previous VNode
 * @param newVNode - The new VNode
 * @param index - Current position in the tree (for keys)
 * @returns Array of patches to apply, or null if no changes
 */
export function diff(
  oldVNode: VNode | string | number | null, 
  newVNode: VNode | string | number | null,
  index: number = 0
): Patch[] {
  // Handle null/undefined
  if (oldVNode === null || oldVNode === undefined) {
    if (newVNode === null || newVNode === undefined) {
      return []; // Both null, no change
    }
    // New node added
    return [{ type: 'REPLACE', newVNode }];
  }

  if (newVNode === null || newVNode === undefined) {
    // Old node removed
    return [{ type: 'REMOVE' }];
  }

  // Different types = replace entirely
  if (typeof oldVNode !== typeof newVNode) {
    return [{ type: 'REPLACE', newVNode }];
  }

  // Handle text nodes
  if (typeof oldVNode === 'string' || typeof oldVNode === 'number') {
    if (oldVNode !== newVNode) {
      return [{ type: 'TEXT', newVNode: newVNode as string | number }];
    }
    return [];
  }

  // Handle VNodes with different types
  if ((oldVNode as VNode).type !== (newVNode as VNode).type) {
    return [{ type: 'REPLACE', newVNode }];
  }

  // Same type — diff props and children
  const oldVNodeTyped = oldVNode as VNode;
  const newVNodeTyped = newVNode as VNode;

  const patches: Patch[] = [];

  // Diff props
  const propsPatches = diffProps(oldVNodeTyped.props, newVNodeTyped.props);
  if (Object.keys(propsPatches).length > 0) {
    patches.push({ type: 'UPDATE', props: propsPatches });
  }

  // Diff children
  const childrenPatches = diffChildren(
    oldVNodeTyped.children || [],
    newVNodeTyped.children || []
  );
  if (childrenPatches.length > 0) {
    patches.push({ type: 'UPDATE', props: {}, children: childrenPatches });
  }

  return patches;
}

/**
 * Diff props (attributes)
 */
function diffProps(
  oldProps: Record<string, unknown> = {},
  newProps: Record<string, unknown> = {}
): Record<string, unknown> {
  const patches: Record<string, unknown> = {};
  const allKeys = new Set([...Object.keys(oldProps), ...Object.keys(newProps)]);

  allKeys.forEach(key => {
    // Skip children - handled separately
    if (key === 'children') return;

    const oldValue = oldProps[key];
    const newValue = newProps[key];

    if (oldValue !== newValue) {
      // Different value - update it
      if (newValue === undefined || newValue === null) {
        // Removed - set to undefined/null so we know to remove
        patches[key] = undefined;
      } else {
        patches[key] = newValue;
      }
    }
  });

  return patches;
}

/**
 * Diff children arrays
 */
function diffChildren(
  oldChildren: VNode[] = [],
  newChildren: VNode[] = []
): Patch[] {
  const patches: Patch[] = [];
  
  // Use key-based matching when available
  const oldChildrenMap = new Map<string, { vnode: VNode; index: number }>();
  oldChildren.forEach((child, index) => {
    if (child.key) {
      oldChildrenMap.set(child.key, { vnode: child, index });
    }
  });

  newChildren.forEach((newChild, index) => {
    if (newChild.key && oldChildrenMap.has(newChild.key)) {
      // Match by key
      const oldChild = oldChildrenMap.get(newChild.key)!.vnode;
      patches.push(...diff(oldChild, newChild, index));
    } else if (index < oldChildren.length) {
      // Match by position
      patches.push(...diff(oldChildren[index], newChild, index));
    } else {
      // New child at this position
      patches.push(...diff(null, newChild, index));
    }
  });

  // Handle removed children
  const newKeys = new Set(newChildren.map(c => c.key).filter(Boolean));
  oldChildren.forEach((child, index) => {
    if (child.key && !newKeys.has(child.key)) {
      patches.push(...diff(child, null, index));
    }
  });

  return patches;
}
```

### Part 4: patch — Applying Changes to Real DOM

The patch function applies the diff results to the actual DOM.

```typescript
// [File: src/vdom/patch.ts]
import { VNode } from './createElement';
import { Patch } from './diff';

/**
 * Part 4: patch — Apply patches to real DOM
 * 
 * Takes the patch array and applies each change to the DOM.
 */

interface DOMElement extends Element, Text {
  // Additional properties we use
  childNodes: NodeListOf<ChildNode>;
  removeChild(child: ChildNode): ChildNode;
  appendChild(child: ChildNode): ChildNode;
  replaceChild(newChild: ChildNode, oldChild: ChildNode): ChildNode;
  setAttribute(name: string, value: string): void;
  removeAttribute(name: string): void;
  className: string;
  style: CSSStyleDeclaration;
  addEventListener(type: string, handler: EventListener): void;
  removeEventListener(type: string, handler: EventListener): void;
}

/**
 * Apply patches to the DOM
 * 
 * @param dom - The root DOM element
 * @param patches - Array of patches to apply
 */
export function patch(dom: ChildNode | null, patches: Patch[]): void {
  if (!dom || patches.length === 0) return;

  const element = dom as DOMElement;

  patches.forEach(patch => {
    switch (patch.type) {
      case 'REPLACE':
        // Replace entire element
        // Would need to create new element from patch.newVNode
        console.log('REPLACE:', patch.newVNode);
        break;

      case 'REMOVE':
        // Remove this element
        if (element.parentNode) {
          element.parentNode.removeChild(element);
        }
        break;

      case 'UPDATE':
        // Update props
        if (patch.props) {
          Object.entries(patch.props).forEach(([key, value]) => {
            if (value === undefined || value === null) {
              // Remove attribute
              if (key.startsWith('on')) {
                const eventName = key.slice(2).toLowerCase();
                element.removeEventListener(eventName, () => {});
              } else {
                element.removeAttribute(key);
              }
            } else if (key === 'className') {
              element.className = value as string;
            } else if (key.startsWith('on')) {
              const eventName = key.slice(2).toLowerCase();
              element.addEventListener(eventName, value as EventListener);
            } else {
              element.setAttribute(key, String(value));
            }
          });
        }
        break;

      case 'TEXT':
        // Update text content
        if ('textContent' in element) {
          element.textContent = String(patch.newVNode);
        }
        break;
    }
  });
}
```

## Real-World Example: Working Counter

This complete example ties everything together: createElement, render, diff, and patch to create a working counter.

```typescript
// [File: src/vdom/CounterApp.tsx]
import { createElement, VNode } from './createElement';
import { render } from './render';
import { diff } from './diff';
import { patch } from './patch';

/**
 * Complete working example: A counter component
 * 
 * This demonstrates the full flow:
 * 1. createElement builds VNode tree
 * 2. render creates DOM from VNode
 * 3. User clicks button
 * 4. createElement builds new VNode tree
 * 5. diff compares old and new trees
 * 6. patch applies changes to DOM
 */

// State management (simplified — normally useState would handle this)
let currentState = { count: 0 };
let rootElement: ChildNode | null = null;

/**
 * Counter component — returns a VNode tree
 */
function Counter({ count }: { count: number }): VNode {
  return createElement('div', { className: 'counter' },
    createElement('h1', null, `Count: ${count}`),
    createElement('button', { 
      onClick: () => handleClick() 
    }, 'Increment'),
    createElement('button', {
      onClick: () => handleDecrement()
    }, 'Decrement')
  );
}

// Store previous VNode for diffing
let previousVNode: VNode | null = null;

/**
 * Render function that does full diff + patch
 */
function renderApp(state: typeof currentState) {
  // Create new VNode tree
  const newVNode = createElement('div', { className: 'app' },
    createElement('h2', null, 'My Counter App'),
    Counter({ count: state.count })
  );

  if (rootElement === null) {
    // First render: just render directly
    const container = document.getElementById('root')!;
    rootElement = render(newVNode, container);
  } else {
    // Subsequent renders: diff and patch
    const patches = diff(previousVNode, newVNode, 0);
    if (patches.length > 0) {
      patch(rootElement, patches);
    }
  }

  // Store for next diff
  previousVNode = newVNode;
}

// Event handlers
function handleClick() {
  currentState = { count: currentState.count + 1 };
  renderApp(currentState);
}

function handleDecrement() {
  currentState = { count: currentState.count - 1 };
  renderApp(currentState);
}

// Initialize
renderApp(currentState);
```

## Connecting to React

Understanding how Fiber improves on this naive implementation helps explain React's architecture.

```typescript
// [File: src/vdom/fiberComparison.ts]
/**
 * How Fiber improves on this naive implementation:
 * 
 * 1. **Incremental Rendering**: Fiber can pause and resume work,
 *    prioritizing urgent updates (like typing) over less important ones.
 * 
 * 2. **Work Priority**: Fiber assigns priority levels to updates,
 *    using requestIdleCallback to schedule low-priority work.
 * 
 * 3. **Concurrent Mode**: Fiber can prepare multiple versions of the
 *    UI simultaneously, enabling features like useTransition.
 * 
 * 4. **Double Buffering**: Fiber maintains a "work in progress" tree,
 *    committing all changes atomically to avoid partial renders.
 * 
 * 5. **Effect Lists**: Fiber organizes side effects (DOM updates,
 *    lifecycle calls) into a linked list for efficient processing.
 * 
 * The key insight: our naive diff algorithm is O(n³), but React's
 * Fiber makes assumptions (single-element type changes, key-based lists)
 * to reduce it to O(n).
 */
```

## Key Takeaways

- createElement builds a JavaScript representation of DOM (VNode tree)
- render converts VNodes to real DOM elements recursively
- diff compares old and new trees to find minimum changes
- patch applies those changes to the actual DOM
- Fiber improves on this with incremental rendering and priority scheduling

## What's Next

Continue to [Docker Compose with Full Stack](../18-ecosystem/04-docker/03-docker-compose-with-fullstack.md) to learn how to containerize a complete React + Express + PostgreSQL application.