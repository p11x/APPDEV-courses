# Generate Metadata Function

## Dynamic Metadata

```typescript
// src/app/products/[id]/page.tsx
interface Props {
  params: Promise<{ id: string }>;
}

export async function generateMetadata({ params }: Props) {
  const { id } = await params;
  const product = await getProduct(id);
  
  return {
    title: `${product.name} - My Store`,
    description: product.description,
  };
}
```

This function generates metadata at request time for dynamic routes.
