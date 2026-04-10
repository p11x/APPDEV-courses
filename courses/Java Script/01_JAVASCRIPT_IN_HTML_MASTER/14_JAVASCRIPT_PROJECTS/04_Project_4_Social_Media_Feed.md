# 📱 Project 4: Social Media Feed

## 📋 Project Overview

Build a social media feed application with post creation, likes, comments, and infinite scroll. This project demonstrates:
- Dynamic content rendering
- Real-time interactions
- Infinite scroll implementation
- Local storage for data persistence

---

## 🎯 Core Features

### Post Manager

```javascript
class PostManager {
    constructor() {
        this.posts = [];
        this.page = 0;
        this.postsPerPage = 10;
        this.loadFromStorage();
    }
    
    createPost(content, author = 'User') {
        const post = {
            id: this.generateId(),
            content,
            author,
            timestamp: new Date().toISOString(),
            likes: 0,
            liked: false,
            comments: []
        };
        
        this.posts.unshift(post);
        this.saveToStorage();
        return post;
    }
    
    addComment(postId, comment) {
        const post = this.posts.find(p => p.id === postId);
        if (post) {
            post.comments.push({
                id: this.generateId(),
                text: comment,
                author: 'User',
                timestamp: new Date().toISOString()
            });
            this.saveToStorage();
        }
    }
    
    toggleLike(postId) {
        const post = this.posts.find(p => p.id === postId);
        if (post) {
            post.liked = !post.liked;
            post.likes += post.liked ? 1 : -1;
            this.saveToStorage();
        }
    }
    
    getPosts(page = 0) {
        const start = page * this.postsPerPage;
        return this.posts.slice(start, start + this.postsPerPage);
    }
    
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    }
    
    saveToStorage() {
        localStorage.setItem('posts', JSON.stringify(this.posts));
    }
    
    loadFromStorage() {
        const stored = localStorage.getItem('posts');
        if (stored) {
            try {
                this.posts = JSON.parse(stored);
            } catch (e) {
                this.posts = [];
            }
        }
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
    <title>Social Feed</title>
    <link rel="stylesheet" href="css/styles.css">
</head>
<body>
    <div class="feed-app">
        <header class="feed-header">
            <h1>📱 Social Feed</h1>
        </header>
        
        <div class="post-composer">
            <textarea id="postInput" placeholder="What's on your mind?"></textarea>
            <button id="postBtn">Post</button>
        </div>
        
        <div class="feed" id="feed"></div>
        <div class="loader" id="loader">Loading more posts...</div>
    </div>
    
    <script src="js/app.js"></script>
</body>
</html>
```

---

## 🔗 Related Topics

- [05_Element_Creation_and_Manipulation.md](../09_DOM_MANIPULATION/05_Element_Creation_and_Manipulation.md)
- [08_Event_Delegation_Patterns.md](../09_DOM_MANIPULATION/08_Event_Delegation_Patterns.md)

---

**Next: [Real-Time Chat Project](./05_Project_5_Real_Time_Chat.md)**