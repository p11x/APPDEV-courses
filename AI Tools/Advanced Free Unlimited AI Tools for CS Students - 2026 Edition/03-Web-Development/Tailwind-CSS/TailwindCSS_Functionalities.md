# Tailwind CSS Functionalities

## What You Can Do with Tailwind CSS

### 1. Rapid Styling

Build custom designs quickly:

```html
<!-- Button -->
<button class="px-4 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600">
  Click Me
</button>

<!-- Card -->
<div class="p-6 bg-white rounded-xl shadow-lg">
  <h2 class="text-2xl font-bold">Title</h2>
</div>
```

### 2. Responsive Layouts

Create mobile-first responsive designs:

```html
<!-- Stack on mobile, side-by-side on desktop -->
<div class="flex flex-col md:flex-row gap-4">
  <div class="w-full md:w-1/2">Content 1</div>
  <div class="w-full md:w-1/2">Content 2</div>
</div>
```

### 3. Dark Mode

Implement dark mode easily:

```html
<!-- Using dark: variant -->
<div class="bg-white dark:bg-gray-900 text-gray-900 dark:text-white">
  Content adapts to dark mode
</div>
```

### 4. Flexbox layouts

Build flexible layouts:

```html
<!-- Center content -->
<div class="flex items-center justify-center h-screen">
  Centered
</div>

<!-- Space between -->
<div class="flex justify-between">
  <div>Left</div>
  <div>Right</div>
</div>
```

### 5. Grid layouts

Create complex grids:

```html
<!-- Simple grid -->
<div class="grid grid-cols-3 gap-4">
  <div>1</div>
  <div>2</div>
  <div>3</div>
</div>

<!-- Responsive grid -->
<div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
```

### 6. Typography

Style text elements:

```html
<h1 class="text-4xl font-bold text-center mb-4">Heading</h1>
<p class="text-lg leading-relaxed text-gray-600">Paragraph</p>
```

### 7. Forms

Build styled forms:

```html
<input class="border-2 border-gray-300 rounded-lg p-2 focus:border-blue-500 focus:outline-none">

<select class="bg-gray-100 rounded p-2">
  <option>Option 1</option>
</select>
```

### 8. Animations

Add animations:

```html
<!-- Spin -->
<div class="animate-spin">...</div>

<!-- Pulse -->
<div class="animate-pulse">...</div>

<!-- Custom transition -->
<button class="transition-transform hover:scale-105">
```

### 9. Custom Components

Build reusable components:

```html
<!-- Alert component -->
<div class="p-4 bg-blue-100 border-l-4 border-blue-500">
  <p class="text-blue-700">Alert message</p>
</div>
```

### 10. AI Code Generation

Use with AI tools:

| Prompt | Result |
|--------|--------|
| "Create responsive navbar" | Navigation code |
| "Build login form" | Form with validation styles |
| "Design dashboard layout" | Grid-based dashboard |

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*