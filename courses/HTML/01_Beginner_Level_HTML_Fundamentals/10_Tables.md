# Tables

## Topic Title
Creating Tables in HTML

## Concept Explanation

### What Are Tables?

Tables are HTML elements used to display data in a structured, grid-like format with rows and columns. They are perfect for presenting tabular data like schedules, comparison charts, and financial reports.

### Table Elements

- `<table>` - The table container
- `<thead>` - Table header section (optional)
- `<tbody>` - Table body section (optional)
- `<tfoot>` - Table footer section (optional)
- `<tr>` - Table row
- `<th>` - Table header cell
- `<td>` - Table data cell

### Table Structure

```
┌──────────┬──────────┬──────────┐
│  Header  │  Header  │  Header  │
├──────────┼──────────┼──────────┤
│   Data   │   Data   │   Data   │
├──────────┼──────────┼──────────┤
│   Data   │   Data   │   Data   │
└──────────┴──────────┴──────────┘
```

### Key Attributes

- `colspan` - Span multiple columns
- `rowspan` - Span multiple rows
- `scope` - Links header to data cells

## Why This Concept Is Important

Tables matter because:

1. **Data presentation** - Best way to show tabular data
2. **Organization** - Makes complex data readable
3. **Accessibility** - Screen readers can interpret tables
4. **Structure** - Provides semantic meaning
5. **Reports** - Essential for financial/data content

## Step-by-Step Explanation

### Step 1: Basic Table

```html
<table>
    <tr>
        <th>Name</th>
        <th>Age</th>
    </tr>
    <tr>
        <td>John</td>
        <td>25</td>
    </tr>
</table>
```

### Step 2: Complete Table with Sections

```html
<table>
    <thead>
        <tr>
            <th>Header 1</th>
            <th>Header 2</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Data 1</td>
            <td>Data 2</td>
        </tr>
    </tbody>
    <tfoot>
        <tr>
            <td>Footer 1</td>
            <td>Footer 2</td>
        </tr>
    </tfoot>
</table>
```

### Step 3: Column Spanning (colspan)

```html
<tr>
    <td colspan="2">Spans 2 columns</td>
</tr>
```

### Step 4: Row Spanning (rowspan)

```html
<td rowspan="2">Spans 2 rows</td>
```

## Code Examples

### Example 1: Basic Student Table

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Student Table</title>
</head>
<body>
    <h1>Student Grades</h1>
    
    <table border="1">
        <thead>
            <tr>
                <th>Name</th>
                <th>Subject</th>
                <th>Grade</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Alice</td>
                <td>Mathematics</td>
                <td>A</td>
            </tr>
            <tr>
                <td>Bob</td>
                <td>Mathematics</td>
                <td>B</td>
            </tr>
            <tr>
                <td>Charlie</td>
                <td>Mathematics</td>
                <td>A-</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Example 2: Complete Table with All Sections

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Product Table</title>
</head>
<body>
    <h1>Product Catalog</h1>
    
    <table border="1">
        <thead>
            <tr>
                <th>Product</th>
                <th>Price</th>
                <th>Quantity</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Laptop</td>
                <td>$999</td>
                <td>2</td>
                <td>$1998</td>
            </tr>
            <tr>
                <td>Mouse</td>
                <td>$29</td>
                <td>5</td>
                <td>$145</td>
            </tr>
            <tr>
                <td>Keyboard</td>
                <td>$79</td>
                <td>3</td>
                <td>$237</td>
            </tr>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="3">Grand Total</td>
                <td>$2380</td>
            </tr>
        </tfoot>
    </table>
</body>
</html>
```

### Example 3: Colspan and Rowspan

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Complex Table</title>
</head>
<body>
    <h1>Schedule</h1>
    
    <table border="1">
        <thead>
            <tr>
                <th rowspan="2">Day</th>
                <th colspan="3">Schedule</th>
            </tr>
            <tr>
                <th>Morning</th>
                <th>Afternoon</th>
                <th>Evening</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Monday</td>
                <td>Meeting</td>
                <td>Workshop</td>
                <td>Free</td>
            </tr>
            <tr>
                <td>Tuesday</td>
                <td>Training</td>
                <td>Training</td>
                <td>Event</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Example 4: Accessible Table

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Accessible Table</title>
</head>
<body>
    <h1>Sales Report</h1>
    
    <table>
        <caption>Quarterly Sales Data (in thousands)</caption>
        <thead>
            <tr>
                <th scope="col">Product</th>
                <th scope="col">Q1</th>
                <th scope="col">Q2</th>
                <th scope="col">Q3</th>
                <th scope="col">Q4</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th scope="row">Widget A</th>
                <td>$150</td>
                <td>$175</td>
                <td>$200</td>
                <td>$225</td>
            </tr>
            <tr>
                <th scope="row">Widget B</th>
                <td>$100</td>
                <td>$125</td>
                <td>$150</td>
                <td>$175</td>
            </tr>
        </tbody>
    </table>
</body>
</html>
```

### Example 5: Angular Table Component

```html
<!-- Angular table with dynamic data -->
<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Role</th>
        </tr>
    </thead>
    <tbody>
        <tr *ngFor="let user of users">
            <td>{{ user.name }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.role }}</td>
        </tr>
    </tbody>
</table>
```

## Best Practices

### Table Best Practices

1. **Use proper structure** - Include thead, tbody, tfoot
2. **Always use headers** - Use `<th>` for header cells
3. **Include captions** - Use `<caption>` for context
4. **Use scope attribute** - Links headers to data cells

### Accessibility Best Practices

1. **Use scope attribute** - Helps screen readers
2. **Include captions** - Provides context
3. **Don't leave cells empty** - Use non-breaking space if needed
4. **Logical reading order** - Data should make sense left-to-right

### Styling Best Practices

1. **Use CSS for styling** - Not HTML attributes
2. **Set fixed widths carefully** - Consider responsive design
3. **Add padding and borders** - Makes tables readable
4. **Zebra striping** - Alternate row colors help readability

## Real-World Examples

### Example 1: Pricing Table

```html
<section class="pricing">
    <h2>Pricing Plans</h2>
    
    <table>
        <thead>
            <tr>
                <th>Feature</th>
                <th>Basic</th>
                <th>Pro</th>
                <th>Enterprise</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Users</td>
                <td>1</td>
                <td>5</td>
                <td>Unlimited</td>
            </tr>
            <tr>
                <td>Storage</td>
                <td>5GB</td>
                <td>50GB</td>
                <td>Unlimited</td>
            </tr>
            <tr>
                <td>Support</td>
                <td>Email</td>
                <td>Priority</td>
                <td>24/7</td>
            </tr>
            <tr>
                <td>Price</td>
                <td>$9/mo</td>
                <td>$29/mo</td>
                <td>Contact Us</td>
            </tr>
        </tbody>
    </table>
</section>
```

### Example 2: Timetable

```html
<h1>Class Schedule</h1>
<table>
    <thead>
        <tr>
            <th>Time</th>
            <th>Monday</th>
            <th>Tuesday</th>
            <th>Wednesday</th>
            <th>Thursday</th>
            <th>Friday</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>9:00</td>
            <td>Math</td>
            <td>English</td>
            <td>Math</td>
            <td>English</td>
            <td>Science</td>
        </tr>
        <tr>
            <td>10:00</td>
            <td>Science</td>
            <td>Math</td>
            <td>English</td>
            <td>Math</td>
            <td>History</td>
        </tr>
    </tbody>
</table>
```

### Example 3: Comparison Table

```html
<h1>Product Comparison</h1>
<table>
    <thead>
        <tr>
            <th>Feature</th>
            <th>Product A</th>
            <th>Product B</th>
            <th>Product C</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>Price</td>
            <td>$99</td>
            <td>$149</td>
            <td>$199</td>
        </tr>
        <tr>
            <td>Battery Life</td>
            <td>10 hours</td>
            <td>15 hours</td>
            <td>20 hours</td>
        </tr>
        <tr>
            <td>Warranty</td>
            <td>1 year</td>
            <td>2 years</td>
            <td>3 years</td>
        </tr>
    </tbody>
</table>
```

## Common Mistakes Students Make

### Mistake 1: Using Tables for Layout

```html
<!-- Wrong - tables shouldn't be used for layout -->
<table>
    <tr>
        <td>Sidebar</td>
        <td>Main Content</td>
    </tr>
</table>

<!-- Correct - use CSS for layout -->
<div class="container">
    <div class="sidebar">Sidebar</div>
    <div class="main">Main Content</div>
</div>
```

### Mistake 2: Missing Headers

```html
<!-- Wrong - all cells are data -->
<tr>
    <td>Name</td>
    <td>John</td>
</tr>

<!-- Correct - header cells for headers -->
<tr>
    <th>Name</th>
    <td>John</td>
</tr>
```

### Mistake 3: Inconsistent Cell Count

```html
<!-- Wrong - different number of cells -->
<tr>
    <td>Cell 1</td>
    <td>Cell 2</td>
</tr>
<tr>
    <td>Cell 1</td>
</tr>

<!-- Correct - same number of cells -->
<tr>
    <td>Cell 1</td>
    <td>Cell 2</td>
</tr>
<tr>
    <td>Cell 1</td>
    <td>Cell 2</td>
</tr>
```

### Mistake 4: Using Images Instead of Tables

```html
<!-- Wrong - table data as image -->
<img src="table-screenshot.png" alt="Price comparison">

<!-- Correct - actual table -->
<table>
    <!-- Table content -->
</table>
```

## Exercises

### Exercise 1: Create a Simple Table
Create a table showing your class schedule.

### Exercise 2: Product Comparison
Create a comparison table for 3 products.

### Exercise 3: Use Colspan
Create a table that uses colspan to merge cells.

### Exercise 4: Accessible Table
Add proper accessibility attributes to a table.

## Mini Practice Tasks

### Task 1: Basic Table
Create a table with 3 rows and 3 columns.

### Task 2: Headers
Add header cells to your table.

### Task 3: Sections
Organize the table with thead, tbody, and tfoot.

### Task 4: Caption
Add a caption to describe the table content.

### Task 5: Merge Cells
Create a table that spans columns or rows.
