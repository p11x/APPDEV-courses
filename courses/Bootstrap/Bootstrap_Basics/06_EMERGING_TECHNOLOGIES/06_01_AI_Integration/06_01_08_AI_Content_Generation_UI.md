---
title: AI Content Generation UI with Bootstrap
category: Emerging Technologies
difficulty: 2
time: 25 min
tags: bootstrap5, ai, streaming, loading-states, skeleton, content-generation
---

## Overview

AI content generation requires specialized UI patterns to handle streaming responses, variable-length outputs, and asynchronous loading states. Bootstrap provides the structural components—spinners, progress bars, cards, and alerts—while custom patterns manage the streaming text display, skeleton loading, and progressive content reveal. These patterns ensure users receive clear feedback during AI processing delays.

## Basic Implementation

Loading states communicate progress during AI generation requests. Bootstrap's spinner and skeleton patterns handle different wait scenarios.

```html
<!-- Skeleton loading state for AI-generated content -->
<div class="card" aria-busy="true" aria-label="Loading AI content">
  <div class="card-body">
    <div class="placeholder-glow">
      <h5 class="card-title placeholder col-6"></h5>
      <p class="placeholder col-12 mb-1"></p>
      <p class="placeholder col-10 mb-1"></p>
      <p class="placeholder col-8 mb-3"></p>
      <a class="btn btn-primary disabled placeholder col-4" aria-disabled="true"></a>
    </div>
  </div>
</div>

<!-- Spinner loading state -->
<div class="d-flex flex-column align-items-center justify-content-center py-5" id="loadingState">
  <div class="spinner-border text-primary mb-3" role="status">
    <span class="visually-hidden">Generating content...</span>
  </div>
  <p class="text-body-secondary">AI is generating your content...</p>
  <div class="progress w-50 mt-2" style="height: 4px;">
    <div class="progress-bar progress-bar-striped progress-bar-animated"
         style="width: 45%" role="progressbar" aria-valuenow="45"
         aria-valuemin="0" aria-valuemax="100"></div>
  </div>
</div>

<!-- Loaded content container -->
<div class="card d-none" id="generatedContent">
  <div class="card-body">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h5 class="card-title mb-0">Generated Result</h5>
      <div class="btn-group btn-group-sm">
        <button class="btn btn-outline-secondary" aria-label="Copy content">
          <i class="bi bi-clipboard"></i>
        </button>
        <button class="btn btn-outline-secondary" aria-label="Regenerate">
          <i class="bi bi-arrow-clockwise"></i>
        </button>
      </div>
    </div>
    <div id="contentBody" class="ai-generated-content"></div>
  </div>
</div>
```

```js
// Loading state manager for AI content generation
class AIContentLoader {
  constructor(loadingEl, contentEl) {
    this.loadingEl = loadingEl;
    this.contentEl = contentEl;
    this.contentBody = contentEl.querySelector('#contentBody');
  }

  showLoading() {
    this.loadingEl.classList.remove('d-none');
    this.contentEl.classList.add('d-none');
    this.loadingEl.setAttribute('aria-busy', 'true');
  }

  showContent(html) {
    this.loadingEl.classList.add('d-none');
    this.contentEl.classList.remove('d-none');
    this.loadingEl.removeAttribute('aria-busy');
    this.contentBody.innerHTML = html;
    this.contentEl.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
  }

  showError(message) {
    this.loadingEl.classList.add('d-none');
    this.contentEl.classList.remove('d-none');
    this.contentBody.innerHTML = `
      <div class="alert alert-danger d-flex align-items-center" role="alert">
        <i class="bi bi-exclamation-triangle-fill me-2"></i>
        <div>${message}</div>
        <button class="btn btn-sm btn-outline-danger ms-auto" onclick="retryGeneration()">
          Retry
        </button>
      </div>
    `;
  }
}
```

## Advanced Variations

Streaming text UI renders AI responses token-by-token as they arrive from the server, providing immediate feedback.

```html
<!-- Streaming response container -->
<div class="card border-0 shadow-sm">
  <div class="card-body">
    <div class="d-flex align-items-start gap-3">
      <div class="rounded-circle bg-primary bg-opacity-10 d-flex align-items-center justify-content-center flex-shrink-0"
           style="width: 36px; height: 36px;">
        <i class="bi bi-stars text-primary"></i>
      </div>
      <div class="flex-grow-1">
        <div id="streamingContent" class="streaming-text" aria-live="polite">
          <span class="cursor-blink">|</span>
        </div>
        <div class="d-flex gap-2 mt-3" id="streamActions" style="display: none !important;">
          <button class="btn btn-sm btn-outline-secondary">
            <i class="bi bi-clipboard me-1"></i> Copy
          </button>
          <button class="btn btn-sm btn-outline-secondary">
            <i class="bi bi-hand-thumbs-up me-1"></i> Helpful
          </button>
          <button class="btn btn-sm btn-outline-secondary">
            <i class="bi bi-arrow-clockwise me-1"></i> Regenerate
          </button>
        </div>
      </div>
    </div>
  </div>
</div>
```

```js
// Streaming text renderer for AI responses
class StreamingRenderer {
  constructor(containerId) {
    this.container = document.getElementById(containerId);
    this.buffer = '';
    this.cursor = null;
  }

  async streamFromAPI(endpoint, payload) {
    const response = await fetch(endpoint, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(payload)
    });

    const reader = response.body.getReader();
    const decoder = new TextDecoder();

    this.addCursor();

    while (true) {
      const { done, value } = await reader.read();
      if (done) break;

      const chunk = decoder.decode(value, { stream: true });
      const lines = chunk.split('\n').filter(line => line.startsWith('data: '));

      for (const line of lines) {
        const data = line.slice(6);
        if (data === '[DONE]') {
          this.removeCursor();
          this.showActions();
          return;
        }
        try {
          const parsed = JSON.parse(data);
          const token = parsed.choices?.[0]?.delta?.content || '';
          if (token) await this.typeToken(token);
        } catch { /* skip malformed chunks */ }
      }
    }
  }

  async typeToken(token) {
    this.buffer += token;
    const textNode = document.createTextNode(token);
    this.container.insertBefore(textNode, this.cursor);
    await new Promise(r => setTimeout(r, 15)); // natural typing speed
  }

  addCursor() {
    this.cursor = document.createElement('span');
    this.cursor.className = 'cursor-blink';
    this.cursor.textContent = '|';
    this.container.appendChild(this.cursor);
  }

  removeCursor() {
    this.cursor?.remove();
  }

  showActions() {
    document.getElementById('streamActions')?.style.removeProperty('display');
  }
}
```

## Best Practices

1. Show skeleton loading immediately on user action for perceived performance
2. Use `aria-busy="true"` on loading containers for screen reader feedback
3. Implement streaming for responses longer than 50 tokens to reduce perceived wait time
4. Provide cancel functionality for long-running generation requests
5. Use `aria-live="polite"` on streaming containers to announce completion
6. Display estimated wait time or progress bar for batch generation tasks
7. Cache generated content to avoid redundant API calls
8. Implement graceful degradation when streaming is unavailable (fall back to polling)
9. Show clear error states with retry options when generation fails
10. Limit generated content display with expand/collapse for long outputs

## Common Pitfalls

1. **No loading feedback** - Users see a blank screen during generation. Always show spinners or skeletons.
2. **Blocking the UI** - Long API calls freeze the interface. Use async/await and loading states.
3. **Missing error handling** - Network failures must show user-friendly messages, not raw errors.
4. **Memory buildup in streaming** - Continuously appending tokens without limits causes memory issues. Implement buffer size limits.
5. **Accessibility gaps in streaming** - Screen readers need `aria-live` regions to announce streaming completion.

## Accessibility Considerations

Loading states require `aria-busy` attributes, streaming content needs `aria-live="polite"` regions, error states should use `role="alert"` for immediate announcement, and cancel buttons must be keyboard accessible. Generated content containers should maintain logical heading hierarchy and provide keyboard navigation for action buttons.

## Responsive Behavior

AI content containers should adapt to viewport width. Use Bootstrap's responsive padding (`px-3 px-md-4`), ensure code blocks in generated content scroll horizontally on narrow screens, and stack action buttons vertically on mobile. Skeleton loading patterns should match the final content's responsive layout to prevent layout shift.
