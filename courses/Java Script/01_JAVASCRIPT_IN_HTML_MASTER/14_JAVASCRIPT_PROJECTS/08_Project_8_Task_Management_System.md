# 📋 Project 8: Task Management System

## 📋 Project Overview

Build a comprehensive task management system with project organization, team collaboration features, due dates, priority levels, and progress tracking. This project demonstrates:
- Multi-project management
- Task organization and filtering
- Progress visualization
- Deadline tracking

---

## 🎯 Core Features

### Project Manager

```javascript
class ProjectManager {
    constructor() {
        this.projects = [];
        this.currentProject = null;
        this.loadFromStorage();
    }
    
    createProject(name, description = '') {
        const project = {
            id: this.generateId(),
            name,
            description,
            tasks: [],
            createdAt: new Date().toISOString(),
            color: this.getRandomColor()
        };
        
        this.projects.push(project);
        this.saveToStorage();
        return project;
    }
    
    addTask(projectId, title, priority = 'medium', dueDate = null) {
        const project = this.projects.find(p => p.id === projectId);
        if (!project) return null;
        
        const task = {
            id: this.generateId(),
            title,
            priority,
            dueDate,
            status: 'pending',
            createdAt: new Date().toISOString(),
            completedAt: null
        };
        
        project.tasks.push(task);
        this.saveToStorage();
        return task;
    }
    
    getProjectProgress(projectId) {
        const project = this.projects.find(p => p.id === projectId);
        if (!project || project.tasks.length === 0) return 0;
        
        const completed = project.tasks.filter(t => t.status === 'completed').length;
        return Math.round((completed / project.tasks.length) * 100);
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    getRandomColor() {
        const colors = ['#e74c3c', '#3498db', '#2ecc71', '#f39c12', '#9b59b6', '#1abc9c'];
        return colors[Math.floor(Math.random() * colors.length)];
    }
    
    saveToStorage() {
        localStorage.setItem('projectData', JSON.stringify(this.projects));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('projectData');
        if (stored) {
            try {
                this.projects = JSON.parse(stored);
            } catch (e) {
                this.projects = [];
            }
        }
    }
}
```

---

## 🔗 Related Topics

- [01_Project_1_Todo_App_Complete.md](./01_Project_1_Todo_App_Complete.md)
- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)

---

**Next: [Calendar Application](./09_Project_9_Calendar_Application.md)**