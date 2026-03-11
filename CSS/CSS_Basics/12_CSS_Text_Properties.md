# CSS Text Properties

## Definition

CSS text properties control how text appears on your webpage. You can change alignment (left, center, right), add decorations (underlines, strikethroughs), transform text (uppercase, lowercase, capitalized), and adjust line spacing. These properties are essential for making text readable and visually appealing.

## Key Points

- text-align controls horizontal alignment (left, center, right, justify)
- text-decoration adds lines above, through, or below text
- text-transform changes text case (uppercase, lowercase, capitalize)
- line-height controls the space between lines of text
- text-indent adds space before the first line of text
- letter-spacing adjusts space between characters
- word-spacing adjusts space between words

## Code Example

```html
<!DOCTYPE html>
<html>
<head>
    <title>CSS Text Properties</title>
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
        
        .text-box {
            background-color: white;
            padding: 20px;
            margin: 10px 0;
            border: 1px solid #ddd;
        }
        
        /* Text Alignment */
        .align-left { text-align: left; }
        .align-center { text-align: center; }
        .align-right { text-align: right; }
        .align-justify { text-align: justify; }
        
        /* Text Decoration */
        .decoration-none { text-decoration: none; }
        .decoration-underline { text-decoration: underline; }
        .decoration-overline { text-decoration: overline; }
        .decoration-line { text-decoration: line-through; }
        
        /* Text Transform */
        .transform-uppercase { text-transform: uppercase; }
        .transform-lowercase { text-transform: lowercase; }
        .transform-capitalize { text-transform: capitalize; }
        
        /* Line Height */
        .line-height-normal { line-height: normal; }
        .line-height-15 { line-height: 1.5; }
        .line-height-2 { line-height: 2; }
        .line-height-3 { line-height: 3; }
        
        /* Letter Spacing */
        .letter-spacing-normal { letter-spacing: normal; }
        .letter-spacing-wide { letter-spacing: 2px; }
        .letter-spacing-tight { letter-spacing: -1px; }
        
        /* Text Indent */
        .text-indent { text-indent: 30px; }
        
        /* Link styling */
        .link {
            color: #3498db;
            text-decoration: none;
        }
        
        .link:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <h1>CSS Text Properties</h1>
    
    <h2>1. Text Align</h2>
    <div class="text-box align-left">This text is aligned to the left (default).</div>
    <div class="text-box align-center">This text is centered.</div>
    <div class="text-box align-right">This text is aligned to the right.</div>
    <div class="text-box align-justify">This text is justified. Justified text creates equal spacing between words to align to both left and right edges. This is common in newspapers and books. The lines stretch to fill the full width.</div>
    
    <h2>2. Text Decoration</h2>
    <div class="text-box decoration-none"><a href="#" class="link">This is a link without underline</a></div>
    <div class="text-box decoration-underline">This text has an underline.</div>
    <div class="text-box decoration-overline">This text has an overline.</div>
    <div class="text-box decoration-line">This text has a strikethrough line.</div>
    
    <h2>3. Text Transform</h2>
    <div class="text-box transform-uppercase">this text is converted to uppercase</div>
    <div class="text-box transform-lowercase">THIS TEXT IS CONVERTED TO LOWERCASE</div>
    <div class="text-box transform-capitalize">this text is capitalized</div>
    
    <h2>4. Line Height</h2>
    <div class="text-box line-height-normal">
        Line height normal: This is a paragraph with normal line height. The spacing between lines is the default. This makes text harder to read when lines are too close together.
    </div>
    <div class="text-box line-height-15">
        Line height 1.5: This is a paragraph with line height of 1.5. More spacing between lines makes text easier to read. This is a good default for body text.
    </div>
    <div class="text-box line-height-2">
        Line height 2: This paragraph has double spacing. This is useful for readability in long-form content or when you want more white space.
    </div>
    
    <h2>5. Letter Spacing</h2>
    <div class="text-box letter-spacing-normal">Normal letter spacing</div>
    <div class="text-box letter-spacing-wide">Wide letter spacing</div>
    <div class="text-box letter-spacing-tight">Tight letter spacing</div>
    
    <h2>6. Text Indent</h2>
    <div class="text-box text-indent">
        This is the first line with a text indent. The first line is indented by 30 pixels, while the rest of the text continues normally. This style is traditionally used in books and academic papers.
    </div>
</body>
</html>
```

## Explanation

### Text Align

- **text-align: left;** - Aligns text to the left (default for most languages)
- **text-align: center;** - Centers text
- **text-align: right;** - Aligns text to the right (common in some languages)
- **text-align: justify;** - Stretches lines to fill both left and right edges

### Text Decoration

- **text-decoration: none;** - Removes decoration (used for links)
- **text-decoration: underline;** - Adds underline
- **text-decoration: overline;** - Adds line above text
- **text-decoration: line-through;** - Adds strikethrough (for deleted content)

### Text Transform

- **text-transform: uppercase;** - Makes all letters capitals
- **text-transform: lowercase;** - Makes all letters small
- **text-transform: capitalize;** - Capitalizes first letter of each word

### Line Height

- **line-height: 1.5;** - Recommended for readability
- **line-height: 2;** - Double spacing
- Values can be numbers (multipliers), pixels, or percentages

### Letter Spacing

- **letter-spacing: 2px;** - Adds 2px between each letter
- Can be negative for tight spacing

### Text Indent

- **text-indent: 30px;** - Indents first line by 30 pixels
- Traditional book style

## Visual Result

- Each text alignment shows different positioning
- Text decoration examples show underline, overline, and strikethrough
- Text transform demonstrates case changes
- Line height examples clearly show spacing differences
- Letter spacing examples show character separation
- Text indent demonstrates traditional paragraph formatting

Text properties are essential for creating readable, professional-looking content.