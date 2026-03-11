# CSS Syntax

## Definition

CSS syntax refers to the rules you must follow when writing CSS code. A CSS rule consists of a selector that identifies which HTML element to style, followed by a declaration block containing property-value pairs that define what the styles should be. Think of it like a sentence where you specify "what element" (selector) and "how to style it" (declarations).

## Key Points

- A CSS rule has two main parts: the selector and the declaration block
- The selector comes before the curly braces and identifies the HTML element
- Inside the curly braces, you write declarations in property: value; format
- Each declaration must end with a semicolon
- You can write multiple declarations in one rule
- The whole structure is called a "rule set"
- White space (spaces, tabs, line breaks) doesn't affect how CSS works

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Syntax Example</title>
    <style>
        /* This is a complete CSS rule set */
        p {
            color: blue;
            font-size: 16px;
        }
        
        /* Another example with multiple properties */
        h1 {
            color: darkred;
            font-size: 32px;
            text-align: center;
            font-family: Arial, sans-serif;
        }
        
        /* Class selector example - we'll learn about this later */
        .highlight {
            background-color: yellow;
            padding: 10px;
        }
        
        /* Multiple selectors can be grouped together */
        h1, h2, h3 {
            font-weight: bold;
        }
    </style>
</head>
<body>
    <h1>CSS Syntax Lesson</h1>
    <p>This paragraph is styled with CSS. The selector "p" targets all paragraph elements.</p>
    <p class="highlight">This paragraph has a yellow background because it uses the "highlight" class.</p>
    <h2>Understanding Selectors</h2>
    <h3>This is an h3 heading</h3>
</body>
</html>
```

## Explanation

Let's break down the CSS syntax using the example `p { color: blue; font-size: 16px; }`:

- **p** - This is the selector. It tells CSS which HTML element to target. In this case, it targets all `<p>` (paragraph) elements in the HTML.

- **{ }** - These curly braces mark the beginning and end of the declaration block. Everything between them contains the styles to apply.

- **color: blue;** - This is a declaration (also called a property-value pair):
  - `color` is the property (what you want to change)
  - `blue` is the value (what you want to change it to)
  - The colon (:) separates property from value
  - The semicolon (;) ends the declaration

- **font-size: 16px;** - This is another declaration:
  - `font-size` controls how big the text appears
  - `16px` means 16 pixels (pixels are tiny dots on your screen)

For the h1 example:
- **color: darkred;** - Sets text color to dark red
- **font-size: 32px;** - Makes text 32 pixels tall
- **text-align: center;** - Centers the text horizontally
- **font-family: Arial, sans-serif;** - Sets the font to Arial, with fallback to any sans-serif font

For the class selector:
- **.highlight** - The dot (.) indicates this is a class selector
- Classes are used to select elements with a specific class attribute

For grouped selectors:
- **h1, h2, h3** - Multiple selectors separated by commas apply the same styles to all of them

## Visual Result

When you view this webpage:
- The main heading "CSS Syntax Lesson" appears as large, dark red, centered text using Arial font
- Regular paragraphs appear as blue text at 16 pixels size
- The paragraph with class "highlight" has a yellow background with some padding around it
- The h2 and h3 headings also appear bold because they inherited the grouped selector styles

The visual hierarchy makes it clear which content is most important (h1), moderately important (h2), and less important (paragraphs), demonstrating how CSS syntax controls the presentation of different elements.