# Create Your First Next.js App

## What You'll Learn
- How to create a new Next.js project using create-next-app
- What the different setup options mean
- How to run your development server

## Prerequisites
- Node.js installed (version 18.17 or later)
- A code editor like VS Code
- Basic familiarity with the terminal/command line

## Concept Explained Simply

The Next.js team created a tool called `create-next-app` that sets up everything you need to start building. It's like a wizard that asks you a few questions and then creates a fully configured project. This saves you from the nightmare of configuring build tools, TypeScript, and all the other stuff that usually takes hours.

Let's walk through creating your first Next.js app together.

## Step-by-Step Setup

### Step 1: Open Your Terminal

Open your terminal or command prompt. On Windows, you can search for "Command Prompt" or "PowerShell". On Mac, open "Terminal" from Applications > Utilities.

### Step 2: Run the Create Command

Type this command and press Enter:

```bash
npx create-next-app@latest my-first-app
```

Replace `my-first-app` with whatever you want to name your project (use lowercase and hyphens).

### Step 3: Answer the Questions

The wizard will ask you some questions. Here's what to choose:

```
Would you like to use TypeScript? → Yes
Would you like to use ESLint? → Yes
Would you like to use Tailwind CSS? → Yes
Would you like to use `src/` directory? → Yes
Would you like to use App Router? (recommended) → Yes
Would you like to customize the default import alias? → No
```

Press Enter after each answer. The process takes a few minutes.

### Step 4: Navigate to Your Project

```bash
cd my-first-app
```

### Step 5: Start the Development Server

```bash
npm run dev
```

Open your browser and go to `http://localhost:3000`. You should see the Next.js welcome page!

## Complete Code Example

Your project structure will look like this after creation:

```
my-first-app/
├── src/
│   ├── app/
│   │   ├── globals.css
│   │   ├── layout.tsx
│   │   ├── page.tsx
│   │   └── page.module.css
│   └── favicon.ico
├── public/
├── package.json
├── tsconfig.json
├── next.config.ts
├── tailwind.config.ts
└── postcss.config.js
```

The main file you'll edit is `src/app/page.tsx`:

```typescript
// src/app/page.tsx
import Image from "next/image";

export default function Home() {
  return (
    <main className="flex min-h-screen flex-col items-center justify-between p-24">
      <div className="z-10 max-w-5xl w-full items-center justify-between font-mono text-sm lg:flex">
        <p className="fixed left-0 top-0 flex w-full justify-center border-b border-gray-300 bg-gradient-to-b from-zinc-200 pb-6 pt-8 backdrop-blur-2xl dark:border-neutral-800 dark:bg-zinc-800/30 dark:from-inherit lg:static lg:w-auto  lg:rounded-xl lg:border lg:bg-gray-200 lg:p-4 lg:dark:bg-zinc-800/30">
          Get started by editing&nbsp;
          <code className="font-mono font-bold">src/app/page.tsx</code>
        </p>
      </div>
    </main>
  );
}
```

## Line-by-Line Breakdown

| Line / Block | What It Does | Why It Matters |
|---|---|---|
| `import Image from "next/image"` | Imports Next.js optimized Image component | Automatically optimizes images for performance |
| `export default function Home()` | Exports the main page component | This becomes the homepage of your app |
| `<main className="...">` | The main content wrapper | Uses Tailwind CSS classes for styling |
| `<code className="font-mono...">` | Displays code inline | Shows the user they can edit this file |

## Common Mistakes

### Mistake #1: Using the Wrong Node Version

If you see errors about Node.js version, make sure you have Node 18.17 or later:

```bash
node --version
```

If you need to update Node, download it from nodejs.org or use nvm (Node Version Manager).

### Mistake #2: Forgetting to cd Into the Project

Many beginners try to run commands without navigating to the project folder first. Always run `cd my-first-app` before running `npm run dev`.

### Mistake #3: Closing the Terminal

Remember, the development server must keep running while you're working. Don't close the terminal window!

## Summary

- Use `npx create-next-app@latest` to create a new Next.js project
- Choose TypeScript, ESLint, Tailwind CSS, and the App Router
- Run `npm run dev` to start the development server
- Your app runs at `http://localhost:3000`
- Edit `src/app/page.tsx` to change the homepage

## Next Steps

Now that your app is running, let's explore what all those files are for:

- [Project Structure Explained →](./project-structure-explained.md)
