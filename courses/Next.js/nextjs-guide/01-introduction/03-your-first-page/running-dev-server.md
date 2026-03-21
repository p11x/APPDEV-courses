# Running the Development Server

## What You'll Learn
- How to start the Next.js development server
- How hot module replacement works
- How to debug common development issues

## Prerequisites
- A Next.js project set up
- Basic terminal/command line knowledge

## Concept Explained Simply

The development server is the heart of your Next.js workflow. It's a program that runs in the background, watches your files for changes, and automatically updates your website whenever you save. This is called **Hot Module Replacement** (HMR) — fancy terminology for "the page updates when you save."

When you run `npm run dev`, Next.js compiles your code and starts a local web server at `http://localhost:3000`. This is like having a mini version of your production website running on your computer.

The best part? You don't need to refresh the page to see changes. Just edit your code, save, and watch the page update instantly. It's incredibly satisfying once you see it in action.

## Starting the Server

### Step 1: Open Your Terminal

Make sure you're in your project directory:

```bash
cd my-first-app
```

### Step 2: Start the Development Server

```bash
npm run dev
```

You'll see output like this:

```
  ▲ Next.js 15.0.0
  - Local: http://localhost:3000
  - Environments: .env.local, .env

 ✓ Ready in 2.1s
 ○ (Static)   page with no dynamic data
 ○ (Server)  server-side-rendered at runtime (uses no cache)
```

### Step 3: Open in Browser

Go to `http://localhost:3000` in your browser. You should see your Next.js app!

## Understanding the Output

The terminal shows important information:

| Message | Meaning |
|---------|---------|
| `Next.js 15.0.0` | The version of Next.js |
| `Local: http://localhost:3000` | Your app's URL |
| `Ready in 2.1s` | How fast it started |
| `○ (Static)` | A static page (no server processing needed) |
| `○ (Server)` | A server-rendered page |

## Hot Module Replacement

Here's how it works:

1. You edit a file (like `page.tsx`)
2. Next.js detects the change
3. It recompiles only what changed (not the whole app)
4. Your browser automatically updates

This happens in milliseconds, making development incredibly fast.

## Common Development Scenarios

### Creating a New Page

1. Create `src/app/new-page/page.tsx`
2. Go to `http://localhost:3000/new-page`
3. The page appears automatically!

### Fixing a TypeScript Error

If you make a TypeScript mistake, the terminal shows the error:

```
Error: Type 'string' is not assignable to type 'number'
```

Fix the error in your code and save. The page will reload automatically.

### Adding a Component

Create a new component file and import it:

```typescript
// src/components/Header.tsx
export default function Header() {
  return <header><h1>My Site</h1></header>;
}
```

```typescript
// src/app/page.tsx
import Header from "@/components/Header";

export default function Home() {
  return (
    <>
      <Header />
      <main>Welcome!</main>
    </>
  );
}
```

## Stopping the Server

When you're done working:

1. Press `Ctrl + C` (Windows) or `Cmd + C` (Mac) in the terminal
2. The server stops

To start again, just run `npm run dev`.

## Common Mistakes

### Mistake #1: Forgetting to Start the Server

You open the browser and see "This site can't be reached." Make sure the server is running!

```bash
npm run dev  # Run this first!
```

### Mistake #2: Wrong Port

If port 3000 is taken, Next.js tries port 3001. Check the terminal for the correct URL.

### Mistake #3: Closing the Terminal

Don't close the terminal window while working! The server must keep running.

### Mistake #4: Not Saving

Make sure you've saved your file (Ctrl+S or Cmd+S) before expecting changes.

## Pro Tips

### Use the Build Output

The terminal shows which pages are static vs. server-rendered:

```
○ (Static)   page with no dynamic data
● (Server)   server-side-rendered at runtime
```

This helps you understand how your pages work.

### Check for Errors

Always check the terminal for errors. They're easier to debug than browser console errors.

### Use VS Code Terminal

Keep the terminal inside VS Code for a smoother workflow:

1. Press `Ctrl + `` (backtick) in VS Code
2. The terminal opens at the bottom
3. Run `npm run dev` there

## Summary

- Run `npm run dev` to start the development server
- Your app is at `http://localhost:3000`
- Save files to trigger automatic updates (Hot Module Replacement)
- Press `Ctrl + C` to stop the server
- Check the terminal for errors and build information

## Next Steps

Congratulations! You've completed the introduction. Now let's dive into the App Router:

- [App Router Basics →](../../02-app-router/01-routing-basics/folder-conventions.md)
