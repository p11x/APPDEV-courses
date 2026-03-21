# Storybook Testing

## Overview
Storybook integrates with testing tools to verify component behavior. You can test components directly in Storybook using the interactions addon.

## Prerequisites
- Storybook knowledge
- Testing basics

## Core Concepts

### Interaction Testing

```tsx
// [File: src/components/Button/Button.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { userEvent, within, expect } from '@storybook/test';
import { Button } from './Button';

const meta: Meta<typeof Button> = {
  title: 'Components/Button',
  component: Button,
};

export default meta;

export const Clickable: StoryObj<typeof Button> = {
  play: async ({ args, canvasElement }) => {
    const canvas = within(canvasElement);
    const button = canvas.getByRole('button');
    
    // Interact with button
    await userEvent.click(button);
    
    // Assert callback was called
    expect(args.onClick).toHaveBeenCalled();
  },
  args: {
    children: 'Click me',
    onClick: async () => {},
  },
};
```

### Visual Testing

```tsx
// [File: src/components/Button/Button.visual.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { Button } from './Button';

const meta: Meta<typeof Button> = {
  title: 'Components/Button/Visual',
  component: Button,
  parameters: {
    // Chromatic visual regression testing
    chromatic: { viewports: [320, 768, 1280] },
  },
};

export default meta;

// Each state becomes a baseline for visual comparison
export const Primary: StoryObj<typeof Button> = {
  args: { variant: 'primary', children: 'Primary' },
};

export const Secondary: StoryObj<typeof Button> = {
  args: { variant: 'secondary', children: 'Secondary' },
};

export const Hover: StoryObj<typeof Button> = {
  args: { variant: 'primary', children: 'Hover me' },
  parameters: {
    pseudo: { hover: true },
  },
};
```

### Accessibility Testing

```tsx
// [File: src/components/Form/Form.stories.tsx]
import type { Meta, StoryObj } from '@storybook/react';
import { checkA11y } from '@storybook/addon-a11y';
import { Form } from './Form';

const meta: Meta<typeof Form> = {
  title: 'Forms/Form',
  component: Form,
  tags: ['autodocs'],
  parameters: {
    // Run a11y check after render
    a11y: {
      config: {
        rules: [{ id: 'label', enabled: true }],
      },
    },
  },
};

export default meta;

export const Accessible: StoryObj<typeof Form> = {
  play: async ({ canvasElement }) => {
    await checkA11y(canvasElement);
  },
};
```

## Key Takeaways
- Play functions enable interaction testing
- Chromatic provides visual regression testing
- A11y addon checks accessibility

## What's Next
Continue to [Turborepo Setup](02-monorepos/01-turborepo-setup.md) for monorepo tooling.