# Screen Reader Testing

## Overview
Screen reader testing ensures blind and visually impaired users can access your application. Learn what to check and how to verify proper announcements.

## Prerequisites
- Accessibility fundamentals
- ARIA knowledge

## Core Concepts

### Testing with jest-axe for Announcements

```tsx
// [File: src/components/Alert/Alert.test.tsx]
import { render, screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import { Alert } from './Alert';

expect.extend(toHaveNoViolations);

describe('Alert screen reader accessibility', () => {
  it('should have role="alert" for important messages', () => {
    render(<Alert type="error">Something went wrong</Alert>);
    
    const alert = screen.getByRole('alert');
    expect(alert).toBeInTheDocument();
  });

  it('should have proper live region for dynamic content', () => {
    render(
      <div aria-live="polite">
        <p>Item added to cart</p>
      </div>
    );
    
    const liveRegion = screen.getByRole('status');
    expect(liveRegion).toHaveAttribute('aria-live', 'polite');
  });
});
```

### Testing ARIA Labels

```tsx
// [File: src/components/IconButton/IconButton.test.tsx]
import { render, screen } from '@testing-library/react';
import { IconButton } from './IconButton';

test('icon buttons have accessible names', () => {
  render(
    <IconButton 
      icon="search" 
      aria-label="Search for products"
    />
  );
  
  const button = screen.getByRole('button', { name: /search for products/i });
  expect(button).toBeInTheDocument();
});

test('icon buttons without labels are invalid', () => {
  // This test documents the anti-pattern
  render(<IconButton icon="search" />);
  
  const button = screen.getByRole('button');
  // Button should have accessible name via aria-label or icon text
  expect(button).toHaveAccessibleName();
});
```

### Testing Tables for Screen Readers

```tsx
// [File: src/components/DataTable/DataTable.test.tsx]
import { render, screen } from '@testing-library/react';
import { DataTable } from './DataTable';

test('data tables have proper structure', () => {
  render(
    <DataTable 
      columns={['Name', 'Email', 'Role']}
      data={[
        { name: 'John', email: 'john@example.com', role: 'Admin' },
        { name: 'Jane', email: 'jane@example.com', role: 'User' }
      ]}
    />
  );
  
  // Check for table structure
  expect(screen.getByRole('table')).toBeInTheDocument();
  expect(screen.getByRole('columnheader', { name: /name/i })).toBeInTheDocument();
  expect(screen.getByRole('columnheader', { name: /email/i })).toBeInTheDocument();
  expect(screen.getByRole('columnheader', { name: /role/i })).toBeInTheDocument();
  
  // Check for row structure
  const rows = screen.getAllByRole('row');
  expect(rows).toHaveLength(3); // header + 2 data rows
});
```

## Key Takeaways
- Use role="alert" for important errors
- Verify all interactive elements have names
- Test table structure for screen readers

## What's Next
This completes the Accessibility module. Continue to [Animation Fundamentals](15-animations/04-keyframe-animations.md) for CSS keyframes.