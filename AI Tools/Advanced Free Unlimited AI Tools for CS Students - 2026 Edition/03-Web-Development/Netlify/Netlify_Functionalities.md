# Netlify Functionalities

## 1. Deploy Static Site

```bash
# Install Netlify CLI
npm install netlify-cli -g

# Deploy
netlify deploy --prod

# Create production deploy
netlify deploy --prod --dir=dist
```

## 2. Connect GitHub

1. Visit: https://app.netlify.com
2. Click "Add new site" → Import an existing project
3. Choose GitHub
4. Select repository
5. Configure build settings
6. Deploy!

## 3. Form Handling

Add form with no backend:

```html
<form name="contact" method="POST" data-netlify="true">
  <input type="text" name="name" placeholder="Name">
  <input type="email" name="email" placeholder="Email">
  <textarea name="message" placeholder="Message"></textarea>
  <button type="submit">Send</button>
</form>
```

## 4. Netlify Functions

```javascript
// functions/hello.js
exports.handler = async (event, context) => {
  return {
    statusCode: 200,
    body: JSON.stringify({ message: 'Hello from Netlify!' })
  }
}
```

## 5. Environment Variables

```bash
# Set via CLI
netlify env:set DATABASE_URL "your-database-url"

# Access in code
const dbUrl = process.env.DATABASE_URL
```

---

*Back to [Web Development README](../README.md)*
*Back to [Main README../../README.md)*