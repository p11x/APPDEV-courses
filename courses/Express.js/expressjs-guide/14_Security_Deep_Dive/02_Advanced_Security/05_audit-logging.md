# Audit Logging

## 📌 What You'll Learn

- Logging who did what when
- Tamper-evident logs
- Compliance requirements

## 💻 Code Example

```js
// Audit log function
function audit(action, user, details) {
  const entry = {
    timestamp: new Date().toISOString(),
    action,
    user,
    details,
    // Hash for integrity
    hash: crypto.createHash('sha256')
      .update(JSON.stringify({ action, user, details }))
      .digest('hex')
  };
  
  // Append to audit log (immutable storage)
  auditLog.push(entry);
}

// Use in routes
app.delete('/api/users/:id', (req, res) => {
  audit('delete_user', req.user?.id, { targetUser: req.params.id });
});
```
