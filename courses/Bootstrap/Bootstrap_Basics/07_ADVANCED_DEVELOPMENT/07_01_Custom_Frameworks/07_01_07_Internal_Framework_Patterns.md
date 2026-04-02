---
title: "Internal Framework Patterns"
difficulty: 3
category: "Advanced Development"
subcategory: "Custom Frameworks"
prerequisites:
  - Enterprise Architecture
  - Bootstrap 5 Theming
  - Monorepo Management
---

## Overview

Enterprise Bootstrap wrappers create an internal design system that standardizes component usage across teams while maintaining Bootstrap's robust foundation. An internal framework defines shared components, enforces brand compliance, provides team-specific documentation, and manages the upgrade path when Bootstrap releases new versions.

The core pattern involves wrapping Bootstrap in an enterprise layer that adds organization-specific components (employee cards, approval workflows, data grids), enforces design tokens (brand colors, typography, spacing), and provides developer tooling (linters, IDE snippets, component generators). This layer lives in a private package registry and is consumed by multiple application teams.

Key challenges include balancing standardization with team autonomy, managing version drift across projects, and maintaining a contribution model that allows teams to propose new shared components without bottlenecking on a central team.

## Basic Implementation

An internal framework extends Bootstrap with enterprise-specific variables, components, and utilities.

```scss
// _enterprise-variables.scss
// Internal design tokens mapped to Bootstrap variables
$enterprise-primary: #1a3a5c;
$enterprise-secondary: #4a90d9;
$enterprise-accent: #e8913a;

// Override Bootstrap defaults
$primary: $enterprise-primary;
$secondary: $enterprise-secondary;
$info: $enterprise-accent;

// Enterprise-specific spacing scale
$enterprise-spacers: (
  'xs': 0.25rem,
  'sm': 0.5rem,
  'md': 1rem,
  'lg': 1.5rem,
  'xl': 2rem,
  '2xl': 3rem
);

// Typography
$enterprise-font-heading: 'Inter', sans-serif;
$enterprise-font-body: 'Source Sans Pro', sans-serif;
$enterprise-font-mono: 'JetBrains Mono', monospace;

$headings-font-family: $enterprise-font-heading;
$font-family-base: $enterprise-font-body;
```

```scss
// enterprise-framework.scss
@import 'bootstrap/scss/functions';
@import 'enterprise-variables';
@import 'bootstrap/scss/variables';
@import 'bootstrap/scss/mixins';

// Bootstrap core
@import 'bootstrap/scss/bootstrap';

// Enterprise components
@import 'components/approval-card';
@import 'components/employee-badge';
@import 'components/data-grid';
@import 'components/status-indicator';
@import 'components/action-bar';
```

```js
// src/js/enterprise.js
import { EmployeeBadge } from './components/employee-badge';
import { ApprovalCard } from './components/approval-card';
import { ActionBar } from './components/action-bar';

class EnterpriseUI {
  static components = {
    EmployeeBadge,
    ApprovalCard,
    ActionBar
  };

  static init(context = document) {
    Object.entries(this.components).forEach(([name, Component]) => {
      if (Component.selector) {
        context.querySelectorAll(Component.selector).forEach(el => {
          if (!Component.INSTANCES.has(el)) {
            new Component(el);
          }
        });
      }
    });
  }

  static register(name, Component) {
    this.components[name] = Component;
  }
}

export default EnterpriseUI;
```

## Advanced Variations

```scss
// Enterprise data grid component
.ent-data-grid {
  @extend .table-responsive;

  .ent-data-grid__table {
    @extend .table;
    @extend .table-hover;
    margin-bottom: 0;

    th {
      @extend .text-uppercase;
      font-size: $small-font-size;
      letter-spacing: 0.05em;
      color: $text-muted;
      border-bottom-width: 2px;
      user-select: none;

      &[data-sortable] {
        cursor: pointer;

        &::after {
          content: '\2195';
          margin-left: 0.5em;
          opacity: 0.3;
        }

        &[aria-sort="ascending"]::after {
          content: '\2191';
          opacity: 1;
        }

        &[aria-sort="descending"]::after {
          content: '\2193';
          opacity: 1;
        }
      }
    }

    td {
      vertical-align: middle;
    }
  }

  &__pagination {
    @extend .d-flex;
    @extend .justify-content-between;
    @extend .align-items-center;
    padding: map-get($spacers, 3);
    border-top: 1px solid $border-color;
  }
}
```

```js
// Enterprise approval card component
export class ApprovalCard {
  static selector = '[data-component="approval-card"]';
  static INSTANCES = new WeakMap();

  constructor(element) {
    this.element = element;
    this.config = {
      approveEndpoint: element.dataset.approveUrl,
      rejectEndpoint: element.dataset.rejectUrl,
      csrfToken: element.dataset.csrfToken
    };

    this._bindEvents();
    ApprovalCard.INSTANCES.set(this.element, this);
  }

  _bindEvents() {
    this.element.querySelector('[data-action="approve"]')
      ?.addEventListener('click', () => this._handleAction('approve'));

    this.element.querySelector('[data-action="reject"]')
      ?.addEventListener('click', () => this._handleAction('reject'));
  }

  async _handleAction(action) {
    const endpoint = action === 'approve'
      ? this.config.approveEndpoint
      : this.config.rejectEndpoint;

    const button = this.element.querySelector(`[data-action="${action}"]`);
    button.disabled = true;
    button.innerHTML = '<span class="spinner-border spinner-border-sm"></span>';

    try {
      const response = await fetch(endpoint, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-CSRF-Token': this.config.csrfToken
        }
      });

      if (!response.ok) throw new Error('Action failed');

      this.element.classList.add(`approval-card--${action}d`);
      this.element.dispatchEvent(new CustomEvent(`approval:${action}`, {
        detail: await response.json()
      }));
    } catch (error) {
      button.disabled = false;
      button.textContent = action === 'approve' ? 'Approve' : 'Reject';

      this.element.dispatchEvent(new CustomEvent('approval:error', {
        detail: { action, error }
      }));
    }
  }

  dispose() {
    ApprovalCard.INSTANCES.delete(this.element);
  }
}
```

```html
<!-- Enterprise component usage -->
<div class="ent-approval-card card" data-component="approval-card"
     data-approve-url="/api/approvals/123/approve"
     data-reject-url="/api/approvals/123/reject"
     data-csrf-token="abc123">
  <div class="card-header d-flex justify-content-between align-items-center">
    <span class="fw-semibold">Expense Report #ER-2025-042</span>
    <span class="badge bg-warning">Pending</span>
  </div>
  <div class="card-body">
    <dl class="row mb-0">
      <dt class="col-sm-4">Submitter</dt>
      <dd class="col-sm-8">Jane Smith</dd>
      <dt class="col-sm-4">Amount</dt>
      <dd class="col-sm-8">$1,234.56</dd>
      <dt class="col-sm-4">Department</dt>
      <dd class="col-sm-8">Engineering</dd>
    </dl>
  </div>
  <div class="card-footer d-flex gap-2">
    <button class="btn btn-success" data-action="approve">Approve</button>
    <button class="btn btn-outline-danger" data-action="reject">Reject</button>
  </div>
</div>
```

## Best Practices

1. **Use a private npm registry** - Host the internal framework on Artifactory, GitHub Packages, or Verdaccio to control access and versioning.
2. **Maintain a changelog** - Every release must document new components, bug fixes, and breaking changes with migration guides.
3. **Provide ESLint and Stylelint plugins** - Create lint rules that enforce framework conventions, such as requiring enterprise components over raw Bootstrap.
4. **Create IDE snippets** - Provide VS Code snippets for common component patterns so developers can scaffold quickly.
5. **Version Bootstrap independently** - Pin Bootstrap as a peer dependency with a semver range; test upgrades centrally before broadening the range.
6. **Establish a component proposal process** - Define a template for teams to propose new shared components with use cases, API design, and accessibility analysis.
7. **Maintain a visual regression test suite** - Use Percy, Chromatic, or Playwright screenshots to detect unintended visual changes.
8. **Document component ownership** - Each component should have an owning team responsible for maintenance, bug fixes, and feature development.
9. **Provide migration codemods** - When components change APIs, provide codemods that automatically update consuming code.
10. **Use semantic versioning strictly** - Major version for breaking changes, minor for new features, patch for bug fixes.
11. **Keep the framework thin** - Resist adding components that only one team uses; keep the shared layer focused on truly common patterns.
12. **Automate dependency updates** - Use Dependabot or Renovate to keep Bootstrap and other dependencies current with automated PRs.

## Common Pitfalls

1. **Central team bottleneck** - When all component changes must go through one team, feature velocity drops and teams bypass the framework.
2. **Version lock-in** - Pinning to a specific Bootstrap version prevents teams from getting security patches and new features.
3. **Undocumented components** - Adding components without documentation leads to incorrect usage and accessibility violations.
4. **Inconsistent naming** - Mixing naming conventions (camelCase, kebab-case, BEM) across components creates confusion.
5. **Breaking changes without migration** - Changing component APIs without codemods forces manual updates across dozens of projects.

## Accessibility Considerations

Enterprise components must meet WCAG 2.1 AA standards at minimum. Internal frameworks should include accessibility testing as part of the CI pipeline, using tools like axe-core to scan generated components.

```html
<!-- Accessible enterprise status indicator -->
<div class="ent-status" role="status" aria-live="polite">
  <span class="ent-status__dot ent-status__dot--active" aria-hidden="true"></span>
  <span class="ent-status__label">System Operational</span>
  <time class="ent-status__time" datetime="2025-01-15T10:30:00Z">
    Last checked: 2 minutes ago
  </time>
</div>
```

```scss
.ent-status {
  display: inline-flex;
  align-items: center;
  gap: map-get($spacers, 2);

  &__dot {
    width: 8px;
    height: 8px;
    border-radius: 50%;

    &--active { background: $success; }
    &--warning { background: $warning; }
    &--error   { background: $danger; }
    &--inactive { background: $gray-500; }
  }

  // Ensure sufficient color contrast for text
  &__label {
    color: $body-color;
    font-weight: $font-weight-medium;
  }

  &__time {
    color: $text-muted;
    font-size: $small-font-size;
  }
}
```

## Responsive Behavior

Enterprise components must work across all devices, from mobile to large desktop screens used in control rooms. Components should use Bootstrap's responsive grid and provide mobile-optimized variants.

```scss
// Responsive approval card
.ent-approval-card {
  .card-body {
    dl.row {
      dt { @extend .col-12, .col-sm-4; }
      dd { @extend .col-12, .col-sm-8; }
    }
  }

  .card-footer {
    @include media-breakpoint-down(sm) {
      flex-direction: column;

      .btn {
        width: 100%;
      }
    }
  }
}

// Responsive data grid - horizontal scroll on mobile
.ent-data-grid {
  @include media-breakpoint-down(md) {
    .ent-data-grid__table {
      font-size: $small-font-size;

      th, td {
        padding: map-get($spacers, 2);
        white-space: nowrap;
      }
    }
  }
}
```
