# Setting Up TypeScript

## Prerequisites

Before installing TypeScript, you need to have **Node.js** and **npm** (Node Package Manager) installed on your computer. Node.js is a JavaScript runtime that allows you to run JavaScript outside of a web browser, and npm is the package manager that comes with Node.js.

To verify if you have Node.js and npm installed, open your terminal and run:

```bash
node --version
npm --version
```

If these commands return version numbers, you're ready to proceed. If not, download and install Node.js from the official website (nodejs.org).

## Installing TypeScript Globally

The TypeScript compiler is called `tsc`. To install it globally on your computer, run the following command in your terminal:

```bash
npm install -g typescript
```

This makes the `tsc` command available from anywhere in your terminal. To verify the installation, check the TypeScript version:

```bash
tsc --version
```

## Compiling TypeScript Files

Once TypeScript is installed, you can compile a TypeScript file to JavaScript using the `tsc` command:

```bash
tsc filename.ts
```

This will create a corresponding JavaScript file (`filename.js`) in the same directory. You can then run the JavaScript file using Node.js:

```bash
node filename.js
```

## Understanding tsconfig.json

For real-world projects, you need a `tsconfig.json` file that tells TypeScript how to compile your code. This file specifies:

- Which TypeScript version to use
- Which files to include/exclude
- Output directory for compiled JavaScript
- JavaScript target version (ES5, ES6, etc.)
- Module system to use
- Strict mode settings

A basic `tsconfig.json` looks like:

```json
{
  "compilerOptions": {
    "target": "ES6",
    "module": "commonjs",
    "outDir": "./dist",
    "strict": true
  },
  "include": ["src/**/*"],
  "exclude": ["node_modules"]
}
```

## Setting Up a Project

To create a TypeScript project:

1. Create a new folder for your project
2. Run `npm init -y` to create a `package.json` file
3. Run `npm install typescript --save-dev` to add TypeScript as a dev dependency
4. Create a `tsconfig.json` file with your desired settings
5. Create TypeScript files in your project
6. Run `tsc` to compile all TypeScript files

## Key Points to Remember

- Node.js and npm are prerequisites for TypeScript
- Install TypeScript globally with `npm install -g typescript`
- Use `tsc filename.ts` to compile individual files
- Use `tsc` command with a `tsconfig.json` for project compilation
- The compiled JavaScript files can be run with Node.js or in browsers
- `tsconfig.json` controls how TypeScript compiles your code
- Always enable strict mode for better type safety

## Connection to Angular

When you create an Angular project using the Angular CLI (`ng new project-name`), TypeScript is automatically set up with the proper configuration. The Angular CLI handles all the compilation and configuration behind the scenes, so you can focus on writing code. Understanding how TypeScript compilation works will help you debug issues and understand how Angular builds your application.
