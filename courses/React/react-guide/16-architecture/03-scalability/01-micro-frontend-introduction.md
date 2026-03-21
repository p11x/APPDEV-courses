# Micro-Frontend Introduction

## Overview
Micro-frontends extend the microservices concept to the frontend, allowing teams to develop and deploy independent pieces of a web application. This guide covers the basics of micro-frontend architecture and when to consider this approach.

## Prerequisites
- Frontend development experience
- Understanding of web architecture

## Core Concepts

### What Are Micro-Frontends?
Micro-frontends divide a large application into smaller, independently deployable pieces that are composed into a single application.

### Approaches

| Approach | Description | Pros | Cons |
|----------|-------------|------|------|
| iframe | Embed separate apps in iframes | Simple isolation | Limited communication |
| Web Components | Use standard web components | Framework-agnostic | Learning curve |
| Module Federation | Webpack/Vite sharing | Performance, flexibility | Setup complexity |

## When to Use

- Multiple teams working on same app
- Need independent deployments
- Different parts need different frameworks
- Large application complexity

## Key Takeaways
- Micro-frontends enable team autonomy
- Choose approach based on needs
- Consider complexity vs benefits

## What's Next
Continue to [Module Federation Basics](02-module-federation-basics.md) to learn about Webpack Module Federation.