# Section 3: Exercises

## Exercise 3.1: HTML Elements Practice

### Objective
Practice using common HTML elements.

### Instructions
Create an HTML page with:
1. A main heading (h1)
2. A subheading (h2)
3. At least 3 paragraphs of text
4. An unordered list with 5 items
5. A link to your favorite website
6. An image
7. A table with at least 3 rows

### Template
```html
<!DOCTYPE html>
<html>
<head>
    <title>My Practice Page</title>
</head>
<body>
    <!-- Add your content here -->
</body>
</html>
```

---

## Exercise 3.2: CSS Styling Practice

### Objective
Apply CSS styling to your HTML page.

### Instructions
1. Create a CSS file called `practice.css`
2. Add styles for:
   - Background color for the body
   - Different font for headings
   - Custom colors for links (normal, hover, visited)
   - Padding and margin for paragraphs
   - Table styling (borders, padding, background colors)

### CSS Requirements
```css
body {
    /* Add your styles */
}

h1 {
    /* Add your styles */
}

table {
    /* Add your styles */
}
```

---

## Exercise 3.3: Build a Product Form

### Objective
Create an HTML form for adding products.

### Requirements
Create a form with these fields:
- Product Name (text input, required)
- Description (textarea)
- Price (number input, required)
- Category (dropdown select)
- In Stock (checkbox)
- Submit button

### HTML Structure
```html
<form class="product-form">
    <div class="form-group">
        <label for="name">Product Name</label>
        <input type="text" id="name" name="name" required>
    </div>
    
    <!-- Add remaining fields -->
    
    <button type="submit">Add Product</button>
</form>
```

---

## Exercise 3.4: Flexbox Layout Challenge

### Objective
Master flexbox layout.

### Task 1: Navigation Bar
Create a horizontal navigation bar:
```
[Logo]    [Home] [Products] [About] [Contact]
```

### Task 2: Card Layout
Create a row of 3 cards that:
- Are evenly spaced
- Wrap on smaller screens
- Have equal height

### Task 3: Centered Content
Create a centered card:
```
           ┌─────────────┐
           │  Centered   │
           │    Card     │
           └─────────────┘
```

---

## Exercise 3.5: Responsive Design

### Objective
Make a page responsive.

### Instructions
1. Create a page with 4 product cards
2. Use media queries to show:
   - 1 column on mobile (under 600px)
   - 2 columns on tablet (600px - 900px)
   - 4 columns on desktop (over 900px)

### Test Your Code
Resize your browser window and verify the layout changes.

---

## Exercise 3.6: CSS Grid vs Flexbox

### Objective
Understand when to use Grid vs Flexbox.

### Instructions
For each scenario, decide whether to use CSS Grid or Flexbox:

1. **Navigation bar** → Flexbox
2. **Photo gallery** → _________
3. **Card layout** → _________
4. **Page layout** → _________
5. **Form controls** → _________

### Answers
1. Grid
2. Flexbox
3. Grid
4. Flexbox

---

## Exercise 3.7: Debug CSS Issues

### Objective
Find and fix common CSS problems.

### Problem 1
The image is too big:
```html
<img src="large-image.jpg" class="product-img">
```
Fix: Add CSS to limit the image size.

### Problem 2
The button is not clickable:
```html
<div class="card">
    <button>Click Me</button>
</div>
```
The card has `position: relative`. What's wrong?

### Problem 3
The margin is not working:
```css
.inline-elements span {
    margin: 20px;
}
```
Why isn't the margin applying?

### Answers
1. Add `.product-img { max-width: 100%; height: auto; }`
2. The button might have `pointer-events: none` or be covered by another element
3. Inline elements don't respond to margin-top/bottom. Use `display: inline-block`

---

## Bonus Exercise: Create Your Portfolio Page

### Objective
Build a personal portfolio page using HTML and CSS.

### Requirements
1. Header with your name and navigation
2. About section with a photo and bio
3. Projects section with at least 3 project cards
4. Contact section with a form
5. Footer with social links
6. Fully responsive design

### Design Guidelines
- Use a consistent color scheme
- Choose appropriate fonts
- Add hover effects
- Ensure readability
