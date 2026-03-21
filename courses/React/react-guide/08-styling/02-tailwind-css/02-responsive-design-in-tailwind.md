# Responsive Design in Tailwind CSS

## Overview

Tailwind CSS uses a mobile-first breakpoint system that makes responsive design intuitive. By applying base styles for mobile and adding responsive variants with min-width breakpoints, you can create fluid layouts that adapt gracefully to any screen size. This guide covers Tailwind's breakpoint system, responsive grid layouts, hiding/showing elements, and responsive typography.

## Prerequisites

- Completed Tailwind Setup guide
- Understanding of CSS flexbox and grid
- Familiarity with responsive design concepts

## Core Concepts

### Tailwind's Breakpoint System

Tailwind includes five responsive breakpoints by default:

```css
/* Default breakpoints (min-width) */
/* sm: 640px - Small phones landscape to tablets */
/* md: 768px - Tablets */
/* lg: 1024px - Laptops */
/* xl: 1280px - Desktops */
/* 2xl: 1536px - Large desktops */
```

### Using Responsive Classes

Apply responsive classes by prefixing with the breakpoint name:

```tsx
// File: src/components/ResponsiveText.tsx

function ResponsiveText() {
  return (
    <div>
      {/* Text that's small on mobile, medium on tablets, large on desktop */}
      <p className="text-sm md:text-base lg:text-lg xl:text-xl">
        Responsive text size
      </p>
      
      {/* A container that changes width at different breakpoints */}
      <div className="w-full md:w-3/4 lg:w-1/2 xl:w-1/3 mx-auto">
        <p>Container width adjusts responsively</p>
      </div>
    </div>
  );
}
```

### Responsive Grid Layouts

Create flexible grid layouts that adapt to screen sizes:

```tsx
// File: src/components/ProductGrid.tsx

interface Product {
  id: string;
  name: string;
  price: number;
  image: string;
}

interface ProductGridProps {
  products: Product[];
}

function ProductGrid({ products }: ProductGridProps) {
  return (
    <div className="grid gap-6">
      {/* Grid that shows 1 column on mobile, 2 on tablets, 3 on laptops, 4 on desktops */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {products.map((product) => (
          <div 
            key={product.id}
            className="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition-shadow"
          >
            <img
              src={product.image}
              alt={product.name}
              className="w-full h-48 object-cover"
            />
            <div className="p-4">
              <h3 className="font-semibold text-gray-900">{product.name}</h3>
              <p className="text-blue-600 font-bold mt-1">
                ${product.price.toFixed(2)}
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
```

### Responsive Navigation

Create a responsive navbar with mobile menu:

```tsx
// File: src/components/Navbar.tsx

import { useState } from 'react';

function Navbar() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  return (
    <nav className="bg-white shadow-md">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between h-16">
          {/* Logo - always visible */}
          <div className="flex-shrink-0 flex items-center">
            <a href="/" className="text-xl font-bold text-blue-600">
              MyApp
            </a>
          </div>

          {/* Desktop navigation - hidden on mobile, visible on md and up */}
          <div className="hidden md:flex items-center space-x-8">
            <a href="/" className="text-gray-700 hover:text-blue-600">
              Home
            </a>
            <a href="/about" className="text-gray-700 hover:text-blue-600">
              About
            </a>
            <a href="/services" className="text-gray-700 hover:text-blue-600">
              Services
            </a>
            <a href="/contact" className="text-gray-700 hover:text-blue-600">
              Contact
            </a>
            <button className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700">
              Get Started
            </button>
          </div>

          {/* Mobile menu button - visible on mobile, hidden on md and up */}
          <div className="flex items-center md:hidden">
            <button
              onClick={() => setIsMenuOpen(!isMenuOpen)}
              className="text-gray-700 hover:text-gray-900 p-2"
              aria-label="Toggle menu"
            >
              <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                {isMenuOpen ? (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                ) : (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
                )}
              </svg>
            </button>
          </div>
        </div>

        {/* Mobile menu - visible on mobile, hidden on md and up */}
        <div className={`md:hidden ${isMenuOpen ? 'block' : 'hidden'}`}>
          <div className="px-2 pt-2 pb-3 space-y-1 sm:px-3">
            <a href="/" className="block px-3 py-2 text-gray-700 hover:bg-gray-100 rounded-md">
              Home
            </a>
            <a href="/about" className="block px-3 py-2 text-gray-700 hover:bg-gray-100 rounded-md">
              About
            </a>
            <a href="/services" className="block px-3 py-2 text-gray-700 hover:bg-gray-100 rounded-md">
              Services
            </a>
            <a href="/contact" className="block px-3 py-2 text-gray-700 hover:bg-gray-100 rounded-md">
              Contact
            </a>
          </div>
        </div>
      </div>
    </nav>
  );
}
```

### Hiding and Showing Elements

Use responsive display utilities to show/hide elements:

```tsx
// File: src/components/VisibilityExamples.tsx

function VisibilityExamples() {
  return (
    <div>
      {/* Only visible on mobile, hidden on md+ */}
      <div className="block md:hidden">
        Mobile Only Content
      </div>

      {/* Hidden on mobile, visible on md+ */}
      <div className="hidden md:block">
        Desktop/Tablet Only Content
      </div>

      {/* Visible on lg and above */}
      <div className="hidden lg:block">
        Large Desktop Content
      </div>

      {/* Stack on mobile, inline on md+ */}
      <div className="flex flex-col md:flex-row">
        <div>Item 1</div>
        <div>Item 2</div>
      </div>
    </div>
  );
}
```

### Responsive Typography

Adjust font sizes and spacing based on screen size:

```tsx
// File: src/components/ResponsiveTypography.tsx

function ResponsiveTypography() {
  return (
    <article className="prose prose-sm sm:prose md:prose-lg lg:prose-xl mx-auto">
      <h1 className="text-2xl sm:text-3xl md:text-4xl lg:text-5xl font-bold">
        Responsive Headline
      </h1>
      
      <p className="text-sm sm:text-base md:text-lg leading-relaxed">
        This paragraph adjusts its size based on the viewport. 
        On mobile it's small and compact, but becomes more readable 
        with larger text on tablets and desktops.
      </p>
      
      <h2 className="text-xl sm:text-2xl md:text-3xl font-semibold mt-6 mb-4">
        Section Title
      </h2>
      
      <p className="text-sm sm:text-base text-gray-600">
        Supporting text that provides additional context. The line height
        also adjusts for better readability on different screen sizes.
      </p>
    </article>
  );
}
```

## Common Mistakes

### Mistake 1: Using Max-Width Instead of Min-Width

Tailwind uses min-width breakpoints—styles apply at and above the breakpoint.

```tsx
// ❌ WRONG - These classes might conflict unexpectedly
<div className="max-w-md lg:max-w-xl">

// ✅ CORRECT - Mobile-first approach
<div className="w-full md:w-3/4 lg:w-1/2">
```

### Mistake 2: Not Testing on Actual Devices

Always test responsive designs on real devices, not just browser dev tools.

## Real-World Example

A complete responsive dashboard layout:

```tsx
// File: src/components/Dashboard.tsx

import { useState } from 'react';

interface DashboardProps {
  children: React.ReactNode;
}

export function Dashboard({ children }: DashboardProps) {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Mobile sidebar overlay */}
      {sidebarOpen && (
        <div 
          className="fixed inset-0 z-40 bg-black/50 md:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Sidebar - fixed on mobile, static on desktop */}
      <aside 
        className={`
          fixed inset-y-0 left-0 z-50 w-64 bg-gray-900 text-white transform transition-transform duration-300
          ${sidebarOpen ? 'translate-x-0' : '-translate-x-full'}
          md:relative md:translate-x-0
        `}
      >
        <div className="p-6">
          <h1 className="text-2xl font-bold">Dashboard</h1>
        </div>
        
        <nav className="mt-6">
          <a href="#" className="flex items-center px-6 py-3 bg-gray-800">
            <span className="ml-3">Overview</span>
          </a>
          <a href="#" className="flex items-center px-6 py-3 hover:bg-gray-800">
            <span className="ml-3">Analytics</span>
          </a>
          <a href="#" className="flex items-center px-6 py-3 hover:bg-gray-800">
            <span className="ml-3">Reports</span>
          </a>
          <a href="#" className="flex items-center px-6 py-3 hover:bg-gray-800">
            <span className="ml-3">Settings</span>
          </a>
        </nav>
      </aside>

      {/* Main content area */}
      <div className="flex-1 flex flex-col">
        {/* Top header */}
        <header className="bg-white shadow-sm h-16 flex items-center justify-between px-4 sm:px-6">
          {/* Mobile menu button */}
          <button
            className="md:hidden p-2 text-gray-600"
            onClick={() => setSidebarOpen(true)}
          >
            <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>

          {/* Search - hidden on small screens */}
          <div className="hidden md:block flex-1 max-w-md">
            <input
              type="search"
              placeholder="Search..."
              className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>

          {/* User menu */}
          <div className="flex items-center gap-4">
            <button className="p-2 text-gray-600 hover:bg-gray-100 rounded-lg">
              <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
              </svg>
            </button>
            <div className="w-8 h-8 bg-blue-500 rounded-full flex items-center justify-center text-white font-medium">
              JD
            </div>
          </div>
        </header>

        {/* Main content */}
        <main className="flex-1 p-4 sm:p-6 lg:p-8">
          {/* Stats grid - responsive columns */}
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <p className="text-sm text-gray-500">Total Users</p>
              <p className="text-2xl font-bold mt-1">12,345</p>
            </div>
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <p className="text-sm text-gray-500">Revenue</p>
              <p className="text-2xl font-bold mt-1">$45,678</p>
            </div>
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <p className="text-sm text-gray-500">Orders</p>
              <p className="text-2xl font-bold mt-1">1,234</p>
            </div>
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <p className="text-sm text-gray-500">Growth</p>
              <p className="text-2xl font-bold mt-1 text-green-600">+23%</p>
            </div>
          </div>

          {/* Chart section - stacked on mobile, side by side on desktop */}
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <h2 className="text-lg font-semibold mb-4">Revenue Over Time</h2>
              <div className="h-64 bg-gray-100 rounded-lg flex items-center justify-center">
                Chart placeholder
              </div>
            </div>
            <div className="bg-white rounded-xl p-6 shadow-sm">
              <h2 className="text-lg font-semibold mb-4">Recent Activity</h2>
              <div className="space-y-4">
                {[1, 2, 3, 4].map((i) => (
                  <div key={i} className="flex items-center gap-4">
                    <div className="w-10 h-10 bg-gray-100 rounded-full" />
                    <div className="flex-1">
                      <p className="text-sm font-medium">Activity {i}</p>
                      <p className="text-xs text-gray-500">2 hours ago</p>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </main>
      </div>
    </div>
  );
}
```

## Key Takeaways

- Tailwind uses mobile-first min-width breakpoints
- Apply base styles for mobile, add responsive prefixes for larger screens
- Use `sm:`, `md:`, `lg:`, `xl:`, `2xl:` prefixes
- Hide elements with `hidden` and show with `block` at specific breakpoints
- Use `flex-col` on mobile and `flex-row` on desktop for responsive layouts
- Test on actual devices, not just browser dev tools

## What's Next

Continue to [Dark Mode Implementation](/08-styling/02-tailwind-css/03-dark-mode-implementation.md) to learn how to implement dark mode support in Tailwind CSS.