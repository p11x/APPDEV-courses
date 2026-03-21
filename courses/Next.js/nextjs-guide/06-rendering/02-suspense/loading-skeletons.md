# Loading Skeletons

## What You'll Learn
- Creating skeleton loading UI

## Example

```typescript
// src/components/PostSkeleton.tsx
export function PostSkeleton() {
  return (
    <div style={{ padding: "1rem" }}>
      <div style={{ 
        height: "20px", 
        width: "60%", 
        backgroundColor: "#ddd",
        marginBottom: "0.5rem" 
      }} />
      <div style={{ 
        height: "16px", 
        width: "90%", 
        backgroundColor: "#ddd" 
      }} />
    </div>
  );
}
```

Use in Suspense fallback for better UX!
