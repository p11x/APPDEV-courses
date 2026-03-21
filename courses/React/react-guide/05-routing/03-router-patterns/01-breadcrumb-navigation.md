# Breadcrumb Navigation in React Router v6

## Overview
Breadcrumbs show users their current location in the site hierarchy and provide easy navigation back to parent pages. They're essential for deep navigation structures.

## Core Concepts

### Creating Breadcrumbs

```jsx
// File: src/components/Breadcrumbs.jsx

import { Link, useLocation } from 'react-router-dom';

function Breadcrumbs() {
  const location = useLocation();
  
  // Build breadcrumb paths
  const paths = location.pathname.split('/').filter(Boolean);
  
  return (
    <nav aria-label="Breadcrumb">
      <ol>
        <li><Link to="/">Home</Link></li>
        {paths.map((path, index) => {
          const to = `/${paths.slice(0, index + 1).join('/')}`;
          const label = path.charAt(0).toUpperCase() + path.slice(1);
          const isLast = index === paths.length - 1;
          
          return (
            <li key={to}>
              {isLast ? (
                <span>{label}</span>
              ) : (
                <Link to={to}>{label}</Link>
              )}
            </li>
          );
        })}
      </ol>
    </nav>
  );
}
```

## Key Takeaways
- Use location.pathname to build breadcrumbs
- Create links for all but the last item
- Use semantic HTML (ol, li)
