# Query Parameters Handling in React Router v6

## Overview
Query parameters allow passing data via URL without creating new routes. They're useful for filtering, sorting, and pagination.

## Core Concepts

### Using useSearchParams Hook

```jsx
// File: src/components/SearchableList.jsx

import { useSearchParams } from 'react-router-dom';

function SearchableList() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const query = searchParams.get('q') || '';
  const page = parseInt(searchParams.get('page')) || 1;
  const sort = searchParams.get('sort') || 'newest';

  const handleSearch = (q) => {
    const params = new URLSearchParams(searchParams);
    if (q) {
      params.set('q', q);
    } else {
      params.delete('q');
    }
    params.set('page', '1'); // Reset to page 1 on search
    setSearchParams(params);
  };

  const handlePageChange = (newPage) => {
    const params = new URLSearchParams(searchParams);
    params.set('page', newPage.toString());
    setSearchParams(params);
  };

  return (
    <div>
      <input
        value={query}
        onChange={(e) => handleSearch(e.target.value)}
        placeholder="Search..."
      />
      <div>
        Page: {page}
        <button onClick={() => handlePageChange(page - 1)}>Prev</button>
        <button onClick={() => handlePageChange(page + 1)}>Next</button>
      </div>
      <div>Sort: {sort}</div>
    </div>
  );
}
```

## Key Takeaways
- Use useSearchParams hook to read/write query params
- Use URLSearchParams for easy manipulation
- Query params don't affect route matching

## What's Next
This concludes the routing section. Continue to [CSS Modules Setup](../08-styling/01-css-modules/01-css-modules-setup.md)
