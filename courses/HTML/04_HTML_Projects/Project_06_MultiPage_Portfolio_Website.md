# Project 6: Multi-Page Portfolio Website

## Project Overview

### Project Title
Multi-Page Portfolio Website

### Project Description
Create a complete multi-page portfolio website with Home, About, Projects, Services, and Contact pages. This project demonstrates how to build a cohesive multi-page website with consistent navigation and styling.

### Learning Objectives

- Create multiple linked HTML pages
- Build consistent navigation across pages
- Apply shared styling
- Create responsive layouts

### Estimated Duration
5-6 hours

---

## Project Structure

```
portfolio/
├── index.html          (Home page)
├── about.html          (About page)
├── projects.html       (Projects page)
├── services.html       (Services page)
├── contact.html        (Contact page)
└── css/
    └── style.css       (Shared styles)
```

---

## File Templates

### Shared CSS (css/style.css)

```css
/* Global Styles */
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    line-height: 1.6;
    color: #333;
}

/* Header & Navigation */
header {
    background: #2c3e50;
    color: white;
    padding: 1rem 0;
}

nav {
    display: flex;
    justify-content: space-between;
    align-items: center;
    max-width: 1200px;
    margin: 0 auto;
    padding: 0 20px;
}

.logo {
    font-size: 1.5rem;
    font-weight: bold;
}

nav ul {
    display: flex;
    list-style: none;
    gap: 2rem;
}

nav a {
    color: white;
    text-decoration: none;
    transition: color 0.3s;
}

nav a:hover,
nav a.active {
    color: #3498db;
}

/* Main Content */
main {
    max-width: 1200px;
    margin: 0 auto;
    padding: 40px 20px;
    min-height: 60vh;
}

/* Footer */
footer {
    background: #2c3e50;
    color: white;
    text-align: center;
    padding: 20px;
    margin-top: 40px;
}
```

### Home Page (index.html)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>John Doe - Web Developer Portfolio</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <header>
        <nav>
            <div class="logo">John Doe</div>
            <ul>
                <li><a href="index.html" class="active">Home</a></li>
                <li><a href="about.html">About</a></li>
                <li><a href="projects.html">Projects</a></li>
                <li><a href="services.html">Services</a></li>
                <li><a href="contact.html">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <section class="hero">
            <h1>Welcome to My Portfolio</h1>
            <p>I am a passionate web developer creating modern, responsive websites.</p>
            <a href="projects.html" class="btn">View My Work</a>
        </section>
        
        <section class="features">
            <h2>What I Do</h2>
            <div class="feature-grid">
                <div class="feature">
                    <h3>Web Development</h3>
                    <p>Custom websites built with modern technologies.</p>
                </div>
                <div class="feature">
                    <h3>Responsive Design</h3>
                    <p>Websites that look great on all devices.</p>
                </div>
                <div class="feature">
                    <h3>Clean Code</h3>
                    <p>Well-structured, maintainable code.</p>
                </div>
            </div>
        </section>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. All rights reserved.</p>
    </footer>
</body>
</html>
```

### About Page (about.html)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>About - John Doe Portfolio</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <header>
        <nav>
            <div class="logo">John Doe</div>
            <ul>
                <li><a href="index.html">Home</a></li>
                <li><a href="about.html" class="active">About</a></li>
                <li><a href="projects.html">Projects</a></li>
                <li><a href="services.html">Services</a></li>
                <li><a href="contact.html">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <section class="about">
            <h1>About Me</h1>
            <img src="profile.jpg" alt="John Doe" class="profile-img">
            <p>Hello! I'm a web developer with 5 years of experience...</p>
            
            <h2>My Skills</h2>
            <ul class="skills">
                <li>HTML5 & CSS3</li>
                <li>JavaScript (ES6+)</li>
                <li>React</li>
                <li>Angular</li>
                <li>Node.js</li>
            </ul>
        </section>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Projects Page (projects.html)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Projects - John Doe Portfolio</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <header>
        <nav>
            <div class="logo">John Doe</div>
            <ul>
                <li><a href="index.html">Home</a></li>
                <li><a href="about.html">About</a></li>
                <li><a href="projects.html" class="active">Projects</a></li>
                <li><a href="services.html">Services</a></li>
                <li><a href="contact.html">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <h1>My Projects</h1>
        
        <div class="projects-grid">
            <article class="project">
                <img src="project1.jpg" alt="Project 1">
                <h3>E-commerce Website</h3>
                <p>A full-featured online store built with React.</p>
                <a href="#">View Project</a>
            </article>
            
            <article class="project">
                <img src="project2.jpg" alt="Project 2">
                <h3>Portfolio Website</h3>
                <p>Personal portfolio with modern design.</p>
                <a href="#">View Project</a>
            </article>
            
            <article class="project">
                <img src="project3.jpg" alt="Project 3">
                <h3>Task Manager</h3>
                <p>Productivity app with Angular and Firebase.</p>
                <a href="#">View Project</a>
            </article>
        </div>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Services Page (services.html)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Services - John Doe Portfolio</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <header>
        <nav>
            <div class="logo">John Doe</div>
            <ul>
                <li><a href="index.html">Home</a></li>
                <li><a href="about.html">About</a></li>
                <li><a href="projects.html">Projects</a></li>
                <li><a href="services.html" class="active">Services</a></li>
                <li><a href="contact.html">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <h1>My Services</h1>
        
        <div class="services-grid">
            <div class="service">
                <h3>Frontend Development</h3>
                <p>Modern, responsive websites built with HTML, CSS, and JavaScript frameworks.</p>
                <p class="price">From $500</p>
            </div>
            
            <div class="service">
                <h3>Backend Development</h3>
                <p>Robust server-side applications with Node.js and databases.</p>
                <p class="price">From $1000</p>
            </div>
            
            <div class="service">
                <h3>Full-Stack Solutions</h3>
                <p>Complete web applications from design to deployment.</p>
                <p class="price">From $2000</p>
            </div>
        </div>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. All rights reserved.</p>
    </footer>
</body>
</html>
```

### Contact Page (contact.html)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Contact - John Doe Portfolio</title>
    <link rel="stylesheet" href="css/style.css">
</head>
<body>
    <header>
        <nav>
            <div class="logo">John Doe</div>
            <ul>
                <li><a href="index.html">Home</a></li>
                <li><a href="about.html">About</a></li>
                <li><a href="projects.html">Projects</a></li>
                <li><a href="services.html">Services</a></li>
                <li><a href="contact.html" class="active">Contact</a></li>
            </ul>
        </nav>
    </header>
    
    <main>
        <h1>Contact Me</h1>
        
        <div class="contact-container">
            <div class="contact-info">
                <h3>Get in Touch</h3>
                <p>Email: john@example.com</p>
                <p>Phone: +1 234 567 890</p>
                <p>Location: San Francisco, CA</p>
            </div>
            
            <form class="contact-form">
                <label>Name</label>
                <input type="text" required>
                
                <label>Email</label>
                <input type="email" required>
                
                <label>Message</label>
                <textarea rows="5" required></textarea>
                
                <button type="submit">Send Message</button>
            </form>
        </div>
    </main>
    
    <footer>
        <p>&copy; 2024 John Doe. All rights reserved.</p>
    </footer>
</body>
</html>
```

---

## Project Checklist

### Structure (15 points)
- [ ] All 5 pages created
- [ ] Shared CSS file
- [ ] Consistent navigation
- [ ] Proper linking between pages

### Content (15 points)
- [ ] Home page with hero
- [ ] About page with bio/skills
- [ ] Projects page with portfolio
- [ ] Services page with offerings
- [ ] Contact page with form

### Navigation (10 points)
- [ ] Active page highlighted
- [ ] All links work
- [ ] Consistent header/footer

### Code Quality (10 points)
- [ ] Semantic HTML
- [ ] Clean, organized CSS
- [ ] Proper structure
