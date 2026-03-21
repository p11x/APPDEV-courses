# MongoDB CRUD Operations

## What You'll Learn

- Creating documents
- Reading documents
- Updating documents
- Deleting documents

## Create

```javascript
// Create one
const user = await User.create({ name: 'Alice', email: 'alice@example.com' });

// Or
const user = new User({ name: 'Bob' });
await user.save();
```

## Read

```javascript
// Find all
const users = await User.find();

// Find by ID
const user = await User.findById(id);

// Find one
const user = await User.findOne({ email: 'alice@example.com' });
```

## Update

```javascript
// Update one
await User.updateOne({ _id: id }, { name: 'New Name' });

// Find and update
const user = await User.findByIdAndUpdate(id, { name: 'New Name' });
```

## Delete

```javascript
// Delete one
await User.deleteOne({ _id: id });

// Find and delete
const user = await User.findByIdAndDelete(id);
```

## Code Example

```javascript
// crud.js - Complete CRUD example

import mongoose from 'mongoose';
import User from './models/User.js';

await mongoose.connect('mongodb://localhost:27017/test');

// CREATE
const alice = await User.create({ name: 'Alice', email: 'alice@example.com' });
console.log('Created:', alice);

// READ
const users = await User.find();
const alice2 = await User.findOne({ name: 'Alice' });
console.log('Found:', users.length, 'users');

// UPDATE
await User.updateOne({ _id: alice._id }, { name: 'Alice Smith' });
console.log('Updated!');

// DELETE
await User.deleteOne({ _id: alice._id });
console.log('Deleted!');

await mongoose.disconnect();
```

## Try It Yourself

### Exercise 1: CRUD Operations
Implement all CRUD operations for users.

### Exercise 2: Query Methods
Practice different query methods like find, findOne, findById.
