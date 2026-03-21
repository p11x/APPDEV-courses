# MongoDB with Mongoose

## What You'll Learn

- Installing Mongoose
- Connecting to MongoDB
- Creating schemas and models

## Installing Mongoose

```bash
npm install mongoose
```

## Connecting to MongoDB

```javascript
// db.js - MongoDB connection

import mongoose from 'mongoose';

await mongoose.connect('mongodb://localhost:27017/myapp');

console.log('Connected to MongoDB!');
```

## Creating Schemas and Models

```javascript
// models/User.js - User model

import mongoose from 'mongoose';

const userSchema = new mongoose.Schema({
  name: String,
  email: { type: String, unique: true },
  age: Number,
  createdAt: { type: Date, default: Date.now }
});

const User = mongoose.model('User', userSchema);

export default User;
```

## Code Example

```javascript
// example.js - Complete Mongoose example

import mongoose from 'mongoose';
import User from './models/User.js';

await mongoose.connect('mongodb://localhost:27017/test');

// Create
const user = await User.create({
  name: 'Alice',
  email: 'alice@example.com'
});

// Find
const users = await User.find();

// Find one
const user = await User.findOne({ email: 'alice@example.com' });

// Update
await User.updateOne({ _id: user._id }, { name: 'Alice Smith' });

// Delete
await User.deleteOne({ _id: user._id });

await mongoose.disconnect();
```

## Try It Yourself

### Exercise 1: Connect to MongoDB
Connect to a MongoDB database.

### Exercise 2: Create Models
Create user and post models with Mongoose.
