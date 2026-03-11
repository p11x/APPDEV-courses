# Section 1: Exercises

## Practice Exercise 1.1: Verify Your Tools

### Objective
Verify that all required development tools are properly installed on your computer.

### Instructions

1. **Open Command Prompt**
   - Press `Windows + R`
   - Type `cmd` and press Enter

2. **Run Verification Commands**
   Run each of the following commands and record the output:

```bash
node -v
```
Expected output example: `v18.17.0`

```bash
npm -v
```
Expected output example: `9.8.1`

```bash
ng version
```
Expected output: Angular CLI version information

```bash
java -version
```
Expected output example: `java version "17.0.8"`

```bash
mvn -version
```
Expected output example: `Apache Maven 3.9.4`

```bash
git --version
```
Expected output example: `git version 2.41.0`

### Verification Checklist

- [ ] Node.js is installed (version 16 or higher)
- [ ] npm is installed
- [ ] Angular CLI is installed
- [ ] Java JDK is installed (version 11 or higher)
- [ ] Maven is installed
- [ ] Git is installed

---

## Practice Exercise 1.2: Configure VS Code

### Objective
Set up Visual Studio Code for optimal development experience.

### Instructions

1. **Launch VS Code**

2. **Install Required Extensions**
   - Click the Extensions icon (Ctrl+Shift+X)
   - Search and install:
     - Angular Language Service
     - Prettier - Code formatter
     - ESLint
     - Live Server
     - Bootstrap 5 Quick Snippets
     - TypeScript Vue Plugin (or Vue 2 Snippets)

3. **Configure VS Code Settings**
   - Go to File > Preferences > Settings
   - Add these settings (click the JSON icon to edit settings.json):

```json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.tabSize": 2,
  "files.autoSave": "onFocusChange",
  "emmet.includeLanguages": {
    "javascript": "javascriptreact"
  }
}
```

---

## Practice Exercise 1.3: Test Your First Angular Command

### Objective
Create your first Angular project to verify Angular CLI is working.

### Instructions

1. **Create a Test Project**
   In your command prompt:

```bash
cd Desktop
ng new hello-world --routing=false --style=css --skip-git --skip-tests
```

2. **Navigate to Project**
```bash
cd hello-world
```

3. **Start Development Server**
```bash
ng serve
```

4. **Verify It Works**
   - Open your browser
   - Go to: http://localhost:4200
   - You should see the Angular welcome page

5. **Stop the Server**
   - Press `Ctrl + C` in the command prompt
   - Type `Y` to confirm

### Cleanup
You can delete the `hello-world` folder since this was just a test.

---

## Practice Exercise 1.4: Set Up Git

### Objective
Configure Git with your personal information.

### Instructions

1. **Open Command Prompt**

2. **Set Your Name**
```bash
git config --global user.name "Your Name"
```

3. **Set Your Email**
```bash
git config --global user.email "your.email@example.com"
```

4. **Verify Configuration**
```bash
git config --list
```

5. **Create a Test Repository**
```bash
cd Desktop
mkdir my-first-repo
cd my-first-repo
git init
```

---

## Practice Exercise 1.5: Explore SQL Server

### Objective
Get familiar with SQL Server Management Studio.

### Instructions

1. **Launch SQL Server Management Studio**

2. **Connect to Local Server**
   - Server name: `localhost` or `(local)`
   - Authentication: Windows Authentication
   - Click Connect

3. **Explore the Interface**
   - Object Explorer (left panel)
   - Menu bar
   - Toolbar

4. **Create a Test Database**
   - Right-click on "Databases"
   - Select "New Database"
   - Name it: `TestDB`
   - Click OK

5. **Verify**
   - Expand "Databases"
   - You should see `TestDB`

---

## Bonus Exercise: Create a Project Template

Create a folder structure that you'll use throughout this course:

```
Projects/
├── Angular-Project-Guide/
│   ├── frontend/
│   ├── backend/
│   └── database/
```

Use these commands:

```bash
cd Desktop
mkdir Projects
cd Projects
mkdir Angular-Project-Guide
cd Angular-Project-Guide
mkdir frontend backend database
```

---

## Answers to Common Issues

### Issue: "ng is not recognized"
**Solution:** 
```bash
npm install -g @angular/cli
```

### Issue: "java is not recognized"
**Solution:** 
- Check JAVA_HOME environment variable
- Add Java bin directory to PATH

### Issue: "mvn is not recognized"
**Solution:**
- Check MAVEN_HOME environment variable
- Add Maven bin directory to PATH

### Issue: "node is not recognized"
**Solution:**
- Reinstall Node.js
- Make sure to check "Add to PATH" during installation
