---
title: "React Bootstrap Setup"
topic: "Framework Integration"
difficulty: 2
duration: "30 minutes"
prerequisites: ["React fundamentals", "JSX syntax", "npm basics"]
tags: ["react", "react-bootstrap", "bootstrap", "jsx", "components"]
---

## Overview

React Bootstrap replaces Bootstrap's JavaScript and jQuery with React components, providing a fully React-native implementation of every Bootstrap 5 component. Instead of using Bootstrap's HTML markup and jQuery-based interactions, `react-bootstrap` offers pre-built components (`<Button>`, `<Modal>`, `<Navbar>`) that manage their own state, DOM, and ARIA attributes through React's declarative paradigm.

The library maps 1:1 to Bootstrap's CSS, so all Bootstrap classes and custom themes work identically. You import the Bootstrap CSS separately (or use SCSS for customization), and use `react-bootstrap` components in JSX. This eliminates jQuery dependency conflicts, enables server-side rendering compatibility, and integrates with React's component lifecycle, hooks, and context system.

## Basic Installation

```bash
npx create-react-app my-bootstrap-app
cd my-bootstrap-app
npm install react-bootstrap bootstrap
```

Import Bootstrap CSS in your entry point:

```js
// src/index.js
import 'bootstrap/dist/css/bootstrap.min.css';
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<React.StrictMode><App /></React.StrictMode>);
```

Basic component usage:

```jsx
// src/App.jsx
import { Container, Row, Col, Button, Card } from 'react-bootstrap';

function App() {
  return (
    <Container className="py-5">
      <Row>
        <Col md={8} className="mx-auto">
          <Card>
            <Card.Header as="h5">React Bootstrap</Card.Header>
            <Card.Body>
              <Card.Title>Component-based Bootstrap</Card.Title>
              <Card.Text>
                Bootstrap components as React primitives.
              </Card.Text>
              <Button variant="primary">Get Started</Button>
              <Button variant="outline-secondary" className="ms-2">
                Learn More
              </Button>
            </Card.Body>
          </Card>
        </Col>
      </Row>
    </Container>
  );
}

export default App;
```

## Advanced Variations

### Custom SCSS Theme Integration

```js
// src/index.js
import './scss/custom.scss';
import App from './App';
```

```scss
// src/scss/custom.scss
$primary: #6366f1;
$enable-rounded: true;
$enable-shadows: true;

@import 'bootstrap/scss/bootstrap';
```

### Component Composition Patterns

```jsx
// src/components/ConfirmModal.jsx
import { useState } from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

export function ConfirmModal({ show, onHide, onConfirm, title, children }) {
  const [reason, setReason] = useState('');

  const handleConfirm = () => {
    onConfirm(reason);
    setReason('');
    onHide();
  };

  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {children}
        <Form.Group className="mt-3">
          <Form.Label>Reason (optional)</Form.Label>
          <Form.Control
            as="textarea"
            rows={2}
            value={reason}
            onChange={(e) => setReason(e.target.value)}
            placeholder="Enter reason..."
          />
        </Form.Group>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>Cancel</Button>
        <Button variant="danger" onClick={handleConfirm}>Confirm</Button>
      </Modal.Footer>
    </Modal>
  );
}
```

### React-Bootstrap with TypeScript

```tsx
// src/components/AlertBanner.tsx
import { Alert, AlertProps } from 'react-bootstrap';
import { useState } from 'react';

interface AlertBannerProps extends Omit<AlertProps, 'onClose'> {
  message: string;
  dismissible?: boolean;
}

export function AlertBanner({
  variant = 'info',
  message,
  dismissible = true,
}: AlertBannerProps) {
  const [show, setShow] = useState(true);

  if (!show) return null;

  return (
    <Alert
      variant={variant}
      dismissible={dismissible}
      onClose={() => setShow(false)}
    >
      {message}
    </Alert>
  );
}
```

### Custom Theme Provider

```jsx
// src/theme/ThemeProvider.jsx
import { createContext, useContext, useState } from 'react';

const ThemeContext = createContext();

export function BootstrapThemeProvider({ children }) {
  const [theme, setTheme] = useState('light');

  const toggleTheme = () => {
    setTheme(prev => {
      const next = prev === 'light' ? 'dark' : 'light';
      document.documentElement.setAttribute('data-bs-theme', next);
      return next;
    });
  };

  return (
    <ThemeContext.Provider value={{ theme, toggleTheme }}>
      <div data-bs-theme={theme}>{children}</div>
    </ThemeContext.Provider>
  );
}

export const useTheme = () => useContext(ThemeContext);
```

## Best Practices

1. **Import `react-bootstrap` components individually** (named imports) to enable tree-shaking and reduce bundle size.
2. **Use `react-bootstrap` components instead of raw HTML** with `data-bs-*` attributes — the library manages ARIA and state automatically.
3. **Import only the Bootstrap CSS** you need, or use SCSS for selective component inclusion to minimize CSS bundle size.
4. **Leverage the `as` prop** to render components as different HTML elements (`<Button as="a" href="/">`).
5. **Use Bootstrap utility classes via `className`** — `react-bootstrap` components accept all standard Bootstrap CSS classes.
6. **Wrap your app in `<ThemeProvider>`** for consistent `data-bs-theme` attribute management across components.
7. **Use the `bsPrefix` prop** to customize CSS class prefixes for BEM-style naming or migration scenarios.
8. **Forward refs** with `React.forwardRef` when building wrapper components around `react-bootstrap` primitives.
9. **Use controlled components** (`show`, `onHide` props) for modals and dropdowns rather than relying on Bootstrap's jQuery toggling.
10. **Avoid mixing `react-bootstrap` with Bootstrap's jQuery plugins** — both attempt to manage the same DOM, causing conflicts.

## Common Pitfalls

1. **Importing both `bootstrap` JS and `react-bootstrap`** causes duplicate event listeners and conflicting DOM manipulations.
2. **Forgetting to import Bootstrap CSS** results in unstyled components that render as plain HTML elements.
3. **Using `data-bs-toggle` attributes** directly — `react-bootstrap` manages toggling through props, not data attributes.
4. **Not using controlled state for modals** leads to inconsistent open/close behavior when React re-renders.
5. **Version mismatch** between `react-bootstrap` and `bootstrap` CSS can cause class name or structure discrepancies.

## Accessibility Considerations

React Bootstrap automatically manages ARIA attributes on interactive components. Modals receive `role="dialog"`, `aria-modal="true"`, and focus trapping. Dropdowns set `aria-expanded`, `aria-haspopup`, and manage keyboard navigation (arrow keys, Escape). Alerts include `role="alert"` by default. Use `aria-label` and `aria-labelledby` props on components that need additional labeling. The `visually-hidden` prop on many components renders screen-reader-only text. Avoid overriding ARIA attributes managed by the library unless you have specific accessibility requirements.

## Responsive Behavior

React Bootstrap's grid components (`Container`, `Row`, `Col`) accept breakpoint-specific props matching Bootstrap's CSS classes. Use `Col md={6} lg={4}` for responsive column sizing. The `Col` component supports `xs`, `sm`, `md`, `lg`, `xl`, and `xxl` props. Responsive utility classes work via `className` — `className="d-none d-md-block"` hides elements below the `md` breakpoint. All responsive behavior is CSS-driven; React Bootstrap's JavaScript components are breakpoint-agnostic by default.