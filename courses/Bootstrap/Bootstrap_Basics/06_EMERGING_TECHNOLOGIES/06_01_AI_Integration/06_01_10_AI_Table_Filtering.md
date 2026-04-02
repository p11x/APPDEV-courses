---
title: "AI Table Filtering"
slug: "ai-table-filtering"
difficulty: 2
tags: ["bootstrap", "ai", "tables", "filtering", "search"]
prerequisites:
  - "03_03_Data_Tables"
  - "06_01_09_AI_Form_Autofill"
related:
  - "06_01_11_AI_Content_Summary"
  - "06_01_13_AI_Dashboard_Insights"
duration: "30 minutes"
---

# AI Table Filtering

## Overview

AI table filtering replaces complex filter UIs with natural language queries. Instead of building multi-column filter panels, users type queries like "show orders over $500 from last month" and the AI translates this into structured filter conditions. Bootstrap tables provide the visual structure, while an AI backend parses intent and returns filter parameters. This dramatically improves data exploration workflows, especially for non-technical users who struggle with traditional filter builders.

## Basic Implementation

A natural language search bar above a Bootstrap table that translates queries into filter logic.

```html
<div class="container mt-4">
  <div class="card">
    <div class="card-header">
      <div class="input-group">
        <span class="input-group-text"><i class="bi bi-search"></i></span>
        <input type="text" class="form-control" id="nlQuery"
               placeholder="Filter with natural language (e.g., 'active users from California')">
        <button class="btn btn-primary" type="button" id="searchBtn">Search</button>
      </div>
      <div id="activeFilters" class="mt-2"></div>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-hover" id="dataTable">
          <thead class="table-light">
            <tr>
              <th>Name</th>
              <th>Email</th>
              <th>Status</th>
              <th>State</th>
              <th>Revenue</th>
            </tr>
          </thead>
          <tbody id="tableBody"></tbody>
        </table>
      </div>
    </div>
  </div>
</div>

<script>
document.getElementById('searchBtn').addEventListener('click', async () => {
  const query = document.getElementById('nlQuery').value;
  if (!query) return;

  const res = await fetch('/api/ai/filter', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ query, dataset: 'users' })
  });
  const result = await res.json();

  renderFilters(result.filters);
  renderTable(result.data);
});

function renderFilters(filters) {
  const container = document.getElementById('activeFilters');
  container.innerHTML = filters.map(f => `
    <span class="badge bg-secondary me-1">
      ${f.field}: ${f.operator} ${f.value}
      <button type="button" class="btn-close btn-close-white ms-1"
              aria-label="Remove filter" data-filter="${f.id}"></button>
    </span>
  `).join('');
}
</script>
```

The backend parses the natural language query and returns structured filter objects with field, operator, and value.

## Advanced Variations

### Query Explanation with Visual Builder

Show users how the AI interpreted their query with an editable filter summary.

```html
<div id="queryExplanation" class="alert alert-info d-none">
  <strong><i class="bi bi-lightbulb"></i> AI interpreted:</strong>
  <span id="explanationText"></span>
  <button class="btn btn-sm btn-outline-primary ms-2" id="editFilters">Edit Filters</button>
</div>

<div id="filterEditor" class="d-none">
  <div class="row g-2 mb-3" id="filterRows"></div>
  <button class="btn btn-sm btn-success" id="applyFilters">Apply</button>
  <button class="btn btn-sm btn-secondary" id="cancelEdit">Cancel</button>
</div>

<script>
function showExplanation(filters, explanation) {
  document.getElementById('explanationText').textContent = explanation;
  document.getElementById('queryExplanation').classList.remove('d-none');
  buildFilterEditor(filters);
}

function buildFilterEditor(filters) {
  const container = document.getElementById('filterRows');
  container.innerHTML = filters.map((f, i) => `
    <div class="col-md-3">
      <select class="form-select form-select-sm" data-idx="${i}" data-prop="field">
        <option ${f.field === 'name' ? 'selected' : ''}>name</option>
        <option ${f.field === 'status' ? 'selected' : ''}>status</option>
        <option ${f.field === 'revenue' ? 'selected' : ''}>revenue</option>
        <option ${f.field === 'state' ? 'selected' : ''}>state</option>
      </select>
    </div>
    <div class="col-md-2">
      <select class="form-select form-select-sm" data-idx="${i}" data-prop="operator">
        <option ${f.operator === '=' ? 'selected' : ''}>=</option>
        <option ${f.operator === '>' ? 'selected' : ''}>></option>
        <option ${f.operator === '<' ? 'selected' : ''}><</option>
        <option ${f.operator === 'contains' ? 'selected' : ''}>contains</option>
      </select>
    </div>
    <div class="col-md-3">
      <input type="text" class="form-control form-control-sm" value="${f.value}"
             data-idx="${i}" data-prop="value">
    </div>
  `).join('');
}

document.getElementById('editFilters').addEventListener('click', () => {
  document.getElementById('filterEditor').classList.toggle('d-none');
});
</script>
```

### Streaming Filter Results

Process large datasets with streaming results showing rows as they match.

```html
<div id="streamStatus" class="mb-2 text-muted small d-none">
  <span class="spinner-border spinner-border-sm"></span>
  Filtering <span id="processedCount">0</span> of <span id="totalCount">0</span> rows...
</div>

<script>
async function streamFilter(query) {
  const status = document.getElementById('streamStatus');
  status.classList.remove('d-none');
  const tableBody = document.getElementById('tableBody');
  tableBody.innerHTML = '';

  const res = await fetch('/api/ai/filter-stream', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ query })
  });

  const reader = res.body.getReader();
  const decoder = new TextDecoder();
  let buffer = '';

  while (true) {
    const { done, value } = await reader.read();
    if (done) break;
    buffer += decoder.decode(value, { stream: true });
    const lines = buffer.split('\n');
    buffer = lines.pop();

    for (const line of lines) {
      if (!line.trim()) continue;
      const row = JSON.parse(line);
      tableBody.insertAdjacentHTML('beforeend', renderRow(row));
      document.getElementById('processedCount').textContent =
        tableBody.children.length;
    }
  }
  status.classList.add('d-none');
}
</script>
```

### Suggested Queries

Offer AI-generated query suggestions based on the dataset and user history.

```html
<div class="mb-3">
  <span class="text-muted small">Suggestions:</span>
  <div id="suggestions" class="mt-1">
    <button class="btn btn-sm btn-outline-secondary me-1 mb-1 suggestion-chip">
      Show high-value customers
    </button>
    <button class="btn btn-sm btn-outline-secondary me-1 mb-1 suggestion-chip">
      Recent signups this week
    </button>
    <button class="btn btn-sm btn-outline-secondary me-1 mb-1 suggestion-chip">
      Inactive accounts older than 90 days
    </button>
  </div>
</div>

<script>
document.querySelectorAll('.suggestion-chip').forEach(btn => {
  btn.addEventListener('click', () => {
    document.getElementById('nlQuery').value = btn.textContent.trim();
    document.getElementById('searchBtn').click();
  });
});
</script>
```

## Best Practices

1. Always show the interpreted query so users understand what filters were applied
2. Allow manual editing of AI-generated filters for correction and refinement
3. Use debouncing on real-time query input to prevent excessive API calls
4. Cache parsed query results to speed up repeated identical queries
5. Provide fallback to traditional column filters when AI is unavailable
6. Show row count before and after filtering to communicate filter impact
7. Include "clear all filters" functionality prominently
8. Support query history so users can recall previous natural language searches
9. Validate AI-returned filter parameters against allowed fields and operators
10. Use skeleton loaders in table body during query processing
11. Log failed query interpretations to improve the NLP model
12. Limit result sets with pagination even when AI returns many matches
13. Sanitize all filter values to prevent injection attacks in client-side filtering
14. Provide keyboard shortcut (Ctrl+K) to focus the query input

## Common Pitfalls

1. **No query explanation**: Users do not understand why certain rows were filtered out
2. **Uneditable filters**: AI output treated as final with no manual adjustment capability
3. **Slow response times**: Queries taking over 2 seconds feel broken for interactive filtering
4. **No fallback**: Application becomes unusable when the AI service is down
5. **Over-filtering**: AI applying too many conditions and returning zero results without notice
6. **Ignoring context**: Not considering the current dataset schema when parsing queries
7. **Security gaps**: Allowing AI filters to expose data the user should not access

## Accessibility Considerations

Use `role="status"` on the filter results count to announce changes to screen readers. Ensure the filter input has a visible label and `aria-label` for assistive technology. Make filter badges removable via keyboard with `tabindex="0"` and Enter/Space handlers. Announce query interpretation results using `aria-live="polite"` regions. Provide a skip link to jump from the search input directly to the filtered results table. Ensure table rows maintain proper `<th>` scope attributes after dynamic filtering.

```html
<div role="status" aria-live="polite" class="visually-hidden" id="filterAnnounce"></div>
<script>
function announceFilter(count, query) {
  document.getElementById('filterAnnounce').textContent =
    `Found ${count} results for: ${query}`;
}
</script>
```

## Responsive Behavior

On mobile, collapse the natural language input to a full-width search bar with an expandable filter summary. Stack filter badges vertically on small screens. Use `table-responsive` to enable horizontal scrolling on the data table. Reduce visible columns to 3 on mobile, hiding revenue and state columns with `d-none d-md-table-cell`. On tablets, show the query input and suggestions in a collapsible panel above the table. Use `d-grid gap-2` for suggestion chips on mobile for full-width touch targets.
