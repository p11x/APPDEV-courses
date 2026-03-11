# HTML Performance Best Practices

## Topic Title
Optimizing HTML for Performance

## Concept Explanation

### Why Performance Matters

Web performance affects user experience, conversion rates, and search engine rankings. Fast sites keep users engaged.

### Performance Factors

1. **Page load time** - Time to first render
2. **Render blocking** - CSS and JS blocking display
3. **Image optimization** - Large images slow loading
4. **Lazy loading** - Load content as needed
5. **Minification** - Remove unnecessary characters

## Code Examples

### Example 1: Lazy Loading Images

```html
<!-- Browser-native lazy loading -->
<img src="image.jpg" loading="lazy" alt="Description">

<!-- Also for iframes -->
<iframe src="content.html" loading="lazy"></iframe>
```

### Example 2: Async and Defer Scripts

```html
<!-- Parse HTML, then load script -->
<script src="script.js" defer></script>

<!-- Load script without blocking, execute when available -->
<script src="script.js" async></script>

<!-- No attribute - blocks HTML parsing -->
<script src="script.js"></script>
```

### Example 3: Preload Resources

```html
<!-- Preload critical resources -->
<link rel="preload" href="style.css" as="style">
<link rel="preload" href="font.woff2" as="font" crossorigin>

<!-- Prefetch for next page -->
<link rel="prefetch" href="next-page.html">
```

### Example 4: Optimized Images

```html
<!-- Responsive images -->
<img src="small.jpg"
     srcset="small.jpg 500w,
             medium.jpg 1000w,
             large.jpg 2000w"
     sizes="(max-width: 600px) 100vw,
            (max-width: 1200px) 50vw,
            33vw"
     alt="Description">

<!-- Modern format -->
<picture>
    <source srcset="image.webp" type="image/webp">
    <source srcset="image.avif" type="image/avif">
    <img src="image.jpg" alt="Description">
</picture>
```

## Best Practices

1. **Use lazy loading** - For images below the fold
2. **Minify HTML** - Remove whitespace in production
3. **Use semantic HTML** - Faster parsing
4. **Optimize images** - Compress and use modern formats
5. **Defer non-critical JS** - Don't block rendering
