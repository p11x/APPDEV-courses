# 📝 React Forms and Validation Complete Guide

## Building Robust Forms in React

---

## Table of Contents

1. [Form Fundamentals](#form-fundamentals)
2. [Controlled Components](#controlled-components)
3. [Form Validation Strategies](#form-validation-strategies)
4. [React Hook Form](#react-hook-form)
5. [Formik Full Guide](#formik-full-guide)
6. [Custom Validation Hooks](#custom-validation-hooks)
7. [Real-World Examples](#real-world-examples)
8. [Best Practices](#best-practices)

---

## Form Fundamentals

### Controlled Inputs

```jsx
import { useState } from 'react';

function SimpleForm() {
  const [values, setValues] = useState({
    name: '',
    email: '',
    password: ''
  });
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
  };
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(values);
  };
  
  return (
    <form onSubmit={handleSubmit}>
      <input
        name="name"
        value={values.name}
        onChange={handleChange}
        placeholder="Name"
      />
      <input
        name="email"
        value={values.email}
        onChange={handleChange}
        placeholder="Email"
      />
      <input
        name="password"
        type="password"
        value={values.password}
        onChange={handleChange}
        placeholder="Password"
      />
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Multiple Input Types

```jsx
function MultiInputForm() {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    bio: '',
    country: 'us',
    newsletter: false,
    gender: ''
  });
  
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };
  
  return (
    <form>
      <input
        name="username"
        value={formData.username}
        onChange={handleChange}
      />
      
      <textarea
        name="bio"
        value={formData.bio}
        onChange={handleChange}
      />
      
      <select
        name="country"
        value={formData.country}
        onChange={handleChange}
      >
        <option value="us">United States</option>
        <option value="uk">United Kingdom</option>
        <option value="ca">Canada</option>
      </select>
      
      <label>
        <input
          name="newsletter"
          type="checkbox"
          checked={formData.newsletter}
          onChange={handleChange}
        />
        Subscribe to newsletter
      </label>
    </form>
  );
}
```

---

## Controlled Components

### Text Input

```jsx
function TextInput({ label, name, value, onChange, error }) {
  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <input
        id={name}
        name={name}
        type="text"
        value={value}
        onChange={onChange}
        className={error ? 'error' : ''}
      />
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}
```

### Select Input

```jsx
function SelectInput({ label, name, value, onChange, options, error }) {
  return (
    <div className="form-group">
      <label htmlFor={name}>{label}</label>
      <select
        id={name}
        name={name}
        value={value}
        onChange={onChange}
        className={error ? 'error' : ''}
      >
        <option value="">Select...</option>
        {options.map(opt => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </select>
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}
```

### Radio Buttons

```jsx
function RadioGroup({ label, name, value, onChange, options, error }) {
  return (
    <div className="form-group">
      <label>{label}</label>
      {options.map(opt => (
        <label key={opt.value}>
          <input
            type="radio"
            name={name}
            value={opt.value}
            checked={value === opt.value}
            onChange={onChange}
          />
          {opt.label}
        </label>
      ))}
      {error && <span className="error-message">{error}</span>}
    </div>
  );
}
```

---

## Form Validation Strategies

### Client-Side Validation

```jsx
function validateForm(values) {
  const errors = {};
  
  if (!values.name) {
    errors.name = 'Name is required';
  } else if (values.name.length < 2) {
    errors.name = 'Name must be at least 2 characters';
  }
  
  if (!values.email) {
    errors.email = 'Email is required';
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(values.email)) {
    errors.email = 'Invalid email format';
  }
  
  if (!values.password) {
    errors.password = 'Password is required';
  } else if (values.password.length < 8) {
    errors.password = 'Password must be at least 8 characters';
  }
  
  if (!values.confirmPassword) {
    errors.confirmPassword = 'Please confirm password';
  } else if (values.password !== values.confirmPassword) {
    errors.confirmPassword = 'Passwords do not match';
  }
  
  return errors;
}
```

### Real-Time Validation

```jsx
function ValidatedForm() {
  const [values, setValues] = useState({ email: '' });
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  
  const validate = (field, value) => {
    const newErrors = { ...errors };
    
    if (field === 'email') {
      if (!value) {
        newErrors.email = 'Email is required';
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
        newErrors.email = 'Invalid email';
      } else {
        delete newErrors.email;
      }
    }
    
    setErrors(newErrors);
  };
  
  const handleBlur = (e) => {
    const { name, value } = e.target;
    setTouched(prev => ({ ...prev, [name]: true }));
    validate(name, value);
  };
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
    
    if (touched[name]) {
      validate(name, value);
    }
  };
  
  return (
    <form>
      <input
        name="email"
        value={values.email}
        onChange={handleChange}
        onBlur={handleBlur}
      />
      {touched.email && errors.email && (
        <span>{errors.email}</span>
      )}
    </form>
  );
}
```

---

## React Hook Form

### Installation

```bash
npm install react-hook-form
```

### Basic Usage

```jsx
import { useForm } from 'react-hook-form';

function ReactHookForm() {
  const { register, handleSubmit, formState: { errors } } = useForm();
  
  const onSubmit = (data) => {
    console.log(data);
  };
  
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input
        {...register('name', { required: 'Name is required' })}
        placeholder="Name"
      />
      {errors.name && <span>{errors.name.message}</span>}
      
      <input
        {...register('email', { 
          required: 'Email is required',
          pattern: {
            value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
            message: 'Invalid email'
          }
        })}
        placeholder="Email"
      />
      {errors.email && <span>{errors.email.message}</span>}
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Validation Rules

```jsx
const { register } = useForm({
  mode: 'onBlur'
});

<input
  {...register('password', {
    required: 'Password is required',
    minLength: {
      value: 8,
      message: 'Password must be at least 8 characters'
    },
    validate: {
      hasUpperCase: value => 
        /[A-Z]/.test(value) || 'Must contain uppercase',
      hasNumber: value => 
        /[0-9]/.test(value) || 'Must contain number'
    }
  })}
/>
```

### Form Provider

```jsx
import { FormProvider, useForm } from 'react-hook-form';

function MyForm() {
  const methods = useForm({
    defaultValues: {
      name: '',
      email: ''
    }
  });
  
  const onSubmit = (data) => {
    console.log(data);
  };
  
  return (
    <FormProvider {...methods}>
      <form onSubmit={methods.handleSubmit(onSubmit)}>
        <FormInput name="name" />
        <FormInput name="email" />
        <button type="submit">Submit</button>
      </form>
    </FormProvider>
  );
}

function FormInput({ name }) {
  const { register, formState: { errors } } = useFormContext();
  
  return (
    <div>
      <input {...register(name)} />
      {errors[name] && <span>{errors[name].message}</span>}
    </div>
  );
}
```

---

## Formik Full Guide

### Installation

```bash
npm install formik
```

### Basic Formik

```jsx
import { useFormik } from 'formik';

function FormikForm() {
  const formik = useFormik({
    initialValues: {
      name: '',
      email: '',
      password: ''
    },
    onSubmit: (values) => {
      console.log(values);
    },
    validate: (values) => {
      const errors = {};
      if (!values.name) errors.name = 'Required';
      if (!values.email) errors.email = 'Required';
      else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(values.email)) {
        errors.email = 'Invalid email';
      }
      return errors;
    }
  });
  
  return (
    <form onSubmit={formik.handleSubmit}>
      <input
        name="name"
        type="text"
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
        value={formik.values.name}
      />
      {formik.touched.name && formik.errors.name && (
        <div>{formik.errors.name}</div>
      )}
      
      <input
        name="email"
        type="email"
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
        value={formik.values.email}
      />
      {formik.touched.email && formik.errors.email && (
        <div>{formik.errors.email}</div>
      )}
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Formik with Yup Validation

```bash
npm install yup
```

```jsx
import { useFormik } from 'formik';
import * as Yup from 'yup';

const schema = Yup.object({
  name: Yup.string()
    .min(2, 'Too short')
    .required('Required'),
  email: Yup.string()
    .email('Invalid email')
    .required('Required'),
  password: Yup.string()
    .min(8, 'Must be at least 8 characters')
    .required('Required')
});

function YupForm() {
  const formik = useFormik({
    initialValues: {
      name: '',
      email: '',
      password: ''
    },
    validationSchema: schema,
    onSubmit: (values) => {
      console.log(values);
    }
  });
  
  return (
    <form onSubmit={formik.handleSubmit}>
      <input
        name="name"
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
        value={formik.values.name}
      />
      {formik.touched.name && formik.errors.name && (
        <div>{formik.errors.name}</div>
      )}
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

### Formik Form Component

```jsx
import { Formik, Form, Field, ErrorMessage } from 'formik';

function FormikComponent() {
  const schema = Yup.object({
    name: Yup.string().required('Required'),
    email: Yup.string().email('Invalid').required('Required')
  });
  
  return (
    <Formik
      initialValues={{ name: '', email: '' }}
      validationSchema={schema}
      onSubmit={(values) => console.log(values)}
    >
      <Form>
        <Field name="name" />
        <ErrorMessage name="name" component="span" />
        
        <Field name="email" type="email" />
        <ErrorMessage name="email" component="span" />
        
        <button type="submit">Submit</button>
      </Form>
    </Formik>
  );
}
```

---

## Custom Validation Hooks

### useField Hook

```jsx
import { useState, useCallback } from 'react';

function useField({ name, initialValue = '', validate }) {
  const [value, setValue] = useState(initialValue);
  const [error, setError] = useState('');
  const [touched, setTouched] = useState(false);
  
  const handleChange = useCallback((e) => {
    const newValue = e.target.value;
    setValue(newValue);
    
    if (validate) {
      const validationError = validate(newValue);
      setError(validationError || '');
    }
  }, [validate]);
  
  const handleBlur = useCallback(() => {
    setTouched(true);
    if (validate) {
      const validationError = validate(value);
      setError(validationError || '');
    }
  }, [validate, value]);
  
  return {
    name,
    value,
    error,
    touched,
    onChange: handleChange,
    onBlur: handleBlur
  };
}

// Usage
function CustomForm() {
  const nameField = useField({
    name: 'name',
    validate: (value) => {
      if (!value) return 'Name is required';
      if (value.length < 2) return 'Name too short';
      return '';
    }
  });
  
  return (
    <form>
      <input
        name={nameField.name}
        value={nameField.value}
        onChange={nameField.onChange}
        onBlur={nameField.onBlur}
      />
      {nameField.touched && nameField.error && (
        <span>{nameField.error}</span>
      )}
    </form>
  );
}
```

### useForm Hook

```jsx
import { useState, useCallback } from 'react';

function useForm({ initialValues, validate }) {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});
  
  const handleChange = useCallback((e) => {
    const { name, value } = e.target;
    setValues(prev => ({ ...prev, [name]: value }));
    
    if (validate) {
      const validationErrors = validate({ ...values, [name]: value });
      setErrors(validationErrors);
    }
  }, [validate, values]);
  
  const handleBlur = useCallback((e) => {
    const { name } = e.target;
    setTouched(prev => ({ ...prev, [name]: true }));
    
    if (validate) {
      const validationErrors = validate(values);
      setErrors(validationErrors);
    }
  }, [validate, values]);
  
  const handleSubmit = useCallback((e, onSubmit) => {
    e.preventDefault();
    setTouched(
      Object.keys(values).reduce((acc, key) => ({ ...acc, [key]: true }), {})
    );
    
    const validationErrors = validate(values);
    setErrors(validationErrors);
    
    if (Object.keys(validationErrors).length === 0) {
      onSubmit(values);
    }
  }, [validate, values]);
  
  const reset = useCallback(() => {
    setValues(initialValues);
    setErrors({});
    setTouched({});
  }, [initialValues]);
  
  return {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    handleSubmit,
    reset
  };
}

// Usage
function BetterForm() {
  const validate = (values) => {
    const errors = {};
    if (!values.name) errors.name = 'Required';
    if (!values.email) errors.email = 'Required';
    return errors;
  };
  
  const { values, errors, touched, handleChange, handleBlur, handleSubmit } = 
    useForm({
      initialValues: { name: '', email: '' },
      validate
    });
  
  return (
    <form onSubmit={(e) => handleSubmit(e, console.log)}>
      <input
        name="name"
        value={values.name}
        onChange={handleChange}
        onBlur={handleBlur}
      />
      {touched.name && errors.name && <span>{errors.name}</span>}
      
      <input
        name="email"
        value={values.email}
        onChange={handleChange}
        onBlur={handleBlur}
      />
      {touched.email && errors.email && <span>{errors.email}</span>}
      
      <button type="submit">Submit</button>
    </form>
  );
}
```

---

## Real-World Examples

### Registration Form

```jsx
import { useForm } from 'react-hook-form';
import * as Yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';

const schema = Yup.object({
  username: Yup.string()
    .min(3, 'Must be at least 3 characters')
    .max(20, 'Must be at most 20 characters')
    .required('Required'),
  email: Yup.string()
    .email('Invalid email')
    .required('Required'),
  password: Yup.string()
    .min(8, 'Must be at least 8 characters')
    .matches(/[A-Z]/, 'Must contain uppercase')
    .matches(/[0-9]/, 'Must contain number')
    .required('Required'),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref('password')], 'Must match password')
    .required('Required'),
  terms: Yup.boolean()
    .oneOf([true], 'Must accept terms')
    .required('Required')
});

function RegisterForm() {
  const { register, handleSubmit, formState: { errors, isSubmitting } } = 
    useForm({
      resolver: yupResolver(schema)
    });
  
  const onSubmit = async (data) => {
    await registerUser(data);
  };
  
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('username')} placeholder="Username" />
      {errors.username && <span>{errors.username.message}</span>}
      
      <input {...register('email')} type="email" placeholder="Email" />
      {errors.email && <span>{errors.email.message}</span>}
      
      <input {...register('password')} type="password" placeholder="Password" />
      {errors.password && <span>{errors.password.message}</span>}
      
      <input {...register('confirmPassword')} type="password" placeholder="Confirm Password" />
      {errors.confirmPassword && <span>{errors.confirmPassword.message}</span>}
      
      <label>
        <input {...register('terms')} type="checkbox" />
        I agree to the terms
      </label>
      {errors.terms && <span>{errors.terms.message}</span>}
      
      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Creating account...' : 'Register'}
      </button>
    </form>
  );
}
```

### Login Form with Remember Me

```jsx
import { useForm } from 'react-hook-form';

const schema = Yup.object({
  email: Yup.string().email('Invalid').required('Required'),
  password: Yup.string().required('Required'),
  rememberMe: Yup.boolean()
});

function LoginForm() {
  const { register, handleSubmit, setValue } = useForm();
  
  useEffect(() => {
    const saved = localStorage.getItem('rememberEmail');
    if (saved) setValue('email', saved);
  }, [setValue]);
  
  const onSubmit = (data) => {
    if (data.rememberMe) {
      localStorage.setItem('rememberEmail', data.email);
    }
    login(data);
  };
  
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('email')} type="email" placeholder="Email" />
      
      <input {...register('password')} type="password" placeholder="Password" />
      
      <label>
        <input {...register('rememberMe')} type="checkbox" />
        Remember me
      </label>
      
      <button type="submit">Login</button>
    </form>
  );
}
```

---

## Best Practices

### Do's and Don'ts

```
DO:
────────────────────────────────────────────
✅ Use controlled components for complex forms
✅ Validate on blur for better UX
✅ Show clear error messages
✅ Disable submit during processing
✅ Clear form after successful submission

DON'T:
────────────────────────────────────────────
❌ Don't validate on every keystroke (performance)
❌ Don't show errors before user interaction
❌ Don't use too many required fields
❌ Don't forget to handle form reset
```

### Performance Tips

```jsx
// ✅ Debounce validation
<input {...register('email', {
  validate: debounce(validateEmail, 500)
})} />

// ✅ Lazy validation
const [touched, setTouched] = useState({});

const handleBlur = (e) => {
  setTouched(prev => ({ ...prev, [e.target.name]: true }));
};
```

---

## Summary

### Key Takeaways

1. **Controlled Components**: Standard React approach
2. **React Hook Form**: Performance-focused solution
3. **Formik**: Full-featured form solution
4. **Validation**: Always validate both client and server
5. **UX**: Show errors only after interaction

### Next Steps

- Continue with: [07_REACT_PERFORMANCE_OPTIMIZATION.md](07_REACT_PERFORMANCE_OPTIMIZATION.md)
- Practice with different validation libraries
- Implement form testing

---

## Cross-References

- **Previous**: [05_REACT_STATE_MANAGEMENT.md](05_REACT_STATE_MANAGEMENT.md)
- **Next**: [07_REACT_PERFORMANCE_OPTIMIZATION.md](07_REACT_PERFORMANCE_OPTIMIZATION.md)

---

*Last updated: 2024*