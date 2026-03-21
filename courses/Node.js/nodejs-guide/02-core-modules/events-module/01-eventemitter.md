# EventEmitter in Node.js

## What You'll Learn

- What the EventEmitter is and why it's important
- How to listen for and emit events
- Common EventEmitter methods: on, emit, once, off
- Understanding the event-driven pattern

## What is EventEmitter?

**EventEmitter** is a built-in Node.js class that implements the observer pattern. It allows objects to:
- **Emit** (send) events when something happens
- **Listen** (receive) events and react to them

This is the foundation of Node.js's event-driven architecture. Many Node.js modules (like HTTP servers, streams) are built on EventEmitter.

## Importing EventEmitter

```javascript
import { EventEmitter } from 'events';
```

## Basic EventEmitter Usage

### Creating an Emitter

```javascript
// basic-emitter.js - Basic EventEmitter example

import { EventEmitter } from 'events';

// Create a new EventEmitter instance
const myEmitter = new EventEmitter();

// Listen for the 'greet' event
// When someone emits 'greet', this callback runs
myEmitter.on('greet', (name) => {
  console.log(`Hello, ${name}!`);
});

// Listen for another event
myEmitter.on('farewell', () => {
  console.log('Goodbye!');
});

// Emit (trigger) the events
console.log('Emitting greet...');
myEmitter.emit('greet', 'Alice');

console.log('Emitting farewell...');
myEmitter.emit('farewell');

console.log('Done!');
```

### EventEmitter Methods

Here are the main methods:

```javascript
// emitter-methods.js - Common EventEmitter methods

import { EventEmitter } from 'events';

const emitter = new EventEmitter();

// .on(eventName, callback) - Listen for events (multiple allowed)
emitter.on('data', (data) => {
  console.log('Listener 1:', data);
});

emitter.on('data', (data) => {
  console.log('Listener 2:', data);
});

// .once(eventName, callback) - Listen for event once, then auto-remove
emitter.once('single', () => {
  console.log('This only runs once!');
});

// .emit(eventName, ...args) - Trigger an event
emitter.emit('data', { message: 'Hello!' });
emitter.emit('data', { message: 'World!' });
emitter.emit('single');

// .off(eventName, callback) - Remove a specific listener
const listener = () => console.log('To be removed');
emitter.on('remove-me', listener);

// Remove the listener
emitter.off('remove-me', listener);

// This won't trigger the removed listener
emitter.emit('remove-me');

// .removeAllListeners([eventName]) - Remove all listeners
emitter.removeAllListeners();
```

### Event Names and Multiple Arguments

```javascript
// emitter-args.js - Multiple arguments in events

import { EventEmitter } from 'events';

const emitter = new EventEmitter();

// Events can pass multiple arguments
emitter.on('userRegistered', (username, email, age) => {
  console.log(`User: ${username}, Email: ${email}, Age: ${age}`);
});

// Emit with multiple arguments
emitter.emit('userRegistered', 'alice', 'alice@example.com', 25);

// Using spread operator
emitter.on('log', (...args) => {
  console.log('Log:', ...args);
});

emitter.emit('log', 'INFO', 'User logged in', new Date());
```

## Code Example: Complete EventEmitter Demo

```javascript
// event-demo.js - Complete EventEmitter demonstration

import { EventEmitter } from 'events';

console.log('=== EventEmitter Demo ===\n');

// Create an emitter
const emitter = new EventEmitter();

// ─────────────────────────────────────────
// 1. Basic on() and emit()
// ─────────────────────────────────────────
console.log('1. Basic Events:');

emitter.on('greet', (name) => {
  console.log(`   Hello, ${name}!`);
});

emitter.emit('greet', 'World');

// ─────────────────────────────────────────
// 2. Multiple listeners
// ─────────────────────────────────────────
console.log('\n2. Multiple Listeners:');

emitter.on('notify', () => console.log('   Listener 1: Notification received'));
emitter.on('notify', () => console.log('   Listener 2: Badge updated'));
emitter.on('notify', () => console.log('   Listener 3: Sound played'));

emitter.emit('notify');

// ─────────────────────────────────────────
// 3. .once() - fires only once
// ─────────────────────────────────────────
console.log('\n3. Once Listener:');

emitter.once('oneTime', () => {
  console.log('   This runs only once!');
});

emitter.emit('oneTime');
emitter.emit('oneTime');  // Won't fire again
emitter.emit('oneTime');  // Won't fire again

// ─────────────────────────────────────────
// 4. Error events
// ─────────────────────────────────────────
console.log('\n4. Error Events:');

emitter.on('error', (err) => {
  console.log('   Error caught:', err.message);
});

// Emit an error
emitter.emit('error', new Error('Something went wrong!'));

// ─────────────────────────────────────────
// 5. Checking listeners
// ─────────────────────────────────────────
console.log('\n5. Event Metadata:');

emitter.on('check', () => {});  // Add a listener
console.log('   Listener count for "check":', emitter.listenerCount('check'));
console.log('   Event names:', emitter.eventNames());

// ─────────────────────────────────────────
// 6. Removing listeners
// ─────────────────────────────────────────
console.log('\n6. Removing Listeners:');

const callback = () => console.log('   Will be removed');
emitter.on('remove', callback);

// Remove the specific listener
emitter.off('remove', callback);

console.log('   After remove, count:', emitter.listenerCount('remove'));

// ─────────────────────────────────────────
// 7. Built-in error handling
// ─────────────────────────────────────────
console.log('\n7. Unhandled Errors:');

// Create a new emitter to demonstrate unhandled errors
const strictEmitter = new EventEmitter();

// Without an 'error' listener, throws uncaught exception
// In production, always handle errors!
strictEmitter.on('error', (err) => {
  console.log('   Handled error:', err.message);
});

strictEmitter.emit('error', new Error('Safe error handling!'));
```

## Understanding the Event Loop with Events

When you use EventEmitter, here's what happens:

1. **Listener registered**: `emitter.on('event', callback)` stores the callback
2. **Event emitted**: `emitter.emit('event')` adds the callback to the event loop
3. **Event loop processes**: After current code completes, the callback runs
4. **Cleanup**: If it was a `.once()` listener, it's automatically removed

## Common Mistakes

### Mistake 1: Forgetting to Handle Errors

```javascript
// WRONG - unhandled errors will crash your app
const emitter = new EventEmitter();
emitter.emit('error', new Error('Oops!'));

// CORRECT - always handle errors
emitter.on('error', (err) => {
  console.error('Error:', err.message);
});
```

### Mistake 2: Memory Leaks from Event Listeners

If you add listeners but never remove them, you can cause memory leaks:

```javascript
// WRONG - adding listeners in a loop without cleanup
setInterval(() => {
  emitter.on('tick', () => {});  // Leaks memory!
}, 1000);

// CORRECT - remove listeners when done
emitter.on('tick', handleTick);
setTimeout(() => {
  emitter.off('tick', handleTick);  // Clean up!
}, 10000);
```

### Mistake 3: Using this in Event Listeners

```javascript
// Problem: 'this' binding in arrow functions
emitter.on('event', () => {
  console.log(this);  // 'this' is not what you expect!
});

// Solution: Use regular functions or don't rely on this
emitter.on('event', function() {
  console.log(this);  // 'this' is the emitter
});
```

## Try It Yourself

### Exercise 1: Counter Events
Create an EventEmitter that emits 'increment' and 'decrement' events, and tracks a count value.

### Exercise 2: File Watcher Events
Create an EventEmitter that wraps fs.watch and emits custom events when files change.

### Exercise 3: Custom Emitter Class
Create a class that extends EventEmitter and emits events when tasks complete.

## Next Steps

Now you understand the basic EventEmitter. Let's learn how to build custom event-driven classes. Continue to [Custom Events](./02-custom-events.md).
