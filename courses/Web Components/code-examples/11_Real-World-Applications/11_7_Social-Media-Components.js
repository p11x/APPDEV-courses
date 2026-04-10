/**
 * Social Media Components - Share buttons, social login, feed widgets, engagement components
 * @module real-world/11_7_Social-Media-Components
 * @version 1.0.0
 * @example <social-share></social-share>
 */

class SocialShare extends HTMLElement {
  constructor() {
    super();
    this.url = '';
    this.title = '';
    this.description = '';
    this.platforms = ['whatsapp', 'facebook', 'twitter', 'linkedin'];
  }

  static get observedAttributes() {
    return ['url', 'title', 'description', 'platforms'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .share-container {
        display: flex;
        gap: 8px;
        flex-wrap: wrap;
      }
      .share-btn {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        padding: 10px 16px;
        border: none;
        border-radius: 8px;
        cursor: pointer;
        font-size: 0.875rem;
        font-weight: 500;
        transition: all 0.2s;
        color: white;
      }
      .share-btn:hover {
        transform: translateY(-2px);
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
      }
      .share-btn.whatsapp {
        background: #25d366;
      }
      .share-btn.facebook {
        background: #1877f2;
      }
      .share-btn.twitter {
        background: #1da1f2;
      }
      .share-btn.linkedin {
        background: #0a66c2;
      }
      .share-btn.email {
        background: #ea4335;
      }
      .share-btn.copy {
        background: #6c757d;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'platforms') {
        this.platforms = newValue.split(',');
      } else {
        this[name] = newValue;
      }
      this.render();
    }
  }

  getShareUrl(platform) {
    const url = this.url || window.location.href;
    const title = this.title || document.title;
    const text = this.description || title;

    switch (platform) {
      case 'whatsapp':
        return `https://wa.me/?text=${encodeURIComponent(`${text} ${url}`)}`;
      case 'facebook':
        return `https://www.facebook.com/sharer/sharer.php?u=${encodeURIComponent(url)}`;
      case 'twitter':
        return `https://twitter.com/intent/tweet?text=${encodeURIComponent(text)}&url=${encodeURIComponent(url)}`;
      case 'linkedin':
        return `https://www.linkedin.com/shareArticle?mini=true&url=${encodeURIComponent(url)}&title=${encodeURIComponent(title)}`;
      case 'email':
        return `mailto:?subject=${encodeURIComponent(title)}&body=${encodeURIComponent(`${text}\n\n${url}`)}`;
      default:
        return url;
    }
  }

  copyToClipboard() {
    const url = this.url || window.location.href;
    navigator.clipboard.writeText(url).then(() => {
      this.showToast('Link copied to clipboard!');
    });
  }

  showToast(message) {
    const toast = document.createElement('div');
    toast.textContent = message;
    toast.style.cssText = `
      position: fixed;
      bottom: 20px;
      left: 50%;
      transform: translateX(-50%);
      background: #212529;
      color: white;
      padding: 12px 24px;
      border-radius: 8px;
      font-size: 0.875rem;
      z-index: 1000;
      animation: fadeIn 0.3s;
    `;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
  }

  share(platform) {
    if (platform === 'copy') {
      this.copyToClipboard();
      return;
    }

    const shareUrl = this.getShareUrl(platform);
    window.open(shareUrl, '_blank', 'width=600,height=400');

    this.dispatchEvent(new CustomEvent('share', {
      detail: { platform, url: this.url },
      bubbles: true,
      composed: true,
    }));
  }

  render() {
    const icons = {
      whatsapp: '💬',
      facebook: '📘',
      twitter: '🐦',
      linkedin: '💼',
      email: '✉️',
      copy: '📋',
    };

    const labels = {
      whatsapp: 'WhatsApp',
      facebook: 'Facebook',
      twitter: 'Twitter',
      linkedin: 'LinkedIn',
      email: 'Email',
      copy: 'Copy',
    };

    this.shadowRoot.innerHTML = `
      <style>${SocialShare.styles}</style>
      <div class="share-container">
        ${this.platforms.map(platform => `
          <button class="share-btn ${platform}" data-platform="${platform}">
            <span>${icons[platform]}</span>
            <span>${labels[platform]}</span>
          </button>
        `).join('')}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const buttons = this.shadowRoot.querySelectorAll('.share-btn');
    buttons.forEach(btn => {
      btn.addEventListener('click', () => {
        const platform = btn.dataset.platform;
        this.share(platform);
      });
    });
  }
}

class SocialLogin extends HTMLElement {
  constructor() {
    super();
    this.providers = ['google', 'facebook', 'apple'];
  }

  static get observedAttributes() {
    return ['providers', 'redirect-url'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .login-container {
        display: flex;
        flex-direction: column;
        gap: 12px;
      }
      .login-btn {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 12px;
        padding: 12px 16px;
        border: 2px solid #dee2e6;
        border-radius: 8px;
        background: white;
        cursor: pointer;
        font-size: 0.875rem;
        font-weight: 600;
        transition: all 0.2s;
        width: 100%;
      }
      .login-btn:hover {
        border-color: #667eea;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
      }
      .login-btn.google {
        color: #4285f4;
      }
      .login-btn.facebook {
        color: #1877f2;
      }
      .login-btn.apple {
        color: #212529;
      }
      .divider {
        display: flex;
        align-items: center;
        gap: 16px;
        margin: 16px 0;
      }
      .divider::before,
      .divider::after {
        content: '';
        flex: 1;
        height: 1px;
        background: #dee2e6;
      }
      .divider span {
        font-size: 0.75rem;
        color: #6c757d;
        text-transform: uppercase;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue && name === 'providers') {
      this.providers = newValue.split(',');
      this.render();
    }
  }

  async login(provider) {
    const redirectUrl = this.getAttribute('redirect-url') || window.location.href;
    
    const config = {
      google: {
        client_id: 'YOUR_GOOGLE_CLIENT_ID',
        redirect_uri: redirectUrl,
        scope: 'email profile',
      },
      facebook: {
        client_id: 'YOUR_FACEBOOK_APP_ID',
        redirect_uri: redirectUrl,
        scope: 'email',
      },
      apple: {
        client_id: 'YOUR_APPLE_CLIENT_ID',
        redirect_uri: redirectUrl,
        scope: 'name email',
      },
    };

    const providerConfig = config[provider];
    if (!providerConfig) return;

    const authUrl = this.buildAuthUrl(provider, providerConfig);
    window.location.href = authUrl;
  }

  buildAuthUrl(provider, config) {
    const urls = {
      google: 'https://accounts.google.com/o/oauth2/v2/auth',
      facebook: 'https://www.facebook.com/v12.0/dialog/oauth',
      apple: 'https://appleid.apple.com/auth/authorize',
    };

    const params = new URLSearchParams({
      client_id: config.client_id,
      redirect_uri: config.redirect_uri,
      scope: config.scope,
      response_type: 'code',
      state: `${provider}_${Date.now()}`,
    });

    return `${urls[provider]}?${params.toString()}`;
  }

  render() {
    const icons = {
      google: '🔵',
      facebook: '📘',
      apple: '🍎',
    };

    const labels = {
      google: 'Continue with Google',
      facebook: 'Continue with Facebook',
      apple: 'Continue with Apple',
    };

    this.shadowRoot.innerHTML = `
      <style>${SocialLogin.styles}</style>
      <div class="login-container">
        ${this.providers.map(provider => `
          <button class="login-btn ${provider}" data-provider="${provider}">
            <span>${icons[provider]}</span>
            <span>${labels[provider]}</span>
          </button>
        `).join('')}
        <div class="divider">
          <span>or</span>
        </div>
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const buttons = this.shadowRoot.querySelectorAll('.login-btn');
    buttons.forEach(btn => {
      btn.addEventListener('click', () => {
        const provider = btn.dataset.provider;
        this.login(provider);
      });
    });
  }
}

class SocialFeed extends HTMLElement {
  constructor() {
    super();
    this.posts = [];
    this.limit = 10;
  }

  static get observedAttributes() {
    return ['limit', 'source'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .feed-container {
        display: flex;
        flex-direction: column;
        gap: 16px;
      }
      .post {
        background: white;
        border-radius: 12px;
        padding: 16px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
      }
      .post-header {
        display: flex;
        align-items: center;
        gap: 12px;
        margin-bottom: 12px;
      }
      .post-avatar {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        background: #e9ecef;
      }
      .post-author {
        font-weight: 600;
        color: #212529;
      }
      .post-time {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .post-content {
        color: #495057;
        line-height: 1.5;
        margin-bottom: 12px;
      }
      .post-image {
        width: 100%;
        border-radius: 8px;
        margin-bottom: 12px;
      }
      .post-actions {
        display: flex;
        gap: 16px;
        padding-top: 12px;
        border-top: 1px solid #e9ecef;
      }
      .post-action {
        display: flex;
        align-items: center;
        gap: 4px;
        background: none;
        border: none;
        color: #6c757d;
        cursor: pointer;
        font-size: 0.875rem;
        transition: color 0.2s;
      }
      .post-action:hover {
        color: #667eea;
      }
      .post-action.liked {
        color: #dc3545;
      }
      .load-more {
        padding: 12px;
        background: #f8f9fa;
        border: none;
        border-radius: 8px;
        color: #667eea;
        cursor: pointer;
        font-weight: 600;
        width: 100%;
      }
      .load-more:hover {
        background: #e9ecef;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.loadPosts();
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = parseInt(newValue);
      this.render();
    }
  }

  async loadPosts() {
    this.posts = [
      {
        id: 1,
        author: 'Tech India',
        avatar: '',
        content: '🚀 Big news! Indian tech startups raised $2.5 billion in Q1 2024. Read more about the exciting developments in the Indian startup ecosystem.',
        image: '',
        time: '2 hours ago',
        likes: 234,
        comments: 45,
        shares: 12,
      },
      {
        id: 2,
        author: 'Startup India',
        avatar: '',
        content: '📢 New scheme announced! Government to provide seed funding to 1000 startups. Applications open now until March 31st.',
        image: '',
        time: '5 hours ago',
        likes: 567,
        comments: 89,
        shares: 234,
      },
    ];
  }

  formatCount(count) {
    if (count >= 1000) {
      return (count / 1000).toFixed(1) + 'K';
    }
    return count.toString();
  }

  formatTime(time) {
    return time;
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${SocialFeed.styles}</style>
      <div class="feed-container">
        ${this.posts.slice(0, this.limit).map(post => `
          <div class="post" data-id="${post.id}">
            <div class="post-header">
              <img class="post-avatar" src="${post.avatar || 'data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 40 40"><circle cx="20" cy="20" r="20" fill="%23e9ecef"/></svg>'}" alt="${post.author}">
              <div>
                <div class="post-author">${post.author}</div>
                <div class="post-time">${post.time}</div>
              </div>
            </div>
            <div class="post-content">${post.content}</div>
            ${post.image ? `<img class="post-image" src="${post.image}" alt="Post image">` : ''}
            <div class="post-actions">
              <button class="post-action" data-action="like">
                <span>❤️</span>
                <span>${this.formatCount(post.likes)}</span>
              </button>
              <button class="post-action" data-action="comment">
                <span>💬</span>
                <span>${this.formatCount(post.comments)}</span>
              </button>
              <button class="post-action" data-action="share">
                <span>🔄</span>
                <span>${this.formatCount(post.shares)}</span>
              </button>
            </div>
          </div>
        `).join('')}
        ${this.posts.length > this.limit ? `
          <button class="load-more">Load More</button>
        ` : ''}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const likeBtns = this.shadowRoot.querySelectorAll('[data-action="like"]');
    likeBtns.forEach(btn => {
      btn.addEventListener('click', () => {
        btn.classList.toggle('liked');
      });
    });

    const loadMoreBtn = this.shadowRoot.querySelector('.load-more');
    loadMoreBtn?.addEventListener('click', () => {
      this.limit += 10;
      this.render();
    });
  }
}

class FollowButton extends HTMLElement {
  constructor() {
    super();
    this.following = false;
    this.followerCount = 0;
  }

  static get observedAttributes() {
    return ['following', 'follower-count'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .follow-btn {
        display: inline-flex;
        align-items: center;
        gap: 8px;
        padding: 8px 16px;
        border: 2px solid #667eea;
        border-radius: 20px;
        background: ${this.following ? 'transparent' : '#667eea'};
        color: ${this.following ? '#667eea' : 'white'};
        cursor: pointer;
        font-weight: 600;
        transition: all 0.2s;
      }
      .follow-btn:hover {
        background: ${this.following ? '#667eea' : '#5a67d8'};
        color: white;
      }
      .follow-btn.following {
        border-color: #28a745;
        color: #28a745;
      }
      .follow-btn.following:hover {
        background: #dc3545;
        color: white;
        border-color: #dc3545;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      if (name === 'following') {
        this.following = newValue !== null;
      } else if (name === 'follower-count') {
        this.followerCount = parseInt(newValue);
      }
      this.render();
    }
  }

  toggle() {
    this.following = !this.following;
    this.followerCount += this.following ? 1 : -1;
    this.setAttribute('following', this.following);
    this.setAttribute('follower-count', this.followerCount);
    this.render();

    this.dispatchEvent(new CustomEvent('follow-change', {
      detail: { following: this.following },
      bubbles: true,
      composed: true,
    }));
  }

  render() {
    const label = this.following ? 'Following' : 'Follow';
    const icon = this.following ? '✓' : '+';
    const count = this.followerCount > 0 ? `${this.followerCount}K` : '';

    this.shadowRoot.innerHTML = `
      <style>${FollowButton.styles}</style>
      <button class="follow-btn ${this.following ? 'following' : ''}">
        <span>${icon}</span>
        <span>${label}</span>
        ${count ? `<span style="opacity: 0.8">${count}</span>` : ''}
      </button>
    `;

    const btn = this.shadowRoot.querySelector('.follow-btn');
    btn?.addEventListener('click', () => this.toggle());
  }
}

class SocialComment extends HTMLElement {
  constructor() {
    super();
    this.comments = [];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .comments-container {
        display: flex;
        flex-direction: column;
        gap: 12px;
      }
      .comment {
        display: flex;
        gap: 12px;
      }
      .comment-avatar {
        width: 32px;
        height: 32px;
        border-radius: 50%;
        background: #e9ecef;
        flex-shrink: 0;
      }
      .comment-body {
        flex: 1;
      }
      .comment-header {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 4px;
      }
      .comment-author {
        font-weight: 600;
        font-size: 0.875rem;
      }
      .comment-time {
        font-size: 0.75rem;
        color: #6c757d;
      }
      .comment-text {
        font-size: 0.875rem;
        color: #495057;
        line-height: 1.5;
      }
      .comment-input-wrapper {
        display: flex;
        gap: 12px;
        padding-top: 12px;
        border-top: 1px solid #e9ecef;
      }
      .comment-input {
        flex: 1;
        padding: 8px 12px;
        border: 1px solid #dee2e6;
        border-radius: 20px;
        font-size: 0.875rem;
      }
      .comment-input:focus {
        outline: none;
        border-color: #667eea;
      }
      .send-btn {
        background: #667eea;
        color: white;
        border: none;
        border-radius: 50%;
        width: 32px;
        height: 32px;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  addComment(text) {
    const comment = {
      id: Date.now(),
      author: 'You',
      text,
      time: 'Just now',
    };
    this.comments = [comment, ...this.comments];
    this.render();
  }

  render() {
    this.shadowRoot.innerHTML = `
      <style>${SocialComment.styles}</style>
      <div class="comments-container">
        <div class="comment-input-wrapper">
          <input type="text" class="comment-input" placeholder="Write a comment...">
          <button class="send-btn">➤</button>
        </div>
        ${this.comments.map(comment => `
          <div class="comment">
            <img class="comment-avatar" src="" alt="${comment.author}">
            <div class="comment-body">
              <div class="comment-header">
                <span class="comment-author">${comment.author}</span>
                <span class="comment-time">${comment.time}</span>
              </div>
              <div class="comment-text">${comment.text}</div>
            </div>
          </div>
        `).join('')}
      </div>
    `;

    this.setupEventListeners();
  }

  setupEventListeners() {
    const input = this.shadowRoot.querySelector('.comment-input');
    const sendBtn = this.shadowRoot.querySelector('.send-btn');

    const handleSend = () => {
      if (input.value.trim()) {
        this.addComment(input.value);
        input.value = '';
      }
    };

    sendBtn?.addEventListener('click', handleSend);
    input?.addEventListener('keypress', (e) => {
      if (e.key === 'Enter') {
        handleSend();
      }
    });
  }
}

class SocialEmbed extends HTMLElement {
  constructor() {
    super();
    this.type = 'twitter';
    this.url = '';
  }

  static get observedAttributes() {
    return ['type', 'url'];
  }

  static get styles() {
    return `
      :host {
        display: block;
      }
      .embed-container {
        background: white;
        border-radius: 12px;
        overflow: hidden;
        border: 1px solid #e9ecef;
      }
      .embed-placeholder {
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        padding: 40px;
        background: #f8f9fa;
      }
      .embed-icon {
        font-size: 2rem;
        margin-bottom: 12px;
      }
      .embed-text {
        color: #6c757d;
        font-size: 0.875rem;
      }
    `;
  }

  connectedCallback() {
    this.attachShadow({ mode: 'open' });
    this.render();
  }

  attributeChangedCallback(name, oldValue, newValue) {
    if (oldValue !== newValue) {
      this[name] = newValue;
      this.render();
    }
  }

  render() {
    const icons = {
      twitter: '🐦',
      instagram: '📸',
      youtube: '▶️',
      linkedin: '💼',
    };

    this.shadowRoot.innerHTML = `
      <style>${SocialEmbed.styles}</style>
      <div class="embed-container">
        <div class="embed-placeholder">
          <div class="embed-icon">${icons[this.type] || '📱'}</div>
          <div class="embed-text">${this.type} embed: ${this.url}</div>
        </div>
      </div>
    `;
  }
}

export { SocialShare, SocialLogin, SocialFeed, FollowButton, SocialComment, SocialEmbed };