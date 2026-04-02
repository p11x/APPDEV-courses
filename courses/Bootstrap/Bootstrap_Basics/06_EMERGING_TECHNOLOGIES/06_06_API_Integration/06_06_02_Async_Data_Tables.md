---
title: "Async Data Tables"
description: "Load table data via REST API with Bootstrap 5 tables, pagination, sorting, and filtering"
difficulty: 2
tags: [api, tables, pagination, sorting, filtering]
prerequisites:
  - "Bootstrap 5 table component"
  - "REST API loading states"
  - "JavaScript array methods"
---

## Overview

Data tables populated from API endpoints require loading states, sortable columns, paginated results, and filter inputs. Bootstrap 5's table component provides the styling foundation, while JavaScript handles data fetching, client-side sorting, pagination logic, and search filtering. This section builds a complete async data table with all these features using vanilla JavaScript and Bootstrap.

## Basic Implementation

### Basic API Table

Fetch and render data into a Bootstrap table.

```html
<div class="table-responsive">
  <table class="table table-striped table-hover">
    <thead class="table-dark">
      <tr>
        <th>ID</th>
        <th>Name</th>
        <th>Email</th>
        <th>City</th>
      </tr>
    </thead>
    <tbody id="tableBody">
      <tr>
        <td colspan="4" class="text-center py-4">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        </td>
      </tr>
    </tbody>
  </table>
</div>

<script>
  async function loadTable() {
    try {
      const res = await fetch('https://jsonplaceholder.typicode.com/users');
      const users = await res.json();
      const tbody = document.getElementById('tableBody');

      tbody.innerHTML = users.map(user => `
        <tr>
          <td>${user.id}</td>
          <td>${user.name}</td>
          <td>${user.email}</td>
          <td>${user.address.city}</td>
        </tr>`).join('');
    } catch (error) {
      document.getElementById('tableBody').innerHTML = `
        <tr><td colspan="4" class="text-center text-danger py-4">
          Failed to load data. <a href="#" onclick="loadTable(); return false;">Retry</a>
        </td></tr>`;
    }
  }
  loadTable();
</script>
```

## Advanced Variations

### Sortable Table

Add click-to-sort functionality on column headers.

```html
<div class="d-flex justify-content-between align-items-center mb-3">
  <input type="text" class="form-control w-auto" id="searchInput" placeholder="Search...">
</div>
<div class="table-responsive">
  <table class="table table-striped table-hover">
    <thead class="table-dark">
      <tr>
        <th class="sortable" data-sort="id" style="cursor: pointer;">ID <i class="bi bi-arrow-down-up"></i></th>
        <th class="sortable" data-sort="name" style="cursor: pointer;">Name <i class="bi bi-arrow-down-up"></i></th>
        <th class="sortable" data-sort="email" style="cursor: pointer;">Email <i class="bi bi-arrow-down-up"></i></th>
        <th>City</th>
      </tr>
    </thead>
    <tbody id="sortTableBody"></tbody>
  </table>
</div>

<script>
  let tableData = [];
  let sortField = 'id';
  let sortDir = 'asc';

  async function loadSortableTable() {
    const res = await fetch('https://jsonplaceholder.typicode.com/users');
    tableData = await res.json();
    renderTable();
  }

  function renderTable() {
    const search = document.getElementById('searchInput').value.toLowerCase();
    let filtered = tableData.filter(u =>
      u.name.toLowerCase().includes(search) || u.email.toLowerCase().includes(search)
    );

    filtered.sort((a, b) => {
      const valA = a[sortField] || '';
      const valB = b[sortField] || '';
      const cmp = typeof valA === 'number' ? valA - valB : valA.localeCompare(valB);
      return sortDir === 'asc' ? cmp : -cmp;
    });

    document.getElementById('sortTableBody').innerHTML = filtered.length
      ? filtered.map(u => `<tr><td>${u.id}</td><td>${u.name}</td><td>${u.email}</td><td>${u.address.city}</td></tr>`).join('')
      : '<tr><td colspan="4" class="text-center text-muted py-3">No results found</td></tr>';
  }

  document.querySelectorAll('.sortable').forEach(th => {
    th.addEventListener('click', () => {
      const field = th.dataset.sort;
      sortDir = sortField === field && sortDir === 'asc' ? 'desc' : 'asc';
      sortField = field;
      renderTable();
    });
  });

  document.getElementById('searchInput').addEventListener('input', renderTable);
  loadSortableTable();
</script>
```

### Paginated Table

```html
<nav>
  <ul class="pagination justify-content-center" id="pagination"></ul>
</nav>

<script>
  const PAGE_SIZE = 5;
  let allData = [];
  let currentPage = 1;

  async function loadPaginated() {
    const res = await fetch('https://jsonplaceholder.typicode.com/posts');
    allData = await res.json();
    renderPage(1);
  }

  function renderPage(page) {
    currentPage = page;
    const start = (page - 1) * PAGE_SIZE;
    const slice = allData.slice(start, start + PAGE_SIZE);

    document.getElementById('paginatedBody').innerHTML = slice.map(item => `
      <tr>
        <td>${item.id}</td>
        <td>${item.title.substring(0, 50)}...</td>
      </tr>`).join('');

    const totalPages = Math.ceil(allData.length / PAGE_SIZE);
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
      pagination.innerHTML += `
        <li class="page-item ${i === currentPage ? 'active' : ''}">
          <a class="page-link" href="#" onclick="renderPage(${i}); return false;">${i}</a>
        </li>`;
    }
  }
  loadPaginated();
</script>
```

## Best Practices

1. **Show loading skeleton in table body** while fetching data to avoid empty table flash.
2. **Use `table-responsive`** wrapper for horizontal scrolling on narrow screens.
3. **Implement client-side search** with `input` event for instant filtering feedback.
4. **Highlight sorted column** with visual indicator (arrow icon) for active sort state.
5. **Use `PAGE_SIZE` constant** for configurable pagination limits.
6. **Sanitize all data** before rendering to prevent XSS attacks from API responses.
7. **Cache fetched data** to avoid re-fetching when switching pages client-side.
8. **Show total record count** and current range (e.g., "Showing 1-10 of 150").
9. **Use `localeCompare()`** for string sorting with proper locale handling.
10. **Disable pagination controls** during loading to prevent rapid page switches.
11. **Add empty state** message when filters return no results.
12. **Use server-side pagination** for large datasets (>1000 records).

## Common Pitfalls

1. **Not handling empty API responses** - always check for empty arrays.
2. **XSS vulnerabilities** from unsanitized API data inserted via `innerHTML`.
3. **Sorting numeric fields as strings** - `'10'` sorts before `'2'` without `typeof` check.
4. **Pagination off-by-one errors** - `(page - 1) * PAGE_SIZE` for zero-indexed slicing.
5. **Search filtering on every keystroke** without debouncing causes performance issues.
6. **Not resetting to page 1** when search filter changes.
7. **Memory leaks** from event listeners on dynamically created pagination elements.
8. **Missing `table-responsive`** causes horizontal overflow on mobile.

## Accessibility Considerations

- Use `<table>` with proper `<thead>` and `<tbody>` for semantic structure.
- Sortable columns should have `aria-sort` attribute indicating current sort direction.
- Pagination links should have `aria-label` describing the page being navigated to.
- Search input needs a visible or `visually-hidden` label.
- Loading states should announce via `aria-live="polite"`.
- Empty state messages should be announced to screen readers.

## Responsive Behavior

- Wrap tables in `.table-responsive` for horizontal scrolling on mobile.
- Consider hiding less important columns on small screens using `.d-none .d-md-table-cell`.
- Stack pagination items horizontally with smaller margins on mobile.
- Use `table-sm` class for more compact tables on narrow screens.
- Search input should be full-width on mobile with `w-100 w-md-auto`.
