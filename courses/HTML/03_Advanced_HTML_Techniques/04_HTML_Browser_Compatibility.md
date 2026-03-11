# HTML Browser Compatibility

## Topic Title
Cross-Browser HTML Compatibility

## Concept Explanation

### What is Browser Compatibility?

Browser compatibility ensures websites work consistently across different browsers (Chrome, Firefox, Safari, Edge) and their versions.

### Key Considerations

1. **Feature support** - Not all features work everywhere
2. **Vendor prefixes** - Browser-specific prefixes
3. **Polyfills** - JavaScript solutions for missing features
4. **Graceful degradation** - Work without certain features
5. **Progressive enhancement** - Build up from basic support

## Code Examples

### Example 1: Vendor Prefixes

```css
/* With vendor prefixes */
.element {
    -webkit-border-radius: 10px;  /* Safari, Chrome */
    -moz-border-radius: 10px;    /* Firefox */
    -ms-border-radius: 10px;      /* Edge */
    border-radius: 10px;
}

/* Modern CSS features */
.feature {
    display: grid;
    display: -webkit-grid;
    display: -ms-grid;
}
```

### Example 2: Feature Detection

```html
<!-- Check for feature support -->
<script>
    if ('geolocation' in navigator) {
        // Use geolocation
    } else {
        // Show alternative
    }
    
    if (HTMLTemplateElement) {
        // Use template element
    } else {
        // Use polyfill
    }
</script>
```

### Example 3: Fallback Content

```html
<!-- Video fallback -->
<video controls>
    <source src="video.mp4" type="video/mp4">
    <source src="video.webm" type="video/webm">
    <!-- Fallback for older browsers -->
    <object data="video.swf" type="application/x-shockwave-flash">
        <param name="movie" value="video.swf">
    </object>
    <!-- Text fallback -->
    <p>Your browser doesn't support video.</p>
</video>
```

## Best Practices

1. **Test in multiple browsers** - Chrome, Firefox, Safari, Edge
2. **Use caniuse.com** - Check feature support
3. **Provide fallbacks** - For unsupported features
4. **Keep browsers updated** - Encourage users
5. **Use normalize.css** - Consistent base styles
