# Next.js vs React: What's the Difference?

## What You'll Learn
- The fundamental differences between React and Next.js
- When to use React alone vs. Next.js
- Why you might choose one over the other

## Prerequisites
- Basic understanding of React components
- Familiarity with JavaScript and JSX

## Concept Explained Simply

This is one of the most common questions beginners ask, and it makes total sense! Let's clear up the confusion once and for all.

**React** is a library for building user interfaces. Think of it like a toolbox — it gives you components, state management, and hooks, but that's it. You're responsible for figuring out everything else: how to route between pages, how to render your app on the server, how to optimize images, and how to deploy.

**Next.js** is a framework built on top of React. It's like a pre-built workshop that includes the toolbox (React) plus all the power tools, workbench, and instructions. Next.js takes React and adds everything you need to ship a production-ready website.

Here's an analogy: If you wanted to build a house, React is like getting lumber and nails delivered — you have the raw materials but need to figure out everything else. Next.js is like getting a pre-fabricated home kit with blueprints, instructions, and all materials included.

## Key Differences

| Feature | React | Next.js |
|---------|-------|---------|
| Routing | Need to install `react-router` | Built-in file-based routing |
| Server Rendering | Need to set up manually | Built-in SSR and SSG |
| Image Optimization | Need third-party library | Built-in `next/image` |
| Font Optimization | Need third-party library | Built-in `next/font` |
| Deployment | Need to configure | One-click deploy to Vercel |
| SEO | Need to configure manually | Automatic metadata handling |

## When to Use Each

**Use React alone when:**
- Building a simple single-page application
- You need maximum flexibility in your setup
- You're embedding React into an existing website
- You have very specific build requirements that Next.js can't handle

**Use Next.js when:**
- Building any website that will be publicly accessed
- SEO matters to you
- You want fast page loads
- You want the easiest deployment experience
- You want built-in optimizations without extra work

## The Bottom Line

Next.js is React plus superpowers. For most new web projects, Next.js is the better choice because it gives you a production-ready foundation. You can always use React directly later if you have a specific reason to.

## Summary

- React is a library for building UIs; Next.js is a framework built on React
- Next.js adds routing, rendering, optimization, and deployment out of the box
- For most projects, especially public websites, Next.js is the better choice
- You can still use React patterns (components, hooks, state) inside Next.js

## Next Steps

Now that you understand the relationship between React and Next.js, let's set up your development environment:

- [Installation and Setup →](../02-installation-and-setup/create-next-app.md)
