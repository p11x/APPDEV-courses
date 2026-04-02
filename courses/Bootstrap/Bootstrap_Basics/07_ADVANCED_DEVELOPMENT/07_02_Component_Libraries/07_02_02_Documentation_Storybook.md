---
title: "Documentation with Storybook"
difficulty: 2
category: "Advanced Development"
subcategory: "Component Libraries"
prerequisites:
  - Storybook 7+
  - Bootstrap 5 Components
  - Component Story Format 3
---

## Overview

Storybook provides an isolated development and documentation environment for Bootstrap components. It enables interactive component exploration, visual testing, and living documentation that stays in sync with the codebase. Each component gets a "story" file that renders the component in various states, configurations, and viewports.

Integrating Storybook with Bootstrap requires configuring SCSS support, setting up viewport addons for responsive testing, and creating stories that showcase component variations. The addon ecosystem extends Storybook with accessibility checks, design token integration, and interaction testing.

Storybook serves three audiences: developers exploring component APIs, designers reviewing implementation fidelity, and QA teams testing edge cases. A well-configured Storybook becomes the single source of truth for the component library.

## Basic Implementation

Storybook configuration with Bootstrap SCSS support and a basic component story.

```js
// .storybook/main.js
module.exports = {
  stories: ['../src/**/*.stories.@(js|jsx|ts|tsx)'],
  addons: [
    '@storybook/addon-essentials',
    '@storybook/addon-a11y',
    '@storybook/addon-viewport',
    '@storybook/addon-interactions'
  ],
  framework: {
    name: '@storybook/html-vite',
    options: {}
  },
  core: {
    disableTelemetry: true
  }
};
```

```js
// .storybook/preview.js
import '../src/scss/main.scss';

export const parameters = {
  actions: { argTypesRegex: '^on[A-Z].*' },
  controls: {
    matchers: {
      color: /(background|color)$/i,
      date: /Date$/i
    }
  },
  viewport: {
    viewports: {
      mobile: { name: 'Mobile', styles: { width: '375px', height: '667px' } },
      tablet: { name: 'Tablet', styles: { width: '768px', height: '1024px' } },
      desktop: { name: 'Desktop', styles: { width: '1280px', height: '800px' } }
    }
  }
};
```

```js
// src/components/Card/Card.stories.js
export default {
  title: 'Components/Card',
  tags: ['autodocs'],
  argTypes: {
    title: { control: 'text', description: 'Card title' },
    body: { control: 'text', description: 'Card body text' },
    variant: {
      control: 'select',
      options: ['', 'primary', 'success', 'danger', 'warning'],
      description: 'Bootstrap color variant'
    },
    image: { control: 'text', description: 'Image URL' },
    bordered: { control: 'boolean', description: 'Show border' }
  },
  args: {
    title: 'Card Title',
    body: 'Some quick example text to build on the card title.',
    variant: '',
    image: '',
    bordered: true
  }
};

const Template = ({ title, body, variant, image, bordered }) => {
  const variantClass = variant ? `border-${variant}` : '';
  const imgHtml = image
    ? `<img src="${image}" class="card-img-top" alt="${title}">`
    : '';

  return `
    <div class="card ${variantClass}" style="width: 18rem;">
      ${imgHtml}
      <div class="card-body">
        <h5 class="card-title">${title}</h5>
        <p class="card-text">${body}</p>
        <a href="#" class="btn btn-primary">Go somewhere</a>
      </div>
    </div>
  `;
};

export const Default = Template.bind({});

export const WithImage = Template.bind({});
WithImage.args = {
  image: 'https://picsum.photos/286/180',
  title: 'Card with Image'
};

export const Primary = Template.bind({});
Primary.args = {
  variant: 'primary',
  title: 'Primary Card'
};

export const Compact = Template.bind({});
Compact.args = {
  title: 'Compact',
  body: 'Short text.'
};
```

## Advanced Variations

```js
// Advanced story with interaction testing
import { within, userEvent, expect } from '@storybook/test';

export default {
  title: 'Components/Accordion',
  tags: ['autodocs']
};

const Template = () => `
  <div class="accordion" id="storyAccordion">
    <div class="accordion-item">
      <h2 class="accordion-header">
        <button class="accordion-button" type="button"
                data-bs-toggle="collapse" data-bs-target="#collapseOne"
                aria-expanded="true" aria-controls="collapseOne">
          Section 1
        </button>
      </h2>
      <div id="collapseOne" class="accordion-collapse collapse show"
           data-bs-parent="#storyAccordion">
        <div class="accordion-body">Content for section 1.</div>
      </div>
    </div>
    <div class="accordion-item">
      <h2 class="accordion-header">
        <button class="accordion-button collapsed" type="button"
                data-bs-toggle="collapse" data-bs-target="#collapseTwo"
                aria-expanded="false" aria-controls="collapseTwo">
          Section 2
        </button>
      </h2>
      <div id="collapseTwo" class="accordion-collapse collapse"
           data-bs-parent="#storyAccordion">
        <div class="accordion-body">Content for section 2.</div>
      </div>
    </div>
  </div>
`;

export const Default = Template.bind({});

export const InteractionTest = Template.bind({});
InteractionTest.play = async ({ canvasElement }) => {
  const canvas = within(canvasElement);

  // Click second accordion button
  const button = canvas.getByText('Section 2');
  await userEvent.click(button);

  // Verify it expanded
  const panel = canvasElement.querySelector('#collapseTwo');
  await expect(panel).toHaveClass('show');

  // Verify first collapsed
  const firstPanel = canvasElement.querySelector('#collapseOne');
  await expect(firstPanel).not.toHaveClass('show');
};

// Multi-viewport story
export const Responsive = Template.bind({});
Responsive.parameters = {
  viewport: {
    defaultViewport: 'mobile'
  }
};
```

## Best Practices

1. **Use CSF 3 format** - Component Story Format 3 provides type-safe args, automatic controls, and cleaner syntax.
2. **Tag with autodocs** - Add `tags: ['autodocs']` to auto-generate documentation pages from argTypes.
3. **Define argTypes explicitly** - Control types, descriptions, and options enable interactive prop exploration.
4. **Include accessibility addon** - `@storybook/addon-a11y` runs axe checks on every story automatically.
5. **Show all component states** - Create stories for loading, empty, error, and edge-case states.
6. **Use decorators for layout** - Wrap stories in containers, padding, or theme providers via decorators.
7. **Test responsive behavior** - Create viewport-specific stories that show mobile and desktop layouts.
8. **Keep stories simple** - Each story should demonstrate one concept; combine via a "Kitchen Sink" story for integration.
9. **Document with MDX** - Use MDX files for long-form documentation alongside stories.
10. **Version-lock Storybook** - Pin Storybook versions to avoid addon incompatibilities during upgrades.

## Common Pitfalls

1. **SCSS not loading** - Missing SCSS configuration in `.storybook/preview.js` causes unstyled components.
2. **Bootstrap JS not initializing** - Components requiring Bootstrap's JS (modals, dropdowns) need manual initialization in stories.
3. **Missing viewport addon** - Without viewport configuration, responsive stories always render at the default width.
4. **Stale stories** - Stories that don't reflect current component APIs mislead developers and designers.
5. **Over-complicated stories** - Stories with too many props and variations overwhelm users exploring the library.

## Accessibility Considerations

Storybook's a11y addon automatically checks stories against axe-core rules. Configure it to fail builds on violations.

```js
// .storybook/main.js - strict a11y configuration
module.exports = {
  addons: [
    {
      name: '@storybook/addon-a11y',
      options: {
        runOnly: {
          type: 'tag',
          values: ['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa']
        }
      }
    }
  ]
};
```

## Responsive Behavior

Configure viewport addon with Bootstrap's breakpoint values for consistent responsive testing.

```js
// .storybook/preview.js
import * as bootstrap from 'bootstrap';

export const parameters = {
  viewport: {
    viewports: {
      xs: { name: 'Extra Small (575px)', styles: { width: '575px', height: '800px' } },
      sm: { name: 'Small (576px)', styles: { width: '576px', height: '800px' } },
      md: { name: 'Medium (768px)', styles: { width: '768px', height: '800px' } },
      lg: { name: 'Large (992px)', styles: { width: '992px', height: '800px' } },
      xl: { name: 'Extra Large (1200px)', styles: { width: '1200px', height: '800px' } },
      xxl: { name: 'XXL (1400px)', styles: { width: '1400px', height: '800px' } }
    }
  }
};
```
