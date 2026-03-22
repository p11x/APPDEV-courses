<div align="center">

[![GitHub last commit](https://img.shields.io/github/last-commit/p11x/APPDEV-courses)](https://github.com/p11x/APPDEV-courses/commits/master)
[![GitHub repo size](https://img.shields.io/github/repo-size/p11x/APPDEV-courses)](https://github.com/p11x/APPDEV-courses)
[![GitHub stars](https://img.shields.io/github/stars/p11x/APPDEV-courses?style=social)](https://github.com/p11x/APPDEV-courses/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/p11x/APPDEV-courses?style=social)](https://github.com/p11x/APPDEV-courses/network)
[![Total Files](https://img.shields.io/badge/Total%20Files-1664-blue)](https://github.com/p11x/APPDEV-courses)
[![Total Courses](https://img.shields.io/badge/Courses-19-brightgreen)](https://github.com/p11x/APPDEV-courses)

</div>

# 📚 APPDEV Courses

<div align="center">

A comprehensive collection of programming course materials

[**View Live Index →**](https://p11x.github.io/APPDEV-courses)

</div>

---

## 📖 About

Welcome to **APPDEV Courses** — a comprehensive repository containing over 1,600 files and 216,621 lines of course material covering 19 different programming courses. This repository is designed to help learners master various technologies from beginner to advanced levels.

Whether you're looking to learn web development, mobile apps, cloud technologies, or data science, you'll find structured learning materials here.

---

## 📂 Courses

### 🐍 Python
Comprehensive Python programming guide from basics to advanced topics including data science, machine learning, and automation.
- **Topics**: `Data Science` `Machine Learning` `Async` `Web Scraping` `Automation`
- [View Course](./Python)

### ⚛️ React
Modern React development with hooks, state management, routing, and Next.js integration.
- **Topics**: `Hooks` `State Management` `Next.js` `TypeScript` `Testing`
- [View Course](./React)

### ▲ Next.js
Full-stack React framework with App Router, Server Components, and server actions.
- **Topics**: `App Router` `Server Components` `API Routes` `Deployment` `Authentication`
- [View Course](./Next.js)

### 🚀 Express.js
Node.js web application framework for building APIs and web servers.
- **Topics**: `Routing` `Middleware` `Templating` `Authentication` `Database`
- [View Course](./Express.js)

### 🐳 Docker & Kubernetes
Container orchestration and DevOps best practices for modern applications.
- **Topics**: `Docker` `Kubernetes` `Helm` `CI/CD` `Cloud Native`
- [View Course](./Docker%20&%20Kubernetes)

### 🟢 Node.js
Server-side JavaScript runtime and backend development fundamentals.
- **Topics**: `Express` `APIs` `Authentication` `Database` `Microservices`
- [View Course](./Node.js)

### 🔶 Angular
Enterprise-grade web application framework with TypeScript.
- **Topics**: `Components` `Services` `Routing` `Forms` `RxJS`
- [View Course](./Angular)

### ☕ Java
Java programming from core concepts to Spring Boot full-stack development.
- **Topics**: `Core Java` `OOP` `Spring Boot` `REST API` `JPA`
- [View Course](./java)

### 🌐 HTML & CSS
Frontend web development fundamentals and modern CSS techniques.
- **Topics**: `HTML5` `CSS3` `Flexbox` `Grid` `Responsive`
- [View Course](./HTML)

### 📘 TypeScript
Typed JavaScript for safer and more maintainable code.
- **Topics**: `Types` `Interfaces` `Generics` `Decorators` `Advanced Patterns`
- [View Course](./Type%20Script)

### ⚗️ Flask
Lightweight Python web framework for building web applications.
- **Topics**: `Routing` `Templates` `Database` `REST APIs` `Authentication`
- [View Course](./Flask)

### 🔧 Jenkins
Automation server for CI/CD pipelines and continuous integration.
- **Topics**: `Pipelines` `Plugins` `Automation` `Deployment` `Integration`
- [View Course](./Jenkins)

### 🅱️ Bootstrap
CSS framework for building responsive, mobile-first websites.
- **Topics**: `Grid` `Components` `Utilities` `Customization` `Responsive`
- [View Course](./Bootstrap)

### 🗄️ SQL Server
Microsoft relational database management and T-SQL programming.
- **Topics**: `Queries` `Tables` `Stored Procedures` `Indexes` `Performance`
- [View Course](./SQL%20Server)

### 📦 Git Hub
Version control and collaborative development with Git.
- **Topics**: `Branching` `Merging` `Pull Requests` `Workflows` `Collaboration`
- [View Course](./Git%20Hub)

### 🤖 AI Tools
Artificial intelligence tools and applications for developers.
- **Topics**: `LLMs` `APIs` `Integration` `Prompt Engineering` `Automation`
- [View Course](./AI%20Tools)

### 🌱 SpringBoot
Java framework for building production-ready applications.
- **Topics**: `REST API` `JPA` `Security` `Microservices` `Testing`
- [View Course](./SpringBoot)

### 🔷 C#
Microsoft .NET programming language for Windows and cross-platform development.
- **Topics**: `OOP` `.NET` `ASP.NET` `LINQ` `Entity Framework`
- [View Course](./C#)

### ⬛ .NET
Microsoft .NET platform for building modern applications.
- **Topics**: `ASP.NET` `Entity Framework` `Web API` `Microservices` `Cloud`
- [View Course](./.NET)

---

## 🚀 Getting Started

### Clone the Repository

```bash
git clone https://github.com/p11x/APPDEV-courses.git
```

### Browse Materials

- **Local**: Open `index.html` in your browser to view the interactive course index
- **Online**: Visit the [Live GitHub Pages](https://p11x.github.io/APPDEV-courses) site

---

## ⚙️ Auto-Commit Watcher

This repository includes an automatic file watcher that monitors changes and automatically commits and pushes them to GitHub.

### What it does

- 🔍 **Monitors** the entire repository for new or modified files
- ⏱️ **Waits** 3 seconds (debounce) to batch rapid changes
- ✅ **Commits** changes with descriptive messages
- 🚀 **Pushes** automatically to the master branch
- 📝 **Logs** all actions to `auto_commit.log`

### How to Start

#### Windows
```cmd
start_watcher.bat
```

#### Mac / Linux
```bash
bash start_watcher.sh
```

### How to Stop

Press `Ctrl + C` in the terminal to stop the watcher.

### Requirements

- Python 3.7+
- `watchdog` library (automatically installed)

---

### 🔁 Run as Background Service (Auto-start on Boot)

Configure the watcher to run automatically in the background on system startup:

| OS      | Setup Command                        | Remove Command                          |
|---------|--------------------------------------|-----------------------------------------|
| Windows | `powershell setup_windows_service.ps1` | `powershell remove_windows_service.ps1`   |
| Linux   | `bash setup_linux_service.sh`          | `bash remove_linux_service.sh`            |
| Mac     | `bash setup_mac_service.sh`            | `bash remove_mac_service.sh`              |
| Any OS  | `python setup_autostart.py`            | (run platform-specific remove script)    |

**Features:**
- 🔄 Auto-starts on every system boot/login
- 📄 Logs are saved to `auto_commit.log`
- ♻️ Service restarts automatically if it crashes
- 🛡️ Runs in the background without showing a terminal window

**Quick One-Command Setup:**
```bash
# Auto-detects OS and configures
python setup_autostart.py
```

---

## 🛠️ Tech Stack

![Python](https://img.shields.io/badge/Python-3776AB?style=for-the-badge&logo=python&logoColor=white)
![HTML5](https://img.shields.io/badge/HTML5-E34F26?style=for-the-badge&logo=html5&logoColor=white)
![CSS3](https://img.shields.io/badge/CSS3-1572B6?style=for-the-badge&logo=css3&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![Java](https://img.shields.io/badge/Java-ED8B00?style=for-the-badge&logo=java&logoColor=white)
![Markdown](https://img.shields.io/badge/Markdown-083FA1?style=for-the-badge&logo=markdown&logoColor=white)

---

## 🌐 Live Preview

<div align="center">

[![View Live Index](https://img.shields.io/badge/Click%20to%20View-Interactive%20Index-3B82F6?style=for-the-badge)](https://p11x.github.io/APPDEV-courses)

*Interactive course index with search, cards, and file tree*

**Live Site**: https://p11x.github.io/APPDEV-courses

</div>

---

## 📝 License

This repository is for educational purposes.

---

<div align="center">

Made with ❤️ for learning and development

*Last updated: March 21, 2026*

</div>
