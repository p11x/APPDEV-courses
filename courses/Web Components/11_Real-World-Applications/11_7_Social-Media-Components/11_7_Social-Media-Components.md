# Social Media Components

## OVERVIEW

Social media components provide UI elements for sharing, social authentication, and feed integration. This guide covers social buttons, share dialogs, and integration patterns.

## IMPLEMENTATION DETAILS

### Share Button Component

```javascript
class SocialShareButton extends HTMLElement {
  #platforms = {
    twitter: 'https://twitter.com/intent/tweet?text=',
    facebook: 'https://www.facebook.com/sharer/sharer.php?u=',
    linkedin: 'https://www.linkedin.com/sharing/share-offsite/?url=',
    whatsapp: 'https://api.whatsapp.com/send?text='
  };
  
  static get observedAttributes() { return ['platform', 'url', 'text']; }
  
  constructor() {
    super();
    this.attachShadow({ mode: 'open' });
  }
  
  connectedCallback() {
    this.render();
  }
  
  openShare() {
    const url = this.getAttribute('url') || window.location.href;
    const text = this.getAttribute('text') || '';
    const platform = this.getAttribute('platform');
    
    const shareUrl = this.#getShareUrl(platform, url, text);
    window.open(shareUrl, '_blank', 'width=600,height=400');
  }
  
  #getShareUrl(platform, url, text) {
    const base = this.#platforms[platform] || '';
    return `${base}${encodeURIComponent(url)}&text=${encodeURIComponent(text)}`;
  }
  
  render() {
    const platform = this.getAttribute('platform') || 'twitter';
    this.shadowRoot.innerHTML = `
      <style>
        button {
          padding: 8px 16px;
          border: none;
          border-radius: 4px;
          cursor: pointer;
          font-size: 14px;
        }
      </style>
      <button @click="this.openShare()">
        <slot></slot>
      </button>
    `;
  }
}
```

## NEXT STEPS

Proceed to `11_Real-World-Applications/11_8_Analytics-Dashboard-Components.md`.