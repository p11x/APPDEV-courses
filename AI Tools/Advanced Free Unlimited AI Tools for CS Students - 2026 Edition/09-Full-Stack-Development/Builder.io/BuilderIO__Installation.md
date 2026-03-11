# Builder.io - Installation Guide

## Web-Based Access

Builder.io is primarily a web-based platform - no installation required.

### Step-by-Step Access:

1. **Visit the Website**
   - Go to: https://builder.io

2. **Create Account**
   - Click "Sign Up Free"
   - Use GitHub, Google, or email
   - Verify your email

3. **Start Building**
   - New Project → Choose template or start blank
   - Use AI or visual editor

---

## VSCode Extension (Optional)

For developers who prefer local development:

1. **Open VSCode Extensions**
   - Press `Ctrl+Shift+X` (Windows) or `Cmd+Shift+X` (Mac)

2. **Search for Builder.io**
   - Search "Builder.io" in marketplace

3. **Install Extension**
   - Click "Install" button
   - Restart VSCode

---

## npm Package (For Developers)

Install Builder.io SDK for custom implementations:

```bash
# Install Builder.io SDK
npm install @builder.io/sdk-react

# Install Builder.io for Vue
npm install @builder.io/sdk-vue
```

---

## Quick Start Template

```jsx
// Example: Using Builder.io React SDK
import { Content } from '@builder.io/sdk-react';

function App() {
  return (
    <Content
      model="page"
      apiKey="YOUR_API_KEY"
    />
  );
}
```

---

## Related Documentation

- [Description](./BuilderIO__Description.md) - Overview
- [Features](./BuilderIO__Features.md) - Key capabilities
- [Functionalities](./BuilderIO__Functionalities.md) - How it works
- [Requirements](./BuilderIO__Requirements.md) - System requirements
- [UseCases](./BuilderIO__UseCases.md) - Student use cases

---

*Back to [09-Full-Stack-Development README](../README.md)*
*Back to [Main README](../../README.md)*