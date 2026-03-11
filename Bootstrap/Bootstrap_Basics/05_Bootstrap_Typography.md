# Bootstrap Typography

## Definition

Bootstrap typography classes allow you to style text content quickly without writing custom CSS. Bootstrap provides special heading classes (display headings), paragraph styling (lead text), text alignment options, and font weight utilities. These classes help create visually appealing text that follows Bootstrap's design conventions.

## Key Bullet Points

- **Display Headings**: display-1 through display-6 are larger, lighter headings for impact
- **.lead Class**: Makes paragraphs stand out with larger font size
- **Text Alignment**: text-start, text-center, text-end for aligning text
- **Font Weight**: fw-bold for bold, fw-normal for normal, fw-light for light text
- **Font Style**: fst-italic for italic text
- **Text Transformation**: text-uppercase, text-lowercase, text-capitalize

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Typography</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Typography Showcase</h2>

    <!-- DISPLAY HEADINGS: Larger, more prominent headings -->
    <h5>Display Headings</h5>
    <p class="mb-1"><strong>display-1:</strong> <span class="display-1">Display Heading 1</span></p>
    <p class="mb-1"><strong>display-2:</strong> <span class="display-2">Display Heading 2</span></p>
    <p class="mb-1"><strong>display-3:</strong> <span class="display-3">Display Heading 3</span></p>
    <p class="mb-1"><strong>display-4:</strong> <span class="display-4">Display Heading 4</span></p>
    <p class="mb-1"><strong>display-5:</strong> <span class="display-5">Display Heading 5</span></p>
    <p class="mb-4"><strong>display-6:</strong> <span class="display-6">Display Heading 6</span></p>

    <!-- REGULAR HEADINGS -->
    <h5>Regular Headings</h5>
    <h1>Heading 1</h1>
    <h2>Heading 2</h2>
    <h3>Heading 3</h3>
    <h4>Heading 4</h4>
    <h5>Heading 5</h5>
    <h6>Heading 6</h6>

    <hr>

    <!-- LEAD PARAGRAPH -->
    <h5>Lead Paragraph (.lead)</h5>
    <p class="lead">
        This is a lead paragraph. It stands out from regular paragraphs with larger font size, font weight, and line height. Perfect for introductory text!
    </p>
    <p>
        This is a regular paragraph. It has normal font size and weight for body text.
    </p>

    <hr>

    <!-- TEXT ALIGNMENT -->
    <h5>Text Alignment</h5>
    <p class="text-start mb-2"><strong>.text-start:</strong> Left-aligned text (default)</p>
    <p class="text-center mb-2"><strong>.text-center:</strong> Center-aligned text</p>
    <p class="text-end mb-2"><strong>.text-end:</strong> Right-aligned text</p>

    <hr>

    <!-- FONT WEIGHT & STYLE -->
    <h5>Font Weight & Style</h5>
    <p class="fw-bold mb-2">Bold text (.fw-bold)</p>
    <p class="fw-normal mb-2">Normal weight text (.fw-normal)</p>
    <p class="fw-light mb-2">Light weight text (.fw-light)</p>
    <p class="fst-italic mb-2">Italic text (.fst-italic)</p>

    <hr>

    <!-- TEXT TRANSFORMATION -->
    <h5>Text Transformation</h5>
    <p class="text-uppercase mb-2">uppercase text (.text-uppercase)</p>
    <p class="text-lowercase mb-2">LOWERCASE TEXT (.text-lowercase)</p>
    <p class="text-capitalize mb-2">capitalized text (.text-capitalize)</p>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.display-1` through `.display-6`**: Extra large heading styles, display-1 being the largest, typically used for hero sections or prominent headlines
- **`.lead`**: A special class for introductory paragraphs that makes text larger and slightly bolder
- **`.text-start`**: Aligns text to the left (default)
- **`.text-center`**: Centers text horizontally
- **`.text-end`**: Aligns text to the right
- **`.fw-bold`**: Sets font-weight to bold (700)
- **`.fw-normal`**: Sets font-weight to normal (400)
- **`.fw-light`**: Sets font-weight to light (300)
- **`.fst-italic`**: Sets font-style to italic
- **`.text-uppercase`**: Transforms text to all uppercase letters
- **`.text-lowercase`**: Transforms text to all lowercase letters
- **`.text-capitalize`**: Capitalizes the first letter of each word

## Expected Visual Result

The page displays multiple typography examples:

1. **Display Headings**: Six progressively smaller headings, with display-1 being very large (about 5rem) and display-6 being similar to regular h1
2. **Regular Headings**: Standard h1-h6 headings showing the default Bootstrap heading hierarchy
3. **Lead Paragraph**: A paragraph with larger, bolder text that clearly stands out from regular paragraphs below it
4. **Text Alignment**: Three lines showing left, center, and right alignment
5. **Font Weights**: Demonstrates bold, normal, and light font weights
6. **Text Transformations**: Shows uppercase, lowercase, and capitalized text variations
