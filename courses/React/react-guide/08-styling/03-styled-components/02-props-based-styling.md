# Props-Based Styling with Styled Components

## Overview

One of styled-components' most powerful features is the ability to dynamically change styles based on props. This guide covers passing props to styled components, creating variant patterns, using TypeScript with styled-components, and building flexible component APIs.

## Prerequisites

- Completed styled-components basics guide
- Familiarity with TypeScript generics
- Understanding of React component patterns

## Core Concepts

### Basic Props-Based Styling

Pass props to styled components to dynamically change styles:

```tsx
// File: src/components/Badge.tsx

import styled from 'styled-components';

// Define prop types
interface BadgeProps {
  $variant: 'success' | 'warning' | 'error' | 'info' | 'default';
  $size: 'small' | 'medium' | 'large';
}

// Color mappings based on variant prop
const variantColors = {
  success: { bg: '#d1fae5', text: '#065f46' },
  warning: { bg: '#fef3c7', text: '#92400e' },
  error: { bg: '#fee2e2', text: '#991b1b' },
  info: { bg: '#dbeafe', text: '#1e40af' },
  default: { bg: '#f3f4f6', text: '#374151' }
};

// Size mappings
const sizeStyles = {
  small: { padding: '0.125rem 0.5rem', fontSize: '0.6875rem' },
  medium: { padding: '0.25rem 0.75rem', fontSize: '0.75rem' },
  large: { padding: '0.375rem 1rem', fontSize: '0.875rem' }
};

// Create styled badge
const StyledBadge = styled.span<BadgeProps>`
  display: inline-flex;
  align-items: center;
  gap: 0.375rem;
  font-weight: 500;
  border-radius: 9999px;
  white-space: nowrap;
  
  /* Dynamic styles from props */
  background-color: ${({ $variant }) => variantColors[$variant].bg};
  color: ${({ $variant }) => variantColors[$variant].text};
  padding: ${({ $size }) => sizeStyles[$size].padding};
  font-size: ${({ $size }) => sizeStyles[$size].fontSize};
`;

function Badge({ 
  children, 
  variant = 'default', 
  size = 'medium' 
}: {
  children: React.ReactNode;
  variant?: 'success' | 'warning' | 'error' | 'info' | 'default';
  size?: 'small' | 'medium' | 'large';
}) {
  return (
    <StyledBadge $variant={variant} $size={size}>
      {children}
    </StyledBadge>
  );
}

export default Badge;
```

### Variant Patterns with TypeScript

Create flexible component variants using TypeScript:

```tsx
// File: src/components/Button.tsx

import styled, { css } from 'styled-components';

// Define variant and size types
type ButtonVariant = 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger';
type ButtonSize = 'sm' | 'md' | 'lg';

// Variant styles as CSS objects
const variantStyles: Record<ButtonVariant, ReturnType<typeof css>> = {
  primary: css`
    background-color: #3b82f6;
    color: white;
    &:hover:not(:disabled) { background-color: #2563eb; }
  `,
  secondary: css`
    background-color: #6b7280;
    color: white;
    &:hover:not(:disabled) { background-color: #4b5563; }
  `,
  outline: css`
    background-color: transparent;
    border: 2px solid #3b82f6;
    color: #3b82f6;
    &:hover:not(:disabled) { background-color: #eff6ff; }
  `,
  ghost: css`
    background-color: transparent;
    color: #374151;
    &:hover:not(:disabled) { background-color: #f3f4f6; }
  `,
  danger: css`
    background-color: #ef4444;
    color: white;
    &:hover:not(:disabled) { background-color: #dc2626; }
  `
};

// Size styles
const sizeStyles: Record<ButtonSize, ReturnType<typeof css>> = {
  sm: css`
    padding: 0.375rem 0.75rem;
    font-size: 0.875rem;
  `,
  md: css`
    padding: 0.5rem 1rem;
    font-size: 1rem;
  `,
  lg: css`
    padding: 0.75rem 1.5rem;
    font-size: 1.125rem;
  `
};

// Button props interface
interface ButtonProps {
  $variant?: ButtonVariant;
  $size?: ButtonSize;
  $fullWidth?: boolean;
}

// Styled button component
const StyledButton = styled.button<ButtonProps>`
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  border-radius: 0.5rem;
  border: none;
  cursor: pointer;
  transition: all 0.2s ease;
  
  /* Apply variant and size styles */
  ${({ $variant = 'primary' }) => variantStyles[$variant]}
  ${({ $size = 'md' }) => sizeStyles[$size]}
  
  /* Full width modifier */
  ${({ $fullWidth }) => $fullWidth && css`
    width: 100%;
  `}
  
  &:focus-visible {
    outline: 2px solid currentColor;
    outline-offset: 2px;
  }
  
  &:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }
`;

// Export styled component for external use
export { StyledButton };

// Main Button component
function Button({
  children,
  variant = 'primary',
  size = 'md',
  fullWidth = false,
  disabled = false,
  loading = false,
  ...props
}: {
  children: React.ReactNode;
  variant?: ButtonVariant;
  size?: ButtonSize;
  fullWidth?: boolean;
  disabled?: boolean;
  loading?: boolean;
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
      {loading && <span>...</span>}
      {children}
    </StyledButton>
  );
}

export default Button;
```

### Conditional Styling Based on State

Apply styles based on component state:

```tsx
// File: src/components/Toggle.tsx

import styled, { css } from 'styled-components';

interface ToggleProps {
  $checked: boolean;
  $disabled?: boolean;
}

const ToggleTrack = styled.div<ToggleProps>`
  position: relative;
  width: 3rem;
  height: 1.5rem;
  background-color: ${({ $checked }) => $checked ? '#3b82f6' : '#d1d5db'};
  border-radius: 9999px;
  cursor: ${({ $disabled }) => $disabled ? 'not-allowed' : 'pointer'};
  transition: background-color 0.2s ease;
  
  opacity: ${({ $disabled }) => $disabled ? 0.5 : 1};
`;

const ToggleThumb = styled.div<ToggleProps>`
  position: absolute;
  top: 0.125rem;
  left: ${({ $checked }) => $checked ? '1.625rem' : '0.125rem'};
  width: 1.25rem;
  height: 1.25rem;
  background-color: white;
  border-radius: 50%;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
  transition: left 0.2s ease;
  
  /* Scale effect when checked */
  transform: ${({ $checked }) => $checked ? 'scale(1.1)' : 'scale(1)'};
`;

function Toggle({ 
  checked, 
  onChange, 
  disabled = false 
}: {
  checked: boolean;
  onChange: (checked: boolean) => void;
  disabled?: boolean;
}) {
  return (
    <ToggleTrack 
      $checked={checked} 
      $disabled={disabled}
      onClick={() => !disabled && onChange(!checked)}
      role="switch"
      aria-checked={checked}
    >
      <ToggleThumb $checked={checked} $disabled={disabled} />
    </ToggleTrack>
  );
}

export default Toggle;
```

## Common Mistakes

### Passing Non-Transient Props

Always prefix styled-component props with $ to prevent them from being passed to the DOM:

```tsx
// ❌ WRONG - hasError becomes a DOM attribute
const Input = styled.input`
  border-color: ${props => props.hasError ? 'red' : 'gray'};
`;

// ✅ CORRECT - $hasError is transient
const Input = styled.input`
  border-color: ${props => props.$hasError ? 'red' : 'gray'};
`;
```

## Real-World Example

A complete flexible card component with variants:

```tsx
// File: src/components/Card.tsx

import styled, { css } from 'styled-components';

// Types
type CardVariant = 'default' | 'outlined' | 'elevated';
type CardPadding = 'none' | 'small' | 'medium' | 'large';

// Padding values
const paddingValues: Record<CardPadding, string> = {
  none: '0',
  small: '0.75rem',
  medium: '1.25rem',
  large: '2rem'
};

// Variant styles
const variantStyles: Record<CardVariant, ReturnType<typeof css>> = {
  default: css`
    background: white;
    border-radius: 12px;
    box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
    &:hover { box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15); }
  `,
  outlined: css`
    background: white;
    border-radius: 12px;
    border: 2px solid #e5e7eb;
    &:hover { border-color: #3b82f6; }
  `,
  elevated: css`
    background: white;
    border-radius: 12px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1), 0 2px 4px rgba(0, 0, 0, 0.06);
    &:hover { box-shadow: 0 10px 15px rgba(0, 0, 0, 0.1); }
  `
};

// Styled card
const StyledCard = styled.div<{
  $variant: CardVariant;
  $padding: CardPadding;
  $clickable: boolean;
}>`
  ${({ $variant }) => variantStyles[$variant]}
  padding: ${({ $padding }) => paddingValues[$padding]};
  overflow: hidden;
  transition: all 0.2s ease;
  
  /* Clickable variant */
  ${({ $clickable }) => $clickable && css`
    cursor: pointer;
    &:active { transform: scale(0.98); }
  `}
`;

// Card sections
const CardHeader = styled.div`
  padding-bottom: 1rem;
  border-bottom: 1px solid #e5e7eb;
  margin-bottom: 1rem;
`;

const CardTitle = styled.h3`
  font-size: 1.125rem;
  font-weight: 600;
  color: #111827;
  margin: 0;
`;

const CardContent = styled.div`
  color: #4b5563;
`;

const CardFooter = styled.div`
  padding-top: 1rem;
  border-top: 1px solid #e5e7eb;
  margin-top: 1rem;
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
`;

// Main Card component
function Card({
  children,
  title,
  variant = 'default',
  padding = 'medium',
  onClick,
  footer
}: {
  children?: React.ReactNode;
  title?: string;
  variant?: CardVariant;
  padding?: CardPadding;
  onClick?: () => void;
  footer?: React.ReactNode;
}) {
  return (
    <StyledCard 
      $variant={variant} 
      $padding={padding}
      $clickable={!!onClick}
      onClick={onClick}
    >
      {title && (
        <CardHeader>
          <CardTitle>{title}</CardTitle>
        </CardHeader>
      )}
      {children && <CardContent>{children}</CardContent>}
      {footer && <CardFooter>{footer}</CardFooter>}
    </StyledCard>
  );
}

export default Card;
```

## Key Takeaways

- Use transient props ($ prefix) for styling-only props
- Create variant and size type definitions for TypeScript safety
- Use Record types to map props to style values
- Apply conditional styles based on boolean props
- Compose styles using css helper for reusability
- Keep styling logic in styled-components, business logic in React

## What's Next

Continue to [Global Styles and Themes](/08-styling/03-styled-components/03-global-styles-and-themes.md) to learn how to create global styles, implement theming with ThemeProvider, and manage design tokens.