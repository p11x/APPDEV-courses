# Container Presenter at Scale

## Overview
The Container/Presenter pattern separates data fetching and business logic (containers) from UI rendering (presenters). At scale, this pattern helps maintain clean separation of concerns and improves testability.

## Prerequisites
- React hooks
- Component patterns

## Core Concepts

### Container Component

```tsx
// [File: src/features/products/containers/ProductListContainer.tsx]
'use client';

import { useState, useEffect } from 'react';
import { ProductListPresenter } from '../presenters/ProductListPresenter';
import { fetchProducts } from '../api/productsApi';

export function ProductListContainer() {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<Error | null>(null);
  
  useEffect(() => {
    fetchProducts()
      .then(setProducts)
      .catch(setError)
      .finally(() => setLoading(false));
  }, []);
  
  return (
    <ProductListPresenter 
      products={products}
      loading={loading}
      error={error}
    />
  );
}
```

### Presenter Component

```tsx
// [File: src/features/products/presenters/ProductListPresenter.tsx]
import { ProductCard } from '@/shared/components';

interface ProductListPresenterProps {
  products: Product[];
  loading: boolean;
  error: Error | null;
}

export function ProductListPresenter({ 
  products, 
  loading, 
  error 
}: ProductListPresenterProps) {
  if (loading) return <Spinner />;
  if (error) return <ErrorMessage error={error} />;
  
  return (
    <div className="product-grid">
      {products.map(product => (
        <ProductCard key={product.id} product={product} />
      ))}
    </div>
  );
}
```

## Key Takeaways
- Containers handle data fetching and state
- Presenters handle UI rendering
- Improves testability and separation of concerns

## What's Next
Continue to [Repository Pattern](02-repository-pattern-for-api-calls.md) to learn about organizing API calls.