---
title: "Infinite Scroll Implementation"
description: "Build infinite scroll with Intersection Observer, Bootstrap lists, and loading indicators"
difficulty: 3
tags: [infinite-scroll, intersection-observer, api, performance, ux]
prerequisites:
  - "JavaScript Intersection Observer API"
  - "REST API loading states"
  - "Bootstrap list group component"
---

## Overview

Infinite scroll automatically loads more content as the user reaches the bottom of the page, replacing traditional pagination with a seamless scrolling experience. The Intersection Observer API provides an efficient way to detect when the user approaches the end of the current content. Combined with Bootstrap's list group and card components, this creates social-media-style feeds, product listings, and search result pages that load progressively.

## Basic Implementation

### Basic Infinite Scroll with List Group

```html
<div class="list-group" id="feedList"></div>
<div id="scrollSentinel" class="py-4 text-center">
  <div class="spinner-border text-primary" role="status">
    <span class="visually-hidden">Loading more...</span>
  </div>
</div>

<script>
  let page = 1;
  let loading = false;
  let hasMore = true;
  const feedList = document.getElementById('feedList');
  const sentinel = document.getElementById('scrollSentinel');

  async function loadItems() {
    if (loading || !hasMore) return;
    loading = true;

    try {
      const res = await fetch(`https://jsonplaceholder.typicode.com/posts?_page=${page}&_limit=10`);
      const items = await res.json();

      if (items.length === 0) {
        hasMore = false;
        sentinel.innerHTML = '<p class="text-muted">No more items to load.</p>';
        return;
      }

      items.forEach(item => {
        feedList.innerHTML += `
          <div class="list-group-item">
            <h6 class="mb-1">${item.title}</h6>
            <p class="mb-0 text-muted small">${item.body.substring(0, 100)}...</p>
          </div>`;
      });

      page++;
    } catch (error) {
      sentinel.innerHTML = `
        <div class="alert alert-danger d-inline-block">
          Failed to load. <a href="#" onclick="loadItems(); return false;">Retry</a>
        </div>`;
    } finally {
      loading = false;
    }
  }

  const observer = new IntersectionObserver(entries => {
    if (entries[0].isIntersecting) loadItems();
  }, { rootMargin: '200px' });

  observer.observe(sentinel);
  loadItems();
</script>
```

## Advanced Variations

### Card-Based Infinite Feed

```html
<div class="row g-4" id="cardFeed"></div>
<div id="cardSentinel" class="text-center py-4">
  <div class="spinner-border text-primary" role="status">
    <span class="visually-hidden">Loading more cards...</span>
  </div>
</div>

<script>
  let cardPage = 1;
  let cardLoading = false;

  async function loadCards() {
    if (cardLoading) return;
    cardLoading = true;

    try {
      const res = await fetch(`https://jsonplaceholder.typicode.com/photos?_page=${cardPage}&_limit=6`);
      const photos = await res.json();

      if (photos.length === 0) {
        document.getElementById('cardSentinel').innerHTML = '<p class="text-muted">End of results.</p>';
        return;
      }

      const feed = document.getElementById('cardFeed');
      photos.forEach(photo => {
        feed.innerHTML += `
          <div class="col-sm-6 col-lg-4">
            <div class="card h-100">
              <img src="${photo.thumbnailUrl}" class="card-img-top" alt="${photo.title}" loading="lazy">
              <div class="card-body">
                <h6 class="card-title">${photo.title.substring(0, 40)}...</h6>
              </div>
            </div>
          </div>`;
      });

      cardPage++;
    } catch {
      document.getElementById('cardSentinel').innerHTML = `
        <div class="alert alert-danger d-inline-block">
          Error loading. <a href="#" onclick="loadCards(); return false;">Retry</a>
        </div>`;
    } finally {
      cardLoading = false;
    }
  }

  const cardObserver = new IntersectionObserver(entries => {
    if (entries[0].isIntersecting) loadCards();
  }, { rootMargin: '300px' });

  cardObserver.observe(document.getElementById('cardSentinel'));
  loadCards();
</script>
```

### Scroll Progress Indicator

```html
<style>
  .scroll-progress {
    position: fixed;
    top: 0;
    left: 0;
    height: 3px;
    background: var(--bs-primary);
    z-index: 2000;
    transition: width 0.1s;
  }
</style>

<div class="scroll-progress" id="scrollProgress"></div>

<script>
  window.addEventListener('scroll', () => {
    const scrolled = window.scrollY;
    const total = document.documentElement.scrollHeight - window.innerHeight;
    const pct = (scrolled / total) * 100;
    document.getElementById('scrollProgress').style.width = pct + '%';
  });
</script>
```

### Infinite Scroll with Pull-to-Refresh Indicator

```html
<div id="refreshIndicator" class="text-center py-3 d-none">
  <div class="spinner-border spinner-border-sm" role="status"></div>
  <span class="ms-2 small text-muted">Refreshing...</span>
</div>

<script>
  let lastScrollY = 0;
  window.addEventListener('scroll', () => {
    const indicator = document.getElementById('refreshIndicator');
    if (window.scrollY === 0 && lastScrollY > 50) {
      indicator.classList.remove('d-none');
      // Reload first page
      setTimeout(() => {
        indicator.classList.add('d-none');
      }, 1500);
    }
    lastScrollY = window.scrollY;
  });
</script>
```

## Best Practices

1. **Use `rootMargin: '200px'`** on Intersection Observer to preload content before user reaches the bottom.
2. **Implement a `loading` flag** to prevent multiple simultaneous fetch requests.
3. **Track `hasMore` state** to stop loading when API returns empty results.
4. **Use `loading="lazy"`** on images within infinite scroll items for performance.
5. **Clean up observers** when component unmounts to prevent memory leaks in SPAs.
6. **Show a spinner at the scroll sentinel** for clear loading feedback.
7. **Use virtual scrolling** for very large datasets (>1000 items) to manage DOM size.
8. **Debounce scroll events** if not using Intersection Observer.
9. **Preserve scroll position** on back navigation using `history.scrollRestoration`.
10. **Use pagination parameters** (`_page`, `_limit`) compatible with your API's pagination scheme.
11. **Set a reasonable page size** (10-20 items) to balance load time and scroll frequency.
12. **Implement error retry** that doesn't increment the page counter on failure.

## Common Pitfalls

1. **Missing `loading` guard** causes duplicate API calls when observer fires rapidly.
2. **Not handling empty responses** results in infinite loading spinner.
3. **Memory leaks** from observers not being disconnected on navigation.
4. **DOM bloat** from never removing items above the viewport.
5. **Scroll position loss** when navigating back to an infinite scroll page.
6. **Missing `rootMargin`** means content loads too late, creating visible gaps.
7. **Not lazy-loading images** causes performance degradation as items accumulate.
8. **Fixed sentinel positioning** fails when page layout changes dynamically.

## Accessibility Considerations

- Provide a "Load More" button fallback for keyboard users who cannot trigger scroll.
- Announce new content loaded via `aria-live="polite"` region.
- Maintain logical focus order as new items are appended to the DOM.
- Provide skip links to bypass long infinite scroll lists.
- Ensure screen readers announce loading state changes.
- Consider offering a "Jump to top" button for accessibility.

## Responsive Behavior

- Card grids should adjust column count via Bootstrap responsive classes (`col-sm-6 col-lg-4`).
- Scroll sentinel should be visible at all viewport sizes.
- Touch devices need larger `rootMargin` values due to faster scrolling.
- Test infinite scroll on slow connections where loading may take several seconds.
- Consider showing item count for context: "Showing 50 of 500 items".
