# HTML Accessibility

## Topic Title
Creating Accessible HTML

## Concept Explanation

### What is Web Accessibility?

Web accessibility means making websites usable for all people, including those with disabilities. This includes visual, hearing, motor, and cognitive impairments.

### Why Accessibility Matters

1. **Legal requirements** - ADA, WCAG compliance
2. **Wider audience** - 15% of world population has disability
3. **SEO benefits** - Accessible sites rank better
4. **Better UX** - Benefits everyone, not just disabled users

### Key Accessibility Principles (POUR)

- **Perceivable** - Information must be presentable
- **Operable** - Interface must be operable
- **Understandable** - Content must be understandable
- **Robust** - Content must be interpreted reliably

### ARIA Attributes

ARIA (Accessible Rich Internet Applications) provides additional accessibility information:

- **role** - Defines what an element does
- **aria-label** - Accessible label
- **aria-describedby** - Links to description
- **aria-hidden** - Hides from screen readers
- **aria-expanded** - For expandable elements

## Why This Concept Is Important

Accessibility matters because:

1. **Inclusive design** - Everyone can use your site
2. **Legal compliance** - Avoid lawsuits
3. **SEO improvement** - Better search rankings
4. **Professional requirement** - Expected skill

## Code Examples

### Example 1: Semantic HTML for Accessibility

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Accessible Website</title>
</head>
<body>
    <!-- Skip link for keyboard users -->
    <a href="#main-content" class="skip-link">Skip to main content</a>
    
    <header role="banner">
        <nav role="navigation" aria-label="Main navigation">
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/about">About</a></li>
                <li><a href="/contact">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main id="main-content" role="main">
        <h1>Welcome to Our Website</h1>
        
        <article>
            <h2>About Us</h2>
            <p>We are committed to accessibility.</p>
        </article>
    </main>
    
    <footer role="contentinfo">
        <p>&copy; 2024 Accessible Website</p>
    </footer>
</body>
</html>
```

### Example 2: Accessible Forms

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Accessible Form</title>
</head>
<body>
    <form>
        <!-- Proper label association -->
        <div>
            <label for="name">Name (required)</label>
            <input type="text" id="name" name="name" required 
                   aria-required="true"
                   aria-describedby="name-hint">
            <span id="name-hint">Enter your full name</span>
        </div>
        
        <!-- Fieldset for grouped inputs -->
        <fieldset>
            <legend>Preferred Contact Method</legend>
            <input type="radio" id="contact-email" name="contact" value="email">
            <label for="contact-email">Email</label>
            
            <input type="radio" id="contact-phone" name="contact" value="phone">
            <label for="contact-phone">Phone</label>
        </fieldset>
        
        <!-- Error handling -->
        <div>
            <label for="email">Email</label>
            <input type="email" id="email" name="email"
                   aria-invalid="true"
                   aria-describedby="email-error">
            <span id="email-error" role="alert">Please enter a valid email</span>
        </div>
        
        <button type="submit">Submit</button>
    </form>
</body>
</html>
```

### Example 3: ARIA Examples

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>ARIA Examples</title>
</head>
<body>
    <!-- Live region for dynamic content -->
    <div aria-live="polite" aria-atomic="true">
        Status message here
    </div>
    
    <!-- Expandable/collapsible -->
    <button aria-expanded="false" 
            aria-controls="menu"
            onclick="toggleMenu()">
        Menu
    </button>
    <ul id="menu" hidden>
        <li>Item 1</li>
        <li>Item 2</li>
    </ul>
    
    <!-- Modal dialog -->
    <div role="dialog" 
         aria-modal="true"
         aria-labelledby="dialog-title"
         aria-describedby="dialog-desc">
        <h2 id="dialog-title">Confirm Action</h2>
        <p id="dialog-desc">Are you sure you want to proceed?</p>
        <button>Yes</button>
        <button>No</button>
    </div>
    
    <!-- Progress indicator -->
    <div role="progressbar" 
         aria-valuenow="75" 
         aria-valuemin="0" 
         aria-valuemax="100"
         aria-label="File upload progress">
        75%
    </div>
    
    <!-- Tabs -->
    <div role="tablist" aria-label="Tabs">
        <button role="tab" aria-selected="true" aria-controls="panel1">Tab 1</button>
        <button role="tab" aria-selected="false" aria-controls="panel2">Tab 2</button>
    </div>
    <div role="tabpanel" id="panel1" aria-labelledby="tab1">
        Content for tab 1
    </div>
</body>
</html>
```

### Example 4: Accessible Images

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Accessible Images</title>
</head>
<body>
    <!-- Informative image -->
    <img src="chart.png" 
         alt="Bar chart showing sales increased by 25% in Q4">
    
    <!-- Decorative image -->
    <img src="decorative-pattern.png" 
         alt=""
         role="presentation">
    
    <!-- Complex image with long description -->
    <img src="diagram.png" 
         alt="System architecture diagram"
         aria-describedby="diagram-desc">
    <p id="diagram-desc">Detailed description of the diagram...</p>
    
    <!-- Image with link -->
    <a href="/home">
        <img src="logo.png" alt="Company Home">
    </a>
</body>
</html>
```

### Example 5: Accessible Tables

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Accessible Table</title>
</head>
<body>
    <table>
        <caption>Student Grades - Spring 2024</caption>
        <thead>
            <tr>
                <th scope="col">Student Name</th>
                <th scope="col">Math</th>
                <th scope="col">Science</th>
                <th scope="col">English</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th scope="row">Alice Johnson</th>
                <td>95</td>
                <td>88</td>
                <td>92</td>
            </tr>
            <tr>
                <th scope="row">Bob Smith</th>
                <td>87</td>
                <td>91</td>
                <td>85</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

## Best Practices

### HTML Best Practices

1. **Use semantic HTML** - nav, main, article, etc.
2. **Proper heading hierarchy** - h1, h2, h3 in order
3. **Descriptive link text** - Not "click here"
4. **Alt text for images** - Describe the image

### Form Best Practices

1. **Always label inputs** - Use label element
2. **Group related inputs** - Use fieldset/legend
3. **Error messages** - Clear and accessible
4. **Required fields** - Indicate clearly

### ARIA Best Practices

1. **Use as last resort** - Prefer semantic HTML
2. **Don't override roles** - Don't change element meaning
3. **Keep it simple** - Only use needed attributes

## Real-World Examples

### Example 1: Accessible Navigation

```html
<nav aria-label="Main">
    <ul role="menubar">
        <li role="none">
            <a role="menuitem" href="/">Home</a>
        </li>
        <li role="none">
            <a role="menuitem" href="/products" aria-haspopup="true">Products</a>
        </li>
    </ul>
</nav>
```

### Example 2: Accessible Video Player

```html
<video controls aria-label="Tutorial video">
    <source src="video.mp4" type="video/mp4">
    <track kind="captions" src="captions.vtt" srclang="en" label="English" default>
    Your browser doesn't support video.
</video>
```

## Common Mistakes

### Mistake 1: Missing Alt Text

```html
<!-- Wrong -->
<img src="photo.jpg">

<!-- Correct -->
<img src="photo.jpg" alt="Description">
```

### Mistake 2: Empty Links

```html
<!-- Wrong -->
<a href="page.html"></a>

<!-- Correct -->
<a href="page.html">Link text</a>
```

### Mistake 3: Skipping Headings

```html
<!-- Wrong -->
<h1>Title</h1>
<h3>Section</h3>

<!-- Correct -->
<h1>Title</h1>
<h2>Section</h2>
```

### Mistake 4: Using Placeholder as Label

```html
<!-- Wrong -->
<input placeholder="Email">

<!-- Correct -->
<label>Email<input type="email"></label>
```

## Exercises

### Exercise 1: Audit a Page
Run accessibility audit on a webpage.

### Exercise 2: Fix Forms
Make a form fully accessible.

### Exercise 3: Add ARIA
Add ARIA attributes to interactive elements.

## Mini Practice Tasks

### Task 1: Add Alt Text
Add alt text to an image.

### Task 2: Add Labels
Add labels to form fields.

### Task 3: Headings
Fix heading hierarchy.

### Task 4: Skip Link
Add a skip link.
