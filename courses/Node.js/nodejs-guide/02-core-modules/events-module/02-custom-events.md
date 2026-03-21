# Building Custom Event-Driven Classes

## What You'll Learn

- How to create classes that emit events
- Building a custom EventEmitter for specific use cases
- Designing event-driven APIs
- Practical examples of event-driven architecture

## Extending EventEmitter

The most common way to create event-driven classes is by extending the EventEmitter class.

### Basic Custom Emitter

```javascript
// custom-emitter.js - Creating an EventEmitter class

import { EventEmitter } from 'events';

// Create a class that extends EventEmitter
class MyEmitter extends EventEmitter {
  // Add custom methods that emit events
  greet(name) {
    // Emit the greet event with the name
    this.emit('greet', name);
  }
  
  notify(message) {
    this.emit('notify', message);
  }
}

// Use the custom emitter
const emitter = new MyEmitter();

// Listen for events
emitter.on('greet', (name) => {
  console.log(`👋 Hello, ${name}!`);
});

emitter.on('notify', (message) => {
  console.log(`📢 Notification: ${message}`);
});

// Trigger events
emitter.greet('Alice');
emitter.notify('Meeting in 5 minutes');
```

## Practical Example: Task Runner

Let's build a more useful example - a task runner that emits events:

```javascript
// task-runner.js - Event-driven task runner

import { EventEmitter } from 'events';

class TaskRunner extends EventEmitter {
  constructor() {
    super();  // Call EventEmitter constructor
    this.tasks = [];
    this.running = false;
  }
  
  // Add a task to the queue
  addTask(name, taskFn) {
    this.tasks.push({ name, taskFn });
    this.emit('taskAdded', name);
    return this;  // Allow chaining
  }
  
  // Run all tasks
  async run() {
    if (this.running) {
      this.emit('error', new Error('Already running!'));
      return;
    }
    
    this.running = true;
    this.emit('start');
    
    while (this.tasks.length > 0) {
      const task = this.tasks.shift();
      
      this.emit('taskStart', task.name);
      
      try {
        // Execute the task (could be async)
        await task.taskFn();
        this.emit('taskComplete', task.name);
      } catch (error) {
        this.emit('taskError', { name: task.name, error });
      }
    }
    
    this.running = false;
    this.emit('finish');
  }
}

// Use the task runner
const runner = new TaskRunner();

// Add some tasks
runner
  .addTask('Download files', async () => {
    console.log('  📥 Downloading files...');
    await new Promise(r => setTimeout(r, 500));
  })
  .addTask('Process data', async () => {
    console.log('  ⚙️  Processing data...');
    await new Promise(r => setTimeout(r, 500));
  })
  .addTask('Upload results', async () => {
    console.log('  📤 Uploading results...');
    await new Promise(r => setTimeout(r, 500));
  });

// Listen to events
runner.on('start', () => console.log('\n🚀 Starting task runner...\n'));
runner.on('taskAdded', (name) => console.log(`+ Added: ${name}`));
runner.on('taskStart', (name) => console.log(`▶️  Starting: ${name}`));
runner.on('taskComplete', (name) => console.log(`✅ Completed: ${name}`));
runner.on('taskError', ({ name, error }) => console.log(`❌ Error in ${name}: ${error.message}`));
runner.on('finish', () => console.log('\n🎉 All tasks finished!\n'));

// Run the tasks
runner.run();
```

## Example: Chat Room with Events

Here's a more complex example - a simple chat room:

```javascript
// chat-room.js - Event-driven chat room

import { EventEmitter } from 'events';

class ChatRoom extends EventEmitter {
  constructor(name) {
    super();
    this.name = name;
    this.messages = [];
    this.users = new Set();
  }
  
  // User joins the chat
  join(username) {
    this.users.add(username);
    this.emit('userJoined', username);
    this.broadcast('system', `${username} has joined the chat`);
    return this;
  }
  
  // User leaves the chat
  leave(username) {
    this.users.delete(username);
    this.emit('userLeft', username);
    this.broadcast('system', `${username} has left the chat`);
  }
  
  // Send a message
  send(username, message) {
    const chatMessage = {
      username,
      message,
      timestamp: new Date()
    };
    
    this.messages.push(chatMessage);
    this.emit('message', chatMessage);
  }
  
  // Broadcast to all users
  broadcast(from, message) {
    const chatMessage = {
      username: 'System',
      message,
      timestamp: new Date()
    };
    
    this.messages.push(chatMessage);
    this.emit('message', chatMessage);
  }
  
  // Get chat history
  getHistory() {
    return [...this.messages];
  }
}

// Create a chat room
const lobby = new ChatRoom('General Lobby');

// Listen for events
lobby.on('userJoined', (username) => {
  console.log(`📢 ${username} joined the room`);
});

lobby.on('userLeft', (username) => {
  console.log(`📢 ${username} left the room`);
});

lobby.on('message', (msg) => {
  const time = msg.timestamp.toLocaleTimeString();
  console.log(`[${time}] ${msg.username}: ${msg.message}`);
});

// Simulate chat
lobby
  .join('Alice')
  .join('Bob')
  .join('Charlie');

lobby.send('Alice', 'Hello everyone!');
lobby.send('Bob', 'Hi Alice!');
lobby.send('Charlie', 'Hey guys!');

lobby.leave('Bob');

lobby.send('Alice', 'Bye Bob!');
```

## Example: Data Processor with Events

A data processing pipeline using events:

```javascript
// data-processor.js - Event-driven data processing

import { EventEmitter } from 'events';

class DataProcessor extends EventEmitter {
  constructor() {
    super();
  }
  
  async process(data) {
    this.emit('start', data);
    
    try {
      // Step 1: Validate
      this.emit('validate', data);
      if (!this.validate(data)) {
        throw new Error('Validation failed');
      }
      
      // Step 2: Transform
      this.emit('transform', data);
      const transformed = this.transform(data);
      
      // Step 3: Save
      this.emit('save', transformed);
      const saved = await this.save(transformed);
      
      this.emit('complete', saved);
      return saved;
      
    } catch (error) {
      this.emit('error', error);
      throw error;
    }
  }
  
  validate(data) {
    // Simple validation - check if data exists
    return data !== null && data !== undefined;
  }
  
  transform(data) {
    // Example transformation - uppercase all strings
    if (typeof data === 'string') {
      return data.toUpperCase();
    }
    if (typeof data === 'object') {
      return JSON.stringify(data).toUpperCase();
    }
    return data;
  }
  
  async save(data) {
    // Simulate async save operation
    await new Promise(r => setTimeout(r, 100));
    return { ...data, savedAt: new Date() };
  }
}

// Use the processor
const processor = new DataProcessor();

// Listen to all events
processor.on('start', (data) => console.log('Starting with:', data));
processor.on('validate', (data) => console.log('Validating...'));
processor.on('transform', (data) => console.log('Transforming...'));
processor.on('save', (data) => console.log('Saving...'));
processor.on('complete', (result) => console.log('Complete!', result));
processor.on('error', (err) => console.error('Error:', err.message));

// Process some data
processor.process({ name: 'alice', email: 'alice@example.com' });
```

## Designing Event-Driven APIs

When designing event-driven classes, follow these principles:

### 1. Use Descriptive Event Names
```javascript
// Good
this.emit('userRegistered', user);
this.emit('paymentReceived', amount);

// Less clear
this.emit('register', user);
this.emit('done', amount);
```

### 2. Emit Error Events for Failures
```javascript
// Always emit errors
try {
  // do something
} catch (err) {
  this.emit('error', err);
}
```

### 3. Provide Metadata in Events
```javascript
// Better - include relevant data
this.emit('taskComplete', { taskName, duration, result });

// Minimal - just the basics
this.emit('complete', result);
```

### 4. Consider Using .once() for One-Time Setup
```javascript
// For initialization that only happens once
this.once('ready', () => {
  console.log('System ready!');
});
```

## Common Mistakes

### Mistake 1: Forgetting to Call super()

```javascript
// WRONG - will cause errors
class MyEmitter extends EventEmitter {
  constructor() {
    // Missing super() call!
  }
}

// CORRECT
class MyEmitter extends EventEmitter {
  constructor() {
    super();  // Always call super()!
  }
}
```

### Mistake 2: Not Handling All Error Cases

```javascript
// WRONG - errors might go unhandled
async process() {
  const result = await riskyOperation();
  this.emit('complete', result);
}

// CORRECT - handle errors explicitly
async process() {
  try {
    const result = await riskyOperation();
    this.emit('complete', result);
  } catch (error) {
    this.emit('error', error);
  }
}
```

### Mistake 3: Too Many Events

Don't create an event for everything. Keep your API simple and focused.

## Try It Yourself

### Exercise 1: Timer with Events
Create a Timer class that emits 'tick' events every second and 'done' when it reaches zero.

### Exercise 2: File Watcher Class
Create a FileWatcher class that extends EventEmitter and emits events when files change.

### Exercise 3: Game Event System
Create a simple Game class that emits events for player actions (move, attack, die).

## Next Steps

Now you know how to build custom event-driven classes. Let's explore the HTTP module to create web servers. Continue to [Basic HTTP Server](../http-module/01-basic-server.md).
