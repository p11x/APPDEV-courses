# Section 1: Environment Setup & Tools Installation

## Welcome to Your Full-Stack Development Journey!

This section will guide you through installing all the essential tools you'll need to become a full-stack developer. Think of this as preparing your kitchen before cooking - you need all your tools and ingredients ready before you can start making a delicious meal!

### What You'll Learn

- Why each tool is important
- How to install each tool step-by-step
- How to verify everything is working correctly

---

## 1.1 Understanding the Tools

Before we install anything, let's understand what each tool does. Imagine building a house - you need different tools for different jobs:

| Tool | Purpose | Analogy |
|------|---------|---------|
| **VS Code** | Code editor for writing front-end code | The architect's drafting table |
| **IntelliJ IDEA / STS** | IDE for Java/Spring Boot development | The engineer's workshop |
| **Node.js** | JavaScript runtime for Angular | The power source for your tools |
| **Angular CLI** | Command-line tool for Angular | The construction foreman |
| **Java JDK** | Java Development Kit | The blueprint language |
| **Maven** | Build tool for Java projects | The project manager |
| **Git** | Version control system | The time machine for your code |
| **SQL Server** | Database management | The filing cabinet |
| **Postman** | API testing tool | The quality inspector |

---

## 1.2 Installing Visual Studio Code (VS Code)

### What is VS Code?

VS Code is like a super-smart text editor that understands programming languages. It helps you write code faster with features like:
- Syntax highlighting (makes code colorful and easy to read)
- Auto-completion (suggests what you might want to type)
- Error detection (shows you mistakes before you run the code)

### Installation Steps

1. **Download VS Code**
   - Open your web browser
   - Go to: https://code.visualstudio.com/
   - Click the "Download for Windows" button

2. **Run the Installer**
   - Locate the downloaded file (usually in your Downloads folder)
   - Double-click `VSCodeUserSetup.exe`
   - Follow these steps:
     - Click "Yes" when asked about administrative rights
     - Accept the license agreement
     - Click "Next" through all options (default settings are fine)
     - Click "Install"
     - Click "Finish" when complete

3. **Launch VS Code**
   - Look for the VS Code icon on your desktop or in your Start menu
   - Click to open it

### Useful VS Code Extensions

VS Code becomes even more powerful with extensions. Let's install the ones you'll need:

1. Open VS Code
2. Click the "Extensions" icon on the left sidebar (or press `Ctrl+Shift+X`)
3. Search for and install these extensions:
   - **Angular Language Service** - For Angular development
   - **Prettier** - For code formatting
   - **ESLint** - For code quality
   - **Live Server** - For previewing web pages

---

## 1.3 Installing Node.js

### What is Node.js?

Think of Node.js as a special engine that runs JavaScript outside of web browsers. It's essential for Angular because:
- Angular uses Node.js to run development servers
- It manages dependencies (external code libraries)
- It runs the Angular CLI tool

### Installation Steps

1. **Download Node.js**
   - Go to: https://nodejs.org/
   - Click the LTS (Long Term Support) version - this is the stable one
   - The file will download automatically

2. **Install Node.js**
   - Run the downloaded `.msi` file
   - Click "Next"
   - Accept the license
   - Click "Next" through all options
   - Click "Install"
   - Click "Finish"

3. **Verify Installation**
   - Open Command Prompt (type "cmd" in Start menu)
   - Type these commands and press Enter after each:

```bash
node -v
```

Expected output: `v18.x.x` or similar (version number)

```bash
npm -v
```

Expected output: `9.x.x` or similar (npm comes with Node.js)

> **What is npm?** npm stands for "Node Package Manager" - it's like an app store for code libraries. You'll use it to install Angular and other tools.

---

## 1.4 Installing Angular CLI

### What is Angular CLI?

CLI stands for "Command Line Interface." This is a tool that runs in your terminal/command prompt. It helps you:
- Create new Angular projects
- Generate components, services, and other files
- Run your application during development
- Build for production

### Installation Steps

1. **Open Command Prompt**
   - Press `Windows + R`
   - Type `cmd` and press Enter

2. **Install Angular CLI globally**
   - Type this command and press Enter:

```bash
npm install -g @angular/cli
```

   - This might take a few minutes as it downloads Angular
   - The `-g` flag means "global" - so you can use it anywhere

3. **Verify Installation**
   - After installation completes, type:

```bash
ng version
```

   - You should see information about Angular CLI

---

## 1.5 Installing Java Development Kit (JDK)

### What is JDK?

Java is a programming language, and JDK is the toolkit you need to write and run Java programs. Spring Boot (our backend framework) is built with Java.

### Installation Steps

1. **Download JDK**
   - Go to: https://www.oracle.com/java/technologies/downloads/
   - Click "JDK Download" for Java 17 (LTS version)
   - Accept the license agreement
   - Download the "x64 Installer" (Windows)

2. **Install JDK**
   - Run the downloaded file
   - Click "Next" through all options
   - Click "Close" when done

3. **Set Up Environment Variables** (Important!)

   This tells your computer where Java is installed:

   a. Press `Windows + R`, type `sysdm.cpl` and press Enter
   b. Click "Advanced" tab
   c. Click "Environment Variables"
   d. Under "System variables", click "New"
   e. Enter these values:
      - Variable name: `JAVA_HOME`
      - Variable value: `C:\Program Files\Java\jdk-17` (or your installation path)
   f. Click "OK"

   g. Find the "Path" variable in the list, click it, then "Edit"
   h. Click "New" and add: `C:\Program Files\Java\jdk-17\bin`
   i. Click "OK" on all windows

4. **Verify Installation**
   - Open a new Command Prompt
   - Type:

```bash
java -version
```

   - You should see: `java version "17.x.x"`

---

## 1.6 Installing Maven

### What is Maven?

Maven is a build tool - it helps you manage Java projects. Think of it as:
- A project manager that downloads required libraries
- A build system that compiles your code
- A way to run tests

### Installation Steps

1. **Download Maven**
   - Go to: https://maven.apache.org/download.cgi
   - Click "Binary zip archive" under "Files"

2. **Extract Maven**
   - Create a folder: `C:\apache-maven`
   - Extract the downloaded zip file into this folder

3. **Set Up Environment Variables**

   a. Press `Windows + R`, type `sysdm.cpl` and press Enter
   b. Click "Advanced" > "Environment Variables"
   c. Under "System variables", click "New":
      - Variable name: `MAVEN_HOME`
      - Variable value: `C:\apache-maven\apache-maven-3.x.x`
   d. Find "Path", click "Edit", click "New":
      - Add: `C:\apache-maven\apache-maven-3.x.x\bin`

4. **Verify Installation**
   - Open new Command Prompt
   - Type:

```bash
mvn -version
```

   - You should see Maven version information

---

## 1.7 Installing IntelliJ IDEA or Spring Tool Suite

### Option A: IntelliJ IDEA (Recommended)

IntelliJ IDEA is a powerful IDE for Java development. The Community edition is free.

1. **Download**
   - Go to: https://www.jetbrains.com/idea/download/
   - Click "Download" under Community edition

2. **Install**
   - Run the installer
   - Click "Next" through all options
   - Click "Install"
   - Click "Finish"

### Option B: Spring Tool Suite (STS)

STS is a customized Eclipse IDE for Spring Boot development.

1. **Download**
   - Go to: https://spring.io/tools
   - Download the Windows version

2. **Install**
   - Extract the zip file
   - Move the extracted folder to a convenient location
   - Open the folder and find "SpringToolSuite4.exe"

---

## 1.8 Installing Git

### What is Git?

Git is a version control system - it tracks changes to your code. It's like:
- A detailed save system for games
- A way to collaborate with others
- A history of every change you make

### Installation Steps

1. **Download Git**
   - Go to: https://git-scm.com/download/win
   - The download should start automatically

2. **Install Git**
   - Run the installer
   - Click "Next" through these options:
     - Select "Git from the command line and also from 3rd-party software"
     - Select "Use the OpenSSL library"
     - Select "Checkout Windows-style, commit Unix-style line endings"
   - Click "Install"
   - Click "Finish"

3. **Configure Git**
   - Open Command Prompt
   - Type these commands (replace with your name and email):

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

4. **Verify Installation**
   - Type:

```bash
git --version
```

   - You should see the version number

---

## 1.9 Installing Microsoft SQL Server

### What is SQL Server?

SQL Server is a database - a structured way to store and retrieve data. Think of it as:
- A highly organized digital filing cabinet
- Where all your application data lives
- Capable of handling millions of records

### Installation Steps (Express Edition - Free)

1. **Download SQL Server Express**
   - Go to: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Click "Download now" under "Express"

2. **Install SQL Server**
   - Run the installer
   - Choose "Basic" installation
   - Accept the license
   - Click "Install"

3. **Download SQL Server Management Studio (SSMS)**
   - After installation, you'll be directed to download SSMS
   - It's a graphical tool for managing databases

4. **Launch SQL Server**
   - Open SSMS from your Start menu
   - Connect using:
     - Server type: Database Engine
     - Server name: `localhost` or `(local)`
     - Authentication: Windows Authentication

---

## 1.10 Installing Postman

### What is Postman?

Postman is a tool for testing APIs. When you build a backend, you need to test if it works correctly. Postman lets you:
- Send requests to your API
- See the responses
- Test different scenarios

### Installation Steps

1. **Download Postman**
   - Go to: https://www.postman.com/downloads/
   - Click "Download for Windows"

2. **Install**
   - Run the installer
   - Create a free account (or skip for now)
   - Start using Postman

---

## 1.11 Creating Your First Project Structure

Now that all tools are installed, let's create a basic folder structure:

```
Angular-Project-Guide/
├── frontend/           # Angular application
│   ├── src/
│   │   ├── app/
│   │   ├── assets/
│   │   └── styles.css
│   └── package.json
├── backend/            # Spring Boot application
│   ├── src/
│   │   └── main/
│   │       └── java/
│   └── pom.xml
└── database/          # SQL scripts
    └── schema.sql
```

---

## 1.12 Verifying Your Setup

Let's verify everything is working by checking all installed tools:

```bash
# Check Node.js
node -v

# Check npm
npm -v

# Check Angular CLI
ng version

# Check Java
java -version

# Check Maven
mvn -version

# Check Git
git --version
```

If all commands return version numbers, congratulations! Your environment is ready!

---

## Summary

In this section, you've learned to:
- Install VS Code, the code editor
- Install Node.js and npm for JavaScript runtime
- Install Angular CLI for Angular development
- Install Java JDK for backend development
- Install Maven for building Java projects
- Install IntelliJ IDEA or STS for Java IDE
- Install Git for version control
- Install SQL Server for database management
- Install Postman for API testing

### What's Next?

In the next section, we'll explore what full-stack development means and understand how all these tools work together to create amazing applications!

---

## Quick Reference: Installation Summary

| Tool | Download URL | Verify Command |
|------|--------------|----------------|
| VS Code | code.visualstudio.com | (GUI application) |
| Node.js | nodejs.org | `node -v` |
| Angular CLI | (npm install) | `ng version` |
| JDK 17 | oracle.com/java | `java -v` |
| Maven | apache.org/maven | `mvn -v` |
| IntelliJ IDEA | jetbrains.com/idea | (GUI application) |
| Git | git-scm.com | `git --version` |
| SQL Server | microsoft.com/sql-server | (GUI application) |
| Postman | postman.com | (GUI application) |
