# ♿ DOM Accessibility Best Practices

## 📋 Overview

Building accessible web applications is essential. This guide covers techniques for making your DOM manipulations accessible to all users, including those using assistive technologies.

---

## 🎯 Why Accessibility Matters

- **Legal requirements** in many jurisdictions
- **Larger audience** - 15% of world population has some disability
- **Better UX** for everyone
- **SEO benefits** - search engines can better index content

---

## 🎯 ARIA Attributes

### Common ARIA Attributes

```javascript
// Role attributes
element.setAttribute('role', 'button');
element.setAttribute('role', 'checkbox');
element.setAttribute('role', 'dialog');

// State attributes
element.setAttribute('aria-checked', 'true');
element.setAttribute('aria-expanded', 'false');
element.setAttribute('aria-disabled', 'true');
element.setAttribute('aria-hidden', 'true');

// Label attributes
element.setAttribute('aria-label', 'Close dialog');
element.setAttribute('aria-describedby', 'description-id');
```

### ARIA Live Regions

```javascript
// Announce dynamic content to screen readers
const announcer = document.createElement('div');
announcer.setAttribute('role', 'status');
announcer.setAttribute('aria-live', 'polite');
announcer.className = 'sr-only';
document.body.appendChild(announcer);

// Announce updates
announcer.textContent = 'Form submitted successfully';
```

---

## 🎯 Keyboard Accessibility

### Focus Management

```javascript
// Focus first element in modal
function openModal(modal) {
    modal.style.display = 'block';
    const firstFocusable = modal.querySelector(
        'button, [href], input, select, textarea, [tabindex]:not([tabindex="-1"])'
    );
    firstFocusable.focus();
    
    // Trap focus
    modal.addEventListener('keydown', trapFocus);
}

// Handle Escape to close
document.addEventListener('keydown', (e) => {
    if (e.key === 'Escape' && modal.style.display === 'block') {
        closeModal(modal);
    }
});
```

### tabindex

```javascript
// Make element focusable
element.setAttribute('tabindex', '0');

// Remove from tab order
element.setAttribute('tabindex', '-1');

// Custom tab order (avoid!)
element.setAttribute('tabindex', '5');
```

---

## 🎯 Semantic HTML

```javascript
// ❌ Bad: Generic elements
<div onclick="submit()">Submit</div>
<div onkeypress="submit()">Submit</div>

// ✅ Good: Semantic elements
<button type="submit">Submit</button>

// ✅ Better with accessibility
<button type="submit" aria-describedby="submit-help">
    Submit
</button>
<span id="submit-help" class="sr-only">
    Click to submit the form
</span>
```

---

## 🎯 Screen Reader Best Practices

### Hiding Content

```javascript
// Visually hidden but accessible to screen readers
const srOnly = document.createElement('style');
srOnly.textContent = `
    .sr-only {
        position: absolute;
        width: 1px;
        height: 1px;
        padding: 0;
        margin: -1px;
        overflow: hidden;
        clip: rect(0, 0, 0, 0);
        border: 0;
    }
`;
document.head.appendChild(srOnly);

// Use for labels, status messages
const label = document.createElement('span');
label.className = 'sr-only';
label.textContent = 'Form field required';
```

### Dynamic Content Announcements

```javascript
class AccessibilityAnnouncer {
    constructor() {
        this.region = document.createElement('div');
        this.region.setAttribute('role', 'status');
        this.region.setAttribute('aria-live', 'polite');
        this.region.className = 'sr-only';
        document.body.appendChild(this.region);
    }
    
    announce(message) {
        this.region.textContent = '';
        setTimeout(() => {
            this.region.textContent = message;
        }, 100);
    }
}
```

---

## 🎯 Form Accessibility

```javascript
function createAccessibleInput(config) {
    const wrapper = document.createElement('div');
    
    // Label
    const label = document.createElement('label');
    label.setAttribute('for', config.id);
    label.textContent = config.label;
    if (config.required) {
        label.innerHTML += ' <span aria-hidden="true">*</span>';
    }
    
    // Input
    const input = document.createElement('input');
    input.id = config.id;
    input.type = config.type || 'text';
    input.required = config.required;
    input.setAttribute('aria-required', config.required ? 'true' : 'false');
    
    if (config.describedBy) {
        input.setAttribute('aria-describedby', config.describedBy);
    }
    
    // Error message
    if (config.errorId) {
        const error = document.createElement('span');
        error.id = config.errorId;
        error.className = 'error-message sr-only';
        error.setAttribute('role', 'alert');
        wrapper.appendChild(error);
    }
    
    wrapper.appendChild(label);
    wrapper.appendChild(input);
    if (config.errorId) wrapper.appendChild(error);
    
    return wrapper;
}
```

---

## 🔗 Related Topics

- [06_Event_Handling_Deep_Dive.md](./06_Event_Handling_Deep_Dive.md)
- [11_DOM_Performance_Optimization.md](./11_DOM_Performance_Optimization.md)

---

**DOM Module Complete!** 🎉

Now you're ready to build real-world projects!