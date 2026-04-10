# 🔒 JavaScript Security Complete Guide

## Secure Development Practices

---

## Table of Contents

1. [XSS Prevention](#xss-prevention)
2. [CSRF Protection](#csrf-protection)
3. [Input Validation](#input-validation)
4. [Secure Storage](#secure-storage)
5. [Content Security Policy](#content-security-policy)

---

## XSS Prevention

### Escaping Output

```javascript
function escapeHtml(text) {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}

const safe = escapeHtml(userInput);
// <script>alert('xss')</script> becomes &lt;script&gt;alert('xss')&lt;/script&gt;
```

### Using DOM

```javascript
// ❌ Dangerous
element.innerHTML = userInput;

// ✅ Safe
element.textContent = userInput;
```

---

## Input Validation

### Basic Validation

```javascript
function validateEmail(email) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

function validateNumber(value) {
  return !isNaN(parseFloat(value)) && isFinite(value);
}
```

---

## Secure Storage

### HTTPOnly Cookies

```javascript
document.cookie = 'token=abc123; HttpOnly; Secure; SameSite=Strict';
```

### sessionStorage

```javascript
sessionStorage.setItem('key', 'value');
// Auto-cleared on tab close
```

---

## Summary

### Key Takeaways

1. **XSS**: Escape output
2. **Validation**: Sanitize input
3. **CSP**: Restrict resources

### Next Steps

- Continue with: [04_JAVASCRIPT_LEARNING_PATH.md](04_JAVASCRIPT_LEARNING_PATH.md)
- Study OWASP guidelines
- Implement security headers

---

## Cross-References

- **Previous**: [02_JAVASCRIPT_DEBUGGING_MASTER.md](02_JAVASCRIPT_DEBUGGING_MASTER.md)
- **Next**: [04_JAVASCRIPT_LEARNING_PATH.md](04_JAVASCRIPT_LEARNING_PATH.md)

---

*Last updated: 2024*