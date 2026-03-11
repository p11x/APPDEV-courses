# Bootstrap Installation Guide

## Method 1: CDN (Quick Start)

Add to HTML `<head>`:

```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
```

Add before `</body>`:

```html
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
```

## Method 2: npm Installation

```bash
npm install bootstrap
```

## Method 3: Download

1. Visit: https://getbootstrap.com/docs/5.3/getting-started/download/
2. Click "Download"
3. Extract files to project

## VS Code Setup

### Create Basic HTML Template

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Bootstrap Project</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
  <h1 class="text-center">Hello Bootstrap!</h1>
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
```

## Project Structure

```
project/
├── index.html
├── css/
│   └── styles.css (optional custom styles)
└── js/
    └── main.js (optional custom scripts)
```

## Verify Installation

Add this code to verify:

```html
<div class="container">
  <div class="row">
    <div class="col-md-4 bg-primary text-white p-3">Column 1</div>
    <div class="col-md-4 bg-success text-white p-3">Column 2</div>
    <div class="col-md-4 bg-danger text-white p-3">Column 3</div>
  </div>
</div>
```

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*