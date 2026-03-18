# Query Params Handling

## Overview

Query parameters are URL parameters after the ? symbol. They're used for filtering, sorting, pagination, and other client-side state that should be shareable via URL.

## Implementation

```jsx
import { useSearchParams } from 'react-router-dom';

function ProductList() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  const category = searchParams.get('category') || 'all';
  const sort = searchParams.get('sort') || 'name';
  
  const setCategory = (cat) => {
    setSearchParams(prev => {
      prev.set('category', cat);
      return prev;
    });
  };
  
  return (
    <div>
      <select value={category} onChange={e => setCategory(e.target.value)}>
        <option value="all">All</option>
        <option value="electronics">Electronics</option>
      </select>
    </div>
  );
}
```

## Key Takeaways

- useSearchParams hook manages query params
- get() reads parameters
- set() updates parameters
- URL remains shareable
