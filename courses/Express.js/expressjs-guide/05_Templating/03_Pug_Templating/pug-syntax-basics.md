# Pug (formerly Jade) Syntax Basics

## 📌 What You'll Learn
- The basic syntax of Pug templating engine
- How Pug differs from HTML (indentation-based syntax)
- How to output data and use conditionals in Pug
- How to use loops and mixins in Pug templates

## 🧠 Concept Explained (Plain English)

Pug (formerly known as Jade) is a templating engine that takes a radically different approach to writing HTML. Instead of using angle brackets and closing tags like traditional HTML, Pug uses indentation (whitespace) to define structure. Think of it like Python for HTML - the indentation tells Pug what elements are nested inside others.

The name "Pug" comes from the dog breed, chosen as a playful reference to how the syntax can feel "squished" and compact compared to traditional HTML.

The advantages of Pug include:
- Less verbose - you write less code to achieve the same result
- Cleaner to read - indentation makes the structure visually obvious
- Built-in features like mixins (reusable components) and includes
- Automatic HTML escaping for security

The main challenge is getting used to the indentation-based syntax, which is different from anything you've seen in traditional web development.

## 💻 Code Example

```javascript
// ES Module - Pug Syntax Examples

import express from 'express';

const app = express();

// Set up Pug as our view engine
app.set('view engine', 'pug');
// Optionally set the views directory (default is 'views')
app.set('views', './views');

// ========================================
// ROUTE FOR DEMONSTRATING PUG SYNTAX
// ========================================
app.get('/demo', (req, res) => {
    // Sample data to pass to our template
    const demoData = {
        title: 'Pug Demo Page',
        user: {
            name: 'Alice Smith',
            isPremium: true,
            points: 150
        },
        items: ['Apple', 'Banana', 'Cherry'],
        showList: true,
        htmlContent: '<strong>This is bold text</strong>',
        currentYear: new Date().getFullYear()
    };
    
    res.render('demo', demoData);
});

// ========================================
// WHAT THE PUG TEMPLATE (views/demo.pug) LOOKS LIKE
// ========================================
/*
// views/demo.pug

// Basic HTML structure with Pug syntax
doctype html
html
    head
        meta(charset='UTF-8')
        meta(name='viewport', content='width=device-width, initial-scale=1.0')
        title= title  // Shorthand for setting title
        
        style.
            body { font-family: Arial, sans-serif; margin: 40px; }
            .premium { background-color: #fff8dc; padding: 10px; border-radius: 5px; }
            .item { padding: 5px; border-bottom: 1px solid #eee; }
    
    body
        h1= title
        
        // ========================================
        // OUTPUTTING VARIABLES
        // ========================================
        h2 User Information
        p Name: #{user.name}
        p Points: #{user.points}
        
        // ========================================
        // CONDITIONALS (IF STATEMENTS)
        // ========================================
        if user.isPremium
            .premium
                p Welcome back, premium member!
                p You have #{user.points} loyalty points.
        else
            p Upgrade to premium for special benefits!
        
        // ========================================
        // UNLESS (NEGATIVE CONDITIONAL)
        // ========================================
        unless user.isPremium
            p Consider upgrading to premium!
        
        // ========================================
        // LOOPS (EACH)
        // ========================================
        if showList
            h2 Shopping List
            each item in items
                .item
                    strong= item
        
        // ========================================
        // LOOP WITH INDEX
        // ========================================
        h2 Items with Index
        each item, index in items
            li #{index + 1}. #{item}
        
        // ========================================
        // FOR LOOP ALTERNATIVE
        // ========================================
        h2 Items (for loop)
        - for (let i = 0; i < items.length; i++)
            p Item #{i + 1}: #{items[i]}
        
        // ========================================
        // OUTPUTTING HTML (ESCAPED VS UNESCAPED)
        // ========================================
        h2 HTML Content
        p Escaped (safe): #{htmlContent}
        //- Unescaped uses != instead of =
        p Unescaped (raw): !{htmlContent}
        
        // ========================================
        // CASE STATEMENT
        // ========================================
        h2 Membership Status
        case user.points
            when 0
                p New member
            when 1
                p Bronze member
            when 100
                p Silver member
            when 150
                p Gold member
            default
                p Member
        
        // ========================================
        // MIXINS (REUSABLE COMPONENTS)
        // ========================================
        mixin userCard(userName, isPremium)
            .user-card(class=isPremium ? 'premium' : '')
                h3= userName
                if isPremium
                    span.badge Premium
        
        +userCard(user.name, user.isPremium)
        
        // ========================================
        // COMMENTS
        // ========================================
        // This comment will appear in the HTML
        //- This comment is server-side only (won't appear)
        
        // ========================================
        // INCLUDES
        // ========================================
        // Include another Pug file
        //- include ./partials/footer
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT`));
```

## 🔍 Line-by-Line Breakdown (Template)

| Line | Code | What It Does |
|------|------|--------------|
| 1 | `doctype html` | Declares the document type as HTML5 |
| 2-3 | `html` / `head` | Creates `<html>` and `<head>` elements |
| 4 | `meta(charset='UTF-8')` | Creates `<meta charset="UTF-8">` |
| 6 | `title= title` | Sets the title to the value of the `title` variable |
| 9 | `style.` | Begins a block of plain CSS text |
| 12 | `body` | Creates the `<body>` element |
| 13 | `h1= title` | Creates `<h1>` with the title variable |
| 17 | `p Name: #{user.name}` | Interpolates variable into text |
| 21 | `if user.isPremium` | Begins a conditional block |
| 28 | `else` | Alternative branch |
| 32 | `unless user.isPremium` | Negative conditional (equivalent to `if !user.isPremium`) |
| 37 | `each item in items` | Loops through an array |
| 43 | `each item, index in items` | Loops with index variable |
| 51 | `case user.points` | Switch/case statement |
| 68 | `mixin userCard(userName, isPremium)` | Defines a reusable component |
| 73 | `+userCard(user.name, user.isPremium)` | Calls a mixin |

## ⚠️ Common Mistakes

**1. Inconsistent indentation**
Pug uses indentation to define structure. Mixing tabs and spaces or having inconsistent indentation will cause errors. Pick one and stick with it (spaces are more common).

**2. Forgetting to close tags**
In Pug, you don't close tags explicitly. The indentation defines what element contains what. When you stop indenting, the element is closed.

**3. Confusing = with #{}**
- `p= var` is shorthand for `<p>#{var}</p>`
- `p #{var}` also outputs the variable
- The `#{}` syntax is explicit interpolation

**4. Using = for unescaped output**
Use `!=` (not `=`) for unescaped HTML output: `p!= htmlContent`

**5. Not understanding how mixins work**
Mixins need to be defined before they're used in the template. They must start with `mixin` keyword.

## ✅ Quick Recap

- Pug uses indentation instead of closing tags to define HTML structure
- Element attributes go in parentheses after the element name: `a(href='/')`
- Variables are interpolated with `#{variableName}` or `= variableName`
- Conditionals use `if`, `else`, and `unless` (negative)
- Loops use `each item in array` syntax
- Mixins (`mixin`) are reusable template components
- Use `!=` for unescaped output, `=` for escaped
- Include other files with `include ./path/to/file`
- Comments use `//` (visible) or `//-` (server-side only)

## 🔗 What's Next

Let's continue exploring Pug's features with layout inheritance, which is Pug's approach to creating reusable page templates.
