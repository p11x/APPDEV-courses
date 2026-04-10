# 📅 Project 9: Calendar Application

## 📋 Project Overview

Build an interactive calendar application with event management, multiple views (month, week, day), and reminders. This project demonstrates:
- Calendar rendering and navigation
- Event CRUD operations
- Multiple view modes
- Date/time manipulation

---

## 🎯 Core Features

### Calendar Manager

```javascript
class CalendarManager {
    constructor() {
        this.currentDate = new Date();
        this.events = [];
        this.loadFromStorage();
    }
    
    getDaysInMonth(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }
    
    getFirstDayOfMonth(year, month) {
        return new Date(year, month, 1).getDay();
    }
    
    addEvent(title, date, time = null, description = '') {
        const event = {
            id: this.generateId(),
            title,
            date,
            time,
            description
        };
        
        this.events.push(event);
        this.saveToStorage();
        return event;
    }
    
    getEventsForDate(date) {
        return this.events.filter(e => e.date === date);
    }
    
    getEventsForMonth(year, month) {
        const startDate = `${year}-${String(month + 1).padStart(2, '0')}`;
        return this.events.filter(e => e.date.startsWith(startDate.substring(0, 7)));
    }
    
    deleteEvent(eventId) {
        this.events = this.events.filter(e => e.id !== eventId);
        this.saveToStorage();
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('calendarEvents', JSON.stringify(this.events));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('calendarEvents');
        if (stored) {
            try {
                this.events = JSON.parse(stored);
            } catch (e) {
                this.events = [];
            }
        }
    }
}
```

---

## 🔗 Related Topics

- [17_Date_Object.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/17_Date_Object.md)
- [05_Element_Creation_and_Manipulation.md](../09_DOM_MANIPULATION/05_Element_Creation_and_Manipulation.md)

---

**Next: [Video Player](./10_Project_10_Video_Player.md)**