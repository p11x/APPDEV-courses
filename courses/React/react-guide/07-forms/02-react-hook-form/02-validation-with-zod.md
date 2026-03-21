# Form Validation with Zod

## Overview

Zod is a TypeScript-first schema validation library that works seamlessly with React Hook Form through the @hookform/resolvers package. Instead of defining validation rules inline in your register calls, Zod allows you to create reusable, composable validation schemas that provide excellent TypeScript type inference. This approach results in cleaner code and better maintainability as your forms grow in complexity.

## Prerequisites

- Completed the React Hook Form setup guide
- Understanding of TypeScript interfaces and types
- Familiarity with React Hook Form's register and formState
- Basic knowledge of form validation concepts

## Core Concepts

### Installing Zod and the Resolver

To use Zod with React Hook Form, you need to install both Zod and the resolver package that connects it to React Hook Form's validation system.

```bash
# File: terminal

# Install Zod - the schema validation library
npm install zod

# Install the resolver that connects Zod to React Hook Form
npm install @hookform/resolvers
```

### Defining Zod Schemas

Zod schemas define the shape and validation rules for your data. The schema is separate from your component, making it reusable across your application.

```tsx
// File: src/schemas/userSchema.ts

// Import z from zod - this is the main entry point for creating schemas
import { z } from 'zod';

// Create a schema using z.object() - this defines the shape of your data
// Each key corresponds to a form field
export const userSchema = z.object({
  // String validation with multiple chained refinements
  username: z
    .string()
    .min(3, "Username must be at least 3 characters")
    .max(20, "Username cannot exceed 20 characters")
    .regex(/^[a-zA-Z0-9_]+$/, "Username can only contain letters, numbers, and underscores"),
  
  // Email validation - zod has built-in email validation
  email: z
    .string()
    .email("Please enter a valid email address"),
  
  // Password with multiple validation rules
  password: z
    .string()
    .min(8, "Password must be at least 8 characters")
    .regex(/[A-Z]/, "Password must contain at least one uppercase letter")
    .regex(/[a-z]/, "Password must contain at least one lowercase letter")
    .regex(/[0-9]/, "Password must contain at least one number"),
  
  // Confirm password - will be validated against password in the form
  confirmPassword: z.string(),
  
  // Optional field with default
  bio: z
    .string()
    .max(500, "Bio cannot exceed 500 characters")
    .optional()
    .default(""),
  
  // Enum/select validation
  role: z
    .enum(['admin', 'user', 'guest'], {
      errorMap: () => ({ message: "Please select a valid role" })
    }),
  
  // Boolean checkbox
  agreeToTerms: z
    .boolean()
    .refine(val => val === true, {
      message: "You must agree to the terms"
    })
}).refine(data => data.password === data.confirmPassword, {
  // Cross-field validation - check that passwords match
  message: "Passwords do not match",
  path: ["confirmPassword"] // The error shows on this field
});

// Infer the TypeScript type from the schema
// This gives you a type that matches exactly what the schema validates
export type UserFormData = z.infer<typeof userSchema>;
```

### Connecting Zod Schema to React Hook Form

The resolver acts as a bridge between Zod schemas and React Hook Form. Instead of defining validation in register calls, you pass the schema to useForm's resolver option.

```tsx
// File: src/components/UserRegistrationForm.tsx

// Import useForm from react-hook-form
import { useForm } from 'react-hook-form';
// Import the zodResolver from @hookform/resolvers
import { zodResolver } from '@hookform/resolvers/zod';
// Import our schema and type
import { userSchema, type UserFormData } from '../schemas/userSchema';

function UserRegistrationForm() {
  // useForm now uses zodResolver as the validation strategy
  // The schema provides all validation rules
  const { 
    register, 
    handleSubmit, 
    formState: { errors, isSubmitting } 
  } = useForm<UserFormData>({
    // resolver tells React Hook Form how to validate
    // zodResolver is a function that takes our schema
    resolver: zodResolver(userSchema),
    // Default values ensure all fields start with valid data
    defaultValues: {
      username: "",
      email: "",
      password: "",
      confirmPassword: "",
      bio: "",
      role: "user",
      agreeToTerms: false
    }
  });

  const onSubmit = async (data: UserFormData) => {
    // Data is fully typed thanks to TypeScript inference from Zod
    console.log("Form submitted with:", data);
    
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1000));
    
    alert("Registration successful!");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      {/* Username */}
      <div>
        <label htmlFor="username">Username</label>
        <input
          id="username"
          {...register("username")}
          placeholder="Enter username"
        />
        {/* errors.username.message is typed as string | undefined */}
        {errors.username && (
          <span>{errors.username.message}</span>
        )}
      </div>

      {/* Email */}
      <div>
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          {...register("email")}
          placeholder="you@example.com"
        />
        {errors.email && (
          <span>{errors.email.message}</span>
        )}
      </div>

      {/* Password */}
      <div>
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          {...register("password")}
          placeholder="Create password"
        />
        {errors.password && (
          <span>{errors.password.message}</span>
        )}
      </div>

      {/* Confirm Password */}
      <div>
        <label htmlFor="confirmPassword">Confirm Password</label>
        <input
          id="confirmPassword"
          type="password"
          {...register("confirmPassword")}
          placeholder="Confirm password"
        />
        {errors.confirmPassword && (
          <span>{errors.confirmPassword.message}</span>
        )}
      </div>

      {/* Bio (Optional) */}
      <div>
        <label htmlFor="bio">Bio (optional)</label>
        <textarea
          id="bio"
          {...register("bio")}
          placeholder="Tell us about yourself"
        />
        {errors.bio && (
          <span>{errors.bio.message}</span>
        )}
      </div>

      {/* Role Select */}
      <div>
        <label htmlFor="role">Role</label>
        <select id="role" {...register("role")}>
          <option value="user">User</option>
          <option value="admin">Admin</option>
          <option value="guest">Guest</option>
        </select>
        {errors.role && (
          <span>{errors.role.message}</span>
        )}
      </div>

      {/* Terms Checkbox */}
      <div>
        <label>
          <input
            type="checkbox"
            {...register("agreeToTerms")}
          />
          I agree to the terms and conditions
        </label>
        {errors.agreeToTerms && (
          <span>{errors.agreeToTerms.message}</span>
        )}
      </div>

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? "Registering..." : "Register"}
      </button>
    </form>
  );
}

export default UserRegistrationForm;
```

### Field-Level Validation with Zod

While Zod is powerful for form-level validation, you can also use it for individual field validation when you need more granular control.

```tsx
// File: src/schemas/fieldSchemas.ts

import { z } from 'zod';

// Individual field schemas - useful when you want to validate 
// a single field independently
export const emailSchema = z.string().email("Invalid email format");

export const passwordSchema = z
  .string()
  .min(8, "Password too short")
  .regex(/[A-Z]/, "Must have uppercase")
  .regex(/[a-z]/, "Must have lowercase")
  .regex(/[0-9]/, "Must have number");

// Phone number with custom regex
export const phoneSchema = z
  .string()
  .regex(/^\+?[1-9]\d{1,14}$/, "Invalid phone number format");

// URL validation
export const urlSchema = z.string().url("Must be a valid URL");

// File: src/components/ProfileForm.tsx

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

// Create a form schema using the individual field schemas
const profileSchema = z.object({
  email: emailSchema,
  website: urlSchema.optional().or(z.literal("")),
  phone: phoneSchema.optional().or(z.literal(""))
});

type ProfileData = z.infer<typeof profileSchema>;

function ProfileForm() {
  const { register, handleSubmit, formState: { errors } } = useForm<ProfileData>({
    resolver: zodResolver(profileSchema)
  });

  return (
    <form onSubmit={handleSubmit(console.log)}>
      <input {...register("email")} placeholder="Email" />
      {errors.email && <span>{errors.email.message}</span>}
      
      <input {...register("website")} placeholder="Website URL" />
      {errors.website && <span>{errors.website.message}</span>}
      
      <input {...register("phone")} placeholder="Phone number" />
      {errors.phone && <span>{errors.phone.message}</span>}
      
      <button type="submit">Save Profile</button>
    </form>
  );
}
```

### Using refine() for Complex Validation

The refine method allows you to add custom validation logic that goes beyond simple type checking. This is useful for business logic validation.

```tsx
// File: src/schemas/complexValidation.ts

import { z } from 'zod';

// Example: Validate that a username isn't taken
// This uses refine to add custom async or sync validation
export const signupSchema = z.object({
  username: z
    .string()
    .min(3)
    .max(20)
    .refine(async (username) => {
      // This runs when the field loses focus
      // In reality, you'd call an API to check availability
      const response = await fetch(`/api/users/check?username=${username}`);
      const data = await response.json();
      return data.available; // Returns true if username is available
    }, {
      message: "This username is already taken"
    }),
  
  age: z
    .number()
    .int()
    .positive()
    .refine(age => age >= 13, {
      message: "You must be at least 13 years old"
    }),
    
  // Cross-field refinement example
  membership: z.object({
    type: z.enum(['free', 'premium']),
    couponCode: z.string().optional()
  })
}).refine(data => {
  // Premium members must provide a coupon code
  if (data.membership.type === 'premium' && !data.membership.couponCode) {
    return false;
  }
  return true;
}, {
  message: "Coupon code is required for premium membership",
  path: ["membership", "couponCode"] // Error attaches to this path
});
```

### Displaying Field-Level Errors

React Hook Form automatically integrates with Zod to provide detailed error messages. Here's how to display them effectively.

```tsx
// File: src/components/FormWithErrorDisplay.tsx

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';

// Define a schema
const loginSchema = z.object({
  email: z.string().min(1, "Email is required").email("Invalid email"),
  password: z.string().min(1, "Password is required").min(8, "Password too short"),
  rememberMe: z.boolean().optional()
});

type LoginData = z.infer<typeof loginSchema>;

function FormWithErrorDisplay() {
  const { register, handleSubmit, formState: { errors } } = useForm<LoginData>({
    resolver: zodResolver(loginSchema)
  });

  return (
    <form onSubmit={handleSubmit(console.log)}>
      {/* Email with inline error display */}
      <div className="field-wrapper">
        <label htmlFor="email">Email</label>
        <input
          id="email"
          type="email"
          {...register("email")}
          // Add error class when there's an error
          className={errors.email ? "input-error" : ""}
          aria-invalid={errors.email ? "true" : "false"}
          // Connect to error message for screen readers
          aria-describedby={errors.email ? "email-error" : undefined}
        />
        {/* Visual error message */}
        {errors.email && (
          <span id="email-error" className="error-message" role="alert">
            {errors.email.message}
          </span>
        )}
      </div>

      {/* Password with error display */}
      <div className="field-wrapper">
        <label htmlFor="password">Password</label>
        <input
          id="password"
          type="password"
          {...register("password")}
          className={errors.password ? "input-error" : ""}
          aria-invalid={errors.password ? "true" : "false"}
          aria-describedby={errors.password ? "password-error" : undefined}
        />
        {errors.password && (
          <span id="password-error" className="error-message" role="alert">
            {errors.password.message}
          </span>
        )}
      </div>

      {/* Checkbox without required validation */}
      <div className="field-wrapper">
        <label>
          <input type="checkbox" {...register("rememberMe")} />
          Remember me
        </label>
      </div>

      <button type="submit">Login</button>
    </form>
  );
}
```

## Common Mistakes

### Mistake 1: Not Handling Async Refine Properly

Async validation in Zod requires proper error handling and user feedback, as it can take time to complete.

```tsx
// ❌ WRONG - Async refine without proper handling
const usernameSchema = z.string().refine(async (val) => {
  const res = await fetch(`/api/check/${val}`);
  return res.ok;
}, { message: "Username taken" });

// The user sees no feedback while the async check runs!

// ✅ CORRECT - Use with isLoading from React Hook Form
const { 
  register, 
  handleSubmit, 
  formState: { errors },
  // You can check if a specific field is being validated
  formState: { 
    isValidating // True while any async validation runs
  } 
} = useForm({
  resolver: zodResolver(schema)
});

// Show loading indicator during async validation
{isValidating && <span>Validating...</span>}
```

### Mistake 2: Missing Default Values

When using Zod with React Hook Form, always provide default values. Otherwise, undefined values can cause type errors.

```tsx
// ❌ WRONG - No default values, fields start as undefined
const { register } = useForm({
  resolver: zodResolver(schema)
  // Missing defaultValues!
});
// This can cause "cannot read property of undefined" errors

// ✅ CORRECT - Always provide default values
const { register } = useForm({
  resolver: zodResolver(schema),
  defaultValues: {
    username: "",
    email: "",
    role: "user"
  }
});
```

### Mistake 3: Not Using the Path Parameter in refine

When doing cross-field validation, make sure to specify the correct path so errors show on the right field.

```tsx
// ❌ WRONG - Missing path, error shows at form root level
z.object({
  password: z.string().min(8),
  confirmPassword: z.string()
}).refine(data => data.password === data.confirmPassword, {
  message: "Passwords don't match"
  // Missing path! Error won't show on confirmPassword field
});

// ✅ CORRECT - Specify the path so error shows on the right field
z.object({
  password: z.string().min(8),
  confirmPassword: z.string()
}).refine(data => data.password === data.confirmPassword, {
  message: "Passwords don't match",
  path: ["confirmPassword"] // Error appears on confirmPassword field
});
```

## Real-World Example

Here's a complete checkout form with Zod validation showing address, payment, and shipping options:

```tsx
// File: src/schemas/checkoutSchema.ts

import { z } from 'zod';

// Define reusable address schema
const addressSchema = z.object({
  street: z.string().min(1, "Street address is required"),
  city: z.string().min(1, "City is required"),
  state: z.string().min(2, "State is required"),
  zipCode: z.string().regex(/^\d{5}(-\d{4})?$/, "Invalid ZIP code"),
  country: z.string().min(1, "Country is required")
});

// Main checkout schema
export const checkoutSchema = z.object({
  // Contact information
  email: z.string().email("Invalid email address"),
  phone: z.string().regex(/^\+?[1-9]\d{1,14}$/, "Invalid phone number"),
  
  // Shipping address (nested object)
  shippingAddress: addressSchema,
  
  // Billing address (can be same as shipping)
  sameAsBilling: z.boolean(),
  billingAddress: addressSchema.optional(),
  
  // Shipping method
  shippingMethod: z.enum(['standard', 'express', 'overnight'], {
    errorMap: () => ({ message: "Please select a shipping method" })
  }),
  
  // Payment information
  cardName: z.string().min(1, "Name on card is required"),
  cardNumber: z.string().regex(/^\d{16}$/, "Card number must be 16 digits"),
  expiryDate: z.string().regex(/^(0[1-9]|1[0-2])\/\d{2}$/, "Use MM/YY format"),
  cvv: z.string().regex(/^\d{3,4}$/, "CVV must be 3-4 digits"),
  
  // Order notes
  notes: z.string().max(500).optional()
}).refine(data => {
  // If "same as billing" is unchecked, billing address is required
  if (!data.sameAsBilling && !data.billingAddress) {
    return false;
  }
  return true;
}, {
  message: "Billing address is required",
  path: ["billingAddress"]
}).refine(data => {
  // Express and overnight shipping require a valid phone number
  if (['express', 'overnight'].includes(data.shippingMethod)) {
    return data.phone.length > 10;
  }
  return true;
}, {
  message: "Phone number required for express shipping",
  path: ["phone"]
});

export type CheckoutFormData = z.infer<typeof checkoutSchema>;

// File: src/components/CheckoutForm.tsx

import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { checkoutSchema, type CheckoutFormData } from '../schemas/checkoutSchema';

function CheckoutForm() {
  const { 
    register, 
    handleSubmit, 
    formState: { errors, isSubmitting },
    watch 
  } = useForm<CheckoutFormData>({
    resolver: zodResolver(checkoutSchema),
    defaultValues: {
      email: "",
      phone: "",
      sameAsBilling: true,
      shippingMethod: "standard",
      shippingAddress: {
        street: "",
        city: "",
        state: "",
        zipCode: "",
        country: "US"
      }
    }
  });

  // Watch the sameAsBilling checkbox to conditionally show billing address
  const sameAsBilling = watch("sameAsBilling");

  const onSubmit = async (data: CheckoutFormData) => {
    console.log("Checkout data:", data);
    // Process payment and create order
    await new Promise(resolve => setTimeout(resolve, 2000));
    alert("Order placed successfully!");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="checkout-form">
      <h2>Checkout</h2>
      
      {/* Contact Section */}
      <section>
        <h3>Contact Information</h3>
        
        <div className="field">
          <label htmlFor="email">Email</label>
          <input id="email" type="email" {...register("email")} />
          {errors.email && <span className="error">{errors.email.message}</span>}
        </div>
        
        <div className="field">
          <label htmlFor="phone">Phone</label>
          <input id="phone" type="tel" {...register("phone")} />
          {errors.phone && <span className="error">{errors.phone.message}</span>}
        </div>
      </section>

      {/* Shipping Address */}
      <section>
        <h3>Shipping Address</h3>
        
        <div className="field">
          <label htmlFor="shippingAddress.street">Street</label>
          <input id="shippingAddress.street" {...register("shippingAddress.street")} />
          {errors.shippingAddress?.street && (
            <span className="error">{errors.shippingAddress.street.message}</span>
          )}
        </div>
        
        <div className="field-row">
          <div className="field">
            <label htmlFor="shippingAddress.city">City</label>
            <input id="shippingAddress.city" {...register("shippingAddress.city")} />
            {errors.shippingAddress?.city && (
              <span className="error">{errors.shippingAddress.city.message}</span>
            )}
          </div>
          
          <div className="field">
            <label htmlFor="shippingAddress.state">State</label>
            <input id="shippingAddress.state" {...register("shippingAddress.state")} />
            {errors.shippingAddress?.state && (
              <span className="error">{errors.shippingAddress.state.message}</span>
            )}
          </div>
          
          <div className="field">
            <label htmlFor="shippingAddress.zipCode">ZIP Code</label>
            <input id="shippingAddress.zipCode" {...register("shippingAddress.zipCode")} />
            {errors.shippingAddress?.zipCode && (
              <span className="error">{errors.shippingAddress.zipCode.message}</span>
            )}
          </div>
        </div>
      </section>

      {/* Billing Address */}
      <section>
        <h3>Billing Address</h3>
        
        <div className="checkbox-field">
          <label>
            <input type="checkbox" {...register("sameAsBilling")} />
            Same as shipping address
          </label>
        </div>
        
        {!sameAsBilling && (
          <div className="billing-fields">
            {/* Same structure as shipping address for billing */}
            <div className="field">
              <label htmlFor="billingAddress.street">Street</label>
              <input id="billingAddress.street" {...register("billingAddress.street")} />
              {errors.billingAddress?.street && (
                <span className="error">{errors.billingAddress.street.message}</span>
              )}
            </div>
            {/* More billing fields... */}
          </div>
        )}
      </section>

      {/* Shipping Method */}
      <section>
        <h3>Shipping Method</h3>
        
        <div className="radio-group">
          <label>
            <input type="radio" {...register("shippingMethod")} value="standard" />
            Standard Shipping (5-7 days) - Free
          </label>
          
          <label>
            <input type="radio" {...register("shippingMethod")} value="express" />
            Express Shipping (2-3 days) - $9.99
          </label>
          
          <label>
            <input type="radio" {...register("shippingMethod")} value="overnight" />
            Overnight Shipping - $19.99
          </label>
        </div>
        {errors.shippingMethod && (
          <span className="error">{errors.shippingMethod.message}</span>
        )}
      </section>

      {/* Payment Information */}
      <section>
        <h3>Payment Information</h3>
        
        <div className="field">
          <label htmlFor="cardName">Name on Card</label>
          <input id="cardName" {...register("cardName")} />
          {errors.cardName && <span className="error">{errors.cardName.message}</span>}
        </div>
        
        <div className="field">
          <label htmlFor="cardNumber">Card Number</label>
          <input 
            id="cardNumber" 
            {...register("cardNumber")} 
            placeholder="1234567812345678"
            maxLength={16}
          />
          {errors.cardNumber && <span className="error">{errors.cardNumber.message}</span>}
        </div>
        
        <div className="field-row">
          <div className="field">
            <label htmlFor="expiryDate">Expiry Date</label>
            <input 
              id="expiryDate" 
              {...register("expiryDate")} 
              placeholder="MM/YY"
            />
            {errors.expiryDate && <span className="error">{errors.expiryDate.message}</span>}
          </div>
          
          <div className="field">
            <label htmlFor="cvv">CVV</label>
            <input 
              id="cvv" 
              {...register("cvv")} 
              maxLength={4}
              type="password"
            />
            {errors.cvv && <span className="error">{errors.cvv.message}</span>}
          </div>
        </div>
      </section>

      <button type="submit" disabled={isSubmitting} className="submit-btn">
        {isSubmitting ? "Processing..." : "Place Order"}
      </button>
    </form>
  );
}

export default CheckoutForm;
```

## Key Takeaways

- Zod provides TypeScript-first schema validation that infers types automatically
- The @hookform/resolvers package connects Zod schemas to React Hook Form
- Use z.object() to define the shape of your form data
- Chain validation methods like .min(), .max(), .email(), and .regex() for field validation
- Use .refine() for complex cross-field validation and custom validation logic
- Always provide default values in useForm when using Zod resolvers
- The .optional() method allows fields to be undefined, while .default() provides fallback values

## What's Next

Continue to [Dynamic Form Fields](/07-forms/02-react-hook-form/03-dynamic-form-fields.md) to learn how to handle variable-length forms like adding/removing line items, using React Hook Form's useFieldArray hook.