---
title: Autocomplete Input
category: Component System
difficulty: 2
time: 30 min
tags: bootstrap5, autocomplete, datalist, search, forms, dropdown
---

## Overview

Bootstrap supports autocomplete through the native HTML `<datalist>` element, which provides a suggestion dropdown when combined with a text input. For richer autocomplete behavior, you can build custom dropdown patterns using Bootstrap's `dropdown` component with JavaScript filtering. These patterns enable search-as-you-type, typeahead selection, and dynamic option loading.

## Basic Implementation

The native `<datalist>` provides a simple autocomplete experience without JavaScript.

```html
<!-- Native datalist autocomplete -->
<div class="mb-3">
  <label for="browserChoice" class="form-label">Choose a browser</label>
  <input class="form-control" list="browserOptions" id="browserChoice" placeholder="Type to search...">
  <datalist id="browserOptions">
    <option value="Chrome">
    <option value="Firefox">
    <option value="Safari">
    <option value="Edge">
    <option value="Opera">
  </datalist>
</div>

<!-- Datalist with form layout -->
<form class="row g-3">
  <div class="col-md-6">
    <label for="cityInput" class="form-label">City</label>
    <input class="form-control" list="cityOptions" id="cityInput" placeholder="Search cities...">
    <datalist id="cityOptions">
      <option value="New York">
      <option value="Los Angeles">
      <option value="Chicago">
      <option value="Houston">
      <option value="Phoenix">
    </datalist>
  </div>
  <div class="col-md-6">
    <label for="stateInput" class="form-label">State</label>
    <input class="form-control" list="stateOptions" id="stateInput" placeholder="Search states...">
    <datalist id="stateOptions">
      <option value="California">
      <option value="Texas">
      <option value="Florida">
      <option value="New York">
      <option value="Illinois">
    </datalist>
  </div>
</form>
```

## Advanced Variations

```html
<!-- Custom autocomplete with dropdown (JavaScript-driven) -->
<div class="mb-3 position-relative">
  <label for="searchInput" class="form-label">Search products</label>
  <input type="text" class="form-control" id="searchInput" placeholder="Type to search..."
         autocomplete="off" role="combobox" aria-expanded="false" aria-controls="searchResults">
  <ul class="list-group position-absolute w-100 mt-1 shadow" id="searchResults"
      style="z-index: 1050; max-height: 200px; overflow-y: auto; display: none;"
      role="listbox">
    <li class="list-group-item list-group-item-action" role="option">Laptop</li>
    <li class="list-group-item list-group-item-action" role="option">Laptop Stand</li>
    <li class="list-group-item list-group-item-action" role="option">Laptop Bag</li>
  </ul>
</div>

<!-- Autocomplete with input group and search icon -->
<div class="mb-3">
  <label for="searchIcon" class="form-label">Quick search</label>
  <div class="input-group">
    <span class="input-group-text"><i class="bi bi-search"></i></span>
    <input type="text" class="form-control" list="searchSuggestions" id="searchIcon" placeholder="Search...">
    <datalist id="searchSuggestions">
      <option value="Bootstrap documentation">
      <option value="Bootstrap components">
      <option value="Bootstrap grid system">
      <option value="Bootstrap utilities">
    </datalist>
    <button class="btn btn-primary" type="button">Search</button>
  </div>
</div>
```

```html
<!-- Autocomplete with recent searches -->
<div class="mb-3">
  <label for="recentSearch" class="form-label">Search</label>
  <input class="form-control" list="recentSearches" id="recentSearch" placeholder="Search or select recent...">
  <datalist id="recentSearches">
    <option value="react hooks">
    <option value="bootstrap grid">
    <option value="css flexbox">
  </datalist>
  <div class="form-text">Start typing or select from recent searches.</div>
</div>
```

## Best Practices

1. Use native `<datalist>` for simple autocomplete needs without JavaScript.
2. Always include a `<label>` associated with the autocomplete input.
3. Set `autocomplete="off"` on custom implementations to prevent browser interference.
4. Use `role="combobox"` with `aria-expanded` and `aria-controls` for custom autocomplete.
5. Provide `role="listbox"` and `role="option"` on custom dropdown items.
6. Debounce search-as-you-type requests to avoid excessive API calls.
7. Highlight matching text in suggestions for quick scanning.
8. Limit visible suggestions with `max-height` and `overflow-y: auto`.
9. Support keyboard navigation with Arrow keys, Enter, and Escape.
10. Show a "No results" message when the filter produces empty results.

## Common Pitfalls

1. **Datalist styling limitations.** Browsers control datalist appearance; you cannot fully customize it with CSS.
2. **Missing `autocomplete="off"`.** Browser autofill overlays custom dropdown suggestions.
3. **No keyboard navigation.** Custom implementations without arrow key support are unusable.
4. **Unbounded suggestion lists.** Showing hundreds of suggestions degrades performance and UX.
5. **No debouncing on API searches.** Each keystroke fires a network request, overwhelming the server.
6. **Z-index conflicts.** Dropdown results hidden behind other elements due to stacking context issues.

## Accessibility Considerations

Native datalists are accessible by default. Custom autocomplete must implement the ARIA combobox pattern: `role="combobox"`, `aria-expanded`, `aria-controls` pointing to the listbox, and `aria-activedescendant` for the highlighted option. Announce result counts with `aria-live="polite"`. Ensure Escape closes the dropdown and returns focus to the input. Provide `aria-label` or visible labels describing the search context.

## Responsive Behavior

Autocomplete inputs fill their container width by default. On mobile, ensure dropdown suggestions are wide enough to read without truncation. Use `position-relative` on the parent to anchor dropdowns correctly. For custom implementations, set dropdown `max-height` to avoid covering the entire screen on mobile. Input groups with search icons stack naturally with Bootstrap's responsive grid.
