# Streaming Large Datasets

## 📌 What You'll Learn

- Node.js streams
- Piping database cursors to response
- Backpressure handling

## 💻 Code Example

```js
app.get('/api/export', (req, res) => {
  res.setHeader('Content-Type', 'text/csv');
  res.setHeader('Content-Disposition', 'attachment; filename=data.csv');
  
  const stream = db.query('SELECT * FROM users').stream();
  
  stream.pipe(res);
  
  stream.on('error', () => res.end());
});
```
