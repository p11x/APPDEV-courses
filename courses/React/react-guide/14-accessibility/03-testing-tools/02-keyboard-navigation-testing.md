# Keyboard Navigation Testing

## Overview
Keyboard accessibility is essential for users who cannot use a mouse. Testing keyboard navigation ensures all interactive elements are accessible via keyboard.

## Prerequisites
- Testing library knowledge
- Keyboard interaction patterns

## Core Concepts

### Testing Keyboard Navigation

```tsx
// [File: src/components/Menu/Menu.keyboard.test.tsx]
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Menu } from './Menu';

const menuItems = [
  { label: 'Profile', href: '/profile' },
  { label: 'Settings', href: '/settings' },
  { label: 'Logout', href: '/logout' }
];

test('keyboard navigation works correctly', async () => {
  const user = userEvent.setup();
  render(<Menu items={menuItems} />);
  
  // Get first menu item
  const firstItem = screen.getByText('Profile');
  
  // Tab should move focus to menu
  await user.tab();
  expect(firstItem).toHaveFocus();
  
  // Arrow down should move to next item
  await user.keyboard('{ArrowDown}');
  const secondItem = screen.getByText('Settings');
  expect(secondItem).toHaveFocus();
  
  // Arrow down again
  await user.keyboard('{ArrowDown}');
  const thirdItem = screen.getByText('Logout');
  expect(thirdItem).toHaveFocus();
  
  // Arrow down should wrap to first
  await user.keyboard('{ArrowDown}');
  expect(firstItem).toHaveFocus();
  
  // Arrow up should go to last
  await user.keyboard('{ArrowUp}');
  expect(thirdItem).toHaveFocus();
});
```

### Testing Focus Management

```tsx
// [File: src/components/Modal/Modal.keyboard.test.tsx]
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { Modal } from './Modal';

test('focus is trapped within modal', async () => {
  const user = userEvent.setup();
  render(
    <>
      <button>Open Modal</button>
      <Modal isOpen={true} title="Test Modal">
        <button>Action 1</button>
        <button>Action 2</button>
      </Modal>
    </>
  );
  
  // Tab should cycle within modal
  const closeButton = screen.getByRole('button', { name: /close/i });
  const action1 = screen.getByRole('button', { name: /action 1/i });
  const action2 = screen.getByRole('button', { name: /action 2/i });
  
  // Focus starts on close button
  expect(closeButton).toHaveFocus();
  
  // Tab moves to next focusable element
  await user.tab();
  expect(action1).toHaveFocus();
  
  await user.tab();
  expect(action2).toHaveFocus();
  
  // Tab returns to close button (focus trap)
  await user.tab();
  expect(closeButton).toHaveFocus();
});

test('escape closes modal', async () => {
  const user = userEvent.setup();
  const handleClose = jest.fn();
  
  render(
    <Modal isOpen={true} onClose={handleClose} title="Test">
      <button>Content</button>
    </Modal>
  );
  
  await user.keyboard('{Escape}');
  expect(handleClose).toHaveBeenCalled();
});
```

## Key Takeaways
- Test Tab, Shift+Tab, and arrow keys
- Verify focus is visible
- Test focus trapping in modals

## What's Next
Continue to [Screen Reader Testing](03-screen-reader-testing.md) for screen reader testing.