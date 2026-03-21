# Handlebars Syntax Basics

## 📌 What You'll Learn
- The basic syntax of Handlebars templating engine
- How Handlebars uses mustache syntax
- How to use helpers in Handlebars
- How Handlebars handles conditional logic and loops

## 🧠 Concept Explained (Plain English)

Handlebars.js (often just called Handlebars) is a semantic templating engine that uses curly braces (which look like a mustache, hence the name) to embed dynamic content into HTML templates. It was inspired by the Mustache templating language but adds more powerful features.

What makes Handlebars special:
- **Logic-less templates**: Handlebars is intentionally designed to keep templates clean by moving most logic into helpers (reusable functions)
- **Semantic syntax**: The {{ }} syntax is intuitive and reads like natural text
- **Mustache compatibility**: Works with many Mustache templates
- **Custom helpers**: You can create your own functions to transform data
- **Safe by default**: All output is HTML-escaped by default

Think of Handlebars like a stamp or stencil: you have a template with blanks ({{name}}), and Handlebars fills in those blanks with actual values. The blanks are the "handlebars" that "hold" the content.

## 💻 Code Example

```javascript
// ES Module - Handlebars Syntax Examples

import express from 'express';
import { engine } from 'express-handlebars';

const app = express();

// ========================================
// SETTING UP HANDLEBARS IN EXPRESS
// ========================================
// Create a Handlebars engine with .hbs extension
app.engine('hbs', engine({
    extname: '.hbs',          // Use .hbs as the file extension
    defaultLayout: 'main',    // Default layout file
    layoutsDir: 'views/layouts/'  // Location of layout files
}));

app.set('view engine', 'hbs');
app.set('views', './views');

// ========================================
// ROUTE FOR DEMONSTRATING HANDLEBARS SYNTAX
// ========================================
app.get('/demo', (req, res) => {
    // Sample data to pass to our template
    const demoData = {
        title: 'Handlebars Demo Page',
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

/*
// ========================================
// WHAT THE HANDLEBARS TEMPLATE (views/demo.hbs) LOOKS LIKE
// ========================================
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>{{title}}</title>
    <style>
        body { font-family: Arial, sans-serif; margin: 40px; }
        .premium { background-color: #fff8dc; padding: 10px; border-radius: 5px; }
    </style>
</head>
<body>
    <h1>{{title}}</h1>
    
    <!-- ========================================
         BASIC VARIABLE OUTPUT
         ======================================== -->
    <h2>User Information</h2>
    <p>Name: {{user.name}}</p>
    <p>Points: {{user.points}}</p>
    
    <!-- ========================================
         CONDITIONALS (#if)
         ======================================== -->
    {{#if user.isPremium}}
        <div class="premium">
            <p>Welcome back, premium member!</p>
            <p>You have {{user.points}} loyalty points.</p>
        </div>
    {{else}}
        <p>Upgrade to premium for special benefits!</p>
    {{/if}}
    
    <!-- ========================================
         UNLESS (NEGATIVE CONDITIONAL)
         ======================================== -->
    {{#unless user.isPremium}}
        <p>Consider upgrading to premium!</p>
    {{/unless}}
    
    <!-- ========================================
         LOOPS (#each)
         ======================================== -->
    {{#if showList}}
        <h2>Shopping List</h2>
        <ul>
            {{#each items}}
                <li>{{this}}</li>
            {{/each}}
        </ul>
    {{/if}}
    
    <!-- ========================================
         LOOP WITH @index
         ======================================== -->
    <h2>Items with Index</h2>
    <ol>
        {{#each items as |item index|}}
            <li>{{index}}: {{item}}</li>
        {{/each}}
    </ol>
    
    <!-- ========================================
         LOOPING THROUGH OBJECTS
         ======================================== -->
    <h2>User Object Properties</h2>
    <ul>
        {{#each user}}
            <li>{{@key}}: {{this}}</li>
        {{/each}}
    </ul>
    
    <!-- ========================================
         HTML ESCAPING
         ======================================== -->
    <h2>HTML Content</h2>
    <p>Escaped (safe): {{htmlContent}}</p>
    {{!-- Triple braces unescape HTML: --}}
    <p>Unescaped (raw): {{{htmlContent}}}</p>
    
    <!-- ========================================
         COMMENTS
         {{!-- This is a comment --}}
    
    <!-- ========================================
         USING BUILT-IN HELPERS
         ======================================== -->
    <h2>Using Helpers</h2>
    <p>Uppercase: {{uppercase user.name}}</p>
    <p>Lowercase: {{lowercase user.name}}</p>
    
    <!-- ========================================
         ACCESSING PARENT CONTEXT (@root)
         ======================================== -->
    {{#each items}}
        <p>{{@index}}: {{this}} (Title is: {{../title}})</p>
    {{/each}}
    
</body>
</html>
*/

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
```

## 🔍 Line-by-Line Breakdown (Template)

| Line | Code | What It Does |
|------|------|--------------|
| 7 | `<title>{{title}}</title>` | Outputs the title variable (HTML-escaped) |
| 19 | `{{user.name}}` | Accesses nested property (dot notation) |
| 25 | `{{#if user.isPremium}}` | Begins a conditional block |
| 32 | `{{else}}` | Alternative branch when condition is false |
| 35 | `{{/if}}` | Closes the if block |
| 39 | `{{#unless user.isPremium}}` | Negative conditional (shows when false) |
| 41 | `{{/unless}}` | Closes the unless block |
| 45 | `{{#each items}}` | Loops through an array |
| 47 | `{{this}}` | Refers to current item in loop |
| 49 | `{{/each}}` | Closes the each block |
| 55 | `{{#each items as \|item index\|}}` | Loop with named variables |
| 57 | `{{index}}` | Access loop index |
| 63 | `{{#each user}}` | Loops through object properties |
| 65 | `{{@key}}` | Gets the property name in object loop |
| 73 | `{{{htmlContent}}}` | Triple braces output unescaped HTML |
| 77 | `{{!-- comment --}}` | Handlebars comment (not in output) |
| 83 | `{{uppercase user.name}}` | Calls a helper function |

## ⚠️ Common Mistakes

**1. Forgetting to close block helpers**
Every `{{#if}}`, `{{#each}}`, `{{#unless}}` must have a matching `{{/if}}`, `{{/each}}`, `{{/unless}}`.

**2. Confusing double vs triple braces**
- `{{variable}}` - HTML-escaped (safe for user input)
- `{{{variable}}}` - Unescaped (use only with trusted content!)

**3. Trying to use JavaScript directly in templates**
Handlebars doesn't execute arbitrary JavaScript in templates. Use helpers for any logic.

**4. Not understanding @index and @key scope**
The `@index` and `@key` variables are only available inside `#each` loops. Use `@../variable` to access parent context.

**5. Missing the file extension configuration**
Make sure you configure Handlebars with the correct file extension (like `.hbs`) in your Express app.

## ✅ Quick Recap

- Handlebars uses `{{variable}}` syntax for output
- All double-brace output is HTML-escaped by default
- Use triple braces `{{{html}}}` for unescaped (raw) output
- Use `#if`, `#unless`, `#each` for conditionals and loops
- Always close block helpers with their closing tag
- Helpers are functions that transform data (built-in or custom)
- Use `../` to access parent context in nested blocks
- Comments use `{{!-- --}}` syntax and don't appear in output

## 🔗 What's Next

Let's look at how to create layouts and partials in Handlebars, which provides another approach to organizing your templates.
