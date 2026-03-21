# Semantic HTML in React

## Overview
Semantic HTML uses meaningful HTML elements that describe their content's purpose. In React applications, using semantic HTML is crucial for accessibility because screen readers rely on these elements to navigate and interpret page content. This guide covers how to use semantic HTML effectively in React components.

## Prerequisites
- Understanding of basic HTML elements
- Familiarity with React components
- Knowledge of WCAG guidelines

## Core Concepts

### Semantic Structure Elements

```tsx
// [File: app/components/SemanticLayout.tsx]
// ❌ WRONG - Div soup - no meaning
function BadLayout() {
  return (
    <div className="container">
      <div className="header">Logo</div>
      <div className="nav">Links</div>
      <div className="main-content">Content</div>
      <div className="sidebar">Sidebar</div>
      <div className="footer">Footer</div>
    </div>
  );
}

// ✅ CORRECT - Semantic HTML - clear structure
function GoodLayout() {
  return (
    <>
      <header>
        <div>Logo</div>
        <nav>Links</nav>
      </header>
      <main>
        <article>Content</article>
        <aside>Sidebar</aside>
      </main>
      <footer>Footer</footer>
    </>
  );
}
```

### When to Use Each Element

```tsx
// [File: app/components/ElementGuide.tsx]
// <header> - Introductory content, may contain nav
function Header() {
  return (
    <header>
      <h1>Site Title</h1>
      <nav aria-label="Main navigation">
        <ul>
          <li><a href="/">Home</a></li>
          <li><a href="/about">About</a></li>
        </ul>
      </nav>
    </header>
  );
}

// <nav> - Navigation links
function Navigation() {
  return (
    <nav aria-label="Main">
      <ul>
        <li><a href="/">Home</a></li>
        <li><a href="/products">Products</a></li>
      </ul>
    </nav>
  );
}

// <main> - Primary content of the page
function Main() {
  return (
    <main id="main-content">
      <h1>Page Title</h1>
      <p>Main content here...</p>
    </main>
  );
}

// <article> - Self-contained content
function BlogPost() {
  return (
    <article>
      <h2>Blog Post Title</h2>
      <p>Blog content...</p>
      <footer>By Author</footer>
    </article>
  );
}

// <section> - Thematic grouping
function Section() {
  return (
    <section>
      <h2>Related Products</h2>
      <ul>...</ul>
    </section>
  );
}

// <aside> - Content related to surrounding content
function Sidebar() {
  return (
    <aside>
      <h3>Related Articles</h3>
      <ul>...</ul>
    </aside>
  );
}

// <footer> - Footer content
function Footer() {
  return (
    <footer>
      <p>&copy; 2024 Company</p>
    </footer>
  );
}
```

### Heading Hierarchy

```tsx
// [File: app/components/HeadingExample.tsx]
// ❌ WRONG - Skipping heading levels
function BadHeadings() {
  return (
    <main>
      <h1>Page Title</h1>
      <h3>Section Title</h3> {/* Skipped h2! */}
      <h5>Subsection</h5>   {/* Skipped h4! */}
    </main>
  );
}

// ✅ CORRECT - Sequential heading levels
function GoodHeadings() {
  return (
    <main>
      <h1>Page Title</h1>
      <p>Intro text...</p>
      
      <h2>Section Title</h2>
      <p>Section content...</p>
      
      <h3>Subsection</h3>
      <p>Subsection content...</p>
      
      <h2>Another Section</h2>
      <p>More content...</p>
    </main>
  );
}
```

### Lists and Groupings

```tsx
// [File: app/components/ListExamples.tsx]
// Unordered list - when order doesn't matter
function NavList() {
  return (
    <nav>
      <ul>
        <li><a href="/">Home</a></li>
        <li><a href="/about">About</a></li>
        <li><a href="/contact">Contact</a></li>
      </ul>
    </nav>
  );
}

// Ordered list - when order matters
function StepsList() {
  return (
    <ol>
      <li>First step</li>
      <li>Second step</li>
      <li>Third step</li>
    </ol>
  );
}

// Description list - for term/definition pairs
function TermsList() {
  return (
    <dl>
      <dt>HTML</dt>
      <dd>HyperText Markup Language</dd>
      <dt>CSS</dt>
      <dd>Cascading Style Sheets</dd>
    </dl>
  );
}

// Fieldset - for form groupings
function FormGrouping() {
  return (
    <fieldset>
      <legend>Shipping Address</legend>
      <label>
        Street
        <input name="street" />
      </label>
      <label>
        City
        <input name="city" />
      </label>
    </fieldset>
  );
}
```

### Interactive Elements

```tsx
// [File: app/components/InteractiveElements.tsx]
// Use <button> for actions
function ActionButton() {
  return (
    <button onClick={handleSubmit}>
      Submit Form
    </button>
  );
}

// Use <a> for navigation links
function NavLink({ href, children }) {
  return (
    <a href={href}>
      {children}
    </a>
  );
}

// ❌ WRONG - Using div as button
function BadButton() {
  return (
    <div onClick={handleClick}>
      Click me
    </div>
  );
}

// ✅ CORRECT - Using button element
function GoodButton() {
  return (
    <button onClick={handleClick}>
      Click me
    </button>
  );
}
```

## Common Mistakes

### Mistake 1: Using div for Everything
```tsx
// ❌ WRONG
<div onClick={handleClick} className="button">Submit</div>

// ✅ CORRECT
<button className="button">Submit</button>
```

### Mistake 2: Incorrect Heading Order
```tsx
// ❌ WRONG
<h1>Title</h1>
<h3>Section</h3>

// ✅ CORRECT
<h1>Title</h1>
<h2>Section</h2>
```

### Mistake 3: Missing Landmark Elements
```tsx
// ❌ WRONG - All content in divs
<div className="main">
  <div className="nav">...</div>
  <div className="content">...</div>
  <div className="footer">...</div>
</div>

// ✅ CORRECT - Using landmarks
<main>
  <nav>...</nav>
  <article>...</article>
  <footer>...</footer>
</main>
```

## Real-World Example

Complete accessible page structure:

```tsx
// [File: app/components/AccessiblePage.tsx]
export default function AccessiblePage() {
  return (
    <>
      {/* Skip link for keyboard users */}
      <a href="#main-content" className="skip-link">
        Skip to main content
      </a>
      
      {/* Header with navigation */}
      <header role="banner">
        <h1>My Website</h1>
        <nav aria-label="Main navigation">
          <ul>
            <li><a href="/">Home</a></li>
            <li><a href="/products">Products</a></li>
            <li><a href="/about">About</a></li>
            <li><a href="/contact">Contact</a></li>
          </ul>
        </nav>
      </header>
      
      {/* Main content area */}
      <main id="main-content" role="main">
        <article>
          <header>
            <h2>Welcome to Our Store</h2>
          </header>
          
          <section>
            <h3>Featured Products</h3>
            <ul>
              <li>Product 1</li>
              <li>Product 2</li>
              <li>Product 3</li>
            </ul>
          </section>
          
          <section>
            <h3>Special Offers</h3>
            <p>Check out our latest deals!</p>
          </section>
        </article>
        
        {/* Complementary sidebar */}
        <aside>
          <h3>Related Links</h3>
          <ul>
            <li><a href="/blog">Blog</a></li>
            <li><a href="/news">News</a></li>
          </ul>
        </aside>
      </main>
      
      {/* Footer */}
      <footer role="contentinfo">
        <p>&copy; 2024 My Website. All rights reserved.</p>
        <nav aria-label="Footer navigation">
          <ul>
            <li><a href="/privacy">Privacy Policy</a></li>
            <li><a href="/terms">Terms of Service</a></li>
          </ul>
        </nav>
      </footer>
    </>
  );
}
```

## Key Takeaways
- Use semantic HTML elements to describe content structure
- Use `<header>`, `<nav>`, `<main>`, `<article>`, `<section>`, `<aside>`, `<footer>` for page structure
- Maintain proper heading hierarchy (h1 → h2 → h3)
- Use `<button>` for actions, `<a>` for navigation
- Use `<ul>`/`<ol>` for lists, `<dl>` for definitions
- Use `<fieldset>` and `<legend>` for form groupings
- Screen readers use semantic elements for navigation and comprehension

## What's Next
Continue to [ARIA Roles and Attributes](03-aria-roles-and-attributes.md) to learn how to use ARIA to enhance accessibility when HTML isn't enough.