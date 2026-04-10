# 💬 Project 5: Real-Time Chat Application

## 📋 Project Overview

Build a real-time chat application with message threading, typing indicators, and message history. This project demonstrates:
- Real-time messaging simulation
- Message threading
- Typing indicators
- Local storage persistence

---

## 🎯 Core Features

### Chat Manager

```javascript
class ChatManager {
    constructor() {
        this.messages = [];
        this.currentUser = 'You';
        this.loadFromStorage();
    }
    
    sendMessage(text, channel = 'general') {
        const message = {
            id: this.generateId(),
            text,
            sender: this.currentUser,
            channel,
            timestamp: new Date().toISOString(),
            read: true
        };
        
        this.messages.push(message);
        this.saveToStorage();
        return message;
    }
    
    getMessages(channel = 'general') {
        return this.messages.filter(m => m.channel === channel);
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('chatMessages', JSON.stringify(this.messages));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('chatMessages');
        if (stored) {
            try {
                this.messages = JSON.parse(stored);
            } catch (e) {
                this.messages = [];
            }
        }
    }
}
```

### Typing Indicator

```javascript
class TypingIndicator {
    constructor(inputElement, onType) {
        this.input = inputElement;
        this.onType = onType;
        this.typingTimer = null;
        this.setupListeners();
    }
    
    setupListeners() {
        this.input.addEventListener('input', () => {
            this.onType(true);
            clearTimeout(this.typingTimer);
            this.typingTimer = setTimeout(() => {
                this.onType(false);
            }, 1000);
        });
    }
}
```

---

## 🎨 HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Chat App</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="chat-app">
        <header class="chat-header">
            <h1>💬 Chat</h1>
        </header>
        
        <div class="chat-messages" id="messages"></div>
        
        <div class="typing-indicator" id="typingIndicator"></div>
        
        <form class="chat-input">
            <input type="text" id="messageInput" placeholder="Type a message...">
            <button type="submit">Send</button>
        </form>
    </div>
    
    <script src="js/app.js"></script>
</body>
</html>
```

---

## 🔗 Related Topics

- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)
- [06_Event_Handling_Deep_Dive.md](../09_DOM_MANIPULATION/06_Event_Handling_Deep_Dive.md)
- [03_Async_Await_Master_Class.md](../08_ASYNC_JAVASCRIPT/03_Async_Await_Master_Class.md)

---

**Projects Module: 5/32 Complete** 🎉

More projects to come! Let me know if you'd like to continue with additional projects or move to other modules.