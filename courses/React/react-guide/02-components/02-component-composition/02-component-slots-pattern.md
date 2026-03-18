# Component Slots Pattern

## Overview

The component slots pattern is an advanced composition technique that allows you to create highly flexible components by defining explicit "slots" where content can be placed. Similar to how web components and Vue work with slots, this pattern in React gives you even more control over component structure. This guide explores how to implement and use the slots pattern to build powerful, flexible React components.

## Prerequisites

- Understanding of React props
- Knowledge of the children prop
- Familiarity with component composition
- Understanding of React hooks

## Core Concepts

### Understanding Slots

Slots are named placeholders in a component where different types of content can be inserted. While React doesn't have a native slot concept like Vue or Web Components, we can achieve the same functionality using props.

```jsx
// File: src/slots-basics.jsx

import React from 'react';

// Instead of just children, we define explicit slots as props
function Card({ 
  header,      // Slot for header content
  children,    // Default slot (main content)
  footer,      // Slot for footer content
  actions      // Slot for action buttons
}) {
  return (
    <div style={{
      border: '1px solid #ddd',
      borderRadius: '8px',
      overflow: 'hidden'
    }}>
      {/* Header slot */}
      {header && (
        <div style={{
          padding: '16px',
          borderBottom: '1px solid #ddd',
          backgroundColor: '#f5f5f5'
        }}>
          {header}
        </div>
      )}
      
      {/* Default slot (children) */}
      <div style={{ padding: '16px' }}>
        {children}
      </div>
      
      {/* Actions slot */}
      {actions && (
        <div style={{
          padding: '12px 16px',
          borderTop: '1px solid #ddd',
          display: 'flex',
          gap: '8px',
          justifyContent: 'flex-end'
        }}>
          {actions}
        </div>
      )}
      
      {/* Footer slot */}
      {footer && (
        <div style={{
          padding: '12px 16px',
          borderTop: '1px solid #ddd',
          backgroundColor: '#fafafa',
          fontSize: '14px',
          color: '#666'
        }}>
          {footer}
        </div>
      )}
    </div>
  );
}

// Using the slots pattern
function App() {
  return (
    <div>
      <Card
        header={<h2>User Profile</h2>}
        footer={<span>Member since 2024</span>}
      >
        <p>Name: John Doe</p>
        <p>Email: john@example.com</p>
      </Card>
      
      <Card
        header={
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <h2>Shopping Cart</h2>
            <span>3 items</span>
          </div>
        }
        actions={
          <>
            <button>Continue Shopping</button>
            <button>Checkout</button>
          </>
        }
      >
        <p>Your items:</p>
        <ul>
          <li>Laptop - $999</li>
          <li>Mouse - $49</li>
          <li>Keyboard - $149</li>
        </ul>
      </Card>
    </div>
  );
}
```

### Slot Patterns in Detail

```jsx
// File: src/slot-patterns.jsx

import React from 'react';

// 1. Required vs Optional Slots
function ModalRequired({ title, content, onClose }) {
  // Title is required
  if (!title) {
    throw new Error('Modal requires a title');
  }
  
  return (
    <div className="modal-overlay">
      <div className="modal">
        <div className="modal-header">
          <h2>{title}</h2>
          <button onClick={onClose}>×</button>
        </div>
        <div className="modal-body">
          {content}
        </div>
      </div>
    </div>
  );
}

// 2. Default Slot Content
function CardWithDefaults({ 
  header, 
  children, 
  footer,
  variant = 'default' 
}) {
  // Default content if no children provided
  const defaultContent = (
    <p style={{ color: '#999', fontStyle: 'italic' }}>
      No content provided
    </p>
  );
  
  return (
    <div className={`card card-${variant}`}>
      {header && <div className="card-header">{header}</div>}
      <div className="card-body">
        {children || defaultContent}
      </div>
      {footer && <div className="card-footer">{footer}</div>}
    </div>
  );
}

// 3. Multiple Slots of the Same Type
function ActionBar({ 
  leftActions, 
  centerActions, 
  rightActions 
}) {
  return (
    <div className="action-bar">
      <div className="action-bar-left">{leftActions}</div>
      <div className="action-bar-center">{centerActions}</div>
      <div className="action-bar-right">{rightActions}</div>
    </div>
  );
}

function App() {
  return (
    <div>
      {/* Required slot */}
      <ModalRequired 
        title="Confirm Delete"
        content="Are you sure you want to delete this item?"
      />
      
      {/* Default content */}
      <CardWithDefaults>
        <p>Custom content here</p>
      </CardWithDefaults>
      
      {/* Using multiple slots */}
      <ActionBar
        leftActions={<button>Back</button>}
        centerActions={
          <>
            <button>Save</button>
            <button>Cancel</button>
          </>
        }
        rightActions={<span>Step 2 of 5</span>}
      />
    </div>
  );
}
```

### Slots with React Components

You can also pass components as slots, making the pattern even more powerful:

```jsx
// File: src/component-slots.jsx

import React, { useState } from 'react';

// Define small components for different slots
const DefaultHeader = ({ title }) => (
  <div style={{ padding: '16px', backgroundColor: '#f5f5f5' }}>
    <h3 style={{ margin: 0 }}>{title}</h3>
  </div>
);

const DefaultFooter = ({ children }) => (
  <div style={{ padding: '12px', borderTop: '1px solid #ddd' }}>
    {children}
  </div>
);

const SuccessAlert = ({ children }) => (
  <div style={{ 
    padding: '12px', 
    backgroundColor: '#E8F5E9', 
    color: '#2E7D32',
    borderRadius: '4px'
  }}>
    ✓ {children}
  </div>
);

const ErrorAlert = ({ children }) => (
  <div style={{ 
    padding: '12px', 
    backgroundColor: '#FFEBEE', 
    color: '#C62828',
    borderRadius: '4px'
  }}>
    ✗ {children}
  </div>
);

// Flexible container with slot components
function AlertBox({ 
  children, 
  variant = 'info',
  icon: IconComponent,
  action 
}) {
  const variants = {
    info: { bg: '#E3F2FD', border: '#1976D2' },
    success: { bg: '#E8F5E9', border: '#388E3C' },
    warning: { bg: '#FFF3E0', border: '#F57C00' },
    error: { bg: '#FFEBEE', border: '#D32F2F' }
  };
  
  const style = variants[variant] || variants.info;
  
  return (
    <div style={{
      padding: '16px',
      backgroundColor: style.bg,
      border: `1px solid ${style.border}`,
      borderRadius: '8px',
      marginBottom: '16px'
    }}>
      <div style={{ display: 'flex', gap: '12px', alignItems: 'flex-start' }}>
        {IconComponent && <IconComponent />}
        <div style={{ flex: 1 }}>{children}</div>
        {action && <div>{action}</div>}
      </div>
    </div>
  );
}

// Using with different slot components
function App() {
  const InfoIcon = () => <span>ℹ️</span>;
  const SuccessIcon = () => <span>✅</span>;
  
  return (
    <div>
      <AlertBox variant="info" icon={InfoIcon}>
        <strong>Info:</strong> Your order is being processed.
      </AlertBox>
      
      <AlertBox variant="success" icon={SuccessIcon}>
        <strong>Success:</strong> Payment completed successfully!
      </AlertBox>
      
      <AlertBox variant="error">
        <strong>Error:</strong> Unable to process payment.
      </AlertBox>
    </div>
  );
}
```

### Advanced: Slot Registry Pattern

For complex applications, you can create a slot registry pattern:

```jsx
// File: src/slot-registry.jsx

import React, { createContext, useContext, useState } from 'react';

// Context for slot registration
const SlotContext = createContext(null);

// Provider component
function SlotProvider({ children }) {
  const [slots, setSlots] = useState({});
  
  const registerSlot = (name, component) => {
    setSlots(prev => ({ ...prev, [name]: component }));
  };
  
  const unregisterSlot = (name) => {
    setSlots(prev => {
      const { [name]: _, ...rest } = prev;
      return rest;
    });
  };
  
  return (
    <SlotContext.Provider value={{ slots, registerSlot, unregisterSlot }}>
      {children}
    </SlotContext.Provider>
  );
}

// Hook to use slots
function useSlot(name) {
  const { slots } = useContext(SlotContext);
  return slots[name];
}

// Component that renders registered slots
function SlotRenderer({ name, defaultContent = null }) {
  const SlotComponent = useSlot(name);
  return SlotComponent ? <SlotComponent /> : defaultContent;
}

// Usage example
function App() {
  return (
    <SlotProvider>
      <Layout>
        <HeaderSlot>
          <h1>My App</h1>
        </HeaderSlot>
        <SidebarSlot>
          <nav>Navigation here</nav>
        </SidebarSlot>
        <MainContent>
          <p>Main content</p>
        </MainContent>
      </Layout>
    </SlotProvider>
  );
}
```

## Common Mistakes

### Mistake 1: Not Providing Defaults for Slots

```jsx
// ❌ WRONG - No fallback for undefined slots
function BadComponent({ header, children }) {
  return (
    <div>
      {header} {/* Crashes if header is undefined */}
      {children}
    </div>
  );
}

// ✅ CORRECT - Always check if slot has content
function GoodComponent({ header, children, footer }) {
  return (
    <div>
      {header && <div className="header">{header}</div>}
      <div className="body">{children || <p>Default content</p>}</div>
      {footer && <div className="footer">{footer}</div>}
    </div>
  );
}
```

### Mistake 2: Too Many Slots

```jsx
// ❌ WRONG - Too many slots makes the component confusing
function ConfusingCard({ 
  headerLeft, headerCenter, headerRight,
  bodyLeft, bodyCenter, bodyRight,
  footerLeft, footerCenter, footerRight 
}) {
  // This is too complex!
  return <div>{/* ... */}</div>;
}

// ✅ CORRECT - Limit slots and use composition instead
function Card({ header, children, footer }) {
  return (
    <div>
      {header}
      {children}
      {footer}
    </div>
  );
}

// For more complex layouts, compose multiple components
function ComplexCard() {
  return (
    <Card
      header={<HeaderWithTabs />}
      footer={<FooterWithPagination />}
    >
      <GridLayout
        left={<Sidebar />}
        right={<MainContent />}
      />
    </Card>
  );
}
```

### Mistake 3: Not Using Children When Appropriate

```jsx
// ❌ WRONG - Forcing slots when children would work
function BadCard({ content }) { // Named slot
  return <div>{content}</div>;
}

// Better with children
function GoodCard({ children }) { // Default slot
  return <div>{children}</div>;
}

// Usage:
// <BadCard content={<p>Content</p>} />
// vs
// <GoodCard><p>Content</p></GoodCard>
```

## Real-World Example

Let's build a comprehensive page builder using slots:

```jsx
// File: src/components/PageBuilder.jsx

import React from 'react';

// 1. Page Layout with Slots
function PageLayout({ 
  // Named slots
  topBar,
  sidebar,
  header,
  actions,
  children,  // Main content (default slot)
  footer
}) {
  return (
    <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      {/* Top Bar Slot */}
      {topBar && (
        <header style={{ 
          backgroundColor: '#1a1a1a', 
          color: 'white', 
          padding: '8px 16px' 
        }}>
          {topBar}
        </header>
      )}
      
      {/* Header Slot */}
      {header && (
        <header style={{ 
          backgroundColor: '#f5f5f5', 
          padding: '20px',
          borderBottom: '1px solid #ddd'
        }}>
          {header}
        </header>
      )}
      
      {/* Main Area with Sidebar */}
      <div style={{ flex: 1, display: 'flex' }}>
        {/* Sidebar Slot */}
        {sidebar && (
          <aside style={{ 
            width: '250px', 
            borderRight: '1px solid #ddd',
            padding: '16px',
            backgroundColor: '#fafafa'
          }}>
            {sidebar}
          </aside>
        )}
        
        {/* Main Content */}
        <main style={{ flex: 1, padding: '20px' }}>
          {children}
        </main>
        
        {/* Actions Panel Slot */}
        {actions && (
          <aside style={{ 
            width: '200px', 
            borderLeft: '1px solid #ddd',
            padding: '16px'
          }}>
            {actions}
          </aside>
        )}
      </div>
      
      {/* Footer Slot */}
      {footer && (
        <footer style={{ 
          backgroundColor: '#f5f5f5', 
          padding: '16px',
          borderTop: '1px solid #ddd',
          textAlign: 'center'
        }}>
          {footer}
        </footer>
      )}
    </div>
  );
}

// 2. Reusable Slot Components
function PageHeader({ title, subtitle, breadcrumbs }) {
  return (
    <div>
      {breadcrumbs && (
        <nav style={{ marginBottom: '8px', fontSize: '14px', color: '#666' }}>
          {breadcrumbs}
        </nav>
      )}
      <h1 style={{ margin: 0 }}>{title}</h1>
      {subtitle && (
        <p style={{ margin: '8px 0 0 0', color: '#666' }}>{subtitle}</p>
      )}
    </div>
  );
}

function SidebarSection({ title, children }) {
  return (
    <div style={{ marginBottom: '24px' }}>
      {title && (
        <h3 style={{ 
          margin: '0 0 12px 0', 
          fontSize: '14px', 
          textTransform: 'uppercase',
          color: '#666'
        }}>
          {title}
        </h3>
      )}
      {children}
    </div>
  );
}

function ActionPanel({ title, children }) {
  return (
    <div style={{ marginBottom: '20px' }}>
      {title && <h3 style={{ margin: '0 0 12px 0' }}>{title}</h3>}
      {children}
    </div>
  );
}

// 3. Button Slot Component
function ActionButtons({ items, align = 'right' }) {
  return (
    <div style={{ 
      display: 'flex', 
      gap: '8px', 
      justifyContent: align 
    }}>
      {items.map((item, index) => (
        <button
          key={index}
          onClick={item.onClick}
          disabled={item.disabled}
          style={{
            padding: '8px 16px',
            backgroundColor: item.variant === 'primary' ? '#4CAF50' : '#fff',
            color: item.variant === 'primary' ? '#fff' : '#333',
            border: '1px solid #ddd',
            borderRadius: '4px',
            cursor: item.disabled ? 'not-allowed' : 'pointer',
            opacity: item.disabled ? 0.6 : 1
          }}
        >
          {item.label}
        </button>
      ))}
    </div>
  );
}

// 4. Using the Page Builder
function DashboardPage() {
  return (
    <PageLayout
      topBar={
        <div style={{ display: 'flex', justifyContent: 'space-between' }}>
          <span>My Application</span>
          <div>
            <span>Settings</span>
            <span style={{ marginLeft: '16px' }}>Logout</span>
          </div>
        }
      }
      header={
        <PageHeader 
          title="Dashboard"
          subtitle="Welcome back, John!"
          breadcrumbs="Home > Dashboard"
        />
      }
      sidebar={
        <>
          <SidebarSection title="Menu">
            <nav>
              <div>Overview</div>
              <div>Analytics</div>
              <div>Reports</div>
            </nav>
          </SidebarSection>
          <SidebarSection title="Quick Actions">
            <nav>
              <div>New Report</div>
              <div>Export Data</div>
            </nav>
          </SidebarSection>
        </>
      }
      actions={
        <ActionPanel title="Quick Stats">
          <div style={{ padding: '12px', backgroundColor: '#f5f5f5', borderRadius: '4px' }}>
            <div style={{ fontSize: '24px', fontWeight: 'bold' }}>150</div>
            <div style={{ fontSize: '12px', color: '#666' }}>Active Users</div>
          </div>
        </ActionPanel>
      }
      footer={<p>© 2024 My Application. All rights reserved.</p>}
    >
      {/* Main content - using children */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(3, 1fr)', gap: '20px' }}>
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Total Revenue</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold', color: '#4CAF50' }}>$45,000</p>
        </div>
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Orders</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold' }}>234</p>
        </div>
        <div style={{ padding: '20px', border: '1px solid #ddd', borderRadius: '8px' }}>
          <h3>Customers</h3>
          <p style={{ fontSize: '32px', fontWeight: 'bold' }}>1,567</p>
        </div>
      </div>
    </PageLayout>
  );
}

export default DashboardPage;
```

## Key Takeaways

- Slots provide explicit, named placeholders for content in components
- Use separate props for each slot (header, footer, actions, etc.)
- Children is the "default slot" - use when the slot is the main content
- Always provide fallback content for optional slots
- Don't create too many slots - use component composition for complex layouts
- Pass components as slots for maximum flexibility
- The slots pattern is ideal for layout components like Card, Modal, Page

## What's Next

Now that you understand slots, let's explore the compound components pattern - a more advanced composition pattern that uses implicit state sharing between components.
