# Lazy Loading Modules

## 📌 What You'll Learn

- Dynamic import()
- Lazy loading on first use
- Startup time reduction

## 💻 Code Example

```js
// Lazy load heavy module
let heavyModule = null;

app.get('/heavy', async (req, res) => {
  if (!heavyModule) {
    heavyModule = (await import('./heavy-module.js')).default;
  }
  const result = heavyModule.process();
  res.json({ result });
});
```
