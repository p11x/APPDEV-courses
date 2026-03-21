# Typing Props and State

## Overview
Properly typing React component props and state is fundamental to building type-safe applications. Props are the inputs to components, and state is the internal data that changes over time. TypeScript provides powerful ways to define both, including handling optional props, default values, and discriminated unions for complex UI states. This guide covers all aspects of typing props and state in React components, with practical examples for real-world scenarios.

## Prerequisites
- TypeScript basics and generics
- Understanding of React functional components
- Familiarity with useState and useEffect hooks

## Core Concepts

### Basic Prop Typing
The foundation of type-safe React components starts with properly typed props:

```typescript
// [File: src/components/BasicProps.tsx]
import React from 'react';

// ======== Required Props ========
// Define the shape of your props with an interface
interface WelcomeProps {
  // All properties are required by default
  name: string;
  age: number;
}

// Component with typed props
function Welcome({ name, age }: WelcomeProps) {
  return (
    <div>
      <h1>Hello, {name}!</h1>
      <p>You are {age} years old.</p>
    </div>
  );
}

// Usage - TypeScript will error if name or age is missing
// <Welcome name="Alice" age={30} /> ✅
// <Welcome name="Alice" /> ❌ Error: age is required

// ======== Optional Props ========
// Use ? to mark props as optional
interface CardProps {
  title: string;
  description?: string; // Optional - may or may not be provided
  footer?: React.ReactNode; // Optional JSX
  variant?: 'default' | 'outlined' | 'elevated'; // Optional with default
}

function Card({ 
  title, 
  description, 
  footer,
  variant = 'default' 
}: CardProps) {
  return (
    <div className={`card card-${variant}`}>
      <h3>{title}</h3>
      {description && <p>{description}</p>}
      {footer && <div className="card-footer">{footer}</div>}
    </div>
  );
}

// Usage - all optional props can be omitted
// <Card title="Hello" /> ✅
// <Card title="Hello" description="World" variant="elevated" /> ✅
```

### Default Prop Values
There are two main approaches to handling default prop values in TypeScript:

```typescript
// [File: src/components/WithDefaults.tsx]

// Approach 1: Default values in destructuring
interface ButtonProps {
  label: string;
  variant?: 'primary' | 'secondary' | 'danger';
  size?: 'small' | 'medium' | 'large';
  disabled?: boolean;
  onClick?: () => void;
}

function Button({
  label,
  variant = 'primary',    // Default value inline
  size = 'medium',
  disabled = false,
  onClick,
}: ButtonProps) {
  return (
    <button 
      className={`btn btn-${variant} btn-${size}`}
      disabled={disabled}
      onClick={onClick}
    >
      {label}
    </button>
  );
}

// Approach 2: DefaultProps (less common in modern React)
// Note: This approach has issues with strict TypeScript
interface LegacyButtonProps {
  label: string;
  variant: 'primary' | 'secondary';
}

// Legacy approach - not recommended for new code
const LegacyButton: React.FC<LegacyButtonProps> = ({
  label,
  variant = 'primary',
}) => (
  <button className={`btn btn-${variant}`}>{label}</button>
);

// DefaultProps approach (older pattern)
// LegacyButton.defaultProps = {
//   variant: 'primary',
// };
```

### Discriminated Union Props
One of the most powerful patterns for complex UI states is discriminated unions:

```typescript
// [File: src/components/AsyncContent.tsx]
import React from 'react';

// Define possible states as a discriminated union
// The 'status' property is the discriminator - TypeScript uses it to narrow types
type ContentState<T> = 
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; error: Error };

interface AsyncContentProps<T> {
  state: ContentState<T>;
  idleContent?: React.ReactNode;
  loadingContent?: React.ReactNode;
  errorContent?: (error: Error) => React.ReactNode;
  children: (data: T) => React.ReactNode;
}

// Generic component that handles all async states
function AsyncContent<T>({
  state,
  idleContent = 'Click to load',
  loadingContent = 'Loading...',
  errorContent = (error) => `Error: ${error.message}`,
  children,
}: AsyncContentProps<T>) {
  // TypeScript narrows the type based on status!
  switch (state.status) {
    case 'idle':
      return <div className="idle">{idleContent}</div>;
      
    case 'loading':
      return <div className="loading">{loadingContent}</div>;
      
    case 'error':
      // state.error is guaranteed to exist here
      return <div className="error">{errorContent(state.error)}</div>;
      
    case 'success':
      // state.data is guaranteed to exist here
      return <div className="success">{children(state.data)}</div>;
      
    default:
      // Exhaustiveness check - ensures all cases are handled
      const _exhaustive: never = state;
      return _exhaustive;
  }
}

// Usage Example
interface User {
  id: number;
  name: string;
  email: string;
}

function UserProfile({ userId }: { userId: number }) {
  const [userState, setUserState] = React.useState<ContentState<User>>({
    status: 'idle',
  });

  const loadUser = async () => {
    setUserState({ status: 'loading' });
    try {
      const response = await fetch(`/api/users/${userId}`);
      if (!response.ok) throw new Error('Failed to fetch');
      const data = await response.json();
      setUserState({ status: 'success', data });
    } catch (error) {
      setUserState({ 
        status: 'error', 
        error: error instanceof Error ? error : new Error('Unknown error') 
      });
    }
  };

  return (
    <div>
      <button onClick={loadUser}>Load User</button>
      <AsyncContent
        state={userState}
        loadingContent={<Spinner />}
        children={(user) => (
          <div>
            <h2>{user.name}</h2>
            <p>{user.email}</p>
          </div>
        )}
      />
    </div>
  );
}
```

### Typing Component State
State in React can contain simple values or complex objects:

```typescript
// [File: src/components/FormWithState.tsx]
import React from 'react';

// ======== Simple State ========
function Counter() {
  // TypeScript infers number from initial value 0
  const [count, setCount] = React.useState(0);
  
  // Explicit typing when needed
  const [name, setName] = React.useState<string>('');
  
  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={() => setCount(c => c + 1)}>Increment</button>
    </div>
  );
}

// ======== Complex Object State ========
interface User {
  id: number;
  name: string;
  email: string;
  role: 'admin' | 'user';
}

interface FormState {
  values: User;
  errors: Partial<Record<keyof User, string>>;
  touched: Partial<Record<keyof User, boolean>>;
  isSubmitting: boolean;
}

function UserForm({ initialUser }: { initialUser?: User }) {
  const [state, setState] = React.useState<FormState>({
    values: initialUser ?? {
      id: 0,
      name: '',
      email: '',
      role: 'user',
    },
    errors: {},
    touched: {},
    isSubmitting: false,
  });

  const handleChange = (field: keyof User, value: string) => {
    setState(prev => ({
      ...prev,
      values: { ...prev.values, [field]: value },
      // Clear error when user types
      errors: { ...prev.errors, [field]: undefined },
    }));
  };

  const handleBlur = (field: keyof User) => {
    setState(prev => ({
      ...prev,
      touched: { ...prev.touched, [field]: true },
    }));
  };

  // Type-safe field access
  const getError = (field: keyof User): string | undefined => {
    return state.touched[field] ? state.errors[field] : undefined;
  };

  return (
    <form>
      <input
        value={state.values.name}
        onChange={(e) => handleChange('name', e.target.value)}
        onBlur={() => handleBlur('name')}
        error={getError('name')}
      />
      <input
        value={state.values.email}
        onChange={(e) => handleChange('email', e.target.value)}
        onBlur={() => handleBlur('email')}
        error={getError('email')}
      />
    </form>
  );
}
```

### Type Narrowing in JSX
TypeScript can narrow types inside JSX based on conditions:

```typescript
// [File: src/components/ConditionalRender.tsx]
import React from 'react'];

interface User {
  name: string;
  avatar?: string;
}

interface Guest {
  isGuest: true;
}

type Person = User | Guest;

function Profile({ person }: { person: Person }) {
  return (
    <div>
      {/* TypeScript knows: */}
      {/* If person has isGuest: true, it has isGuest property */}
      {/* Otherwise, it has name and optional avatar */}
      
      {person.isGuest ? (
        <p>Welcome, Guest!</p>
      ) : (
        // TypeScript narrows to User here
        <div>
          <h2>{person.name}</h2>
          {/* Optional chaining - avatar might not exist */}
          {person.avatar && (
            <img src={person.avatar} alt={`${person.name}'s avatar`} />
          )}
        </div>
      )}
    </div>
  );
}

// Another example with null checks
function DisplayName({ name }: { name: string | null }) {
  // TypeScript knows name could be null
  // Using || provides a fallback
  const displayName = name ?? 'Anonymous';
  
  return <h1>{displayName}</h1>;
}
```

## Common Mistakes

### Mistake 1: Not Typing Event Handlers
```typescript
// ❌ WRONG - Event type is any
const handleChange = (e) => {
  console.log(e.target.value); // No type safety!
};

// ✅ CORRECT - Type the event
const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  console.log(e.target.value); // Properly typed!
};
```

### Mistake 2: Forgetting Optional Chaining
```typescript
// ❌ WRONG - Assuming object exists
function UserName({ user }: { user?: User }) {
  return <h1>{user.name}</h1>; // Error if user is undefined!
}

// ✅ CORRECT - Handle potential undefined
function UserName({ user }: { user?: User }) {
  return <h1>{user?.name ?? 'Unknown'}</h1>;
}
```

### Mistake 3: Not Using Partial for Update States
```typescript
// ❌ WRONG - Can't easily update partial fields
const [user, setUser] = React.useState<User>(initialUser);
setUser({ ...user, name: 'New Name' }); // Tedious

// ✅ CORRECT - Use functional updates properly
const [user, setUser] = React.useState<User>(initialUser);
setUser(prev => ({ ...prev, name: 'New Name' }));
```

## Real-World Example

Here's a complete Product Card component demonstrating all prop typing patterns:

```typescript
// [File: src/components/ProductCard.tsx]
import React from 'react';

// ======== Types ========

// Product data type
interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  originalPrice?: number;
  image: string;
  category: string;
  rating: number;
  reviewCount: number;
  inStock: boolean;
  tags: string[];
}

// Variant types for different appearances
type ProductCardVariant = 'default' | 'compact' | 'featured';

// Event callbacks
interface ProductActions {
  onAddToCart?: (product: Product) => void;
  onAddToWishlist?: (product: Product) => void;
  onViewDetails?: (productId: string) => void;
}

// Combined props interface
interface ProductCardProps extends ProductActions {
  product: Product;
  variant?: ProductCardVariant;
  showReviews?: boolean;
  className?: string;
}

// ======== Component ========

export const ProductCard: React.FC<ProductCardProps> = ({
  product,
  variant = 'default',
  showReviews = true,
  className = '',
  onAddToCart,
  onAddToWishlist,
  onViewDetails,
}) => {
  // Calculate discount percentage
  const discountPercent = product.originalPrice
    ? Math.round(
        ((product.originalPrice - product.price) / product.originalPrice) * 100
      )
    : 0;

  // Handle click on entire card
  const handleClick = () => {
    onViewDetails?.(product.id);
  };

  // Handle add to cart
  const handleAddToCart = (e: React.MouseEvent) => {
    e.stopPropagation(); // Prevent card click
    onAddToCart?.(product);
  };

  // Handle wishlist
  const handleWishlist = (e: React.MouseEvent) => {
    e.stopPropagation();
    onAddToWishlist?.(product);
  };

  // Render stars for rating
  const renderRating = () => {
    const stars = Array.from({ length: 5 }, (_, i) => (
      <span key={i} className={i < Math.floor(product.rating) ? 'filled' : ''}>
        ★
      </span>
    ));
    return (
      <div className="rating">
        {stars}
        {showReviews && (
          <span className="review-count">({product.reviewCount})</span>
        )}
      </div>
    );
  };

  // Compact variant
  if (variant === 'compact') {
    return (
      <div 
        className={`product-card compact ${className}`}
        onClick={handleClick}
        role="button"
        tabIndex={0}
      >
        <img src={product.image} alt={product.name} />
        <div className="info">
          <h4>{product.name}</h4>
          <p className="price">${product.price.toFixed(2)}</p>
        </div>
      </div>
    );
  }

  // Default and Featured variants
  return (
    <div 
      className={`product-card ${variant} ${className}`}
      onClick={handleClick}
      role="button"
      tabIndex={0}
      onKeyDown={(e) => e.key === 'Enter' && handleClick()}
    >
      {/* Image Section */}
      <div className="image-container">
        <img src={product.image} alt={product.name} />
        
        {/* Badges */}
        <div className="badges">
          {!product.inStock && <span className="badge out-of-stock">Out of Stock</span>}
          {discountPercent > 0 && (
            <span className="badge sale">-{discountPercent}%</span>
          )}
          {variant === 'featured' && (
            <span className="badge featured">Featured</span>
          )}
        </div>

        {/* Quick Actions */}
        <button
          className="wishlist-btn"
          onClick={handleWishlist}
          aria-label="Add to wishlist"
        >
          ♡
        </button>
      </div>

      {/* Content Section */}
      <div className="content">
        <span className="category">{product.category}</span>
        <h3 className="name">{product.name}</h3>
        
        {showReviews && renderRating()}
        
        <p className="description">{product.description}</p>

        {/* Price */}
        <div className="price-section">
          <span className="current-price">${product.price.toFixed(2)}</span>
          {product.originalPrice && (
            <span className="original-price">
              ${product.originalPrice.toFixed(2)}
            </span>
          )}
        </div>

        {/* Tags */}
        <div className="tags">
          {product.tags.slice(0, 3).map(tag => (
            <span key={tag} className="tag">{tag}</span>
          ))}
        </div>

        {/* Actions */}
        <div className="actions">
          <button
            className="add-to-cart"
            onClick={handleAddToCart}
            disabled={!product.inStock}
          >
            {product.inStock ? 'Add to Cart' : 'Out of Stock'}
          </button>
        </div>
      </div>
    </div>
  );
};

// ======== Usage Examples ========

const mockProduct: Product = {
  id: '1',
  name: 'Wireless Headphones',
  description: 'Premium noise-canceling headphones with 30-hour battery life.',
  price: 199.99,
  originalPrice: 249.99,
  image: '/images/headphones.jpg',
  category: 'Electronics',
  rating: 4.5,
  reviewCount: 128,
  inStock: true,
  tags: ['wireless', 'bluetooth', 'noise-canceling'],
};

function ProductList() {
  return (
    <div className="product-list">
      {/* Default variant */}
      <ProductCard
        product={mockProduct}
        onAddToCart={(p) => console.log('Add to cart:', p.name)}
        onAddToWishlist={(p) => console.log('Wishlist:', p.name)}
        onViewDetails={(id) => console.log('View:', id)}
      />

      {/* Compact variant */}
      <ProductCard
        product={mockProduct}
        variant="compact"
        onViewDetails={(id) => console.log('View:', id)}
      />

      {/* Featured variant */}
      <ProductCard
        product={mockProduct}
        variant="featured"
        showReviews={false}
      />
    </div>
  );
}

export default ProductCard;
```

## Key Takeaways
- Use interfaces for props to enable extension and readability
- Mark optional props with `?` and provide default values in destructuring
- Use discriminated unions for complex UI states (loading, error, success)
- Always type event handlers with specific React event types
- Use `Partial<T>` when updating nested state fields
- TypeScript narrows types in JSX based on conditions
- Export types separately for reuse across components
- Use `React.FC` or explicit prop typing - both work, be consistent

## What's Next
Continue to [Typing Hooks](02-typing-hooks.md) to learn how to properly type useRef, useReducer, useCallback, and custom hooks.