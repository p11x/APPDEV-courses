# Translation Files

## What You'll Learn
- Creating and organizing translation files
- Using namespaces to organize translations
- Handling variables in translations
- Best practices for translation structure

## Prerequisites
- Understanding of i18n concepts
- Basic JSON knowledge
- Familiarity with next-intl setup

## Concept Explained Simply

Translation files are like dictionaries — they contain all the text in your app mapped to keys. Instead of writing "Welcome" in your code, you write a key like `home.title`, and the translation file contains both English and Spanish versions.

Organizing these files well is crucial as your app grows. You don't want one giant file with 500 translations — you want them grouped logically so translators (and you) can find what they need.

## Complete Code Example

### Basic Translation File Structure

```json
// messages/en.json
{
  "common": {
    "welcome": "Welcome",
    "logout": "Log out",
    "login": "Log in",
    "save": "Save",
    "cancel": "Cancel",
    "delete": "Delete",
    "edit": "Edit"
  },
  "home": {
    "title": "Welcome to Our App",
    "subtitle": "The best solution for your needs",
    "cta": "Get Started"
  },
  "products": {
    "title": "Our Products",
    "filter": "Filter by",
    "sort": "Sort by",
    "price": "Price",
    "addToCart": "Add to Cart",
    "outOfStock": "Out of Stock"
  }
}
```

```json
// messages/es.json
{
  "common": {
    "welcome": "Bienvenido",
    "logout": "Cerrar sesión",
    "login": "Iniciar sesión",
    "save": "Guardar",
    "cancel": "Cancelar",
    "delete": "Eliminar",
    "edit": "Editar"
  },
  "home": {
    "title": "Bienvenido a Nuestra App",
    "subtitle": "La mejor solución para tus necesidades",
    "cta": "Comenzar"
  },
  "products": {
    "title": "Nuestros Productos",
    "filter": "Filtrar por",
    "sort": "Ordenar por",
    "price": "Precio",
    "addToCart": "Añadir al Carrito",
    "outOfStock": "Agotado"
  }
}
```

### Using Namespaces

Namespaces are like folders in your dictionary — they group related translations together. Instead of one big file, you can split into multiple files:

```typescript
// lib/i18n.ts - Using namespace files
import { getRequestConfig } from "next-intl/server";

export default getRequestConfig(async ({ locale }) => ({
  messages: {
    // Import each namespace separately
    common: (await import("../messages/common.json")).default,
    home: (await import("../messages/home.json")).default,
    products: (await import("../messages/products.json")).default,
    cart: (await import("../messages/cart.json")).default,
    auth: (await import("../messages/auth.json")).default,
  }
}));

// messages/common.json
{
  "welcome": "Welcome",
  "logout": "Log out"
}

// messages/home.json
{
  "title": "Welcome to Our App",
  "subtitle": "The best solution"
}
```

### Variables in Translations

```typescript
// Translation file with variables
{
  "greeting": "Hello, {name}!",
  "items": "You have {count, plural, 
    =0 {no items}
    one {1 item}
    other {# items}
  }",
  "price": "The price is {price, number, USD}",
  "date": "Posted on {date, date, long}"
}

// Usage in component
const t = await getTranslations("home");

// Simple variable
t("greeting", { name: "Alice" }); // "Hello, Alice!"

// Pluralization
t("items", { count: 5 }); // "You have 5 items"

// Number formatting
t("price", { price: 99.99 }); // "The price is 99.99"

// Date formatting
t("date", { date: new Date() }); // "Posted on January 1, 2025"
```

### Complete Example with All Features

```json
// messages/en.json
{
  "auth": {
    "login": {
      "title": "Welcome back",
      "email": "Email address",
      "password": "Password",
      "submit": "Sign in",
      "forgotPassword": "Forgot password?",
      "errors": {
        "invalidEmail": "Please enter a valid email",
        "requiredField": "This field is required",
        "wrongCredentials": "Invalid email or password"
      }
    },
    "signup": {
      "title": "Create an account",
      "terms": "I agree to the {terms} and {privacy}",
      "termsLink": "Terms of Service",
      "privacyLink": "Privacy Policy"
    }
  },
  "cart": {
    "items": "{count, plural, =0 {Your cart is empty} one {1 item in cart} other {# items in cart}}",
    "total": "Total: {total, number, currency}",
    "checkout": "Proceed to Checkout"
  }
}
```

## Common Mistakes

### Mistake 1: Inconsistent Keys Across Languages

```typescript
// WRONG - Different keys in different files
// en.json
{ "home.title": "Welcome" }

// es.json  
{ "inicio.titulo": "Bienvenido" } // Different key!

// CORRECT - Same keys in all files
// en.json
{ "home.title": "Welcome" }

// es.json
{ "home.title": "Bienvenido" }
```

### Mistake 2: Missing Fallback Translations

```typescript
// WRONG - No fallback when key missing
// If translation is missing, returns the key itself

// CORRECT - Use fallback in component
const t = await getTranslations("home");
t("title", { fallback: "Welcome" });
```

### Mistake 3: Not Using Namespaces

```typescript
// WRONG - Flat structure gets messy
{
  "welcome": "...",
  "login": "...",
  "products": "...",
  "productDetail": "...",
  "cart": "...",
  "cartItem": "..."
  // Gets unmanageable!
}

// CORRECT - Use namespaces
{
  "common": { "welcome": "..." },
  "auth": { "login": "..." },
  "products": { 
    "list": "...",
    "detail": "..." 
  },
  "cart": { "items": "..." }
}
```

## Best Practices

1. **Use descriptive keys**: `auth.login.title` is better than `lt`
2. **Group by feature**: Namespaces should match your app's sections
3. **Keep files flat**: Nested objects more than 2-3 levels get confusing
4. **Add comments**: Help translators understand context
5. **Use TypeScript**: Add type safety for translation keys

### Type-Safe Translations

```typescript
// messages.d.ts
import "next-intl";

declare module "next-intl" {
  interface Messages {
    common: {
      welcome: string;
      logout: string;
    };
    home: {
      title: string;
      subtitle: string;
    };
    auth: {
      login: {
        title: string;
        email: string;
      };
    };
  }
}
```

## Summary

- Translation files use JSON format with key-value pairs
- Use dot notation for nesting: `auth.login.title`
- Namespaces group related translations
- Use variables with `{name}` syntax
- Handle plurals with the `plural` option
- Keep keys consistent across all language files
- Consider TypeScript for type-safe keys

## Next Steps

- [useTranslations-hook.md](./useTranslations-hook.md) - Using translations in components
- [server-side-translations.md](../03-advanced-i18n/server-side-translations.md) - Server-side translation patterns
