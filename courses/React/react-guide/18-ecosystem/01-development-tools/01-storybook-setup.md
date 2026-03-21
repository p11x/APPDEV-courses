# Storybook Setup

## Overview
Storybook is a tool for developing UI components in isolation. It allows you to build, test, and preview components without running your full application.

## Prerequisites
- Node.js and npm
- React project

## Core Concepts

### Installation

```bash
# [File: Terminal command]
# Add Storybook to your project
npx storybook@latest init

# Start Storybook
npm run storybook
```

### Configuration

```javascript
// [File: .storybook/main.js]
module.exports = {
  stories: [
    '../src/**/*.stories.mdx',
    '../src/**/*.stories.@(js|jsx|ts|tsx)',
  ],
  addons: [
    '@storybook/addon-links',
    '@storybook/addon-essentials',
    '@storybook/addon-interactions',
  ],
  framework: {
    name: '@storybook/react-vite',
    options: {},
  },
};
```

### Creating Stories

```tsx
// [File: src/components/Button/Button.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { Button } from './Button';

// Meta defines component metadata
const meta: Meta<typeof Button> = {
  title: 'Components/Button',
  component: Button,
  tags: ['autodocs'],
  argTypes: {
    onClick: { action: 'clicked' },
    variant: {
      control: 'select',
      options: ['primary', 'secondary', 'danger'],
    },
  },
};

export default meta;
type Story = StoryObj<typeof Button>;

// Story 1: Primary button
export const Primary: Story = {
  args: {
    variant: 'primary',
    children: 'Primary Button',
  },
};

// Story 2: Secondary button
export const Secondary: Story = {
  args: {
    variant: 'secondary',
    children: 'Secondary Button',
  },
};

// Story 3: All variants
export const AllVariants: Story = {
  render: () => (
    <div style={{ display: 'flex', gap: '1rem' }}>
      <Button variant="primary">Primary</Button>
      <Button variant="secondary">Secondary</Button>
      <Button variant="danger">Danger</Button>
    </div>
  ),
};
```

## Key Takeaways
- Storybook enables isolated component development
- Use stories to test component variations
- Addons extend Storybook functionality

## What's Next
Continue to [Writing Stories](02-writing-stories.md) for more advanced story patterns.