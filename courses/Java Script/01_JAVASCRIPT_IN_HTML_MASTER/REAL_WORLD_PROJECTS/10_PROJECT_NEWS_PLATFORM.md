# 📰 Project 25: News Platform

## Content Management and Publishing System

---

## Table of Contents

1. [Platform Overview](#platform-overview)
2. [Article Management](#article-management)
3. [Categories and Tags](#categories-and-tags)
4. [User Interactions](#user-interactions)
5. [Publishing Workflow](#publishing-workflow)

---

## Platform Overview

### Architecture

```
┌─────────────────────────────────────────────────────────────┐
│              NEWS PLATFORM                               │
├─────────────────────────────────────────────────────────────┤
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐   │
│  │   Editor   │ │  Publisher │ │  Reader   │   │
│  │   Panel   │ │   Queue   │ │    View   │   │
│  └──────────────┘ └──────────────┘ └──────────────┘   │
│                      │                                    │
│                      ▼                                    │
│            ┌─────────────────┐                            │
│            │   Article DB    │                            │
│            └─────────────────┘                            │
└─────────────────────────────────────────────────────────────┘
```

### Article Schema

```javascript
const articleSchema = {
  id: 'ART-001',
  title: 'Breaking: New JavaScript Features',
  slug: 'breaking-new-javascript-features',
  content: 'Full article HTML content...',
  excerpt: 'Short description for preview...',
  author: {
    id: 'user-001',
    name: 'John Doe',
    avatar: '/avatars/john.jpg',
    bio: 'Tech writer'
  },
  category: 'technology',
  tags: ['javascript', 'web', 'programming'],
  status: 'published',
  publishedAt: '2024-06-15T10:00:00Z',
  featured: true,
  image: '/images/cover.jpg',
  readTime: 5,
  views: 1500,
  likes: 45,
  comments: 12
};
```

---

## Article Management

### Content Editor

```javascript
class ArticleEditor {
  constructor(options = {}) {
    this.autosave = options.autosave || true;
    this.autosaveInterval = options.autosaveInterval || 30000;
    this.draft = null;
  }

  async init(articleId) {
    if (articleId) {
      this.draft = await ArticleAPI.getDraft(articleId);
    } else {
      this.draft = this.createNewDraft();
    }
    this.render();
    this.setupAutosave();
  }

  createNewDraft() {
    return {
      title: '',
      content: '',
      excerpt: '',
      category: '',
      tags: [],
      status: 'draft'
    };
  }

  render() {
    return `
      <div class="article-editor">
        <input type="text" class="title-input" placeholder="Article Title" 
               value="${this.draft.title}" />
        
        <div class="toolbar">
          <button data-action="bold"><b>B</b></button>
          <button data-action="italic"><i>I</i></button>
          <button data-action="heading">H1</button>
          <button data-action="link">🔗</button>
          <button data-action="image">🖼️</button>
          <button data-action="code">```</button>
        </div>
        
        <textarea class="content-editor" 
                 placeholder="Write your article...">
          ${this.draft.content}
        </textarea>
        
        <div class="meta-fields">
          <input type="text" class="excerpt" placeholder="Excerpt" 
                 value="${this.draft.excerpt}" />
          <select class="category">
            <option value="">Select Category</option>
            <option value="technology">Technology</option>
            <option value="business">Business</option>
            <option value="sports">Sports</option>
          </select>
          <input type="text" class="tags" placeholder="Tags (comma separated)" />
        </div>
        
        <div class="actions">
          <button class="save-draft">Save Draft</button>
          <button class="publish">Publish</button>
        </div>
      </div>
    `;
  }

  setupAutosave() {
    if (this.autosave) {
      setInterval(() => {
        this.saveDraft();
      }, this.autosaveInterval);
    }
  }

  async saveDraft() {
    const data = this.collectFormData();
    await ArticleAPI.saveDraft(data);
  }
}
```

---

## Categories and Tags

### Taxonomy Management

```javascript
class TaxonomyManager {
  constructor() {
    this.categories = new Map();
    this.tags = new Map();
  }

  async load() {
    const [categories, tags] = await Promise.all([
      ArticleAPI.getCategories(),
      ArticleAPI.getTags()
    ]);
    
    categories.forEach(c => this.categories.set(c.id, c));
    tags.forEach(t => this.tags.set(t.id, t));
  }

  createCategory(name, slug, parent = null) {
    const category = {
      id: `cat-${Date.now()}`,
      name,
      slug,
      parent,
      articleCount: 0
    };
    this.categories.set(category.id, category);
    return category;
  }

  createTag(name) {
    const slug = name.toLowerCase().replace(/\s+/g, '-');
    const tag = {
      id: `tag-${Date.now()}`,
      name,
      slug,
      articleCount: 0
    };
    this.tags.set(tag.id, tag);
    return tag;
  }

  getArticlesByCategory(categoryId) {
    return ArticleAPI.getByCategory(categoryId);
  }

  getArticlesByTag(tagId) {
    return ArticleAPI.getByTag(tagId);
  }
}
```

---

## User Interactions

### Comments System

```javascript
class CommentsManager {
  constructor(articleId) {
    this.articleId = articleId;
    this.comments = [];
  }

  async load() {
    this.comments = await ArticleAPI.getComments(this.articleId);
    this.render();
  }

  async addComment(userId, content, parentId = null) {
    const comment = {
      id: `comment-${Date.now()}`,
      articleId: this.articleId,
      userId,
      content,
      parentId,
      createdAt: new Date(),
      likes: 0,
      replies: []
    };

    await ArticleAPI.createComment(comment);
    this.comments.push(comment);
    return comment;
  }

  async likeComment(commentId, userId) {
    await ArticleAPI.likeComment(commentId, userId);
  }

  render() {
    const rootComments = this.comments.filter(c => !c.parentId);
    
    return rootComments.map(comment => this.renderComment(comment)).join('');
  }

  renderComment(comment) {
    return `
      <div class="comment" data-id="${comment.id}">
        <div class="comment-header">
          <img src="${comment.user.avatar}" class="avatar">
          <span class="author">${comment.user.name}</span>
          <span class="date">${new Date(comment.createdAt).toLocaleDateString()}</span>
        </div>
        <div class="comment-content">${comment.content}</div>
        <div class="comment-actions">
          <button class="like-btn" data-id="${comment.id}">
            👍 ${comment.likes}
          </button>
          <button class="reply-btn" data-id="${comment.id}">Reply</button>
        </div>
        ${comment.replies.map(r => this.renderComment(r)).join('')}
      </div>
    `;
  }
}
```

---

## Publishing Workflow

### Editorial Pipeline

```javascript
class PublishingPipeline {
  constructor() {
    this.queue = [];
    this.steps = ['draft', 'review', 'approved', 'published'];
  }

  async submitForReview(articleId) {
    await this.transition(articleId, 'draft', 'review');
  }

  async approve(articleId) {
    await this.transition(articleId, 'review', 'approved');
  }

  async publish(articleId, publishAt = null) {
    const article = await ArticleAPI.get(articleId);
    
    if (publishAt) {
      await this.schedulePublish(articleId, publishAt);
    } else {
      await this.transition(articleId, 'approved', 'published');
    }
  }

  async transition(articleId, fromStatus, toStatus) {
    await ArticleAPI.updateStatus(articleId, toStatus);
    await this.logHistory(articleId, fromStatus, toStatus);
  }

  async schedulePublish(articleId, publishAt) {
    await ArticleAPI.schedule(articleId, publishAt);
  }

  async getQueue() {
    return ArticleAPI.getQueuedArticles();
  }
}
```

---

## Summary

### Key Takeaways

1. **Editor**: Rich text editor with toolbar
2. **Taxonomy**: Categories and tags
3. **Comments**: Threaded discussions
4. **Publishing**: Editorial workflow

---

## Cross-References

- **Previous**: [09_PROJECT_TRAVEL_BOOKING.md](09_PROJECT_TRAVEL_BOOKING.md)

---

*Last updated: 2024*