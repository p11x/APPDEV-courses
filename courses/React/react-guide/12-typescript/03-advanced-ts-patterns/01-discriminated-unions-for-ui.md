# Discriminated Unions for UI

## Overview
Discriminated unions (also called tagged unions) are one of the most powerful TypeScript patterns for building type-safe UI state machines. By using a common "tag" or "discriminator" property, TypeScript can automatically narrow types and know exactly which properties are available at any given time. This pattern is perfect for handling async data states (loading, error, success), form validation, modal visibility, wizard steps, and any complex UI that transitions between distinct states. This guide covers building robust UI components using discriminated unions.

## Prerequisites
- TypeScript basics (types, interfaces, generics)
- Understanding of React hooks (useState, useEffect)
- Familiarity with TypeScript union types

## Core Concepts

### Understanding Discriminated Unions
A discriminated union is a union of types that share a common property (the discriminator) that TypeScript uses to narrow which type you're working with:

```typescript
// [File: src/examples/discriminatedUnions.ts]

// ======== Basic Discriminated Union ========

// The 'status' property is our discriminator
type AsyncState<T> = 
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; error: Error };

// TypeScript knows:
// - In 'idle' state: only status property exists
// - In 'loading' state: only status property exists  
// - In 'success' state: status AND data exist
// - In 'error' state: status AND error exist

// Usage in a function
function handleState<T>(state: AsyncState<T>) {
  switch (state.status) {
    case 'idle':
      // TypeScript knows: state.status === 'idle'
      console.log('Ready to load');
      return;
      
    case 'loading':
      // TypeScript knows: state.status === 'loading'
      console.log('Loading...');
      return;
      
    case 'success':
      // TypeScript knows: state.data exists and is type T!
      console.log('Data:', state.data);
      return;
      
    case 'error':
      // TypeScript knows: state.error exists and is Error!
      console.log('Error:', state.error.message);
      return;
  }
}

// ======== Realistic Example: Payment Flow ========

type PaymentState =
  | { status: 'idle' }
  | { status: 'processing' }
  | { status: 'success'; transactionId: string }
  | { status: 'failed'; error: string; retryCount: number };

function processPayment(state: PaymentState) {
  switch (state.status) {
    case 'idle':
      return <button>Pay Now</button>;
      
    case 'processing':
      return <Spinner />;
      
    case 'success':
      // state.transactionId is available!
      return <div>Payment successful! ID: {state.transactionId}</div>;
      
    case 'failed':
      // state.error and state.retryCount are available!
      return (
        <div>
          <p>Error: {state.error}</p>
          <p>Retries: {state.retryCount}</p>
        </div>
      );
  }
}
```

### Exhaustive Switch Checking
A powerful pattern that ensures all union cases are handled at compile time:

```typescript
// [File: src/examples/exhaustiveCheck.ts]

type Status = 'pending' | 'approved' | 'rejected' | 'cancelled';

function getStatusColor(status: Status): string {
  switch (status) {
    case 'pending':
      return 'yellow';
    case 'approved':
      return 'green';
    case 'rejected':
      return 'red';
    case 'cancelled':
      return 'gray';
    default:
      // If we add a new status but forget to handle it,
      // TypeScript will error here!
      const _exhaustive: never = status;
      throw new Error(`Unhandled status: ${_exhaustive}`);
  }
}

// This is great for development - you'll get compile errors
// if you add new statuses but forget to update this function
```

### Building an Async Data Component
Here's a complete example of a reusable async data component:

```typescript
// [File: src/components/AsyncData.tsx]
import React, { useState, useEffect } from 'react';

// ======== Discriminated Union Types ========

// Generic async data state with discriminated union
type AsyncData<T> = 
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; error: Error };

// Props for the async data component
interface AsyncDataProps<T> {
  // The current state
  state: AsyncData<T>;
  
  // Optional: Trigger data fetch
  fetchData?: () => void;
  
  // Render functions for each state
  idle?: () => React.ReactNode;
  loading?: () => React.ReactNode;
  success: (data: T) => React.ReactNode;
  error?: (error: Error) => React.ReactNode;
  
  // Optional: Cache key for preventing duplicate fetches
  cacheKey?: string;
}

// Generic component that handles all async states
function AsyncData<T>({
  state,
  fetchData,
  idle = () => <div>Click to load</div>,
  loading = () => <div className="loading">Loading...</div>,
  success,
  error = (err) => <div className="error">Error: {err.message}</div>,
}: AsyncDataProps<T>) {
  // TypeScript narrows the type based on status!
  switch (state.status) {
    case 'idle':
      return (
        <div>
          {idle()}
          {fetchData && (
            <button onClick={fetchData}>Load Data</button>
          )}
        </div>
      );
      
    case 'loading':
      return <>{loading()}</>;
      
    case 'error':
      return <>{error(state.error)}</>;
      
    case 'success':
      // TypeScript knows state.data exists here!
      return <>{success(state.data)}</>;
      
    default:
      // Exhaustiveness check - ensures all cases are handled
      const _exhaustive: never = state;
      return null;
  }
}

// ======== Usage Example ========

interface User {
  id: number;
  name: string;
  email: string;
}

function UserProfile({ userId }: { userId: number }) {
  const [userState, setUserState] = useState<AsyncData<User>>({
    status: 'idle',
  });

  const fetchUser = async () => {
    setUserState({ status: 'loading' });
    
    try {
      const response = await fetch(`/api/users/${userId}`);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      setUserState({ status: 'success', data });
    } catch (e) {
      setUserState({ 
        status: 'error', 
        error: e instanceof Error ? e : new Error('Unknown error') 
      });
    }
  };

  return (
    <AsyncData
      state={userState}
      fetchData={fetchUser}
      idle={() => (
        <div className="idle-message">
          <p>User data not loaded</p>
        </div>
      )}
      loading={() => <Spinner size="large" />}
      success={(user) => (
        <div className="user-profile">
          <h2>{user.name}</h2>
          <p>{user.email}</p>
        </div>
      )}
      error={(err) => (
        <div className="error-message">
          <p>Failed to load user: {err.message}</p>
          <button onClick={fetchUser}>Retry</button>
        </div>
      )}
    />
  );
}
```

### Complex UI State with Multiple Discriminators
For complex UI, you can use multiple discriminators:

```typescript
// [File: src/components/WizardForm.tsx]
import React, { useState } from 'react';

// ======== Wizard Steps ========

type WizardStep = 'personal' | 'contact' | 'review' | 'submitting' | 'complete';

interface PersonalInfo {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
}

interface ContactInfo {
  email: string;
  phone: string;
  address: string;
}

// Combined form data
interface FormData {
  personal: PersonalInfo;
  contact: ContactInfo;
}

// ======== Wizard State ========

// Multiple discriminators: step and subStatus
type WizardState =
  | { 
      step: 'personal'; 
      data: { personal: PersonalInfo; contact: ContactInfo };
    }
  | { 
      step: 'contact'; 
      data: { personal: PersonalInfo; contact: ContactInfo };
    }
  | { 
      step: 'review'; 
      data: { personal: PersonalInfo; contact: ContactInfo };
    }
  | { 
      step: 'submitting'; 
      data: { personal: PersonalInfo; contact: ContactInfo };
    }
  | { 
      step: 'complete'; 
      confirmationNumber: string;
    };

// Initial state
const initialData = {
  personal: { firstName: '', lastName: '', dateOfBirth: '' },
  contact: { email: '', phone: '', address: '' },
};

const initialState: WizardState = {
  step: 'personal',
  data: initialData,
};

// ======== Wizard Component ========

function WizardForm() {
  const [state, setState] = useState<WizardState>(initialState);

  // Type-safe navigation
  const goToStep = (step: WizardStep) => {
    if (state.step === 'submitting' || state.step === 'complete') return;
    
    setState(prev => ({
      ...prev,
      step,
    } as WizardState);
  };

  const updatePersonal = (personal: Partial<PersonalInfo>) => {
    if (state.step !== 'personal') return;
    setState(prev => ({
      ...prev,
      data: {
        ...prev.data,
        personal: { ...prev.data.personal, ...personal },
      },
    } as WizardState);
  };

  const updateContact = (contact: Partial<ContactInfo>) => {
    if (state.step !== 'contact') return;
    setState(prev => ({
      ...prev,
      data: {
        ...prev.data,
        contact: { ...prev.data.contact, ...contact },
      },
    } as WizardState);
  };

  const submit = async () => {
    setState({ step: 'submitting', data: state.data });
    
    // Simulate API call
    await new Promise(resolve => setTimeout(resolve, 1500));
    
    setState({ 
      step: 'complete', 
      confirmationNumber: 'CONF-' + Date.now() 
    });
  };

  // Render based on step - TypeScript knows what data is available!
  switch (state.step) {
    case 'personal':
      return (
        <div className="wizard-step personal">
          <h2>Personal Information</h2>
          <input
            value={state.data.personal.firstName}
            onChange={(e) => updatePersonal({ firstName: e.target.value })}
            placeholder="First Name"
          />
          <input
            value={state.data.personal.lastName}
            onChange={(e) => updatePersonal({ lastName: e.target.value })}
            placeholder="Last Name"
          />
          <button onClick={() => goToStep('contact')}>Next</button>
        </div>
      );

    case 'contact':
      return (
        <div className="wizard-step contact">
          <h2>Contact Information</h2>
          <input
            value={state.data.contact.email}
            onChange={(e) => updateContact({ email: e.target.value })}
            placeholder="Email"
          />
          <input
            value={state.data.contact.phone}
            onChange={(e) => updateContact({ phone: e.target.value })}
            placeholder="Phone"
          />
          <button onClick={() => goToStep('personal')}>Back</button>
          <button onClick={() => goToStep('review')}>Next</button>
        </div>
      );

    case 'review':
      return (
        <div className="wizard-step review">
          <h2>Review Your Information</h2>
          <pre>{JSON.stringify(state.data, null, 2)}</pre>
          <button onClick={() => goToStep('contact')}>Back</button>
          <button onClick={submit}>Submit</button>
        </div>
      );

    case 'submitting':
      return <Spinner />;

    case 'complete':
      return (
        <div className="wizard-step complete">
          <h2>Submission Complete!</h2>
          <p>Confirmation: {state.confirmationNumber}</p>
        </div>
      );
  }
}
```

## Common Mistakes

### Mistake 1: Not Using Discriminators
```typescript
// ❌ WRONG - Can't narrow types without discriminator
type BadState = {
  status: string;
  data?: User;
  error?: Error;
};

// Can't tell which properties exist!

// ✅ CORRECT - Use discriminated union
type GoodState = 
  | { status: 'loading' }
  | { status: 'success'; data: User }
  | { status: 'error'; error: Error };
// TypeScript knows exactly what's available!
```

### Mistake 2: Forgetting Default Case
```typescript
// ❌ WRONG - Missing default can cause issues
function handle(state: State) {
  switch (state.type) {
    case 'a':
      return state.valueA;
    case 'b':
      return state.valueB;
    // If we add 'c' later, no warning!
  }
}

// ✅ CORRECT - Add default with exhaustiveness check
function handle(state: State) {
  switch (state.type) {
    case 'a':
      return state.valueA;
    case 'b':
      return state.valueB;
    default:
      const _exhaustive: never = state;
      return _exhaustive;
  }
}
```

### Mistake 3: Using String as Discriminator
```typescript
// ❌ WRONG - String can be any value
type BadState = {
  status: string; // Too broad!
  data?: User;
};

// ✅ CORRECT - Use literal types for exact values
type GoodState = 
  | { status: 'loading'; }
  | { status: 'success'; data: User };
```

## Real-World Example

Complete modal component with discriminated union state:

```typescript
// [File: src/components/Modal.tsx]
import React, { useEffect, useRef } from 'react';

// ======== Modal Types ========

type ModalVariant = 'default' | 'confirm' | 'form';

type ModalState<T = unknown> =
  | { isOpen: false }
  | { 
      isOpen: true; 
      variant: 'default'; 
      title?: string;
      content: React.ReactNode;
    }
  | { 
      isOpen: true; 
      variant: 'confirm'; 
      title?: string;
      message: string;
      confirmLabel?: string;
      cancelLabel?: string;
      onConfirm: () => void;
      onCancel?: () => void;
    }
  | { 
      isOpen: true; 
      variant: 'form'; 
      title?: string;
      formData: T;
      onSubmit: (data: T) => void;
      onCancel?: () => void;
    };

// ======== Modal Component ========

interface ModalProps {
  state: ModalState;
  onClose: () => void;
}

function Modal({ state, onClose }: ModalProps) {
  const modalRef = useRef<HTMLDivElement>(null);
  const previousFocusRef = useRef<HTMLElement | null>(null);

  // Focus management
  useEffect(() => {
    if (state.isOpen) {
      // Store previously focused element
      previousFocusRef.current = document.activeElement as HTMLElement;
      
      // Focus modal
      modalRef.current?.focus();
      
      // Prevent body scroll
      document.body.style.overflow = 'hidden';
    } else {
      // Restore body scroll
      document.body.style.overflow = '';
      
      // Return focus to previous element
      previousFocusRef.current?.focus();
    }
  }, [state.isOpen]);

  // Close on escape
  useEffect(() => {
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.key === 'Escape' && state.isOpen) {
        onClose();
      }
    };
    
    document.addEventListener('keydown', handleKeyDown);
    return () => document.removeEventListener('keydown', handleKeyDown);
  }, [state.isOpen, onClose]);

  if (!state.isOpen) return null;

  // Type-safe rendering based on variant
  const renderContent = () => {
    switch (state.variant) {
      case 'default':
        return (
          <div className="modal-content">
            {state.content}
          </div>
        );
        
      case 'confirm':
        return (
          <div className="modal-confirm">
            <p className="confirm-message">{state.message}</p>
            <div className="confirm-actions">
              <button 
                onClick={state.onCancel ?? onClose}
                className="btn-secondary"
              >
                {state.cancelLabel ?? 'Cancel'}
              </button>
              <button 
                onClick={state.onConfirm}
                className="btn-primary"
              >
                {state.confirmLabel ?? 'Confirm'}
              </button>
            </div>
          </div>
        );
        
      case 'form':
        return (
          <form
            onSubmit={(e) => {
              e.preventDefault();
              state.onSubmit(state.formData);
            }}
          >
            {/* Render form content based on formData */}
            <p>Form data: {JSON.stringify(state.formData)}</p>
            <div className="form-actions">
              <button 
                type="button"
                onClick={state.onCancel ?? onClose}
                className="btn-secondary"
              >
                Cancel
              </button>
              <button type="submit" className="btn-primary">
                Submit
              </button>
            </div>
          </form>
        );
        
      default:
        const _exhaustive: never = state;
        return null;
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        ref={modalRef}
        className={`modal modal-${state.variant}`}
        onClick={(e) => e.stopPropagation()}
        role="dialog"
        aria-modal="true"
        aria-labelledby={state.title ? 'modal-title' : undefined}
        tabIndex={-1}
      >
        {state.title && (
          <div className="modal-header">
            <h2 id="modal-title">{state.title}</h2>
            <button 
              onClick={onClose}
              aria-label="Close modal"
              className="modal-close"
            >
              ×
            </button>
          </div>
        )}
        <div className="modal-body">
          {renderContent()}
        </div>
      </div>
    </div>
  );
}

// ======== Usage ========

function App() {
  const [modalState, setModalState] = React.useState<ModalState>({
    isOpen: false,
  });

  const openConfirm = () => {
    setModalState({
      isOpen: true,
      variant: 'confirm',
      title: 'Delete Item',
      message: 'Are you sure you want to delete this item? This action cannot be undone.',
      confirmLabel: 'Delete',
      cancelLabel: 'Keep',
      onConfirm: () => {
        console.log('Confirmed!');
        setModalState({ isOpen: false });
      },
      onCancel: () => {
        console.log('Cancelled!');
        setModalState({ isOpen: false });
      },
    });
  };

  return (
    <div>
      <button onClick={openConfirm}>Open Confirm Modal</button>
      <Modal 
        state={modalState} 
        onClose={() => setModalState({ isOpen: false })} 
      />
    </div>
  );
}

export default Modal;
```

## Key Takeaways
- Use discriminated unions with a common "tag" property for type narrowing
- The discriminator should be a literal type (not just string)
- Always include a default case with exhaustiveness checking
- TypeScript narrows types automatically in switch/if statements
- Perfect for async data states, form wizards, modals, and complex UI
- Combine multiple discriminators for complex state machines
- The never type helps catch missing cases at compile time

## What's Next
Continue to [Utility Types in React](02-utility-types-in-react.md) to learn how to use TypeScript's built-in utility types like Partial, Required, Pick, and Omit in React components.