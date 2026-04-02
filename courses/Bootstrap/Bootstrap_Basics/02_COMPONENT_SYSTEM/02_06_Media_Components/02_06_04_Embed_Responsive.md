---
title: "Embed Responsive"
course: "Bootstrap Basics"
module: "02_COMPONENT_SYSTEM"
lesson: "02_06_Media_Components"
file: "02_06_04_Embed_Responsive"
difficulty: 1
framework_version: "Bootstrap 5.3"
tags: [embed, ratio, responsive, iframe, video, youtube, vimeo, aspect-ratio]
prerequisites:
  - "02_06_01_Responsive_Images"
description: "Learn how to embed responsive videos, maps, and other iframes using Bootstrap's ratio utility classes for 16x9, 21x9, 4x3, and 1x1 aspect ratios."
---

## Overview

Embedding third-party content like YouTube videos, Vimeo players, Google Maps, and other iframes requires careful handling of aspect ratio. Without proper CSS, embedded content can overflow its container or collapse to zero height. Bootstrap's `.ratio` utility solves this by creating a responsive wrapper that maintains a fixed aspect ratio regardless of viewport width.

In Bootstrap 5, the legacy `.embed-responsive` component was replaced by the simpler `.ratio` utility. You apply `.ratio` to a container and specify the aspect ratio with modifier classes like `.ratio-16x9`, `.ratio-4x3`, `.ratio-21x9`, or `.ratio-1x1`. The child `<iframe>`, `<embed>`, `<video>`, or `<object>` is sized automatically.

## Basic Implementation

The standard 16:9 ratio is the most common for video content:

```html
<div class="ratio ratio-16x9">
  <iframe src="https://www.youtube.com/embed/dQw4w9WgXcQ" title="YouTube video" allowfullscreen></iframe>
</div>
```

For square content like Instagram embeds or portrait-oriented media:

```html
<div class="ratio ratio-1x1">
  <iframe src="https://www.instagram.com/p/example/embed/" title="Instagram post"></iframe>
</div>
```

A 4:3 ratio works for older video formats and presentations:

```html
<div class="ratio ratio-4x3">
  <iframe src="https://player.vimeo.com/video/123456789" title="Vimeo video" allowfullscreen></iframe>
</div>
```

## Advanced Variations

The ultra-wide 21:9 ratio suits cinematic content:

```html
<div class="ratio ratio-21x9">
  <iframe src="https://www.youtube.com/embed/UltraWideVideoID" title="Cinematic trailer" allowfullscreen></iframe>
</div>
```

Embed a Google Map responsively:

```html
<div class="ratio ratio-16x9">
  <iframe
    src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d..."
    style="border:0;"
    allowfullscreen=""
    loading="lazy"
    referrerpolicy="no-referrer-when-downgrade"
    title="Office location map">
  </iframe>
</div>
```

Constrain embed width with grid columns and add a shadow for visual depth:

```html
<div class="row justify-content-center">
  <div class="col-lg-8">
    <div class="ratio ratio-16x9 shadow rounded overflow-hidden">
      <iframe src="https://www.youtube.com/embed/VideoID" title="Product demo" allowfullscreen></iframe>
    </div>
    <p class="text-center text-muted mt-2">Watch our 2-minute product overview.</p>
  </div>
</div>
```

Use the `--bs-aspect-ratio` custom property for non-standard ratios:

```html
<div class="ratio" style="--bs-aspect-ratio: 56.25%;">
  <iframe src="https://example.com/custom-embed" title="Custom embed"></iframe>
</div>
```

Embed HTML5 video with controls:

```html
<div class="ratio ratio-16x9">
  <video controls preload="metadata" poster="images/video-poster.jpg">
    <source src="videos/intro.mp4" type="video/mp4">
    <track kind="captions" src="captions/intro-en.vtt" srclang="en" label="English">
    Your browser does not support the video tag.
  </video>
</div>
```

## Best Practices

1. **Always wrap iframes in `.ratio`** to prevent layout breakage across viewports.
2. **Use `ratio-16x9` for video embeds** as it matches the standard widescreen format of YouTube and Vimeo.
3. **Add `title` attributes to all iframes** for screen reader identification.
4. **Include `loading="lazy"`** on below-the-fold embeds to defer iframe loading.
5. **Use `allowfullscreen`** on video iframes to enable full-screen playback.
6. **Constrain embed width** with grid columns (`.col-lg-8`, `.col-md-10`) rather than letting them span full width.
7. **Apply `rounded` and `shadow`** to the ratio container for visual polish on card layouts.
8. **Use `overflow-hidden`** on the ratio container to prevent iframe scrollbars from appearing.
9. **Provide a fallback message** inside `<video>` tags for browsers without support.
10. **Add `<track>` elements** for captions/subtitles on self-hosted video embeds.
11. **Test embeds with Content Security Policy** headers to ensure iframes are not blocked.
12. **Use `referrerpolicy="no-referrer-when-downgrade"`** on map embeds for privacy compliance.

## Common Pitfalls

1. **Not wrapping iframes in `.ratio`.** The iframe may default to a fixed size (e.g., 300x150) and overflow or leave gaps.
2. **Using the wrong aspect ratio.** A 4:3 wrapper around 16:9 content causes letterboxing or cropping.
3. **Forgetting the `title` attribute on iframes.** Screen readers cannot identify the embedded content, failing WCAG requirements.
4. **Setting explicit `height` on the iframe.** This overrides the ratio utility's padding-based sizing, breaking responsiveness.
5. **Using the deprecated `.embed-responsive` class.** Bootstrap 5 uses `.ratio` instead; the old class has no effect.
6. **Not adding `loading="lazy"`** on multiple video embeds, causing excessive simultaneous network requests.
7. **Embedding untrusted iframes** without sandbox attributes, creating security vulnerabilities.
8. **Forgetting `allowfullscreen`** and confusing users who expect full-screen video playback.

## Accessibility Considerations

Every `<iframe>` must have a `title` attribute describing its content (e.g., `title="YouTube: Introduction to Bootstrap"`). This allows screen reader users to understand what the embed contains before interacting with it. For self-hosted `<video>` elements, provide `<track kind="captions">` for spoken dialogue and `<track kind="descriptions">` for audio descriptions of visual content. Ensure that video players have keyboard-accessible controls. If the embedded content is decorative (e.g., a background video with no informational value), consider hiding it from assistive technology with `aria-hidden="true"`.

## Responsive Behavior

The `.ratio` utility uses the CSS `aspect-ratio` property (with a padding-bottom fallback for older browsers) to maintain proportions. The embedded content stretches to fill the container. When placed inside a `.col-*` column, the embed width is controlled by the grid, scaling down proportionally on smaller viewports. The aspect ratio remains constant regardless of screen size. On very narrow screens (below 320px), ensure your ratio container does not become too short for the content to be usable; consider using `ratio-1x1` on mobile for better visibility.
