# Bootstrap Tables

## Definition

Bootstrap tables provide elegant, responsive table styling with minimal effort. The basic `.table` class adds padding, horizontal dividers, and hover effects to HTML tables. Additional classes can add features like zebra striping, borders, dark themes, and more. Tables are essential for displaying data in rows and columns.

## Key Bullet Points

- **`.table`**: Base class that provides basic table styling
- **`.table-striped`**: Adds alternating background colors (zebra pattern)
- **`.table-bordered`**: Adds borders around all table cells
- **`.table-hover`**: Adds hover effect on table rows
- **`.table-dark`**: Dark theme for tables (inverts colors)
- **Responsive Tables**: Wrap tables in `.table-responsive` for horizontal scrolling on mobile

## HTML + Bootstrap Example

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bootstrap Tables</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="p-4">
    
    <h2 class="mb-4">Bootstrap Table Examples</h2>

    <!-- BASIC TABLE -->
    <h5>Basic Table (.table)</h5>
    <table class="table mb-4">
        <thead>
            <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>Role</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1</td>
                <td>John Doe</td>
                <td>john@example.com</td>
                <td>Admin</td>
            </tr>
            <tr>
                <td>2</td>
                <td>Jane Smith</td>
                <td>jane@example.com</td>
                <td>User</td>
            </tr>
            <tr>
                <td>3</td>
                <td>Bob Wilson</td>
                <td>bob@example.com</td>
                <td>Editor</td>
            </tr>
        </tbody>
    </table>

    <!-- STRIPED TABLE -->
    <h5>Striped Table (.table-striped)</h5>
    <table class="table table-striped mb-4">
        <thead>
            <tr>
                <th>Product</th>
                <th>Category</th>
                <th>Price</th>
                <th>Stock</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Laptop</td>
                <td>Electronics</td>
                <td>$999</td>
                <td>15</td>
            </tr>
            <tr>
                <td>Headphones</td>
                <td>Electronics</td>
                <td>$149</td>
                <td>30</td>
            </tr>
            <tr>
                <td>Keyboard</td>
                <td>Electronics</td>
                <td>$79</td>
                <td>25</td>
            </tr>
        </tbody>
    </table>

    <!-- BORDERED TABLE -->
    <h5>Bordered Table (.table-bordered)</h5>
    <table class="table table-bordered mb-4">
        <thead>
            <tr>
                <th>Header 1</th>
                <th>Header 2</th>
                <th>Header 3</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Cell 1</td>
                <td>Cell 2</td>
                <td>Cell 3</td>
            </tr>
            <tr>
                <td>Cell 4</td>
                <td>Cell 5</td>
                <td>Cell 6</td>
            </tr>
        </tbody>
    </table>

    <!-- HOVER TABLE -->
    <h5>Hover Table (.table-hover)</h5>
    <table class="table table-hover mb-4">
        <thead>
            <tr>
                <th>Task</th>
                <th>Status</th>
                <th>Priority</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Complete project</td>
                <td>In Progress</td>
                <td>High</td>
            </tr>
            <tr>
                <td>Send report</td>
                <td>Pending</td>
                <td>Medium</td>
            </tr>
            <tr>
                <td>Update website</td>
                <td>Done</td>
                <td>Low</td>
            </tr>
        </tbody>
    </table>

    <!-- DARK TABLE -->
    <h5>Dark Table (.table-dark)</h5>
    <table class="table table-dark mb-4">
        <thead>
            <tr>
                <th>Username</th>
                <th>Joined</th>
                <th>Posts</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>admin_user</td>
                <td>2023-01-15</td>
                <td>150</td>
            </tr>
            <tr>
                <td>new_member</td>
                <td>2023-06-20</td>
                <td>12</td>
            </tr>
        </tbody>
    </table>

    <!-- COMBINED TABLE -->
    <h5>Combined: Striped + Bordered + Hover</h5>
    <table class="table table-striped table-bordered table-hover">
        <thead class="table-primary">
            <tr>
                <th>Order #</th>
                <th>Customer</th>
                <th>Total</th>
                <th>Status</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>1001</td>
                <td>Alice Brown</td>
                <td>$250.00</td>
                <td><span class="badge bg-success">Shipped</span></td>
            </tr>
            <tr>
                <td>1002</td>
                <td>Charlie Davis</td>
                <td>$175.00</td>
                <td><span class="badge bg-warning text-dark">Processing</span></td>
            </tr>
            <tr>
                <td>1003</td>
                <td>Eve Martinez</td>
                <td>$320.00</td>
                <td><span class="badge bg-danger">Cancelled</span></td>
            </tr>
        </tbody>
    </table>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Component Explanation

- **`.table`**: Base class that applies standard Bootstrap table styling (padding, borders, font)
- **`.table-striped`**: Adds alternating light gray background to alternate rows for readability
- **`.table-bordered`**: Adds borders to all sides of every cell
- **`.table-hover`**: Adds a subtle background highlight when mouse hovers over rows
- **`.table-dark`**: Inverts colors - dark background with light text
- **`<thead>`**: Table header section - bold text, darker background
- **`<tbody>`**: Table body section - regular table data
- **`<tr>`**: Table row element
- **`<th>`**: Table header cell - bold and centered by default
- **`<td>`**: Table data cell - regular table cell
- **`.table-primary`**: Applies primary color to the header row

## Expected Visual Result

The page displays 6 different table examples:

1. **Basic Table**: Clean white table with light borders, blue header row
2. **Striped Table**: Alternating white and light gray rows
3. **Bordered Table**: All cells have visible borders
4. **Hover Table**: Rows highlight when mouse hovers over them
5. **Dark Table**: Dark gray background with white text
6. **Combined Table**: Uses striped, bordered, hover together with colored header - most complete example

Each table has proper spacing, readable text, and professional styling suitable for displaying data.
