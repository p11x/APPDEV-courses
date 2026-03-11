# Semantic Elements

## Topic Title
HTML5 Semantic Elements

## Concept Explanation

### What Are Semantic Elements?

Semantic elements clearly describe their meaning to both the browser and developer. Unlike `<div>` which provides no meaning, semantic elements convey the purpose of their content.

### Why Semantic HTML Matters

| Non-Semantic | Semantic |
|--------------|----------|
| `<div>` | `<article>` |
| `<div>` | `<aside>` |
| `<div>` | `<footer>` |
| `<div>` | `<header>` |
| `<div>` | `<main>` |
| `<div>` | `<nav>` |
| `<div>` | `<section>` |

### Key Semantic Elements

- **`<header>`** - Introduction or navigation
- **`<nav>`** - Navigation links
- **`<main>`** - Main content area
- **`<section>`** - Thematic grouping
- **`<article>`** - Self-contained content
- **`<aside>`** - Sidebar content
- **`<footer>`** - Footer information
- **`<figure>`** - Images with captions
- **`<figcaption>`** - Figure caption

## Why This Concept Is Important

Semantic elements matter because:

1. **Accessibility** - Screen readers understand content structure
2. **SEO** - Search engines better index content
3. **Maintainability** - Code is easier to understand
4. **Standards** - Modern best practice
5. **Framework ready** - Angular uses semantic structure

## Step-by-Step Explanation

### Step 1: Page Structure

```html
<body>
    <header>
        <nav>Navigation</nav>
    </header>
    
    <main>
        <section>Content</section>
    </main>
    
    <footer>Footer</footer>
</body>
```

### Step 2: Article vs Section

- **`<section>`** - Thematic grouping of content
- **`<article>`** - Self-contained, independent content

### Step 3: Aside Usage

- **`<aside>`** - Content related to main content but separate

### Step 4: Figure and Figcaption

```html
<figure>
    <img src="image.jpg" alt="Description">
    <figcaption>Caption text</figcaption>
</figure>
```

## Code Examples

### Example 1: Complete Semantic Layout

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Semantic Layout</title>
</head>
<body>
    <!-- Header -->
    <header>
        <h1>My Website</h1>
        <nav>
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/about">About</a></li>
                <li><a href="/services">Services</a></li>
                <li><a href="/contact">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <!-- Main Content -->
    <main>
        <!-- Blog Section -->
        <section>
            <h2>Latest Blog Posts</h2>
            
            <article>
                <h3>Getting Started with HTML</h3>
                <p>Published on January 15, 2024</p>
                <p>HTML is the foundation of every website...</p>
                <a href="/blog/html-basics">Read more</a>
            </article>
            
            <article>
                <h3>Understanding CSS</h3>
                <p>Published on January 10, 2024</p>
                <p>CSS allows you to style your web pages...</p>
                <a href="/blog/css-basics">Read more</a>
            </article>
        </section>
        
        <!-- About Section -->
        <section>
            <h2>About Us</h2>
            <p>We are a web development company dedicated to creating amazing websites.</p>
        </section>
    </main>
    
    <!-- Sidebar -->
    <aside>
        <h3>Categories</h3>
        <ul>
            <li><a href="/category/html">HTML</a></li>
            <li><a href="/category/css">CSS</a></li>
            <li><a href="/category/javascript">JavaScript</a></li>
        </ul>
        
        <h3>Popular Posts</h3>
        <ul>
            <li><a href="/post1">Post 1</a></li>
            <li><a href="/post2">Post 2</a></li>
        </ul>
    </aside>
    
    <!-- Footer -->
    <footer>
        <p>&copy; 2024 My Website. All rights reserved.</p>
        <nav>
            <a href="/privacy">Privacy Policy</a> |
            <a href="/terms">Terms of Service</a>
        </nav>
    </footer>
</body>
</html>
```

### Example 2: Blog Post with Semantic HTML

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Blog Post</title>
</head>
<body>
    <header>
        <h1>Tech Blog</h1>
        <nav>
            <a href="/">Home</a> /
            <a href="/blog">Blog</a> /
            <a href="/about">About</a>
        </nav>
    </header>
    
    <main>
        <article>
            <header>
                <h1>The Future of Web Development</h1>
                <p><time datetime="2024-01-15">January 15, 2024</time> by <strong>John Doe</strong></p>
            </header>
            
            <section>
                <h2>Introduction</h2>
                <p>Web development is constantly evolving. In this article, we'll explore the latest trends and technologies.</p>
            </section>
            
            <section>
                <h2>Key Trends</h2>
                <p>Here are the major trends shaping web development...</p>
            </section>
            
            <section>
                <h2>Conclusion</h2>
                <p>The future looks bright for web developers.</p>
            </section>
            
            <footer>
                <p>Tags: <a href="/tag/web-dev">Web Development</a>, <a href="/tag/tech">Technology</a></p>
            </footer>
        </article>
        
        <aside>
            <h3>About the Author</h3>
            <p>John Doe is a web developer with 10 years of experience.</p>
            
            <h3>Related Posts</h3>
            <ul>
                <li><a href="/post/react">Getting Started with React</a></li>
                <li><a href="/post/angular">Angular vs React</a></li>
            </ul>
        </aside>
    </main>
    
    <footer>
        <p>&copy; 2024 Tech Blog. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Example 3: E-commerce Product Page

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Product Page</title>
</head>
<body>
    <header>
        <h1>TechStore</h1>
        <nav>
            <ul>
                <li><a href="/">Home</a></li>
                <li><a href="/products">Products</a></li>
                <li><a href="/cart">Cart</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <article>
            <h1>Wireless Headphones</h1>
            
            <figure>
                <img src="headphones.jpg" alt="Premium Wireless Headphones">
                <figcaption>Premium Wireless Headphones - Silver</figcaption>
            </figure>
            
            <section>
                <h2>Features</h2>
                <ul>
                    <li>Active Noise Cancellation</li>
                    <li>30-hour battery life</li>
                    <li>Premium comfort</li>
                </ul>
            </section>
            
            <section>
                <h2>Specifications</h2>
                <dl>
                    <dt>Driver Size</dt>
                    <dd>40mm</dd>
                    <dt>Frequency Response</dt>
                    <dd>20Hz - 20kHz</dd>
                    <dt>Weight</dt>
                    <dd>250g</dd>
                </dl>
            </section>
            
            <section>
                <h2>Price</h2>
                <p><strong>$199.99</strong></p>
                <button>Add to Cart</button>
            </section>
        </article>
        
        <aside>
            <h3>Customer Reviews</h3>
            <article>
                <p><strong>5/5 Stars</strong> - Great product!</p>
            </article>
            <article>
                <p><strong>4/5 Stars</strong> - Good value</p>
            </article>
        </aside>
    </main>
    
    <footer>
        <p>&copy; 2024 TechStore. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Example 4: Angular Component Structure

```html
<!-- Angular uses semantic elements in templates -->
<header>
    <nav>
        <a routerLink="/">Home</a>
        <a routerLink="/about">About</a>
    </nav>
</header>

<main>
    <article>
        <header>
            <h1>{{article.title}}</h1>
        </header>
        
        <section [innerHTML]="article.content"></section>
        
        <footer>
            <p>By {{article.author}}</p>
        </footer>
    </article>
</main>

<footer>
    <p>&copy; 2024</p>
</footer>
```

## Best Practices

### Semantic Element Best Practices

1. **Use for meaning, not style** - Semantic elements describe content purpose
2. **Don't over-structure** - Don't use sections where divs suffice
3. **Maintain hierarchy** - Proper heading levels within sections
4. **Multiple articles** - Use article for independent content items

### When to Use Section vs Article

- **Use `<section>`** for thematically related content
- **Use `<article>`** for content that can stand alone

### Accessibility Benefits

1. **Screen readers** - Navigate by semantic landmarks
2. **Keyboard navigation** - Skip to main content
3. **Structure understanding** - Users understand page layout

## Real-World Examples

### Example 1: News Website

```html
<header>
    <h1>News Daily</h1>
    <nav>...</nav>
</header>

<main>
    <section id="breaking">
        <article>Breaking news story</article>
    </section>
    
    <section id="latest">
        <article>News article</article>
        <article>News article</article>
        <article>News article</article>
    </section>
</main>

<aside>
    <section id="trending">
        <h2>Trending</h2>
    </section>
</aside>

<footer>...</footer>
```

### Example 2: Documentation Site

```html
<main>
    <nav id="toc">
        <h1>Table of Contents</h1>
    </nav>
    
    <article>
        <h1>Getting Started</h1>
        <section id="installation">
            <h2>Installation</h2>
        </section>
        <section id="configuration">
            <h2>Configuration</h2>
        </section>
    </article>
</main>
```

## Common Mistakes Students Make

### Mistake 1: Using Semantic Elements for Layout

```html
<!-- Wrong - using section for layout -->
<section>
    <div>Sidebar content</div>
</section>

<!-- Correct - use semantic meaning -->
<aside>Sidebar content</aside>
```

### Mistake 2: Replacing All Divs

```html
<!-- Not every div needs to be semantic -->
<div class="wrapper">
    <div class="button-group">
        <!-- Divs are fine for grouping -->
    </div>
</div>
```

### Mistake 3: Missing Header/Footer in Article

```html
<!-- Better - with header and footer -->
<article>
    <header>
        <h2>Article Title</h2>
    </header>
    <p>Content</p>
    <footer>Author info</footer>
</article>
```

### Mistake 4: Nesting Errors

```html
<!-- Wrong - article inside section inside article -->
<article>
    <section>
        <article>Wrong!</article>
    </section>
</article>

<!-- Correct -->
<article>
    <section>Valid</section>
</article>
```

## Exercises

### Exercise 1: Convert to Semantic HTML
Take a non-semantic page and convert it to semantic HTML.

### Exercise 2: Create Blog Layout
Create a blog post with proper semantic structure.

### Exercise 3: Add Navigation
Add semantic navigation to a page.

### Exercise 4: Figure and Figcaption
Add images with captions using figure/figcaption.

## Mini Practice Tasks

### Task 1: Header
Add a header element to a page.

### Task 2: Nav
Add navigation links in a nav element.

### Task 3: Main
Wrap main content in main element.

### Task 4: Article
Add an article element to a page.

### Task 5: Footer
Add a footer to a page.
