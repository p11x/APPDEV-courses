# 📅 Project 13: Booking System

## 📋 Project Overview

Build a room/service booking system with date selection, availability checking, and reservation management. This project demonstrates:
- Date range selection
- Availability checking
- Booking management
- Calendar integration

---

## 🎯 Core Features

```javascript
class BookingSystem {
    constructor() {
        this.bookings = [];
        this.loadFromStorage();
    }
    
    checkAvailability(startDate, endDate, resourceId) {
        return !this.bookings.some(booking => 
            booking.resourceId === resourceId &&
            ((startDate >= booking.startDate && startDate <= booking.endDate) ||
             (endDate >= booking.startDate && endDate <= booking.endDate))
        );
    }
    
    createBooking(customerName, resourceId, startDate, endDate) {
        if (!this.checkAvailability(startDate, endDate, resourceId)) {
            throw new Error('Dates not available');
        }
        
        const booking = {
            id: this.generateId(),
            customerName,
            resourceId,
            startDate,
            endDate,
            status: 'confirmed',
            createdAt: new Date().toISOString()
        };
        
        this.bookings.push(booking);
        this.saveToStorage();
        return booking;
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('bookings', JSON.stringify(this.bookings));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('bookings');
        if (stored) {
            try {
                this.bookings = JSON.parse(stored);
            } catch (e) {
                this.bookings = [];
            }
        }
    }
}
```

---

## 🔗 Related Topics

- [17_Date_Object.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/17_Date_Object.md)
- [09_Conditional_Statements.md](../02_JAVASCRIPT_SYNTAX_AND_BASICS/09_Conditional_Statements.md)

---

**Next: [Survey Platform](./14_Project_14_Survey_Platform.md)**