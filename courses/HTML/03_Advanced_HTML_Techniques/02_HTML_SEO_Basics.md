# HTML SEO Basics

## Topic Title
Search Engine Optimization with HTML

## Concept Explanation

### What is SEO?

SEO (Search Engine Optimization) is the practice of optimizing websites to rank higher in search engine results. HTML plays a crucial role in this process.

### Key SEO Elements

1. **Title Tag** - Most important on-page factor
2. **Meta Description** - Appears in search results
3. **Heading Structure** - Content hierarchy
4. **Semantic HTML** - Helps search engines understand
5. **Image Optimization** - Alt text, file names
6. **Internal Linking** - Site structure

## Code Examples

### Example 1: Complete SEO Setup

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Learn Web Development - Free Online Courses | CodeAcademy</title>
    <meta name="description" content="Master web development with free online courses in HTML, CSS, JavaScript, and Angular. Start learning today!">
    <meta name="keywords" content="web development, html, css, javascript, angular, programming courses">
    <meta name="author" content="CodeAcademy">
    <link rel="canonical" href="https://example.com/web-development">
    
    <!-- Open Graph -->
    <meta property="og:title" content="Learn Web Development - Free Online Courses">
    <meta property="og:description" content="Master web development with free online courses.">
    <meta property="og:image" content="https://example.com/images/courses.jpg">
    <meta property="og:url" content="https://example.com/web-development">
    <meta property="og:type" content="website">
    
    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:title" content="Learn Web Development">
    <meta name="twitter:description" content="Master web development with free online courses.">
    <meta name="twitter:image" content="https://example.com/images/courses.jpg">
</head>
<body>
    <header>
        <h1>Web Development Courses</h1>
        <nav>
            <a href="/html-course" title="Learn HTML">HTML</a>
            <a href="/css-course" title="Learn CSS">CSS</a>
            <a href="/js-course" title="Learn JavaScript">JavaScript</a>
        </nav>
    </header>
    
    <main>
        <article>
            <h2>Why Learn Web Development?</h2>
            <p>Web development is one of the most in-demand skills...</p>
            
            <h3>Start Your Journey</h3>
            <p>Our comprehensive courses cover everything...</p>
        </article>
    </main>
</body>
</html>
```

### Example 2: Heading Structure

```html
<!-- Good heading structure -->
<body>
    <h1>Main Topic</h1>
    
    <section>
        <h2>Major Section</h2>
        <p>Content...</p>
        
        <h3>Subsection</h3>
        <p>Content...</p>
        
        <h3>Another Subsection</h3>
        <p>Content...</p>
    </section>
    
    <section>
        <h2>Another Major Section</h2>
        <p>Content...</p>
    </section>
</body>
```

### Example 3: SEO-Friendly Links

```html
<!-- Good link text -->
<nav>
    <a href="/products/electronics/laptops">Best Laptops for Developers</a>
    <a href="/blog/seo-guide">Complete SEO Guide 2024</a>
    <a href="/contact">Contact Our Support Team</a>
</nav>

<!-- Avoid -->
<!-- Don't use: Click here, Read more, Learn more -->
```

## Best Practices

### Title Tag Best Practices

1. **Unique titles** - Every page should have unique title
2. **Include keywords** - Put important keywords first
3. **Keep under 60 characters** - Avoid truncation
4. **Brand at end** - Usually: Page Title | Brand Name

### Meta Description Best Practices

1. **Unique descriptions** - Each page different
2. **Include CTA** - Action-oriented language
3. **Under 160 characters** - Optimal length
4. **Keywords naturally** - Include primary keyword

### Content Best Practices

1. **Use headings** - Proper hierarchy
2. **Quality content** - Valuable information
3. **Include keywords** - Naturally throughout
4. **Internal links** - Connect related content
