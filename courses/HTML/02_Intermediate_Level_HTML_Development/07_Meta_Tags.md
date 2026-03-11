# Meta Tags

## Topic Title
HTML Meta Tags and SEO

## Concept Explanation

### What Are Meta Tags?

Meta tags provide metadata about the HTML document. They don't appear on the page but tell browsers and search engines about the page.

### Types of Meta Tags

1. **Character Set** - Character encoding
2. **Viewport** - Mobile responsiveness
3. **Description** - Search engine snippet
4. **Keywords** - Search terms (deprecated)
5. **Author** - Document author
6. **Open Graph** - Social media sharing
7. **Robots** - Search engine indexing

### Essential Meta Tags

```html
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<meta name="description" content="Page description">
```

## Why This Concept Is Important

Meta tags matter because:

1. **SEO** - Search engine visibility
2. **Social sharing** - Preview on social media
3. **Mobile** - Responsive design support
4. **Browser** - Character encoding and rendering

## Code Examples

### Example 1: Complete Head Section

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <!-- Character encoding -->
    <meta charset="UTF-8">
    
    <!-- Viewport for responsive design -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    
    <!-- Page title -->
    <title>Page Title | Website Name</title>
    
    <!-- Meta description -->
    <meta name="description" content="A brief description of the page content for search engines.">
    
    <!-- Keywords (deprecated but sometimes used) -->
    <meta name="keywords" content="html, css, web development, tutorial">
    
    <!-- Author -->
    <meta name="author" content="John Doe">
    
    <!-- Robots -->
    <meta name="robots" content="index, follow">
    
    <!-- Open Graph for social media -->
    <meta property="og:title" content="Page Title">
    <meta property="og:description" content="Description for social media">
    <meta property="og:image" content="image.jpg">
    <meta property="og:url" content="https://example.com/page">
    
    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:title" content="Page Title">
    <meta name="twitter:description" content="Description">
    <meta name="twitter:image" content="image.jpg">
    
    <!-- Favicon -->
    <link rel="icon" href="favicon.ico">
</head>
<body>
    <h1>Page Content</h1>
</body>
</html>
```

### Example 2: SEO Meta Tags

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    
    <!-- Important for SEO -->
    <title>Best Web Development Courses | Learn HTML CSS JS</title>
    
    <meta name="description" content="Learn web development with our comprehensive courses. 
    Master HTML, CSS, JavaScript, and modern frameworks like Angular and React. 
    Start coding today!">
    
    <meta name="keywords" content="web development, html, css, javascript, 
    programming course, angular, react">
    
    <meta name="author" content="Code Academy">
    
    <!-- Canonical URL -->
    <link rel="canonical" href="https://example.com/web-development">
    
    <!-- Open Graph -->
    <meta property="og:type" content="website">
    <meta property="og:title" content="Best Web Development Courses">
    <meta property="og:description" content="Master web development from scratch">
    <meta property="og:image" content="https://example.com/og-image.jpg">
    <meta property="og:url" content="https://example.com/web-development">
    <meta property="og:site_name" content="Code Academy">
</head>
</html>
```

### Example 3: Social Media Meta Tags

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    
    <!-- Open Graph for Facebook -->
    <meta property="og:title" content="Amazing Product - Must Buy!">
    <meta property="og:description" content="Get this incredible product today. Limited time offer!">
    <meta property="og:image" content="https://example.com/product.jpg">
    <meta property="og:url" content="https://example.com/product">
    <meta property="og:type" content="product">
    <meta property="og:price:amount" content="99.99">
    <meta property="og:price:currency" content="USD">
    
    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:title" content="Amazing Product - Must Buy!">
    <meta name="twitter:description" content="Get this incredible product today.">
    <meta name="twitter:image" content="https://example.com/product.jpg">
    <meta name="twitter:site" content="@company">
    <meta name="twitter:creator" content="@author">
</head>
</html>
```

### Example 4: Mobile Meta Tags

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    
    <!-- Essential for mobile -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    
    <!-- Mobile-specific -->
    <meta name="mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="apple-mobile-web-app-title" content="App Name">
    <meta name="format-detection" content="telephone=no">
</head>
</html>
```

## Best Practices

### SEO Best Practices

1. **Unique title** - Every page should have unique title
2. **Unique description** - Write unique meta descriptions
3. **Keep titles under 60 characters** - Avoid truncation
4. **Keep descriptions under 160 characters** - Optimal length

### Social Media Best Practices

1. **Use Open Graph** - For Facebook sharing
2. **Use Twitter Cards** - For Twitter sharing
3. **Use proper image sizes** - 1200x630 for og:image

## Real-World Examples

### Example 1: Blog Post

```html
<meta property="og:type" content="article">
<meta property="article:published_time" content="2024-01-15">
<meta property="article:author" content="https://example.com/author/john">
<meta property="article:section" content="Technology">
<meta property="article:tag" content="Web Development">
```

### Example 2: E-commerce

```html
<meta property="og:type" content="product">
<meta property="product:price:amount" content="29.99">
<meta property="product:price:currency" content="USD">
<meta property="product:availability" content="in stock">
```

## Common Mistakes

### Mistake 1: Duplicate Meta Tags

```html
<!-- Wrong - multiple descriptions -->
<meta name="description" content="First description">
<meta name="description" content="Second description">

<!-- Correct - one description -->
<meta name="description" content="Single description">
```

### Mistake 2: Missing Viewport

```html
<!-- Wrong - no viewport -->
<meta charset="UTF-8">

<!-- Correct - with viewport -->
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
```

### Mistake 3: Too Long Descriptions

```html
<!-- Wrong - too long -->
<meta name="description" content="Very long description that goes on and on...">

<!-- Correct - optimal length -->
<meta name="description" content="Short, descriptive sentence about the page.">
```

## Exercises

### Exercise 1: Add Basic Meta Tags
Add charset and viewport meta tags.

### Exercise 2: Add SEO Tags
Add title, description, and keywords.

### Exercise 3: Add Open Graph
Add Open Graph meta tags for social sharing.

## Mini Practice Tasks

### Task 1: Charset
Add character set meta tag.

### Task 2: Viewport
Add viewport meta tag.

### Task 3: Description
Add meta description.

### Task 4: Open Graph
Add og:title and og:description.
