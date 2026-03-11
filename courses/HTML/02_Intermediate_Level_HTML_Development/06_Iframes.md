# Iframes

## Topic Title
Using Iframes in HTML

## Concept Explanation

### What is an Iframe?

An iframe (inline frame) is an HTML element that allows embedding another HTML page within the current page. It's like a window that shows another webpage inside your page.

### Basic Syntax

```html
<iframe src="page.html"></iframe>
```

### Iframe Attributes

- **src** - URL of the page to embed
- **width** - Width of the iframe
- **height** - Height of the iframe
- **title** - Accessibility title
- **frameborder** - Border around iframe (0 or 1)
- **allow** - Permissions for features
- **sandbox** - Security restrictions

## Why This Concept Is Important

Iframes matter because:

1. **Content embedding** - Embed external content
2. **Third-party content** - YouTube, maps, payments
3. **Isolation** - Sandboxed content doesn't affect parent
4. **Single page apps** - Used in modern applications
5. **Ad placement** - Common for advertising

## Code Examples

### Example 1: Basic Iframe

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Iframe Demo</title>
</head>
<body>
    <h1>Iframe Examples</h1>
    
    <!-- Basic iframe -->
    <iframe src="https://en.wikipedia.org/wiki/HTML"></iframe>
</body>
</html>
```

### Example 2: Iframe with Attributes

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Iframe with Attributes</title>
</head>
<body>
    <h1>YouTube Video Embed</h1>
    
    <iframe 
        width="560" 
        height="315" 
        src="https://www.youtube.com/embed/dQw4w9WgXcQ"
        title="YouTube video player"
        frameborder="0"
        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
        allowfullscreen>
    </iframe>
    
    <h1>Google Maps Embed</h1>
    
    <iframe 
        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3022.1422937950147!2d-73.98731968482413!3d40.75889497932681!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x89c25855c6480299%3A0x55194ec5a1ae072e!2sTimes%20Square!5e0!3m2!1sen!2sus!4v1633000000000!5m2!1sen!2sus"
        width="600" 
        height="450" 
        style="border:0;" 
        allowfullscreen="" 
        loading="lazy">
    </iframe>
</body>
</html>
```

### Example 3: Responsive Iframe

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Responsive Iframe</title>
    <style>
        .video-container {
            position: relative;
            width: 100%;
            padding-bottom: 56.25%; /* 16:9 aspect ratio */
        }
        .video-container iframe {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        }
    </style>
</head>
<body>
    <h1>Responsive YouTube Embed</h1>
    
    <div class="video-container">
        <iframe 
            src="https://www.youtube.com/embed/dQw4w9WgXcQ"
            title="YouTube video"
            frameborder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
            allowfullscreen>
        </iframe>
    </div>
</body>
</html>
```

### Example 4: Sandboxed Iframe

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Sandboxed Iframe</title>
</head>
<body>
    <h1>Sandboxed Iframe Examples</h1>
    
    <!-- Allow scripts but not forms -->
    <iframe src="external-page.html" sandbox="allow-scripts allow-same-origin"></iframe>
    
    <!-- No restrictions removed (most secure) -->
    <iframe src="external-page.html" sandbox></iframe>
    
    <!-- Allow specific features -->
    <iframe src="external-page.html" 
            sandbox="allow-forms allow-scripts allow-top-navigation"></iframe>
</body>
</html>
```

## Best Practices

### Security Best Practices

1. **Use sandbox attribute** - Limit iframe capabilities
2. **Use HTTPS** - Secure content delivery
3. **Validate sources** - Only embed trusted content
4. **Consider X-Frame-Options** - Control embedding of your site

### Accessibility Best Practices

1. **Always add title** - Screen readers need context
2. **Provide fallback** - For unsupported browsers

## Real-World Examples

### Example 1: YouTube Embed

```html
<iframe 
    width="560" 
    height="315" 
    src="https://www.youtube.com/embed/VIDEO_ID"
    title="YouTube video player"
    frameborder="0"
    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
    allowfullscreen>
</iframe>
```

### Example 2: Payment Integration

```html
<iframe 
    src="/payment-widget"
    width="400" 
    height="500"
    title="Secure Payment Form"
    frameborder="0">
</iframe>
```

## Common Mistakes

### Mistake 1: No Title

```html
<!-- Wrong -->
<iframe src="page.html"></iframe>

<!-- Correct -->
<iframe src="page.html" title="Description"></iframe>
```

### Mistake 2: Using HTTP for HTTPS Site

```html
<!-- Wrong -->
<iframe src="http://example.com">

<!-- Correct -->
<iframe src="https://example.com">
```

## Exercises

### Exercise 1: Embed YouTube
Embed a YouTube video using iframe.

### Exercise 2: Embed Map
Embed a Google Map location.

### Exercise 3: Responsive Iframe
Make an iframe responsive.
