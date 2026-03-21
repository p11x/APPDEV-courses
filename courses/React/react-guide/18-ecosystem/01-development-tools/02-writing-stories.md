# Writing Stories

## Overview
Learn advanced patterns for writing comprehensive stories in Storybook, including controls, decorators, and complex component states.

## Prerequisites
- Storybook basics
- React components

## Core Concepts

### Using Controls

```tsx
// [File: src/components/Input/Input.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { Input } from './Input';

const meta: Meta<typeof Input> = {
  title: 'Forms/Input',
  component: Input,
  argTypes: {
    onChange: { action: 'changed' },
    error: { control: 'text' },
    disabled: { control: 'boolean' },
  },
  // Add decorators for global styling
  decorators: [
    (Story) => (
      <div style={{ padding: '2rem' }}>
        <Story />
      </div>
    ),
  ],
};

export default meta;
type Story = StoryObj<typeof Input>;

export const Default: Story = {
  args: {
    placeholder: 'Enter text...',
  },
};

export const WithLabel: Story = {
  args: {
    label: 'Email Address',
    placeholder: 'Enter your email',
    type: 'email',
  },
};

export const WithError: Story = {
  args: {
    label: 'Password',
    value: 'wrongpassword',
    error: 'Password is incorrect',
    type: 'password',
  },
};

export const Disabled: Story = {
  args: {
    label: 'Disabled Input',
    disabled: true,
    value: 'Cannot edit this',
  },
};
```

### Complex Component Stories

```tsx
// [File: src/components/Card/Card.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { Card } from './Card';
import { Button } from '../Button/Button';

const meta: Meta<typeof Card> = {
  title: 'Layout/Card',
  component: Card,
  parameters: {
    layout: 'centered',
  },
};

export default meta;
type Story = StoryObj<typeof Card>;

export const Simple: Story = {
  args: {
    children: 'Simple card content',
  },
};

export const CardWithHeader: Story = {
  args: {
    header: 'Card Title',
    children: 'Card body content goes here.',
  },
};

export const CardWithActions: Story = {
  args: {
    header: 'User Profile',
    children: (
      <div>
        <p>Name: John Doe</p>
        <p>Email: john@example.com</p>
      </div>
    ),
    actions: (
      <>
        <Button variant="primary">Edit</Button>
        <Button variant="secondary">Delete</Button>
      </>
    ),
  },
};
```

### Mocking Data

```tsx
// [File: src/stories/mockData.ts]
export const mockUsers = [
  { id: 1, name: 'John Doe', email: 'john@example.com' },
  { id: 2, name: 'Jane Smith', email: 'jane@example.com' },
];

export const mockProducts = [
  { id: 1, name: 'Laptop', price: 999, inStock: true },
  { id: 2, name: 'Phone', price: 699, inStock: false },
];
```

## Key Takeaways
- Use controls for interactive props
- Decorators wrap stories with context
- Mock data keeps stories isolated

## What's Next
Continue to [Storybook Testing](03-storybook-testing.md) to learn about testing with Storybook.