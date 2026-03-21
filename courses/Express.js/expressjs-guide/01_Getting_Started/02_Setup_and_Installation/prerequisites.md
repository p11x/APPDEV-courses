# Prerequisites for Express.js

## 📌 What You'll Learn
- What you need before learning Express.js
- Basic concepts you should understand
- How to assess if you're ready for Express

## 🧠 Concept Explained (Plain English)

Before diving into Express.js, it's helpful to have some foundational knowledge. Think of it like learning to drive — you need to understand the basics of how a car works before you can learn advanced driving techniques.

The good news is that Express builds on technologies you might already know. If you've worked with JavaScript in the browser, you'll find many concepts translate directly to server-side JavaScript with Express.

You don't need to be an expert in everything listed below, but having a basic understanding will make your Express journey much smoother. Let's look at what's recommended.

## Key Prerequisites

### 1. JavaScript Fundamentals

You should be comfortable with:

| Concept | Why It Matters |
|---------|----------------|
| Variables (`let`, `const`) | Express uses these extensively |
| Functions | Route handlers are functions |
| Objects and Arrays | Working with data |
| Arrow Functions | Common in modern Express code |
| Promises/Async | Handling database and file operations |
| ES6+ Features | import/export, destructuring, template literals |

### 2. Basic HTML & HTTP Understanding

| Concept | Why It Matters |
|---------|----------------|
| HTML Basics | Building web pages or APIs |
| HTTP Methods | GET, POST, PUT, DELETE |
| JSON Format | Most Express APIs use JSON |
| URLs and Paths | Understanding routing |

### 3. Command Line Basics

| Skill | Why It Matters |
|-------|----------------|
| Navigating folders | Moving around your project |
| Running commands | Starting your server |
| Installing packages | Using npm |

## 💻 Quick Self-Assessment

Try to understand this Express code. If it makes sense, you're ready!

```javascript
// ES Module

import express from 'express';

const app = express();
const PORT = process.env.PORT || 3000;

app.get('/users/:id', async (req, res) => {
    // Can you identify: import, const, arrow function, async/await, parameter?
    const userId = req.params.id;
    const user = await findUserById(userId);
    
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }
    
    res.json(user);
});

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
```

## ⚠️ Common Mistakes

**1. Trying to learn everything first**
You don't need to master JavaScript before starting Express. Learn by doing!

**2. Skipping fundamentals entirely**
At least understand the basics of JavaScript and HTTP before diving in.

**3. Not practicing**
Reading about Express isn't enough — you need to build projects!

## ✅ Quick Recap

- JavaScript fundamentals are essential
- Basic understanding of HTTP and HTML helps
- Command line skills are needed
- The best way to learn is by building

## 🔗 What's Next

Let's install Node.js and npm, the foundation of your Express development environment.
