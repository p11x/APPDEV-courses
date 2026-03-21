# Mongoose Validation

## What You'll Learn

- Built-in validators
- Custom validators
- Validation error handling

## Built-in Validators

```javascript
const userSchema = new mongoose.Schema({
  name: { 
    type: String, 
    required: true,
    minlength: 2,
    maxlength: 50 
  },
  email: { 
    type: String, 
    required: true,
    unique: true,
    match: /.+@.+\..+/
  },
  age: { 
    type: Number, 
    min: 0, 
    max: 150 
  },
  role: { 
    type: String, 
    enum: ['user', 'admin', 'moderator'] 
  }
});
```

## Custom Validators

```javascript
const userSchema = new mongoose.Schema({
  password: {
    type: String,
    validate: {
      validator: function(v) {
        return v.length >= 8;
      },
      message: 'Password must be at least 8 characters'
    }
  }
});
```

## Handling Validation Errors

```javascript
try {
  const user = await User.create({ name: 'A' }); // Too short
} catch (error) {
  console.log(error.errors.name.message); // Validation error message
}
```

## Code Example

```javascript
// validation.js - Complete validation example

import mongoose from 'mongoose';

const userSchema = new mongoose.Schema({
  name: {
    type: String,
    required: 'Name is required',
    minlength: [2, 'Name must be at least 2 characters'],
    maxlength: [50, 'Name cannot exceed 50 characters']
  },
  email: {
    type: String,
    required: 'Email is required',
    unique: true,
    match: [/.+@.+\..+/, 'Please enter a valid email']
  },
  age: {
    type: Number,
    min: [0, 'Age cannot be negative'],
    max: [150, 'Age seems incorrect']
  },
  role: {
    type: String,
    enum: {
      values: ['user', 'admin', 'moderator'],
      message: 'Invalid role'
    },
    default: 'user'
  }
});

const User = mongoose.model('User', userSchema);

// Test validation
try {
  await User.create({ name: 'A' }); // Will fail
} catch (error) {
  console.log('Validation error:', error.errors.name.message);
}
```

## Try It Yourself

### Exercise 1: Add Validators
Add validation rules to your user model.

### Exercise 2: Custom Validation
Create a custom validator for password strength.
