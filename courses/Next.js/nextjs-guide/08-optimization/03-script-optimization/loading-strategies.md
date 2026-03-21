# Script Loading Strategies

## After Interactive

```typescript
<Script 
  src="https://analytics.com/script.js"
  strategy="afterInteractive" 
/>
```

## Lazy On Load

```typescript
<Script 
  src="https://ads.com/script.js"
  strategy="lazyOnload" 
/>
```

## With onLoad Callback

```typescript
<Script 
  src="https://example.com/script.js"
  strategy="afterInteractive"
  onLoad={() => {
    console.log("Script loaded!");
  }}
/>
```
