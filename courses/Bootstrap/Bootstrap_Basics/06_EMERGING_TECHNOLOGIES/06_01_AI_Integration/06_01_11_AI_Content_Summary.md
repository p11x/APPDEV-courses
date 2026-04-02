---
title: "AI Content Summary"
slug: "ai-content-summary"
difficulty: 2
tags: ["bootstrap", "ai", "cards", "summary", "content"]
prerequisites:
  - "03_01_Card_Components"
  - "06_01_09_AI_Form_Autofill"
related:
  - "06_01_10_AI_Table_Filtering"
  - "06_01_13_AI_Dashboard_Insights"
duration: "25 minutes"
---

# AI Content Summary

## Overview

AI-generated content summaries condense long articles, reports, or documents into concise insights displayed within Bootstrap cards. This pattern leverages language models to extract key points, generate abstracts, and highlight actionable items. Bootstrap cards serve as containers for these summaries, with expandable sections allowing users to drill into details. This reduces cognitive load and helps users quickly decide which content warrants deeper reading.

## Basic Implementation

A card that displays an AI-generated summary of associated content with a loading state.

```html
<div class="container mt-4">
  <div class="card" id="summaryCard">
    <div class="card-header d-flex justify-content-between align-items-center">
      <h5 class="mb-0"><i class="bi bi-stars"></i> AI Summary</h5>
      <button class="btn btn-sm btn-outline-primary" id="regenerateBtn">
        <i class="bi bi-arrow-clockwise"></i> Regenerate
      </button>
    </div>
    <div class="card-body">
      <div id="summaryLoading" class="d-none">
        <div class="placeholder-glow">
          <span class="placeholder col-12 mb-2"></span>
          <span class="placeholder col-10 mb-2"></span>
          <span class="placeholder col-8"></span>
        </div>
      </div>
      <div id="summaryContent">
        <p class="card-text" id="summaryText"></p>
        <div id="keyPoints"></div>
      </div>
    </div>
    <div class="card-footer text-muted small">
      <span id="tokenCount"></span> &middot; <span id="generatedAt"></span>
    </div>
  </div>
</div>

<script>
async function loadSummary(contentId) {
  const loading = document.getElementById('summaryLoading');
  const content = document.getElementById('summaryContent');
  loading.classList.remove('d-none');
  content.classList.add('opacity-50');

  try {
    const res = await fetch(`/api/ai/summarize`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ contentId, style: 'concise' })
    });
    const data = await res.json();

    document.getElementById('summaryText').textContent = data.summary;
    document.getElementById('keyPoints').innerHTML = data.keyPoints.map(p =>
      `<div class="d-flex align-items-start mb-2">
        <i class="bi bi-check-circle-fill text-success me-2 mt-1"></i>
        <span>${p}</span>
      </div>`
    ).join('');
    document.getElementById('tokenCount').textContent = `${data.tokens} tokens`;
    document.getElementById('generatedAt').textContent =
      new Date(data.generatedAt).toLocaleString();
  } finally {
    loading.classList.add('d-none');
    content.classList.remove('opacity-50');
  }
}

loadSummary('article-123');
</script>
```

## Advanced Variations

### Expandable Insight Cards

Summaries with collapsible detail sections for deeper analysis.

```html
<div class="card mb-3">
  <div class="card-body">
    <p class="card-text" id="briefSummary">Loading summary...</p>
    <div class="collapse" id="detailCollapse">
      <hr>
      <h6>Detailed Analysis</h6>
      <p id="detailedAnalysis" class="text-muted"></p>
      <h6>Sentiment</h6>
      <div class="progress mb-3" style="height: 20px;">
        <div class="progress-bar bg-success" id="sentimentBar" style="width: 0%"></div>
      </div>
      <h6>Topics</h6>
      <div id="topicTags"></div>
    </div>
    <button class="btn btn-sm btn-link p-0" data-bs-toggle="collapse"
            data-bs-target="#detailCollapse" id="expandBtn">
      Show details <i class="bi bi-chevron-down"></i>
    </button>
  </div>
</div>

<script>
async function loadDetailedSummary(id) {
  const res = await fetch(`/api/ai/summarize-detailed`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ contentId: id })
  });
  const data = await res.json();

  document.getElementById('briefSummary').textContent = data.brief;
  document.getElementById('detailedAnalysis').textContent = data.analysis;
  document.getElementById('sentimentBar').style.width = `${data.sentiment.score}%`;
  document.getElementById('sentimentBar').textContent = data.sentiment.label;
  document.getElementById('topicTags').innerHTML = data.topics.map(t =>
    `<span class="badge bg-primary me-1 mb-1">${t}</span>`
  ).join('');
}
</script>
```

### Multi-Document Summary Comparison

Display side-by-side AI summaries for comparing multiple documents.

```html
<div class="row">
  <div class="col-md-6 mb-3">
    <div class="card h-100 border-primary">
      <div class="card-header bg-primary text-white">
        <h6 class="mb-0">Document A</h6>
      </div>
      <div class="card-body">
        <p class="card-text summary-a"></p>
        <ul class="list-unstyled key-points-a"></ul>
      </div>
    </div>
  </div>
  <div class="col-md-6 mb-3">
    <div class="card h-100 border-success">
      <div class="card-header bg-success text-white">
        <h6 class="mb-0">Document B</h6>
      </div>
      <div class="card-body">
        <p class="card-text summary-b"></p>
        <ul class="list-unstyled key-points-b"></ul>
      </div>
    </div>
  </div>
</div>
<div class="card mt-3">
  <div class="card-header"><h6 class="mb-0"><i class="bi bi-arrows-angle-contract"></i> Comparison</h6></div>
  <div class="card-body" id="comparisonSummary"></div>
</div>
```

### Summary with Citation Links

Link summary points back to source paragraphs with scroll-to highlighting.

```html
<div class="card">
  <div class="card-body">
    <div id="citedSummary"></div>
  </div>
</div>

<script>
function renderCitedSummary(data) {
  document.getElementById('citedSummary').innerHTML = data.points.map(p => `
    <p class="mb-2">
      ${p.text}
      <a href="#source-${p.sourceParagraph}" class="badge bg-light text-primary citation-link"
         data-paragraph="${p.sourceParagraph}">
        <i class="bi bi-link-45deg"></i> Source
      </a>
    </p>
  `).join('');
}

document.addEventListener('click', (e) => {
  const link = e.target.closest('.citation-link');
  if (!link) return;
  e.preventDefault();
  const target = document.getElementById(`source-${link.dataset.paragraph}`);
  target.classList.add('bg-warning-subtle');
  target.scrollIntoView({ behavior: 'smooth', block: 'center' });
  setTimeout(() => target.classList.remove('bg-warning-subtle'), 2000);
});
</script>
```

## Best Practices

1. Always show a loading state while the AI generates the summary
2. Display generation timestamp so users know how current the summary is
3. Provide a regenerate button for users who want alternative summaries
4. Limit summary length to 2-3 sentences for quick scanning
5. Use Bootstrap's `placeholder-glow` for skeleton loading states
6. Cache summaries to avoid re-generating for the same content
7. Offer multiple summary styles (brief, detailed, bullet points)
8. Include source attribution or citation links where applicable
9. Validate that AI summaries do not hallucinate content not in the source
10. Use consistent card styling across all summary components
11. Provide fallback text when the AI service is unavailable
12. Store summary metadata (model version, token count) for debugging
13. Allow users to copy summary text with a single click
14. Rate-limit regeneration to prevent excessive API costs

## Common Pitfalls

1. **No loading indicator**: Users think the app is frozen while waiting for AI
2. **Stale summaries**: Showing outdated summaries without indicating when they were generated
3. **Missing source**: Summaries that cannot be traced back to original content
4. **Hallucinated content**: AI adding information not present in the source material
5. **No regeneration option**: Stuck with a single poor-quality summary
6. **Ignoring length constraints**: Summaries that are too long defeat the purpose
7. **No error handling**: Crashing when the AI API returns an error

## Accessibility Considerations

Use `aria-busy="true"` on the summary container during loading. Provide `aria-label` on regenerate buttons describing their action. Ensure summary text maintains sufficient color contrast at 4.5:1 minimum. Use semantic HTML (`<p>`, `<ul>`) for summary content rather than generic `<div>` elements. Announce summary completion with `aria-live="polite"` regions. Provide text alternatives for any visual sentiment indicators like progress bars.

```html
<div id="summaryContainer" aria-busy="true" aria-live="polite">
  <p class="visually-hidden" id="summaryStatus">Generating summary...</p>
</div>
```

## Responsive Behavior

On mobile, display summary cards in a single column stack with full-width layouts. Collapse multi-document comparisons into a tabbed interface using Bootstrap's `nav-tabs` on small screens. Reduce padding and font sizes in summary cards using `p-2` and `fs-6` classes on mobile. Use `d-grid` for regenerate and expand buttons on mobile for easier touch targets. On desktop, use `col-lg-4` for side-by-side summary cards. Ensure citation links remain tappable on touch devices with adequate padding.
