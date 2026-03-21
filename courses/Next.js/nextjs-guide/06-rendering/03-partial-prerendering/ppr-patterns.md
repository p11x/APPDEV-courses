# PPR Patterns

## Common Patterns

```typescript
// Static shell + dynamic content
export default function Page() {
  return (
    <div>
      <Navigation />        {/* Static */}
      <Suspense fallback={<FeedSkeleton />}>
        <Feed />          {/* Dynamic */}
      </Suspense>
      <Footer />           {/* Static */}
    </div>
  );
}
```

```typescript
// E-commerce product page
export default function ProductPage({ params }) {
  return (
    <>
      <ProductHeader />           {/* Static */}
      <Suspense fallback={<Loading />}>
        <ProductReviews id={params.id} />  {/* Dynamic */}
      </Suspense>
      <RelatedProducts />          {/* Static */}
    </>
  );
}
```
