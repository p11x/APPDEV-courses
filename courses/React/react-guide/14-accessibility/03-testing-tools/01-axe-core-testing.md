# axe-core Testing

## Overview
axe-core is an accessibility testing engine that automatically checks for common accessibility issues. It can be integrated into development workflows and CI/CD pipelines.

## Prerequisites
- JavaScript testing
- Accessibility basics

## Core Concepts

### Installation

```bash
# [File: Terminal]
npm install -D @axe-core/react
```

### Basic Usage

```tsx
// [File: src/components/Button/Button.test.tsx]
import { render, screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import { Button } from './Button';

expect.extend(toHaveNoViolations);

test('should have no accessibility violations', async () => {
  const { container } = render(<Button>Click me</Button>);
  const results = await axe(container);
  
  expect(results).toHaveNoViolations();
});
```

### Component-Specific Testing

```tsx
// [File: src/components/Form/Form.test.tsx]
import { render, screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import { Form } from './Form';

expect.extend(toHaveNoViolations);

describe('Form accessibility', () => {
  it('should have no accessibility violations', async () => {
    const { container } = render(<Form />);
    const results = await axe(container);
    
    expect(results).toHaveNoViolations();
  });

  it('should have proper form labels', () => {
    render(<Form />);
    
    const nameInput = screen.getByLabelText(/name/i);
    const emailInput = screen.getByLabelText(/email/i);
    
    expect(nameInput).toBeInTheDocument();
    expect(emailInput).toBeInTheDocument();
  });

  it('should have accessible error messages', async () => {
    const { container } = render(<Form />);
    
    // Trigger validation
    const submitButton = screen.getByRole('button', { name: /submit/i });
    submitButton.click();
    
    const results = await axe(container);
    expect(results).toHaveNoViolations();
  });
});
```

### Running in Development

```tsx
// [File: src/App.tsx]
import React from 'react';
import Axe from '@axe-core/react';

function App() {
  // Only run axe in development
  if (process.env.NODE_ENV !== 'production') {
    return (
      <Axe>
        <YourApp />
      </Axe>
    );
  }
  
  return <YourApp />;
}
```

## Key Takeaways
- axe-core automates accessibility testing
- Integrate into CI/CD pipelines
- Test specific components for violations

## What's Next
Continue to [Keyboard Navigation Testing](02-keyboard-navigation-testing.md) for manual testing.