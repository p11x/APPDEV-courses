---
title: Chatbot UI with Bootstrap
category: Emerging Technologies
difficulty: 2
time: 25 min
tags: bootstrap5, chatbot, chat-ui, message-bubbles, typing-indicator
---

## Overview

Building chatbot interfaces with Bootstrap leverages the framework's grid system, utility classes, and components to create responsive, accessible chat UIs. Bootstrap provides the structural foundation for message containers, input areas, and typographic styling, while custom CSS handles chat-specific elements like message bubbles and typing indicators. This approach works with any AI backend through JavaScript fetch calls.

## Basic Implementation

A functional chat interface consists of a scrollable message container, message bubbles with sender differentiation, and an input area.

```html
<div class="container-fluid vh-100 d-flex flex-column p-0">
  <!-- Chat Header -->
  <div class="bg-primary text-white p-3 d-flex align-items-center shadow-sm">
    <div class="position-relative me-3">
      <div class="rounded-circle bg-white d-flex align-items-center justify-content-center"
           style="width: 40px; height: 40px;">
        <i class="bi bi-robot text-primary"></i>
      </div>
      <span class="position-absolute bottom-0 end-0 bg-success rounded-circle border border-2 border-white"
            style="width: 12px; height: 12px;" aria-label="Online"></span>
    </div>
    <div>
      <h1 class="h6 mb-0">AI Assistant</h1>
      <small class="opacity-75">Online</small>
    </div>
  </div>

  <!-- Messages Container -->
  <div class="flex-grow-1 overflow-auto p-3" id="chatMessages" role="log" aria-live="polite">
    <!-- Bot message -->
    <div class="d-flex mb-3">
      <div class="bg-light rounded-3 p-3 shadow-sm" style="max-width: 75%;">
        <p class="mb-1">Hello! How can I help you with Bootstrap today?</p>
        <small class="text-muted">10:30 AM</small>
      </div>
    </div>

    <!-- User message -->
    <div class="d-flex justify-content-end mb-3">
      <div class="bg-primary text-white rounded-3 p-3 shadow-sm" style="max-width: 75%;">
        <p class="mb-1">How do I create a responsive navbar?</p>
        <small class="opacity-75">10:31 AM</small>
      </div>
    </div>
  </div>

  <!-- Input Area -->
  <div class="border-top p-3 bg-white">
    <form id="chatForm" class="d-flex gap-2">
      <label for="messageInput" class="visually-hidden">Type a message</label>
      <input type="text" class="form-control" id="messageInput"
             placeholder="Type your message..." autocomplete="off">
      <button type="submit" class="btn btn-primary" aria-label="Send message">
        <i class="bi bi-send-fill"></i>
      </button>
    </form>
  </div>
</div>
```

```js
// Chat message handler with streaming support
class ChatBot {
  constructor(containerId, apiEndpoint) {
    this.container = document.getElementById(containerId);
    this.apiEndpoint = apiEndpoint;
    this.form = document.getElementById('chatForm');
    this.input = document.getElementById('messageInput');
    this.form.addEventListener('submit', (e) => this.handleSubmit(e));
  }

  async handleSubmit(e) {
    e.preventDefault();
    const message = this.input.value.trim();
    if (!message) return;

    this.appendMessage(message, 'user');
    this.input.value = '';
    this.showTypingIndicator();

    try {
      const response = await this.sendToAPI(message);
      this.hideTypingIndicator();
      this.appendMessage(response, 'bot');
    } catch (error) {
      this.hideTypingIndicator();
      this.appendMessage('Sorry, something went wrong. Please try again.', 'bot');
    }
  }

  appendMessage(text, sender) {
    const wrapper = document.createElement('div');
    wrapper.className = sender === 'user'
      ? 'd-flex justify-content-end mb-3'
      : 'd-flex mb-3';

    const bubble = document.createElement('div');
    bubble.className = sender === 'user'
      ? 'bg-primary text-white rounded-3 p-3 shadow-sm'
      : 'bg-light rounded-3 p-3 shadow-sm';
    bubble.style.maxWidth = '75%';
    bubble.setAttribute('role', 'article');

    bubble.innerHTML = `
      <p class="mb-1">${this.escapeHtml(text)}</p>
      <small class="${sender === 'user' ? 'opacity-75' : 'text-muted'}">
        ${new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
      </small>
    `;

    wrapper.appendChild(bubble);
    this.container.appendChild(wrapper);
    this.scrollToBottom();
  }

  scrollToBottom() {
    this.container.scrollTop = this.container.scrollHeight;
  }

  escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }
}
```

## Advanced Variations

Rich message types support images, code blocks, cards, and quick-reply buttons within the chat interface.

```html
<!-- Rich message types -->
<div class="d-flex mb-3">
  <div class="bg-light rounded-3 p-3 shadow-sm" style="max-width: 75%;">
    <p class="mb-2">Here's a code example for you:</p>
    <pre class="bg-dark text-light p-3 rounded-2 mb-2 overflow-auto"><code>&lt;nav class="navbar navbar-expand-lg navbar-dark bg-dark"&gt;
  &lt;div class="container-fluid"&gt;
    &lt;a class="navbar-brand" href="#"&gt;Brand&lt;/a&gt;
  &lt;/div&gt;
&lt;/nav&gt;</code></pre>
    <div class="d-flex gap-2 mb-2">
      <button class="btn btn-sm btn-outline-primary">Copy Code</button>
      <button class="btn btn-sm btn-outline-secondary">Show More</button>
    </div>
    <small class="text-muted">10:32 AM</small>
  </div>
</div>

<!-- Card message -->
<div class="d-flex mb-3">
  <div class="shadow-sm" style="max-width: 75%;">
    <div class="card">
      <img src="tutorial-preview.jpg" class="card-img-top" alt="Navbar tutorial preview">
      <div class="card-body">
        <h6 class="card-title">Navbar Tutorial</h6>
        <p class="card-text small text-body-secondary">Step-by-step guide to responsive navbars.</p>
        <a href="#" class="btn btn-sm btn-primary">Read Tutorial</a>
      </div>
    </div>
  </div>
</div>
```

```js
// Typing indicator component
showTypingIndicator() {
  const indicator = document.createElement('div');
  indicator.id = 'typingIndicator';
  indicator.className = 'd-flex mb-3';
  indicator.setAttribute('aria-label', 'AI is typing');
  indicator.innerHTML = `
    <div class="bg-light rounded-3 p-3 shadow-sm">
      <div class="typing-dots d-flex gap-1">
        <span class="rounded-circle bg-secondary" style="width: 8px; height: 8px; animation: bounce 1.4s infinite ease-in-out;"></span>
        <span class="rounded-circle bg-secondary" style="width: 8px; height: 8px; animation: bounce 1.4s infinite ease-in-out 0.2s;"></span>
        <span class="rounded-circle bg-secondary" style="width: 8px; height: 8px; animation: bounce 1.4s infinite ease-in-out 0.4s;"></span>
      </div>
    </div>
  `;
  this.container.appendChild(indicator);
  this.scrollToBottom();
}

hideTypingIndicator() {
  document.getElementById('typingIndicator')?.remove();
}
```

## Best Practices

1. Use `role="log"` and `aria-live="polite"` on the message container for screen reader announcements
2. Limit message bubble width to 75% for readability across screen sizes
3. Escape all user input before rendering to prevent XSS attacks
4. Implement auto-scroll to the latest message with smooth scrolling
5. Use `visually-hidden` labels for icon-only buttons
6. Debounce rapid message sending to prevent API flooding
7. Store chat history in localStorage for session persistence
8. Provide keyboard shortcuts (Enter to send, Shift+Enter for new line)
9. Show clear loading states during API calls
10. Implement retry logic for failed message delivery

## Common Pitfalls

1. **XSS vulnerabilities** - Never use `innerHTML` with unsanitized user input. Use `textContent` or a sanitizer library.
2. **Memory leaks** - Long conversations with many DOM nodes degrade performance. Implement virtual scrolling or message pagination.
3. **Accessibility gaps** - Typing indicators and new messages must be announced to screen readers via `aria-live` regions.
4. **Missing input validation** - Empty or excessively long messages should be handled gracefully before sending.
5. **Layout shifts** - Dynamic message heights cause scroll jumps. Use `scrollIntoView` with `behavior: 'smooth'`.

## Accessibility Considerations

Chat interfaces require `role="log"` on the message container, `aria-live="polite"` for new message announcements, keyboard-navigable quick-reply buttons, focus management that moves to new bot messages, and clear error states announced to assistive technology. All interactive elements within messages must be keyboard accessible.

## Responsive Behavior

Chat UIs must adapt to mobile viewports where the keyboard reduces visible space. Use Bootstrap's flex utilities (`flex-grow-1`, `overflow-auto`) to let the message area fill available space. On mobile, input areas should use `position: sticky` or fixed positioning. Message bubbles should scale to near-full-width on narrow screens while maintaining padding and readability.
