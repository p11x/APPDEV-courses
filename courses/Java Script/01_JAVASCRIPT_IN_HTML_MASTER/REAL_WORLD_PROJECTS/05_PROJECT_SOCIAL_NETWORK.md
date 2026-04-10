# 👥 Project 20: Social Network Platform

## Social Features and Real-Time Updates

---

## Table of Contents

1. [User Profiles](#user-profiles)
2. [Posts and Feed](#posts-and-feed)
3. [Real-Time Updates](#real-time-updates)
4. [Interactions](#interactions)

---

## User Profiles

### Profile Structure

```javascript
const userProfile = {
  id: 'user-001',
  username: 'johndoe',
  displayName: 'John Doe',
  bio: 'Software developer',
  avatar: '/avatars/user.jpg',
  cover: '/covers/default.jpg',
  stats: {
    posts: 42,
    followers: 1250,
    following: 380
  },
  joined: new Date('2023-01-15')
};
```

### Profile Component

```javascript
class UserProfile {
  constructor(userId) {
    this.userId = userId;
    this.data = null;
  }
  
  async load() {
    this.data = await API.getProfile(this.userId);
    this.render();
  }
  
  render() {
    const element = document.getElementById('profile');
    element.innerHTML = `
      <img src="${this.data.avatar}" alt="${this.data.displayName}">
      <h1>${this.data.displayName}</h1>
      <p>@${this.data.username}</p>
      <p>${this.data.bio}</p>
      <div class="stats">
        <span>${this.data.stats.posts} Posts</span>
        <span>${this.data.stats.followers} Followers</span>
        <span>${this.data.stats.following} Following</span>
      </div>
    `;
  }
}
```

---

## Posts and Feed

### Post Structure

```javascript
const postSchema = {
  id: 'post-001',
  author: { id: 'user-001', username: 'johndoe', avatar: '/avatars/john.jpg' },
  content: 'Hello world!',
  media: [],
  createdAt: new Date(),
  likes: 42,
  comments: 5,
  shares: 3
};
```

### Feed System

```javascript
class Feed {
  constructor() {
    this.posts = [];
    this.page = 0;
  }
  
  async loadMore() {
    const newPosts = await API.getFeed(this.page++);
    this.posts.push(...newPosts);
    this.render();
  }
  
  async createPost(content) {
    const post = await API.createPost({ content });
    this.posts.unshift(post);
    this.renderNewPost(post);
  }
}
```

---

## Real-Time Updates

### WebSocket Integration

```javascript
class RealtimeFeed {
  constructor() {
    this.socket = null;
    this.listeners = [];
  }
  
  connect() {
    this.socket = new WebSocket('wss://api.example.com/feed');
    
    this.socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      this.notify(data);
    };
  }
  
  onUpdate(callback) {
    this.listeners.push(callback);
  }
  
  notify(data) {
    this.listeners.forEach(cb => cb(data));
  }
}
```

---

## Interactions

### Like System

```javascript
async function likePost(postId) {
  const button = document.querySelector(`[data-post="${postId}"] .like`);
  button.classList.toggle('liked');
  
  const count = await API.toggleLike(postId);
  button.querySelector('.count').textContent = count;
}
```

### Follow System

```javascript
async function toggleFollow(userId) {
  const button = document.querySelector('.follow-button');
  const isFollowing = button.classList.contains('following');
  
  if (isFollowing) {
    await API.unfollow(userId);
    button.classList.remove('following');
    button.textContent = 'Follow';
  } else {
    await API.follow(userId);
    button.classList.add('following');
    button.textContent = 'Following';
  }
}
```

---

## Summary

### Key Takeaways

1. **Profiles**: User data
2. **Feed**: Timeline posts
3. **Realtime**: WebSocket updates

### Next Steps

- Continue with: [06_PROJECT_JOB_BOARD.md](06_PROJECT_JOB_BOARD.md)
- Add direct messages
- Implement notifications

---

## Cross-References

- **Previous**: [04_PROJECT_HEALTH_TRACKER.md](04_PROJECT_HEALTH_TRACKER.md)
- **Next**: [06_PROJECT_JOB_BOARD.md](06_PROJECT_JOB_BOARD.md)

---

*Last updated: 2024*