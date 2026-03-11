# Vercel Functionalities

## 1. Deploy Static Site

```bash
# Install Vercel CLI
npm i -g vercel

# Deploy current directory
vercel

# Deploy with options
vercel --prod
```

## 2. Connect GitHub Repository

1. Visit: https://vercel.com
2. Click "Add New..." → Project
3. Import GitHub repository
4. Configure build settings
5. Deploy!

## 3. API Route (Serverless)

```javascript
// pages/api/hello.js (Pages Router)
// or app/api/hello/route.js (App Router)

export default function handler(req, res) {
  res.status(200).json({
    message: 'Hello from Vercel!'
  })
}
```

## 4. Environment Variables

```bash
# Set via CLI
vercel env add DATABASE_URL

# Access in code
const dbUrl = process.env.DATABASE_URL
```

## 5. Preview Deployments

Automatic for every PR:

| Trigger | URL Format |
|---------|------------|
| Pull Request | `project-git-branch.vercel.app` |
| Production | `project.vercel.app` |

## 6. Custom Domain

```bash
# Add domain
vercel domains add mydomain.com

# Configure DNS
# Add CNAME: www -> cname.vercel-dns.com
```

---

*Back to [Web Development README](../README.md)*
*Back to [Main README](../../README.md)*