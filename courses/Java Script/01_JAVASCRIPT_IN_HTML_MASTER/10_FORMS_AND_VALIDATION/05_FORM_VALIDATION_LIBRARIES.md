# Form Validation Libraries: Comprehensive Guide

**Table of Contents**
1. [Introduction](#introduction)
2. [Library Comparison](#library-comparison)
3. [Yup Validation](#yup-validation)
4. [Zod Validation](#zod-validation)
5. [Joi Validation](#joi-validation)
6. [Implementation Examples](#implementation-examples)
7. [Integration with Frameworks](#integration-with-frameworks)
8. [Performance Considerations](#performance-considerations)
9. [Key Takeaways](#key-takeaways)
10. [Common Pitfalls](#common-pitfalls)

---

## Introduction

While JavaScript provides native validation capabilities through the Constraint Validation API, many developers prefer using dedicated validation libraries for more complex forms. These libraries offer composable validation rules, schema-based validation, TypeScript support, and easier integration with modern frameworks.

This guide covers three popular JavaScript validation libraries: Yup, Zod, and Joi. Each library has its strengths and use cases, and understanding them helps you choose the right tool for your project.

All three libraries follow a declarative approach where you define a validation schema separately from your form logic, making code more maintainable and testable.

---

## Library Comparison

| Feature | Yup | Zod | Joi |
|---------|-----|-----|-----|
| Bundle Size | ~12KB | ~10KB | ~200KB |
| TypeScript | Optional | Native | Optional |
| Immutability | No | Yes | No |
| Modifiable Schema | Yes | No | Yes |
| Error Messages | Customizable | Customizable | Customizable |
| Async Validation | Yes | Yes | Yes |
| Browser Support | Modern | Modern | Modern |
| Date Validation | Yes | Yes | Yes |
| Array Validation | Yes | Yes | Yes |

### When to Use Each Library

**Yup**: Best for React applications, especially with Formik. Lightweight and integrates well with component-based frameworks.

**Zod**: Best for TypeScript projects requiring runtime type safety. Native immutability, great type inference.

**Joi**: Best for large applications needing comprehensive validation. Most features, but larger bundle size.

---

## Yup Validation

Yup is a JavaScript schema builder for value parsing and validation. It integrates particularly well with React and Formik.

### Installation

```bash
npm install yup
```

### Basic Schema Definition

```javascript
import * as yup from 'yup';

const schema = yup.object().shape({
  name: yup.string().required('Name is required'),
  email: yup.string().email('Invalid email').required('Email is required'),
  age: yup.number().positive().integer().min(18, 'Must be at least 18'),
  password: yup.string().min(8, 'Password must be at least 8 characters'),
  confirmPassword: yup.string()
    .oneOf([yup.ref('password')], 'Passwords must match')
    .required('Please confirm your password')
});
```

### String Validation

```javascript
const stringSchema = yup.object().shape({
  username: yup.string()
    .required('Username is required')
    .min(3, 'Username must be at least 3 characters')
    .max(20, 'Username must be at most 20 characters')
    .matches(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),
  
  bio: yup.string()
    .max(500, 'Bio must be at most 500 characters'),
  
  url: yup.string()
    .url('Must be a valid URL')
});
```

### Number Validation

```javascript
const numberSchema = yup.object().shape({
  age: yup.number()
    .required('Age is required')
    .min(0, 'Age cannot be negative')
    .max(150, 'Invalid age'),
  
  price: yup.number()
    .positive('Price must be positive')
    .lessThan(10000, 'Price must be less than 10000')
    .round('floor'),
  
  percentage: yup.number()
    .min(0)
    .max(100)
});
```

### Date Validation

```javascript
const dateSchema = yup.object().shape({
  startDate: yup.date()
    .required('Start date is required')
    .min(new Date(), 'Start date must be in the future'),
  
  endDate: yup.date()
    .required('End date is required')
    .when('$startDate', (startDate, schema) => {
      return schema.min(startDate, 'End date must be after start date');
    })
});
```

### Array Validation

```javascript
const arraySchema = yup.object().shape({
  tags: yup.array()
    .of(yup.string().max(20, 'Each tag must be at most 20 characters'))
    .min(1, 'At least one tag is required')
    .max(5, 'Maximum 5 tags allowed')
    .unique('Tags must be unique'),
  
  emails: yup.array()
    .of(yup.string().email('Invalid email format'))
    .min(1, 'At least one email is required')
});
```

### Object Validation

```javascript
const objectSchema = yup.object().shape({
  address: yup.object().shape({
    street: yup.string().required('Street is required'),
    city: yup.string().required('City is required'),
    zipCode: yup.string()
      .required('ZIP code is required')
      .matches(/^[0-9]{5}$/, 'Invalid ZIP code format'),
    country: yup.string().required('Country is required')
  })
});
```

### Conditional Validation

```javascript
const conditionalSchema = yup.object().shape({
  accountType: yup.string().required(),
  
  companyName: yup.string().when('accountType', {
    is: 'business',
    then: yup.string().required('Company name is required for business accounts')
  }),
  
  taxId: yup.string().when('accountType', {
    is: 'business',
    then: yup.string().required('Tax ID is required for business accounts')
      .matches(/^[0-9]{2}-[0-9]{7}$/, 'Invalid Tax ID format'),
    otherwise: yup.string()
  })
});
```

### Async Validation

```javascript
const asyncSchema = yup.object().shape({
  username: yup.string()
    .required('Username is required')
    .test('unique', 'Username is already taken', async function(value) {
      if (!value) return false;
      
      const response = await fetch(`/api/check-username?username=${value}`);
      const data = await response.json();
      return data.available;
    })
});
```

### Custom Validation Rules

```javascript
const customSchema = yup.object().shape({
  password: yup.string()
    .required('Password is required')
    .test('strength', 'Password is too weak', (value) => {
      if (!value) return false;
      
      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumbers = /[0-9]/.test(value);
      const hasSpecial = /[^A-Za-z0-9]/.test(value);
      
      const strength = [hasUpperCase, hasLowerCase, hasNumbers, hasSpecial]
        .filter(Boolean).length;
      
      return strength >= 3;
    })
});
```

### Form Integration

```javascript
import { useFormik } from 'formik';
import * as yup from 'yup';

const validationSchema = yup.object({
  email: yup.string()
    .email('Invalid email format')
    .required('Email is required'),
  password: yup.string()
    .min(8, 'Password must be at least 8 characters')
    .required('Password is required')
});

function LoginForm() {
  const formik = useFormik({
    initialValues: {
      email: '',
      password: ''
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      await login(values);
    }
  });
  
  return (
    <form onSubmit={formik.handleSubmit}>
      <input
        id="email"
        type="email"
        {...formik.getFieldProps('email')}
      />
      {formik.touched.email && formik.errors.email ? (
        <div className="error">{formik.errors.email}</div>
      ) : null}
      
      <input
        id="password"
        type="password"
        {...formik.getFieldProps('password')}
      />
      {formik.touched.password && formik.errors.password ? (
        <div className="error">{formik.errors.password}</div>
      ) : null}
      
      <button type="submit">Login</button>
    </form>
  );
}
```

---

## Zod Validation

Zod is a TypeScript-first schema declaration and validation library. It provides excellent type inference and immutability.

### Installation

```bash
npm install zod
```

### Basic Schema Definition

```typescript
import { z } from 'zod';

const userSchema = z.object({
  name: z.string().min(1, 'Name is required'),
  email: z.string().email('Invalid email'),
  age: z.number().int().positive().min(18, 'Must be at least 18'),
  password: z.string().min(8, 'Password must be at least 8 characters'),
  confirmPassword: z.string()
});

userSchema.refine(
  (data) => data.password === data.confirmPassword,
  { message: 'Passwords do not match', path: ['confirmPassword'] }
);
```

### String Validation

```typescript
const stringSchema = z.object({
  username: z.string()
    .min(3, 'Username must be at least 3 characters')
    .max(20, 'Username must be at most 20 characters')
    .regex(/^[a-zA-Z0-9_]+$/, 'Username can only contain letters, numbers, and underscores'),
  
  email: z.string()
    .email('Invalid email address'),
  
  url: z.string()
    .url('Must be a valid URL'),
  
  phone: z.string()
    .regex(/^\+?[1-9]\d{1,14}$/, 'Invalid phone number format')
});
```

### Number Validation

```typescript
const numberSchema = z.object({
  age: z.number()
    .int('Age must be an integer')
    .min(0, 'Age cannot be negative')
    .max(150, 'Invalid age'),
  
  price: z.number()
    .positive('Price must be positive')
    .max(10000, 'Price exceeds maximum'),
  
  score: z.number()
    .min(0)
    .max(100, 'Score must be between 0 and 100')
});
```

### Boolean and Enum Validation

```typescript
const booleanSchema = z.object({
  isActive: z.boolean(),
  
  subscription: z.enum(['free', 'basic', 'premium']),
  
  role: z.enum(['user', 'admin', 'moderator'], {
    errorMap: () => ({ message: 'Invalid role' })
  })
});
```

### Optional and Nullable

```typescript
const optionalSchema = z.object({
  name: z.string().min(1),
  
  email: z.string().email().optional(),
  
  nickname: z.string().optional(),
  
  bio: z.string().nullable(),
  
  avatar: z.string().nullish()
});
```

### Default Values

```typescript
const defaultSchema = z.object({
  role: z.enum(['user', 'admin']).default('user'),
  
  notifications: z.boolean().default(true),
  
  theme: z.enum(['light', 'dark']).default('light')
});
```

### Array Validation

```typescript
const arraySchema = z.object({
  tags: z.array(z.string().max(20)).min(1, 'At least one tag required').max(5),
  
  numbers: z.array(z.number()).length(3, 'Must have exactly 3 numbers'),
  
  items: z.array(z.object({
    id: z.string(),
    quantity: z.number().int().positive()
  }))
});
```

### Nested Objects

```typescript
const nestedSchema = z.object({
  user: z.object({
    name: z.string().min(1),
    email: z.string().email()
  }),
  
  address: z.object({
    street: z.string(),
    city: z.string(),
    zipCode: z.string()
  }).optional()
});
```

### Discriminated Unions

```typescript
const messageSchema = z.discriminatedUnion('type', [
  z.object({ type: z.literal('text'), content: z.string() }),
  z.object({ type: z.literal('image'), url: z.string().url() }),
  z.object({ type: z.literal('file'), fileName: z.string(), size: z.number() })
]);

const textMessage = { type: 'text', content: 'Hello' };
const imageMessage = { type: 'image', url: 'https://example.com/image.jpg' };

messageSchema.parse(textMessage);
messageSchema.parse(imageMessage);
```

### Effects: Transform and Preprocess

```typescript
const transformSchema = z.object({
  email: z.string()
    .email()
    .transform((val) => val.toLowerCase()),
  
  age: z.string()
    .transform((val) => parseInt(val, 10))
});

const preprocessSchema = z.preprocess(
  (val) => (val === '' ? undefined : val),
  z.string().min(3)
);
```

### Async Refine

```typescript
const asyncSchema = z.object({
  username: z.string()
    .min(3)
    .refine(
      async (val) => {
        const response = await fetch(`/api/users/${val}/exists`);
        const data = await response.json();
        return !data.exists;
      },
      { message: 'Username already taken' }
    )
});
```

### TypeScript Integration

```typescript
const userSchema = z.object({
  id: z.string().uuid(),
  name: z.string().min(1),
  email: z.string().email(),
  role: z.enum(['user', 'admin']),
  createdAt: z.date()
});

type User = z.infer<typeof userSchema>;

const user: User = {
  id: '123e4567-e89b-12d3-a456-426614174000',
  name: 'John Doe',
  email: 'john@example.com',
  role: 'user',
  createdAt: new Date()
};

userSchema.parse(user);
```

---

## Joi Validation

Joi is the most feature-rich validation library, suitable for complex validation scenarios in large applications.

### Installation

```bash
npm install joi
```

### Basic Schema Definition

```javascript
import Joi from 'joi';

const schema = Joi.object({
  name: Joi.string().required(),
  email: Joi.string().email().required(),
  age: Joi.number().integer().min(18).required(),
  password: Joi.string().min(8).required(),
  confirmPassword: Joi.string().valid(Joi.ref('password')).required()
});
```

### String Validation

```javascript
const stringSchema = Joi.object({
  username: Joi.string()
    .alphanum()
    .min(3)
    .max(20)
    .required(),
  
  email: Joi.string()
    .email({ tlds: { allow: ['com', 'net', 'org'] } }),
  
  url: Joi.string()
    .uri({ scheme: ['http', 'https'] }),
  
  phone: Joi.string()
    .pattern(/^\+?[1-9]\d{1,14}$/)
});
```

### Number Validation

```javascript
const numberSchema = Joi.object({
  age: Joi.number()
    .integer()
    .min(0)
    .max(150)
    .required(),
  
  price: Joi.number()
    .positive()
    .precision(2)
    .required(),
  
  percentage: Joi.number()
    .min(0)
    .max(100)
});
```

### Date Validation

```javascript
const dateSchema = Joi.object({
  birthDate: Joi.date()
    .iso()
    .less('now')
    .required(),
  
  startDate: Joi.date()
    .iso()
    .required(),
  
  endDate: Joi.date()
    .iso()
    .min(Joi.ref('startDate'))
    .required()
});
```

### Array Validation

```javascript
const arraySchema = Joi.object({
  tags: Joi.array()
    .items(Joi.string().max(20))
    .min(1)
    .max(5)
    .required(),
  
  numbers: Joi.array()
    .items(Joi.number())
    .unique()
    .required()
});
```

### Object Validation

```javascript
const objectSchema = Joi.object({
  name: Joi.string().required(),
  
  settings: Joi.object({
    theme: Joi.string().valid('light', 'dark'),
    notifications: Joi.boolean(),
    language: Joi.string().default('en')
  })
});
```

### Alternatives and Conditionals

```javascript
const conditionalSchema = Joi.object({
  type: Joi.string().valid('personal', 'business').required(),
  
  companyName: Joi.string().when('type', {
    is: 'business',
    then: Joi.string().required()
  }),
  
  taxId: Joi.string().when('type', {
    is: 'business',
    then: Joi.string().required()
  })
});
```

### Custom Validation Rules

```javascript
const customSchema = Joi.object({
  password: Joi.string()
    .min(8)
    .pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/)
    .messages({
      'string.pattern.base': 'Password must contain uppercase, lowercase, and number'
    })
});

const customValidator = Joi.string().custom((value, helpers) => {
  if (value === 'password') {
    return helpers.error('string.forbidden');
  }
  return value;
});

customSchema.keys({ custom: customValidator });
```

### Async Validation

```javascript
const asyncSchema = Joi.object({
  username: Joi.string()
    .min(3)
    .external(async (value) => {
      const exists = await checkUsernameExists(value);
      if (exists) {
        throw new Error('Username already exists');
      }
      return value;
    })
});
```

### Compile-Time Validation

```javascript
const schema = Joi.object({
  id: [Joi.string().uuid(), Joi.number().integer()],
  
  name: Joi.alternatives().try(
    Joi.string(),
    Joi.number()
  )
});
```

---

## Implementation Examples

### Example 1: Formik with Yup Integration

```javascript
// File: components/LoginForm.jsx

import React from 'react';
import { useFormik } from 'formik';
import * as yup from 'yup';

const validationSchema = yup.object({
  email: yup.string()
    .email('Enter a valid email')
    .required('Email is required'),
  password: yup.string()
    .min(8, 'Password must be at least 8 characters')
    .required('Password is required')
});

export function LoginForm() {
  const formik = useFormik({
    initialValues: {
      email: '',
      password: '',
      rememberMe: false
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        await loginUser(values);
        alert('Login successful!');
      } catch (error) {
        console.error('Login failed:', error);
      }
    }
  });
  
  return (
    <form onSubmit={formik.handleSubmit} className="login-form">
      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          {...formik.getFieldProps('email')}
          className={formik.touched.email && formik.errors.email ? 'error' : ''}
        />
        {formik.touched.email && formik.errors.email && (
          <div className="error-message">{formik.errors.email}</div>
        )}
      </div>
      
      <div className="form-group">
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          {...formik.getFieldProps('password')}
          className={formik.touched.password && formik.errors.password ? 'error' : ''}
        />
        {formik.touched.password && formik.errors.password && (
          <div className="error-message">{formik.errors.password}</div>
        )}
      </div>
      
      <div className="form-group">
        <label>
          <input
            type="checkbox"
            name="rememberMe"
            checked={formik.values.rememberMe}
            onChange={formik.handleChange}
          />
          Remember me
        </label>
      </div>
      
      <button type="submit" disabled={formik.isSubmitting}>
        {formik.isSubmitting ? 'Logging in...' : 'Login'}
      </button>
    </form>
  );
}
```

### Example 2: React Hook Form with Zod

```javascript
// File: components/RegistrationForm.jsx

import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

const registrationSchema = z.object({
  firstName: z.string().min(1, 'First name is required'),
  lastName: z.string().min(1, 'Last name is required'),
  email: z.string().email('Invalid email address'),
  password: z.string()
    .min(8, 'Password must be at least 8 characters')
    .regex(/[A-Z]/, 'Password must contain uppercase letter')
    .regex(/[0-9]/, 'Password must contain a number'),
  confirmPassword: z.string(),
  terms: z.boolean().refine(val => val === true, 'You must accept the terms')
}).refine(data => data.password === data.confirmPassword, {
  message: 'Passwords do not match',
  path: ['confirmPassword']
});

type RegistrationFormData = z.infer<typeof registrationSchema>;

export function RegistrationForm() {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting }
  } = useForm<RegistrationFormData>({
    resolver: zodResolver(registrationSchema)
  });
  
  const onSubmit = async (data: RegistrationFormData) => {
    try {
      await registerUser(data);
      alert('Registration successful!');
    } catch (error) {
      console.error('Registration failed:', error);
    }
  };
  
  return (
    <form onSubmit={handleSubmit(onSubmit)} className="registration-form">
      <div className="form-group">
        <label htmlFor="firstName">First Name</label>
        <input id="firstName" {...register('firstName')} />
        {errors.firstName && (
          <span className="error">{errors.firstName.message}</span>
        )}
      </div>
      
      <div className="form-group">
        <label htmlFor="lastName">Last Name</label>
        <input id="lastName" {...register('lastName')} />
        {errors.lastName && (
          <span className="error">{errors.lastName.message}</span>
        )}
      </div>
      
      <div className="form-group">
        <label htmlFor="email">Email</label>
        <input id="email" type="email" {...register('email')} />
        {errors.email && (
          <span className="error">{errors.email.message}</span>
        )}
      </div>
      
      <div className="form-group">
        <label htmlFor="password">Password</label>
        <input id="password" type="password" {...register('password')} />
        {errors.password && (
          <span className="error">{errors.password.message}</span>
        )}
      </div>
      
      <div className="form-group">
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input id="confirmPassword" type="password" {...register('confirmPassword')} />
        {errors.confirmPassword && (
          <span className="error">{errors.confirmPassword.message}</span>
        )}
      </div>
      
      <div className="form-group">
        <label>
          <input type="checkbox" {...register('terms')} />
          I accept the terms and conditions
        </label>
        {errors.terms && (
          <span className="error">{errors.terms.message}</span>
        )}
      </div>
      
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Creating Account...' : 'Register'}
      </button>
    </form>
  );
}
```

### Example 3: Vanilla JavaScript with Joi

```javascript
// File: js/validation.js

import Joi from 'joi';

const formSchema = Joi.object({
  firstName: Joi.string()
    .min(1)
    .max(50)
    .required()
    .messages({
      'string.empty': 'First name is required',
      'string.max': 'First name must be at most 50 characters'
    }),
  
  lastName: Joi.string()
    .min(1)
    .max(50)
    .required()
    .messages({
      'string.empty': 'Last name is required'
    }),
  
  email: Joi.string()
    .email({ tlds: { allow: ['com', 'net', 'org'] } })
    .required()
    .messages({
      'string.email': 'Please enter a valid email address',
      'string.empty': 'Email is required'
    }),
  
  password: Joi.string()
    .min(8)
    .pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/)
    .required()
    .messages({
      'string.min': 'Password must be at least 8 characters',
      'string.pattern.base': 'Password must contain uppercase, lowercase, and number'
    }),
  
  confirmPassword: Joi.string()
    .valid(Joi.ref('password'))
    .required()
    .messages({
      'any.only': 'Passwords do not match'
    }),
  
  birthDate: Joi.date()
    .iso()
    .less('now')
    .required()
    .messages({
      'date.less': 'Birth date must be in the past',
      'date.iso': 'Please enter a valid date'
    })
});

function validateFormData(formData) {
  const data = {
    firstName: formData.get('firstName'),
    lastName: formData.get('lastName'),
    email: formData.get('email'),
    password: formData.get('password'),
    confirmPassword: formData.get('confirmPassword'),
    birthDate: formData.get('birthDate')
  };
  
  const { error, value } = formSchema.validate(data, {
    abortEarly: false,
    allowUnknown: true
  });
  
  if (error) {
    const errors = {};
    error.details.forEach(detail => {
      const field = detail.path.join('.');
      errors[field] = detail.message;
    });
    return { valid: false, errors };
  }
  
  return { valid: true, value };
}

function displayErrors(errors) {
  document.querySelectorAll('.error-message').forEach(el => {
    el.textContent = '';
    el.parentElement.classList.remove('has-error');
  });
  
  Object.entries(errors).forEach(([field, message]) => {
    const input = document.querySelector(`[name="${field}"]`);
    const errorEl = input?.closest('.form-group')?.querySelector('.error-message');
    
    if (errorEl) {
      errorEl.textContent = message;
      errorEl.parentElement.classList.add('has-error');
    }
  });
}

document.getElementById('registrationForm').addEventListener('submit', function(e) {
  e.preventDefault();
  
  const formData = new FormData(this);
  const { valid, errors } = validateFormData(formData);
  
  if (!valid) {
    displayErrors(errors);
    return;
  }
  
  console.log('Form is valid:', valid);
});
```

### Example 4: Node.js Backend Validation

```javascript
// File: server/middleware/validation.js

import Joi from 'joi';

export const validateRequest = (schema) => {
  return (req, res, next) => {
    const { error, value } = schema.validate(req.body, {
      abortEarly: false,
      stripUnknown: true
    });
    
    if (error) {
      const errors = error.details.map(detail => ({
        field: detail.path.join('.'),
        message: detail.message
      }));
      
      return res.status(400).json({
        success: false,
        errors
      });
    }
    
    req.validatedBody = value;
    next();
  };
};

export const userSchema = Joi.object({
  email: Joi.string().email().required(),
  password: Joi.string()
    .min(8)
    .pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/)
    .required(),
  firstName: Joi.string().required(),
  lastName: Joi.string().required()
});

export const updateSchema = Joi.object({
  email: Joi.string().email(),
  firstName: Joi.string(),
  lastName: Joi.string()
}).min(1);

export const loginSchema = Joi.object({
  email: Joi.string().email().required(),
  password: Joi.string().required()
});
```

### Example 5: Real-Time Validation Hook

```javascript
// File: hooks/useValidation.js

import { useState, useCallback } from 'react';
import { z } from 'zod';

export function useValidation(schema) {
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  
  const validate = useCallback((name, value) => {
    try {
      schema.parse({ [name]: value });
      setErrors(prev => ({ ...prev, [name]: null }));
      return true;
    } catch (error) {
      if (error.errors) {
        const fieldError = error.errors.find(e => e.path[0] === name);
        const message = fieldError?.message || null;
        setErrors(prev => ({ ...prev, [name]: message }));
        return !message;
      }
      return false;
    }
  }, [schema]);
  
  const validateAll = useCallback((data) => {
    try {
      schema.parse(data);
      setErrors({});
      return true;
    } catch (error) {
      const newErrors = {};
      error.errors?.forEach(err => {
        const path = err.path.join('.');
        newErrors[path] = err.message;
      });
      setErrors(newErrors);
      return false;
    }
  }, [schema]);
  
  const handleBlur = useCallback((name) => {
    setTouched(prev => ({ ...prev, [name]: true }));
  }, []);
  
  const handleChange = useCallback((name, value) => {
    if (touched[name]) {
      validate(name, value);
    }
  }, [touched, validate]);
  
  const clearErrors = useCallback(() => setErrors({}), []);
  const clearTouched = useCallback(() => setTouched({}), []);
  
  return {
    errors,
    touched,
    validate,
    validateAll,
    handleBlur,
    handleChange,
    clearErrors,
    clearTouched,
    isValid: Object.values(errors).every(e => e === null)
  };
}
```

---

## Integration with Frameworks

### React Integration

```javascript
// With Formik + Yup
const formikConfig = {
  validationSchema: yup.object({...})
};

// With React Hook Form + Zod
const resolver = zodResolver(schema);
const { register, handleSubmit } = useForm({ resolver });
```

### Vue Integration

```javascript
// vee-validate with Yup
import { setErrors } from 'vee-validate';

const schema = yup.object({...});

// Validate on change
await schema.validate(value, { abortEarly: false });
```

### Node.js Integration

```javascript
// Express middleware with Joi
app.post('/users', validateRequest(userSchema), createUser);

// koa middleware
router.post('/users', validate(userSchema), createUser);
```

---

## Performance Considerations

### Lazy Validation

```javascript
const schema = z.lazy(() => z.object({
  id: z.string().uuid(),
  items: z.array(schema).max(100)
}));
```

### Caching Schemas

```javascript
const cachedSchema = new Map();

function getSchema(type) {
  if (!cachedSchema.has(type)) {
    cachedSchema.set(type, createSchema(type));
  }
  return cachedSchema.get(type);
}
```

### Validation Only What Changed

```javascript
function validateChanged(form, changedFields) {
  const partialSchema = {};
  
  changedFields.forEach(field => {
    partialSchema[field] = form.schema[field];
  });
  
  return z.object(partialSchema).safeParse(form.data);
}
```

---

## Key Takeaways

1. **Yup**: Best for React apps with Formik. Declarative schema with good error customization.

2. **Zod**: Best for TypeScript. Native type inference, immutability, excellent for runtime safety.

3. **Joi**: Most features but largest bundle. Best for complex server-side validation.

4. **All Support Async**: All three libraries support async validation for server checks.

5. **Schema Reuse**: Define schemas once, reuse across client and server for consistency.

---

## Common Pitfalls

1. **Wrong Import**: Make sure you're importing correctly (`import * as yup` vs `import { object } from 'yup'`).

2. **Not Catching Errors**: Always wrap validation in try/catch or use safeParse for better error handling.

3. **Schema Mutation**: Zod schemas are immutable. Use `.parse()` or `.optional()` carefully.

4. **TypeScript Types**: Zod infers types at parse time; other libraries need explicit typing.

5. **Performance**: For large forms, use lazy validation or partial validation.

---

## Cross-Reference

- Previous: [Advanced Form Patterns](./04_ADVANCED_FORM_PATTERNS.md)
- Related: [JavaScript Form Validation](./02_JAVASCRIPT_FORM_VALIDATION.md)
- Related: [Form Data Handling](./03_FORM_DATA_HANDLING.md)