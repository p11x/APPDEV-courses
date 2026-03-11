# Project 4: Semantic Blog Page

## Project Overview

### Project Title
Semantic Blog Page Layout

### Project Description
Create a fully semantic blog page using HTML5 semantic elements including header, navigation, articles, sidebar, and footer. This project demonstrates advanced semantic HTML knowledge.

### Learning Objectives

- Use semantic HTML5 elements correctly
- Create accessible blog structure
- Apply proper heading hierarchy
- Include all blog components

### Estimated Duration
2-3 hours

---

## Project Requirements

### Required Sections

1. **Header**
   - Site logo/name
   - Main navigation
   - Site tagline

2. **Navigation**
   - Home, About, Categories, Contact links
   - Semantic nav element

3. **Main Content Area**
   - Featured blog post (article)
   - Multiple blog articles
   - Proper article structure with header, content, footer

4. **Sidebar**
   - About author
   - Categories list
   - Recent posts
   - Tags

5. **Footer**
   - Copyright
   - Social links
   - Secondary navigation

---

## Complete Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>TechBlog - Web Development Insights</title>
    <meta name="description" content="Expert insights on web development, programming, and technology">
    <meta name="author" content="TechBlog Team">
    
    <!-- Open Graph -->
    <meta property="og:title" content="TechBlog - Web Development Insights">
    <meta property="og:description" content="Expert insights on web development">
    <meta property="og:type" content="website">
</head>
<body>
    <!-- Site Header -->
    <header role="banner" style="background: #2c3e50; color: white; padding: 20px;">
        <div style="max-width: 1200px; margin: 0 auto; display: flex; justify-content: space-between; align-items: center;">
            <div>
                <h1 style="margin: 0; font-size: 2em;">TechBlog</h1>
                <p style="margin: 5px 0 0; opacity: 0.8;">Web Development Insights</p>
            </div>
            
            <!-- Main Navigation -->
            <nav role="navigation" aria-label="Main navigation">
                <ul style="display: flex; list-style: none; gap: 30px; margin: 0; padding: 0;">
                    <li><a href="/" style="color: white; text-decoration: none;">Home</a></li>
                    <li><a href="/about" style="color: white; text-decoration: none;">About</a></li>
                    <li><a href="/categories" style="color: white; text-decoration: none;">Categories</a></li>
                    <li><a href="/contact" style="color: white; text-decoration: none;">Contact</a></li>
                </ul>
            </nav>
        </div>
    </header>
    
    <!-- Main Content -->
    <main role="main" style="max-width: 1200px; margin: 40px auto; padding: 0 20px; display: grid; grid-template-columns: 2fr 1fr; gap: 40px;">
        
        <!-- Blog Posts -->
        <div>
            <!-- Featured Article -->
            <article style="margin-bottom: 40px; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;">
                <img src="featured-post.jpg" alt="Featured post image" style="width: 100%; height: 300px; object-fit: cover;">
                <div style="padding: 30px;">
                    <header>
                        <span style="background: #3498db; color: white; padding: 5px 10px; border-radius: 3px; font-size: 0.8em;">Featured</span>
                        <h2 style="margin: 15px 0; font-size: 1.8em;">
                            <a href="/posts/getting-started-with-angular" style="color: #333; text-decoration: none;">
                                Getting Started with Angular: A Complete Guide
                            </a>
                        </h2>
                        <div style="color: #666; font-size: 0.9em; margin-bottom: 15px;">
                            <time datetime="2024-01-15">January 15, 2024</time> • 
                            <span>By <a href="/author/john-smith" style="color: #3498db;">John Smith</a></span> • 
                            <span><a href="/category/angular" style="color: #3498db;">Angular</a></span>
                        </div>
                    </header>
                    
                    <p style="line-height: 1.6; color: #555;">
                        Angular is a powerful framework for building modern web applications. In this comprehensive guide, 
                        we'll explore the core concepts, architecture, and best practices that will help you get started 
                        with Angular development...
                    </p>
                    
                    <footer style="margin-top: 20px;">
                        <a href="/posts/getting-started-with-angular" style="color: #3498db; text-decoration: none; font-weight: bold;">
                            Read More →
                        </a>
                    </footer>
                </div>
            </article>
            
            <!-- Regular Article 1 -->
            <article style="margin-bottom: 40px; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;">
                <div style="padding: 30px;">
                    <header>
                        <h2 style="margin: 0 0 15px; font-size: 1.5em;">
                            <a href="/posts/css-grid-vs-flexbox" style="color: #333; text-decoration: none;">
                                CSS Grid vs Flexbox: When to Use Which?
                            </a>
                        </h2>
                        <div style="color: #666; font-size: 0.9em; margin-bottom: 15px;">
                            <time datetime="2024-01-12">January 12, 2024</time> • 
                            <span>By <a href="/author/jane-doe" style="color: #3498db;">Jane Doe</a></span> • 
                            <span><a href="/category/css" style="color: #3498db;">CSS</a></span>
                        </div>
                    </header>
                    
                    <p style="line-height: 1.6; color: #555;">
                        Understanding when to use CSS Grid and when to use Flexbox is essential for modern web layout. 
                        Both have their strengths and knowing the differences will help you build better layouts...
                    </p>
                    
                    <footer style="margin-top: 20px;">
                        <a href="/posts/css-grid-vs-flexbox" style="color: #3498db; text-decoration: none; font-weight: bold;">
                            Read More →
                        </a>
                    </footer>
                </div>
            </article>
            
            <!-- Regular Article 2 -->
            <article style="margin-bottom: 40px; border: 1px solid #ddd; border-radius: 10px; overflow: hidden;">
                <div style="padding: 30px;">
                    <header>
                        <h2 style="margin: 0 0 15px; font-size: 1.5em;">
                            <a href="/posts/javascript-es6-features" style="color: #333; text-decoration: none;">
                                10 Essential ES6 Features Every Developer Should Know
                            </a>
                        </h2>
                        <div style="color: #666; font-size: 0.9em; margin-bottom: 15px;">
                            <time datetime="2024-01-10">January 10, 2024</time> • 
                            <span>By <a href="/author/john-smith" style="color: #3498db;">John Smith</a></span> • 
                            <span><a href="/category/javascript" style="color: #3498db;">JavaScript</a></span>
                        </div>
                    </header>
                    
                    <p style="line-height: 1.6; color: #555;">
                        ES6 (ECMAScript 2015) introduced many powerful features that have revolutionized JavaScript development. 
                        From arrow functions to destructuring, these features have become essential...
                    </p>
                    
                    <footer style="margin-top: 20px;">
                        <a href="/posts/javascript-es6-features" style="color: #3498db; text-decoration: none; font-weight: bold;">
                            Read More →
                        </a>
                    </footer>
                </div>
            </article>
        </div>
        
        <!-- Sidebar -->
        <aside role="complementary" style="padding: 20px; background: #f9f9f9; border-radius: 10px; height: fit-content;">
            
            <!-- About Author -->
            <section style="margin-bottom: 30px;">
                <h3 style="margin-top: 0; padding-bottom: 10px; border-bottom: 2px solid #3498db;">About Author</h3>
                <div style="text-align: center;">
                    <img src="author.jpg" alt="John Smith" style="width: 100px; height: 100px; border-radius: 50%; margin-bottom: 15px;">
                    <h4 style="margin: 0 0 10px;">John Smith</h4>
                    <p style="color: #666; font-size: 0.9em;">
                        Full-stack developer with 10+ years of experience. Passionate about web technologies and teaching.
                    </p>
                </div>
            </section>
            
            <!-- Categories -->
            <section style="margin-bottom: 30px;">
                <h3 style="margin-top: 0; padding-bottom: 10px; border-bottom: 2px solid #3498db;">Categories</h3>
                <ul style="list-style: none; padding: 0;">
                    <li style="margin-bottom: 10px;">
                        <a href="/category/html" style="color: #333; text-decoration: none;">HTML</a> 
                        <span style="color: #999;">(12)</span>
                    </li>
                    <li style="margin-bottom: 10px;">
                        <a href="/category/css" style="color: #333; text-decoration: none;">CSS</a> 
                        <span style="color: #999;">(8)</span>
                    </li>
                    <li style="margin-bottom: 10px;">
                        <a href="/category/javascript" style="color: #333; text-decoration: none;">JavaScript</a> 
                        <span style="color: #999;">(15)</span>
                    </li>
                    <li style="margin-bottom: 10px;">
                        <a href="/category/angular" style="color: #333; text-decoration: none;">Angular</a> 
                        <span style="color: #999;">(6)</span>
                    </li>
                    <li style="margin-bottom: 10px;">
                        <a href="/category/react" style="color: #333; text-decoration: none;">React</a> 
                        <span style="color: #999;">(9)</span>
                    </li>
                </ul>
            </section>
            
            <!-- Recent Posts -->
            <section style="margin-bottom: 30px;">
                <h3 style="margin-top: 0; padding-bottom: 10px; border-bottom: 2px solid #3498db;">Recent Posts</h3>
                <ul style="list-style: none; padding: 0;">
                    <li style="margin-bottom: 15px;">
                        <a href="/posts/typescript-tips" style="color: #333; text-decoration: none; font-weight: 500;">
                            TypeScript Tips for Better Code
                        </a>
                    </li>
                    <li style="margin-bottom: 15px;">
                        <a href="/posts/web-performance" style="color: #333; text-decoration: none; font-weight: 500;">
                            Web Performance Optimization
                        </a>
                    </li>
                    <li style="margin-bottom: 15px;">
                        <a href="/posts/accessibility-guide" style="color: #333; text-decoration: none; font-weight: 500;">
                            Complete Accessibility Guide
                        </a>
                    </li>
                </ul>
            </section>
            
            <!-- Tags -->
            <section>
                <h3 style="margin-top: 0; padding-bottom: 10px; border-bottom: 2px solid #3498db;">Tags</h3>
                <div style="display: flex; flex-wrap: wrap; gap: 8px;">
                    <a href="/tag/webdev" style="background: #e0e0e0; color: #333; padding: 5px 10px; border-radius: 3px; text-decoration: none; font-size: 0.85em;">webdev</a>
                    <a href="/tag/tutorial" style="background: #e0e0e0; color: #333; padding: 5px 10px; border-radius: 3px; text-decoration: none; font-size: 0.85em;">tutorial</a>
                    <a href="/tag/beginners" style="background: #e0e0e0; color: #333; padding: 5px 10px; border-radius: 3px; text-decoration: none; font-size: 0.85em;">beginners</a>
                    <a href="/tag/coding" style="background: #e0e0e0; color: #333; padding: 5px 10px; border-radius: 3px; text-decoration: none; font-size: 0.85em;">coding</a>
                    <a href="/tag/design" style="background: #e0e0e0; color: #333; padding: 5px 10px; border-radius: 3px; text-decoration: none; font-size: 0.85em;">design</a>
                </div>
            </section>
        </aside>
    </main>
    
    <!-- Footer -->
    <footer role="contentinfo" style="background: #2c3e50; color: white; padding: 40px 20px; margin-top: 60px;">
        <div style="max-width: 1200px; margin: 0 auto; display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 40px;">
            <!-- About -->
            <div>
                <h4 style="margin-top: 0;">TechBlog</h4>
                <p style="opacity: 0.8; font-size: 0.9em;">
                    Providing quality content about web development, programming, and technology since 2020.
                </p>
            </div>
            
            <!-- Quick Links -->
            <div>
                <h4 style="margin-top: 0;">Quick Links</h4>
                <nav style="display: flex; flex-direction: column; gap: 10px;">
                    <a href="/" style="color: white; text-decoration: none; opacity: 0.8;">Home</a>
                    <a href="/about" style="color: white; text-decoration: none; opacity: 0.8;">About</a>
                    <a href="/contact" style="color: white; text-decoration: none; opacity: 0.8;">Contact</a>
                    <a href="/privacy" style="color: white; text-decoration: none; opacity: 0.8;">Privacy Policy</a>
                </nav>
            </div>
            
            <!-- Social -->
            <div>
                <h4 style="margin-top: 0;">Follow Us</h4>
                <nav style="display: flex; gap: 15px;">
                    <a href="https://twitter.com" style="color: white; text-decoration: none;">Twitter</a>
                    <a href="https://facebook.com" style="color: white; text-decoration: none;">Facebook</a>
                    <a href="https://linkedin.com" style="color: white; text-decoration: none;">LinkedIn</a>
                    <a href="https://github.com" style="color: white; text-decoration: none;">GitHub</a>
                </nav>
            </div>
        </div>
        
        <div style="max-width: 1200px; margin: 40px auto 0; padding-top: 20px; border-top: 1px solid rgba(255,255,255,0.2); text-align: center; opacity: 0.8;">
            <p>&copy; 2024 TechBlog. All rights reserved.</p>
        </div>
    </footer>
</body>
</html>
```

---

## Project Checklist

### Semantic Structure (20 points)
- [ ] Header element for site header
- [ ] Nav element for navigation
- [ ] Main element for main content
- [ ] Article elements for blog posts
- [ ] Aside element for sidebar
- [ ] Footer element for site footer

### Blog Post Structure (15 points)
- [ ] Article with header (title, meta info)
- [ ] Article content
- [ ] Article footer (read more link)

### Sidebar Components (10 points)
- [ ] Author bio section
- [ ] Categories list
- [ ] Recent posts
- [ ] Tags

### Accessibility (10 points)
- [ ] Role attributes where needed
- [ ] Proper heading hierarchy
- [ ] Semantic element usage

### Meta & SEO (5 points)
- [ ] Meta description
- [ ] Proper title tag
- [ ] Author meta

---

## Grading Rubric

| Criteria | Excellent (A) | Good (B) | Needs Work (C) |
|----------|---------------|----------|----------------|
| Semantic Elements | All used correctly | Most correct | Limited use |
| Structure | Perfect blog layout | Good structure | Incomplete |
| Content | Complete articles | Adequate | Missing sections |
| Accessibility | Fully accessible | Partially | Not accessible |
| Code Quality | Clean, organized | Acceptable | Needs work |

---

## Key Semantic Elements Used

| Element | Purpose |
|---------|---------|
| `<header>` | Page or section header |
| `<nav>` | Navigation links |
| `<main>` | Main content area |
| `<article>` | Self-contained content |
| `<aside>` | Sidebar content |
| `<footer>` | Page or section footer |
| `<time>` | Date/time information |
| `<figure>` | Images with captions |
| `<section>` | Thematic grouping |

---

## Tips for Success

1. **Use semantic elements** - Not divs for everything
2. **Maintain heading hierarchy** - h1 > h2 > h3
3. **Include meta information** - Dates, authors, categories
4. **Make it accessible** - Proper labels and structure
5. **Validate your code** - Check for errors
