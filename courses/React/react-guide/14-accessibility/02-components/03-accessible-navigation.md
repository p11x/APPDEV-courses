# Accessible Navigation

## Overview
Navigation patterns must be keyboard accessible and properly structured for screen readers. Learn how to build accessible menus, tabs, and navigation systems.

## Prerequisites
- HTML semantics
- Keyboard navigation

## Core Concepts

### Accessible Navigation Menu

```tsx
// [File: src/components/Navigation/AccessibleNav.tsx]
import React, { useState } from 'react';

interface NavItem {
  label: string;
  href: string;
  children?: NavItem[];
}

export function AccessibleNav({ items }: { items: NavItem[] }) {
  return (
    <nav aria-label="Main navigation">
      <ul className="nav-list">
        {items.map((item, index) => (
          <NavItemWithDropdown key={item.href} item={item} index={index} />
        ))}
      </ul>
    </nav>
  );
}

function NavItemWithDropdown({ item, index }: { item: NavItem; index: number }) {
  const [isOpen, setIsOpen] = useState(false);
  const hasDropdown = item.children && item.children.length > 0;

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Escape') {
      setIsOpen(false);
    }
  };

  return (
    <li 
      className="nav-item"
      onKeyDown={handleKeyDown}
    >
      {hasDropdown ? (
        <>
          <button
            aria-expanded={isOpen}
            aria-haspopup="true"
            aria-label={`${item.label}, has submenu`}
            onClick={() => setIsOpen(!isOpen)}
          >
            {item.label}
            <span aria-hidden="true">{isOpen ? '▲' : '▼'}</span>
          </button>
          
          {isOpen && (
            <ul 
              role="menu" 
              aria-label={item.label}
              className="dropdown-menu"
            >
              {item.children!.map((child) => (
                <li key={child.href} role="menuitem">
                  <a href={child.href}>{child.label}</a>
                </li>
              ))}
            </ul>
          )}
        </>
      ) : (
        <a href={item.href}>{item.label}</a>
      )}
    </li>
  );
}
```

### Skip Links

```tsx
// [File: src/components/Navigation/SkipLinks.tsx]
export function SkipLinks() {
  return (
    <a href="#main-content" className="skip-link">
      Skip to main content
    </a>
  );
}

// Usage in layout
function Layout({ children }) {
  return (
    <>
      <SkipLinks />
      <header>
        <AccessibleNav items={navItems} />
      </header>
      <main id="main-content">
        {children}
      </main>
    </>
  );
}
```

## Key Takeaways
- Use semantic <nav> elements
- Implement skip links for keyboard users
- Use aria-expanded for dropdowns

## What's Next
Continue to [axe-core Testing](03-testing-tools/01-axe-core-testing.md) for accessibility testing.