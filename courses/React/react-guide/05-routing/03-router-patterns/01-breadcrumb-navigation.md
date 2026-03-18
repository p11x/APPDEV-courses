# Breadcrumb Navigation

## Overview

Breadcrumbs show users their current location in the site hierarchy and provide easy navigation back to parent pages. They're essential for large, hierarchical applications.

## Implementation

```jsx
import { Link, useLocation } from 'react-router-dom';

function Breadcrumb() {
  const location = useLocation();
  const paths = location.pathname.split('/').filter(Boolean);
  
  return (
    <nav>
      <Link to="/">Home</Link>
      {paths.map((path, index) => {
        const to = `/${paths.slice(0, index + 1).join('/')}`;
        return (
          <span key={to}>
            {' > '}
            <Link to={to}>{path}</Link>
          </span>
        );
      })}
    </nav>
  );
}
```

## Key Takeaways

- Use useLocation to get current path
- Split and map path segments
- Link to parent routes

## What's Next

Let's explore scroll restoration.
