---
title: "GraphQL Basic Integration"
description: "Fetch data with GraphQL queries and display results using Bootstrap 5 loading, error, and data states"
difficulty: 3
tags: [graphql, api, fetch, loading-states, data-fetching]
prerequisites:
  - "REST API loading states"
  - "JavaScript Fetch API"
  - "GraphQL query syntax basics"
---

## Overview

GraphQL provides a flexible query language for fetching exactly the data needed from an API. Unlike REST, GraphQL uses a single endpoint with POST requests containing query strings. Bootstrap 5 components handle the UI states - loading spinners, error alerts, and data display. This section covers constructing GraphQL queries, handling the response envelope, managing loading/error/data states, and building reusable query patterns.

## Basic Implementation

### Basic GraphQL Query

```html
<div id="graphqlResult">
  <div class="text-center py-5" id="gqlLoading">
    <div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>
    <p class="mt-2 text-muted">Fetching data...</p>
  </div>
</div>

<script>
  async function graphqlQuery(query, variables = {}) {
    const response = await fetch('https://rickandmortyapi.com/graphql', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ query, variables })
    });

    const result = await response.json();

    if (result.errors) {
      throw new Error(result.errors.map(e => e.message).join(', '));
    }

    return result.data;
  }

  async function loadCharacters() {
    const container = document.getElementById('graphqlResult');

    try {
      const data = await graphqlQuery(`
        query {
          characters(page: 1) {
            results {
              id
              name
              status
              species
              image
            }
          }
        }
      `);

      container.innerHTML = data.characters.results.map(char => `
        <div class="card mb-3">
          <div class="row g-0">
            <div class="col-4">
              <img src="${char.image}" class="img-fluid rounded-start" alt="${char.name}">
            </div>
            <div class="col-8">
              <div class="card-body">
                <h5 class="card-title">${char.name}</h5>
                <span class="badge ${char.status === 'Alive' ? 'bg-success' : 'bg-danger'}">${char.status}</span>
                <p class="card-text mt-2"><small class="text-muted">${char.species}</small></p>
              </div>
            </div>
          </div>
        </div>`).join('');
    } catch (error) {
      container.innerHTML = `
        <div class="alert alert-danger">
          <i class="bi bi-exclamation-triangle me-2"></i>
          <strong>GraphQL Error:</strong> ${error.message}
          <button class="btn btn-sm btn-outline-danger ms-3" onclick="loadCharacters()">Retry</button>
        </div>`;
    }
  }

  loadCharacters();
</script>
```

## Advanced Variations

### GraphQL with Variables

Pass dynamic parameters to queries using variables.

```html
<div class="mb-3">
  <label class="form-label">Search Characters</label>
  <div class="input-group">
    <input type="text" class="form-control" id="searchName" placeholder="Enter name...">
    <button class="btn btn-primary" onclick="searchCharacters()">Search</button>
  </div>
</div>
<div id="searchResults"></div>

<script>
  async function searchCharacters() {
    const name = document.getElementById('searchName').value;
    const results = document.getElementById('searchResults');

    results.innerHTML = `
      <div class="text-center py-3">
        <div class="spinner-border spinner-border-sm" role="status">
          <span class="visually-hidden">Searching...</span>
        </div>
      </div>`;

    try {
      const data = await graphqlQuery(`
        query($name: String!) {
          characters(filter: { name: $name }) {
            results {
              id
              name
              status
              species
              location { name }
            }
          }
        }
      `, { name });

      if (!data.characters.results?.length) {
        results.innerHTML = '<div class="alert alert-info">No characters found.</div>';
        return;
      }

      results.innerHTML = `
        <ul class="list-group">
          ${data.characters.results.map(c => `
            <li class="list-group-item d-flex justify-content-between align-items-center">
              <div>
                <strong>${c.name}</strong>
                <br><small class="text-muted">${c.species} - ${c.location.name}</small>
              </div>
              <span class="badge ${c.status === 'Alive' ? 'bg-success' : 'bg-secondary'}">${c.status}</span>
            </li>`).join('')}
        </ul>`;
    } catch (error) {
      results.innerHTML = `<div class="alert alert-danger">${error.message}</div>`;
    }
  }
</script>
```

### Reusable Query Component with States

```html
<div id="userCard" class="card">
  <div class="card-body">
    <!-- Loading State -->
    <div id="userLoading" class="placeholder-glow">
      <span class="placeholder col-6 mb-2"></span>
      <span class="placeholder col-8 mb-1"></span>
      <span class="placeholder col-4"></span>
    </div>
    <!-- Data State -->
    <div id="userData" class="d-none">
      <h5 id="userName"></h5>
      <p id="userEmail" class="text-muted"></p>
      <p id="userLocation" class="small"></p>
    </div>
    <!-- Error State -->
    <div id="userError" class="d-none">
      <div class="alert alert-danger mb-0">
        <span id="userErrorMsg"></span>
        <button class="btn btn-sm btn-outline-danger ms-2" onclick="loadUser()">Retry</button>
      </div>
    </div>
  </div>
</div>

<script>
  async function loadUser() {
    document.getElementById('userLoading').classList.remove('d-none');
    document.getElementById('userData').classList.add('d-none');
    document.getElementById('userError').classList.add('d-none');

    try {
      const data = await graphqlQuery(`
        { character(id: 1) { id name status species location { name } } }
      `);

      document.getElementById('userName').textContent = data.character.name;
      document.getElementById('userEmail').textContent = `Status: ${data.character.status} | Species: ${data.character.species}`;
      document.getElementById('userLocation').textContent = `Location: ${data.character.location.name}`;

      document.getElementById('userLoading').classList.add('d-none');
      document.getElementById('userData').classList.remove('d-none');
    } catch (error) {
      document.getElementById('userErrorMsg').textContent = error.message;
      document.getElementById('userLoading').classList.add('d-none');
      document.getElementById('userError').classList.remove('d-none');
    }
  }

  loadUser();
</script>
```

### Query with Pagination

```html
<nav>
  <ul class="pagination" id="gqlPagination"></ul>
</nav>

<script>
  let gqlPage = 1;

  async function loadGQLPage(page) {
    gqlPage = page;
    const data = await graphqlQuery(`
      query($page: Int!) {
        characters(page: $page) {
          info { pages count }
          results { id name status }
        }
      }
    `, { page });

    // Render results
    document.getElementById('gqlBody').innerHTML = data.characters.results.map(c =>
      `<tr><td>${c.id}</td><td>${c.name}</td><td>${c.status}</td></tr>`
    ).join('');

    // Render pagination
    const pagination = document.getElementById('gqlPagination');
    pagination.innerHTML = '';
    for (let i = 1; i <= Math.min(data.characters.info.pages, 10); i++) {
      pagination.innerHTML += `
        <li class="page-item ${i === page ? 'active' : ''}">
          <a class="page-link" href="#" onclick="loadGQLPage(${i}); return false;">${i}</a>
        </li>`;
    }
  }
</script>
```

## Best Practices

1. **Create a reusable `graphqlQuery` function** that handles headers, errors, and response parsing.
2. **Always check `result.errors`** - GraphQL returns 200 even on query errors.
3. **Use variables** instead of string interpolation for dynamic query parameters.
4. **Implement three states**: loading (skeleton), data (content), error (alert with retry).
5. **Use placeholder glow** for skeleton loading to match the expected content structure.
6. **Extract only needed fields** in queries to minimize response size.
7. **Handle empty results** separately from errors - "no data" is not an error.
8. **Cache query results** client-side to avoid redundant network requests.
9. **Use `async/await`** for cleaner error handling with try/catch.
10. **Sanitize GraphQL responses** before rendering to prevent XSS.
11. **Log query errors** with context for debugging in development.
12. **Use named queries** for better debugging and caching in production.

## Common Pitfalls

1. **Not checking `result.errors`** - GraphQL always returns 200, errors are in the response body.
2. **String interpolation for variables** creates injection risks - use the variables object.
3. **Over-fetching fields** defeats GraphQL's purpose of requesting only needed data.
4. **Missing Content-Type header** causes server to reject the request.
5. **Not handling null fields** in response - GraphQL fields can be null without being errors.
6. **Treating GraphQL errors like HTTP errors** - they have different structures.
7. **No loading state** leaves UI blank during queries that take time.
8. **Hardcoding API endpoint** instead of using environment configuration.

## Accessibility Considerations

- Loading states need `role="status"` and descriptive `visually-hidden` text.
- Error messages should use `role="alert"` for immediate screen reader announcement.
- Search inputs need proper labels or `aria-label` attributes.
- Pagination controls require `aria-label` and current page `aria-current="page"`.
- Data tables from GraphQL queries should maintain semantic `<table>` structure.
- Empty states should be announced to screen readers.

## Responsive Behavior

- GraphQL data cards should use Bootstrap responsive grid for layout.
- Search results should stack vertically on mobile using `list-group`.
- Pagination should wrap or use compact display on narrow screens.
- Skeleton loading states should match the responsive layout at each breakpoint.
- Tables from GraphQL data should use `table-responsive` on mobile.
