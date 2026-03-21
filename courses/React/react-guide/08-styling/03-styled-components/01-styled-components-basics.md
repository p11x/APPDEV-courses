# Styled Components Basics

## Overview

Styled Components is a CSS-in-JS library that allows you to write CSS directly inside your JavaScript/TypeScript files using tagged template literals. It provides component-level styling, automatic critical CSS injection, and theming support out of the box. This guide covers the basics of creating styled components, extending styles, and naming conventions.

## Prerequisites

- Basic understanding of CSS
- Familiarity with React components
- Understanding of JavaScript template literals

## Core Concepts

### Installing Styled Components

```bash
# File: terminal

# Install styled-components
npm install styled-components

# Install TypeScript types
npm install @types/styled-components --save-dev
```

### Creating Basic Styled Components

Styled Components use tagged template literals to create components with built-in styles:

```tsx
// File: src/components/Button.tsx

// Import styled from styled-components
import styled from 'styled-components';

// Create a styled button component
// The styled.button template creates a button element with these styles
const StyledButton = styled.button`
  /* Base styles */
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  
  /* Primary variant colors */
  background-color: #3b82f6;
  color: white;
  
  /* Focus styles for accessibility */
  &:focus-visible {
    outline: 2px solid #3b82f6;
    outline-offset: 2px;
  }
  
  /* Hover state */
  &:hover {
    background-color: #2563eb;
  }
  
  /* Active/pressed state */
  &:active {
    transform: scale(0.98);
  }
  
  /* Disabled state */
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

// Use the styled component like a regular React component
function Button({ children, onClick, disabled, type = 'button' }: {
  children: React.ReactNode;
  onClick?: () => void;
  disabled?: boolean;
  type?: 'button' | 'submit' | 'reset';
}) {
  return (
    <StyledButton 
      onClick={onClick} 
      disabled={disabled}
      type={type}
    >
      {children}
    </StyledButton>
  );
}

export default Button;
```

### Extending Styles

Create variations of components by extending existing styled components:

```tsx
// File: src/components/ButtonVariants.tsx

import styled, { css } from 'styled-components';

// Base button styles shared by all variants
const buttonBase = css`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 0.75rem 1.5rem;
  font-size: 1rem;
  font-weight: 600;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

// Primary variant
export const PrimaryButton = styled.button`
  ${buttonBase}
  background-color: #3b82f6;
  color: white;
  
  &:hover:not(:disabled) {
    background-color: #2563eb;
  }
`;

// Secondary variant
export const SecondaryButton = styled.button`
  ${buttonBase}
  background-color: #6b7280;
  color: white;
  
  &:hover:not(:disabled) {
    background-color: #4b5563;
  }
`;

// Outline variant
export const OutlineButton = styled.button`
  ${buttonBase}
  background-color: transparent;
  border: 2px solid #3b82f6;
  color: #3b82f6;
  
  &:hover:not(:disabled) {
    background-color: #eff6ff;
  }
`;

// Ghost variant - no background
export const GhostButton = styled.button`
  ${buttonBase}
  background-color: transparent;
  color: #374151;
  
  &:hover:not(:disabled) {
    background-color: #f3f4f6;
  }
`;

// Danger variant for destructive actions
export const DangerButton = styled.button`
  ${buttonBase}
  background-color: #ef4444;
  color: white;
  
  &:hover:not(:disabled) {
    background-color: #dc2626;
  }
`;
```

### Using Props for Dynamic Styling

Pass props to styled components for dynamic styling:

```tsx
// File: src/components/Input.tsx

import styled from 'styled-components';

interface InputProps {
  // Size variant prop
  $size?: 'small' | 'medium' | 'large';
  // Error state prop
  $hasError?: boolean;
}

// Create styled input that responds to props
const StyledInput = styled.input<InputProps>`
  /* Base styles */
  width: 100%;
  padding: 0.625rem 0.875rem;
  font-size: 1rem;
  border: 1px solid #d1d5db;
  border-radius: 0.5rem;
  background: white;
  transition: all 0.15s ease;
  
  /* Size variants via props */
  padding: ${({ $size }) => {
    switch ($size) {
      case 'small': return '0.375rem 0.75rem';
      case 'large': return '0.875rem 1.25rem';
      default: return '0.625rem 0.875rem';
    }
  }};
  
  font-size: ${({ $size }) => 
    $size === 'small' ? '0.875rem' : 
    $size === 'large' ? '1.125rem' : '1rem'
  };
  
  /* Error state styling */
  border-color: ${({ $hasError }) => $hasError ? '#ef4444' : '#d1d5db'};
  
  &:focus {
    outline: none;
    border-color: ${({ $hasError }) => $hasError ? '#ef4444' : '#3b82f6'};
    box-shadow: 0 0 0 3px ${({ $hasError }) => 
      $hasError ? 'rgba(239, 68, 68, 0.1)' : 'rgba(59, 130, 246, 0.1)'
    };
  }
  
  &:disabled {
    background-color: #f9fafb;
    cursor: not-allowed;
    opacity: 0.6;
  }
`;

// Label styled component
const Label = styled.label`
  display: block;
  font-size: 0.875rem;
  font-weight: 500;
  color: #374151;
  margin-bottom: 0.375rem;
`;

// Error message styled component
const ErrorText = styled.span`
  display: block;
  font-size: 0.75rem;
  color: #ef4444;
  margin-top: 0.375rem;
`;

// Complete Input component
function Input({ 
  label, 
  error, 
  size = 'medium',
  ...props 
}: {
  label?: string;
  error?: string;
  size?: 'small' | 'medium' | 'large';
  [key: string]: any;
}) {
  return (
    <div>
      {label && <Label>{label}</Label>}
      <StyledInput 
        $size={size} 
        $hasError={!!error}
        aria-invalid={!!error}
        aria-describedby={error ? 'error-message' : undefined}
        {...props}
      />
      {error && <ErrorText id="error-message">{error}</ErrorText>}
    </div>
  );
}

export default Input;
```

### Creating Semantic Components

Use semantic HTML elements with styled-components:

```tsx
// File: src/components/Card.tsx

import styled, { css } from 'styled-components';

// Card container
const CardContainer = styled.div`
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: all 0.2s ease;
  
  &:hover {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
`;

// Card header
const CardHeader = styled.div`
  padding: 1.25rem;
  border-bottom: 1px solid #e5e7eb;
`;

// Card title
const CardTitle = styled.h3`
  font-size: 1.125rem;
  font-weight: 600;
  color: #111827;
  margin: 0;
`;

// Card subtitle
const CardSubtitle = styled.p`
  font-size: 0.875rem;
  color: #6b7280;
  margin: 0.25rem 0 0 0;
`;

// Card body
const CardBody = styled.div`
  padding: 1.25rem;
`;

// Card footer
const CardFooter = styled.div`
  padding: 1rem 1.25rem;
  background: #f9fafb;
  border-top: 1px solid #e5e7eb;
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
`;

// Card component
function Card({ 
  title, 
  subtitle, 
  children, 
  footer 
}: {
  title?: string;
  subtitle?: string;
  children?: React.ReactNode;
  footer?: React.ReactNode;
}) {
  return (
    <CardContainer>
      {(title || subtitle) && (
        <CardHeader>
          {title && <CardTitle>{title}</CardTitle>}
          {subtitle && <CardSubtitle>{subtitle}</CardSubtitle>}
        </CardHeader>
      )}
      {children && <CardBody>{children}</CardBody>}
      {footer && <CardFooter>{footer}</CardFooter>}
    </CardContainer>
  );
}

export default Card;
```

## Common Mistakes

### Not Using Transient Props

Styled-components will pass all props to the underlying DOM element, which can cause warnings. Use $ prefix for props that shouldn't render as attributes.

```tsx
// ❌ WRONG - $hasError renders as DOM attribute
const Input = styled.input`
  border-color: ${props => props.$hasError ? 'red' : 'gray'};
`;
<input $hasError /> // Warning: Unknown prop $hasError

// ✅ CORRECT - Use $ prefix for transient props
const Input = styled.input`
  border-color: ${props => props.$hasError ? 'red' : 'gray'};
`;
<input $hasError /> // Works correctly
```

## Real-World Example

A complete button component with variants, sizes, and states:

```tsx
// File: src/components/Button/Button.tsx

import styled, { css } from 'styled-components';

// Type definitions for props
type ButtonVariant = 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger';
type ButtonSize = 'small' | 'medium' | 'large';

// Base styles shared by all variants
const baseStyles = css`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-weight: 600;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  position: relative;
  
  &:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
  }
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

// Size variants
const sizeStyles = {
  small: css`
    padding: 0.375rem 0.75rem;
    font-size: 0.875rem;
  `,
  medium: css`
    padding: 0.5rem 1rem;
    font-size: 1rem;
  `,
  large: css`
    padding: 0.75rem 1.5rem;
    font-size: 1.125rem;
  `
};

// Color variants
const variantStyles = {
  primary: css`
    ${baseStyles}
    background-color: #3b82f6;
    color: white;
    
    &:hover:not(:disabled) {
      background-color: #2563eb;
    }
  `,
  secondary: css`
    ${baseStyles}
    background-color: #6b7280;
    color: white;
    
    &:hover:not(:disabled) {
      background-color: #4b5563;
    }
  `,
  outline: css`
    ${baseStyles}
    background-color: transparent;
    border: 2px solid #3b82f6;
    color: #3b82f6;
    
    &:hover:not(:disabled) {
      background-color: #eff6ff;
    }
  `,
  ghost: css`
    ${baseStyles}
    background-color: transparent;
    color: #374151;
    
    &:hover:not(:disabled) {
      background-color: #f3f4f6;
    }
  `,
  danger: css`
    ${baseStyles}
    background-color: #ef4444;
    color: white;
    
    &:hover:not(:disabled) {
      background-color: #dc2626;
    }
  `
};

// FullWidth prop
const fullWidthStyles = css`
  width: 100%;
`;

// Styled component definition
const StyledButton = styled.button<{
  $variant: ButtonVariant;
  $size: ButtonSize;
  $fullWidth: boolean;
}>`
  ${({ $variant }) => variantStyles[$variant]}
  ${({ $size }) => sizeStyles[$size]}
  ${({ $fullWidth }) => $fullWidth && fullWidthStyles}
`;

// Loading spinner
const Spinner = styled.span`
  width: 1rem;
  height: 1rem;
  border: 2px solid currentColor;
  border-top-color: transparent;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
  
  @keyframes spin {
    to { transform: rotate(360deg); }
  }
`;

// Button component
function Button({
  children,
  variant = 'primary',
  size = 'medium',
  fullWidth = false,
  loading = false,
  disabled = false,
  ...props
}: {
  children: React.ReactNode;
  variant?: ButtonVariant;
  size?: ButtonSize;
  fullWidth?: boolean;
  loading?: boolean;
  disabled?: boolean;
  onClick?: () => void;
  type?: 'button' | 'submit' | 'reset';
  [key: string]: any;
}) {
  return (
    <StyledButton
      $variant={variant}
      $size={size}
      $fullWidth={fullWidth}
      disabled={disabled || loading}
      {...props}
    >
      {loading && <Spinner aria-hidden="true" />}
      {children}
    </StyledButton>
  );
}

export default Button;
```

## Key Takeaways

- Use tagged template literals with styled to create styled components
- Import styled from 'styled-components' and use styled.htmlElement
- Pass props to styled components using template literal interpolation
- Use $ prefix for transient props that shouldn't render to DOM
- Extend styles using css helper or by composing styled components
- Create semantic components for better accessibility
- Use variants and sizes to create flexible, reusable components

## What's Next

Continue to [Props-Based Styling](/08-styling/03-styled-components/02-props-based-styling.md) to learn advanced patterns for creating dynamic styles based on component props and variants.