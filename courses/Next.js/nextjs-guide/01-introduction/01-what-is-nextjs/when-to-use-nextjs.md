# When to Use Next.js

## What You'll Learn
- Real-world scenarios where Next.js excels
- Projects that might not need Next.js
- How to decide if Next.js is right for your project

## Prerequisites
- Understanding of what Next.js is (covered in previous pages)
- Basic knowledge of web development concepts

## Concept Explained Simply

Not every project needs Next.js, and that's okay! Let's talk about when Next.js makes sense and when it might be overkill.

Next.js shines brightest when you're building **content-focused websites** that people will find through search engines. This includes blogs, marketing pages, documentation sites, e-commerce stores, and portfolios. Any website where speed and SEO matter (which is almost all public websites!) benefits greatly from Next.js.

Think about the last time you visited a slow website. You probably left before the page finished loading, right? Next.js prevents that by pre-rendering pages on the server, so users see content immediately. This is called **perceived performance** — the site feels faster even if it's only marginally so.

Next.js is also perfect for **dashboards and web apps** where you need authentication, database connections, and dynamic data. You can mix static pages (like your pricing page) with dynamic pages (like a user's dashboard) in the same application.

## When Next.js is the Right Choice

1. **E-commerce websites** — Fast loading times mean more sales
2. **Content sites** — Blogs, news sites, documentation (great SEO)
3. **Marketing pages** — Landing pages that need to rank on Google
4. **SaaS applications** — Dashboards with user accounts
5. **Portfolios** — Show off your work with fast load times
6. **Any public-facing website** — If strangers will visit, Next.js helps

## When You Might Not Need Next.js

1. **Simple widgets** — Embeddable components for existing sites
2. **Internal tools** — Used by only a few people behind a VPN
3. **Maximum flexibility** — When you need complete control over your build
4. **Learning React** — If you're just learning React fundamentals

## A Quick Decision Framework

Ask yourself these questions:

1. **Will strangers visit this website?** → Yes → Next.js is a good fit
2. **Do I need good Google rankings?** → Yes → Next.js is a good fit
3. **Is this a simple widget or embed?** → Yes → Plain React might work
4. **Do I need authentication and databases?** → Yes → Next.js is a good fit
5. **Am I just learning React?** → Yes → Plain React is fine to start

## The Real Answer

Here's the honest truth: for 95% of web development projects, Next.js is the right choice. The few exceptions are edge cases that most developers won't encounter. Even internal tools often become public eventually, and you'll wish you started with Next.js.

## Summary

- Next.js is ideal for public websites, e-commerce, content sites, and web apps
- Simple widgets and learning projects might not need Next.js
- For most projects, especially public ones, Next.js is the better choice
- The benefits of SEO, performance, and easy deployment outweigh any added complexity

## Next Steps

Ready to get started? Let's install Next.js and create your first project:

- [Create Your First Next.js App →](../02-installation-and-setup/create-next-app.md)
