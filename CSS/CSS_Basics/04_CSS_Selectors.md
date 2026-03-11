# CSS Selectors

## Definition

CSS selectors are patterns used to select and target specific HTML elements that you want to style. Think of selectors as addresses that tell CSS exactly which elements to apply styles to. Just like how you'd address a letter to a specific person, CSS selectors "address" specific elements in your HTML to apply styles.

## Key Points

- Element selectors target all HTML elements of a specific type (like all paragraphs)
- Class selectors target elements with a specific class attribute (use a dot . before the name)
- ID selectors target one unique element with a specific ID (use a hash # before the name)
- The universal selector (*) targets all elements on the page
- Group selectors let you apply the same styles to multiple different elements at once
- ID selectors should be unique on a page, classes can be reused on many elements
- Selectors can be combined (like targeting paragraphs inside a specific class)

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Selectors</title>
    <style>
        /* Element Selector - targets all paragraphs */
        p {
            color: #333;
            line-height: 1.6;
        }
        
        /* Class Selector - targets elements with class="highlight" */
        .highlight {
            background-color: yellow;
            padding: 10px;
        }
        
        /* ID Selector - targets the element with id="main-title" */
        #main-title {
            color: darkblue;
            font-size: 32px;
            text-align: center;
        }
        
        /* Universal Selector - applies to everything */
        * {
            box-sizing: border-box;
        }
        
        /* Group Selector - applies same styles to multiple elements */
        h1, h2, h3 {
            font-family: Arial, sans-serif;
            color: #2c3e50;
        }
        
        /* Combined Selector - targets only paragraphs inside .container */
        .container p {
            font-size: 18px;
        }
        
        /* Another class example */
        .important {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body>
    <!-- ID example - using id="main-title" -->
    <h1 id="main-title">CSS Selectors Lesson</h1>
    
    <!-- Regular paragraph -->
    <p>This paragraph uses the element selector "p" to style all paragraphs.</p>
    
    <!-- Class example -->
    <p class="highlight">This paragraph has the "highlight" class applied.</p>
    
    <div class="container">
        <h2>Inside Container</h2>
        <!-- This paragraph is affected by the combined selector -->
        <p>This paragraph is inside a div with class="container", so it's styled differently.</p>
    </div>
    
    <!-- Important class -->
    <p class="important">This paragraph has the "important" class making it bold and red!</p>
    
    <h2>Group Selectors</h2>
    <h3>This h3 heading</h3>
</body>
</html>
```

## Explanation

### Element Selector: `p { }`

- Targets all `<p>` (paragraph) elements on the page
- Any style inside these braces applies to every paragraph

### Class Selector: `.highlight { }`

- The dot (.) indicates a class selector
- Targets any element with `class="highlight"` in its HTML
- Classes can be reused on many elements
- Multiple classes can be applied: `class="highlight important"`

### ID Selector: `#main-title { }`

- The hash (#) indicates an ID selector
- Targets one unique element with `id="main-title"`
- Should only be used once per page (IDs must be unique)
- More specific than class selectors

### Universal Selector: `* { }`

- The asterisk (*) targets ALL elements
- Commonly used to set `box-sizing: border-box` for easier sizing calculations

### Group Selector: `h1, h2, h3 { }`

- Multiple selectors separated by commas
- Applies the same styles to all listed elements
- Reduces code repetition

### Combined Selector: `.container p { }`

- Targets paragraphs that are inside elements with class="container"
- The space between means "inside"
- This is called a "descendant selector"

## Visual Result

- The main title appears as large, dark blue, centered text
- Regular paragraphs have dark gray text with comfortable line spacing
- The paragraph with class="highlight" has a yellow background with padding
- The h2 and h3 headings share the same font and color (from group selector)
- The paragraph inside .container is larger (18px) because of the combined selector
- The paragraph with class="important" appears bold and red
- Everything fits properly within the viewport due to box-sizing

The visual difference clearly shows how different selectors target different elements for precise styling control.