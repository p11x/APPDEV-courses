# CSS Pseudo Elements

## Definition

CSS pseudo elements let you style specific parts of an element without adding extra HTML. They start with double colons (::) and allow you to insert content before or after an element's content, style the first letter or first line of text, or style selections. They're powerful tools for adding decorative effects without cluttering your HTML.

## Key Points

- ::before inserts content before an element's content
- ::after inserts content after an element's content
- ::first-line styles the first line of text
- ::first-letter styles the first letter of text
- ::selection styles highlighted/selected text
- Pseudo elements create "virtual" elements that don't exist in HTML
- Content property is often used with ::before and ::after

## Code Example

<file content>
<!DOCTYPE html>
<html>
<head>
    <title>CSS Pseudo Elements</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            padding: 20px;
            background-color: #f5f5f5;
        }
        
        h1 {
            text-align: center;
            color: #333;
        }
        
        h2 {
            color: #555;
            margin-top: 40px;
        }
        
        .demo-section {
            background-color: white;
            padding: 20px;
            margin: 20px 0;
            border: 1px solid #ddd;
        }
        
        /* ::before - insert content before */
        .before-example {
            position: relative;
            padding-left: 30px;
        }
        
        .before-example::before {
            content: "★ ";
            color: #f39c12;
            font-size: 20px;
            position: absolute;
            left: 0;
        }
        
        /* ::after - insert content after */
        .after-example {
            position: relative;
            padding-right: 50px;
        }
        
        .after-example::after {
            content: " (New!)";
            color: #e74c3c;
            font-size: 12px;
        }
        
        /* ::first-letter - style first letter */
        .first-letter::first-letter {
            font-size: 3em;
            float: left;
            line-height: 1;
            color: #3498db;
            margin-right: 5px;
        }
        
        /* ::first-line - style first line */
        .first-line::first-line {
            color: #e74c3c;
            font-weight: bold;
        }
        
        /* ::selection - style selected text */
        .selection-example::selection {
            background-color: #3498db;
            color: white;
        }
        
        /* Decorative ::before and ::after */
        .card {
            position: relative;
            background-color: white;
            border: 2px solid #3498db;
            padding: 30px;
            margin: 20px 0;
            max-width: 300px;
        }
        
        .card::before {
            content: "";
            position: absolute;
            top: -5px;
            left: -5px;
            right: -5px;
            bottom: -5px;
            border: 2px solid #3498db;
            z-index: -1;
        }
        
        .card::after {
            content: "";
            position: absolute;
            top: 10px;
            right: 10px;
            width: 20px;
            height: 20px;
            background-color: #3498db;
            border-radius: 50%;
        }
        
        /* Icon button example */
        .icon-btn {
            position: relative;
            display: inline-block;
            padding: 10px 20px 10px 40px;
            background-color: #2ecc71;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }
        
        .icon-btn::before {
            content: "✓";
            position: absolute;
            left: 15px;
            font-weight: bold;
        }
        
        .icon-btn:hover {
            background-color: #27ae60;
        }
    </style>
</head>
<body>
    <h1>CSS Pseudo Elements</h1>
    
    <h2>1. ::before</h2>
    <div class="demo-section">
        <p class="before-example">This paragraph has a star icon before it using ::before</p>
    </div>
    
    <h2>2. ::after</h2>
    <div class="demo-section">
        <p class="after-example">This text has "New!" added after it</p>
    </div>
    
    <h2>3. ::first-letter</h2>
    <div class="demo-section">
        <p class="first-letter">This paragraph has a large, blue first letter. This is created using the ::first-letter pseudo element. It's a classic design technique used in newspapers and books.</p>
    </div>
    
    <h2>4. ::first-line</h2>
    <div class="demo-section">
        <p class="first-line">This is the first line of text. It should be bold and red. This is the second line of text, which should look normal because only the first line is styled.</p>
    </div>
    
    <h2>5. ::selection</h2>
    <div class="demo-section">
        <p class="selection-example">Highlight some of this text with your mouse to see the selection styling!</p>
    </div>
    
    <h2>6. Decorative Card</h2>
    <div class="demo-section">
        <div class="card">
            <h3>Styled Card</h3>
            <p>This card has decorative borders created with ::before and ::after pseudo elements.</p>
        </div>
    </div>
    
    <h2>7. Icon Button</h2>
    <div class="demo-section">
        <a href="#" class="icon-btn">Submit</a>
    </div>
</body>
</html>
```

## Explanation

### ::before

```css
.element::before {
    content: "★ ";
    /* styling */
}
```
- Inserts content before the element's actual content
- Often used for icons, quotes, or decorative elements
- Must include content property

### ::after

```css
.element::after {
    content: " (New!)";
    /* styling */
}
```
- Inserts content after the element's actual content
- Commonly used for badges, labels, or clearing floats

### ::first-letter

```css
.element::first-letter {
    font-size: 3em;
    float: left;
}
```
- Styles the first letter of text
- Creates drop cap effects
- Only works on block-level elements

### ::first-line

```css
.element::first-line {
    color: red;
    font-weight: bold;
}
```
- Styles the first line of text
- Adapts if viewport changes

### ::selection

```css
.element::selection {
    background-color: blue;
    color: white;
}
```
- Styles highlighted/selected text
- Great for custom selection colors

## Visual Result

- ::before adds a star icon before the text
- ::after adds "(New!)" after the text
- ::first-letter creates a large drop cap effect
- ::first-line styles only the first line
- ::selection shows blue background when text is highlighted
- Decorative card shows extra borders and a dot
- Icon button has a checkmark icon added via pseudo element

Pseudo elements are incredibly useful for adding visual effects without modifying HTML.