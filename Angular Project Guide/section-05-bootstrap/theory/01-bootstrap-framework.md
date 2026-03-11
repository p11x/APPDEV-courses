# Section 5: Bootstrap Framework

## Introduction to Bootstrap

Bootstrap is a popular CSS framework that provides pre-built components and responsive grid system. It speeds up development significantly!

### What You'll Learn

- Bootstrap grid system
- Pre-built components
- Responsive design with Bootstrap
- Customizing Bootstrap

---

## 5.1 Getting Started with Bootstrap

### Including Bootstrap

**Option 1: CDN (recommended for learning)**
```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

**Option 2: npm**
```bash
npm install bootstrap
```

---

## 5.2 Bootstrap Grid System

### The Grid
```html
<div class="container">
    <div class="row">
        <div class="col-12 col-md-6 col-lg-4">
            Column 1
        </div>
        <div class="col-12 col-md-6 col-lg-4">
            Column 2
        </div>
        <div class="col-12 col-md-6 col-lg-4">
            Column 3
        </div>
    </div>
</div>
```

### Grid Breakpoints

| Breakpoint | Width | Class |
|------------|-------|-------|
| Extra small | <576px | col- |
| Small | ≥576px | col-sm- |
| Medium | ≥768px | col-md- |
| Large | ≥992px | col-lg- |
| Extra large | ≥1200px | col-xl- |

---

## 5.3 Bootstrap Components

### Buttons
```html
<button class="btn btn-primary">Primary</button>
<button class="btn btn-secondary">Secondary</button>
<button class="btn btn-success">Success</button>
<button class="btn btn-danger">Danger</button>
<button class="btn btn-warning">Warning</button>
<button class="btn btn-info">Info</button>
<button class="btn btn-light">Light</button>
<button class="btn btn-dark">Dark</button>
```

### Cards
```html
<div class="card" style="width: 18rem;">
    <img src="..." class="card-img-top" alt="...">
    <div class="card-body">
        <h5 class="card-title">Card title</h5>
        <p class="card-text">Some quick example text.</p>
        <a href="#" class="btn btn-primary">Go somewhere</a>
    </div>
</div>
```

### Forms
```html
<form>
    <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input type="email" class="form-control" id="email">
    </div>
    <div class="mb-3">
        <label for="password" class="form-label">Password</label>
        <input type="password" class="form-control" id="password">
    </div>
    <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

### Navigation
```html
<nav class="navbar navbar-expand-lg navbar-light bg-light">
    <a class="navbar-brand" href="#">Brand</a>
    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <span class="navbar-toggler-icon"></span>
    </button>
    <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link active" href="#">Home</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="#">Features</a>
            </li>
        </ul>
    </div>
</nav>
```

### Tables
```html
<table class="table table-striped table-hover">
    <thead>
        <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>1</td>
            <td>Laptop</td>
            <td>$999</td>
        </tr>
    </tbody>
</table>
```

### Modals
```html
<button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#myModal">
    Open Modal
</button>

<div class="modal" id="myModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Modal title</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                Modal content here
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary">Save</button>
            </div>
        </div>
    </div>
</div>
```

### Alerts
```html
<div class="alert alert-success" role="alert">
    Success! Your action was completed.
</div>
<div class="alert alert-danger" role="alert">
    Error! Something went wrong.
</div>
```

### Badges
```html
<span class="badge bg-primary">Primary</span>
<span class="badge bg-success">Success</span>
<span class="badge bg-danger">Danger</span>
```

---

## 5.4 Utility Classes

### Spacing
```html
<div class="m-3 p-3">Margin and Padding</div>
<div class="mt-3 mb-3 ms-3 me-3 pt-3 pb-3 ps-3 pe-3">Individual sides</div>
```

### Text
```html
<p class="text-start">Left aligned</p>
<p class="text-center">Center aligned</p>
<p class="text-end">Right aligned</p>
<p class="text-primary">Primary color</p>
```

### Display
```html
<div class="d-none">Hidden</div>
<div class="d-block">Visible</div>
<div class="d-flex">Flexbox</div>
```

---

## 5.5 Bootstrap with Our Product App

```html
<!-- Product Grid with Bootstrap -->
<div class="container">
    <div class="row g-4">
        <div class="col-12 col-md-6 col-lg-4" ng-repeat="product in products">
            <div class="card h-100">
                <img [src]="product.image" class="card-img-top" [alt]="product.name">
                <div class="card-body">
                    <h5 class="card-title">{{ product.name }}</h5>
                    <p class="card-text">{{ product.description }}</p>
                    <p class="text-success fw-bold">\${{ product.price }}</p>
                </div>
                <div class="card-footer">
                    <button class="btn btn-primary w-100">Add to Cart</button>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## 5.6 Summary

### Key Takeaways

1. Bootstrap provides ready-to-use components
2. Grid system makes responsive design easy
3. Use utility classes for spacing and text
4. Components include buttons, cards, forms, modals, etc.

### What's Next?

Now let's learn **TypeScript** - the language Angular uses!
